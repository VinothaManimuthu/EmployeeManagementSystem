using System;
using System.Collections.Generic;
using Employee_System.Models;

namespace Employee_System.Caching
{
    public class EmployeeCacheService
    {
        private static readonly Lazy<EmployeeCacheService> _instance = new Lazy<EmployeeCacheService>(() => new EmployeeCacheService());
        private readonly Dictionary<int, Employee> _cache = new Dictionary<int, Employee>();

        // Singleton instance
        public static EmployeeCacheService Instance => _instance.Value;

        private EmployeeCacheService() { }

        // Get employee by ID from cache
        public Employee Get(int id)
        {
            _cache.TryGetValue(id, out var employee);
            return employee;
        }

        // Add employee to cache
        public void Add(int id, Employee employee)
        {
            if (!_cache.ContainsKey(id))
            {
                _cache[id] = employee;
            }
        }

        // Remove employee from cache
        public void Remove(int id)
        {
            if (_cache.ContainsKey(id))
            {
                _cache.Remove(id);
            }
        }

        // Clear all cache
        public void Clear()
        {
            _cache.Clear();
        }
    }
}
