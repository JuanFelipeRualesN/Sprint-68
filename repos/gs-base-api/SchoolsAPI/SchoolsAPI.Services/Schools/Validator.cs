using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Text.RegularExpressions;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools.Request;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools.Response;
using SchoolsAPI.SchoolsAPI.Infrastructure.Repositories.Schools;
using SchoolsAPI.SchoolsAPI.Services.Generals;
using SchoolsAPI.SchoolsAPI.Infrastructure.Repositories.DapperHandler;

namespace SchoolsAPI.SchoolsAPI.Services.Schools
{
    /// <summary>
    /// To handle the validations.
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// To check if the academic detail year overlaps with other.
        /// </summary>
        /// <param name="academicDetailRequestModel">Academic detail request model to check.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="langCode">The language's code.</param>
        /// <returns>Returns the response model with details.</returns>
        public static ResponseModel ValidateAcademicDetailOverlappingDates(
            AcademicDetailRequestModel academicDetailRequestModel,
            string connectionString, string langCode)
        {
            // Call the academic details. 
            var academicDetails = AcademicDetails.GetAcademicDetails();
            // If there is no academic details, then there is no overlapping.
            if (academicDetails.Count == 0)
            {
                return new ResponseModel() { Valid = true };
            }

            // Check if the dates overlap
            foreach (var academicDetail in academicDetails)
            {
                if (DateTime.Parse(academicDetail.FullTermStartDate) <= DateTime.Parse(academicDetailRequestModel.AcademicDetail.FullTermEndDate) &&
                    DateTime.Parse(academicDetail.FullTermEndDate) >= DateTime.Parse(academicDetailRequestModel.AcademicDetail.FullTermStartDate))
                {
                    return new ResponseModel()
                    {
                        Valid = false,
                        Messages = new List<string>()
                        {
                            new MessageManager(connectionString, langCode).GetMessageDetails(
                                AppConfiguration.Configuration["AppSettings:sch_academic_year_overlapping"])
                        }
                    };
                }
            }

            return new ResponseModel() { Valid = true };
        }

        /// <summary>
        /// To validate the school academic details.
        /// </summary>
        /// <param name="entityAcademicDetailModel">Academic detail to validate.</param>
        /// <param name="isEdit">If it is edit or not.</param>
        /// <param name="includeEntitySubscriptionId">True to include the entity subscription id, else false.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="langCode">The language code to return to the user the details.</param>
        /// <returns>Returns the response model with the validationd etails.</returns>
        public static ResponseModel ValidateSchoolAcademicDetails(EntityAcademicDetailModel entityAcademicDetailModel, bool isEdit, bool includeEntitySubscriptionId, string connectionString, string langCode)
        {
            ResponseModel responseModel = new ResponseModel()
            {
                Valid = true,
                MessageCodes = new List<string>(),
                Messages = new List<string>(),
            };

            MessageManager messageManager = new MessageManager(connectionString, langCode);

            if (isEdit && string.IsNullOrWhiteSpace(entityAcademicDetailModel.Id))
            {
                responseModel.Valid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_academic_details_id_required"]));
                return responseModel;
            }

            // Entity subscription id cannot be duplicated, so we must evaluate it. 
            if (!string.IsNullOrWhiteSpace(entityAcademicDetailModel.EntitySubscriptionId) && !isEdit)
            {
                if (new EntityAcademicDetailDAO().Read(connectionString, new EntityAcademicDetailModel() { EntitySubscriptionId = entityAcademicDetailModel.EntitySubscriptionId }, true, true).Count > 0)
                {
                    responseModel.Valid = false;
                    responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_academic_details_entity_subscription_id_already_exists"]));
                    return responseModel;
                }
            }

            if (string.IsNullOrWhiteSpace(entityAcademicDetailModel.DaysToClosePeriod))
            {
                responseModel.Valid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_academic_details_number_wrong"]));
                return responseModel;
            }
            else
            {
                int numberOut = 0;
                bool isNumber = int.TryParse(entityAcademicDetailModel.DaysToClosePeriod, out numberOut); //i now = 108  
                if (!isNumber)
                {
                    responseModel.Valid = false;
                    responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_academic_details_number_wrong"]));
                    return responseModel;
                }
            }

            if (string.IsNullOrWhiteSpace(entityAcademicDetailModel.PeriodQuantity))
            {
                responseModel.Valid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_academic_details_periods_wrong"]));
                return responseModel;
            }
            else
            {
                int numberOut = 0;
                bool isNumber = int.TryParse(entityAcademicDetailModel.PeriodQuantity, out numberOut); //i now = 108  
                if (!isNumber)
                {
                    responseModel.Valid = false;
                    responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_academic_details_periods_wrong"]));
                    return responseModel;
                }
            }

            if (includeEntitySubscriptionId && string.IsNullOrWhiteSpace(entityAcademicDetailModel.EntitySubscriptionId))
            {
                responseModel.Valid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_academic_details_entity_null"]));
                return responseModel;
            }

            if (string.IsNullOrWhiteSpace(entityAcademicDetailModel.TermId))
            {
                responseModel.Valid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_academic_details_term_null"]));
                return responseModel;
            }

            // Validate date 
            if (string.IsNullOrWhiteSpace(entityAcademicDetailModel.FullTermStartDate))
            {
                responseModel.Valid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_academic_details_term_start_incorrect_date"]));
                return responseModel;
            }
            else
            {
                var validDate = ValidateDate(entityAcademicDetailModel.FullTermStartDate, false, connectionString, langCode);
                if (!validDate.Item1.Valid)
                {
                    responseModel.Valid = false;
                    responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_academic_details_term_start_incorrect_date"]));
                    return responseModel;
                }
            }

            try
            {
                DateTime dateFrom = DateTime.Parse(entityAcademicDetailModel.FullTermStartDate);
                DateTime dateTo = DateTime.Parse(entityAcademicDetailModel.FullTermEndDate);
                if(dateFrom >= dateTo)
                {
                    responseModel.Valid = false;
                    responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_academic_details_term_date_invalids"]));
                    return responseModel;
                }
            }
            catch(Exception)
            {
                responseModel.Valid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_academic_details_term_date_invalids"]));
                return responseModel;
            }

            if (string.IsNullOrWhiteSpace(entityAcademicDetailModel.FullTermEndDate))
            {
                responseModel.Valid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_academic_details_term_end_incorrect_date"]));
                return responseModel;
            }
            else
            {
                var validDate = ValidateDate(entityAcademicDetailModel.FullTermEndDate, false, connectionString, langCode);
                if (!validDate.Item1.Valid)
                {
                    responseModel.Valid = false;
                    responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_academic_details_term_end_incorrect_date"]));
                    return responseModel;
                }
            }

            return responseModel;
        }

        /// <summary>
        /// To validate the school academic details.
        /// </summary>
        /// <param name="academicDetailRequest">Academic detail to validate.</param>
        /// <param name="isEdit">If it is edit or not.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="langCode">The language code to return to the user the details.</param>
        /// <returns>Returns the response model with the validation details.</returns>
        public static ResponseModel ValidateSchoolAcademicDetails(AcademicDetailRequestModel academicDetailRequest, bool isEdit, string connectionString, string langCode)
        {
            var responseModel = new ResponseModel()
            {
                Valid = true,
                MessageCodes = new List<string>(),
                Messages = new List<string>(),
            };

            var messageManager = new MessageManager(connectionString, langCode);

            if (academicDetailRequest.AcademicDetail == null || academicDetailRequest.AcademicPeriods == null || academicDetailRequest.AcademicPeriods.Count == 0)
            {
                responseModel.Valid = false;
                return responseModel;
            }

            if (isEdit && string.IsNullOrWhiteSpace(academicDetailRequest.AcademicDetail.Id))
            {
                responseModel.Valid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:entity_academic_details_id_required"]));
                return responseModel;
            }

            if (string.IsNullOrWhiteSpace(academicDetailRequest.AcademicDetail.DaysToClosePeriod))
            {
                responseModel.Valid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:entity_academic_details_number_wrong"]));
                return responseModel;
            }
            else
            {
                int numberOut = 0;
                bool isNumber = int.TryParse(academicDetailRequest.AcademicDetail.DaysToClosePeriod, out numberOut); //i now = 108  
                if (!isNumber)
                {
                    responseModel.Valid = false;
                    responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:entity_academic_details_number_wrong"]));
                    return responseModel;
                }
            }

            if (string.IsNullOrWhiteSpace(academicDetailRequest.AcademicDetail.PeriodQuantity))
            {
                responseModel.Valid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:entity_academic_details_periods_wrong"]));
                return responseModel;
            }
            else
            {
                int numberOut = 0;
                bool isNumber = int.TryParse(academicDetailRequest.AcademicDetail.PeriodQuantity, out numberOut); //i now = 108  
                if (!isNumber)
                {
                    responseModel.Valid = false;
                    responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:entity_academic_details_periods_wrong"]));
                    return responseModel;
                }
            }

            if (string.IsNullOrWhiteSpace(academicDetailRequest.AcademicDetail.TermId))
            {
                responseModel.Valid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:entity_academic_details_term_null"]));
                return responseModel;
            }

            // Validate date 
            if (string.IsNullOrWhiteSpace(academicDetailRequest.AcademicDetail.FullTermStartDate))
            {
                responseModel.Valid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:entity_academic_details_term_start_incorrect_date"]));
                return responseModel;
            }
            else
            {
                var validDate = ValidateDate(academicDetailRequest.AcademicDetail.FullTermStartDate, !isEdit, connectionString, langCode);
                if (!validDate.Item1.Valid)
                {
                    responseModel.Valid = false;
                    responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:entity_academic_details_term_start_incorrect_date"]));
                    return responseModel;
                }
            }

            try
            {
                DateTime dateFrom = DateTime.Parse(academicDetailRequest.AcademicDetail.FullTermStartDate);
                DateTime dateTo = DateTime.Parse(academicDetailRequest.AcademicDetail.FullTermEndDate);
                if (dateFrom >= dateTo)
                {
                    responseModel.Valid = false;
                    responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:entity_academic_details_term_date_invalids"]));
                    return responseModel;
                }
            }
            catch (Exception)
            {
                responseModel.Valid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:entity_academic_details_term_date_invalids"]));
                return responseModel;
            }

            if (string.IsNullOrWhiteSpace(academicDetailRequest.AcademicDetail.FullTermEndDate))
            {
                responseModel.Valid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:entity_academic_details_term_end_incorrect_date"]));
                return responseModel;
            }
            else
            {
                var validDate = ValidateDate(academicDetailRequest.AcademicDetail.FullTermEndDate, !isEdit, connectionString, langCode);
                if (!validDate.Item1.Valid)
                {
                    responseModel.Valid = false;
                    responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:entity_academic_details_term_end_incorrect_date"]));
                    return responseModel;
                }
            }

            // Validate the overlapping of the dates
            var overlappingDates = ValidateAcademicDetailOverlappingDates(academicDetailRequest, connectionString, langCode);
            if (!overlappingDates.Valid)
            {
                return overlappingDates;
            }

            return responseModel;
        }

        /// <summary>
        /// To validate the academic periods.
        /// </summary>
        /// <param name="academicPeriods">The academic periods lists to validate.</param>
        /// <param name="includeEntityAcademicDetailId">If included, the evaluations will check for duplicated already existing periods and if this field is null.</param>
        /// <param name="isEdit">If it is an edit.</param>
        /// <param name="connectionString">The connection string for messages.</param>
        /// <param name="langCode">The language code in the user interface.</param>
        /// <returns>Returns a response model if the periods are valid.</returns>
        public static ResponseModel ValidateSchoolPeriods(List<AcademicPeriodModel> academicPeriods, bool isEdit, bool includeEntityAcademicDetailId, string connectionString, string langCode)
        {
            ResponseModel responseModel = new ResponseModel()
            {
                Valid = true,
                MessageCodes = new List<string>(),
                Messages = new List<string>(),
            };

            MessageManager messageManager = new MessageManager(connectionString, langCode);

            // Check if period numbers are repeated. 
            List<string> periodNumbers = new List<string>();
            List<AcademicPeriodModel> academicPeriodModelsToCheckIfExists = new List<AcademicPeriodModel>();
            foreach (AcademicPeriodModel academicPeriod in academicPeriods)
            {
                if (isEdit && string.IsNullOrWhiteSpace(academicPeriod.Id))
                {
                    responseModel.Valid = false;
                    responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_academic_details_period_id_required"]));
                    return responseModel;
                }

                if (string.IsNullOrWhiteSpace(academicPeriod.EntityAcademicDetailId) && includeEntityAcademicDetailId)
                {
                    responseModel.Valid = false;
                    responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_academic_details_period_required"]));
                    return responseModel;
                }

                if (string.IsNullOrWhiteSpace(academicPeriod.PeriodNumber))
                {
                    responseModel.Valid = false;
                    responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_academic_details_period_required"]));
                    return responseModel;
                }

                if (periodNumbers.Contains(academicPeriod.PeriodNumber))
                {
                    responseModel.Valid = false;
                    responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_academic_details_period_cannot_be_duplicated"]));
                    return responseModel;
                }

                // Check the dates
                var dateFrom = ValidateDate(academicPeriod.PeriodFrom, false, connectionString, langCode);
                if (!dateFrom.Item1.Valid)
                {
                    responseModel.Valid = false;
                    responseModel.Messages = dateFrom.Item1.Messages;
                    return responseModel;
                }

                var dateTo = ValidateDate(academicPeriod.PeriodFrom, false, connectionString, langCode);
                if (!dateTo.Item1.Valid)
                {
                    responseModel.Valid = false;
                    responseModel.Messages = dateTo.Item1.Messages;
                    return responseModel;
                }

                // Parse date 
                try
                {
                    DateTime dateTimeFrom = DateTime.Parse(academicPeriod.PeriodFrom);
                    DateTime dateTimeTo = DateTime.Parse(academicPeriod.PeriodTo);

                    if (dateTimeFrom >= dateTimeTo)
                    {
                        responseModel.Valid = false;
                        responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_academic_period_period_dates_overlap"]));
                        return responseModel;
                    }
                }
                catch(Exception)
                {
                    responseModel.Valid = false;
                    responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_academic_period_period_dates_overlap"]));
                    return responseModel;
                }

                academicPeriodModelsToCheckIfExists.Add(new AcademicPeriodModel()
                {
                    EntityAcademicDetailId = academicPeriod.EntityAcademicDetailId,
                });
            }

            // Check if periods overlap 
            if (academicPeriods.Any(r => academicPeriods.Where(q => q != r).Any(q => DateTime.Parse(q.PeriodTo) >= DateTime.Parse(r.PeriodFrom) && DateTime.Parse(q.PeriodFrom) <= DateTime.Parse(r.PeriodTo))))
            {
                responseModel.Valid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_academic_period_period_dates_overlap"]));
                return responseModel;
            }

            // Check for existing
            if (academicPeriodModelsToCheckIfExists.Count > 0 && includeEntityAcademicDetailId)
            {
                List<AcademicPeriodModel> academicPeriodModelsExisting = new AcademicPeriodDAO().Read(connectionString, academicPeriodModelsToCheckIfExists);
                if (academicPeriodModelsExisting.Count > 0)
                {
                    foreach (AcademicPeriodModel academicPeriod in academicPeriods)
                    {
                        AcademicPeriodModel academicPeriodModelExisting = academicPeriodModelsExisting.Find(x => x.PeriodNumber == academicPeriod.PeriodNumber);

                        // The period alread exists
                        if (string.IsNullOrWhiteSpace(academicPeriod.Id) && academicPeriodModelExisting != null && academicPeriodModelExisting.Id != academicPeriod.Id)
                        {
                            // Code already exists
                            responseModel.Valid = false;
                            responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_academic_details_period_cannot_be_duplicated"]));
                            break;
                        }
                    }

                    // Only for creation, since the existing period al ready exists.
                    if (!isEdit)
                    {
                        academicPeriodModelsExisting.AddRange(academicPeriods);
                    }
                    else
                    {
                        // Update the existing periods for validation 
                        List<AcademicPeriodModel> academicPeriodsToValidate = new List<AcademicPeriodModel>();
                        foreach (AcademicPeriodModel academicPeriodExisting in academicPeriodModelsExisting)
                        {
                            AcademicPeriodModel academicPeriodToSubstitute = academicPeriods.Find(x => x.Id == academicPeriodExisting.Id);
                            if (academicPeriodToSubstitute != null)
                            {
                                academicPeriodToSubstitute.EntityAcademicDetailId = null;
                                academicPeriodsToValidate.Add(academicPeriodToSubstitute);
                            }
                            else
                            {
                                academicPeriodsToValidate.Add(academicPeriodExisting);
                            }
                        }
                        academicPeriodModelsExisting = academicPeriodsToValidate;
                    }

                    // If dates overlap
                    if (academicPeriodModelsExisting.Any(r => academicPeriodModelsExisting.Where(q => q != r).Any(q => DateTime.Parse(q.PeriodTo) >= DateTime.Parse(r.PeriodFrom) && DateTime.Parse(q.PeriodFrom) <= DateTime.Parse(r.PeriodTo))))
                    {
                        responseModel.Valid = false;
                        responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_academic_period_period_dates_overlap"]));
                    }
                }
            }

            return responseModel;
        }

        /// <summary>
        /// To validate if a date is valid.
        /// </summary>
        /// <param name="date">The date to validate.</param>
        /// <param name="requiredToBeMoreThanToday">If it is required that the date is more than today.</param>
        /// <param name="messageManagerConnectionString">The connection string.</param>
        /// <param name="languageCode">Language Code of the client interface to return a message accoerding to that language.</param>
        /// <returns>Returns a Tuple with a ResponseModel as T1, and the date into a MySQL formar as T2.</returns>
        public static Tuple<ResponseModel, string> ValidateDate(string date, bool requiredToBeMoreThanToday, string messageManagerConnectionString, string languageCode)
        {
            MessageManager messageManager = new MessageManager(messageManagerConnectionString, languageCode);

            ResponseModel responseModel = new ResponseModel()
            {
                Valid = true,
                MessageCodes = new List<string>(),
                Messages = new List<string>(),
            };

            DateTime temp;
            if (!DateTime.TryParse(date, out temp))
            {
                responseModel.Valid = false;
                responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_message_code_error_invalid_date"]));
                return Tuple.Create(responseModel, string.Empty);
            }

            DateTime dateTimeToInsert = DateTime.Parse(date);
            if (requiredToBeMoreThanToday)
            {
                if (dateTimeToInsert.Date < DateTime.Now.Date)
                {
                    responseModel.Valid = false;
                    responseModel.Messages.Add(messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_message_code_error_invalid_date"]));
                    return Tuple.Create(responseModel, string.Empty);
                }
            }

            return Tuple.Create(responseModel, dateTimeToInsert.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        /// <summary>
        /// Validates the entity to create or update.
        /// </summary>
        /// <param name="connectionString">The connection string to return any message.</param>
        /// <param name="langCode">The language code of the user to return the message according to the user's language.</param>
        /// <param name="entity">The entity to insert or modify.</param>
        /// <param name="isUpdate">true if it is an update, else false.</param>
        /// <returns>Returns the response details according to validations' results.</returns>
        public static ResponseModel ValidateEntity(string connectionString, string langCode, EntityModifierModel entity, bool isUpdate)
        {
            ResponseModel response = new ResponseModel()
            {
                Valid = true
            };

            if (entity == null)
            {
                response.Valid = false;
                response.MessageCodes.Add(AppConfiguration.Configuration["AppSettings:sch_entity_require"]);
            }
            else
            {
                if (entity.Entities == null)
                {
                    response.Valid = false;
                    response.MessageCodes.Add(AppConfiguration.Configuration["AppSettings:sch_entity_require"]);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(entity.Entities[0].Token))
                    {
                        // Validate if code exists 
                        // First, select the entity by token
                        EntityModel entityToCheckIfTokenExists = new EntityModel()
                        {
                            Token = entity.Entities[0].Token,
                        };
                        List<EntityModel> entityModelToCheckIfExistsList = new EntityDAO().Read(connectionString, entityToCheckIfTokenExists, true, false);

                        if (entityModelToCheckIfExistsList.Count > 0)
                        {
                            if (isUpdate && entity.Entities[0].Id != entityModelToCheckIfExistsList[0].Id)
                            {
                                response.Valid = false;
                                response.MessageCodes.Add(AppConfiguration.Configuration["AppSettings:sch_entity_code_exists"]);
                            }
                            else if (!isUpdate)
                            {
                                response.Valid = false;
                                response.MessageCodes.Add(AppConfiguration.Configuration["AppSettings:sch_entity_code_exists"]);
                            }
                        }
                    }


                    if (string.IsNullOrWhiteSpace(entity.Entities[0].Id) && isUpdate)
                    {
                        response.Valid = false;
                        response.MessageCodes.Add(AppConfiguration.Configuration["AppSettings:sch_entity_id_require"]);
                    }

                    if (string.IsNullOrWhiteSpace(entity.Entities[0].Name))
                    {
                        response.Valid = false;
                        response.MessageCodes.Add(AppConfiguration.Configuration["AppSettings:sch_entity_name_require"]);
                    }
                }
            }

            if (response.MessageCodes.Count > 0)
                response.Messages = new MessageManager(connectionString, langCode).GetMessageDetails(response.MessageCodes);

            return response;
        }

        /// <summary>
        /// To clean the phones and put only numbers.
        /// </summary>
        /// <param name="phone">The phone to clean.</param>
        /// <returns>Returns the cleaned phone.</returns>
        public static string CleanPhone(string phone)
        {
            Regex digitsOnly = new Regex(@"[^\d]");
            return digitsOnly.Replace(phone, "");
        }

        /// <summary>
        /// Validate the address.
        /// </summary>
        /// <param name="connectionString">Connection string to return any message.</param>
        /// <param name="langCode">The language of the user.</param>
        /// <param name="address">The address to validate.</param>
        /// <returns>Returns a response with the validation's results.</returns>
        public static ResponseModel ValidateAddress(string connectionString, string langCode, AddressModel address)
        {
            ResponseModel response = new ResponseModel()
            {
                Valid = true
            };

            if(string.IsNullOrWhiteSpace(address.City))
            {
                // Please, add the city
                response.Valid = false;
                response.MessageCodes.Add(AppConfiguration.Configuration["AppSettings:sch_entity_address_must_have_city"]);
            }

            if (string.IsNullOrWhiteSpace(address.AddressLine1))
            {
                // Please, add the address line 1
                response.Valid = false;
                response.MessageCodes.Add(AppConfiguration.Configuration["AppSettings:sch_entity_address_must_have_address_line_1"]);
            }

            if (string.IsNullOrWhiteSpace(address.Zip))
            {
                // Please, add the zip code
                response.Valid = false;
                response.MessageCodes.Add(AppConfiguration.Configuration["AppSettings:sch_entity_address_must_have_zip_code"]);
            }

            if (string.IsNullOrWhiteSpace(address.Country))
            {
                // Please, add the Country
                response.Valid = false;
                response.MessageCodes.Add(AppConfiguration.Configuration["AppSettings:sch_entity_address_must_have_country"]);
            }

            if (string.IsNullOrWhiteSpace(address.EntityId))
            {
                // Please, add the entity id
                response.Valid = false;
                response.MessageCodes.Add(AppConfiguration.Configuration["AppSettings:sch_entity_address_must_have_entity_id"]);
            }

            if (response.MessageCodes.Count > 0)
                response.Messages = new MessageManager(connectionString, langCode).GetMessageDetails(response.MessageCodes);

            return response;
        }

        /// <summary>
        /// Validates the relation between entities. 
        /// </summary>
        /// <param name="connectionString">Connection string to return any message.</param>
        /// <param name="langCode">The language of the user.</param>
        /// <param name="relation">The relation to validate.</param>
        /// <param name="isDelete">True if delete, else false.</param>
        /// <returns>Returns a response with the validation's results.</returns>
        public static ResponseModel ValidateRelation(string connectionString, string langCode, RelationModel relation, bool isDelete)
        {
            ResponseModel response = new ResponseModel()
            {
                Valid = true
            };

            if (isDelete && (string.IsNullOrWhiteSpace(relation.RelationEntityId)
                    || string.IsNullOrWhiteSpace(relation.EntityId)))
            {
                response.Valid = false;
                response.MessageCodes.Add(AppConfiguration.Configuration["AppSettings:sch_entity_id_require"]);
            }
            else if (!isDelete && (string.IsNullOrWhiteSpace(relation.RelationEntityId)
                    || string.IsNullOrWhiteSpace(relation.EntityId)
                    || string.IsNullOrWhiteSpace(relation.TypeId)))
            {
                // Cannot delete it because there is no relation specified
                response.Valid = false;
                response.MessageCodes.Add(AppConfiguration.Configuration["AppSettings:sch_entity_id_require"]);
            }
            if (response.MessageCodes.Count > 0)
                response.Messages = new MessageManager(connectionString, langCode).GetMessageDetails(response.MessageCodes);

            return response;
        }

        /// <summary>
        /// To validate the phone.
        /// </summary>
        /// <param name="connectionString">Connection string to return any message.</param>
        /// <param name="langCode">The language of the user.</param>
        /// <param name="phone">The phone to validate.</param>
        /// <returns>Returns a response with the validation's results.</returns>
        public static ResponseModel ValidatePhone(string connectionString, string langCode, PhoneModel phone)
        {
            if (phone != null && !string.IsNullOrWhiteSpace(phone.Phone) && !string.IsNullOrWhiteSpace(phone.EntityId))
            {
                return new ResponseModel() { Valid = true };
            }
            else
            {
                ResponseModel response = new ResponseModel()
                {
                    Valid = false,
                    MessageCodes = new List<string> { AppConfiguration.Configuration["AppSettings:sch_entity_phone_seems_invalid"] }
                };
                response.Messages = new MessageManager(connectionString, langCode).GetMessageDetails(response.MessageCodes);
                return response;
            }
        }
    }
}
