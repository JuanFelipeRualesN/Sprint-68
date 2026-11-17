// Empty slide factory
// order is 1-based and must be contiguous
export const emptySlide = (order) => ({
    id: null,
    slideOrder: order,
    textEn: '',
    textEs: ''
});

// Empty story form used when creating a new story
export const emptyForm = () => ({
    id: null,
    levelId: null,
    categoryId: null,
    titleEn: '',
    titleEs: '',
    fluencyGoal: 70,
    accuracyGoal: 70,
    completenessGoal: 70,
    pronunciationGoal: 70,
    slides: [emptySlide(1)]
});

// Count words in a slide for a given language
export const countWords = (slides, key) =>
    slides.reduce((total, s) => {
        const words = (s[key] || '').trim().split(/\s+/).filter(Boolean);
        return total + words.length;
    }, 0);

// Formats the backend response into the form shape
export const dtoToForm = (dto) => ({
    id: dto.Id,
    levelId: dto.LevelId,
    categoryId: dto.CategoryId,
    titleEn: dto.TitleEn || '',
    titleEs: dto.TitleEs || '',
    fluencyGoal: dto.FluencyGoal || 0,
    accuracyGoal: dto.AccuracyGoal || 0,
    completenessGoal: dto.CompletenessGoal || 0,
    pronunciationGoal: dto.PronunciationGoal || 0,
    slides: (dto.Slides || [])
        .slice()
        .sort((a, b) => a.SlideOrder - b.SlideOrder)
        .map((s) => ({
            id: s.Id,
            slideOrder: s.SlideOrder,
            textEn: s.TextEn || '',
            textEs: s.TextEs || ''
        }))
});

// Formats the form data into the shape the backend expects
export const formToDto = (form) => ({
    LevelId: form.levelId,
    CategoryId: form.categoryId,
    TitleEn: form.titleEn.trim(),
    TitleEs: form.titleEs.trim(),
    ImageUrl: "https://stpubliceastus001.blob.core.windows.net/public-assets/categories/scifi.png",
    FluencyGoal: Number(form.fluencyGoal) || 0,
    AccuracyGoal: Number(form.accuracyGoal) || 0,
    CompletenessGoal: Number(form.completenessGoal) || 0,
    PronunciationGoal: Number(form.pronunciationGoal) || 0,
    Slides: form.slides.map((s, i) => ({
        SlideOrder: i + 1,
        TextEn: s.textEn.trim(),
        TextEs: s.textEs.trim(),
        ImageUrl: null
    }))
});
