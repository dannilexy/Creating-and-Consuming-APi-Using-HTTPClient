using ParkyApi.Models;

namespace ParkyApi.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUserUnique (string username);
        User Authenticate(string UserName, string Password);
        User Register(string UserName, string Password);
    }
}
