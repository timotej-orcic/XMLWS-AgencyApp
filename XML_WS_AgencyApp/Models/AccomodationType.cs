using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XML_WS_AgencyApp.Models
{
    public class AccomodationType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        public long? MainServerId { get; set; }

        [Required]
        [MaxLength(60)]
        public string Name { get; set; }
    }
}