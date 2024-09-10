using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Data;
using EcommerceApi.DTOs;
using EcommerceApi.Models;
using EcommerceApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using EcommerceApi.Repositories;

namespace EcommerceApi.Controller
{
    [ApiController]
    [Route("sellers")]

    public class SellersController : ControllerBase
    {

        private readonly SellersRepository _sellersRepository;
        private readonly SellersServices _sellersServices;
        private readonly TokenServices _tokensServices;

        public SellersController(SellersRepository sellersRepository, SellersServices sellersServices, TokenServices tokensServices)
        {
            _sellersRepository = sellersRepository;
            _sellersServices   = sellersServices;
            _tokensServices = tokensServices;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Seller>>> GetAll()
        {
            var sellers = await _sellersRepository.GetAll();
            return Ok(sellers);
        }


        [HttpPost("research_seller")]
        [Consumes("application/json")]
        public async Task<ActionResult<Seller>> Research([FromBody] researchSellersDto research)
        {
            var sellers = await _sellersRepository.GetResearch(research);
            if (sellers == null)
            {
                return NotFound($"Vendedor(a) não encontrado. Id: {research.id} e a Referencia: {research.reference} invalidados!");
            }
            return Ok(sellers);
        }


        [HttpPost("create_seller")]
        [Consumes("application/json")]
        public async Task<ActionResult<Seller>> Create([FromBody] createSellersDto createSellers)
        {
            var Reference = await _sellersRepository.Create(createSellers);

            if (Reference == null)
            {
                return BadRequest($@"Referencia {createSellers.reference} já existe, por favor, utilize outra!");
            }

            return Ok(Reference);
        }

        [HttpPut("update_all")]
        [Consumes("application/json")]
        public async Task<ActionResult<Seller>> UpdateSellers([FromBody] updateSellersDto update)
        {

            bool validRef = _sellersServices.ValidReference(update.reference);

            if (validRef) return BadRequest($"Referencia {update.reference} não pode ser utilizada!!!");


            var seller = await _sellersRepository.update(update);

            if (seller == null) return NotFound($"Cadastro de vendedor não encontrado. Por favor, valide o ID: {update.id}.");


            return Ok(seller);

        }



        [HttpPost("delete_sellers_id_ref")]
        [Consumes("application/json")]
        public async Task<ActionResult<Seller>> Delete([FromBody] deleteSellersDto deleteSellers)
        {
            var seller = await _sellersRepository.Delete(deleteSellers);
            if (seller == null)
            {
                return NotFound($"Cadastro não encontrado, valide se o id: {deleteSellers.id} e a Referencia: {deleteSellers.reference}, estão corretas!");
            }

            return Ok($"Cadastro excluido com sucesso!!! Referencia {deleteSellers.reference}, pode ser utilizada novamente.");
        }

        [HttpDelete("delete_sellers_id/{id}")]
        public async Task<ActionResult> DeleteId(int id)
        {
            var seller = await _sellersRepository.DeleteId(id);
            if (seller == null)
            {
                return NotFound($"Exclusão não executada, valide o ID: {id}");
            }

            return Ok(seller);
        }
        
        [HttpPost("login_seller")]
        [Consumes("application/json")]

        public async Task<ActionResult<dynamic?>> LoginSeller([FromBody] LoginSellerDTO logon)
        {
            var logonSeller = await _sellersRepository.LoginSeller(logon);
            if(logonSeller == null) return BadRequest("User or Password Incorrect!!!");

             var token = await _tokensServices.GenerateAndSaveToken();

             logonSeller.token = token.token;

            return Ok(logonSeller);
        }


    }

}

