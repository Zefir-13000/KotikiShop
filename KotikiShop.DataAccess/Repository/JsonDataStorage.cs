using KotikiShop.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KotikiShop.DataAccess.Repository
{
    public class JsonDataStorage<T> : IDataStorage<T> where T : class
    {
        private readonly string _filePath;
        private readonly List<T> _data;
        private readonly object _lock = new();

        public JsonDataStorage(string filePath)
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
                return new List<T>(); // Handle error (e.g., log exception)
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

        public T Get(int id)
        {
            return _data.FirstOrDefault(x => x.GetHashCode() == id);
        }

        public IEnumerable<T> GetAll()
        {
            return _data;
        }

        public void Add(T entity)
        {
            lock (_lock)
            {
                _data.Add(entity);
                SaveData();
            }
        }

        public void Remove(int id)
        {
            lock (_lock)
            {
                var entity = Get(id);
                if (entity != null)
                {
                    _data.Remove(entity);
                    SaveData();
                }
            }
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

        public void Update(T entity)
        {
            lock (_lock)
            {
                var existing = _data.FirstOrDefault(x => x.GetHashCode() == entity.GetHashCode());
                if (existing != null)
                {
                    _data.Remove(existing);
                    _data.Add(entity);
                    SaveData();
                }
            }
        }
    }
}
