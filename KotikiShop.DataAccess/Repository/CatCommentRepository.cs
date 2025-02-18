using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotikiShop.DataAccess.Data;
using KotikiShop.DataAccess.Repository.IRepository;
using KotikiShop.Models;

namespace KotikiShop.DataAccess.Repository
{
    public class CatCommentRepository : Repository<CatComment>, ICatCommentRepository
    {
        private ApplicationDbContext _db;
        public CatCommentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        void ICatCommentRepository.Update(CatComment obj)
        {
            _db.CatComments.Update(obj);
        }
    }
}
