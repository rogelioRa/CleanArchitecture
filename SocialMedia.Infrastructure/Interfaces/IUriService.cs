using System;
using SocialMedia.Domain.QueryFilters;

namespace SocialMedia.Infrastructure.Interfaces
{
    public interface IUriService
    {
         Uri GetUrl(PostQueryFilters filter, string actionUrl);
    }
}