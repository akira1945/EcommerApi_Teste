using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApi.Data;

namespace EcommerceApi.Controllers
{
    public class TokensController
    {
        private readonly AppDbContext dbContext;

        public TokensController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        
    }
}