using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using SocialMedia.Domain.Entities;

namespace SocialMedia.Domain.Interfaces
{
    public interface IPostRepository: IRepository<Post>
    {
        Task<IEnumerable<Post>> GetUserPost(int userId);

    }
}
