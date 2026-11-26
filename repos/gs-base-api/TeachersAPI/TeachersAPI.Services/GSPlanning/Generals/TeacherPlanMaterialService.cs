using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GSPlanning.Domain;
using GSPlanning.Infrastructure.Repositories;
using GSPlanning.Services.Utilities;
using GSPlanning.Models.Translations;

namespace GSPlanning.Services.Generals
{
    public class TeacherPlanMaterialService : Service<MaterialType>
    {
        private readonly Repository<TeacherPlanMaterial> _teacherPlanMaterialRepository;
        private readonly Repository<TeacherPlanMaterialType> _teacherPlanMaterialTypeRepository;
        private readonly Repository<TeacherPlanMaterialDate> _teacherPlanMaterialDateRepository;
        private readonly Repository<MaterialTypeTranslation> _materialTypeTranslationRepository;

        public TeacherPlanMaterialService(
            Repository<MaterialType> materialTypeRepository, 
            Repository<TeacherPlanMaterial> teacherPlanMaterialRepository, 
            Repository<TeacherPlanMaterialType> teacherPlanMaterialTypeRepository, 
            Repository<TeacherPlanMaterialDate> teacherPlanMaterialDateRepository,
            Repository<MaterialTypeTranslation> materialTypeTranslationRepository
            ) 
            : base(materialTypeRepository)
        {
            _teacherPlanMaterialRepository = teacherPlanMaterialRepository;
            _teacherPlanMaterialTypeRepository = teacherPlanMaterialTypeRepository;
            _teacherPlanMaterialDateRepository = teacherPlanMaterialDateRepository;
            _materialTypeTranslationRepository = materialTypeTranslationRepository;
        }

        public async Task<List<MaterialType>> GetMaterialTypes()
        {
           return await GetTypes();
        }

        public async Task<List<MaterialType>> GetMaterialTypeTranslations(string langCode = "es")
        {
            var materialTypes = await GetTypes();
            var translations = (await _materialTypeTranslationRepository.GetAllByAsync(x => x.Lang, langCode)).ToList();

            foreach (var materialType in materialTypes)
            {
                var translation = translations.FirstOrDefault(x => x.MaterialTypeId == materialType.Id);
                if (translation != null)
                {
                    materialType.Description = translation.Description;
                    materialType.Name = translation.Name;
                }
            }
            return materialTypes;
        }


        public async Task<MaterialType> CreateEvaluationType(MaterialType materialType)
        {
            // Check if null, if null, throw an exception
            if (materialType == null)
            {
                throw new ArgumentNullException(nameof(materialType), "Material type cannot be null.");
            }

            // Check if the description is null or empty, if so, throw an exception
            if (string.IsNullOrWhiteSpace(materialType.Description))
            {
                throw new ArgumentException("Material type description cannot be null or empty.", nameof(materialType));
            }

            var materialTypes =
                (await TypeRepository.GetAllByAsync(x => x.Description, materialType.Description)).ToList();
            
            if (materialTypes.Any())
            {
                return materialTypes.FirstOrDefault();
            }
            
            var id = await TypeRepository.AddAsync(new MaterialType()
            {
                Name = materialType.Description,
                Description =  materialType.Description,
                ModifiedBy = materialType.ModifiedBy,
                Type = MaterialType.IsCustom,
            });

            return await TypeRepository.GetByIdAsync(id);
        }
        
        public async Task<List<TeacherPlanMaterial>> GetTeacherPlanAMaterialByTeacherPlanId(int teacherPlanId, string langCode = "es")
        {
            var teacherPlanMaterials =
                (await _teacherPlanMaterialRepository.GetAllByAsync(x => x.TeacherPlanId, teacherPlanId)).ToList();

            if (!teacherPlanMaterials.Any())
            {
                return new List<TeacherPlanMaterial>();
            }
            var teacherPLanMaterialIds = teacherPlanMaterials.Select(x => x.Id).ToArray();
            
            var teacherPlanMaterialDates = (await _teacherPlanMaterialDateRepository.GetAllInByAsync(x => x.TeacherPlanMaterialId,
                teacherPLanMaterialIds)).ToList();

            var teacherPlanMaterialTypes = (await _teacherPlanMaterialTypeRepository.GetAllInByAsync(x => x.TeacherPlanMaterialId,
                teacherPLanMaterialIds)).ToList();

            var types = (await GetMaterialTypeTranslations(langCode))
                .ToDictionary(x => x.Id);
            
            foreach (var teacherPlanMaterial in teacherPlanMaterials)
            {
                teacherPlanMaterial.TeacherPlanMaterialDates = teacherPlanMaterialDates
                    .Where(x => x.TeacherPlanMaterialId == teacherPlanMaterial.Id).ToList();

                var materialTypes = teacherPlanMaterialTypes
                    .Where(x => x.TeacherPlanMaterialId == teacherPlanMaterial.Id).ToList();
                materialTypes.ForEach(x => x.MaterialType = types[x.MaterialTypeId]);
                teacherPlanMaterial.TeacherPlanMaterialTypes = materialTypes;
            }

            return teacherPlanMaterials;
        }

        public async Task CreateTeacherPlanMaterial(TeacherPlanMaterial teacherPlanMaterial)
        {
            // Check if null, if null, throw an exception
            if (teacherPlanMaterial == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanMaterial), "TeacherPlanMaterial cannot be null.");
            }

            var id = await _teacherPlanMaterialRepository.AddAsync(teacherPlanMaterial);
            
            if (teacherPlanMaterial.TeacherPlanMaterialTypes.Count > 0)
            {
                await SaveTeacherPlanMaterialTypes(teacherPlanMaterial, id);
            }

            if (teacherPlanMaterial.TeacherPlanMaterialDates.Count > 0)
            {
                await SaveTeacherPlanMaterialDates(teacherPlanMaterial, id);
            }
        }
        
        public async Task UpdateTeacherPlanMaterial(TeacherPlanMaterial teacherPlanMaterial)
        {
            // Check if null, if null, throw an exception
            if (teacherPlanMaterial == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanMaterial), "TeacherPlanMaterial cannot be null.");
            }

            if (teacherPlanMaterial.Id == 0)
            {
                throw new ArgumentException(
                    $"Invalid update. Is missing the {nameof(TeacherPlanMaterial)}.{nameof(teacherPlanMaterial.Id)} property.");
            }
            
            if (teacherPlanMaterial.TeacherPlanMaterialTypes.Count > 0)
            {
                await _teacherPlanMaterialTypeRepository.DeleteByAsync(x => x.TeacherPlanMaterialId, teacherPlanMaterial.Id);
                await SaveTeacherPlanMaterialTypes(teacherPlanMaterial, teacherPlanMaterial.Id);
            }

            if (teacherPlanMaterial.TeacherPlanMaterialDates.Count > 0)
            {
                await _teacherPlanMaterialDateRepository.DeleteByAsync(x => x.TeacherPlanMaterialId,
                    teacherPlanMaterial.Id);
                await SaveTeacherPlanMaterialDates(teacherPlanMaterial, teacherPlanMaterial.Id);
            }
        }

        private async Task SaveTeacherPlanMaterialDates(
            TeacherPlanMaterial teacherPlanMaterial, int id)
        {
            if (teacherPlanMaterial.TeacherPlanMaterialDates.Count > 0)
            {
                foreach (var teacherPlanMaterialDate in teacherPlanMaterial.TeacherPlanMaterialDates)
                {
                    if (teacherPlanMaterialDate == null)
                        throw new ArgumentNullException(nameof(teacherPlanMaterialDate), "TeacherPlanMaterialDate cannot be null.");

                    // Check if the date is valid
                    DateUtil.ValidateMySqlDate(teacherPlanMaterialDate.ImplementationDate);
                    teacherPlanMaterialDate.TeacherPlanMaterialId = id;
                }

                await _teacherPlanMaterialDateRepository.AddAsync(teacherPlanMaterial
                    .TeacherPlanMaterialDates);
            }
        }

        private async Task SaveTeacherPlanMaterialTypes(
            TeacherPlanMaterial teacherPlanMaterial, int id)
        {
            // Check if null, if null, throw an exception
            if (teacherPlanMaterial == null)
            {
                throw new ArgumentNullException(nameof(teacherPlanMaterial), "TeacherPlanMaterial cannot be null.");
            }

            if (id == 0)
            {
                throw new ArgumentException(
                    $"Invalid save. Is missing the {nameof(id)} property.");
            }

            foreach (var teacherPlanMaterialType in teacherPlanMaterial.TeacherPlanMaterialTypes)
            {
                teacherPlanMaterialType.TeacherPlanMaterialId = id;
            }

            await _teacherPlanMaterialTypeRepository.AddAsync(teacherPlanMaterial
                .TeacherPlanMaterialTypes);
        
        }
        
        public async Task RemoveTeacherPlanMaterial(int teacherPlanMaterialId)
        {
            if (teacherPlanMaterialId == 0)
            {
                throw new ArgumentException(
                    $"Invalid remove. Is missing the {nameof(teacherPlanMaterialId)} property.");
            }

            await _teacherPlanMaterialDateRepository.DeleteByAsync(
                x => x.TeacherPlanMaterialId, teacherPlanMaterialId);
            
            await _teacherPlanMaterialTypeRepository.DeleteByAsync(
                x => x.TeacherPlanMaterialId, teacherPlanMaterialId);
            
            await _teacherPlanMaterialRepository.DeleteAsync(teacherPlanMaterialId);
        }


        /// <summary>
        /// To copy the Plan Accomodation from an old teacher plan to a new one.
        /// </summary>
        /// <param name="oldTeacherPlan">The old plan.</param>
        /// <param name="newTeacherPlan">The new plan.</param>
        /// <param name="suggestedStartDate">The suggested start date when the period is distinct.</param>
        /// <param name="suggestedCloseDate">The suggested close date when the period is distinct.</param>
        /// <returns></returns>
        public async Task CopyTeacherPlanAccommodation(TeacherPlan oldTeacherPlan,
            TeacherPlan newTeacherPlan, string suggestedStartDate, string suggestedCloseDate)
        {
            // Check if null, if null, throw an exception
            if (oldTeacherPlan == null)
            {
                throw new ArgumentNullException(nameof(oldTeacherPlan), "Old teacher plan cannot be null.");
            }
            if (newTeacherPlan == null)
            {
                throw new ArgumentNullException(nameof(newTeacherPlan), "New teacher plan cannot be null.");
            }

            var materialsToCopy = await GetTeacherPlanAMaterialByTeacherPlanId(oldTeacherPlan.Id);

            foreach (var teacherPlanMaterial in materialsToCopy)
            {
                var newDates = teacherPlanMaterial.TeacherPlanMaterialDates
                        .Select(x => x.ImplementationDate)
                        .ToArray();

                if (oldTeacherPlan.PeriodId != newTeacherPlan.PeriodId)
                {
                    newDates = DateUtil.CreateDates(
                        DateTime.Parse(suggestedStartDate),
                        DateTime.Parse(suggestedCloseDate));
                }

                var material = new TeacherPlanMaterial
                {
                    TeacherPlanId = newTeacherPlan.Id,
                    ModifiedBy = newTeacherPlan.ModifiedBy,
                    TeacherPlanMaterialDates = newDates.
                        Select(date => new TeacherPlanMaterialDate
                        {
                            ImplementationDate = date,
                            ModifiedBy = newTeacherPlan.ModifiedBy
                        })
                        .ToList(),
                    TeacherPlanMaterialTypes = teacherPlanMaterial.TeacherPlanMaterialTypes
                        .Select(x => new TeacherPlanMaterialType
                        {
                            MaterialTypeId = x.MaterialTypeId, 
                            ModifiedBy = newTeacherPlan.ModifiedBy
                        })
                        .ToList()
                };
                
                await CreateTeacherPlanMaterial(material);
            }
        }
    }
}