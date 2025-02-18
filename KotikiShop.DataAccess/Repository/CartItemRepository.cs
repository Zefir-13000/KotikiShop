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
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        private ApplicationDbContext _db;
        public CartItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        void ICartItemRepository.Update(CartItem obj)
        {
            _db.CartItems.Update(obj);
        }
    }
}
