﻿using KotikiShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotikiShop.DataAccess.Repository.IRepository
{
    public interface ICartItemRepository : IRepository<CartItem>
    {
        public void Update(CartItem obj);
    }
}
