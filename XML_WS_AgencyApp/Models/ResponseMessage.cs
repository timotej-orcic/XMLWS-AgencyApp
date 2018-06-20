using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XML_WS_AgencyApp.Models
{
    public class ResponseMessage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [MaxLength(1000)]
        public string Content { get; set; }
    }
}