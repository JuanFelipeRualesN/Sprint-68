using SchoolsAPI.SchoolsAPI.Domain.Models.Schools;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools.Request;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools.Response;
using SchoolsAPI.SchoolsAPI.Infrastructure.Repositories.DapperHandler;
using SchoolsAPI.SchoolsAPI.Infrastructure.Repositories.Schools;
using SchoolsAPI.SchoolsAPI.Services.Generals;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SchoolsAPI.SchoolsAPI.Services.Schools
{
    public class AcademicService
    {
        private string _connectionStringWrite;
        private string _connectionStringRead;
        private string _langCode;

        /// <summary>
        /// To handle the academic details.
        /// </summary>
        /// <param name="connectionStringWrite">The connection string write.</param>
        /// <param name="connectionStringRead">The connection string read.</param>
        /// <param name="langCode">The language code.</param>
        public AcademicService(string connectionStringWrite, string connectionStringRead, string langCode)
        {
            _connectionStringWrite = connectionStringWrite;
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// To get the academic years.
        /// </summary>
        /// <param name="fromPage">The page.</param>
        /// <param name="limit">How many items per page.</param>
        /// <param name="startDate">The start date of the year.</param>
        /// <param name="endDate">The end date of the year.</param>
        /// <param name="termId">The term id.</param>
        /// <param name="filterOutExpiredAcademicYears">To filter out the academic years that has been expired.</param>
        /// <returns>Returns the entity academic details list (academic years).</returns>
        public ResponsePagination<AcademicDetailModel> GetAcademicYears(int fromPage, int limit, string startDate = null, string endDate = null, string termId = null, bool filterOutExpiredAcademicYears = false)
        {
            var academicYears = AcademicDetails.GetAcademicDetails(fullTermStartDate: startDate,
                fullTermEndDate: endDate, termId: termId);
            if (academicYears == null || academicYears.Count == 0)
            {
                return new ResponsePagination<AcademicDetailModel>();
            }

            // Filter out the academic years that has been expired.
            if (filterOutExpiredAcademicYears)
            {
                academicYears = academicYears.Where(x => DateTime.Parse(x.FullTermEndDate) >= DateTime.Now).ToList();
            }

            // Limit
            int totalItems = academicYears.Count;
            var entityAcademicDetailResponseModel = new ResponsePagination<AcademicDetailModel>()
            {
                TotalItems = totalItems,
                PagesTotal = int.Parse(Math.Ceiling(decimal.Parse(totalItems.ToString()) / decimal.Parse(limit.ToString())).ToString()),
                Items = academicYears.Skip(fromPage * limit).Take(limit > totalItems ? totalItems : limit).ToList()
            };
            return entityAcademicDetailResponseModel;
        }

        /// <summary>
        /// To get academic years by ids.
        /// </summary>
        /// <param name="ids">The ids to find.</param>
        /// <returns>Return academic detail request model.</returns>
        public List<AcademicDetailRequestModel> GetAcademicYearsByIdList(List<int> ids)
        {
            var academicDetailsModelToFind = new List<AcademicDetailModel>();
            var academicPeriodsModelToFind = new List<AcademicPeriodModel>();
            foreach (var id in ids)
            {
                academicDetailsModelToFind.Add(new AcademicDetailModel()
                {
                    Id = id.ToString()
                });
                academicPeriodsModelToFind.Add(new AcademicPeriodModel()
                {
                    AcademicDetailId = id.ToString()
                });
            }

            var academicYears = new AcademicDetailDAO().Read(
                _connectionStringRead, academicDetailsModelToFind);
            var academicPeriods = new AcademicPeriodDAO().Read(
                _connectionStringRead, academicPeriodsModelToFind);

            var academicDetailRequestModels = new List<AcademicDetailRequestModel>();
            foreach (var academicYear in academicYears)
            {
                academicDetailRequestModels.Add(new AcademicDetailRequestModel()
                {
                    AcademicDetail = academicYear,
                    AcademicPeriods = academicPeriods.Where(x => x.AcademicDetailId == academicYear.Id).ToList()
                });
            }

            return academicDetailRequestModels;
        }

        /// <summary>
        /// To get the academic year by id.
        /// </summary>
        /// <param name="id">Id of academic year.</param>
        /// <returns>Returns the entity academic details.</returns>
        public AcademicDetailRequestModel GetAcademicYearsId(int id)
        {
            try
            {
                // Get the academic year. 
                return new AcademicDetailRequestModel
                {
                    AcademicDetail = new AcademicDetailDAO().Read(_connectionStringRead, new AcademicDetailModel()
                    {
                        Id = id.ToString()
                    }, false, false)[0],
                    AcademicPeriods = new AcademicPeriodDAO().Read(_connectionStringRead, new AcademicPeriodModel()
                    {
                        AcademicDetailId = id.ToString()
                    }, false, false)
                };
            }
            catch (Exception e)
            {
                return new AcademicDetailRequestModel();
            }
        }

        /// <summary>
        /// To get the academic year by subscription id.
        /// </summary>
        /// <param name="subscriptionId">The subscription id to call.</param>
        /// <returns>Returns the academic detail dto model. </returns>
        public AcademicDetailRequestModel GetAcademicDetailBySubscriptionId(int subscriptionId)
        {
            // First, read the subscription entity details. 
            var subscriptionEntityDetails = new EntitySubscriptionDAO().Read(_connectionStringRead, new EntitySubscriptionModel()
            {
                SubscriptionId = subscriptionId.ToString()
            }, false, false);

            if (subscriptionEntityDetails.Count == 0)
            {
                return new AcademicDetailRequestModel();
            }

            // Now read the academic details.
            return GetAcademicYearsId(int.Parse(subscriptionEntityDetails[0].AcademicDetailId));
        }

        /// <summary>
        /// To get the academic details (list) by subscription ids.
        /// </summary>
        /// <param name="subscriptionIds">The subscription ids to find.</param>
        /// <returns>Returns the list of academic details.</returns>
        public List<EntityAcademicDetailFullResponseModel> GetAcademicEntityDetailsBySubscriptionIds(List<string> subscriptionIds)
        {
            var entitySubscriptionModelsToFind = new List<EntitySubscriptionModel>();
            entitySubscriptionModelsToFind.AddRange(subscriptionIds.Select(subscriptionId => new EntitySubscriptionModel()
            {
                SubscriptionId = subscriptionId
            }));

            // First, read the subscription entity details. 
            var subscriptionEntityDetails =
            new EntitySubscriptionDAO().Read(_connectionStringRead, entitySubscriptionModelsToFind);

            if (subscriptionEntityDetails.Count == 0)
            {
                return new List<EntityAcademicDetailFullResponseModel>();
            }

            // Prepare the academic detail ids.
            var academicDetailIds = subscriptionEntityDetails.Select(x => int.Parse(x.AcademicDetailId)).ToList();

            // Now read the academic details.
            var academicDetails = GetAcademicYearsByIdList(academicDetailIds);

            // Prepare the response.
            var entityAcademicDetailFullResponseModels = new List<EntityAcademicDetailFullResponseModel>();
            foreach (var entitySubscription in subscriptionEntityDetails)
            {
                var academicDetail = academicDetails.FirstOrDefault(x => x.AcademicDetail.Id == entitySubscription.AcademicDetailId);
                if (academicDetail == null)
                {
                    continue;
                }
                entityAcademicDetailFullResponseModels.Add(new EntityAcademicDetailFullResponseModel()
                {
                    EntitySubscriptionModel = entitySubscription,
                    AcademicDetailModel = academicDetail.AcademicDetail,
                    AcademicPeriodModelList = academicDetail.AcademicPeriods
                });
            }
            return entityAcademicDetailFullResponseModels;
        }

        /// <summary>
        /// To add the academic year.
        /// </summary>
        /// <param name="academicDetailRequest">The academic details to add.</param>
        /// <param name="userRequesterId">The user id of the requester.</param>
        /// <returns>Returns the academic details.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public AcademicDetailRequestModel AddAcademicYear(AcademicDetailRequestModel academicDetailRequest, string userRequesterId)
        {
            var validateSchoolDetails = Validator.ValidateSchoolAcademicDetails(academicDetailRequest, false, _connectionStringRead, _langCode);
            if (!validateSchoolDetails.Valid)
            {
                academicDetailRequest.Valid = false;
                academicDetailRequest.Messages = validateSchoolDetails.Messages;
                return academicDetailRequest;
            }

            // Validate the periods before any insertion. 
            var validatePeriodsToCreate =
                Validator.ValidateSchoolPeriods(academicDetailRequest.AcademicPeriods, false, false,
                        _connectionStringRead, _langCode);

            if (!validatePeriodsToCreate.Valid)
            {
                academicDetailRequest.Valid = false;
                academicDetailRequest.Messages = validatePeriodsToCreate.Messages;
                return academicDetailRequest;
            }

            // Enter the academic details.
            var academicDetailDao = new AcademicDetailDAO();
            academicDetailRequest.AcademicDetail.ModifiedBy = userRequesterId;
            if (academicDetailDao == null) throw new ArgumentNullException(nameof(academicDetailDao));
            var academicDetail = academicDetailDao.Create(_connectionStringWrite, academicDetailRequest.AcademicDetail)[0];
            // Create the periods.
            // Set the id of the academic detail to the periods.
            foreach (var academicPeriod in academicDetailRequest.AcademicPeriods)
            {
                academicPeriod.AcademicDetailId = academicDetail.Id;
                academicPeriod.ModifiedBy = userRequesterId;
            }
            var academicPeriodDao = new AcademicPeriodDAO();
            academicPeriodDao.Create(_connectionStringWrite, academicDetailRequest.AcademicPeriods);

            academicDetailRequest = GetAcademicYearsId(int.Parse(academicDetail.Id));
            academicDetailRequest.Valid = true;
            academicDetailRequest.Messages = new List<string>()
            {
                new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_academic_year_create_success"])
            };
            return academicDetailRequest;
        }

        /// <summary>
        /// To modify the academic year.
        /// </summary>
        /// <param name="academicDetailRequest">The academic detail request to modify.</param>
        /// <param name="userRequesterId">The user that is modifying the request.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public AcademicDetailRequestModel ModifyAcademicYear(AcademicDetailRequestModel academicDetailRequest, string userRequesterId)
        {
            var validateSchoolDetails = Validator.ValidateSchoolAcademicDetails(
                academicDetailRequest, true, _connectionStringRead, _langCode);
            if (!validateSchoolDetails.Valid)
            {
                academicDetailRequest.Valid = false;
                academicDetailRequest.Messages = validateSchoolDetails.Messages;
                return academicDetailRequest;
            }

            // Validate the periods before any insertion. 
            var validatePeriodsToCreate =
                Validator.ValidateSchoolPeriods(academicDetailRequest.AcademicPeriods, true, false,
                    _connectionStringRead, _langCode);

            if (!validatePeriodsToCreate.Valid)
            {
                academicDetailRequest.Valid = false;
                academicDetailRequest.Messages = validatePeriodsToCreate.Messages;
                return academicDetailRequest;
            }

            // Enter the academic details.
            var academicDetailDao = new AcademicDetailDAO();
            academicDetailRequest.AcademicDetail.ModifiedBy = userRequesterId;
            if (academicDetailDao == null) throw new ArgumentNullException(nameof(academicDetailDao));
            var academicDetail = academicDetailDao.Update(_connectionStringWrite,
                new AcademicDetailModel()
                {
                    FullTermEndDate = academicDetailRequest.AcademicDetail.FullTermEndDate,
                    FullTermStartDate = academicDetailRequest.AcademicDetail.FullTermStartDate,
                    DaysToClosePeriod = academicDetailRequest.AcademicDetail.DaysToClosePeriod,
                    PeriodQuantity = academicDetailRequest.AcademicDetail.PeriodQuantity,
                    TermId = academicDetailRequest.AcademicDetail.TermId,
                    ModifiedBy = userRequesterId
                },
                new AcademicDetailModel()
                {
                    Id = academicDetailRequest.AcademicDetail.Id
                })[0];

            // Retrieve the old periods to validate the differences.
            var oldPeriods = new AcademicPeriodDAO().Read(_connectionStringRead, new AcademicPeriodModel()
            {
                AcademicDetailId = academicDetailRequest.AcademicDetail.Id
            }, false, false);

            // Compare the old periods with the new ones.

            // First, check the ones to delete if any. 
            var periodsToDelete = oldPeriods.Where(x => !academicDetailRequest.AcademicPeriods.Select(y => y.Id).Contains(x.Id)).ToList();
            if (periodsToDelete.Count > 0)
            {
                var periodsToDeleteModels = periodsToDelete.Select(x => new AcademicPeriodModel()
                {
                    Id = x.Id
                }).ToList();

                // Delete the periods.
                new AcademicPeriodDAO().Delete(_connectionStringWrite, periodsToDeleteModels);
            }

            // Check the ones to add if any.
            var periodsToAdd = academicDetailRequest.AcademicPeriods.Where(x => !oldPeriods.Select(y => y.Id).Contains(x.Id)).ToList();
            if (periodsToAdd.Count > 0)
            {
                // Set the id of the academic detail to the periods.
                foreach (var academicPeriod in periodsToAdd)
                {
                    academicPeriod.AcademicDetailId = academicDetailRequest.AcademicDetail.Id;
                    academicPeriod.ModifiedBy = userRequesterId;
                }
                new AcademicPeriodDAO().Create(_connectionStringWrite, periodsToAdd);
            }

            // Check the ones to update if any.
            var periodsToUpdate = academicDetailRequest.AcademicPeriods.Where(x => oldPeriods.Select(y => y.Id).Contains(x.Id)).ToList();
            if (periodsToUpdate.Count > 0)
            {
                var periodsToUpdateModelList = new List<AcademicPeriodModel>();
                var periodsToUpdateModelWhereList = new List<AcademicPeriodModel>();

                // Set the id of the academic detail to the periods.
                foreach (var academicPeriod in periodsToUpdate)
                {
                    periodsToUpdateModelList.Add(new AcademicPeriodModel()
                    {
                        PeriodNumber = academicPeriod.PeriodNumber,
                        PeriodFrom = academicPeriod.PeriodFrom,
                        PeriodTo = academicPeriod.PeriodTo,
                        ModifiedBy = userRequesterId
                    });

                    periodsToUpdateModelWhereList.Add(new AcademicPeriodModel()
                    {
                        Id = academicPeriod.Id
                    });
                }
                new AcademicPeriodDAO().Update(_connectionStringWrite,
                    periodsToUpdateModelList, periodsToUpdateModelWhereList);
            }

            academicDetailRequest = GetAcademicYearsId(int.Parse(academicDetailRequest.AcademicDetail.Id));
            academicDetailRequest.Valid = true;
            academicDetailRequest.Messages = new List<string>()
            {
                new MessageManager(this._connectionStringRead, this._langCode).GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_academic_year_update_success"])
            };
            return academicDetailRequest;
        }

        /// <summary>
        /// To get the academic period by list, with academic details. It will return the academic details
        /// with the periods that are in the list. 
        /// </summary>
        /// <param name="academicPeriodIdList">The academic period id list.</param>
        /// <returns>Returns the academic details details with the periods that are in the list.</returns>
        public List<AcademicDetailRequestModel> GetAcademicFullDetailsByPeriodIdList(List<string> academicPeriodIdList)
        {
            // Get the academic details by period ids.
            var academicPeriodModelList = new AcademicPeriodDAO().Read(this._connectionStringRead, academicPeriodIdList.Select(x => new AcademicPeriodModel()
            {
                Id = x
            }).ToList());

            // Get the academic details by academic detail ids.
            var academicDetailIdList = academicPeriodModelList.Select(x => x.EntityAcademicDetailId).Distinct().ToList();
            
            var academicDetailModelList = new AcademicDetailDAO().Read(this._connectionStringRead, academicDetailIdList.Select(x => new AcademicDetailModel()
            {
                Id = x
            }).ToList());

            // Prepare the response.
            var academicDetailRequestModelList = new List<AcademicDetailRequestModel>();
            foreach (var academicDetailModel in academicDetailModelList)
            {
                academicDetailRequestModelList.Add(new AcademicDetailRequestModel()
                {
                    AcademicDetail = academicDetailModel,
                    AcademicPeriods = academicPeriodModelList.Where(x => x.EntityAcademicDetailId == academicDetailModel.Id).ToList()
                });
            }

            return academicDetailRequestModelList;
        }
    }
}