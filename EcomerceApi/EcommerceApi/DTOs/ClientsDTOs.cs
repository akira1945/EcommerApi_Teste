namespace EcommerceApi.ClientsDTOs
{
    public class researchClientsDto
    {
        public int id { get; set; }
    }
    
    public class CreateClientsDto
    {
        public required string name { get; set; }
        public required string cpf_cnpj { get; set; }
        public string? phone { get; set; }
        public required string email { get; set; }
        public required string password { get; set; }
        public required string street { get; set; }
        public string? number { get; set; }
        public string? complement { get; set; }
        public required string city { get; set; }
        public required string state { get; set; }
        public required string postal_code { get; set; }
        public required string country { get; set; }  
    }

    public class UpdateClientsDto
    {
        public int id { get; set; }
        public  string? name { get; set; }
        public  string? cpf_cnpj { get; set; }
        public string?  phone { get; set; }
        public  string? email { get; set; }
        public  string? password { get; set; }
        public  string? street { get; set; }
        public string?  number { get; set; }
        public string?  complement { get; set; }
        public  string? city { get; set; }
        public  string? state { get; set; }
        public  string? postal_code { get; set; }
        public  string? country { get; set; }  
    }

    public class deleteClientsDto
    {
        public int id { get; set; }
        public string? cpf_cnpj { get; set; }
        public string? name { get; set; }

    }

}