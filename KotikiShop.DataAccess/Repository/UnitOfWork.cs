using KotikiShop.DataAccess.Data;
using KotikiShop.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotikiShop.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            ApplicationUser = new ApplicationUserRepository(_db);
            Cat = new CatRepository(_db);
            CatFamily = new CatFamilyRepository(_db);
            CatComment = new CatCommentRepository(_db);
            Cart = new CartRepository(_db);
            CartItem = new CartItemRepository(_db);
        }

        public IApplicationUserRepository ApplicationUser { get; private set; }
        public ICatRepository Cat { get; }
        public ICatFamilyRepository CatFamily { get; }
        public ICatCommentRepository CatComment { get; }
        public ICartRepository Cart { get; }
        public ICartItemRepository CartItem { get; }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
