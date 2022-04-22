using System;
using System.Threading.Tasks;
using SocialMedia.Domain.Entities;

namespace SocialMedia.Domain.Interfaces
{
    public interface IUnitOfWOrk: IDisposable
    {
         IPostRepository postRepository { get; }
         IRepository<User> userRepository { get; }
         IRepository<Comment> commentRepository { get; }

         Task SaveChangesAsync();
         void SaveChanges();

    }
}