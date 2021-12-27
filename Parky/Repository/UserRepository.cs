using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ParkyApi.Data;
using ParkyApi.Models;
using ParkyApi.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace ParkyApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly AppSettings appSettings;
        public UserRepository(ApplicationDbContext _context, IOptions<AppSettings> appSettings)
        {
            this._context = _context;
            this.appSettings = appSettings.Value;
        }
        public User Authenticate(string UserName, string Password)
        {
            var user = _context.Users.Where(x=>x.Username == UserName && x.Password == Password).FirstOrDefault();
            if (user == null)
                return null;

            //if user is found
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = System.DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.Password = "";

            return user;
        }

        public bool IsUserUnique(string username)
        {
           var user = _context.Users.SingleOrDefault(x => x.Username == username);

            //returns true when user is null
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public User Register(string UserName, string Password)
        {
            User user = new User()
            {
                Username = UserName,
                Password = Password,
                Role = "Admin"
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            user.Password = "";
            return user;
        }
    }
}
