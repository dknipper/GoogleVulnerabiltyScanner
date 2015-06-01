using System.Data.Entity;

namespace DorkDataAccess
{
    public class DorkDatabaseContext : DbContext
    {
        public DbSet<GoogleDork> GoogleDorks { get; set; }
        public DbSet<GoogleDorkParent> GoogleDorkParents { get; set; }
        public DbSet<VulnerableSite> VulnerableSites { get; set; }
        public DbSet<FullGoogleDork> FullGoogleDorks { get; set; }

        public DorkDatabaseContext()
            : base("Name=DorkEntities")
        {
            
        }

        static DorkDatabaseContext()
        {
            Database.SetInitializer<DorkDatabaseContext>(null);
        }
    }
}
