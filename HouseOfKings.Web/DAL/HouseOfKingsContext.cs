using HouseOfKings.Web.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace HouseOfKings.Web.DAL
{
    public class HouseOfKingsContext : IdentityDbContext<ApplicationUser>
    {
        public HouseOfKingsContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<GameGroup> GameGroup { get; set; }

        public DbSet<Rule> Rule { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public static HouseOfKingsContext Create()
        {
            return new HouseOfKingsContext();
        }
    }
}