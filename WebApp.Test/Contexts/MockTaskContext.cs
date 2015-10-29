using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebApp.Domain;
using WebApp.DAL.Mapping;
using WebApp.Domain.Interface;
using WebApp.Service.Interface;

namespace WebApp.Test
{
    public class MockTaskContext : MockBaseContext, ITaskContext
    {

    }
}
