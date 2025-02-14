using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotikiShop.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICatRepository Cat { get; }
        ICatFamilyRepository CatFamily { get; }
        IApplicationUserRepository ApplicationUser { get; }

        void Save();
    }
}
