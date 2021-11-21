using MySql.Data.EntityFramework;
using System.Data.Entity;
using XSquareCalculationsApi.Entities;

namespace XSquareCalculationsApi.Persistance
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class XSquareCalculationContext : DbContext
    {
        public XSquareCalculationContext()
            : base("XSquareCalculationContext")
        {
        }

        public DbSet<Template> Templates { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Authenticate> Authenticates { get; set; }
    }
}