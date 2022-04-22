using System.Collections.Generic;
using System.Threading.Tasks;
using SocialMedia.Domain.Entities;

namespace SocialMedia.Domain.Interfaces
{
    public interface IUserRepository
    {
          Task<IEnumerable<User>> Get();
          Task<User> Get(int id);
    }
}