using Microsoft.AspNetCore.Http;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.PronunciationAssessment;
using Newtonsoft.Json;
using ReadingAPI.ReadingAPI.Domain.DTOs;
using System.Text.RegularExpressions;

public sealed class SpeechService
{

    public async Task<AssessmentResultDto> AssessAndSummarizeAsync(
        Stream audioStream,
        string contentType,
        string referenceText,
        string language,
        double accuracyThreshold = 85.0,
        bool flagAnyErrorType = true)
    {
        // 1) Configure Speech
        var key = ReadingAPI.AppConfiguration.Configuration["AppSettings:AzureSpeechKey"];
        var region = ReadingAPI.AppConfiguration.Configuration["AppSettings:eastus"];

        var endpoint = new Uri("https://eastus.api.cognitive.microsoft.com/sts/v1.0/issueToken");
        var cfg = SpeechConfig.FromEndpoint(endpoint, key);

        //  var speechConfig = SpeechConfig.FromSubscription(key, region);
        cfg.SpeechRecognitionLanguage = language;

        // 2) Map input MIME
        AudioStreamFormat format = contentType switch
        {
            "audio/wav" or "audio/x-wav" => AudioStreamFormat.GetWaveFormatPCM(16000, 16, 1),
            "audio/mpeg" or "audio/mp3" => AudioStreamFormat.GetCompressedFormat(AudioStreamContainerFormat.MP3),
            "audio/ogg" or "audio/ogg;codecs=opus" or "audio/webm" or "audio/webm;codecs=opus"
                => AudioStreamFormat.GetCompressedFormat(AudioStreamContainerFormat.OGG_OPUS),
            _ => AudioStreamFormat.GetCompressedFormat(AudioStreamContainerFormat.MP3)
        };

        var pushStream = AudioInputStream.CreatePushStream(format);
        var buffer = new byte[81920];
        int read;
        while ((read = await audioStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            pushStream.Write(buffer.AsSpan(0, read).ToArray());
        pushStream.Close();

        using var audioConfig = AudioConfig.FromStreamInput(pushStream);
        using var recognizer = new SpeechRecognizer(cfg, audioConfig);

        // 3) Pronunciation assessment config
        var paConfig = new PronunciationAssessmentConfig(
            referenceText,
            GradingSystem.HundredMark,
            Granularity.Phoneme,
            enableMiscue: true)
        {
            PhonemeAlphabet = "IPA"
        };
        paConfig.ApplyTo(recognizer);

        // 4) Recognize (single utterance)
        var stt = await recognizer.RecognizeOnceAsync();

        // Canceled is a real service failure, not silence from the student
        if (stt.Reason == ResultReason.Canceled)
        {
            var cancellation = CancellationDetails.FromResult(stt);
            throw new Exception(
                $"Azure Speech Error (Reason={cancellation.Reason}, " +
                $"ErrorCode={cancellation.ErrorCode}): {cancellation.ErrorDetails}");
        }

        // On silence/noise/NoMatch there is no pronunciation result
        var pa = stt.Reason == ResultReason.RecognizedSpeech ? PronunciationAssessmentResult.FromResult(stt) : null;

        if (pa == null)
        {
            var missingWords = Tokenize(referenceText)
                .Select(word => new WordIssueDto { Word = word, AccuracyScore = 0, ErrorType = "Omission" })
                .ToList();

            return new AssessmentResultDto
            {
                EngineText = stt.Text ?? string.Empty,
                Overall = new OverallScoresDto
                {
                    Accuracy = 0,
                    Fluency = 0,
                    Completeness = 0,
                    Pronunciation = 0
                },
                Issues = missingWords,
                SpeechRecognized = false,
                RawNBestJson = stt.Properties?.GetProperty(PropertyId.SpeechServiceResponse_JsonResult)
            };
        }

        // 5) Parse Azure raw JSON (contains NBest -> Words -> PronunciationAssessment)
        var rawJson = stt.Properties.GetProperty(PropertyId.SpeechServiceResponse_JsonResult);

        // Defensive parse with minimal POCOs
        var parsed = JsonConvert.DeserializeObject<AzureSttEnvelope>(rawJson);
        var best = parsed?.NBest?.FirstOrDefault();

        var issues = new List<WordIssueDto>();
        if (best?.Words != null)
        {
            foreach (var w in best.Words)
            {
                var s = w?.PronunciationAssessment?.AccuracyScore;
                var err = w?.PronunciationAssessment?.ErrorType ?? "None";

                bool belowThreshold = s.HasValue && s.Value < accuracyThreshold;
                bool hasError = flagAnyErrorType && !string.Equals(err, "None", StringComparison.OrdinalIgnoreCase);

                if (belowThreshold || hasError)
                {
                    // Consider a "None" error in value below threshold as a "Mispronunciation" error
                    var effectiveError = (belowThreshold && string.Equals(err, "None", StringComparison.OrdinalIgnoreCase)) ? "Mispronunciation" : err;

                    issues.Add(new WordIssueDto
                    {
                        Word = w!.Word ?? string.Empty,
                        AccuracyScore = s ?? 0,
                        ErrorType = effectiveError,
                        // Optional timings if you want to highlight in audio later:
                        Offset = w.Offset,
                        Duration = w.Duration
                    });
                }
            }
        }

        // Azure may report Completeness = 100 on an incomplete reading,
        // so we calculate completeness by comparing the reference against what was recognized, so partial reads don't pass
        var referenceWords = Tokenize(referenceText);
        var recognizedWords = Tokenize(stt.Text);

        // How many times each word appears in the reference, used later to dedupe issues
        var refCounts = referenceWords
            .GroupBy(t => t, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.Count(), StringComparer.OrdinalIgnoreCase);

        // Mark which reference words were actually spoken
        var matchedRefWord = MatchReferenceWords(referenceWords, recognizedWords);

        int matchedRefWords = 0;
        for (var i = 0; i < referenceWords.Count; i++)
        {
            if (matchedRefWord[i])
            {
                matchedRefWords++;
            }
            else
            {
                // Reference word that wasn't spoken, mark it as an omission
                issues.Add(new WordIssueDto
                {
                    Word = referenceWords[i],
                    AccuracyScore = 0,
                    ErrorType = "Omission"
                });
            }
        }

        var calculatedCompleteness = referenceWords.Count == 0 ? 100.0 : Math.Round(100.0 * matchedRefWords / referenceWords.Count, 0);

        // Keep the lower of the two completeness values
        // So an incomplete read is never scored as complete
        var effectiveCompleteness = Math.Min(pa.CompletenessScore, calculatedCompleteness);

        // Scale the scores by how much of the text was actually read. A full read keeps
        // its scores, a partial read is lowered
        var completenessFactor = effectiveCompleteness / 100.0;
        var effectiveAccuracy = Math.Round(pa.AccuracyScore * completenessFactor, 0);
        var effectiveFluency = Math.Round(pa.FluencyScore * completenessFactor, 0);
        var effectivePronunciation = Math.Round(pa.PronunciationScore * completenessFactor, 0);

        return new AssessmentResultDto
        {
            EngineText = stt.Text,
            Overall = new OverallScoresDto
            {
                Accuracy = effectiveAccuracy,
                Fluency = effectiveFluency,
                Completeness = effectiveCompleteness,
                Pronunciation = effectivePronunciation
            },
            // When a student repeats a word, Azure recognizes it several times
            // Deduplicate keeping the worst occurrences, but only up to how many times the word appears in the reference
            Issues = issues
                .GroupBy(i => i.Word, StringComparer.OrdinalIgnoreCase)
                .SelectMany(g =>
                {
                    var keep = refCounts.TryGetValue(g.Key, out var c) ? Math.Max(c, 1) : 1;
                    return g.OrderBy(i => i.AccuracyScore).Take(keep);
                })
                .OrderBy(i => i.AccuracyScore)
                .ToList(),
            SpeechRecognized = recognizedWords.Count > 0,
            // For debugging/advanced UI you may also return rawJson:
            RawNBestJson = rawJson
        };
    }



    public async Task<byte[]> SynthesizeTextToAudioAsync(string text, string language, string? voiceName = null)
    {
        var key = ReadingAPI.AppConfiguration.Configuration["AppSettings:AzureSpeechKey"];
        var region = ReadingAPI.AppConfiguration.Configuration["AppSettings:eastus"];

        var endpoint = new Uri("https://eastus.api.cognitive.microsoft.com/sts/v1.0/issueToken");
        var cfg = SpeechConfig.FromEndpoint(endpoint, key);

        // 1️⃣  Remove HTML tags to avoid TTS mispronunciation or XSS issues
        text = Regex.Replace(text, "<.*?>", string.Empty).Trim();

        // 2️⃣  Configure voice & format
        cfg.SpeechSynthesisLanguage = language;
        cfg.SpeechSynthesisVoiceName = voiceName ?? GetDefaultVoiceName(language);
        cfg.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Riff16Khz16BitMonoPcm);

        // 3️⃣  Perform synthesis
        using (var synthesizer = new SpeechSynthesizer(cfg, null))
        {
            using (var result = await synthesizer.SpeakTextAsync(text))
            {
                if (result.Reason == ResultReason.SynthesizingAudioCompleted)
                    return result.AudioData;

                var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                throw new Exception($"Speech synthesis failed. Reason: {cancellation.Reason}");
            }
        }
    }

    public async Task<TtsResult> SynthesizeTextWithBoundariesAsync(string text, string language, string? voiceName = null)
    {
        var key = ReadingAPI.AppConfiguration.Configuration["AppSettings:AzureSpeechKey"];
        var endpoint = new Uri("https://eastus.api.cognitive.microsoft.com/sts/v1.0/issueToken");
        var cfg = SpeechConfig.FromEndpoint(endpoint, key);

        text = Regex.Replace(text, "<.*?>", string.Empty).Trim();

        cfg.SpeechSynthesisLanguage = language;
        cfg.SpeechSynthesisVoiceName = voiceName ?? GetDefaultVoiceName(language);
        cfg.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Riff16Khz16BitMonoPcm);

        var words = new List<TtsWord>();

        using (var synthesizer = new SpeechSynthesizer(cfg, null))
        {
            // Subscribe BEFORE speaking — events fire during synthesis.
            synthesizer.WordBoundary += (s, e) =>
            {
                // Skip punctuation/sentence markers so the count matches the
                // client's word tokens. (BoundaryType requires Speech SDK ≥ ~1.21;
                // if your SDK lacks it, drop this line — see note below.)
                if (e.BoundaryType != SpeechSynthesisBoundaryType.Word) return;

                words.Add(new TtsWord
                {
                    Offset = e.AudioOffset / 10000000.0,   // 100-ns ticks → seconds
                    Duration = e.Duration.TotalSeconds,
                    Text = e.Text
                });
            };

            using (var result = await synthesizer.SpeakTextAsync(text))
            {
                if (result.Reason == ResultReason.SynthesizingAudioCompleted)
                    return new TtsResult { Audio = result.AudioData, Words = words };

                var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                throw new Exception($"Speech synthesis failed. Reason: {cancellation.Reason}");
            }
        }
    }

    // Splits text into normalized words, 
    // Used to compare reference and recognized text and find word omissions
    private static List<string> Tokenize(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return new List<string>();

        return Regex.Matches(text.ToLowerInvariant(), @"\p{L}+")
            .Select(m => m.Value)
            .ToList();
    }

    // A reference word is spoken if the recognized text still has an unused copy of it
    // Counting copies this way handles repeated words and ignores order
    private static bool[] MatchReferenceWords(List<string> referenceWords, List<string> recognizedWords)
    {
        int n = referenceWords.Count;
        var matched = new bool[n];
        if (n == 0 || recognizedWords.Count == 0)
            return matched;

        // Count of each recognized word still available to match
        var available = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        foreach (var word in recognizedWords)
            available[word] = available.TryGetValue(word, out var count) ? count + 1 : 1;

        for (var i = 0; i < n; i++)
        {
            if (available.TryGetValue(referenceWords[i], out var count) && count > 0)
            {
                matched[i] = true;
                available[referenceWords[i]] = count - 1;
            }
        }

        return matched;
    }

    // Simple helper to pick a voice for the specified language
    private static string GetDefaultVoiceName(string language)
    {
        return language switch
        {
            "es-ES" => "es-ES-ElviraNeural",
            "es-MX" => "es-MX-DaliaNeural",
            // You can find a complete list of voices in the Azure documentation
            _ => "en-US-JennyNeural"
        };
    }
}

// ---------- Config + DTOs ----------

public sealed class AzureSpeechOptions
{
    public string Key { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
}

// DTOs/PronunciationAssessForm.cs
public class PronunciationAssessForm
{
    public string StudentGrade { get; set; } = "CEFR-en";

    /// <summary>Recorded audio (webm/ogg/mp3/wav)</summary>
    public IFormFile Audio { get; set; }

    /// <summary>The reference text the student read</summary>
    public string ReferenceText { get; set; }

    /// <summary>BCP-47 language tag (default en-US; try es-MX / es-ES)</summary>
    public string? Language { get; set; }

    /// <summary>Minimum per-word accuracy to flag as an issue (0-100, default 85)</summary>
    public double? MinAccuracy { get; set; }

    /// <summary>The story Id being read</summary>
    public int StoryId { get; set; }

    /// <summary>The slide Id being assessed</summary>
    public int SlideId { get; set; }
}

public sealed class AssessmentResultDto
{
    public string? EngineText { get; set; }
    public OverallScoresDto Overall { get; set; } = new();
    public List<WordIssueDto> Issues { get; set; } = new();
    public bool SpeechRecognized { get; set; } = true;
    public string? RawNBestJson { get; set; } // optional, handy in dev
}

public sealed class OverallScoresDto
{
    public double Accuracy { get; set; }
    public double Fluency { get; set; }
    public double Completeness { get; set; }
    public double Pronunciation { get; set; }
}

public sealed class WordIssueDto
{
    public string Word { get; set; } = "";
    public double AccuracyScore { get; set; }
    public string? ErrorType { get; set; } // None | Insertion | Omission | Mispronunciation
    public long? Offset { get; set; }      // 100-ns ticks from start (Azure units)
    public long? Duration { get; set; }
}

// ---------- Minimal POCOs for Azure JSON ----------

public sealed class AzureSttEnvelope
{
    public List<AzureNBestEntry>? NBest { get; set; }
}

public sealed class AzureNBestEntry
{
    public List<AzureWord>? Words { get; set; }
    public AzurePaScores? PronunciationAssessment { get; set; }
}

public sealed class AzureWord
{
    public string? Word { get; set; }
    public long? Offset { get; set; }
    public long? Duration { get; set; }
    public AzurePaScores? PronunciationAssessment { get; set; }
}

public sealed class AzurePaScores
{
    public double? AccuracyScore { get; set; }
    public double? FluencyScore { get; set; }
    public double? CompletenessScore { get; set; }
    public double? PronunciationScore { get; set; }
    public string? ErrorType { get; set; }
}

// Models/FinalAssessmentResponse.cs
public sealed class FinalAssessmentResponse
{
    // The raw and detailed results from Azure (using your provided DTOs)
    public AssessmentResultDto AzureResult { get; set; } = new();

    // Custom level calculated by your application
    public string LanguageLevel { get; set; } = default!;
    public string LevelDescription { get; set; } = default!;
}