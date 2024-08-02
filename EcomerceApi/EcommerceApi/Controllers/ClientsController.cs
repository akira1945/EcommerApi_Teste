using System.Text;
using EcommerceApi.DTOs;
using EcommerceApi.Data;
using EcommerceApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcommerceApi.Repositories;

namespace EcommerceApi.Controllers
{
    [ApiController]
    [Route("clients")]

    public class ClientsController : ControllerBase
    {
        private readonly ClientsRepository _clientsRepository; // Propriedade do banco de dados

        public ClientsController(ClientsRepository clientsRepository) // Construtor
        {
            _clientsRepository = clientsRepository;
        }

        // Metodo para ler todos os clientes.

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> Get()
        {
            var clients = await _clientsRepository.GetAll();
            return Ok(clients);
        }

        // Metodo para Ler clientes por ID
        [HttpPost("researh_client")]
        [Consumes("application/json")]
        public async Task<ActionResult<Client>> GetId([FromBody] researchClientsDto researchClientsId )
        {
            var client = await _clientsRepository.GetId(researchClientsId);
            if (client == null)
            {
                return NotFound("Cliente não encontrado, Valide o ID informado!");
            }
            
            return Ok(client);  
        }

        // Metodo para criar um cliente
        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult<Client>> CreateClient([FromBody] CreateClientsDto createClients)
        {
            var client = await _clientsRepository.Create(createClients).ConfigureAwait(false);
            
            return Ok(client);
        }

        //Metodo para alterar informações especificas de um cadastro.
        [HttpPut]
        [Consumes("application/json")]
        public async Task<ActionResult<Client>> UpdateClients([FromBody] UpdateClientsDto updateClient)
        {
            var client = await _clientsRepository.Update(updateClient);
            switch (client)
            {
                case null:
                    return NotFound("Cliente não encontrado, Valide o ID informado!");
                default:
                    return Ok(client);
            }
        }

        //Metodo para deletar um cadastro
        [HttpDelete]
        [Consumes("application/json")]
        public async Task<IActionResult> DeleteClient([FromBody] deleteClientsDto deleteClients)
        {
            var client = await _clientsRepository.Delete( deleteClients );
            if(client == null)
            {
                return NotFound($"Cadastro a ser deletado não encontrado, valide o ID: {deleteClients.id}  ou CPF informado {deleteClients.cpf_cnpj}, pertencem ao cliente {deleteClients.name}!!");
            }
            
            return Ok($"Cadastro do cliente {deleteClients.name} excluído com sucesso!!!");
        }
    }
}