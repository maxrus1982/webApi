using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace WebApp.DAL
{
    public class MainConfiguration : DbConfiguration
    {
        public MainConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
        }
    }
}