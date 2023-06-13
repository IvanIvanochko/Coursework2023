using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coursework.Models
{
    [Table("adressInfo", Schema = "dbo")]
    public class AddressInfoModel
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(255)]
        public string City { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(255)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Company Address is required.")]
        public bool CompanyAddress { get; set; }
    }
}
