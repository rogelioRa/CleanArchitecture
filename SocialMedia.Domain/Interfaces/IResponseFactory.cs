using SocialMedia.Domain.Responses;

namespace SocialMedia.Domain.Interfaces
{
    public interface IResponseFactory<T>
    {
         ApiResponse<T> GetResponse(object source);
    }
}