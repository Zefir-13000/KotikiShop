using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotikiShop.DataAccess.Repository.IRepository
{
    public interface IDataStorage<T> where T : class
    {
        T Get(int id);
        IEnumerable<T> GetAll();
        void Add(T entity);
        void Remove(int id);
        void Remove(T entity);
        void Update(T entity);
    }
}
