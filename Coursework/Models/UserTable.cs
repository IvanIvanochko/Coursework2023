using System.ComponentModel.DataAnnotations.Schema;

namespace Coursework.Models
{
    [Table("UserTable", Schema = "dbo")]
    public class UserTable
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime Birthdate { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
