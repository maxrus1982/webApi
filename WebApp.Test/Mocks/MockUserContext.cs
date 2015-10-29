using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebApp.Core;

namespace WebApp.Test
{
    public class MockUserContext : IUserContext
    {
        public string UserID { get; set; }
    }
}
