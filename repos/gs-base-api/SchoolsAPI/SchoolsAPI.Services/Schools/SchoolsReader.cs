using System;
using System.Collections.Generic;
using System.Linq;
using SchoolsAPI.SchoolsAPI.Infrastructure.Repositories.Schools;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools.Response;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools.Request;

namespace SchoolsAPI.SchoolsAPI.Services.Schools
{
    /// <summary>
    /// To read the details about the entities (schools, programs, etc).
    /// </summary>
    public class SchoolsReader
    {
        private readonly string _connectionStringRead, _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionStringRead">The connection string (read).</param>
        /// <param name="langCode">The language code to return the messages to the user.</param>
        public SchoolsReader(string connectionStringRead, string langCode)
        {
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// To read the regions (ORE). An ORE/Region are the educational regions of the state/country.
        /// </summary>
        /// <param name="regionModel">The region model to filter.</param>
        /// <returns>Returns the region according to filters.</returns>
        public List<RegionModel> ReadRegions(RegionModel regionModel)
        {
            return new RegionDAO().Read(_connectionStringRead, regionModel, false, false);
        }

        /// <summary>
        /// Returns a list of entity subscription models by filtering it.
        /// </summary>
        /// <param name="subscriptionFilterList">The entity subscription model to filter.</param>
        /// <returns>Returns the list of entity subscriptions models.</returns>
        public List<EntitySubscriptionModel> GetEntitySubcriptionModelList(List<EntitySubscriptionModel> subscriptionFilterList)
        {
            List<EntitySubscriptionModel> subscriptionListResult = new List<EntitySubscriptionModel>();
            if (subscriptionFilterList != null && subscriptionFilterList.Count > 0)
            {
                List<EntitySubscriptionModel> subscriptionToSearchList = new List<EntitySubscriptionModel>();
                foreach (EntitySubscriptionModel subscription in subscriptionFilterList)
                {
                    if (!string.IsNullOrWhiteSpace(subscription.SubscriptionId))
                        subscription.SubscriptionId = subscription.SubscriptionId;
                    subscriptionToSearchList.Add(subscription);
                }
                subscriptionListResult = new EntitySubscriptionDAO().Read(_connectionStringRead, subscriptionToSearchList);
            }
            return subscriptionListResult;
        }

        /// <summary>
        /// To find the entity subscriptions by school or/and program model list (entity list).
        /// </summary>
        /// <param name="entityIds">The entities' ids.</param>
        /// <param name="schoolTypeId">The school type id.</param>
        /// <param name="programTypeId">The program type id.</param>
        /// <returns>Returns a list of entity subscriptions' models.</returns>
        public List<EntitySubscriptionModel> GetEntitySubcriptionBySchoolAndProgramModelList(List<string> entityIds, string schoolTypeId, string programTypeId)
        {

            // Identify the programs and the Schools 
            List<EntityModel> schooModelToFind = new List<EntityModel>();
            List<EntityModel> programModelToFind = new List<EntityModel>();

            foreach (string entitySubscription in entityIds)
            {
                schooModelToFind.Add(new EntityModel()
                {
                    Id = entitySubscription,
                    EntityTypeId = schoolTypeId
                });
                programModelToFind.Add(new EntityModel()
                {
                    Id = entitySubscription,
                    EntityTypeId = programTypeId
                });
            }

            EntityDAO entityDAO = new EntityDAO();
            List<EntityModel> schoolResult = entityDAO.Read(_connectionStringRead, schooModelToFind);
            List<EntityModel> programResult = entityDAO.Read(_connectionStringRead, programModelToFind);

            List<EntitySubscriptionModel> entitySubscriptionBySchoolToFindList = new List<EntitySubscriptionModel>();
            List<EntitySubscriptionModel> programSubscriptionSchoolToFindList = new List<EntitySubscriptionModel>();

            foreach (EntityModel school in schoolResult)
            {
                entitySubscriptionBySchoolToFindList.Add(new EntitySubscriptionModel()
                {
                    EntityId = school.Id
                });
            }

            foreach (EntityModel program in programResult)
            {
                programSubscriptionSchoolToFindList.Add(new EntitySubscriptionModel()
                {
                    EntityId = program.Id
                });
            }

            EntitySubscriptionDAO entitySubscriptionDAO = new EntitySubscriptionDAO();
            List<EntitySubscriptionModel> entitySchoolsSubscriptionSchools = entitySubscriptionDAO.Read(_connectionStringRead, entitySubscriptionBySchoolToFindList);
            List<EntitySubscriptionModel> programSubscriptionSchools = entitySubscriptionDAO.Read(_connectionStringRead, programSubscriptionSchoolToFindList);

            // Now return the entity the schools that appear in both
            List<EntitySubscriptionModel> entitySubscriptionModelListToReturn = new List<EntitySubscriptionModel>();
            foreach (EntitySubscriptionModel entitySubscriptionModel in entitySchoolsSubscriptionSchools)
            {
                if (programSubscriptionSchools.Count > 0)
                {
                    if (programSubscriptionSchools.Find(x => x.SubscriptionId == entitySubscriptionModel.SubscriptionId) != null)
                        entitySubscriptionModelListToReturn.Add(entitySubscriptionModel);
                }
                else
                    entitySubscriptionModelListToReturn.Add(entitySubscriptionModel);
            }

            return entitySubscriptionModelListToReturn;
        }

        /// <summary>
        /// To get the phone model's list of entities.
        /// </summary>
        /// <param name="phoneFilterList">The phones to filter.</param>
        /// <returns>Returns a list of phone models.</returns>
        public List<PhoneModel> GetPhoneModelList(List<PhoneModel> phoneFilterList)
        {
            List<PhoneModel> phoneModelListResult = new List<PhoneModel>();
            if (phoneFilterList != null && phoneFilterList.Count > 0)
            {
                // Remove phone symbols 
                foreach (PhoneModel phone in phoneFilterList)
                {
                    if (!string.IsNullOrWhiteSpace(phone.Phone))
                        phone.Phone = Validator.CleanPhone(phone.Phone);
                }
                phoneModelListResult = new PhoneDAO().Read(_connectionStringRead, phoneFilterList);
            }
            return phoneModelListResult;
        }

        /// <summary>
        /// To get the entities models (schools, programs, etc).
        /// </summary>
        /// <param name="entityModelToFilter">The entities list to filter.</param>
        /// <param name="orderByName">If order by name or not.</param>
        /// <returns>Returns a list of entities' models.</returns>
        public List<EntityModel> GetEntityModels(List<EntityModel> entityModelToFilter, bool orderByName)
        {
            List<EntityModel> entitiesList = new EntityDAO().Read(_connectionStringRead, entityModelToFilter);
            if (orderByName)
                return entitiesList.OrderBy(o => o.Name).ToList();
            else
                return entitiesList;
        }

        /// <summary>
        /// To get the entity details.
        /// </summary>
        /// <param name="entityFilterList">The entities' models to filter.</param>
        /// <param name="relationFilterList">The relations' models to filter.</param>
        /// <param name="phoneFilterList">The entities' phones to filter.</param>
        /// <param name="addressFilterList">The addresses to filter.</param>
        /// <param name="subscriptionFilterList">The entities' subscriptions to filter.</param>
        /// <param name="fromRow">From which row we want to call the results.</param>
        /// <param name="limit">How many items per list.</param>
        /// <returns>Returns the entity response with the results.</returns>
        public EntityResponseModel GetEntityDetails(
            List<EntityModel> entityFilterList,
            List<RelationModel> relationFilterList,
            List<PhoneModel> phoneFilterList,
            List<AddressModel> addressFilterList,
            List<EntitySubscriptionModel> subscriptionFilterList,
            int fromRow,
            int limit)
        {
            int totalPages = 0;
            if (limit == 0)
                limit = 10;

            // Search by phone, which is the best to be filtered
            List<PhoneModel> phoneModelListResult = new List<PhoneModel>();
            if (phoneFilterList != null && phoneFilterList.Count > 0)
                phoneModelListResult = GetPhoneModelList(phoneFilterList);

            // Next, search by Address
            List<AddressModel> addressListResult = new List<AddressModel>();
            if (addressFilterList != null && addressFilterList.Count > 0)
                addressListResult = new AddressDAO().Read(_connectionStringRead, addressFilterList);

            // Or search by subscription id 
            List<EntitySubscriptionModel> entitySubscriptionModelListResult = new List<EntitySubscriptionModel>();
            if (subscriptionFilterList != null && subscriptionFilterList.Count > 0)
                entitySubscriptionModelListResult = GetEntitySubcriptionModelList(subscriptionFilterList);

            // Search by relation 
            List<RelationModel> relationModelsResult = new List<RelationModel>();
            if (relationFilterList != null && relationFilterList.Count > 0)
                relationModelsResult = new RelationDAO().Read(_connectionStringRead, relationFilterList);

            // Filter by entity 
            List<EntityModel> entityModelListResult = new List<EntityModel>();
            if (entityFilterList != null && entityFilterList.Count > 0)
                entityModelListResult = new EntityDAO().Read(_connectionStringRead, entityFilterList);

            // Now prepare the official entities to search, and finally, grab the information to relate it. 

            // Call the entities in common: Order must be: Phone, subscription, relations, address, entity 
            List<EntityModel> entityToSearchFinalList = new List<EntityModel>();

            if (phoneModelListResult.Count > 0)
            {
                foreach (PhoneModel phone in phoneModelListResult)
                {
                    entityToSearchFinalList.Add(new EntityModel()
                    {
                        Id = phone.EntityId
                    });
                }
            }

            // Filter subscriptions
            if (entitySubscriptionModelListResult.Count > 0)
            {
                if (entityToSearchFinalList.Count > 0)
                {
                    // Filter the ones in common 
                    List<EntityModel> entityModelsToAdd = new List<EntityModel>();
                    foreach (EntitySubscriptionModel entitySubscription in entitySubscriptionModelListResult)
                    {
                        if (entityToSearchFinalList.Find(x => x.Id == entitySubscription.EntityId) != null)
                            entityModelsToAdd.Add(new EntityModel()
                            {
                                Id = entitySubscription.EntityId
                            });
                    }
                    if (entityModelsToAdd.Count > 0)
                        entityToSearchFinalList.AddRange(entityModelsToAdd);
                }
                else
                {
                    // Add everything 
                    foreach (EntitySubscriptionModel entitySubscription in entitySubscriptionModelListResult)
                    {
                        entityToSearchFinalList.Add(new EntityModel()
                        {
                            Id = entitySubscription.EntityId
                        });
                    }
                }
            }

            // Filter relations
            if (relationModelsResult.Count > 0)
            {
                if (entityToSearchFinalList.Count > 0)
                {
                    // Filter the ones in common 
                    List<EntityModel> entityModelsToAdd = new List<EntityModel>();
                    foreach (RelationModel relation in relationModelsResult)
                    {
                        if (entityToSearchFinalList.Find(x => x.Id == relation.EntityId) != null)
                            entityModelsToAdd.Add(new EntityModel()
                            {
                                Id = relation.EntityId
                            });
                    }
                    if (entityModelsToAdd.Count > 0)
                        entityToSearchFinalList.AddRange(entityModelsToAdd);
                }
                else
                {
                    // Add everything 
                    foreach (RelationModel relation in relationModelsResult)
                    {
                        entityToSearchFinalList.Add(new EntityModel()
                        {
                            Id = relation.EntityId
                        });
                    }
                }
            }

            // Filter addresses 
            if (addressListResult.Count > 0)
            {
                if (entityToSearchFinalList.Count > 0)
                {
                    // Filter the ones in common 
                    List<EntityModel> entityModelsToAdd = new List<EntityModel>();
                    foreach (AddressModel address in addressListResult)
                    {
                        if (entityToSearchFinalList.Find(x => x.Id == address.EntityId) != null)
                            entityModelsToAdd.Add(new EntityModel()
                            {
                                Id = address.EntityId
                            });
                    }
                    if (entityModelsToAdd.Count > 0)
                        entityToSearchFinalList.AddRange(entityModelsToAdd);
                }
                else
                {
                    // Add everything 
                    foreach (AddressModel address in addressListResult)
                    {
                        entityToSearchFinalList.Add(new EntityModel()
                        {
                            Id = address.EntityId
                        });
                    }
                }
            }

            // Filter entities
            if (entityModelListResult.Count > 0)
            {
                if (entityToSearchFinalList.Count > 0)
                {
                    // Filter the ones in common 
                    List<EntityModel> entityModelsToAdd = new List<EntityModel>();
                    foreach (EntityModel entity in entityModelListResult)
                    {
                        if (entityToSearchFinalList.Find(x => x.Id == entity.Id) != null)
                            entityModelsToAdd.Add(new EntityModel()
                            {
                                Id = entity.Id
                            });
                    }
                    if (entityModelsToAdd.Count > 0)
                        entityToSearchFinalList.AddRange(entityModelsToAdd);
                }
                else
                {
                    // Add everything 
                    foreach (EntityModel entity in entityModelListResult)
                    {
                        entityToSearchFinalList.Add(new EntityModel()
                        {
                            Id = int.Parse(entity.Id).ToString()
                        });
                    }
                }
            }

            if (entityToSearchFinalList.Count == 0)
            {
                return new EntityResponseModel()
                {
                    TotalPages = 0,
                    Entities = new List<EntityModifierModel>()
                };
            }
            // Now do the final search to relate everything 
            List<EntityModel> entityModelToReturn = new List<EntityModel>();
            entityModelToReturn = new EntityDAO().Read(_connectionStringRead, entityToSearchFinalList);

            if (entityModelToReturn.Count == 0)
                return new EntityResponseModel()
                {
                    TotalPages = 0,
                    Entities = new List<EntityModifierModel>()
                };

            entityModelToReturn = entityModelToReturn.OrderBy(x => x.Name).ToList();

            totalPages = (int)Math.Ceiling((double)entityModelToReturn.Count() / limit);
            entityModelToReturn = entityModelToReturn.Skip(fromRow).Take(limit).ToList();

            // else, continue
            List<AddressModel> addressToFindToAttach = new List<AddressModel>();
            List<EntitySubscriptionModel> entitySubscriptionToFindToAttach = new List<EntitySubscriptionModel>();
            List<PhoneModel> phoneToFindToAttach = new List<PhoneModel>();
            List<RelationModel> relationModelToFindToAttach = new List<RelationModel>();

            foreach (EntityModel entity in entityModelToReturn)
            {
                addressToFindToAttach.Add(new AddressModel()
                {
                    EntityId = entity.Id
                });
                entitySubscriptionToFindToAttach.Add(new EntitySubscriptionModel()
                {
                    EntityId = entity.Id
                });
                phoneToFindToAttach.Add(new PhoneModel()
                {
                    EntityId = entity.Id
                });
                relationModelToFindToAttach.Add(new RelationModel()
                {
                    EntityId = entity.Id
                });
            }

            // Do the final searches 
            List<AddressModel> addressFinalSearch = new List<AddressModel>();
            if (addressToFindToAttach.Count > 0)
                addressFinalSearch = new AddressDAO().Read(_connectionStringRead, addressToFindToAttach);
            List<EntitySubscriptionModel> entitySubscriptionsFinalSearch = new List<EntitySubscriptionModel>();
            if (entitySubscriptionToFindToAttach.Count > 0)
                entitySubscriptionsFinalSearch = new EntitySubscriptionDAO().Read(_connectionStringRead, entitySubscriptionToFindToAttach);
            List<PhoneModel> phoneFinalSearch = new List<PhoneModel>();
            if (phoneToFindToAttach.Count > 0)
                phoneFinalSearch = new PhoneDAO().Read(_connectionStringRead, phoneToFindToAttach);
            List<RelationModel> relationFinalSearch = new List<RelationModel>();
            if (relationModelToFindToAttach.Count > 0)
                relationFinalSearch = new RelationDAO().Read(_connectionStringRead, relationModelToFindToAttach);

            // Finally, attach everything
            List<EntityModifierModel> entityModifierModelList = new List<EntityModifierModel>();
            foreach (EntityModel entity in entityModelToReturn)
            {
                EntityModifierModel entityModiferToAdd = new EntityModifierModel()
                {
                    Entities = new List<EntityModel>() { entity }
                };

                // Attach address
                List<AddressModel> addressModelsToAddEntity = addressFinalSearch.FindAll(x => x.EntityId == entity.Id);
                if (addressModelsToAddEntity != null && addressModelsToAddEntity.Count > 0)
                    entityModiferToAdd.Addresses = addressModelsToAddEntity;

                // Attach phone 
                List<PhoneModel> phoneModeltsToAddEntity = phoneFinalSearch.FindAll(x => x.EntityId == entity.Id);
                if (phoneModeltsToAddEntity != null && phoneModeltsToAddEntity.Count > 0)
                    entityModiferToAdd.Phones = phoneModeltsToAddEntity;

                // Add subscriptions
                List<EntitySubscriptionModel> entitySubscriptionsModelsToAddEntity = entitySubscriptionsFinalSearch.FindAll(x => x.EntityId == entity.Id);
                if (entitySubscriptionsModelsToAddEntity != null && entitySubscriptionsModelsToAddEntity.Count > 0)
                    entityModiferToAdd.Subscriptions = entitySubscriptionsModelsToAddEntity;

                List<RelationModel> relationModetlsToAddEntity = relationFinalSearch.FindAll(x => x.EntityId == entity.Id);
                if (relationModetlsToAddEntity != null && relationModetlsToAddEntity.Count > 0)
                    entityModiferToAdd.Relations = relationModetlsToAddEntity;

                entityModifierModelList.Add(entityModiferToAdd);
            }


            return new EntityResponseModel()
            {
                TotalPages = totalPages,
                Entities = entityModifierModelList
            };
        }

        /// <summary>
        /// Return the entities with the details.
        /// </summary>
        /// <param name="entityModel">Entity model to find (by entityId only).</param>
        /// <param name="programTypeId">Program Type Id, which must be in the web config configurations.</param>
        /// <param name="institutionTypeId">Institution Type Id, which must be in the web config configurations.</param>
        /// <param name="companyTypeId">Company Type Id,which must be in the web config configurations.</param>
        /// <param name="schoolTypeId">School Type Id, which must be in the web config configurations.</param>
        /// <returns>Return DetailsResponseModel with all details of the entity.</returns>
        public DetailsResponseModel GetEntityWithDetails(EntityModel entityModel, string programTypeId,
            string institutionTypeId, string companyTypeId, string schoolTypeId)
        {
            DetailsResponseModel detailsResponseModel = new DetailsResponseModel();
            // First, we get the entity general details 
            EntityResponseModel entityResponseModel = GetEntityDetails(new List<EntityModel>() { new EntityModel() { Id = entityModel.Id } },
                new List<RelationModel>(), new List<PhoneModel>(),
                new List<AddressModel>(), new List<EntitySubscriptionModel>(), 0, 1);
            EntityModifierModel entityModifierModel = entityResponseModel.Entities[0];
            detailsResponseModel.Entities = entityModifierModel.Entities;
            if (entityModifierModel.Addresses != null && entityModifierModel.Addresses.Count > 0)
                detailsResponseModel.Addresses = entityModifierModel.Addresses;
            if (entityModifierModel.Phones != null && entityModifierModel.Phones.Count > 0)
                detailsResponseModel.Phones = entityModifierModel.Phones;
            if (entityModifierModel.Subscriptions != null && entityModifierModel.Subscriptions.Count > 0)
                detailsResponseModel.Subscriptions = entityModifierModel.Subscriptions;

            // Get institution details
            List<EntityModel> institutionsToGetInformationList = new List<EntityModel>();
            List<EntityModel> programsToGetInformationList = new List<EntityModel>();
            List<EntityModel> companyToGetInformationList = new List<EntityModel>();
            List<EntityModel> schoolToGetInformationList = new List<EntityModel>();

            if (entityModifierModel.Relations != null && entityModifierModel.Relations.Count > 0)
            {
                // Classify relation to find details
                foreach (RelationModel relationModel in entityModifierModel.Relations)
                {
                    if (relationModel.TypeId == programTypeId)
                        programsToGetInformationList.Add(new EntityModel() { Id = relationModel.RelationEntityId });
                    else if (relationModel.TypeId == institutionTypeId)
                        institutionsToGetInformationList.Add(new EntityModel() { Id = relationModel.RelationEntityId });
                    else if (relationModel.TypeId == companyTypeId)
                        companyToGetInformationList.Add(new EntityModel() { Id = relationModel.RelationEntityId });
                    else if (relationModel.TypeId == schoolTypeId)
                        schoolToGetInformationList.Add(new EntityModel() { Id = relationModel.RelationEntityId });
                }
            }

            // Now do the search for the relations
            EntityDAO entityDAO = new EntityDAO();
            if (programsToGetInformationList.Count > 0)
                detailsResponseModel.Programs = entityDAO.Read(_connectionStringRead, programsToGetInformationList);
            if (institutionsToGetInformationList.Count > 0)
                detailsResponseModel.Institutions = entityDAO.Read(_connectionStringRead, institutionsToGetInformationList);
            if (schoolToGetInformationList.Count > 0)
                detailsResponseModel.Schools = entityDAO.Read(_connectionStringRead, schoolToGetInformationList);
            if (companyToGetInformationList.Count > 0)
                detailsResponseModel.Companies = entityDAO.Read(_connectionStringRead, companyToGetInformationList);


            return detailsResponseModel;
        }

        /// <summary>
        /// To return the platforms that can access to the School API.
        /// </summary>
        /// <param name="platformToFind">The platform details to find. For example, if we want to filter by a code, we send the code here in the model. </param>
        /// <returns>Returns the list of platforms found.</returns>
        public List<PlatformModel> GetPlatformModels(List<PlatformModel> platformToFind)
        {
            return new PlatformDAO().Read(_connectionStringRead, platformToFind);
        }

       /// <summary>
       /// To get the addresses of entities.
       /// </summary>
       /// <param name="addressModels">The address models to filter.</param>
       /// <returns>Returns a list of addresses.</returns>
        public List<AddressModel> GetAddressModels(List<AddressModel> addressModels)
        {
            return new AddressDAO().Read(_connectionStringRead, addressModels);
        }

        /// <summary>
        /// To read the terms (as semester, trimester, etc).
        /// </summary>
        /// <param name="termModels">Term models to filter.</param>
        /// <returns>Returns a list of terms.</returns>
        public List<TermModel> GetTerms(List<TermModel> termModels)
        {
            return new TermDAO().Read(_connectionStringRead, termModels);
        }

        /// <summary>
        /// To read the academic periods.
        /// </summary>
        /// <param name="academicPeriodModelsToFind">A list of academic period models to find.</param>
        /// <returns>Returns a list of academic periods.</returns>
        public List<AcademicPeriodModel> GetAcademicPeriodModels(List<AcademicPeriodModel> academicPeriodModelsToFind)
        {
            return new AcademicPeriodDAO().Read(_connectionStringRead, academicPeriodModelsToFind);
        }
    }
}
