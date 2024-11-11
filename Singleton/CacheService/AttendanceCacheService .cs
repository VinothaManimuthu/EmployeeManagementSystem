using System;
using System.Collections.Generic;
using Employee_System.Models;

namespace Employee_System.Caching
{
    public class AttendanceCacheService
    {
        private static readonly Lazy<AttendanceCacheService> _instance = new Lazy<AttendanceCacheService>(() => new AttendanceCacheService());
        private readonly Dictionary<int, Attendance> _cache = new Dictionary<int, Attendance>();

        // Singleton instance
        public static AttendanceCacheService Instance => _instance.Value;

        private AttendanceCacheService() { }

        // Get attendance by ID from cache
        public Attendance Get(int id)
        {
            _cache.TryGetValue(id, out var attendance);
            return attendance;
        }

        // Add attendance to cache
        public void Add(int id, Attendance attendance)
        {
            if (!_cache.ContainsKey(id))
            {
                _cache[id] = attendance;
            }
        }

        // Remove attendance from cache
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
