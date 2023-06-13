using System.ComponentModel.DataAnnotations.Schema;

namespace Coursework.Models
{
    [Table("OrderType", Schema = "dbo")]
    public class OrderTypeModel
    {
        public int ID { get; set; }
        public string OrderType { get; set; }
    }

}
