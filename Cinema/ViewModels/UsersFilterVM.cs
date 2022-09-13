using Cinema.Models;

namespace Cinema.ViewModels
{
    public class UsersFilterVM
    {
        public IEnumerable<User> users { get; set; }
        public string searchUser { get; set; }
    }
}
