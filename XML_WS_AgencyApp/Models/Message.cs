using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XML_WS_AgencyApp.Models
{
    public class Message
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [MaxLength(1000)]
        public string Content { get; set; }

        public long RegisteredUserId { get; set; }

        public ApplicationUser AgentUser { get; set; }
    }
}