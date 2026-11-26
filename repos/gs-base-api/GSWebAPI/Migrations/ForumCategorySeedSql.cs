namespace GSWebAPI.Migrations
{
    /// <summary>
    /// The forum category catalog and its English / Spanish names, as idempotent upserts. Used by the migrations that create
    /// and align <c>community_forum_category_translations</c>, so they work on a new database (old seed) and on one whose
    /// categories were already edited. Only <c>code</c>, <c>name</c> and <c>moderators_only</c> are updated; dates of
    /// existing rows are kept. Must match the HasData of the category and translation configurations.
    /// </summary>
    internal static class ForumCategorySeedSql
    {
        /// <summary>Insert or update the seven categories.</summary>
        public const string Categories = """
            INSERT INTO `community_forum_categories` (`id`, `code`, `name`, `moderators_only`, `created_at`, `updated_at`) VALUES
            (1, 'general', 'General', 0, '2026-11-05 00:00:00', '2026-11-05 00:00:00'),
            (2, 'math', 'Matemáticas', 0, '2026-11-05 00:00:00', '2026-11-05 00:00:00'),
            (3, 'reading_writing', 'Lectura y Escritura', 0, '2026-11-05 00:00:00', '2026-11-05 00:00:00'),
            (4, 'science', 'Ciencias', 0, '2026-11-05 00:00:00', '2026-11-05 00:00:00'),
            (5, 'organization', 'Organización', 0, '2026-11-05 00:00:00', '2026-11-05 00:00:00'),
            (6, 'legal_depr', 'Legal y DEPR', 0, '2026-11-19 14:46:19', '2026-11-19 14:46:19'),
            (7, 'first_steps', 'Primeros pasos', 0, '2026-11-19 14:46:19', '2026-11-19 14:46:19')
            ON DUPLICATE KEY UPDATE `code` = VALUES(`code`), `name` = VALUES(`name`), `moderators_only` = VALUES(`moderators_only`);
            """;

        /// <summary>Insert or update the English and Spanish name of each category.</summary>
        public const string Translations = """
            INSERT INTO `community_forum_category_translations` (`community_forum_category_id`, `lang_code`, `name`, `created_at`, `updated_at`) VALUES
            (1, 'en', 'General', '2026-11-05 00:00:00', '2026-11-05 00:00:00'),
            (1, 'es', 'General', '2026-11-05 00:00:00', '2026-11-05 00:00:00'),
            (2, 'en', 'Math', '2026-11-05 00:00:00', '2026-11-05 00:00:00'),
            (2, 'es', 'Matemáticas', '2026-11-05 00:00:00', '2026-11-05 00:00:00'),
            (3, 'en', 'Reading and Writing', '2026-11-05 00:00:00', '2026-11-05 00:00:00'),
            (3, 'es', 'Lectura y Escritura', '2026-11-05 00:00:00', '2026-11-05 00:00:00'),
            (4, 'en', 'Science', '2026-11-05 00:00:00', '2026-11-05 00:00:00'),
            (4, 'es', 'Ciencias', '2026-11-05 00:00:00', '2026-11-05 00:00:00'),
            (5, 'en', 'Organization', '2026-11-05 00:00:00', '2026-11-05 00:00:00'),
            (5, 'es', 'Organización', '2026-11-05 00:00:00', '2026-11-05 00:00:00'),
            (6, 'en', 'Legal and DEPR', '2026-11-05 00:00:00', '2026-11-05 00:00:00'),
            (6, 'es', 'Legal y DEPR', '2026-11-05 00:00:00', '2026-11-05 00:00:00'),
            (7, 'en', 'First Steps', '2026-11-05 00:00:00', '2026-11-05 00:00:00'),
            (7, 'es', 'Primeros pasos', '2026-11-05 00:00:00', '2026-11-05 00:00:00')
            ON DUPLICATE KEY UPDATE `name` = VALUES(`name`);
            """;
    }
}
