using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Domain.Entities;
using SocialMedia.Domain.Interfaces;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories
{
  public class PostRepository : Repository<Post>, IPostRepository
  {
      public PostRepository(RestFullApiContext _context): base(_context)
      {
      }
    public async Task<IEnumerable<Post>> GetUserPost(int userId)
    {
        return await this.entity.Where(x => x.UserId == userId).ToListAsync();
    }
  }
}