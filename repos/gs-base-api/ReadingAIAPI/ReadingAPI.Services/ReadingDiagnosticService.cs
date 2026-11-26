using ReadingAPI.Domain.DTOs;
using ReadingAPI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReadingAPI.Services;

    /// <summary>
    /// Service layer for managing custom reading diagnostics and their associated questions.
    /// </summary>
    public class ReadingDiagnosticsService
    {
        private readonly CustomReadingDiagnosticInfrastructure _infra;

        public ReadingDiagnosticsService()
        {
            _infra = new CustomReadingDiagnosticInfrastructure();
        }

        public ReadingDiagnosticsService(CustomReadingDiagnosticInfrastructure infra)
        {
            _infra = infra;
        }

        /// <summary>
        /// Creates a new diagnostic and assigns a list of questions to it.
        /// </summary>
        /// <param name="diagnostic">Main diagnostic data.</param>
        /// <param name="questionIds">List of question IDs to attach to the diagnostic.</param>
        /// <param name="modifiedBy">User who performed the operation.</param>
        /// <returns>The ID of the newly created diagnostic.</returns>
        public int CreateDiagnosticWithQuestions(
            CustomReadingDiagnosticDto diagnostic,
            IEnumerable<int> questionIds,
            string modifiedBy)
        {
            diagnostic.ModifiedBy = modifiedBy;

            // Create diagnostic
            var diagnosticId = _infra.CreateDiagnostic(diagnostic);

            // Assign questions
            if (questionIds != null)
            {
                var questions = questionIds.Select(qId => new CustomReadingDiagnosticQuestionDto
                {
                    CustomDiagnosticId = diagnosticId,
                    QuestionId = qId,
                    ModifiedBy = modifiedBy
                }).ToList();

                if (questions.Count > 0)
                {
                    _infra.CreateDiagnosticQuestionsBulk(questions);
                }
            }

            return diagnosticId;
        }

        /// <summary>
        /// Retrieves a diagnostic by ID, with the option to include its related questions.
        /// </summary>
        public CustomReadingDiagnosticDto GetDiagnostic(
            int diagnosticId,
            bool includeQuestions,
            out List<CustomReadingDiagnosticQuestionDto> questions)
        {
            var diagnostic = _infra.GetDiagnosticById(diagnosticId);
            questions = new List<CustomReadingDiagnosticQuestionDto>();

            if (diagnostic != null && includeQuestions)
            {
                questions = _infra.GetQuestionsByDiagnosticId(diagnosticId).ToList();
            }

            return diagnostic;
        }

        /// <summary>
        /// Retrieves all diagnostics created by a specific teacher.
        /// </summary>
        public List<CustomReadingDiagnosticDto> GetDiagnosticsByTeacher(string teacherId)
        {
            return _infra.GetDiagnosticsByTeacher(teacherId).ToList();
        }

        /// <summary>
        /// Updates an existing diagnostic and replaces the associated question list.
        /// </summary>
        public void UpdateDiagnosticWithQuestions(
            CustomReadingDiagnosticDto diagnostic,
            IEnumerable<int> newQuestionIds,
            string modifiedBy)
        {
            diagnostic.ModifiedBy = modifiedBy;
            diagnostic.UpdatedAt = DateTime.UtcNow;

            // Update diagnostic record
            _infra.UpdateDiagnostic(diagnostic);

            // Remove existing question links
            _infra.DeleteQuestionsByDiagnosticId(diagnostic.Id);

            // Insert new question links
            if (newQuestionIds != null)
            {
                var questions = newQuestionIds.Select(qId => new CustomReadingDiagnosticQuestionDto
                {
                    CustomDiagnosticId = diagnostic.Id,
                    QuestionId = qId,
                    ModifiedBy = modifiedBy
                }).ToList();

                if (questions.Count > 0)
                {
                    _infra.CreateDiagnosticQuestionsBulk(questions);
                }
            }
        }

        /// <summary>
        /// Deletes a diagnostic and all associated questions.
        /// </summary>
        public void DeleteDiagnostic(int diagnosticId)
        {
            // The FK has ON DELETE CASCADE, but explicit deletion keeps the intent clear.
            _infra.DeleteQuestionsByDiagnosticId(diagnosticId);
            _infra.DeleteDiagnostic(diagnosticId);
        }
    }
