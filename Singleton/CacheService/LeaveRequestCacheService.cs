using System;
using System.Collections.Generic;
using Employee_System.Models;

namespace Employee_System.Caching
{
    public class LeaveRequestCacheService
    {
        private static readonly Lazy<LeaveRequestCacheService> _instance = new Lazy<LeaveRequestCacheService>(() => new LeaveRequestCacheService());
        private readonly Dictionary<int, LeaveRequest> _cache = new Dictionary<int, LeaveRequest>();

        // Singleton instance
        public static LeaveRequestCacheService Instance => _instance.Value;

        private LeaveRequestCacheService() { }

        // Get leave request by ID from cache
        public LeaveRequest Get(int id)
        {
            _cache.TryGetValue(id, out var leaveRequest);
            return leaveRequest;
        }

        // Add leave request to cache
        public void Add(int id, LeaveRequest leaveRequest)
        {
            if (!_cache.ContainsKey(id))
            {
                _cache[id] = leaveRequest;
            }
        }

        // Remove leave request from cache
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
