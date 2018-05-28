using Microsoft.Owin;
using Owin;
using XML_WS_AgencyApp.Models;

[assembly: OwinStartupAttribute(typeof(XML_WS_AgencyApp.Startup))]
namespace XML_WS_AgencyApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            ApplicationDbContext AppContext = ApplicationDbContext.Create();
            Initializer Initializer = new Initializer();
            Initializer.InitializeDatabase(AppContext);
        }
    }
}
