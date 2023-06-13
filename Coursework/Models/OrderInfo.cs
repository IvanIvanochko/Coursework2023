using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace Coursework.Models
{
    [Table("OrderInfo", Schema = "dbo")]
    public class OrderInfoModel
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "User Info ID is required.")]
        public int UserInfoID { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "From City ID is required.")]
        public int FromCityID { get; set; }

        [Required(ErrorMessage = "To City ID is required.")]
        public int ToCityID { get; set; }

        //[Required(ErrorMessage = "Driver ID is required.")]
        public int DriverID { get; set; }

        [Required(ErrorMessage = "Capacity (kg) is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Capacity (kg) must be a positive number.")]
        public decimal Capacity_kg { get; set; }

        [Required(ErrorMessage = "Order Type ID is required.")]
        public int OrderTypeID { get; set; }

        // Define navigation properties if needed
        [ForeignKey("UserInfoID")]
        public UserTable UserInfo { get; set; }

        [ForeignKey("FromCityID")]
        public AddressInfoModel FromCity { get; set; }

        [ForeignKey("ToCityID")]
        public AddressInfoModel ToCity { get; set; }

        [ForeignKey("DriverID")]
        public DriverInfo Driver { get; set; }

        [ForeignKey("OrderTypeID")]
        public OrderTypeModel OrderType { get; set; }
    }
}
