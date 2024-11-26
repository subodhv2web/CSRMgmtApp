using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using CSRMGMT.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CSRMGMT
{
    public class AppdbContext : IdentityDbContext<AppUser>
    {
        private string ConnectionString { get; }
        public AppdbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<AppUser> AppUser { get; set; }
        internal DbSet<LinksInfo> tblLinksInfo { get; set; }
        internal DbSet<LinkTypes> tblLinkTypes { get; set; }
        internal DbSet<Client> Client { get; set; }
        internal DbSet<LookupMaster> LookupMaster { get; set; }
        internal DbSet<CsrProject> CsrProject { get; set; }
        internal DbSet<Milestone> Milestone { get; set; }
        internal DbSet<ProjectAgency> ProjectAgency { get; set; }
        internal DbSet<State> State { get; set; }
        internal DbSet<City> City { get; set; }
        internal DbSet<ProjectAllocation> ProjectAllocation { get; set; }

    }
}
