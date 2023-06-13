using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coursework.Models
{
    [Table("Roles", Schema = "dbo")]
    public class RolesModel
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "User Info ID is required.")]
        public int UserInfoID { get; set; }

        public bool Driver { get; set; }

        public bool Admin { get; set; }

        // Define navigation property if needed
        [ForeignKey("UserInfoID")]
        public UserTable UserInfo { get; set; }
    }
}

