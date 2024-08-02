using System.ComponentModel.DataAnnotations.Schema;
using EcommerceApi.Data;

namespace EcommerceApi.Models
{
    public class Seller
    {
        public int id { get; set; }
        public required string name { get; set; }
        public required string reference { get; set; }
        public string? phone { get; set; }
        public required string email { get; set; }
        public required string password { get; set; }
        
    }

}