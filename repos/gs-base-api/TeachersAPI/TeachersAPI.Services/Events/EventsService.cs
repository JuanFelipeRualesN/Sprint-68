using System.Collections.Generic;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using TeachersAPI.Domain.Models.Events.Admin;
using TeachersAPI.Domain.Models.Generals.Responses.Admin;
using TeachersAPI.TeachersAPI.Infrastructure.DapperHandler;

namespace TeachersAPI.TeachersAPI.Services.Events
{
    /// <summary>
    /// Provides event-related operations for teachers.
    /// </summary>
    public class EventsService
    {
        /// <summary>
        /// Gets the lesson, lesson-quiz, homework and calendar events for a teacher
        /// within the given month and year.
        /// </summary>
        /// <param name="month">The month (1-12).</param>
        /// <param name="year">The four-digit year.</param>
        /// <param name="userId">The teacher's user id.</param>
        /// <param name="roleId">The role id.</param>
        /// <param name="schoolIds">The schools ids.</param>
        /// <param name="oreIds">The regions ids. </param>
        /// <param name="langCode">The language code.</param>
        /// <returns>Returns the event response models for the given month and year.</returns>
        public List<EventResponseModel> GetEventsByMonthAndYear(int month, int year, int userId, string roleId, List<int> schoolIds = null, List<string> oreIds = null, string langCode = null)
        {
            var events = EventsDapperHandler.GetEventsByMonthAndYear(month, year, userId, roleId, schoolIds, oreIds);

            // Now, classify the ones that are lesson to call the lesson titles from Athenas Feign. 
            var lessonIdsToCall = new List<string>();
            foreach (var eventResponse in events)
            {
                if (eventResponse.EventModel.EventType == EventType.Lesson)
                {
                    lessonIdsToCall.Add(eventResponse.EventModel.Id);
                }
            }

            if (lessonIdsToCall.Count > 0)
            {
                // Call Athenas Feign to get the lesson titles for the lessonIdsToCall
                var athenasLessonsResults = AthenasHandler.GetLessonsWithUnits(lessonIdsToCall, langCode);

                // Attach the lesson titles to the corresponding events
                foreach (var eventResponse in events)
                {
                    if (eventResponse.EventModel.EventType == EventType.Lesson)
                    {
                        var lesson = athenasLessonsResults.Find(l => l.LessonId == eventResponse.EventModel.Id);
                        if (lesson != null)
                        {
                            if (!string.IsNullOrWhiteSpace(lesson.Unit))
                            {
                                eventResponse.EventModel.Title = lesson?.Unit + " - " + lesson?.LessonTitle;
                            }
                            else
                            {
                                eventResponse.EventModel.Title = lesson?.LessonTitle;
                            }
                        }
                    }
                }
            }

            return events;
        }
    }
}