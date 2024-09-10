using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApi.Models;
using Microsoft.VisualBasic;

namespace EcommerceApi.DTOs
{
    public class LoginClientDTO
    {
        public required string email { get; set; }
        public required string password { get; set; }

    }

    public class LoginSellerDTO
    {
        public required string email { get; set; }
        public required string password { get; set; }

    }

    public class UserDTO
    {
        public int id { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        [NotMapped]
        public string? token { get; set; }
    }

    public class TestDTO
    {
        public string? token { get; set; }
    }

    public class ListClientDTO
    {
        public int id { get; set; }
        public string reference { get; set; }
        public string cpf_cnpj { get; set; }
        public Collection<Client> clients { get; set; }
        public Collection<Seller> sellers { get; set; }

    }

}