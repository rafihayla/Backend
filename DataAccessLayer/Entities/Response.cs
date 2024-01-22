using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class Response
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
    }

    public class TokenResponse : Response
    {
        public required string Token { get; set; }
    }
}
