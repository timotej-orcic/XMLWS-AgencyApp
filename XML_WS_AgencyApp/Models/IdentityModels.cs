using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace XML_WS_AgencyApp.Models
{
    public class CustomUserRole : IdentityUserRole<long> { }
    public class CustomUserClaim : IdentityUserClaim<long> { }
    public class CustomUserLogin : IdentityUserLogin<long> { }

    public class CustomRole : IdentityRole<long, CustomUserRole>
    {
        public CustomRole() { }
        public CustomRole(string name) { Name = name; }
    }

    public class CustomUserStore : UserStore<ApplicationUser, CustomRole, long,
        CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public CustomUserStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }

    public class CustomRoleStore : RoleStore<CustomRole, long, CustomUserRole>
    {
        public CustomRoleStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<long, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        [Required]
        [MinLength(13), MaxLength(13)]
        public string Pmb { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, long> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, CustomRole, long, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public ApplicationDbContext()
            : base("DatabaseContext")
        {
        }

        public static ApplicationDbContext Create()
        {
            ApplicationDbContext ctx = new ApplicationDbContext();
            return ctx;
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<AccomodationType> AccomodationTypes { get; set; }
        public DbSet<BookingUnit> BookingUnits { get; set; }
        public DbSet<BonusFeatures> BonusFeatures { get; set; }
        public DbSet<BookingUnitPicture> Pictures { get; set; }

        protected override void OnModelCreating(DbModelBuilder mb)
        {
            mb.Entity<ApplicationUser>().Property(x => x.Email).HasMaxLength(90);
            mb.Entity<ApplicationUser>().Property(x => x.UserName).HasMaxLength(60);



            base.OnModelCreating(mb);
        }

        public async void addInitialUsers()
        {
            using (var um = new UserManager<ApplicationUser, long>(new CustomUserStore(new ApplicationDbContext())))
            {
                IdentityResult admin;
                if (um.Users.FirstOrDefault(usr => usr.Email == "main.admin@xml.com") == null)
                {
                    ApplicationUser mainAdmin = new ApplicationUser
                    {
                        Email = "main.admin@xml.com",
                        Pmb = "0123456789012",
                        UserName = "MainAdmin"
                    };
                    admin = await um.CreateAsync(mainAdmin, "MAdmin123!");
                }
            }
        }
    }
}