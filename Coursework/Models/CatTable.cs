using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coursework.Models
{
    [Table("CarTable", Schema = "dbo")]
    public class CarTable
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Car Type is required.")]
        [MaxLength(255)]
        public string CarType { get; set; }

        [Required(ErrorMessage = "Year is required.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Brand is required.")]
        [MaxLength(255)]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Model is required.")]
        [MaxLength(255)]
        public string Model { get; set; }

        [Required(ErrorMessage = "Capacity (kg) is required.")]
        [Range(0, 99999999.99, ErrorMessage = "Capacity (kg) must be a non-negative number.")]
        public decimal Capacity_kg { get; set; }
    }
}
