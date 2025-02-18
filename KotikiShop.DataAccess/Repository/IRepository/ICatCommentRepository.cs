using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotikiShop.DataAccess.Data;
using KotikiShop.Models;

namespace KotikiShop.DataAccess.Repository.IRepository
{
    public interface ICatCommentRepository : IRepository<CatComment>
    {
        public void Update(CatComment obj);
    }
}
