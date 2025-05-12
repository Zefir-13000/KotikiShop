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
    public class CatLikesRepository : Repository<CatLike>, ICatLikesRepository
    {
        private ApplicationDbContext _db;
        public CatLikesRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        void ICatLikesRepository.Update(CatLike obj)
        {
            _db.CatLikes.Update(obj);
        }
    }
}