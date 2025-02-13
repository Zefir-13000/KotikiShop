using KotikiShop.DataAccess.Data;
using KotikiShop.DataAccess.Repository.IRepository;
using KotikiShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotikiShop.DataAccess.Repository
{
    public class CatRepository : Repository<Cat>, ICatRepository
    {
        private ApplicationDbContext _db;
        public CatRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        void ICatRepository.Update(Cat obj)
        {
            _db.Cats.Update(obj);
        }
    }
}
