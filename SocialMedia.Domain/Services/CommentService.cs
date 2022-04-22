using SocialMedia.Domain.Interfaces;

namespace SocialMedia.Domain.Services
{

  public class CommentService : ICommentService
  {
    private readonly IUserService userService;
    public CommentService(IUserService userService)
    {
      this.userService = userService;
    }

    public bool HasComment(long postId)
    {
      return postId >= 0;
    }
  }
}