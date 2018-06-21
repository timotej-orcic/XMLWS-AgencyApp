using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XML_WS_AgencyApp.Models
{
    public class Message
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        public long? MainServerId { get; set; }

        [MaxLength(1000)]
        public string Content { get; set; }

        public RegisteredUserInfo RegisteredUserInfo { get; set; }

        public long AgentUserId { get; set; }

        public bool IsRead { get; set; }

        public bool HasResponse { get; set; }

        public ResponseMessage ResponseMessage { get; set; }
    }
}