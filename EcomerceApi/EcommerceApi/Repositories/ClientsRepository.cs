using EcommerceApi.Data;
using EcommerceApi.DTOs;
using EcommerceApi.Models;
using EcommerceApi.Services;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Repositories;

public class ClientsRepository
{
    private readonly AppDbContext _dbContext;
    private readonly TokenServices _tokenServices;

        public ClientsRepository(AppDbContext dbContext, TokenServices tokenServices)
        {
            _dbContext = dbContext;
            _tokenServices = tokenServices;
        }
       
    public async Task<IEnumerable<Client>> GetAll()
        {
            return await _dbContext.Clients.ToListAsync();
        }
    
    public async Task<IEnumerable<GetClientByEmialDto>> GetClientByEmail()
        {
            return await _dbContext.Set<GetClientByEmialDto>().ToListAsync();
        }
    public async Task<Client?> GetId( researchClientsDto researchClientsId )
        {
            return  await _dbContext.Clients.FirstOrDefaultAsync(c => c.id == researchClientsId.id);        
        }

    public async Task<Client> Create( CreateClientsDto createClients)
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

            return client;
        }

    public async Task<dynamic?> Update( UpdateClientsDto updateClient)
        {
            var client = await _dbContext.Clients.FindAsync(updateClient.id);

            if(client == null)
            {
                return null;
            }
            else{            
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

            return client;      
        }

    public async Task<Client?> Delete( deleteClientsDto deleteClients)
        {
            var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.id == deleteClients.id || c.cpf_cnpj == deleteClients.cpf_cnpj);
            if(client == null)
            {
                return null;
            }
           
            _dbContext.Clients.Remove(client);
            await _dbContext.SaveChangesAsync();    

            return client;

        }
            
            public async Task <dynamic?> LoginClient( LoginClientDTO login )
        {
            var parameters = new[]
            {
                new Npgsql.NpgsqlParameter("email", login.email),
                new Npgsql.NpgsqlParameter("password", login.password)
            };

            dynamic? user;

             user = await _dbContext.Set<UserDTO>().FromSqlRaw("SELECT * FROM clients WHERE email = @email AND password = @password LIMIT 1;  ", parameters).FirstOrDefaultAsync();

            if(user == null) return null;

            string token = _tokenServices.GenerateToken();

            user.token = token;

            return token;
        
        }




}
