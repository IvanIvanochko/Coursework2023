using Microsoft.EntityFrameworkCore;

namespace Coursework.Models
{
    public class DriverPageViewModel
    {
        public List<UserTable> Users { get; set; }
        public List<CarTable> Cars { get; set; }
        public DriverInfo DriverInfo { get; set; }
    }
}
