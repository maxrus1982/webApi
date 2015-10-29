using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebApp.Core;

namespace WebApp
{
    public class UserContext : IUserContext
    {
        public String UserID { get; set; }
    }
}
