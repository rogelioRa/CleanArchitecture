using AutoMapper;
using SocialMedia.Domain.CustomEntities;
using SocialMedia.Domain.DTOs;
using SocialMedia.Domain.Entities;
using SocialMedia.Domain.Interfaces;

namespace SocialMedia.Infrastructure.Mappings
{
    public class AutomapperProfile: Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Post, PostDTO>();
            CreateMap<PostDTO, Post>();
        }
    }
}