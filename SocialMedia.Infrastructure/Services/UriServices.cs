using System;
using SocialMedia.Domain.QueryFilters;
using SocialMedia.Infrastructure.Interfaces;

namespace SocialMedia.Infrastructure.Services
{
    public class UriServices: IUriService
    {
        private readonly string baseUrl;

        public UriServices(string UrlService)
        {
            this.baseUrl = UrlService;
        }

        public Uri GetUrl(PostQueryFilters filter, string actionUrl)
        {
            string url = $"{this.baseUrl}{actionUrl}";
            return new Uri(url);
        }
    }
}