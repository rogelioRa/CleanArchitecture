using System.Collections.Generic;
using System.Threading.Tasks;
using SocialMedia.Domain.CustomEntities;
using SocialMedia.Domain.DTOs;
using SocialMedia.Domain.Entities;
using SocialMedia.Domain.QueryFilters;

namespace SocialMedia.Domain.Interfaces
{
    public interface IPostService
    {
        PageList<Post> Get(PostQueryFilters filter);
        Task Store(Post post);
        Task<Post> Show(int id);
        Task<bool> Update(Post post);
        Task<bool> Delete(int id);
    }
}