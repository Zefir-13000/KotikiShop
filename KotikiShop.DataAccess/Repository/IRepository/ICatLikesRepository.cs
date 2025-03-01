using KotikiShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotikiShop.DataAccess.Repository.IRepository
{
    public interface ICatLikesRepository : IRepository<CatLike>
    {
        public void Update(CatLike obj);
    }
}
