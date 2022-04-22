using System.Threading.Tasks;
using SocialMedia.Domain.Entities;
using SocialMedia.Domain.Interfaces;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories
{
  public class UnitOfWork : IUnitOfWOrk
  {
    private readonly IPostRepository _postRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Comment> _commentRepository;
    private readonly RestFullApiContext _context; 
    public UnitOfWork(RestFullApiContext _context)
    {
        this._context = _context;
    }
    public IPostRepository postRepository => this._postRepository ?? new PostRepository(this._context);

    public IRepository<User> userRepository => this._userRepository ?? new Repository<User>(this._context);

    public IRepository<Comment> commentRepository => this._commentRepository ?? new Repository<Comment>(this._context);
    

    public void Dispose()
    {
      if(this._context != null)
      {
          this._context.Dispose();
      }
    }

    public async Task SaveChangesAsync()
    {
      await this._context.SaveChangesAsync();
    }

    public void SaveChanges()
    {
        this._context.SaveChanges();
    }
  }
}