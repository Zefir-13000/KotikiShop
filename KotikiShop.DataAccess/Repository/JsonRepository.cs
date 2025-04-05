using KotikiShop.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KotikiShop.DataAccess.Repository
{
    public class JsonRepository<T> : IRepository<T> where T : class
    {
        private readonly string _filePath;
        private readonly List<T> _data;
        private readonly object _lock = new();

        public JsonRepository(string filePath)
        {
            _filePath = filePath;
            _data = LoadData();
        }

        private List<T> LoadData()
        {
            if (!File.Exists(_filePath)) return new List<T>();
            try
            {
                string json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
            catch
            {
                return new List<T>();
            }
        }

        private void SaveData()
        {
            lock (_lock)
            {
                string json = JsonSerializer.Serialize(_data, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
            }
        }

        public void Add(T entity)
        {
            lock (_lock)
            {
                _data.Add(entity);
                SaveData();
            }
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            return filter == null ? _data : _data.AsQueryable().Where(filter);
        }

        public IEnumerable<T> GetAllAsNoTracking(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            return GetAll(filter, includeProperties);
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = true)
        {
            return _data.AsQueryable().FirstOrDefault(filter);
        }

        public void Remove(T entity)
        {
            lock (_lock)
            {
                if (_data.Contains(entity))
                {
                    _data.Remove(entity);
                    SaveData();
                }
            }
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            lock (_lock)
            {
                _data.RemoveAll(x => entities.Contains(x));
                SaveData();
            }
        }
    }

}
