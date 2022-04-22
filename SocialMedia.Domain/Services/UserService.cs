using SocialMedia.Domain.Interfaces;

namespace SocialMedia.Domain.Services
{
  public class UserService : IUserService
  {
    private readonly ICommentService commentService;
    public UserService(ICommentService commentService)
    {
      this.commentService = commentService;
    }

    public bool HasComment(long postId)
    {
      return postId >= 0;
    }

  }
}