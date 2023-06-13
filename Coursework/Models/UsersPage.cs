using Microsoft.EntityFrameworkCore;

namespace Coursework.Models
{
    public class UserPageViewModel
    {
        public List<UserTable> Users { get; set; }
        public List<RolesModel> Roles { get; set; }
    }
}
