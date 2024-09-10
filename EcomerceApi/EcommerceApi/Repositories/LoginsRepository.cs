using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApi.Data;
using EcommerceApi.DTOs;
using EcommerceApi.Models;
using EcommerceApi.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Repositories
{
    public class LoginsRepository
    {
         private readonly AppDbContext _dbContext;
         private readonly TokenServices _tokenServices;

        public LoginsRepository(AppDbContext dbContext, TokenServices tokenServices )
        {
            _dbContext = dbContext;
            _tokenServices = tokenServices;
        }


    }
}