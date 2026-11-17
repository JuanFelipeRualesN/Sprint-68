using System;
using System.Collections.Generic;
using System.Configuration;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools.Request;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools.Response;
using SchoolsAPI.SchoolsAPI.Infrastructure.Repositories.Schools;
using SchoolsAPI.SchoolsAPI.Services.Schools.CustomClasses;
using SchoolsAPI.SchoolsAPI.Services.Generals;

namespace SchoolsAPI.SchoolsAPI.Services.Schools
{
    /// <summary>
    /// To handle the schools' details.
    /// </summary>
    public class SchoolModifierManager
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;
        private readonly string _langCode;
        private readonly Random _random;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">The connection string (write).</param>
        /// <param name="connectionStringRead">The connection string (read).</param>
        /// <param name="langCode">The language code to return to the user the messages from the transactions.</param>
        public SchoolModifierManager(string connectionString, string connectionStringRead, string langCode)
        {
            _connectionString = connectionString;
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
            _random = new Random();
        }

        /// <summary>
        /// To create or modify an entity (an entity is a school, program, etc).
        /// </summary>
        /// <param name="entityModify">The entity to modify or to create.</param>
        /// <param name="userModifiedBy">The user that is modifying or creating the row.</param>
        /// <returns>Returns the transaction's details.</returns>
        public EntityResponseModel ModifyEntity(EntityModifierModel entityModify, string userModifiedBy)
        {
            ResponseModel responseValidation = new ResponseModel();
            if (string.IsNullOrWhiteSpace(entityModify.Entities[0].Id))
            {
                responseValidation = Validator.ValidateEntity(_connectionStringRead, _langCode, entityModify, false);
            }
            else
            {
                responseValidation = Validator.ValidateEntity(_connectionStringRead, _langCode, entityModify, true);
            }
            if (!responseValidation.Valid)
            {
                return new EntityResponseModel() { Valid = responseValidation.Valid, Messages = responseValidation.Messages };
            }

            // Now create entity 
            EntityDAO entityDAO = new EntityDAO();
            entityModify.Entities[0].ModifiedBy = userModifiedBy;
            if (string.IsNullOrWhiteSpace(entityModify.Entities[0].Id))
            {
                if (string.IsNullOrWhiteSpace(entityModify.Entities[0].Token))
                    entityModify.Entities[0].Token = GenerateTokenManager();
                entityModify.Entities[0] = entityDAO.Create(_connectionString, entityModify.Entities[0])[0];
            }
            else
            {
                string entityId = entityModify.Entities[0].Id;
                entityModify.Entities[0] = entityDAO.Update(_connectionString, new EntityModel()
                {
                    Name = entityModify.Entities[0].Name,
                    Token = entityModify.Entities[0].Token,
                    EntityTypeId = entityModify.Entities[0].EntityTypeId,
                    Private = entityModify.Entities[0].Private,
                    ModifiedBy = userModifiedBy
                }, new EntityModel()
                {
                    Id = entityModify.Entities[0].Id
                })[0];
                entityModify.Entities[0].Id = entityId; // Apart, for security reasons

            }

            // Now create the address if any 
            if (entityModify.Addresses != null && entityModify.Addresses.Count > 0)
            {
                // Evaluates the ones to create 
                List<AddressModel> addressesToCreate = new List<AddressModel>();
                foreach (AddressModel address in entityModify.Addresses)
                {
                    ResponseModel responseAddress = new ResponseModel();
                    address.EntityId = entityModify.Entities[0].Id;
                    responseAddress = Validator.ValidateAddress(_connectionStringRead, _langCode, address);
                    if (responseAddress.Valid)
                    {
                        address.ModifiedBy = userModifiedBy;
                        address.CreatedAt = null;
                        address.UpdatedAt = null;
                        addressesToCreate.Add(address);
                    }
                }

                if (addressesToCreate.Count > 0)
                    new AddressDAO().Create(_connectionString, addressesToCreate);
            }

            // Same with phone
            if (entityModify.Phones != null && entityModify.Phones.Count > 0)
            {
                // Evaluates the ones to create 
                List<PhoneModel> phonesToCreate = new List<PhoneModel>();
                foreach (PhoneModel phone in entityModify.Phones)
                {
                    phone.EntityId = entityModify.Entities[0].Id;
                    ResponseModel responsePhone = Validator.ValidatePhone(_connectionStringRead, _langCode, phone);
                    if (responsePhone.Valid)
                    {
                        phone.Phone = Validator.CleanPhone(phone.Phone);
                        phone.ModifiedBy = userModifiedBy;
                        phone.CreatedAt = null;
                        phone.UpdatedAt = null;
                        phonesToCreate.Add(phone);
                    }
                }

                if (phonesToCreate.Count > 0)
                    new PhoneDAO().Create(_connectionString, phonesToCreate);
            }

            // Same for relations 
            if (entityModify.Relations != null && entityModify.Relations.Count > 0)
            {
                // Evaluates the ones to create 
                List<RelationModel> relationsToCreate = new List<RelationModel>();
                foreach (RelationModel relation in entityModify.Relations)
                {
                    relation.EntityId = entityModify.Entities[0].Id;
                    // Validate 
                    ResponseModel responseEntity = new ResponseModel();
                    responseEntity = Validator.ValidateRelation(_connectionStringRead, _langCode, relation, false);

                    if (responseEntity.Valid)
                        relationsToCreate.Add(relation);
                }

                if (relationsToCreate.Count > 0)
                    new RelationDAO().Create(_connectionString, entityModify.Relations);
            }

            // Return message transaction ssuccess
            EntityResponseModel responseToReturnFinal = new EntityResponseModel
            {
                Valid = true
            };
            responseToReturnFinal.Messages.Add(new MessageManager(_connectionString, _langCode).
                GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_success"]));

            responseToReturnFinal.Entities = new List<EntityModifierModel>();
            EntityModifierModel entityModifierModel = new EntityModifierModel()
            {
                Entities = new List<EntityModel>()
                {
                    entityModify.Entities[0]
                }
            };
            responseToReturnFinal.Entities.Add(entityModifierModel);
            return responseToReturnFinal;
        }

        /// <summary>
        /// To create or modify the entity's address. 
        /// </summary>
        /// <param name="address">The address to modify or to create.</param>
        /// <param name="userModifiedBy">The user that is modifying or creating the row.</param>
        /// <returns>Returns the tranaction's details.</returns>
        public ResponseModel ModifyAddress(AddressModel address, string userModifiedBy)
        {
            ResponseModel responseValidation = Validator.ValidateAddress(_connectionStringRead, _langCode, address);
            if (!responseValidation.Valid)
                return responseValidation;
            else
            {
                if (!string.IsNullOrWhiteSpace(address.Id))
                {
                    // Update 
                    AddressModel addressModelWhere = new AddressModel()
                    {
                        Id = address.Id,
                        EntityId = address.EntityId
                    };

                    address.ModifiedBy = userModifiedBy;
                    address.CreatedAt = null;
                    address.UpdatedAt = null;
                    new AddressDAO().Update(_connectionString, address, addressModelWhere);
                    return new ResponseModel()
                    {
                        Valid = true,
                        Messages = new List<string>()
                    {
                        new MessageManager(_connectionString, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_success"])}
                    };
                }
                else
                {
                    // Insert the new address
                    address.ModifiedBy = userModifiedBy;
                    address.CreatedAt = null;
                    address.UpdatedAt = null;
                    new AddressDAO().Create(_connectionString, address);
                    return new ResponseModel()
                    {
                        Valid = true,
                        Messages = new List<string>()
                    {
                        new MessageManager(_connectionString, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_success"])}
                    };
                }
            }
        }

        /// <summary>
        /// To delete an entity's address.
        /// </summary>
        /// <param name="address">The address model to delete.</param>
        /// <returns>Returns the transaction's details.</returns>
        public ResponseModel DeleteAddress(AddressModel address)
        {
            if (!string.IsNullOrWhiteSpace(address.Id))
            {
                new AddressDAO().Delete(_connectionString, new AddressModel()
                {
                    Id = address.Id
                });

                return new ResponseModel()
                {
                    Valid = true,
                    Messages = new List<string>()
                    {
                        new MessageManager(_connectionString, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_success"])}
                };
            }
            else
            {
                return new ResponseModel()
                {
                    Valid = false,
                    Messages = new List<string>()
                    {
                        new MessageManager(_connectionString, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_id_require"])}
                };
            }
        }

        /// <summary>
        /// To create or update the entity's phone.
        /// </summary>
        /// <param name="phone">The phone model to add.</param>
        /// <param name="userModifiedBy">The user that is modifying or creating the row.</param>
        /// <returns>Returns the transaction's details.</returns>
        public ResponseModel ModifyPhone(PhoneModel phone, string userModifiedBy)
        {
            ResponseModel responseValidation = Validator.ValidatePhone(_connectionStringRead, _langCode, phone);
            if (!responseValidation.Valid)
                return responseValidation;
            else
            {
                phone.Phone = Validator.CleanPhone(phone.Phone);

                if (!string.IsNullOrWhiteSpace(phone.Id))
                {
                    // Update 
                    PhoneModel phoneModelWhere = new PhoneModel()
                    {
                        Id = phone.Id,
                        EntityId = phone.EntityId
                    };

                    phone.ModifiedBy = userModifiedBy;
                    phone.CreatedAt = null;
                    phone.UpdatedAt = null;
                    new PhoneDAO().Update(_connectionString, phone, phoneModelWhere);
                    return new ResponseModel()
                    {
                        Valid = true,
                        Messages = new List<string>()
                    {
                        new MessageManager(_connectionString, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_success"])}
                    };
                }
                else
                {
                    // Insert the new address
                    phone.ModifiedBy = userModifiedBy;
                    phone.CreatedAt = null;
                    phone.UpdatedAt = null;
                    new PhoneDAO().Create(_connectionString, phone);
                    return new ResponseModel()
                    {
                        Valid = true,
                        Messages = new List<string>()
                    {
                        new MessageManager(_connectionString, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_success"])}
                    };
                }
            }
        }

        /// <summary>
        /// To delete an entity's phone.
        /// </summary>
        /// <param name="phone">The phone model to delete.</param>
        /// <returns>Returns the transaction's details.</returns>
        public ResponseModel DeletePhone(PhoneModel phone)
        {
            if (!string.IsNullOrWhiteSpace(phone.Id))
            {
                new PhoneDAO().Delete(_connectionString, new PhoneModel()
                {
                    Id = phone.Id
                });

                return new ResponseModel()
                {
                    Valid = true,
                    Messages = new List<string>()
                    {
                        new MessageManager(_connectionString, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_success"])}
                };
            }
            else
            {
                return new ResponseModel()
                {
                    Valid = false,
                    Messages = new List<string>()
                    {
                        new MessageManager(_connectionString, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_id_require"])}
                };
            }
        }

        /// <summary>
        /// To add a relation between the entities. For example, a school and a program.
        /// </summary>
        /// <param name="relationModel">The relation model to add.</param>
        /// <param name="userModifiedBy">The user that is modifying the record.</param>
        /// <returns>Returns the response model with the transaction details.</returns>
        public ResponseModel AddRelation(RelationModel relationModel, string userModifiedBy)
        {
            if (string.IsNullOrWhiteSpace(relationModel.EntityId) || string.IsNullOrWhiteSpace(relationModel.RelationEntityId))
            {
                // Wrong 
                return new ResponseModel()
                {
                    Valid = false,
                    Messages = new List<string>()
                    {
                        new MessageManager(_connectionString, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_id_require"])}
                };
            }
            else
            {
                relationModel.ModifiedBy = userModifiedBy;
                relationModel.CreatedAt = null;
                relationModel.UpdatedAt = null;
                new RelationDAO().Create(_connectionString, relationModel);
                ResponseModel responseToReturnFinal = new ResponseModel
                {
                    Valid = true
                };
                responseToReturnFinal.Messages.Add(new MessageManager(_connectionString, _langCode).
                    GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_success"]));

                return responseToReturnFinal;
            }
        }

        /// <summary>
        /// To delete a relation between entities.
        /// </summary>
        /// <param name="relationModel">Relation model to delete</param>
        /// <returns>Returns a response with the transaction details. </returns>
        public ResponseModel DeleteRelation(RelationModel relationModel)
        {
            ResponseModel responseValid = Validator.ValidateRelation(_connectionStringRead, _langCode, relationModel, true);

            if (responseValid.Valid)
            {
                new RelationDAO().Delete(_connectionString, relationModel);
                // Return message transaction ssuccess
                ResponseModel responseToReturnFinal = new ResponseModel
                {
                    Valid = true
                };
                responseToReturnFinal.Messages.Add(new MessageManager(_connectionString, _langCode).
                    GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_success"]));

                return responseToReturnFinal;
            }
            else
                return responseValid;
        }

        /// <summary>
        /// To create a relation between the entity and the subscription.
        /// </summary>
        /// <param name="entitySubscription">Entity subscription to create.</param>
        /// <param name="userModifiedBy">The user that is modifying the record.</param>
        /// <returns>Returns a HTTP Response Model and entity subscription created.</returns>
        public EntitySubscriptionModel CreateEntitySubscription(EntitySubscriptionModel entitySubscription, string userModifiedBy)
        {
            if (entitySubscription == null || string.IsNullOrWhiteSpace(entitySubscription.EntityId) || string.IsNullOrWhiteSpace(entitySubscription.SubscriptionId))
            {
                // Return message transaction ssuccess
                return new EntitySubscriptionModel()
                {
                    Valid = false,
                    Messages = new List<string>()
                    {
                        new MessageManager(_connectionString, _langCode).
                    GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_subscriptions_required"]),
                    }
                };
            }
            else
            {
                entitySubscription.ModifiedBy = userModifiedBy;
                entitySubscription = new EntitySubscriptionDAO().Create(_connectionString, entitySubscription)[0];
                entitySubscription.Valid = true;
                entitySubscription.Messages = new List<string>()
                {
                    new MessageManager(_connectionString, _langCode).
                        GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_success"])
                };

                return entitySubscription;

            }
        }

        /// <summary>
        /// To create the entity's academic details. The academic details are the academic start date, end date, the terms' details.
        /// </summary>
        /// <param name="entityAcademicDetailModel">Entity academic detail to model to create.</param>
        /// <param name="userModifiedBy">The user that is modifying the records.</param>
        /// <returns>Returns a response with the transaction's details and the academic details.</returns>
        public EntityAcademicDetailModel CreateEntityAcademicDetails(EntityAcademicDetailModel entityAcademicDetailModel, string userModifiedBy)
        {
            entityAcademicDetailModel.ModifiedBy = userModifiedBy;
            ResponseModel responseModel = Validator.ValidateSchoolAcademicDetails(entityAcademicDetailModel, false, false, _connectionStringRead, _langCode);

            if (!responseModel.Valid)
            {
                return new EntityAcademicDetailModel()
                {
                    Valid = false,
                    Messages = responseModel.Messages,
                };
            }
            else
            {
                entityAcademicDetailModel.FullTermEndDate = DateConverter.ConvertDateTimeToMySQLFormat(entityAcademicDetailModel.FullTermEndDate);
                entityAcademicDetailModel.FullTermStartDate = DateConverter.ConvertDateTimeToMySQLFormat(entityAcademicDetailModel.FullTermStartDate);
                EntityAcademicDetailModel entityAcademicModelCreated = new EntityAcademicDetailDAO().Create(_connectionString, entityAcademicDetailModel)[0];
                entityAcademicModelCreated.Messages = new List<string>()
                {
                    new MessageManager(_connectionString, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_academic_details_success"])
                };
                entityAcademicModelCreated.Valid = true;
                return entityAcademicModelCreated;
            }
        }

        /// <summary>
        /// To update the entity's academic details. The academic details are the academic start date, end date, the terms' details.
        /// </summary>
        /// <param name="entityAcademicDetailModel">Entity academic detail to model to modify.</param>
        /// <param name="userModifiedBy">The user that is modifying the records.</param>
        /// <returns>Returns a response with the transaction's details and the academic details.</returns>
        public EntityAcademicDetailModel UpdateEntityAcademicDetails(EntityAcademicDetailModel entityAcademicDetailModel, string userModifiedBy)
        {

            EntityAcademicDetailModel entityAcademicDetailToReturn = new EntityAcademicDetailDAO().Update(_connectionString, new EntityAcademicDetailModel()
            {
                DaysToClosePeriod = entityAcademicDetailModel.DaysToClosePeriod,
                FullTermEndDate = DateConverter.ConvertDateTimeToMySQLFormat(entityAcademicDetailModel.FullTermEndDate),
                FullTermStartDate = DateConverter.ConvertDateTimeToMySQLFormat(entityAcademicDetailModel.FullTermStartDate),
                PeriodQuantity = entityAcademicDetailModel.PeriodQuantity,
                TermId = entityAcademicDetailModel.TermId,

            }, new EntityAcademicDetailModel()
            {
                Id = entityAcademicDetailModel.Id,
                ModifiedBy = userModifiedBy
            })[0];

            entityAcademicDetailToReturn.Valid = true;
            entityAcademicDetailToReturn.Messages = new List<string>
                {
                    new MessageManager(_connectionString, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_academic_details_success"])
                };
            return entityAcademicDetailToReturn;

        }

        /// <summary>
        /// To create academic periods.
        /// </summary>
        /// <param name="academicPeriods">Academic periods list to create.</param>
        /// <param name="userModifiedBy">The user in session.</param>
        /// <returns>Returns the transaction's details.</returns>
        public ResponseModel CreateAcademicPeriods(List<AcademicPeriodModel> academicPeriods, string userModifiedBy)
        {
            foreach (AcademicPeriodModel academicPeriod in academicPeriods)
            {
                academicPeriod.PeriodFrom = DateConverter.ConvertDateTimeToMySQLFormat(academicPeriod.PeriodFrom);
                academicPeriod.PeriodTo = DateConverter.ConvertDateTimeToMySQLFormat(academicPeriod.PeriodTo);
                academicPeriod.ModifiedBy = userModifiedBy;
            }
            new AcademicPeriodDAO().Create(_connectionString, academicPeriods);
            return new ResponseModel()
            {
                Valid = true,
                Messages = new List<string>()
                {
                    new MessageManager(_connectionString, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_academic_period_success"])
                }
            };
        }

        /// <summary>
        /// To update a list of academic periods.
        /// </summary>
        /// <param name="academicPeriods">The academic period list to update.</param>
        /// <param name="userModifiedBy">The user that is modifying the records.</param>
        /// <returns>Returns the transaction's details.</returns>
        public ResponseModel UpdateAcademicPeriods(List<AcademicPeriodModel> academicPeriods, string userModifiedBy)
        {
            List<AcademicPeriodModel> academicPeriodToUpdateList = new List<AcademicPeriodModel>();
            List<AcademicPeriodModel> academemicPeriodWhereList = new List<AcademicPeriodModel>();
            foreach (AcademicPeriodModel academicPeriod in academicPeriods)
            {
                academicPeriodToUpdateList.Add(new AcademicPeriodModel()
                {
                    PeriodFrom = DateConverter.ConvertDateTimeToMySQLFormat(academicPeriod.PeriodFrom),
                    PeriodTo = DateConverter.ConvertDateTimeToMySQLFormat(academicPeriod.PeriodTo),
                    ModifiedBy = userModifiedBy,
                    PeriodNumber = academicPeriod.PeriodNumber
                });

                academemicPeriodWhereList.Add(new AcademicPeriodModel()
                {
                    Id = academicPeriod.Id,
                });
            }

            new AcademicPeriodDAO().Update(_connectionString, academicPeriodToUpdateList, academemicPeriodWhereList);
            return new ResponseModel()
            {
                Valid = true,
                Messages = new List<string>()
                {
                    new MessageManager(_connectionString, _langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_academic_period_success"])
                }
            };
        }

        /// <summary>
		/// Generates a Token. It is unique because it use dates and some characters are modified
		/// </summary>
		/// <returns></returns>
        public string GenerateTokenManager()
        {
            // Must be unique, so we will work with the date and some random letters
            // No 0s or Os or Ls or Is or Zs to avoid confusion between characters.
            // "A" will be used to replace 0s
            const string chars = "BCDEFGHJKMNPQRSTUVWXY";
            var stringChars = new char[5];
            string randomStr = "";
            for (int i = 0; i < stringChars.Length; i++)
            {
                randomStr += chars[_random.Next(chars.Length)];
            }

            DateTime dateTimeNow = DateTime.Now;
            string dateStr = dateTimeNow.ToString("ddMMyyHHss");
            dateStr = dateStr.Replace("0", "X");

            string returnString = "";
            for (int i = 0; i < dateStr.Length; i++)
            {
                if (i == 0)
                {
                    returnString += randomStr[0];
                }
                else if (i == 2)
                {
                    returnString += randomStr[2];
                }
                else if (i == 5)
                {
                    returnString += randomStr[3];
                }
                else
                {
                    returnString += dateStr[i];
                }
            }
            return returnString;
        }
    }
}
