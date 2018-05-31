using System.Collections.Generic;
using XML_WS_AgencyApp.Models;

namespace XML_WS_AgencyApp
{
    public class Initializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        public Initializer()
        {
        }

        protected override void Seed(ApplicationDbContext context)
        {
            
        }
    }
}