using SocialMedia.Domain.CustomEntities;

namespace SocialMedia.Domain.Responses
{
    public class ApiResponse<T>
    {
        public ApiResponse(T data)
        {
            this.Data = data;
        }
        public T Data { get; set; }

        public Metadata metadata { get; set; }

    }
}