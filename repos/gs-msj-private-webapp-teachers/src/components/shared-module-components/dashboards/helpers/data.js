export const students = [
    { id: 1, name: 'Sofía Martínez' },
    { id: 2, name: 'Alejandro Vargas' },
    { id: 3, name: 'Isabella Gómez' },
    { id: 4, name: 'Juan Perez' },
    { id: 5, name: 'María López' },
];

export const groups = [
    { id: 1, name: 'Grupo A - 3.er grado' },
    { id: 2, name: 'Grupo B - 4.o grado' },
];

export const languages = ['English', 'Español'];

// Focus Areas filter options (Only applicable to Progress & Mastery)
export const focusAreas = [
    { id: 1, name: 'Reconocimiento fonético y conciencia fonológica', en: 'Phonic recognition and Phonological awareness' },
    { id: 2, name: 'Fluidez', en: 'Fluency' },
    { id: 3, name: 'Comprensión lectora (conceptos impresos)', en: 'Reading comprehension (print concepts)' },
];

export const regions = {
    admin : ['Arecibo', 'Bayamón', 'Humacao', 'San Juan'],
    adminOre : ['San Juan'],
}

export const schools = [
    { id: 101, name: 'Escuela Intermedia Central', region: 'San Juan' },
    { id: 105, name: 'Escuela Primaria de San Juan', region: 'San Juan' },
    { id: 102, name: 'Academia del Sur', region: 'Humacao' },
    { id: 103, name: 'Bayamón High School', region: 'Bayamón' },
    { id: 104, name: 'Escuela Secundaria de Arecibo', region: 'Arecibo' },
    // ... more schools
];


export const diagnosticResultsMockedData = [
    { studentName: 'Sofía Martínez', score: 78, subject: 'en', level: 'Kindergarten' },
    { studentName: 'Sofía Martínez', score: 91, subject: 'sp', level: 'Kindergarten' },
    { studentName: 'Sofía Martínez', score: 85, subject: 'en', level: 'Primer Grado' },

    { studentName: 'Alejandro Vargas', score: 45, subject: 'en', level: 'Segundo Grado' },
    { studentName: 'Alejandro Vargas', score: 62, subject: 'sp', level: 'Segundo Grado' },
    { studentName: 'Alejandro Vargas', score: 70, subject: 'en', level: 'Tercer Grado' },

    { studentName: 'Isabella Gómez', score: 92, subject: 'en', level: 'Tercer Grado' },
    { studentName: 'Isabella Gómez', score: 79, subject: 'sp', level: 'Tercer Grado' },
    { studentName: 'Isabella Gómez', score: 88, subject: 'en', level: 'Cuarto Grado' },

    { studentName: 'Juan Perez', score: 35, subject: 'en', level: 'Cuarto Grado' },
    { studentName: 'Juan Perez', score: 55, subject: 'sp', level: 'Cuarto Grado' },
    { studentName: 'Juan Perez', score: 40, subject: 'en', level: 'Quinto Grado' },

    { studentName: 'María López', score: 60, subject: 'en', level: 'Quinto Grado' },
    { studentName: 'María López', score: 88, subject: 'sp', level: 'Quinto Grado' },
    { studentName: 'María López', score: 95, subject: 'en', level: 'Cuarto Grado' },
];

export const studentExamResults = [
    {
        id: 1,
        name: 'Sofía Martínez',
        exams: [
            {
                examName: 'Examen 1: Fonética, reconocimiento de palabras y fluidez',
                score: 78,
                topic: 'Identificación de fonemas',
                standardCode: 'K.L.2.1q',
                language: 'sp',
                category: 'c1',
                completedDate: '2025-10-10',
                subject: 'Spanish',
                focusArea: 'Reconocimiento fonético y conciencia fonológica'
            },
            {
                examName: 'Examen 2: Describe Ideas: Predict, Infer, and Draw Conclusions',
                score: 85,
                topic: 'Idea principal en párrafos cortos',
                standardCode: '4.R.1.1',
                language: 'en',
                category: 'c2',
                completedDate: '2025-10-14',
                subject: 'English',
                focusArea: 'Fluidez'
            }
        ]
    },
    {
        id: 2,
        name: 'Alejandro Vargas',
        exams: [
            {
                examName: 'Examen 1: Fonética, reconocimiento de palabras y fluidez',
                score: 45,
                topic: 'Identificación de fonemas',
                standardCode: '1.L.2.1f',
                language: 'sp',
                category: 'c1',
                completedDate: '2025-10-09',
                subject: 'Spanish',
                focusArea: 'Reconocimiento fonético y conciencia fonológica'
            }
        ]
    },
    {
        id: 3,
        name: 'Isabella Gómez',
        exams: [
            {
                examName: 'Examen 1: Fonética, reconocimiento de palabras y fluidez',
                score: 92,
                topic: 'Identificación de fonemas',
                standardCode: 'K.L.2.1m',
                language: 'sp',
                category: 'c1',
                completedDate: '2025-10-11',
                subject: 'Spanish',
                focusArea: 'Reconocimiento fonético y conciencia fonológica'
            },
            {
                examName: 'Examen 2: Describe Ideas: Predict, Infer, and Draw Conclusions',
                score: 72,
                topic: 'Idea principal en párrafos cortos',
                standardCode: '5.R.1.1',
                language: 'en',
                category: 'c2',
                completedDate: '2025-10-15',
                subject: 'English',
                focusArea: 'Comprensión lectora (conceptos impresos)'

            }
        ]
    },
        {
        id: 4,
        name: 'Juan Perez',
        exams: [
            {
                examName: 'Examen 1: Fonética, reconocimiento de palabras y fluidez',
                score: 0,
                topic: 'Identificación de fonemas',
                standardCode: 'K.L.2.1m',
                language: 'sp',
                category: 'c1',
                completedDate: 'N/A',
                subject: 'Spanish',
                focusArea: 'Reconocimiento fonético y conciencia fonológica'
            },
        ]
    },
    {
        id: 5,
        name: 'María López',
        exams: [
            {
                examName: 'Examen 1: Fonética, reconocimiento de palabras y fluidez',
                score: 60,
                topic: 'Identificación de fonemas',
                standardCode: 'K.L.2.1i',
                language: 'sp',
                category: 'c1',
                completedDate: '2025-10-08',
                subject: 'Spanish',
                focusArea: 'Reconocimiento fonético y conciencia fonológica'
            },
            {
                examName: 'Examen 3: Point of View',
                score: 95,
                topic: 'Lectura de palabras de alta frecuencia',
                standardCode: '5.R.8.1',
                language: 'en',
                category: 'c2',
                completedDate: '2025-10-16',
                subject: 'English',
                focusArea: 'Fluidez'
            }
        ]
    }
];


export const mockUsageMetrics = {
    teacher : [{
        studentName: 'Sofía Martínez',
        totalSessions: 35,
        totalActiveDays: 12,
        averageSessionDuration: 28, // min
        engagementScore: 92,
        sessionDuration: [
            { day: 'monday', duration: 25 },
            { day: 'tuesday', duration: 30 },
            { day: 'wednesday', duration: 28 },
            { day: 'thursday', duration: 32 },
            { day: 'friday', duration: 27 },
            { day: 'saturday', duration: 29 },
            { day: 'sunday', duration: 0 },
        ],
        sessionsPerDay: [
            { day: 'monday', sessions: 5 },
            { day: 'tuesday', sessions: 6 },
            { day: 'wednesday', sessions: 4 },
            { day: 'thursday', sessions: 7 },
            { day: 'friday', sessions: 5 },
            { day: 'saturday', sessions: 3 },
            { day: 'sunday', sessions: 0 },
        ],
        devicesUsage: [{ desktop: 55, tablet: 35, mobile: 10 }],
        OffVsInSchoolUsage: [{ inSchool: 70, offSchool: 30 }],
    },
    {
        studentName: 'Alejandro Vargas',
        totalSessions: 15,
        totalActiveDays: 5,
        averageSessionDuration: 18,
        engagementScore: 55,
        sessionDuration: [
            { day: 'monday', duration: 17 },
            { day: 'tuesday', duration: 19 },
            { day: 'wednesday', duration: 18 },
            { day: 'thursday', duration: 20 },
            { day: 'friday', duration: 17 },
            { day: 'saturday', duration: 18 },
            { day: 'sunday', duration: 0 },
        ],
        sessionsPerDay: [
            { day: 'monday', sessions: 2 },
            { day: 'tuesday', sessions: 3 },
            { day: 'wednesday', sessions: 3 },
            { day: 'thursday', sessions: 3 },
            { day: 'friday', sessions: 2 },
            { day: 'saturday', sessions: 2 },
            { day: 'sunday', sessions: 0 },
        ],
        devicesUsage: [{ desktop: 30, tablet: 25, mobile: 45 }],
        OffVsInSchoolUsage: [{ inSchool: 60, offSchool: 40 }],
    },
    {
        studentName: 'Isabella Gómez',
        totalSessions: 42,
        totalActiveDays: 18,
        averageSessionDuration: 35,
        engagementScore: 98,
        sessionDuration: [
            { day: 'monday', duration: 34 },
            { day: 'tuesday', duration: 36 },
            { day: 'wednesday', duration: 35 },
            { day: 'thursday', duration: 38 },
            { day: 'friday', duration: 34 },
            { day: 'saturday', duration: 33 },
            { day: 'sunday', duration: 0 },
        ],
        sessionsPerDay: [
            { day: 'monday', sessions: 7 },
            { day: 'tuesday', sessions: 8 },
            { day: 'wednesday', sessions: 6 },
            { day: 'thursday', sessions: 9 },
            { day: 'friday', sessions: 7 },
            { day: 'saturday', sessions: 5 },
            { day: 'sunday', sessions: 0 },
        ],
        devicesUsage: [{ desktop: 60, tablet: 25, mobile: 15 }],
        OffVsInSchoolUsage: [{ inSchool: 78, offSchool: 22 }],
    },
    {
        studentName: 'María López',
        totalSessions: 22,
        totalActiveDays: 8,
        averageSessionDuration: 21,
        engagementScore: 70,
        sessionDuration: [
            { day: 'monday', duration: 22 },
            { day: 'tuesday', duration: 21 },
            { day: 'wednesday', duration: 20 },
            { day: 'thursday', duration: 23 },
            { day: 'friday', duration: 22 },
            { day: 'saturday', duration: 21 },
            { day: 'sunday', duration: 0 },
        ],
        sessionsPerDay: [
            { day: 'monday', sessions: 4 },
            { day: 'tuesday', sessions: 4 },
            { day: 'wednesday', sessions: 3 },
            { day: 'thursday', sessions: 4 },
            { day: 'friday', sessions: 4 },
            { day: 'saturday', sessions: 3 },
            { day: 'sunday', sessions: 0 },
        ],
        devicesUsage: [{ desktop: 45, tablet: 35, mobile: 20 }],
        OffVsInSchoolUsage: [{ inSchool: 65, offSchool: 35 }],
    },
    {
        // Aggregate data for "All Students" or when no student is selected
        studentName: 'Todos los estudiantes',
        totalSessions: 114, // Sum of all students above
        totalActiveDays: 43, // Sum of all students above
        averageSessionDuration: 25.5, // Average duration
        engagementScore: 80, // Average score
        sessionDuration: [
            { day: 'monday', duration: 27 },
            { day: 'tuesday', duration: 29 },
            { day: 'wednesday', duration: 27 },
            { day: 'thursday', duration: 31 },
            { day: 'friday', duration: 28 },
            { day: 'saturday', duration: 27 },
            { day: 'sunday', duration: 0 },
        ],
        sessionsPerDay: [
            { day: 'monday', sessions: 18 },
            { day: 'tuesday', sessions: 21 },
            { day: 'wednesday', sessions: 21 },
            { day: 'thursday', sessions: 23 },
            { day: 'friday', sessions: 18 },
            { day: 'saturday', sessions: 13 },
            { day: 'sunday', sessions: 0 },
        ],
        devicesUsage: [{ desktop: 52, tablet: 30, mobile: 18 }],
        OffVsInSchoolUsage: [{ inSchool: 71, offSchool: 29 }]
    },
    {
        groupName: 'Grupo A - 3.er grado',
        totalSessions: 80,
        totalActiveDays: 30,
        averageSessionDuration: 26,
        engagementScore: 85,
        sessionDuration: [
            { day: 'monday', duration: 26 },
            { day: 'tuesday', duration: 28 },
            { day: 'wednesday', duration: 26 },
            { day: 'thursday', duration: 30 },
            { day: 'friday', duration: 27 },
            { day: 'saturday', duration: 26 },
            { day: 'sunday', duration: 0 },
        ],
        sessionsPerDay: [
            { day: 'monday', sessions: 13 },
            { day: 'tuesday', sessions: 15 },
            { day: 'wednesday', sessions: 14 },
            { day: 'thursday', sessions: 16 },
            { day: 'friday', sessions: 12 },
            { day: 'saturday', sessions: 10 },
            { day: 'sunday', sessions: 0 },
        ],
        devicesUsage: [{ desktop: 50, tablet: 35, mobile: 15 }],
        OffVsInSchoolUsage: [{ inSchool: 73, offSchool: 27 }]
    },
    {
        groupName: 'Grupo B - 4.o grado',
        totalSessions: 34,
        totalActiveDays: 13,
        averageSessionDuration: 24,
        engagementScore:  67,
        sessionDuration: [
            { day: 'monday', duration: 24 },
            { day: 'tuesday', duration: 26 },
            { day: 'wednesday', duration: 24 },
            { day: 'thursday', duration: 27 },
            { day: 'friday', duration: 23 },
            { day: 'saturday', duration: 24 },
            { day: 'sunday', duration: 0 },
        ],
        sessionsPerDay: [
            { day: 'monday', sessions: 5 },
            { day: 'tuesday', sessions: 6 },
            { day: 'wednesday', sessions: 7 },
            { day: 'thursday', sessions: 7 },
            { day: 'friday', sessions: 6 },
            { day: 'saturday', sessions: 3 },
            { day: 'sunday', sessions: 0 },
        ],
        devicesUsage: [{ desktop: 55, tablet: 25, mobile: 20 }],
        OffVsInSchoolUsage: [{ inSchool: 68, offSchool: 32 }],
    }],
    admin: [
        {
            totalRegions: [
                { 
                    region: 'Arecibo', 
                    totalSessions: 278, 
                    engagementScore: 76,
                    averageSessionDuration: 36,
                    totalActiveDays: 85,
                    sessionDuration: [
                        { day: 'monday', duration: 40 },
                        { day: 'tuesday', duration: 41 },
                        { day: 'wednesday', duration: 38 },
                        { day: 'thursday', duration: 39 },
                        { day: 'friday', duration: 42 },
                        { day: 'saturday', duration: 37 },
                        { day: 'sunday', duration: 15 }
                    ],
                    sessionsPerDay: [
                        { day: 'monday', sessions: 45 },
                        { day: 'tuesday', sessions: 40 },
                        { day: 'wednesday', sessions: 35 },
                        { day: 'thursday', sessions: 38 },
                        { day: 'friday', sessions: 40 },
                        { day: 'saturday', sessions: 25 },
                        { day: 'sunday', sessions: 5 }
                    ],
                    devicesUsage: [{ desktop: 50, tablet: 30, mobile: 20 }],
                    OffVsInSchoolUsage: [{ inSchool: 72, offSchool: 28 }]
                },
                { 
                    region: 'Bayamón', 
                    totalSessions: 235,
                    totalActiveDays: 90,
                    averageSessionDuration: 38,
                    engagementScore: 78,
                    sessionDuration: [
                        { day: 'monday', duration: 39 },
                        { day: 'tuesday', duration: 38 },
                        { day: 'wednesday', duration: 41 },
                        { day: 'thursday', duration: 40 },
                        { day: 'friday', duration: 37 },
                        { day: 'saturday', duration: 35 },
                        { day: 'sunday', duration: 0 }
                    ],
                    sessionsPerDay: [
                        { day: 'monday', sessions: 40 },
                        { day: 'tuesday', sessions: 35 },
                        { day: 'wednesday', sessions: 38 },
                        { day: 'thursday', sessions: 42 },
                        { day: 'friday', sessions: 37 },
                        { day: 'saturday', sessions: 23 },
                        { day: 'sunday', sessions: 0 }
                    ],
                    devicesUsage: [{ desktop: 48, tablet: 32, mobile: 20 }],
                    OffVsInSchoolUsage: [{ inSchool: 68, offSchool: 32 }]
                },
                { 
                    region: 'Humacao', 
                    totalSessions: 110,
                    totalActiveDays: 60,
                    averageSessionDuration: 35,
                    engagementScore: 75,
                    sessionDuration: [
                        { day: 'monday', duration: 37 },
                        { day: 'tuesday', duration: 40 },
                        { day: 'wednesday', duration: 39 },
                        { day: 'thursday', duration: 42 },
                        { day: 'friday', duration: 38 },
                        { day: 'saturday', duration: 36 },
                        { day: 'sunday', duration: 0 }
                    ],
                    sessionsPerDay: [
                        { day: 'monday', sessions: 20 },
                        { day: 'tuesday', sessions: 18 },
                        { day: 'wednesday', sessions: 15 },
                        { day: 'thursday', sessions: 22 },
                        { day: 'friday', sessions: 20 },
                        { day: 'saturday', sessions: 15 },
                        { day: 'sunday', sessions: 0 }
                    ],
                    devicesUsage: [{ desktop: 50, tablet: 30, mobile: 20 }],
                    OffVsInSchoolUsage: [{ inSchool: 75, offSchool: 25 }]
                },
                {
                    region: 'San Juan', 
                    totalSessions: 520,
                    totalActiveDays: 210,
                    averageSessionDuration: 42,
                    engagementScore:  82,
                    sessionDuration: [
                        { day: 'monday', duration: 42 },
                        { day: 'tuesday', duration: 43 },
                        { day: 'wednesday', duration: 41 },
                        { day: 'thursday', duration: 44 },
                        { day: 'friday', duration: 40 },
                        { day: 'saturday', duration: 38 },
                        { day: 'sunday', duration: 10 }
                    ],
                    sessionsPerDay: [
                        { day: 'monday', sessions: 70 },
                        { day: 'tuesday', sessions: 65 },
                        { day: 'wednesday', sessions: 68 },
                        { day: 'thursday', sessions: 72 },
                        { day: 'friday', sessions: 66 },
                        { day: 'saturday', sessions: 40 },
                        { day: 'sunday', sessions: 9 }
                    ],
                    devicesUsage: [{ desktop: 53, tablet: 33, mobile: 14 }],
                    OffVsInSchoolUsage: [{ inSchool: 74, offSchool: 26 }]
                }
            ],
            schoolName: 'Escuela Intermedia Central',
            totalSessions: 300,
            totalActiveDays: 120,
            averageSessionDuration: 45,
            engagementScore: 85,
            sessionDuration: [
                { day: 'monday', duration: 40 },
                { day: 'tuesday', duration: 42 },
                { day: 'wednesday', duration: 44 },
                { day: 'thursday', duration: 46 },
                { day: 'friday', duration: 43 },
                { day: 'saturday', duration: 41 },
                { day: 'sunday', duration: 0 }
            ],
            sessionsPerDay: [
                { day: 'monday', sessions: 50 },
                { day: 'tuesday', sessions: 45 },
                { day: 'wednesday', sessions: 48 },
                { day: 'thursday', sessions: 52 },
                { day: 'friday', sessions: 47 },
                { day: 'saturday', sessions: 35 },
                { day: 'sunday', sessions: 0 }
            ],
            devicesUsage: [{ desktop: 55, tablet: 30, mobile: 15 }],
            OffVsInSchoolUsage: [{ inSchool: 75, offSchool: 25 }]
        },
        {
            schoolName: 'Escuela Primaria de San Juan',
            totalSessions: 220,
            totalActiveDays: 100,
            averageSessionDuration: 40,
            engagementScore: 80,
            sessionDuration: [
                { day: 'monday', duration: 38 },
                { day: 'tuesday', duration: 39 },
                { day: 'wednesday', duration: 40 },
                { day: 'thursday', duration: 41 },
                { day: 'friday', duration: 39 },
                { day: 'saturday', duration: 37 },
                { day: 'sunday', duration: 0 }
            ],
            sessionsPerDay: [
                { day: 'monday', sessions: 35 },
                { day: 'tuesday', sessions: 30 },
                { day: 'wednesday', sessions: 32 },
                { day: 'thursday', sessions: 38 },
                { day: 'friday', sessions: 35 },
                { day: 'saturday', sessions: 20 },
                { day: 'sunday', sessions: 0 }
            ],
            devicesUsage: [{ desktop: 45, tablet: 40, mobile: 15 }],
            OffVsInSchoolUsage: [{ inSchool: 70, offSchool: 30 }]
        },
        {
            schoolName: 'Academia del Sur',
            totalSessions: 110,
            totalActiveDays: 60,
            averageSessionDuration: 35,
            engagementScore: 75,
            sessionDuration: [
                { day: 'monday', duration: 37 },
                { day: 'tuesday', duration: 40 },
                { day: 'wednesday', duration: 39 },
                { day: 'thursday', duration: 42 },
                { day: 'friday', duration: 38 },
                { day: 'saturday', duration: 36 },
                { day: 'sunday', duration: 0 }
            ],
            sessionsPerDay: [
                { day: 'monday', sessions: 20 },
                { day: 'tuesday', sessions: 18 },
                { day: 'wednesday', sessions: 15 },
                { day: 'thursday', sessions: 22 },
                { day: 'friday', sessions: 20 },
                { day: 'saturday', sessions: 15 },
                { day: 'sunday', sessions: 0 }
            ],
            devicesUsage: [{ desktop: 50, tablet: 30, mobile: 20 }],
            OffVsInSchoolUsage: [{ inSchool: 75, offSchool: 25 }]
        },
        {
            schoolName: 'Bayamón High School',
            totalSessions: 235,
            totalActiveDays: 90,
            averageSessionDuration: 38,
            engagementScore: 78,
            sessionDuration: [
                { day: 'monday', duration: 39 },
                { day: 'tuesday', duration: 38 },
                { day: 'wednesday', duration: 41 },
                { day: 'thursday', duration: 40 },
                { day: 'friday', duration: 37 },
                { day: 'saturday', duration: 35 },
                { day: 'sunday', duration: 0 }
            ],
            sessionsPerDay: [
                { day: 'monday', sessions: 40 },
                { day: 'tuesday', sessions: 35 },
                { day: 'wednesday', sessions: 38 },
                { day: 'thursday', sessions: 42 },
                { day: 'friday', sessions: 37 },
                { day: 'saturday', sessions: 23 },
                { day: 'sunday', sessions: 0 }
            ],
            devicesUsage: [{ desktop: 48, tablet: 32, mobile: 20 }],
            OffVsInSchoolUsage: [{ inSchool: 68, offSchool: 32 }]
        },
        {
            schoolName: 'Escuela Secundaria de Arecibo',
            totalSessions: 278,
            totalActiveDays: 85,
            averageSessionDuration: 36,
            engagementScore: 76,
            sessionDuration: [
                { day: 'monday', duration: 40 },
                { day: 'tuesday', duration: 41 },
                { day: 'wednesday', duration: 38 },
                { day: 'thursday', duration: 39 },
                { day: 'friday', duration: 42 },
                { day: 'saturday', duration: 37 },
                { day: 'sunday', duration: 15 }
            ],
            sessionsPerDay: [
                { day: 'monday', sessions: 45 },
                { day: 'tuesday', sessions: 40 },
                { day: 'wednesday', sessions: 35 },
                { day: 'thursday', sessions: 38 },
                { day: 'friday', sessions: 40 },
                { day: 'saturday', sessions: 25 },
                { day: 'sunday', sessions: 5 }
            ],
            devicesUsage: [{ desktop: 50, tablet: 30, mobile: 20 }],
            OffVsInSchoolUsage: [{ inSchool: 72, offSchool: 28 }]
        },
        {
            schoolName: 'Todas las escuelas',
            totalSessions: 1143,
            totalActiveDays: 520,
            averageSessionDuration: 40,
            engagementScore: 80,
            sessionDuration: [
                { day: 'monday', duration: 39 },
                { day: 'tuesday', duration: 40 },
                { day: 'wednesday', duration: 39 },
                { day: 'thursday', duration: 41 },
                { day: 'friday', duration: 40 },
                { day: 'saturday', duration: 37 },
                { day: 'sunday', duration: 15 }
            ],
            sessionsPerDay: [
                { day: 'monday', sessions: 190 },
                { day: 'tuesday', sessions: 170 },
                { day: 'wednesday', sessions: 168 },
                { day: 'thursday', sessions: 190 },
                { day: 'friday', sessions: 182 },
                { day: 'saturday', sessions: 138 },
                { day: 'sunday', sessions: 5 }
            ],
            devicesUsage: [{ desktop: 50, tablet: 32, mobile: 18 }],
            OffVsInSchoolUsage: [{ inSchool: 71, offSchool: 29 }]
        },
    ],
    adminOre: [
        {
            schoolName: 'Escuela Intermedia Central',
            totalSessions: 300,
            totalActiveDays: 120,
            averageSessionDuration: 45,
            engagementScore: 85,
            sessionDuration: [
                { day: 'monday', duration: 40 },
                { day: 'tuesday', duration: 42 },
                { day: 'wednesday', duration: 44 },
                { day: 'thursday', duration: 46 },
                { day: 'friday', duration: 43 },
                { day: 'saturday', duration: 41 },
                { day: 'sunday', duration: 0 }
            ],
            sessionsPerDay: [
                { day: 'monday', sessions: 50 },
                { day: 'tuesday', sessions: 45 },
                { day: 'wednesday', sessions: 48 },
                { day: 'thursday', sessions: 52 },
                { day: 'friday', sessions: 47 },
                { day: 'saturday', sessions: 35 },
                { day: 'sunday', sessions: 0 }
            ],
            devicesUsage: [{ desktop: 55, tablet: 30, mobile: 15 }],
            OffVsInSchoolUsage: [{ inSchool: 75, offSchool: 25 }]
        },
        {
            schoolName: 'Escuela Primaria de San Juan',
            totalSessions: 220,
            totalActiveDays: 100,
            averageSessionDuration: 40,
            engagementScore: 80,
            sessionDuration: [
                { day: 'monday', duration: 38 },
                { day: 'tuesday', duration: 39 },
                { day: 'wednesday', duration: 40 },
                { day: 'thursday', duration: 41 },
                { day: 'friday', duration: 39 },
                { day: 'saturday', duration: 37 },
                { day: 'sunday', duration: 0 }
            ],
            sessionsPerDay: [
                { day: 'monday', sessions: 35 },
                { day: 'tuesday', sessions: 30 },
                { day: 'wednesday', sessions: 32 },
                { day: 'thursday', sessions: 38 },
                { day: 'friday', sessions: 35 },
                { day: 'saturday', sessions: 20 },
                { day: 'sunday', sessions: 0 }
            ],
            devicesUsage: [{ desktop: 45, tablet: 40, mobile: 15 }],
            OffVsInSchoolUsage: [{ inSchool: 70, offSchool: 30 }]
        },
        {
            schoolName: 'Todas las escuelas',
            totalSessions: 520,
            totalActiveDays: 220,
            averageSessionDuration: 42,
            engagementScore: 82,
            sessionDuration: [
                { day: 'monday', duration: 39 },
                { day: 'tuesday', duration: 40 },
                { day: 'wednesday', duration: 39 },
                { day: 'thursday', duration: 41 },
                { day: 'friday', duration: 40 },
                { day: 'saturday', duration: 37 },
                { day: 'sunday', duration: 0 }
            ],
            sessionsPerDay: [
                { day: 'monday', sessions: 85 },
                { day: 'tuesday', sessions: 75 },
                { day: 'wednesday', sessions: 80 },
                { day: 'thursday', sessions: 90 },
                { day: 'friday', sessions: 82 },
                { day: 'saturday', sessions: 55 },
                { day: 'sunday', sessions: 0 }
            ],
            devicesUsage: [{ desktop: 50, tablet: 35, mobile: 15 }],
            OffVsInSchoolUsage: [{ inSchool: 72, offSchool: 28 }]
        },
    ],
    director: [
        {
            groupName: 'Grupo A - 3.er grado',
            totalSessions: 80,
            totalActiveDays: 30,
            averageSessionDuration: 26,
            engagementScore: 85,
            sessionDuration: [
                { day: 'monday', duration: 26 },
                { day: 'tuesday', duration: 28 },
                { day: 'wednesday', duration: 26 },
                { day: 'thursday', duration: 30 },
                { day: 'friday', duration: 27 },
                { day: 'saturday', duration: 26 },
                { day: 'sunday', duration: 0 },
            ],
            sessionsPerDay: [
                { day: 'monday', sessions: 13 },
                { day: 'tuesday', sessions: 15 },
                { day: 'wednesday', sessions: 14 },
                { day: 'thursday', sessions: 16 },
                { day: 'friday', sessions: 12 },
                { day: 'saturday', sessions: 10 },
                { day: 'sunday', sessions: 0 },
            ],
            devicesUsage: [{ desktop: 50, tablet: 35, mobile: 15 }],
            OffVsInSchoolUsage: [{ inSchool: 73, offSchool: 27 }]
        },
        {
            groupName: 'Grupo B - 4.o grado',
            totalSessions: 34,
            totalActiveDays: 13,
            averageSessionDuration: 24,
            engagementScore:  67,
            sessionDuration: [
                { day: 'monday', duration: 24 },
                { day: 'tuesday', duration: 26 },
                { day: 'wednesday', duration: 24 },
                { day: 'thursday', duration: 27 },
                { day: 'friday', duration: 23 },
                { day: 'saturday', duration: 24 },
                { day: 'sunday', duration: 0 },
            ],
            sessionsPerDay: [
                { day: 'monday', sessions: 5 },
                { day: 'tuesday', sessions: 6 },
                { day: 'wednesday', sessions: 7 },
                { day: 'thursday', sessions: 7 },
                { day: 'friday', sessions: 6 },
                { day: 'saturday', sessions: 3 },
                { day: 'sunday', sessions: 0 },
            ],
            devicesUsage: [{ desktop: 55, tablet: 25, mobile: 20 }],
            OffVsInSchoolUsage: [{ inSchool: 68, offSchool: 32 }],
        },
        {
            groupName: 'Todos los grupos',
            totalSessions: 114,
            totalActiveDays: 43,
            averageSessionDuration: 25.5,
            engagementScore: 80,
            sessionDuration: [
                { day: 'monday', duration: 27 },
                { day: 'tuesday', duration: 29 },
                { day: 'wednesday', duration: 27 },
                { day: 'thursday', duration: 31 },
                { day: 'friday', duration: 28 },
                { day: 'saturday', duration: 27 },
                { day: 'sunday', duration: 0 },
            ],
            sessionsPerDay: [
                { day: 'monday', sessions: 18 },
                { day: 'tuesday', sessions: 21 },
                { day: 'wednesday', sessions: 21 },
                { day: 'thursday', sessions: 23 },
                { day: 'friday', sessions: 18 },
                { day: 'saturday', sessions: 13 },
                { day: 'sunday', sessions: 0 },
            ],
            devicesUsage: [{ desktop: 52, tablet: 30, mobile: 18 }],
            OffVsInSchoolUsage: [{ inSchool: 71, offSchool: 29 }]
        }
    ],
    parent: [{
        studentName: 'Sofía Martínez',
        totalSessions: 35,
        totalActiveDays: 12,
        averageSessionDuration: 28,
        engagementScore: 92,
        sessionDuration: [
            { day: 'monday', duration: 25 },
            { day: 'tuesday', duration: 30 },
            { day: 'wednesday', duration: 28 },
            { day: 'thursday', duration: 32 },
            { day: 'friday', duration: 27 },
            { day: 'saturday', duration: 29 },
            { day: 'sunday', duration: 0 },
        ],
        sessionsPerDay: [
            { day: 'monday', sessions: 5 },
            { day: 'tuesday', sessions: 6 },
            { day: 'wednesday', sessions: 4 },
            { day: 'thursday', sessions: 7 },
            { day: 'friday', sessions: 5 },
            { day: 'saturday', sessions: 3 },
            { day: 'sunday', sessions: 0 },
        ],
        devicesUsage: [{ desktop: 55, tablet: 35, mobile: 10 }],
        OffVsInSchoolUsage: [{ inSchool: 70, offSchool: 30 }],
    }]
};

export const activitiesAndSuggestions = [
    { id: 1, title: 'Letras mayúsculas y minúsculas (I)', standardCode: '1.L.2.1b', pdfUrl: '/pdfs/leccion-1.pdf', lessonId: '663', assignedStudents: "Alejandro Vargas" },
    { id: 2, title: 'Letras mayúsculas y minúsculas (II)', standardCode: '1.L.2.1a', pdfUrl: '/pdfs/leccion-2.pdf', lessonId: '202496', assignedStudents: "Grupo Seleccionado"  },
    { id: 3, title: 'Narraciones simples (formato de carta)', standardCode: '1.L.2.1a', pdfUrl: '/pdfs/leccion-3.pdf', lessonId: '666', assignedStudents: "Sofía Martínez"  },
    { id: 4, title: 'Uso del género masculino y femenino', standardCode: '1.L.1.1c', pdfUrl: '/pdfs/leccion-4.pdf', lessonId: '669', assignedStudents: "Alejandro Vargas"  },
    { id: 5, title: 'Tiempos verbales', standardCode: '1.L.1.1f', pdfUrl: '/pdfs/leccion-5.pdf', lessonId: '204766', assignedStudents: "Grupo Seleccionado"  },
    { id: 6, title: 'Conversaciones eficaces', standardCode: 'K.L.1.1a', pdfUrl: '/pdfs/leccion-6.pdf', lessonId: '202184', assignedStudents: "Sofía Martínez"  },
];

export const readingLessons = [
    { id: 1, title: 'Fonética, reconocimiento de palabras y fluidez (I)', standardCode: 'K.L.2.1q', pdfUrl: '/pdfs/lectura-1.pdf', lessonId: '204913'  },
    { id: 2, title: 'Fonética, reconocimiento de palabras y fluidez (II)', standardCode: '1.L.2.1f', pdfUrl: '/pdfs/lectura-2.pdf', lessonId: '204914'  },
    { id: 3, title: 'Fonética, reconocimiento de palabras y fluidez (III)', standardCode: 'K.L.2.1m', pdfUrl: '/pdfs/lectura-3.pdf' },
    { id: 4, title: 'Fonética, reconocimiento de palabras y fluidez (IV)', standardCode: 'K.L.2.1i', pdfUrl: '/pdfs/lectura-4.pdf' },
    { id: 5, title: 'Describe Ideas: Predict, Infer, and Draw Conclusions (Grade 4)', standardCode: '4.R.1.1', pdfUrl: '/pdfs/lectura-5.pdf', lessonId: '204944'  },
    { id: 6, title: 'Describe Ideas: Predict, Infer, and Draw Conclusions (Grade 5)', standardCode: '5.R.1.1', pdfUrl: '/pdfs/lectura-6.pdf' },
    { id: 7, title: 'Point of View (Grade 5)', standardCode: '5.R.8.1', pdfUrl: '/pdfs/lectura-7.pdf', lessonId: '116'  },
    { id: 8, title: 'Point of View (Grade 4)', standardCode: '4.R.6.1L', pdfUrl: '/pdfs/lectura-8.pdf', lessonId: '204949'  },
    { id: 9, title: 'Support Main Ideas with Facts and Details (Grade 9)', standardCode: '9.R.1.1c', pdfUrl: '/pdfs/lectura-9.pdf', lessonId: '204947'  },
    { id: 10, title: 'Support Main Ideas with Facts and Details (Grade 10)', standardCode: '10.R.2.1L', pdfUrl: '/pdfs/lectura-10.pdf' },
    { id: 11, title: 'Support Main Ideas with Facts and Details (Grade 11)', standardCode: '11.R.1.1', pdfUrl: '/pdfs/lectura-11.pdf' },
    { id: 12, title: 'Support Main Ideas with Facts and Details (Grade 12)', standardCode: '12.R.2.1L', pdfUrl: '/pdfs/lectura-12.pdf' },
    { id: 13, title: 'Determine Theme and Summarize (Grade 7)', standardCode: '7.R.10.1', pdfUrl: '/pdfs/lectura-13.pdf', lessonId: '204948' }
];

export const baseDiagnostics = [
    { month: 'Agosto', score: 75, status: 'completed' },
    { month: 'Diciembre', score: 60, status: 'completed' },
    { month: 'Enero', score: 65, status: 'completed' },
];

export const diagnosticsMockedData = {
    teacher: [
        {
            studentName: 'Todos los estudiantes',
            diagnostics: baseDiagnostics,
        },
        {
            studentName: 'Sofía Martínez',
            diagnostics: [
                { month: 'Agosto', score: 65, status: 'completed' },
                { month: 'Diciembre', score: 85, status: 'completed' },
                { month: 'Enero', score: 0, status: 'pending' },
            ],
        },
        {
            studentName: 'Alejandro Vargas',
            diagnostics: [
                { month: 'Agosto', score: 40, status: 'completed' },
                { month: 'Diciembre', score: 0, status: 'pending' },
                { month: 'Enero', score: 66, status: 'completed' },
            ],
        },
        {
            studentName: 'María López',
            diagnostics: [
                { month: 'Agosto', score: 75, status: 'completed' },
                { month: 'Diciembre', score: 80, status: 'completed' },
                { month: 'Enero', score: 85, status: 'completed' },
            ],
        },
        {
            studentName: 'Juan Perez',
            diagnostics: [
                { month: 'Agosto', score: 80, status: 'completed' },
                { month: 'Diciembre', score: 85, status: 'completed' },
                { month: 'Enero', score: 90, status: 'completed' },
            ],
        },
        {
            studentName: 'Isabella Gómez',
            diagnostics: [
                { month: 'Agosto', score: 80, status: 'completed' },
                { month: 'Diciembre', score: 85, status: 'completed' },
                { month: 'Enero', score: 90, status: 'completed' },
            ],
        },
        {
            studentName: 'Grupo A - 3.er grado',
            diagnostics: [
                { month: 'Agosto', score: 77, status: 'completed' },
                { month: 'Diciembre', score: 67, status: 'completed' },
                { month: 'Enero', score: 45, status: 'completed' },
            ],
        },
        {
            studentName: 'Grupo B - 4.o grado',
            diagnostics: [
                { month: 'Agosto', score: 89, status: 'completed' },
                { month: 'Diciembre', score: 92, status: 'completed' },
                { month: 'Enero', score: 69, status: 'completed' },
            ],
        },
    ],
    director: [
        {
            groupName: 'Todos los grupos',
            diagnostics: baseDiagnostics,
        },
        {
            groupName: 'Grupo A - 3.er grado',
            diagnostics: [
                { month: 'Agosto', score: 58, status: 'completed' },
                { month: 'Diciembre', score: 72, status: 'completed' },
                { month: 'Enero', score: 88, status: 'completed' },
            ],
        },
        {
            groupName: 'Grupo B - 4.o grado',
            diagnostics: [
                { month: 'Agosto', score: 62, status: 'completed' },
                { month: 'Diciembre', score: 68, status: 'completed' },
                { month: 'Enero', score: 77, status: 'completed' },
            ],
        },
    ],
    admin: [
        {
            schoolName: 'Todas las escuelas',
            diagnostics: baseDiagnostics,
        },
        {
            schoolName: 'San Juan',
            diagnostics: [
                { month: 'Agosto', score: 60, status: 'completed' },
                { month: 'Diciembre', score: 75, status: 'completed' },
                { month: 'Enero', score: 85, status: 'completed' },
            ],
        },
        {
            schoolName: 'Humacao',
            diagnostics: [
                { month: 'Agosto', score: 50, status: 'completed' },
                { month: 'Diciembre', score: 65, status: 'completed' },
                { month: 'Enero', score: 75, status: 'completed' },
            ],
        },
        {
            schoolName: 'Arecibo',
            diagnostics: [
                { month: 'Agosto', score: 57, status: 'completed' },
                { month: 'Diciembre', score: 70, status: 'completed' },
                { month: 'Enero', score: 78, status: 'completed' },
            ],
        },
        {
            schoolName: 'Bayamón',
            diagnostics: [
                { month: 'Agosto', score: 58, status: 'completed' },
                { month: 'Diciembre', score: 72, status: 'completed' },
                { month: 'Enero', score: 85, status: 'completed' },
            ],
        },
        {
            schoolName: 'Escuela Intermedia Central',
            diagnostics: [
                { month: 'Agosto', score: 65, status: 'completed' },
                { month: 'Diciembre', score: 80, status: 'completed' },
                { month: 'Enero', score: 90, status: 'completed' },
            ],
        },
        {
            schoolName: 'Escuela Primaria de San Juan',
            diagnostics: [
                { month: 'Agosto', score: 55, status: 'completed' },
                { month: 'Diciembre', score: 70, status: 'completed' },
                { month: 'Enero', score: 80, status: 'completed' },
            ],
        },
        {
            schoolName: 'Academia del Sur',
            diagnostics: [
                { month: 'Agosto', score: 50, status: 'completed' },
                { month: 'Diciembre', score: 65, status: 'completed' },
                { month: 'Enero', score: 75, status: 'completed' },
            ],
        },
        {
            schoolName: 'Bayamón High School',
            diagnostics: [
                { month: 'Agosto', score: 58, status: 'completed' },
                { month: 'Diciembre', score: 72, status: 'completed' },
                { month: 'Enero', score: 85, status: 'completed' },
            ],
        },
        {
            schoolName: 'Escuela Secundaria de Arecibo',
            diagnostics: [
                { month: 'Agosto', score: 57, status: 'completed' },
                { month: 'Diciembre', score: 70, status: 'completed' },
                { month: 'Enero', score: 78, status: 'completed' },
            ],
        },
    ],
    adminOre: [
        {
            schoolName: 'Todas las escuelas',
            diagnostics: baseDiagnostics,
        },
        {
            schoolName: 'Escuela Intermedia Central',
            diagnostics: [
                { month: 'Agosto', score: 65, status: 'completed' },
                { month: 'Diciembre', score: 80, status: 'completed' },
                { month: 'Enero', score: 90, status: 'completed' },
            ],
        },
        {
            schoolName: 'Escuela Primaria de San Juan',
            diagnostics: [
                { month: 'Agosto', score: 55, status: 'completed' },
                { month: 'Diciembre', score: 70, status: 'completed' },
                { month: 'Enero', score: 80, status: 'completed' },
            ],
        }
    ],
    parent: [
        { 
            studentName: 'Sofía Martínez',
            diagnostics: [
                { month: 'Agosto', score: 65, status: 'completed' },
                { month: 'Diciembre', score: 85, status: 'completed' },
                { month: 'Enero', score: 0, status: 'pending' },
            ],
        }    
    ]
};

export  const mockUsageData = {
    admin: [
        { id: 1, name: 'School A', usage: 85 },
        { id: 2, name: 'School B', usage: 45 },
        { id: 3, name: 'School C', usage: 95 },
    ],
    // Mock data for Teacher (Students) view
    teacher: [
        { id: 1, name: 'Sofía Martínez', isUsing: true },
        { id: 2, name: 'Alejandro Vargas', isUsing: false },
        { id: 3, name: 'Isabella Gómez', isUsing: true },
        { id: 4, name: 'Juan Perez', isUsing: false },
        { id: 5, name: 'María López', isUsing: true },
    ],
    director: [
        { id: 1, name: 'Grupo A - 3.er grado', usage: 85 },
        { id: 2, name: 'Grupo B - 4.o grado', usage: 66 }
    ],
    adminOre: [
        { id: 1, name: 'School A', usage: 90 },
        { id: 2, name: 'School B', usage: 70 },
    ]
};

export const averageLessonResultMockedData = [
    { title: 'Fonética, reconocimiento de palabras y fluidez', code: '3.L.2.1q', average: 78, level: 'Average', group: "Grupo A - 3.er grado", subject: 'Spanish', focusArea: 'Reconocimiento fonético y conciencia fonológica' },
    { title: 'Describe Ideas: Predict, Infer, and Draw Conclusions', code: '4.R.1.1', average: 92, level: 'High', group: "Grupo A - 4.o grado", subject: 'English', focusArea: 'Fluidez' },
    { title: 'Point of View', code: '4.R.8.1', average: 65, level: 'Low', group: "Grupo A - 4.o grado", subject: 'English', focusArea: 'Comprensión lectora (conceptos impresos)' },
    { title: 'Fonética, reconocimiento de palabras y fluidez', code: '4.L.2.1f', average: 85, level: 'Average', group: "Grupo B - 4.o grado", subject: 'Spanish', focusArea: 'Reconocimiento fonético y conciencia fonológica' },
];

export const mockReadingCharts = [
    { month: "dashboards.chart-january", level_literal: 62, level_inferential: 48, level_critical: 35, level_appreciative: 30, precision: 88, fluency: 72, completeness: 90, pronunciation: 80 },
    { month: "dashboards.chart-february", level_literal: 68, level_inferential: 54, level_critical: 40, level_appreciative: 36, precision: 89, fluency: 74, completeness: 92, pronunciation: 82 },
    { month: "dashboards.chart-march", level_literal: 73, level_inferential: 60, level_critical: 46, level_appreciative: 42, precision: 90, fluency: 76, completeness: 93, pronunciation: 84 },
    { month: "dashboards.chart-april", level_literal: 77, level_inferential: 65, level_critical: 51, level_appreciative: 47, precision: 91, fluency: 78, completeness: 94, pronunciation: 85 },
    { month: "dashboards.chart-may", level_literal: 82, level_inferential: 71, level_critical: 56, level_appreciative: 53, precision: 92, fluency: 80, completeness: 95, pronunciation: 87 },
    { month: "dashboards.chart-june", level_literal: 86, level_inferential: 75, level_critical: 61, level_appreciative: 59, precision: 93, fluency: 82, completeness: 96, pronunciation: 88 }
];

export const readingChartsMockedData = {
    teacher: [
        { studentName: 'Todos los estudiantes', data: mockReadingCharts }, 
        { 
            studentName: 'Sofía Martínez', 
            data: mockReadingCharts.map(d => ({ ...d, level_literal: d.level_literal + 5, precision: d.precision + 2, pronunciation: d.pronunciation + 2 }))
        },
        { 
            studentName: 'Alejandro Vargas', 
            data: mockReadingCharts.map(d => ({ ...d, level_literal: d.level_literal - 10, precision: d.precision - 5, pronunciation: d.pronunciation - 5 }))
        },
        { 
            studentName: 'Isabella Gómez', 
            data: mockReadingCharts.map(d => ({ ...d, level_literal: d.level_literal + 10, fluency: d.fluency + 5 }))
        },
        { 
            studentName: 'María López', 
            data: mockReadingCharts.map(d => ({ ...d, level_literal: d.level_literal - 2, fluency: d.fluency + 1 }))
        },
        { 
            studentName: 'Juan Perez', 
            data: mockReadingCharts.map(d => ({ ...d, level_literal: d.level_literal + 3, fluency: d.fluency + 2 }))
        },
        {
            studentName: 'Grupo A - 3.er grado', 
            data: mockReadingCharts.map(d => ({ ...d, level_literal: d.level_literal + 7, fluency: d.fluency + 4 }))
        },
        {
            studentName: 'Grupo B - 4.o grado', 
            data: mockReadingCharts.map(d => ({ ...d, level_literal: d.level_literal - 4, fluency: d.fluency - 2 }))
        },
    ],
    director: [
        { groupName: 'Todos los grupos', data: mockReadingCharts }, 
        { 
            groupName: 'Grupo A - 3.er grado', 
            data: mockReadingCharts.map(d => ({ ...d, level_literal: d.level_literal + 3, fluency: d.fluency + 2 }))
        },
        { 
            groupName: 'Grupo B - 4.o grado', 
            data: mockReadingCharts.map(d => ({ ...d, level_literal: d.level_literal - 5, fluency: d.fluency - 3 }))
        },
    ],
    
    admin: [
        { schoolName: 'Todas las escuelas', data: mockReadingCharts },
        {
            schoolName: 'San Juan', 
            data: mockReadingCharts.map(d => ({ 
                ...d, 
                level_literal: d.level_literal + 3, 
                precision: d.precision + 1 
            })) 
        },
        { 
            schoolName: 'Humacao', 
            data: mockReadingCharts.map(d => ({ 
                ...d, 
                level_literal: d.level_literal - 5, 
                precision: d.precision - 4 
            })) 
        },
        { 
            schoolName: 'Bayamón', 
            data: mockReadingCharts.map(d => ({ 
                ...d, 
                level_literal: d.level_literal + 1, 
                precision: d.precision + 1 
            })) 
        },
        { 
            schoolName: 'Arecibo', 
            data: mockReadingCharts.map(d => ({ 
                ...d, 
                level_literal: d.level_literal + 5, 
                precision: d.precision + 2 
            })) 
        },
        { schoolName: 'Escuela Intermedia Central', data: mockReadingCharts.map(d => ({ ...d, level_literal: d.level_literal + 8, precision: d.precision + 3 })) },
        { schoolName: 'Escuela Primaria de San Juan', data: mockReadingCharts.map(d => ({ ...d, level_literal: d.level_literal - 2, precision: d.precision - 1 })) },
        { schoolName: 'Academia del Sur', data: mockReadingCharts.map(d => ({ ...d, level_literal: d.level_literal - 5, precision: d.precision - 4 })) },
        { schoolName: 'Bayamón High School', data: mockReadingCharts.map(d => ({ ...d, level_literal: d.level_literal + 1, precision: d.precision + 1 })) },
        { schoolName: 'Escuela Secundaria de Arecibo', data: mockReadingCharts.map(d => ({ ...d, level_literal: d.level_literal + 5, precision: d.precision + 2 })) },
    ],
    
    adminOre: [
        { schoolName: 'Todas las escuelas', data: mockReadingCharts },
        { schoolName: 'Escuela Intermedia Central', data: mockReadingCharts.map(d => ({ ...d, level_literal: d.level_literal + 8, precision: d.precision + 3 })) },
        { schoolName: 'Escuela Primaria de San Juan', data: mockReadingCharts.map(d => ({ ...d, level_literal: d.level_literal - 2, precision: d.precision - 1 })) },
    ],
    
    parent: [
        { studentName: 'Sofía Martínez', data: mockReadingCharts.map(d => ({ ...d, level_literal: d.level_literal + 5, precision: d.precision + 2, pronunciation: d.pronunciation + 2 })) },
    ]
}

export const baseSkillsProgress = [
    { skill: "dashboards.progress-and-mastery.fluency", value: 80 },
    { skill: "dashboards.progress-and-mastery.vocabulary", value: 65 },
    { skill: "dashboards.progress-and-mastery.comprehension", value: 47 },
];

export const skillsProgressMockedData = {
    teacher: [
        { 
            studentName: 'Todos los estudiantes', 
            data: baseSkillsProgress 
        },
        { 
            studentName: 'Sofía Martínez', 
            data: [
                { skill: "dashboards.progress-and-mastery.fluency", value: 95 },
                { skill: "dashboards.progress-and-mastery.vocabulary", value: 75 },
                { skill: "dashboards.progress-and-mastery.comprehension", value: 67 },
            ]
        },
        { 
            studentName: 'Alejandro Vargas', 
            data: [
                { skill: "dashboards.progress-and-mastery.fluency", value: 65 },
                { skill: "dashboards.progress-and-mastery.vocabulary", value: 55 },
                { skill: "dashboards.progress-and-mastery.comprehension", value: 37 },
            ]
        },
        { 
            studentName: 'Isabella Gómez', 
            data: [
                { skill: "dashboards.progress-and-mastery.fluency", value: 85 },
                { skill: "dashboards.progress-and-mastery.vocabulary", value: 65 },
                { skill: "dashboards.progress-and-mastery.comprehension", value: 57 },
            ]
        },
        { 
            studentName: 'María López', 
            data: [
                { skill: "dashboards.progress-and-mastery.fluency", value: 75 },
                { skill: "dashboards.progress-and-mastery.vocabulary", value: 70 },
                { skill: "dashboards.progress-and-mastery.comprehension", value: 47 },
            ]
        },
        { 
            studentName: 'Grupo A - 3.er grado', 
            data: [
                { skill: "dashboards.progress-and-mastery.fluency", value: 85 },
                { skill: "dashboards.progress-and-mastery.vocabulary", value: 70 },
                { skill: "dashboards.progress-and-mastery.comprehension", value: 55 },
            ]
        },
        { 
            studentName: 'Grupo B - 4.o grado', 
            data: [
                { skill: "dashboards.progress-and-mastery.fluency", value: 75 },
                { skill: "dashboards.progress-and-mastery.vocabulary", value: 60 },
                { skill: "dashboards.progress-and-mastery.comprehension", value: 42 },
            ] 
        },
    ],
    director: [
        { 
            groupName: 'Todos los grupos', 
            data: baseSkillsProgress 
        }, 
        { 
            groupName: 'Grupo A - 3.er grado', 
            data: [
                { skill: "dashboards.progress-and-mastery.fluency", value: 85 },
                { skill: "dashboards.progress-and-mastery.vocabulary", value: 70 },
                { skill: "dashboards.progress-and-mastery.comprehension", value: 55 },
            ] 
        },
        { 
            groupName: 'Grupo B - 4.o grado', 
            data: [
                { skill: "dashboards.progress-and-mastery.fluency", value: 75 },
                { skill: "dashboards.progress-and-mastery.vocabulary", value: 60 },
                { skill: "dashboards.progress-and-mastery.comprehension", value: 42 },
            ] 
        },
    ],
    admin: [
        { 
            schoolName: 'Todas las escuelas', 
            data: baseSkillsProgress 
        },
        {
            schoolName: 'San Juan', 
            data: [ 
                { skill: "dashboards.progress-and-mastery.fluency", value: 84 },
                { skill: "dashboards.progress-and-mastery.vocabulary", value: 72 },
                { skill: "dashboards.progress-and-mastery.comprehension", value: 52 },
            ] 
        },
        {
            schoolName: 'Humacao', 
            data: [
                { skill: "dashboards.progress-and-mastery.fluency", value: 70 },
                { skill: "dashboards.progress-and-mastery.vocabulary", value: 55 },
                { skill: "dashboards.progress-and-mastery.comprehension", value: 37 },
            ] 
        },
        {
            schoolName: 'Bayamón', 
            data: [ 
                { skill: "dashboards.progress-and-mastery.fluency", value: 85 },
                { skill: "dashboards.progress-and-mastery.vocabulary", value: 70 },
                { skill: "dashboards.progress-and-mastery.comprehension", value: 52 },
            ] 
        },
        {
            schoolName: 'Arecibo', 
            data: [ 
                { skill: "dashboards.progress-and-mastery.fluency", value: 88 },
                { skill: "dashboards.progress-and-mastery.vocabulary", value: 73 },
                { skill: "dashboards.progress-and-mastery.comprehension", value: 55 },
            ] 
        },
        { 
            schoolName: 'Escuela Intermedia Central', 
            data: [
                { skill: "dashboards.progress-and-mastery.fluency", value: 90 },
                { skill: "dashboards.progress-and-mastery.vocabulary", value: 80 },
                { skill: "dashboards.progress-and-mastery.comprehension", value: 57 },
            ]
        },
        { 
            schoolName: 'Escuela Primaria de San Juan', 
            data: [
                { skill: "dashboards.progress-and-mastery.fluency", value: 77 },
                { skill: "dashboards.progress-and-mastery.vocabulary", value: 63 },
                { skill: "dashboards.progress-and-mastery.comprehension", value: 46 },
            ] 
        },
        { 
            schoolName: 'Academia del Sur', 
            data: [
                { skill: "dashboards.progress-and-mastery.fluency", value: 70 },
                { skill: "dashboards.progress-and-mastery.vocabulary", value: 55 },
                { skill: "dashboards.progress-and-mastery.comprehension", value: 37 },
            ] 
        },
        { 
            schoolName: 'Bayamón High School', 
            data: [
                { skill: "dashboards.progress-and-mastery.fluency", value: 85 },
                { skill: "dashboards.progress-and-mastery.vocabulary", value: 70 },
                { skill: "dashboards.progress-and-mastery.comprehension", value: 52 },
            ] 
        },
        { 
            schoolName: 'Escuela Secundaria de Arecibo', 
            data: [
                { skill: "dashboards.progress-and-mastery.fluency", value: 88 },
                { skill: "dashboards.progress-and-mastery.vocabulary", value: 73 },
                { skill: "dashboards.progress-and-mastery.comprehension", value: 55 },
            ] 
        },
    ],
    adminOre: [
        { 
            schoolName: 'Todas las escuelas', 
            data: baseSkillsProgress 
        },
        { 
            schoolName: 'Escuela Intermedia Central', 
            data: [
                { skill: "dashboards.progress-and-mastery.fluency", value: 90 },
                { skill: "dashboards.progress-and-mastery.vocabulary", value: 80 },
                { skill: "dashboards.progress-and-mastery.comprehension", value: 57 },
            ] 
        },
        { 
            schoolName: 'Escuela Primaria de San Juan', 
            data: [
                { skill: "dashboards.progress-and-mastery.fluency", value: 77 },
                { skill: "dashboards.progress-and-mastery.vocabulary", value: 63 },
                { skill: "dashboards.progress-and-mastery.comprehension", value: 46 },
            ] 
        },
    ],
    parent: [
        { 
            studentName: 'Sofía Martínez', 
            data: [
                { skill: "dashboards.progress-and-mastery.fluency", value: 95 },
                { skill: "dashboards.progress-and-mastery.vocabulary", value: 75 },
                { skill: "dashboards.progress-and-mastery.comprehension", value: 67 },
            ] 
        },
    ]
}

export const baseUnitsCompleted = [
    { unit: "Unidad 1", percentage: 100 },
    { unit: "Unidad 2", percentage: 70 },
    { unit: "Unidad 3", percentage: 75 },
    { unit: "Unidad 4", percentage: 90 },
];

export const unitsCompletedMockedData = {
    teacher: [
        { 
            studentName: 'Todos los estudiantes', 
            data: baseUnitsCompleted 
        }, 
        { 
            studentName: 'Sofía Martínez', 
            data: [
                { unit: "Unidad 1", percentage: 100 },
                { unit: "Unidad 2", percentage: 95 },
                { unit: "Unidad 3", percentage: 80 },
                { unit: "Unidad 4", percentage: 95 },
            ]
        },
        { 
            studentName: 'Alejandro Vargas', 
            data: [
                { unit: "Unidad 1", percentage: 80 },
                { unit: "Unidad 2", percentage: 40 },
                { unit: "Unidad 3", percentage: 50 },
                { unit: "Unidad 4", percentage: 30 },
            ]
        },
        {
            studentName: 'Isabella Gómez',
            data: [
                { unit: "Unidad 1", percentage: 90 },
                { unit: "Unidad 2", percentage: 75 },
                { unit: "Unidad 3", percentage: 70 },
                { unit: "Unidad 4", percentage: 85 },
            ]
        },
        {
            studentName: 'María López',
            data: [
                { unit: "Unidad 1", percentage: 100 },
                { unit: "Unidad 2", percentage: 80 },
                { unit: "Unidad 3", percentage: 70 },
                { unit: "Unidad 4", percentage: 85 },
            ]
        },
        {
            studentName: 'Juan Perez',
            data: [
                { unit: "Unidad 1", percentage: 60 },
                { unit: "Unidad 2", percentage: 50 },
                { unit: "Unidad 3", percentage: 40 },
                { unit: "Unidad 4", percentage: 30 },
            ]
        },
        { 
            studentName: 'Grupo A - 3.er grado', 
            data: [
                { unit: "Unidad 1", percentage: 100 },
                { unit: "Unidad 2", percentage: 80 },
                { unit: "Unidad 3", percentage: 70 },
                { unit: "Unidad 4", percentage: 85 },
            ] 
        },
        { 
            studentName: 'Grupo B - 4.o grado', 
            data: [
                { unit: "Unidad 1", percentage: 90 },
                { unit: "Unidad 2", percentage: 60 },
                { unit: "Unidad 3", percentage: 55 },
                { unit: "Unidad 4", percentage: 70 },
            ]
        },
    ],
    director: [
        { 
            groupName: 'Todos los grupos', 
            data: baseUnitsCompleted 
        }, 
        { 
            groupName: 'Grupo A - 3.er grado', 
            data: [
                { unit: "Unidad 1", percentage: 100 },
                { unit: "Unidad 2", percentage: 80 },
                { unit: "Unidad 3", percentage: 70 },
                { unit: "Unidad 4", percentage: 85 },
            ] 
        },
        { 
            groupName: 'Grupo B - 4.o grado', 
            data: [
                { unit: "Unidad 1", percentage: 90 },
                { unit: "Unidad 2", percentage: 60 },
                { unit: "Unidad 3", percentage: 55 },
                { unit: "Unidad 4", percentage: 70 },
            ] 
        },
    ],
    admin: [
        { 
            schoolName: 'Todas las escuelas', 
            data: baseUnitsCompleted 
        },
        {
            schoolName: 'San Juan', 
            data: [ 
                { unit: "Unidad 1", percentage: 98 },
                { unit: "Unidad 2", percentage: 75 },
                { unit: "Unidad 3", percentage: 75 },
                { unit: "Unidad 4", percentage: 88 },
            ] 
        },
        {
            schoolName: 'Humacao', 
            data: [
                { unit: "Unidad 1", percentage: 85 },
                { unit: "Unidad 2", percentage: 55 },
                { unit: "Unidad 3", percentage: 60 },
                { unit: "Unidad 4", percentage: 70 },
            ] 
        },
        {
            schoolName: 'Bayamón', 
            data: [ 
                { unit: "Unidad 1", percentage: 90 },
                { unit: "Unidad 2", percentage: 70 },
                { unit: "Unidad 3", percentage: 65 },
                { unit: "Unidad 4", percentage: 80 },
            ] 
        },
        {
            schoolName: 'Arecibo', 
            data: [ 
                { unit: "Unidad 1", percentage: 100 },
                { unit: "Unidad 2", percentage: 100 },
                { unit: "Unidad 3", percentage: 55 },
                { unit: "Unidad 4", percentage: 78 },
            ] 
        },
        { 
            schoolName: 'Escuela Intermedia Central', 
            data: [
                { unit: "Unidad 1", percentage: 100 },
                { unit: "Unidad 2", percentage: 85 },
                { unit: "Unidad 3", percentage: 80 },
                { unit: "Unidad 4", percentage: 95 },
            ]
        },
        { 
            schoolName: 'Escuela Primaria de San Juan', 
            data: [
                { unit: "Unidad 1", percentage: 95 },
                { unit: "Unidad 2", percentage: 65 },
                { unit: "Unidad 3", percentage: 70 },
                { unit: "Unidad 4", percentage: 80 },
            ] 
        },
        {
            schoolName: 'Academia del Sur',
            data: [
                { unit: "Unidad 1", percentage: 85 },
                { unit: "Unidad 2", percentage: 55 },
                { unit: "Unidad 3", percentage: 60 },
                { unit: "Unidad 4", percentage: 70 },
            ]
        },
        {   
            schoolName: 'Bayamón High School',
            data: [
                { unit: "Unidad 1", percentage: 90 },
                { unit: "Unidad 2", percentage: 70 },
                { unit: "Unidad 3", percentage: 65 },
                { unit: "Unidad 4", percentage: 80 },
            ]
        },
        {
            schoolName: 'Escuela Secundaria de Arecibo',
            data: [
                { unit: "Unidad 1", percentage: 100 },
                { unit: "Unidad 2", percentage: 100 },
                { unit: "Unidad 3", percentage: 55 },
                { unit: "Unidad 4", percentage: 78 },
            ]
        }
    ],
    adminOre: [
        { 
            schoolName: 'Todas las escuelas', 
            data: baseUnitsCompleted 
        },
        { 
            schoolName: 'Escuela Intermedia Central', 
            data: [
                { unit: "Unidad 1", percentage: 100 },
                { unit: "Unidad 2", percentage: 85 },
                { unit: "Unidad 3", percentage: 80 },
                { unit: "Unidad 4", percentage: 95 },
            ] 
        },
        { 
            schoolName: 'Escuela Primaria de San Juan', 
            data: [
                { unit: "Unidad 1", percentage: 95 },
                { unit: "Unidad 2", percentage: 65 },
                { unit: "Unidad 3", percentage: 70 },
                { unit: "Unidad 4", percentage: 80 },
            ] 
        },
    ],
    parent: [
        { 
            studentName: 'Sofía Martínez', 
            data: [
                { unit: "Unidad 1", percentage: 100 },
                { unit: "Unidad 2", percentage: 95 },
                { unit: "Unidad 3", percentage: 80 },
                { unit: "Unidad 4", percentage: 95 },
            ]
        },
    ]
}

export const baseAverageTimeReading = [
    { month: "dashboards.chart-january", level: 50 },
    { month: "dashboards.chart-february", level: 40 },
    { month: "dashboards.chart-march", level: 60 },
    { month: "dashboards.chart-april", level: 70 },
    { month: "dashboards.chart-may", level: 33 }
];

export const averageTimeReadingMockedData = {
    teacher: [
        { 
            studentName: 'Todos los estudiantes', 
            data: baseAverageTimeReading 
        }, 
        { 
            studentName: 'Sofía Martínez', 
            data: [
                { month: "dashboards.chart-january", level: 60 },
                { month: "dashboards.chart-february", level: 55 },
                { month: "dashboards.chart-march", level: 75 },
                { month: "dashboards.chart-april", level: 80 },
                { month: "dashboards.chart-may", level: 40 }
            ]
        },
        { 
            studentName: 'Alejandro Vargas', 
            data: [
                { month: "dashboards.chart-january", level: 30 },
                { month: "dashboards.chart-february", level: 25 },
                { month: "dashboards.chart-march", level: 40 },
                { month: "dashboards.chart-april", level: 45 },
                { month: "dashboards.chart-may", level: 15 }
            ]
        },
        { 
            studentName: 'Isabella Gómez', 
            data: [
                { month: "dashboards.chart-january", level: 55 },
                { month: "dashboards.chart-february", level: 45 },
                { month: "dashboards.chart-march", level: 65 },
                { month: "dashboards.chart-april", level: 75 },
                { month: "dashboards.chart-may", level: 35 }
            ]
        },
        { 
            studentName: 'María López',
            data: [
                { month: "dashboards.chart-january", level: 50 },
                { month: "dashboards.chart-february", level: 40 },
                { month: "dashboards.chart-march", level: 60 },
                { month: "dashboards.chart-april", level: 70 },
                { month: "dashboards.chart-may", level: 33 }
            ]
        },
        { 
            studentName: 'Juan Perez', 
            data: [
                { month: "dashboards.chart-january", level: 25 },
                { month: "dashboards.chart-february", level: 20 },
                { month: "dashboards.chart-march", level: 30 },
                { month: "dashboards.chart-april", level: 35 },
                { month: "dashboards.chart-may", level: 10 }
            ]
        },
        {
            studentName: 'Grupo B - 4.o grado',
            data: [
                { month: "dashboards.chart-january", level: 45 },
                { month: "dashboards.chart-february", level: 35 },
                { month: "dashboards.chart-march", level: 55 }, 
                { month: "dashboards.chart-april", level: 65 },
                { month: "dashboards.chart-may", level: 30 }
            ]
        },
        { 
            studentName: 'Grupo A - 3.er grado', 
            data: [
                { month: "dashboards.chart-january", level: 55 },
                { month: "dashboards.chart-february", level: 45 },
                { month: "dashboards.chart-march", level: 65 },
                { month: "dashboards.chart-april", level: 75 },
                { month: "dashboards.chart-may", level: 38 }
            ] 
        },
    ],
    director: [
        { 
            groupName: 'Todos los grupos', 
            data: baseAverageTimeReading 
        }, 
        { 
            groupName: 'Grupo A - 3.er grado', 
            data: [ 
                { month: "dashboards.chart-january", level: 55 },
                { month: "dashboards.chart-february", level: 45 },
                { month: "dashboards.chart-march", level: 65 },
                { month: "dashboards.chart-april", level: 75 },
                { month: "dashboards.chart-may", level: 38 }
            ] 
        },
        { 
            groupName: 'Grupo B - 4.o grado', 
            data: [ 
                { month: "dashboards.chart-january", level: 45 },
                { month: "dashboards.chart-february", level: 35 },
                { month: "dashboards.chart-march", level: 55 },
                { month: "dashboards.chart-april", level: 65 },
                { month: "dashboards.chart-may", level: 30 }
            ] 
        },
    ],
    admin: [
        { 
            schoolName: 'Todas las escuelas', 
            data: baseAverageTimeReading 
        },
        {
            schoolName: 'San Juan', 
            data: [ 
                { month: "dashboards.chart-january", level: 55 },
                { month: "dashboards.chart-february", level: 45 },
                { month: "dashboards.chart-march", level: 65 },
                { month: "dashboards.chart-april", level: 75 },
                { month: "dashboards.chart-may", level: 39 },
            ] 
        },
        {
            schoolName: 'Humacao', 
            data: [
                { month: "dashboards.chart-january", level: 40 },
                { month: "dashboards.chart-february", level: 30 },
                { month: "dashboards.chart-march", level: 50 },
                { month: "dashboards.chart-april", level: 60 },
                { month: "dashboards.chart-may", level: 25 },
            ] 
        },
        {
            schoolName: 'Bayamón', 
            data: [ 
                { month: "dashboards.chart-january", level: 52 },
                { month: "dashboards.chart-february", level: 42 },
                { month: "dashboards.chart-march", level: 62 },
                { month: "dashboards.chart-april", level: 72 },
                { month: "dashboards.chart-may", level: 35 },
            ] 
        },
        {
            schoolName: 'Arecibo', 
            data: [ 
                { month: "dashboards.chart-january", level: 55 },
                { month: "dashboards.chart-february", level: 45 },
                { month: "dashboards.chart-march", level: 65 },
                { month: "dashboards.chart-april", level: 75 },
                { month: "dashboards.chart-may", level: 38 },
            ] 
        },
        { 
            schoolName: 'Escuela Intermedia Central', 
            data: [ 
                { month: "dashboards.chart-january", level: 60 },
                { month: "dashboards.chart-february", level: 50 },
                { month: "dashboards.chart-march", level: 70 },
                { month: "dashboards.chart-april", level: 80 },
                { month: "dashboards.chart-may", level: 45 }
            ]
        },
        { 
            schoolName: 'Escuela Primaria de San Juan', 
            data: [ 
                { month: "dashboards.chart-january", level: 50 },
                { month: "dashboards.chart-february", level: 40 },
                { month: "dashboards.chart-march", level: 60 },
                { month: "dashboards.chart-april", level: 70 },
                { month: "dashboards.chart-may", level: 33 }
            ] 
        },
        { 
            schoolName: 'Academia del Sur', 
            data: [ 
                { month: "dashboards.chart-january", level: 40 },
                { month: "dashboards.chart-february", level: 30 },
                { month: "dashboards.chart-march", level: 50 },
                { month: "dashboards.chart-april", level: 60 },
                { month: "dashboards.chart-may", level: 25 }
            ] 
        },
        { 
            schoolName: 'Bayamón High School', 
            data: [ 
                { month: "dashboards.chart-january", level: 52 },
                { month: "dashboards.chart-february", level: 42 },
                { month: "dashboards.chart-march", level: 62 },
                { month: "dashboards.chart-april", level: 72 },
                { month: "dashboards.chart-may", level: 35 }
            ] 
        },
        { 
            schoolName: 'Escuela Secundaria de Arecibo', 
            data: [ 
                { month: "dashboards.chart-january", level: 55 },
                { month: "dashboards.chart-february", level: 45 },
                { month: "dashboards.chart-march", level: 65 },
                { month: "dashboards.chart-april", level: 75 },
                { month: "dashboards.chart-may", level: 38 }
            ] 
        },
    ],
    adminOre: [
        { 
            schoolName: 'Todas las escuelas', 
            data: baseAverageTimeReading 
        },
        { 
            schoolName: 'Escuela Intermedia Central', 
            data: [
                { month: "dashboards.chart-january", level: 60 },
                { month: "dashboards.chart-february", level: 50 },
                { month: "dashboards.chart-march", level: 70 },
                { month: "dashboards.chart-april", level: 80 },
                { month: "dashboards.chart-may", level: 45 }
            ]
        },
        { 
            schoolName: 'Escuela Primaria de San Juan', 
            data: [
                { month: "dashboards.chart-january", level: 50 },
                { month: "dashboards.chart-february", level: 40 },
                { month: "dashboards.chart-march", level: 60 },
                { month: "dashboards.chart-april", level: 70 },
                { month: "dashboards.chart-may", level: 33 }
            ] 
        },
    ],
    parent: [
        { 
            studentName: 'Sofía Martínez', 
            data: [
                { month: "dashboards.chart-january", level: 60 },
                { month: "dashboards.chart-february", level: 55 },
                { month: "dashboards.chart-march", level: 75 },
                { month: "dashboards.chart-april", level: 80 },
                { month: "dashboards.chart-may", level: 40 }
            ]
        },
    ]
}

// Definición base para el desempeño promedio (Todos los...)
export const baseReadingVolume = [
    { session: "1", volume: 10 },
    { session: "2", volume: 15 },
    { session: "3", volume: 10 },
    { session: "4", volume: 18 },
    { session: "5", volume: 26 },
];

export const readingVolumeMockedData = {
    teacher: [
        { 
            studentName: 'Todos los estudiantes', 
            data: baseReadingVolume 
        }, 
        { 
            studentName: 'Sofía Martínez', 
            data: [
                { session: "1", volume: 15 },
                { session: "2", volume: 20 },
                { session: "3", volume: 18 },
                { session: "4", volume: 25 },
                { session: "5", volume: 30 },
            ]
        },
        { 
            studentName: 'Alejandro Vargas', 
            data: [
                { session: "1", volume: 5 },
                { session: "2", volume: 8 },
                { session: "3", volume: 6 },
                { session: "4", volume: 10 },
                { session: "5", volume: 12 },
            ]
        },
        { 
            studentName: 'Isabella Gómez', 
            data: [
                { session: "1", volume: 12 },
                { session: "2", volume: 14 },
                { session: "3", volume: 16 },
                { session: "4", volume: 18 },
                { session: "5", volume: 20 },
            ]
        },
        { 
            studentName: 'María López', 
            data: [ 
                { session: "1", volume: 8 },
                { session: "2", volume: 12 },
                { session: "3", volume: 14 },
                { session: "4", volume: 16 },
                { session: "5", volume: 22 },
            ]
        },
        {
            studentName: 'Juan Perez', 
            data: [ 
                { session: "1", volume: 6 },
                { session: "2", volume: 9 },
                { session: "3", volume: 7 },
                { session: "4", volume: 11 },
                { session: "5", volume: 13 },
            ]
        },
        { 
            studentName: 'Grupo A - 3.er grado', 
            data: [
                { session: "1", volume: 12 },
                { session: "2", volume: 17 },
                { session: "3", volume: 13 },
                { session: "4", volume: 20 },
                { session: "5", volume: 28 },
            ] 
        },
        { 
            studentName: 'Grupo B - 4.o grado', 
            data: [
                { session: "1", volume: 8 },
                { session: "2", volume: 13 },
                { session: "3", volume: 10 },
                { session: "4", volume: 15 },
                { session: "5", volume: 21 },
            ]
        },
    ],
    director: [
        { 
            groupName: 'Todos los grupos', 
            data: baseReadingVolume 
        }, 
        { 
            groupName: 'Grupo A - 3.er grado', 
            data: [
                { session: "1", volume: 12 },
                { session: "2", volume: 17 },
                { session: "3", volume: 13 },
                { session: "4", volume: 20 },
                { session: "5", volume: 28 },
            ] 
        },
        { 
            groupName: 'Grupo B - 4.o grado', 
            data: [
                { session: "1", volume: 8 },
                { session: "2", volume: 13 },
                { session: "3", volume: 10 },
                { session: "4", volume: 15 },
                { session: "5", volume: 21 },
            ] 
        },
    ],
    admin: [
        { 
            schoolName: 'Todas las escuelas', 
            data: baseReadingVolume 
        }, 
        {
            schoolName: 'San Juan', 
            data: [ 
                { session: "1", volume: 27 },
                { session: "2", volume: 35 },
                { session: "3", volume: 22 },
                { session: "4", volume: 34 },
                { session: "5", volume: 28 },
            ] 
        },
        {
            schoolName: 'Humacao', 
            data: [
                { session: "1", volume: 8 },
                { session: "2", volume: 12 },
                { session: "3", volume: 9 },
                { session: "4", volume: 15 },
                { session: "5", volume: 20 },
            ] 
        },
        {
            schoolName: 'Bayamón', 
            data: [ 
                { session: "1", volume: 11 },
                { session: "2", volume: 16 },
                { session: "3", volume: 12 },
                { session: "4", volume: 19 },
                { session: "5", volume: 27 },
            ] 
        },
        {
            schoolName: 'Arecibo', 
            data: [ 
                { session: "1", volume: 14 },
                { session: "2", volume: 19 },
                { session: "3", volume: 15 },
                { session: "4", volume: 23 },
                { session: "5", volume: 32 },
            ] 
        },
        { 
            schoolName: 'Escuela Intermedia Central', 
            data: [
                { session: "1", volume: 44 },
                { session: "2", volume: 55 },
                { session: "3", volume: 33 },
                { session: "4", volume: 50 },
                { session: "5", volume: 30 },
            ]
        },
        { 
            schoolName: 'Escuela Primaria de San Juan', 
            data: [
                { session: "1", volume: 10 },
                { session: "2", volume: 15 },
                { session: "3", volume: 10 },
                { session: "4", volume: 18 },
                { session: "5", volume: 26 },
            ] 
        },
        { 
            schoolName: 'Academia del Sur', 
            data: [
                { session: "1", volume: 8 },
                { session: "2", volume: 12 },
                { session: "3", volume: 9 },
                { session: "4", volume: 15 },
                { session: "5", volume: 20 },
            ] 
        },
        { 
            schoolName: 'Bayamón High School', 
            data: [
                { session: "1", volume: 11 },
                { session: "2", volume: 16 },
                { session: "3", volume: 12 },
                { session: "4", volume: 19 },
                { session: "5", volume: 27 },
            ] 
        },
        { 
            schoolName: 'Escuela Secundaria de Arecibo', 
            data: [
                { session: "1", volume: 14 },
                { session: "2", volume: 19 },
                { session: "3", volume: 15 },
                { session: "4", volume: 23 },
                { session: "5", volume: 32 },
            ] 
        },
    ],
    adminOre: [
        { 
            schoolName: 'Todas las escuelas', 
            data: baseReadingVolume 
        },
        { 
            schoolName: 'Escuela Intermedia Central', 
            data: [
                { session: "1", volume: 44 },
                { session: "2", volume: 55 },
                { session: "3", volume: 33 },
                { session: "4", volume: 50 },
                { session: "5", volume: 30 },
            ]
        },
        { 
            schoolName: 'Escuela Primaria de San Juan', 
            data: [
                { session: "1", volume: 10 },
                { session: "2", volume: 15 },
                { session: "3", volume: 10 },
                { session: "4", volume: 18 },
                { session: "5", volume: 26 },
            ] 
        },
    ],
    parent: [
        { 
            studentName: 'Sofía Martínez', 
            data: [
                { session: "1", volume: 15 },
                { session: "2", volume: 20 },
                { session: "3", volume: 18 },
                { session: "4", volume: 25 },
                { session: "5", volume: 30 },
            ] 
        },
    ]
}

export const baseReadingCategories = [
    { topic: "Ciencias", completed: 10 },
    { topic: "Historia", completed: 5 },
    { topic: "Ficción", completed: 12 },
    { topic: "Lenguas", completed: 17 },
];

export const readingCategoriesMockedData = {
    teacher: [
        { 
            studentName: 'Todos los estudiantes', 
            data: baseReadingCategories 
        }, 
        { 
            studentName: 'Sofía Martínez', 
            data: [
                { topic: "Ciencias", completed: 15 },
                { topic: "Historia", completed: 8 },
                { topic: "Ficción", completed: 20 },
                { topic: "Lenguas", completed: 25 },
            ]
        },
        { 
            studentName: 'Alejandro Vargas', 
            data: [
                { topic: "Ciencias", completed: 5 },
                { topic: "Historia", completed: 2 },
                { topic: "Ficción", completed: 8 },
                { topic: "Lenguas", completed: 6 },
            ]
        },
        { 
            studentName: 'Isabella Gómez', 
            data: [
                { topic: "Ciencias", completed: 12 },
                { topic: "Historia", completed: 6 },
                { topic: "Ficción", completed: 15 },
                { topic: "Lenguas", completed: 20 },
            ]
        },
        {
            studentName: 'María López',
            data: [
                { topic: "Ciencias", completed: 10 },
                { topic: "Historia", completed: 5 },
                { topic: "Ficción", completed: 12 },
                { topic: "Lenguas", completed: 17 },
            ]
        },
        {
            studentName: 'Juan Perez',
            data: [
                { topic: "Ciencias", completed: 6 },
                { topic: "Historia", completed: 3 },
                { topic: "Ficción", completed: 9 },
                { topic: "Lenguas", completed: 8 },
            ]
        },
        {
            studentName: 'Grupo B - 4.o grado',
            data: [
                { topic: "Ciencias", completed: 22 },
                { topic: "Historia", completed: 18 },
                { topic: "Ficción", completed: 19 },
                { topic: "Lenguas", completed: 33 },
            ]
        },
        { 
            studentName: 'Grupo A - 3.er grado', 
            data: [
                { topic: "Ciencias", completed: 35 },
                { topic: "Historia", completed: 26 },
                { topic: "Ficción", completed: 28 },
                { topic: "Lenguas", completed: 37 },
            ]
        },
    ],
    director: [
        { 
            groupName: 'Todos los grupos', 
            data: baseReadingCategories 
        }, 
        { 
            groupName: 'Grupo A - 3.er grado', 
            data: [
                { topic: "Ciencias", completed: 35 },
                { topic: "Historia", completed: 26 },
                { topic: "Ficción", completed: 28 },
                { topic: "Lenguas", completed: 37 },
            ]
        },
        { 
            groupName: 'Grupo B - 4.o grado', 
            data: [
                { topic: "Ciencias", completed: 22 },
                { topic: "Historia", completed: 18 },
                { topic: "Ficción", completed: 19 },
                { topic: "Lenguas", completed: 33 },
            ]
        },
    ],
    admin: [
        { 
            schoolName: 'Todas las escuelas', 
            data: baseReadingCategories 
        },
        {
            schoolName: 'San Juan', 
            data: [ 
                { topic: "Ciencias", completed: 61 },
                { topic: "Historia", completed: 55 },
                { topic: "Ficción", completed: 70 },
                { topic: "Lenguas", completed: 77 },
            ] 
        },
        {
            schoolName: 'Humacao', 
            data: [
                { topic: "Ciencias", completed: 44 },
                { topic: "Historia", completed: 55 },
                { topic: "Ficción", completed: 71 },
                { topic: "Lenguas", completed: 45 },
            ] 
        },
        {
            schoolName: 'Bayamón', 
            data: [ 
                { topic: "Ciencias", completed: 65 },
                { topic: "Historia", completed: 67 },
                { topic: "Ficción", completed: 30 },
                { topic: "Lenguas", completed: 45 },
            ] 
        },
        {
            schoolName: 'Arecibo', 
            data: [ 
                { topic: "Ciencias", completed: 22 },
                { topic: "Historia", completed: 42 },
                { topic: "Ficción", completed: 33 },
                { topic: "Lenguas", completed: 29 },
            ] 
        },
        { 
            schoolName: 'Escuela Intermedia Central', 
            data: [
                { topic: "Ciencias", completed: 66 },
                { topic: "Historia", completed: 42 },
                { topic: "Ficción", completed: 72 },
                { topic: "Lenguas", completed: 66 },
            ]
        },
        { 
            schoolName: 'Escuela Primaria de San Juan', 
            data: [ 
                { topic: "Ciencias", completed: 55 },
                { topic: "Historia", completed: 67 },
                { topic: "Ficción", completed: 68 },
                { topic: "Lenguas", completed: 88 },
            ] 
        },
        { 
            schoolName: 'Academia del Sur', 
            data: [
                { topic: "Ciencias", completed: 44 },
                { topic: "Historia", completed: 55 },
                { topic: "Ficción", completed: 71 },
                { topic: "Lenguas", completed: 45 },
            ] 
        },
        { 
            schoolName: 'Bayamón High School', 
            data: [ 
                { topic: "Ciencias", completed: 65 },
                { topic: "Historia", completed: 67 },
                { topic: "Ficción", completed: 30 },
                { topic: "Lenguas", completed: 45 },
            ] 
        },
        { 
            schoolName: 'Escuela Secundaria de Arecibo', 
            data: [ 
                { topic: "Ciencias", completed: 22 },
                { topic: "Historia", completed: 42 },
                { topic: "Ficción", completed: 33 },
                { topic: "Lenguas", completed: 29 },
            ] 
        },
    ],
    adminOre: [
        { 
            schoolName: 'Todas las escuelas', 
            data: baseReadingCategories 
        },
        { 
            schoolName: 'Escuela Intermedia Central', 
            data: [
                { topic: "Ciencias", completed: 66 },
                { topic: "Historia", completed: 42 },
                { topic: "Ficción", completed: 72 },
                { topic: "Lenguas", completed: 66 },
            ]
        },
        { 
            schoolName: 'Escuela Primaria de San Juan', 
            data: [ 
                { topic: "Ciencias", completed: 55 },
                { topic: "Historia", completed: 67 },
                { topic: "Ficción", completed: 68 },
                { topic: "Lenguas", completed: 88 },
            ] 
        },
    ],
    parent: [
        { 
            studentName: 'Sofía Martínez', 
            data: [
                { topic: "Ciencias", completed: 15 },
                { topic: "Historia", completed: 8 },
                { topic: "Ficción", completed: 20 },
                { topic: "Lenguas", completed: 25 },
            ]
        },
    ]
}

export const baseReadingKeyMetrics = {
    completionRate: 80,
    totalReadings: 15,
};

export const readingKeyMetricsMockedData = {
    teacher: [
        { 
            studentName: 'Todos los estudiantes', 
            data: baseReadingKeyMetrics 
        }, 
        { 
            studentName: 'Sofía Martínez', 
            data: { completionRate: 95, totalReadings: 25 }
        },
        { 
            studentName: 'Alejandro Vargas', 
            data: { completionRate: 60, totalReadings: 8 }
        },
        { 
            studentName: 'Isabella Gómez', 
            data: { completionRate: 85, totalReadings: 18 }
        },
        { 
            studentName: 'María López',
            data: { completionRate: 90, totalReadings: 20 }
        },
        { 
            studentName: 'Juan Perez',
            data: { completionRate: 55, totalReadings: 7 }
        },
        { 
            studentName: 'Grupo B - 4.o grado', 
            data: { completionRate: 75, totalReadings: 12 }
        },
        { 
            studentName: 'Grupo A - 3.er grado', 
            data: { completionRate: 85, totalReadings: 18 }
        },
    ],
    director: [
        { 
            groupName: 'Todos los grupos', 
            data: baseReadingKeyMetrics
        }, 
        { 
            groupName: 'Grupo A - 3.er grado', 
            data: { completionRate: 85, totalReadings: 18 } 
        },
        { 
            groupName: 'Grupo B - 4.o grado', 
            data: { completionRate: 75, totalReadings: 12 } 
        },
    ],
    admin: [
        { 
            schoolName: 'Todas las escuelas', 
            data: baseReadingKeyMetrics 
        },
        {
            schoolName: 'San Juan', 
            data: { completionRate: 85, totalReadings: 19 } 
        },
        {
            schoolName: 'Humacao', 
            data: { completionRate: 70, totalReadings: 10 } 
        },
        {
            schoolName: 'Bayamón', 
            data: { completionRate: 82, totalReadings: 16 } 
        },
        {
            schoolName: 'Arecibo', 
            data: { completionRate: 88, totalReadings: 20 } 
        },
        { 
            schoolName: 'Escuela Intermedia Central', 
            data: { completionRate: 90, totalReadings: 22 }
        },
        { 
            schoolName: 'Escuela Primaria de San Juan', 
            data: { completionRate: 80, totalReadings: 15 } 
        },
        { 
            schoolName: 'Academia del Sur', 
            data: { completionRate: 70, totalReadings: 10 } 
        },
        { 
            schoolName: 'Bayamón High School', 
            data: { completionRate: 82, totalReadings: 16 } 
        },
        { 
            schoolName: 'Escuela Secundaria de Arecibo', 
            data: { completionRate: 88, totalReadings: 20 } 
        },
    ],
    parent: [
        { 
            studentName: 'Sofía Martínez', 
            data: { completionRate: 95, totalReadings: 25 }
        },
    ]
}