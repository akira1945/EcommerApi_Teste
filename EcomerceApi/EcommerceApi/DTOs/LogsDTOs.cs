using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceApi.DTOs
{
    public class LogsDTOs
    {
        public class MessageWithoutClientDTO
        {
            public required string message { get; set; }
        }

        public class MessageWithClientDTO
        {
            public required string message { get; set; }

            public required string connectionId { get; set; }
        }
    }
}