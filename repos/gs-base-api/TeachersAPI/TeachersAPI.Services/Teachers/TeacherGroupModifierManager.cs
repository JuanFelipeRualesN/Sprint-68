using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TeachersAPI.Domain.Models.Teachers;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.TeachersAPI.Infrastructure.DapperHandler;
using TeachersAPI.TeachersAPI.Infrastructure.Repositories.Teachers;
using TeachersAPI.TeachersAPI.Services.Generals;
using TeachersAPI.Utils;
using TeachersAPI;

namespace TeachersAPI.TeachersAPI.Services.Teachers
{
    /// <summary>
    /// To handle the groups.
    /// </summary>
    public class TeacherGroupModifierManager
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;
        private readonly string _langCode;
        private const int MaxLengthStringName = 150;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">The connection string (writer).</param>
        /// <param name="connectionStringRead">The connection string (reader).</param>
        /// <param name="langCode">The language code of the user.</param>
        public TeacherGroupModifierManager(string connectionString, string connectionStringRead, string langCode)
        {
            _connectionString = connectionString;
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// To archive a group.
        /// </summary>
        /// <param name="teacherGroupModel">Teacher Group Model to archive.</param>
        /// <param name="userSessionId">The user id of the user that is making the request.</param>
        /// <returns>Return a response model</returns>
        public ResponseModel ArchiveGroup(TeacherGroupModel teacherGroupModel, string userSessionId)
        {
            if (teacherGroupModel == null || string.IsNullOrWhiteSpace(teacherGroupModel.TeacherId) || string.IsNullOrWhiteSpace(teacherGroupModel.Id))
            {
                return new ResponseModel()
                {
                    Valid = false,
                };
            }

            new TeacherGroupDAO().Update(_connectionString, new TeacherGroupModel()
            {
                Available = "0",
                ModifiedBy = userSessionId,
            }, new TeacherGroupModel()
            {
                TeacherId = teacherGroupModel.TeacherId,
                Id = teacherGroupModel.Id,
            });

            // Return archive successful
            return new ResponseModel()
            {
                Valid = true,
                Messages = new List<string>()
                {
                    new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_success"]),
                }
            };
        }

        /// <summary>
        /// Delete the entire group events calendar.
        /// </summary>
        /// <param name="groupId">The group Id.</param>
        /// <param name="responseSession">Session info.</param>
        /// <returns></returns>
        public async Task RemoveAllEventCalendarFromGroup(string groupId, ValidationResponseModel responseSession)
        {
            // Remove event in Microsoft Teams for all users.
            await GraphAPI.DeleteEventAsync(responseSession.Email, string.Empty, groupId);
        }

        /// <summary>
        /// Manage for remove or add all events calendar from participant.
        /// </summary>
        /// <param name="userId">The student Id.</param>
        /// <param name="groupId">The group Id.</param>
        /// <param name="responseSession">Session info.</param>
        /// <returns></returns>
        private async Task ManageAllEventCalendarFromParticipant(string userId, string groupId, ValidationResponseModel responseSession, bool remove = false)
        {
            UserResponse student = GlobalManager.GetStudent(userId, responseSession);

            // Remove event in Microsoft Teams for the unique user.
            await GraphAPI.PatchEventAsync(responseSession.Email, student, groupId, remove);
        }

        /// <summary>
        /// Deletes a student from a group.
        /// </summary>
        /// <param name="teacherGroupStudentModel">Teacher groupt student.</param>
        /// <param name="deleteByTeacher">If the deleter is a teacher.</param>
        /// <param name="teacherId">Teacher Id</param>
        /// <returns>Return the response model with transaction details.</returns>
        public ResponseModel DeleteStudentFromGroup(TeacherGroupStudentModel teacherGroupStudentModel, bool deleteByTeacher, string teacherId)
        {
            bool canBeModified = true;
            if (deleteByTeacher)
            {
                TeacherGroupModel teacherGroupStudent = new TeacherGroupModel()
                {
                    Id = teacherGroupStudentModel.TeacherGroupId,
                    TeacherId = teacherId
                };

                if (new TeacherGroupDAO().Read(_connectionStringRead, teacherGroupStudent, true, true).Count == 0)
                {
                    canBeModified = false;
                }
            }

            if (!canBeModified)
            {
                return new ResponseModel()
                {
                    Valid = false,
                };
            }

            new TeacherGroupStudentDAO().Delete(_connectionString, new TeacherGroupStudentModel()
            {
                StudentId = teacherGroupStudentModel.StudentId,
                TeacherGroupId = teacherGroupStudentModel.TeacherGroupId,
            });

            // Return archive successful
            return new ResponseModel()
            {
                Valid = true,
                Messages = new List<string>()
                {
                    new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_success"]),
                }
            };

        }

        /// <summary>
        /// Deletes a student from a group.
        /// </summary>
        /// <param name="teacherGroupStudentModel">Teacher groupt student.</param>
        /// <param name="deleteByTeacher">If the deleter is a teacher.</param>
        /// <param name="responseSession">Session info.</param>
        /// <returns>Return the response model with transaction details.</returns>
        public async Task<ResponseModel> DeleteStudentFromGroup(TeacherGroupStudentModel teacherGroupStudentModel, bool deleteByTeacher, ValidationResponseModel responseSession)
        {
            bool canBeModified = true;
            if (deleteByTeacher)
            {
                TeacherGroupModel teacherGroupStudent = new TeacherGroupModel()
                {
                    Id = teacherGroupStudentModel.TeacherGroupId,
                    TeacherId = responseSession.UserId
                };

                if (new TeacherGroupDAO().Read(_connectionStringRead, teacherGroupStudent, true, true).Count == 0)
                {
                    canBeModified = false;
                }
            }

            if (!canBeModified)
            {
                return new ResponseModel()
                {
                    Valid = false,
                };
            }

            new TeacherGroupStudentDAO().Delete(_connectionString, new TeacherGroupStudentModel()
            {
                StudentId = teacherGroupStudentModel.StudentId,
                TeacherGroupId = teacherGroupStudentModel.TeacherGroupId,
            });

            try
            {
                await ManageAllEventCalendarFromParticipant(teacherGroupStudentModel.StudentId, teacherGroupStudentModel.TeacherGroupId, responseSession, true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An exception has occurred in the graph process, detail: {ex.Message}");
            }

            // Return archive successful
            return new ResponseModel()
            {
                Valid = true,
                Messages = new List<string>()
                {
                    new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_success"]),
                }
            };

        }

        /// <summary>
        /// Adds a student to a group.
        /// </summary>
        /// <param name="teacherGroupStudentModel">Teacher group with the student details.</param>
        /// <param name="createByTeacher">Created by teacher.</param>
        /// <param name="teacherId">Teacher Id.</param>
        /// <param name="userSessionId">The user id of the user that is making the request.</param>
        /// <param name="applyDuplicationSameSubjectLevelForStudentLogic"> To apply the evaluation of avoid duplication for a student in groups of the same subject and level.</param>
        /// <returns>ResponseModel with details.</returns>
        public ResponseModel AddStudentToGroup(TeacherGroupStudentModel teacherGroupStudentModel, bool createByTeacher, string teacherId, string userSessionId, bool applyDuplicationSameSubjectLevelForStudentLogic)
        {
            TeacherGroupDAO teacherGroupDAO = new TeacherGroupDAO();
            TeacherGroupStudentDAO teacherGroupStudentDAO = new TeacherGroupStudentDAO();

            // Check if the group was created by the teacher, to allow changes. Otherwise, do not allow it.
            TeacherGroupModel teacherGroupExists = teacherGroupDAO.Read(_connectionStringRead, new TeacherGroupModel() { Id = teacherGroupStudentModel.TeacherGroupId }, true, true)[0];

            // If created by teacher required and the teacher Id is not equals to teacher id, then return false.
            if (createByTeacher && teacherGroupExists.TeacherId != teacherId)
            {
                return new ResponseModel()
                {
                    Valid = false,
                };
            }

            // Get the teacher group students 
            List<TeacherGroupStudentModel> teacherGroupStudentList = teacherGroupStudentDAO.Read(_connectionStringRead, new TeacherGroupStudentModel()
            {
                StudentId = teacherGroupStudentModel.StudentId,
            }, false, false);

            // Check if not exists in the group already 
            if (teacherGroupStudentList.Find(x => x.TeacherGroupId == teacherGroupStudentModel.TeacherGroupId) == null)
            {
                // Continue with everything else 

                // Now check if avoid duplication for level and subject has to be applied
                if (applyDuplicationSameSubjectLevelForStudentLogic)
                {
                    // Get the group details 
                    List<TeacherGroupModel> teacherGroupsStudentListToFind = new List<TeacherGroupModel>();
                    foreach (TeacherGroupStudentModel teacherGroupStudent in teacherGroupStudentList)
                    {
                        teacherGroupsStudentListToFind.Add(new TeacherGroupModel()
                        {
                            Id = teacherGroupStudent.TeacherGroupId,
                            Available = "1"
                        });
                    }

                    List<TeacherGroupModel> teacherGroupStudentResults = teacherGroupDAO.Read(_connectionStringRead, teacherGroupsStudentListToFind);

                    // Now check if not exists
                    if (teacherGroupStudentResults.Find(x => x.LevelCode == teacherGroupExists.LevelCode && x.SubjectCode == teacherGroupExists.SubjectCode) == null)
                    {
                        new TeacherGroupStudentDAO().Create(_connectionString, new TeacherGroupStudentModel()
                        {
                            StudentId = teacherGroupStudentModel.StudentId,
                            TeacherGroupId = teacherGroupStudentModel.TeacherGroupId,
                            ModifiedBy = userSessionId,
                        });

                        // Return archive successful
                        return new ResponseModel()
                        {
                            Valid = true,
                            Messages = new List<string>()
                            {
                                new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_success"]),
                            }
                        };
                    }
                    else
                    {
                        // Duplicated 
                        return new ResponseModel()
                        {
                            Valid = false,
                            Messages = new List<string>()
                        {
                            new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_student_duplicated_group_subject_level"]),
                        }
                        };
                    }
                }
                else
                {
                    new TeacherGroupStudentDAO().Create(_connectionString, new TeacherGroupStudentModel()
                    {
                        StudentId = teacherGroupStudentModel.StudentId,
                        TeacherGroupId = teacherGroupStudentModel.TeacherGroupId,
                        ModifiedBy = userSessionId,
                    });

                    // Return archive successful
                    return new ResponseModel()
                    {
                        Valid = true,
                        Messages = new List<string>()
                            {
                                new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_success"]),
                            }
                    };
                }
            }
            else
            {
                // Duplicated in the group, but nothing happens, so just return okay.
                return new ResponseModel()
                {
                    Valid = true,
                    Messages = new List<string>()
                            {
                                new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_success"]),
                            }
                };
            }
        }

        /// <summary>
        /// Adds a student to a group.
        /// </summary>
        /// <param name="teacherGroupStudentModel">Teacher group with the student details.</param>
        /// <param name="createByTeacher">Created by teacher.</param>
        /// <param name="responseSession">Session info.</param>
        /// <param name="applyDuplicationSameSubjectLevelForStudentLogic"> To apply the evaluation of avoid duplication for a student in groups of the same subject and level.</param>
        /// <returns>ResponseModel with details.</returns>
        public async Task<ResponseModel> AddStudentToGroup(TeacherGroupStudentModel teacherGroupStudentModel, bool createByTeacher, ValidationResponseModel responseSession, bool applyDuplicationSameSubjectLevelForStudentLogic)
        {
            TeacherGroupDAO teacherGroupDAO = new TeacherGroupDAO();
            TeacherGroupStudentDAO teacherGroupStudentDAO = new TeacherGroupStudentDAO();

            // Check if the group was created by the teacher, to allow changes. Otherwise, do not allow it.
            TeacherGroupModel teacherGroupExists = teacherGroupDAO.Read(_connectionStringRead, new TeacherGroupModel() { Id = teacherGroupStudentModel.TeacherGroupId }, true, true)[0];

            // If created by teacher required and the teacher Id is not equals to teacher id, then return false.
            if (createByTeacher && teacherGroupExists.TeacherId != responseSession.UserId)
            {
                return new ResponseModel()
                {
                    Valid = false,
                };
            }

            // Get the teacher group students 
            List<TeacherGroupStudentModel> teacherGroupStudentList = teacherGroupStudentDAO.Read(_connectionStringRead, new TeacherGroupStudentModel()
            {
                StudentId = teacherGroupStudentModel.StudentId,
            }, false, false);

            // Check if not exists in the group already 
            if (teacherGroupStudentList.Find(x => x.TeacherGroupId == teacherGroupStudentModel.TeacherGroupId) == null)
            {
                // Continue with everything else 

                // Now check if avoid duplication for level and subject has to be applied
                if (applyDuplicationSameSubjectLevelForStudentLogic)
                {
                    // Get the group details 
                    List<TeacherGroupModel> teacherGroupsStudentListToFind = new List<TeacherGroupModel>();
                    foreach (TeacherGroupStudentModel teacherGroupStudent in teacherGroupStudentList)
                    {
                        teacherGroupsStudentListToFind.Add(new TeacherGroupModel()
                        {
                            Id = teacherGroupStudent.TeacherGroupId,
                            Available = "1"
                        });
                    }

                    List<TeacherGroupModel> teacherGroupStudentResults = teacherGroupDAO.Read(_connectionStringRead, teacherGroupsStudentListToFind);

                    // Now check if not exists
                    if (teacherGroupStudentResults.Find(x => x.LevelCode == teacherGroupExists.LevelCode && x.SubjectCode == teacherGroupExists.SubjectCode) == null)
                    {
                        new TeacherGroupStudentDAO().Create(_connectionString, new TeacherGroupStudentModel()
                        {
                            StudentId = teacherGroupStudentModel.StudentId,
                            TeacherGroupId = teacherGroupStudentModel.TeacherGroupId,
                            ModifiedBy = responseSession.UserId,
                        });

                        try
                        {
                            await ManageAllEventCalendarFromParticipant(teacherGroupStudentModel.StudentId, teacherGroupStudentModel.TeacherGroupId, responseSession);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"An exception has occurred in the graph process, detail: {ex.Message}");
                        }

                        // Return archive successful
                        return new ResponseModel()
                        {
                            Valid = true,
                            Messages = new List<string>()
                            {
                                new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_success"]),
                            }
                        };
                    }
                    else
                    {
                        // Duplicated 
                        return new ResponseModel()
                        {
                            Valid = false,
                            Messages = new List<string>()
                        {
                            new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_student_duplicated_group_subject_level"]),
                        }
                        };
                    }
                }
                else
                {
                    new TeacherGroupStudentDAO().Create(_connectionString, new TeacherGroupStudentModel()
                    {
                        StudentId = teacherGroupStudentModel.StudentId,
                        TeacherGroupId = teacherGroupStudentModel.TeacherGroupId,
                        ModifiedBy = responseSession.UserId,
                    });

                    try
                    {
                        await ManageAllEventCalendarFromParticipant(teacherGroupStudentModel.StudentId, teacherGroupStudentModel.TeacherGroupId, responseSession);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"An exception has occurred in the graph process, detail: {ex.Message}");
                    }

                    // Return archive successful
                    return new ResponseModel()
                    {
                        Valid = true,
                        Messages = new List<string>()
                            {
                                new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_success"]),
                            }
                    };
                }
            }
            else
            {
                // Duplicated in the group, but nothing happens, so just return okay.
                return new ResponseModel()
                {
                    Valid = true,
                    Messages = new List<string>()
                            {
                                new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_success"]),
                            }
                };
            }
        }

        /// <summary>
        /// To create the teacher's group. (A group of students that the teacher will handle).
        /// </summary>
        /// <param name="teacherGroupModel">Teacher group model to create.</param>
        /// <param name="userSessionId">The id of the user that is making the request..</param>
        /// <returns>Returns a Response Model with transaction details.</returns>
        public ResponseModel CreateGroup(TeacherGroupModel teacherGroupModel, string userSessionId)
        {
            if (teacherGroupModel == null || string.IsNullOrWhiteSpace(teacherGroupModel.TeacherId))
            {
                return new ResponseModel()
                {
                    Valid = false,
                };
            }

            if (string.IsNullOrWhiteSpace(teacherGroupModel.Name))
            {
                // Name require
                return new ResponseModel()
                {
                    Valid = false,
                    Messages = new List<string>()
                    {
                        new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_error_name_empty"]),
                    }
                };
            }

            // Truncate the string
            teacherGroupModel.Name = teacherGroupModel.Name.Trim();
            teacherGroupModel.Name = TruncateString(teacherGroupModel.Name, MaxLengthStringName);

            // Set the user modified by
            teacherGroupModel.ModifiedBy = userSessionId;

            // Create the group
            new TeacherGroupDAO().Create(_connectionString, teacherGroupModel);
            return new ResponseModel()
            {
                Valid = true,
                Messages = new List<string>()
                    {
                        new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_success"]),
                    }
            };
        }

        /// <summary>
        /// To auto attach students to teacher main sub group.
        /// </summary>
        /// <param name="teacherGroups">Subgroups details to add.</param>
        /// <param name="userSessionId">The user making the request.</param>
        public void AutoTeacherGroupCreation(List<Domain.Models.Generals.Dapper.TeacherGroupStudentModel> teacherGroups, string userSessionId)
        {
            // First, create some sublist by teacher and group name
            var sublists = teacherGroups
                .GroupBy(t => new { t.GroupName, t.TeacherId, t.LevelCode, t.SubjectCode })
                .Select(group => group.ToList())
                .ToList();

            // Check the existing groups
            var teacherGroupModelsToCheckIfExists = new List<TeacherGroupModel>();
            foreach (var group in sublists)
            {
                var commonProperties = group.FirstOrDefault();
                teacherGroupModelsToCheckIfExists.Add(new TeacherGroupModel()
                {
                    Name = commonProperties.GroupName,
                    TeacherId = commonProperties.TeacherId,
                });
            }

            var teacherGroup = new TeacherGroupDAO();

            var teacherGroupsThatExists = teacherGroup.Read(this._connectionStringRead, teacherGroupModelsToCheckIfExists);

            // Identify the new groups, and prepare the existings to students to add.
            var teacherGroupStudentModelsToInsert = new List<TeacherGroupStudentModel>(); 
            var teacherGroupModelToCreateList = new List<TeacherGroupModel>();
            foreach (var group in sublists)
            {
                var commonProperties = group.FirstOrDefault();

                var teacherGroupExisting = teacherGroupsThatExists.Find(x => x.Name == commonProperties.GroupName
                && x.TeacherId == commonProperties.TeacherId);

                if (teacherGroupExisting == null)
                {
                    teacherGroupModelToCreateList.Add(new TeacherGroupModel()
                    {
                        Name = commonProperties.GroupName,
                        TeacherId = commonProperties.TeacherId,
                        LevelCode = commonProperties.LevelCode,
                        SubjectCode = commonProperties.SubjectCode,
                        Available = "1"
                    });
                }
                else
                {
                    // Since the group exists, prepare the list for student-teachergroup insert 

                    // Retrieve the students 
                    var studentsInTeacherGroup = teacherGroups.FindAll(x => 
                    x.GroupName == commonProperties.GroupName && 
                    x.TeacherId == commonProperties.TeacherId); 
          
                    if(studentsInTeacherGroup == null)
                    {
                        continue;
                    }

                    foreach(var student in studentsInTeacherGroup)
                    {
                        teacherGroupStudentModelsToInsert.Add(new TeacherGroupStudentModel()
                        {
                            StudentId = student.StudentId,
                            TeacherGroupId = teacherGroupExisting.Id,
                            ModifiedBy = userSessionId
                        });
                    }                 
                }
            }

            // Now, read the new groups 
            teacherGroup.Create(this._connectionString, teacherGroupModelToCreateList);
            var teacherGroupRecentlyList = teacherGroup.Read(this._connectionStringRead, teacherGroupModelToCreateList);

            // Now, for recently added groups, add the teacherGroupStudentsToInsert. 
            foreach(var teacherGroupRecently in teacherGroupRecentlyList)
            {
                // Retrieve the students 
                var studentsInTeacherGroup = teacherGroups.FindAll(x =>
                x.GroupName == teacherGroupRecently.Name &&
                x.TeacherId == teacherGroupRecently.TeacherId);

                if (studentsInTeacherGroup == null)
                {
                    continue;
                }

                foreach (var student in studentsInTeacherGroup)
                {
                    teacherGroupStudentModelsToInsert.Add(new TeacherGroupStudentModel()
                    {
                        StudentId = student.StudentId,
                        TeacherGroupId = teacherGroupRecently.Id,
                        ModifiedBy = userSessionId
                    });
                }
            }

            // Create the teacher groups 
            if (teacherGroupStudentModelsToInsert.Count > 0)
            {
                // Insert them 
                new TeacherGroupStudentDAO().Create(this._connectionString, teacherGroupStudentModelsToInsert);
            }
        }

        /// <summary>
        /// To update the group name.
        /// </summary>
        /// <param name="groupId">The group's id.</param>
        /// <param name="groupName">The group's name.</param>
        /// <param name="modifiedBy">The user that is modifying the row.</param>
        /// <returns>Returns the response model with the updated group.</returns>
        public ResponseModel UpdateGroupName(int groupId, string groupName, string modifiedBy)
        {

            if (string.IsNullOrWhiteSpace(groupName))
            {
                // Name require
                return new ResponseModel()
                {
                    Valid = false,
                    Messages = new List<string>()
                    {
                        new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_error_name_empty"]),
                    }
                };
            }

            // Truncate the string
            groupName = groupName.Trim();
            groupName = TruncateString(groupName, MaxLengthStringName);


            try
            {
                new TeacherGroupDAO().Update(_connectionString, new TeacherGroupModel()
                {
                    Name = groupName, 
                    ModifiedBy = modifiedBy
                }, new TeacherGroupModel()
                {
                    Id = groupId.ToString()
                });

                return new ResponseModel()
                {
                    Valid = true,
                    Messages = new List<string>()
                    {
                        new MessageManager(_connectionStringRead, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_success"]),
                    }
                };
            }
            catch
            {
                return new ResponseModel()
                {
                    Valid = false,
                };
            }
        }

        /// <summary>
        /// To truncate a string.
        /// </summary>
        /// <param name="value">The string.</param>
        /// <param name="maxLength">The length.</param>
        /// <returns>Returns the string truncated.</returns>
        private string TruncateString(string value, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            if (value.Length > maxLength)
            {
                return value.Substring(0, maxLength);
            }

            return value;
        }
    }
}
