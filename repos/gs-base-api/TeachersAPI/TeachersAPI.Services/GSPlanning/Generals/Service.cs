using GSPlanning.Infrastructure.Interfaces;
using GSPlanning.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GSPlanning.Domain;
using GSPlanning.Infrastructure.Repositories;

namespace GSPlanning.Services.Generals
{
    public class Service<TType> 
        where TType : class 
    {
        protected readonly Repository<TType> TypeRepository;

        protected Service(Repository<TType> typeRepository)
        {
            TypeRepository = typeRepository;
        }
        
        protected async Task<List<TType>> GetTypes()
        {
            return (await TypeRepository.GetAllAsync()).ToList();
        }

        protected async Task<List<TType>> GetTypes(Expression<Func<TType, int>> propertyExpression,
            int value)
        {
            return (await TypeRepository.GetAllByAsync(propertyExpression, value)).ToList();
        }
        
        protected async Task<List<TType>> GetTypes(Expression<Func<TType, int>> propertyExpression,
            int[] values)
        {
            return (await TypeRepository.GetAllInByAsync(propertyExpression, values)).ToList();
        }
    }
}
