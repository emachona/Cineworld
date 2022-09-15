using Cinema.Models;

namespace Cinema.ViewModels
{
    public class UsersFilterVM
    {
        public IEnumerable<Client> users { get; set; }
        public string searchUser { get; set; }
    }
}
