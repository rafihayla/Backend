using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Helpers
{
    internal class AuthHelper
    {
        public static bool IsAuthorized(ApplicationDbContext _context, ClaimsIdentity? identity)
        {
            if (identity == null)
            {
                return false;
            }
            var id = identity.FindFirst("Id")?.Value;

            if (id == null)
            {
                return false;
            }

            var token = _context.Tokens.FirstOrDefault(t => t.Token == id);
            if (token == null)
            {
                return true;
            }
            return false;
        }
    }
}
