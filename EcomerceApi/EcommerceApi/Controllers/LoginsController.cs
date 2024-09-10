using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApi.Data;
using EcommerceApi.DTOs;
using EcommerceApi.Models;
using EcommerceApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Controllers
{
    [ApiController]
    [Route("login")]
    public class LoginsController : ControllerBase
    {
        private readonly LoginsRepository _loginsRepository;

        public LoginsController(LoginsRepository loginsRepository)
        {
            _loginsRepository = loginsRepository;
        }

        [HttpGet("publica")]
        public ActionResult<IEnumerable<Token>> Publica ()
        {
            
            return Ok($"Rota Publica: ");
        }
        
    }
}