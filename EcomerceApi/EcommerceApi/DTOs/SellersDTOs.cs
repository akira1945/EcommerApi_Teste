namespace EcommerceApi.DTOs
{
    public class createSellersDto
    {
        public required string name { get; set; }
        public required string reference { get; set; }
        public string? phone { get; set; }
        public required string email { get; set; }
        public required string password { get; set; }        
    }

    public class researchSellersDto
    {
        public int id { get; set; }
        public required string reference { get; set; }
    }

    public class updateSellersDto
    {
        public int id { get; set; }
        public  string? name { get; set; }
        public  string? reference { get; set; }
        public string?  phone { get; set; }
        public  string? email { get; set; }
        public  string? password { get; set; }
    }
    public class deleteSellersDto
    {
        public int id { get; set; }
        public required string reference { get; set; }
    }

      public class deleteSellersIdDto
    {
        public int id { get; set; }
    }
    public class ServicesSellersModelReferenceDto
    {
        public required string reference { get; set; }
    }
    
}