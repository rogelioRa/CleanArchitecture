using System.Diagnostics.Tracing;
using System;
using AutoMapper;
using SocialMedia.Domain.CustomEntities;
using SocialMedia.Domain.Interfaces;
using SocialMedia.Domain.Responses;
using SocialMedia.Domain.Entities;

namespace SocialMedia.Infrastructure.Factory
{
    public class ResponseFactory<Destination, Entity>
    {
        private readonly IMapper mapper;

        public ResponseFactory(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public ApiResponse<Destination> GetResponse(object source)
        {
            var responseDestination = mapper.Map<Destination>(source);
            var apiResponse = new ApiResponse<Destination>(responseDestination);
            if(source?.GetType() == typeof(PageList<Entity>))
            {
                var page = (PageList<Entity>)source;
                apiResponse.metadata = new Metadata()
                {
                    CurrentPage =  page.CurrentPage,
                    TotalPage =  page.TotalPage,
                    PageSize =  page.PageSize,
                    TotalCount =  page.TotalCount,
                    HasPrevPage =  page.HasPrevPage,
                    HasNextPage =  page.HasNextPage
                };
            }
            return apiResponse;
        }

    }
}