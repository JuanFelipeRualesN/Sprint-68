using Dapper;
using GSPlanning.Infrastructure.Interfaces;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GSPlanning.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IDbConnection _dbConnection;

        public Repository(MySqlConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var tableName = FormatTableName<T>();
            var properties = typeof(T).GetProperties()
                                      .Where(p => !p.GetGetMethod().IsVirtual)
                                      .Select(p => new
                                      {
                                          OriginalName = p.Name,
                                          SnakeCaseName = ToSnakeCase(p.Name)
                                      });
            var columns = string.Join(", ", properties.Select(p => $"{p.SnakeCaseName} AS {p.OriginalName}"));
            var query = $"SELECT {columns} FROM {tableName}";
            return await _dbConnection.QueryAsync<T>(query);
        }
        
        public async Task<IEnumerable<T>> GetAllByAsync(Expression<Func<T, int>> propertyExpression, object value)
        {
            return await ExecuteQueryBy(propertyExpression, value);
        }
        
        public async Task<IEnumerable<T>> GetAllByAsync(Expression<Func<T, string>> propertyExpression, object value)
        {
            return await ExecuteQueryBy(propertyExpression, value);
        }

        private async Task<IEnumerable<T>> ExecuteQueryBy<T, TProperty>(Expression<Func<T, TProperty>> propertyExpression, object value)
        {
            var tableName = FormatTableName<T>();
            var properties = typeof(T).GetProperties()
                .Where(p => !p.GetGetMethod().IsVirtual)
                .Select(p => new
                {
                    OriginalName = p.Name,
                    SnakeCaseName = ToSnakeCase(p.Name)
                });
            var columns = string.Join(", ", properties.Select(p => $"{p.SnakeCaseName} AS {p.OriginalName}"));
            var propertyName = GetPropertyName(propertyExpression);
            var query = $"SELECT {columns} FROM {tableName} WHERE {ToSnakeCase(propertyName)} = {FormatValue(value)}";
            return await _dbConnection.QueryAsync<T>(query);
        }
        
        public async Task<IEnumerable<T>> GetAllInByAsync(Expression<Func<T, int>> propertyExpression, int[] values)
        {
            return await ExecuteQueryIn(propertyExpression, values);
        }
        
        public async Task<IEnumerable<T>> GetAllInByAsync(Expression<Func<T, string>> propertyExpression, string[] values)
        {
            return await ExecuteQueryIn(propertyExpression, values);
        }
        private async Task<IEnumerable<T>> ExecuteQueryIn<T, TProperty>(Expression<Func<T, TProperty>> propertyExpression, TProperty[] values)
        {
            var tableName = FormatTableName<T>();
            var properties = typeof(T).GetProperties()
                .Where(p => !p.GetGetMethod().IsVirtual)
                .Select(p => new
                {
                    OriginalName = p.Name,
                    SnakeCaseName = ToSnakeCase(p.Name)
                });
            var columns = string.Join(", ", properties.Select(p => $"{p.SnakeCaseName} AS {p.OriginalName}"));
            var propertyName = GetPropertyName(propertyExpression);
            var query = $"SELECT {columns} FROM {tableName} WHERE {ToSnakeCase(propertyName)} IN ({string.Join(", ", values.Select(v => v))})";
            return await _dbConnection.QueryAsync<T>(query);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var tableName = FormatTableName<T>();
            var properties = typeof(T).GetProperties()
                                      .Where(p => !p.GetGetMethod().IsVirtual)
                                      .Select(p => new
                                      {
                                          OriginalName = p.Name,
                                          SnakeCaseName = ToSnakeCase(p.Name)
                                      });
            var columns = string.Join(", ", properties.Select(p => $"{p.SnakeCaseName} AS {p.OriginalName}"));
            var query = $"SELECT {columns} FROM {tableName} WHERE id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<T>(query, new { Id = id });
        }

        public async Task<int> AddAsync(T entity)
        {
            var query = CreateInsertString(entity);
            
            return await _dbConnection.ExecuteScalarAsync<int>($"{query}; SELECT LAST_INSERT_ID();", entity);
        }
        
        public async Task<int> AddAsync(ICollection<T> entities)
        {
            int count = 0;
            
            foreach (var entity in entities)
            {
                var query = CreateInsertString(entity);
                count += await _dbConnection.ExecuteAsync(query, entity);
            }

            return count;
        }
        
        private string CreateInsertString(T entity)
        {
            var tableName = FormatTableName<T>();
            var properties = typeof(T).GetProperties()
                .Where(p => !p.GetGetMethod().IsVirtual)
                .Where(p => p.GetValue(entity) != null)
                .Select(p => p.Name);
      
            var columns = string.Join(", ", properties.Select(ToSnakeCase));
            var values = string.Join(", ", properties.Select(p => "@" + p));
            var query = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";
            return query;
        }

        public async Task<int> UpdateAsync(T entity)
        {
            var tableName = FormatTableName<T>();
            var properties = typeof(T).GetProperties().Where(p => !p.GetGetMethod().IsVirtual).Select(p => p.Name).Where(p => p != "Id");
            var setClause = string.Join(", ", properties.Select(p => $"{ToSnakeCase(p)} = @{p}"));
            var query = $"UPDATE {tableName} SET {setClause} WHERE id = @Id";
            return await _dbConnection.ExecuteAsync(query, entity);
        }

        public async Task<int> DeleteAsync(int id)
        {
            var tableName = FormatTableName<T>();
            var query = $"DELETE FROM {tableName} WHERE id = @Id";
            return await _dbConnection.ExecuteAsync(query, new { Id = id });
        }
        
        public async Task<int> DeleteByAsync(Expression<Func<T, int>> propertyExpression, object value)
        {
            var tableName = FormatTableName<T>();
            var propertyName = GetPropertyName(propertyExpression);
            var query = $"DELETE FROM {tableName} WHERE {ToSnakeCase(propertyName)} = {value}";
            
            return await _dbConnection.ExecuteAsync(query);
        }
        
        public async Task<int> DeleteByInAsync(Expression<Func<T, int>> propertyExpression, int[] values)
        {
            var tableName = FormatTableName<T>();
            var propertyName = GetPropertyName(propertyExpression);
            var query = $"DELETE FROM {tableName} WHERE {ToSnakeCase(propertyName)} IN ({string.Join(", ", values)})";
            
            return await _dbConnection.ExecuteAsync(query);
        }
        
        private string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            if (!(propertyExpression.Body is MemberExpression memberExpression))
            {
                throw new ArgumentException("Expression must be a MemberExpression", nameof(propertyExpression));
            }

            return memberExpression.Member.Name;
        }
        
        private string FormatTableName<T>()
        {
            return typeof(T).Name.ToLower().EndsWith("y") // words end like "Category, Strategy, etc"
                ? ToSnakeCase(typeof(T).Name.Remove(typeof(T).Name.Length - 1) + "ies")
                : $"{ToSnakeCase(typeof(T).Name)}s";
        }

        private string FormatValue(object value)
        {
            if (value is int)
            {
                return $"{value}";
            }

            if (value is string)
            {
                return $"'{value}'";
            }

            return null;
        }
        
        // Helper method to convert PascalCase to snake_case.
        private string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }
    }
}
