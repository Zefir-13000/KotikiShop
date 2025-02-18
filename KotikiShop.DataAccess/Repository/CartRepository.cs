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
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private ApplicationDbContext _db;
        public CartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        void ICartRepository.Update(Cart obj)
        {
            _db.Carts.Update(obj);
        }
    }
}
