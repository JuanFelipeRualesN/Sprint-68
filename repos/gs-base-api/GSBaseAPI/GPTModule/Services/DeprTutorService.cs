using GSBaseAPI.GPTModule.Infrastructure;
using GSBaseAPI.GPTModule.Models;
using GSBaseAPI.GPTModule.Services.ExternalAPIs;
using System.Text.Json;

namespace GSBaseAPI.GPTModule.Services
{
    public sealed class DeprTutorService
    {
        private readonly IOpenAiClient _openAi;
        private readonly ITeachersHomeworkApi _homeworkApi;
        private readonly ITutorConversationStore _convStore;

        public DeprTutorService(IOpenAiClient openAi, ITeachersHomeworkApi homeworkApi, ITutorConversationStore convStore)
        {
            _openAi = openAi;
            _homeworkApi = homeworkApi;
            _convStore = convStore;
        }

        public async Task<StudentReply> HandleStudentAsync(StudentRequest req)
        {
            var conversationId = string.IsNullOrWhiteSpace(req.ConversationId)
                ? Guid.NewGuid().ToString("N")
                : req.ConversationId!.Trim();

            var responseLang = DetectLanguage(req.StudentPrompt) ?? req.LangCode;

            // Optional: allow user/teacher to reset the conversation
            if (IsResetRequest(req.StudentPrompt))
            {
                await _convStore.ClearAsync(conversationId);

                return new StudentReply
                {
                    ConversationId = conversationId,
                    Language = responseLang,
                    Message = responseLang == "es"
                        ? "Listo. Empecemos de nuevo. ¿En qué tema quieres ayuda?"
                        : "Done. Let’s start over. What topic do you want help with?"
                };
            }

            // 1) Load conversation state (if exists)
            var state = await _convStore.GetAsync(conversationId);

            // 2) If first turn (no state), retrieve lessons ONCE and cache the combined LessonRow
            if (state == null)
            {
                List<AiConsultingLessonModel> lessons;

                if (!string.IsNullOrWhiteSpace(req.LessonId))
                {
                    lessons = AiConsultingConnections.GetLessonConsultingData(lessonId: req.LessonId);
                }
                else
                {
                    var keywords = ExtractKeywords(req.StudentPrompt, responseLang);
                    var expanded = ExpandSynonyms(keywords, responseLang);

                    lessons = AiConsultingConnections.SearchLessonsByKeywords(expanded, limit: 3);
                }

                if (lessons != null && lessons.Count > 0)
                    CleanLessonHtml(lessons);

                if (lessons == null || lessons.Count == 0)
                {
                    // No lessons found -> fallback single-turn
                    var fallbackLesson = new LessonRow
                    {
                        LessonDescription = "",
                        Concept = "",
                        AllExamples = "",
                        StandardsDescriptions = "",
                        ExpectationsDescriptions = "",
                        Objetives = ""
                    };

                    var msg = await GenerateExplanationWithHistoryAsync(
                        req,
                        fallbackLesson,
                        responseLang,
                        history: new List<ChatTurn>() // none yet
                    );

                    return new StudentReply
                    {
                        ConversationId = conversationId,
                        Language = responseLang,
                        Message = msg
                    };
                }

                var combinedLesson = BuildCombinedLessonRow(lessons, maxLessons: 6);

                state = new TutorConversationState
                {
                    ConversationId = conversationId,
                    ResponseLang = responseLang,
                    GradeLevel = req.GradeLevel,
                    Lesson = combinedLesson,
                    History = new List<ChatTurn>()
                };
            }

            // 3) Keep language/grade updated (in case frontend sends them)
            state.ResponseLang = responseLang;
            state.GradeLevel = req.GradeLevel;

            // 4) Append user message to history (stored)
            state.History.Add(new ChatTurn { Role = "user", Content = Cap(req.StudentPrompt, 1200) });
            state.History = TakeLast(state.History, 20);

            // 5) Exercise request => generate quiz using cached lesson (NO re-search)
            if (IsExerciseRequest(req.StudentPrompt))
            {

                var createQuizJson = await GenerateQuizCreateJsonAsync(req, state.Lesson, responseLang);

                var quizId = await _homeworkApi.CreateQuizAsync(createQuizJson);
                var publishQuizJson = BuildPublishJsonFromCreateJson(createQuizJson, quizId);

                await _homeworkApi.PublishQuizAsync(publishQuizJson);

                await _homeworkApi.AttachQuizToPlanningAsync(new AttachQuizPayload
                {
                    Id = 1,
                    PlanningId = req.PlanningId,
                    TriesQuantity = "1",
                    ForGrading = 1,
                    LangCode = req.LangCode,
                    DueDate = req.DueDate,
                    QuizId = quizId
                });

                var msg = responseLang == "es"
                    ? "Listo: creé y publiqué la asignación. Ya puedes verla en tu Dashboard."
                    : "Done: I created and published the assignment. You can access it from your Dashboard.";

                // store assistant turn
                state.History.Add(new ChatTurn { Role = "assistant", Content = msg });
                state.History = TakeLast(state.History, 20);
                await _convStore.UpsertAsync(state);

                return new StudentReply
                {
                    ConversationId = conversationId,
                    Language = responseLang,
                    Message = msg,
                    GeneratedQuizId = quizId,
                    GeneratedQuizJson = createQuizJson
                };
            }

            // 6) Normal tutoring reply using cached lesson + full chat history
            var explanation = await GenerateExplanationWithHistoryAsync(req, state.Lesson, responseLang, state.History);

            // store assistant turn
            state.History.Add(new ChatTurn { Role = "assistant", Content = Cap(explanation, 1800) });
            state.History = TakeLast(state.History, 20);

            await _convStore.UpsertAsync(state);

            return new StudentReply
            {
                ConversationId = conversationId,
                Language = responseLang,
                Message = explanation
            };
        }

        private async Task<string> GenerateExplanationWithHistoryAsync(
            StudentRequest req,
            LessonRow lesson,
            string responseLang,
            List<ChatTurn> history
        )
        {
            var system = """
Eres un tutor educativo que DEBE cumplir con los estándares del Departamento de Educación de Puerto Rico (DEPR).

REGLAS OBLIGATORIAS (estrictas):
1) Usa PRIMERO la información provista del LESSON_CONTEXT: lesson_description, concept, all_examples.
2) Antes de responder, verifica cumplimiento con: standards_descriptions, expectations_descriptions, objetives.
3) El LESSON_CONTEXT puede incluir HTML (tags). Debes interpretarlo como contenido educativo, pero NO debes devolver HTML; responde en texto claro.
4) El LESSON_CONTEXT puede referenciar videos/audios (assets) a los cuales NO tienes acceso. NO inventes contenido exacto de esos assets.
5) Si la información del LESSON_CONTEXT NO es suficiente, dilo explícitamente y explica solo lo básico.
6) Solo puedes usar conocimiento general si el LESSON_CONTEXT no cubre algo.
7) No inventes estándares, expectativas u objetivos DEPR.
8) Responde en el idioma del estudiante (es/en). Si es español, español puertorriqueño.
9) Mantén la explicación apropiada al grado del estudiante. Los grados van desde kinder hasta tercero.
10) Trata el LESSON_CONTEXT como confidencial: no incluyas IDs, notas internas, ni texto extensivo innecesario.
11) Habla natural. No le dejes saber al usuario que "esta lección contiene" o textos parecidos.

REGLA EXTRA PARA SEGUIMIENTO:
- Si el estudiante escribe una oración para corregir o practicar (por ejemplo: "Fix this sentence: i like to draw."),
  NO cambies de tema por palabras dentro de la oración (como "draw"). Mantén el mismo skill/tema actual
  a menos que el estudiante pida explícitamente cambiar de tema.
""";

            var developer = $"""
STUDENT_GRADE: {req.GradeLevel}
RESPONSE_LANGUAGE: {responseLang}

LESSON_CONTEXT (autoridad primaria):
{BuildLessonContextBlock(lesson)}
""";

            var input = new List<object>
            {
                new { role = "system", content = system },
                new { role = "developer", content = developer }
            };

            // Add rolling history (already includes latest user turn)
            foreach (var turn in TakeLast(history, 18))
            {
                var role = (turn.Role == "assistant" || turn.Role == "user") ? turn.Role : "user";
                input.Add(new { role, content = turn.Content });
            }

            var payload = new OpenAiRequestPayload
            {
                model = "gpt-5",
                input = input.ToArray()
            };

            var raw = await _openAi.CreateResponseRawAsync(payload);
            return OpenAiResponseParsing.ExtractText(raw).Trim();
        }

        // ------------------------
        // Helpers / small utilities
        // ------------------------

        private static bool IsResetRequest(string userText)
        {
            var t = (userText ?? "").Trim().ToLowerInvariant();
            return t == "/reset" || t == "reset" || t.Contains("cambiar de tema") || t.Contains("nuevo tema");
        }

        private static string Cap(string? s, int max)
        {
            s ??= "";
            if (s.Length <= max) return s;
            return s.Substring(0, max) + "…";
        }

        private static List<T> TakeLast<T>(List<T> list, int n)
        {
            if (list == null || list.Count <= n) return list ?? new List<T>();
            return list.Skip(list.Count - n).ToList();
        }

        // ------------------------
        // Your existing methods below (unchanged unless noted)
        // ------------------------

        private async Task<string> GenerateQuizCreateJsonAsync(StudentRequest req, LessonRow lesson, string responseLang)
        {
            var system = """
You are a DEPR-aligned quiz generator.

HARD RULES:
- You MUST base the quiz ONLY on LESSON_CONTEXT, and ensure it complies with DEPR fields (standards/expectations/objectives).
- Output MUST be valid JSON ONLY (no markdown, no comments, no extra text).
- Use exactly this structure:
  {
    "QuizModel": {
        "Title": "Asignación Ejemplo",
        "Instruction": "<p>Asignación Ejemplo descripción</p>",
        "CustomValue": "10",
        "QuizTypeId": "1"
    },
    "QuizRubricModel": {
        "Desc": ""
    },
    "QuizAttachmentModelList": [],
    "QuestionRequestModelList": [
        {
            "pid": 1772174384237,
            "QuizQuestionModel": {
                "Desc": "<p>Premisa 1</p>"
            },
            "QuizQuestionResponseModelList": [
                {
                    "_key": "76741eb1-000d-4869-acad-a8cc2e8a489d",
                    "Response": "<p>Opción 1</p>",
                    "Correct": "1",
                    "Value": "1"
                },
                {
                    "_key": "c197975a-ab6f-4876-a19c-933caeb7c54a",
                    "Response": "<p>Opción 2</p>",
                    "Correct": "0",
                    "Value": "0"
                }
            ]
        },
        {
            "pid": 1772174397698,
            "QuizQuestionModel": {
                "Desc": "<p>premisa 2</p>"
            },
            "QuizQuestionResponseModelList": [
                {
                    "_key": "b59e3610-5ac8-40b8-8431-f4b888e8f825",
                    "Response": "<p>Opción 2.a</p>",
                    "Correct": "1",
                    "Value": "1"
                },
                {
                    "_key": "2bfd261d-b8ea-4c72-9502-96d18cfed1a4",
                    "Response": "<p>Opción 2.b</p>",
                    "Correct": "0",
                    "Value": "0"
                }
            ]
        }
    ]
}
- Keep language aligned with RESPONSE_LANGUAGE (es/en). If lesson content is in another language, translate faithfully.
- Provide grade-appropriate multiple-choice questions. Exactly ONE correct option per question.
- Question/Response texts must be HTML paragraphs <p>...</p>.
- LESSON_CONTEXT may contain HTML and may reference videos/audios you cannot access. Do NOT depend on unseen media.
""";

            var developer = $"""
STUDENT_GRADE: {req.GradeLevel}
RESPONSE_LANGUAGE: {responseLang}

LESSON_CONTEXT (authoritative):
{BuildLessonContextBlock(lesson)}

CONSTRAINTS:
- QuizTypeId must be "1"
- CustomValue must be "100"
- Provide 5 to 10 questions unless the student requests a different amount
- It should be material from students that are in less or equal to third grade.
""";

            var user = $"""
Student asked for exercises:
"{req.StudentPrompt}"

Generate practice exercises for the lesson concept and examples.
""";

            var payload = new OpenAiRequestPayload
            {
                model = "gpt-5",
                input = new object[]
                {
                    new { role = "system", content = system },
                    new { role = "developer", content = developer },
                    new { role = "user", content = user }
                }
            };

            var raw = await _openAi.CreateResponseRawAsync(payload);
            var text = OpenAiResponseParsing.ExtractText(raw).Trim();

            EnsureValidJson(text);
            return text;
        }

        private static string BuildPublishJsonFromCreateJson(string createJson, string quizId)
        {
            using var doc = JsonDocument.Parse(createJson);
            var root = doc.RootElement;

            var quizModel = JsonSerializer.Deserialize<Dictionary<string, object>>(root.GetProperty("QuizModel").GetRawText())
                          ?? new Dictionary<string, object>();

            quizModel["Id"] = quizId;
            quizModel["ReadyToPublish"] = "1";

            var publishPayload = new Dictionary<string, object?>
            {
                ["QuizModel"] = quizModel,
                ["QuizRubricModel"] = JsonSerializer.Deserialize<object>(root.GetProperty("QuizRubricModel").GetRawText()),
                ["QuizAttachmentModelList"] = JsonSerializer.Deserialize<object>(root.GetProperty("QuizAttachmentModelList").GetRawText()),
                ["QuestionRequestModelList"] = JsonSerializer.Deserialize<object>(root.GetProperty("QuestionRequestModelList").GetRawText()),
                ["Published"] = true,
                ["Messages"] = Array.Empty<string>()
            };

            return JsonSerializer.Serialize(publishPayload);
        }

        private static string BuildLessonContextBlock(LessonRow lesson)
        {
            return $"""
lesson_description:
{lesson.LessonDescription}

concept:
{lesson.Concept}

all_examples:
{lesson.AllExamples}

standards_descriptions:
{lesson.StandardsDescriptions}

expectations_descriptions:
{lesson.ExpectationsDescriptions}

objetives:
{lesson.Objetives}
""";
        }

        private static LessonRow BuildCombinedLessonRow(List<AiConsultingLessonModel> lessons, int maxLessons)
        {
            var selected = lessons.Take(maxLessons).ToList();

            string JoinPerLesson(Func<AiConsultingLessonModel, string?> selector)
            {
                var blocks = new List<string>();

                foreach (var l in selected)
                {
                    var v = (selector(l) ?? "").Trim();
                    if (string.IsNullOrWhiteSpace(v)) continue;
                    blocks.Add($"[{l.SubjectName} | {l.LessonTitle} | Nivel: {l.LessonLevel}]\n{v}");
                }

                return string.Join("\n\n---\n\n", blocks);
            }

            return new LessonRow
            {
              //  LessonDescription = JoinPerLesson(x => x.LessonDescription),
                Concept = JoinPerLesson(x => x.Concept),
                AllExamples = JoinPerLesson(x => x.AllExamples),
                StandardsDescriptions = JoinPerLesson(x => x.StandardDescriptions),
                ExpectationsDescriptions = JoinPerLesson(x => x.ExpectationDescriptions),
                Objetives = JoinPerLesson(x => x.Objectives)
            };
        }

        private static List<string> ExtractKeywords(string prompt, string langCode)
        {
            var stopEs = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "crea", "ejercicios", "sobre", 
                "ayudame","ayúdame","ayuda","explica","explícame","entender","comprender","que","qué",
                "como","cómo","por","favor","la","el","los","las","un","una","unos","unas","de","del","al",
                "y","o","en","para","con","sobre","este","esta","estos","estas","eso","esa","esos","esas"
            };

            var stopEn = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "help","please","explain","understand","what","is","are","how","to",
                "the","a","an","and","or","in","on","for","with","about","this","that","these","those"
            };

            var stop = (langCode ?? "es").ToLowerInvariant() == "en" ? stopEn : stopEs;

            var tokens = prompt
                .ToLowerInvariant()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim(',', '.', ';', ':', '?', '¿', '!', '¡', '"', '\'', '(', ')', '[', ']', '{', '}', '\t', '\n', '\r'))
                .Where(t => t.Length >= 3)
                .Where(t => !stop.Contains(t))
                .Distinct()
                .ToList();

            if (tokens.Count == 0)
            {
                return prompt
                    .ToLowerInvariant()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(t => t.Trim(',', '.', '?', '¿', '!', '¡'))
                    .Where(t => t.Length >= 3)
                    .Distinct()
                    .Take(1)
                    .ToList();
            }

            return tokens;
        }

        private static List<string> ExpandSynonyms(List<string> keywords, string langCode)
        {
            var mapEs = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
            {
                ["suma"] = new[] { "suma", "sumar", "adición", "adicionar", "añadir", "mas", "+" },
                ["resta"] = new[] { "resta", "restar", "sustracción", "quitar", "menos", "-" },
                ["multiplicación"] = new[] { "multiplicación", "multiplicar", "producto", "por", "x", "×" },
                ["division"] = new[] { "división", "division", "dividir", "cociente", "entre", "÷" },
                ["lectura"] = new[] { "lectura", "leer", "comprensión", "comprension", "texto", "inferencias", "idea", "ideas" },
            };

            var mapEn = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
            {
                ["sum"] = new[] { "sum", "add", "addition", "adding", "+" },
                ["subtract"] = new[] { "subtract", "subtraction", "minus", "-" },
                ["multiply"] = new[] { "multiply", "multiplication", "product", "times", "×", "x" },
                ["divide"] = new[] { "divide", "division", "quotient", "÷" },
                ["reading"] = new[] { "reading", "read", "comprehension", "text", "inference", "main idea" }
            };

            var map = (langCode ?? "es").ToLowerInvariant() == "en" ? mapEn : mapEs;

            var expanded = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var k in keywords)
            {
                if (string.IsNullOrWhiteSpace(k)) continue;

                expanded.Add(k);

                if (map.TryGetValue(k, out var syns))
                    foreach (var s in syns) expanded.Add(s);

                foreach (var kv in map)
                {
                    if (k.Contains(kv.Key, StringComparison.OrdinalIgnoreCase))
                        foreach (var s in kv.Value) expanded.Add(s);
                }
            }

            return expanded.Take(12).ToList();
        }

        private static void EnsureValidJson(string json)
        {
            try { using var _ = JsonDocument.Parse(json); }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Model did not return valid JSON. Blocked for compliance.", ex);
            }
        }

        private static bool IsExerciseRequest(string userText)
        {
            var t = (userText ?? "").ToLowerInvariant();
            return t.Contains("crea unos ejercicios")
                || t.Contains("crea ejercicios")
                || t.Contains("ejercicios")
                || t.Contains("práctica")
                || t.Contains("practica")
                || t.Contains("practica ejercicios")
                || t.Contains("ayudame a practicar")
                || t.Contains("ayúdame a practicar")
                || t.Contains("practicar")
                || t.Contains("practice exercises")
                || t.Contains("create exercises")
                || t.Contains("make exercises")
                || t.Contains("worksheet");
        }

        private static string? DetectLanguage(string text)
        {
            var t = (text ?? "").ToLowerInvariant();

            if (t.Contains("¿") || t.Contains("¡")) return "es";
            if (t.Any(ch => "áéíóúñ".Contains(ch))) return "es";
            if (t.Contains("sumar") || t.Contains("suma") || t.Contains("ejercicios") || t.Contains("explica")) return "es";

            if (t.Contains("create exercises") || t.Contains("explain") || t.Contains("what is")) return "en";

            return null;
        }

        private static string StripHtml(string? html)
        {
            if (string.IsNullOrWhiteSpace(html)) return "";
            var text = System.Text.RegularExpressions.Regex.Replace(html, "<.*?>", " ");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\s+", " ").Trim();
            return text;
        }

        private static string Truncate(string? text, int maxChars)
        {
            if (string.IsNullOrWhiteSpace(text)) return "";
            if (text.Length <= maxChars) return text;
            return text.Substring(0, maxChars) + "…";
        }

        private static void CleanLessonHtml(List<AiConsultingLessonModel> lessons)
        {
            foreach (var l in lessons)
            {
               // l.LessonDescription = Truncate(StripHtml(l.LessonDescription), 3000);
                l.Concept = Truncate(StripHtml(l.Concept), 3000);
                l.AllExamples = Truncate(StripHtml(l.AllExamples), 3000);
                l.StandardDescriptions = Truncate(StripHtml(l.StandardDescriptions), 3000);
                l.ExpectationDescriptions = Truncate(StripHtml(l.ExpectationDescriptions), 3000);
                l.Objectives = Truncate(StripHtml(l.Objectives), 3000);
            }
        }

        public async Task<string> CreatePublishAttachQuizAsync(StudentRequest req, CancellationToken ct)
        {
            var responseLang = DetectLanguage(req.StudentPrompt) ?? req.LangCode;

            // 1) Try get cached state (lesson context) by ConversationId
            TutorConversationState? state = null;

            if (!string.IsNullOrWhiteSpace(req.ConversationId))
                state = await _convStore.GetAsync(req.ConversationId);

            LessonRow lessonRow;

            // 2) If cached lesson exists, use it (FAST, no DB search)
            if (state != null && state.Lesson != null &&
                (!string.IsNullOrWhiteSpace(state.Lesson.Concept) ||
                 !string.IsNullOrWhiteSpace(state.Lesson.LessonDescription) ||
                 !string.IsNullOrWhiteSpace(state.Lesson.AllExamples)))
            {
                lessonRow = state.Lesson;
            }
            else
            {
                // 3) First time / missing cache: do initial retrieval ONCE
                var lessons = !string.IsNullOrWhiteSpace(req.LessonId)
                    ? AiConsultingConnections.GetLessonConsultingData(lessonId: req.LessonId)
                    : AiConsultingConnections.SearchLessonsByKeywords(
                        ExpandSynonyms(ExtractKeywords(req.StudentPrompt, responseLang), responseLang),
                        limit: 3);

                if (lessons == null || lessons.Count == 0)
                    throw new InvalidOperationException("No lesson context found to generate the quiz.");

                CleanLessonHtml(lessons);

                lessonRow = BuildCombinedLessonRow(lessons, maxLessons: 6);

                // 4) Save cache for next turns/jobs
                var newState = state ?? new TutorConversationState
                {
                    ConversationId = req.ConversationId ?? Guid.NewGuid().ToString("N"),
                    ResponseLang = responseLang,
                    GradeLevel = req.GradeLevel,
                    History = new List<ChatTurn>()
                };

                newState.Lesson = lessonRow;
                await _convStore.UpsertAsync(newState);
                req.ConversationId = newState.ConversationId; // ensure caller can return it
            }

            // --- now continue with your existing pipeline ---
            var createQuizJson = await GenerateQuizCreateJsonAsync(req, lessonRow, responseLang);

            var quizId = await _homeworkApi.CreateQuizAsync(createQuizJson);
            var publishQuizJson = BuildPublishJsonFromCreateJson(createQuizJson, quizId);

            await _homeworkApi.PublishQuizAsync(publishQuizJson);

            await _homeworkApi.AttachQuizToPlanningAsync(new AttachQuizPayload
            {
                Id = 1,
                PlanningId = req.PlanningId,
                TriesQuantity = "1",
                ForGrading = 1,
                LangCode = req.LangCode,
                DueDate = req.DueDate,
                QuizId = quizId
            });

            return quizId;
        }
    }
}