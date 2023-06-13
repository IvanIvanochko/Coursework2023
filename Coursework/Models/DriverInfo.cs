using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coursework.Models
{
    [Table("DriverInfo", Schema = "dbo")]
    public class DriverInfo
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "UserInfo ID is required.")]
        public int UserInfoID { get; set; }

        [Required(ErrorMessage = "Vehicle ID is required.")]
        public int VehicleID { get; set; }

        // Define navigation properties if needed
        [ForeignKey("UserInfoID")]
        public UserTable UserInfo { get; set; }

        [ForeignKey("VehicleID")]
        public CarTable Vehicle { get; set; }

        public DriverInfo()
        {
            // Default constructor
        }
        public DriverInfo(int userInfoID, int vehicleID)
        {
            UserInfoID = userInfoID;
            VehicleID = vehicleID;
        }
    }
}
