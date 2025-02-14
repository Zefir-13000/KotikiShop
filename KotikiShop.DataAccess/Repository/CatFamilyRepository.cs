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
    public class CatFamilyRepository : Repository<CatFamily>, ICatFamilyRepository
    {
        private ApplicationDbContext _db;
        public CatFamilyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        void ICatFamilyRepository.Update(CatFamily obj)
        {
            _db.CatFamilies.Update(obj);
        }
    }
}
