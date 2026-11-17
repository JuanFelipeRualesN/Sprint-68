import { useState, useCallback, useEffect } from 'react';
// Assuming this path is correct for your API
import { getReadingQuestionDiagnostics } from '../../../../../api/athenas';

// --- INITIAL DIAGNOSTIC DATA ---
const initialDiagnostics = [
    { Id: 1, Title: 'Diagnostic 1', LevelCode: 'K', SubjectCode: 'English', Published: true, questions: [] },
    { Id: 2, Title: 'Diagnostic 2', LevelCode: '1', SubjectCode: 'English', Published: true, questions: [] },
];

// --- FALLBACK PLACEHOLDER QUESTIONS ---
const fallbackQuestions = [{
    id: "R001",
    code: "READ01",
    title: "Vocabulary in Context",
    subject: "English Reading",
    grade: "1st",
    standard: "1.R.1.1",
    text: "Reading is a ____ way to learn.",
    answers: [
        { id: "A", text: "wonderful", isCorrect: true },
        { id: "B", text: "bad", isCorrect: false },
        { id: "C", text: "silly", isCorrect: false },
    ]
}];

/** Map API -> UI question shape */
function mapApiToUi(apiItems) {
    if (!Array.isArray(apiItems)) return [];

    return apiItems.map((item, idx) => ({
        id: `R${String(idx + 1).padStart(3, '0')}`,
        code: item?.ConstructedFullCode || '',
        title: item?.Question || 'Reading Question',
        subject: item?.SubjectCode || '',
        grade: item?.LevelCode || '',
        standard: item?.ConstructedFullCode || '',
        text: item?.Question || '',
        answers: (item?.Responses || []).map((r, i) => ({
            id: String.fromCharCode(65 + i), // A, B, C...
            text: r?.Text ?? '',
            isCorrect: !!r?.IsCorrect
        }))
    }));
}

export const formatQuestionSummary = (htmlText) => {
    if (!htmlText) return '';

    // 1. Check for media tags (<img> or <audio>) ONLY
    // Regex to match a string containing ONLY img/audio tags and optional whitespace.
    const mediaOnlyRegex = /^\s*(<img[^>]*>|<audio[^>]*>)\s*$/i;

    if (mediaOnlyRegex.test(htmlText.trim())) {
        // Find the tag and return its code (e.g., "<img src='...'")
        const imgMatch = htmlText.match(/<img[^>]*>/i);
        if (imgMatch) return imgMatch[0];

        const audioMatch = htmlText.match(/<audio[^>]*>/i);
        if (audioMatch) return audioMatch[0];
    }

    // 2. Default: Strip HTML and limit length
    // Create a temporary div, set innerHTML, and retrieve textContent to strip HTML
    const tempDiv = document.createElement('div');
    tempDiv.innerHTML = htmlText;
    let plainText = tempDiv.textContent || tempDiv.innerText || '';

    // Clean up whitespace
    plainText = plainText.trim().replace(/\s+/g, ' ');

    const maxLength = 100;

    if (plainText.length > maxLength) {
        return plainText.substring(0, maxLength) + '...';
    }

    return plainText;
}


// ---------------------------------
export const useDiagnostics = () => {
    const [diagnostics, setDiagnostics] = useState(initialDiagnostics);
    const [currentDiagnostic, setCurrentDiagnostic] = useState(null);

    // This state holds the questions for the modal
    const [availableQuestions, setAvailableQuestions] = useState(fallbackQuestions);

    // Load real questions once from the API
    useEffect(() => {
        let cancelled = false;

        (async () => {
            try {
                // API call occurs here
                const apiData = await getReadingQuestionDiagnostics(null, null, null);
                const mapped = mapApiToUi(apiData).filter(q => q && q.id);
                if (!cancelled && mapped.length) setAvailableQuestions(mapped);
            } catch (err) {
                console.error('Failed to load diagnostic questions:', err);
                if (!cancelled) setAvailableQuestions(fallbackQuestions); // keep fallback on error
            }
        })();

        return () => { cancelled = true; };
    }, []);

    const getDiagnosticById = useCallback((id) => {
        return diagnostics.find(d => d.id === id);
    }, [diagnostics]);

    const saveDiagnostic = (diagnostic) => {
        if (!diagnostic?.questions || diagnostic.questions.length < 3) {
            alert("Diagnostic must contain at least three (3) questions before saving.");
            return false;
        }

        setDiagnostics(prev => {
            const index = prev.findIndex(d => d.id === diagnostic.id);
            if (index !== -1) {
                return prev.map(d => (d.id === diagnostic.id ? diagnostic : d));
            } else {
                const newId = Math.max(...prev.map(d => d.id), 0) + 1;
                return [...prev, { ...diagnostic, id: newId }];
            }
        });
        return true;
    };

    const deleteDiagnostic = (id) => {
        setDiagnostics(prev => prev.filter(d => d.id !== id));
    };

    const publishDiagnostic = (id) => {
        setDiagnostics(prev => prev.map(d =>
            d.id === id ? { ...d, published: true } : d
        ));
    };

    return {
        diagnostics,
        currentDiagnostic,
        setCurrentDiagnostic,
        availableQuestions,
        getDiagnosticById,
        saveDiagnostic,
        deleteDiagnostic,
        publishDiagnostic
    };
};