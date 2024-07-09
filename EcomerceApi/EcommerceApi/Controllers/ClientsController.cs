using System.Text;
using EcommerceApi.DTOs;
using EcommerceApi.Data;
using EcommerceApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Controllers
{
    [ApiController]
    [Route("clients")]

    public class ClientsController : ControllerBase
    {
        private readonly AppDbContext _dbContext; // Propriedade do banco de dados

        public ClientsController(AppDbContext dbContext) // Construtor
        {
            _dbContext = dbContext;
        }

        // Metodo para ler todos os clientes.

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetAll()
        {
            var clients = await _dbContext.Clients.ToListAsync();
            return Ok(clients);
        }

        // Metodo para Ler clientes por ID
        [HttpPost("researh_client")]
        [Consumes("application/json")]
        public async Task<ActionResult<Client>> GetId([FromBody] researchClientsDto researchClientsId )
        {
            var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.id == researchClientsId.id);
            if (client == null)
            {
                return NotFound("Cliente não encontrado, Valide o ID informado!");
            }
            
            return Ok(client);  
        }

        // Metodo para criar um cliente
        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult<Client>> Create([FromBody] CreateClientsDto createClients)
        {
            var client = new Client 
            {
                name = createClients.name,
                cpf_cnpj = createClients.cpf_cnpj,
                phone = createClients.phone,
                email = createClients.email,
                password = createClients.password,
                street = createClients.street,
                number = createClients.number,
                complement = createClients.complement,
                city = createClients.city,
                state = createClients.state,
                postal_code = createClients.postal_code,
                country = createClients.country  
            };         

            _dbContext.Clients.Add(client);
            await _dbContext.SaveChangesAsync();
            return Ok(client);
        }

        //Metodo para alterar informações especificas de um cadastro.
        [HttpPut]
        [Consumes("application/json")]
        public async Task<ActionResult<Client>> UpdateClients([FromBody] UpdateClientsDto updateClient)
        {
            var client = await _dbContext.Clients.FindAsync(updateClient.id);
          if(client == null)
            {
                return NotFound("Cliente não encontrado, Valide o ID informado!");
            }
            else
            {   
                if(updateClient.name != null){client.name = updateClient.name;};
                if(updateClient.cpf_cnpj != null){client.cpf_cnpj = updateClient.cpf_cnpj;};
                if(updateClient.phone != null){client.phone = updateClient.phone;};
                if(updateClient.email != null){client.email = updateClient.email;};
                if(updateClient.password != null){client.password = updateClient.password;};
                if(updateClient.street != null){client.street = updateClient.street;};
                if(updateClient.number != null){client.number = updateClient.number;};
                if(updateClient.complement != null){client.complement = updateClient.complement;};
                if(updateClient.city != null){client.city = updateClient.city;};
                if(updateClient.state != null){client.state = updateClient.state;};
                if(updateClient.postal_code != null){client.postal_code = updateClient.postal_code;};
                if(updateClient.country != null){client.country = updateClient.country;};
            }
            await _dbContext.SaveChangesAsync();
            return Ok(client);
        }

        //Metodo para deletar um cadastro
        [HttpDelete]
        [Consumes("application/json")]
        public async Task<IActionResult> Delete([FromBody] deleteClientsDto deleteClients)
        {
            var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.id == deleteClients.id || c.cpf_cnpj == deleteClients.cpf_cnpj);
            if(client == null)
            {
                return NotFound($"Cadastro a ser deletado não encontrado, valide o ID: {deleteClients.id}  ou CPF informado {deleteClients.cpf_cnpj}, pertencem ao cliente {deleteClients.name}!!");
            }
            _dbContext.Clients.Remove(client);
            await _dbContext.SaveChangesAsync();
            return Ok($"Cadastro do cliente {deleteClients.name} excluído com sucesso!!!");
        }
    }
}