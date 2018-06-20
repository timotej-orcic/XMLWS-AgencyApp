using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XML_WS_AgencyApp.Models
{
    public class RegisteredUserInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        public long? MainServerId { get; set; }

        public string UserName { get; set; }
    }
}