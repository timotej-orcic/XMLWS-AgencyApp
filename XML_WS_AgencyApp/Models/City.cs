using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XML_WS_AgencyApp.Models
{
    public class City
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        public long? MainServerId { get; set; }

        [Required]
        [MaxLength(90)]
        public string Name { get; set; }

        [Required]
        [MaxLength(30)]
        public string PostalCode { get; set; }

        public Country Country { get; set; }
    }
}