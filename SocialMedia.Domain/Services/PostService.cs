using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SocialMedia.Domain.CustomEntities;
using SocialMedia.Domain.Entities;
using SocialMedia.Domain.Exceptions;
using SocialMedia.Domain.Interfaces;
using SocialMedia.Domain.QueryFilters;

namespace SocialMedia.Domain.Services
{
    public class PostService: IPostService
    {
        private readonly IUnitOfWOrk unitOfWork;
        private readonly PaginationConfiguration _paginationConfiguration;

        public PostService(IUnitOfWOrk unitOfWork, IOptions<PaginationConfiguration> options)
        {
            this.unitOfWork = unitOfWork;
            this._paginationConfiguration = options.Value;
        }

        public async Task<bool> Delete(int id)
        {
            await this.unitOfWork.postRepository.Delete(id);
            await this.unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<Post> Show(int id)
        {
            return await this.unitOfWork.postRepository.Show(id);
        }

        public PageList<Post> Get(PostQueryFilters filter)
        {
            var posts = this.unitOfWork.postRepository.Get();
            filter.PageSize = filter.PageSize == 0 ? this._paginationConfiguration.DefaultPageSize : filter.PageSize;
            filter.PageNumber = filter.PageNumber == 0 ? this._paginationConfiguration.DefaultPageNumber : filter.PageNumber;
            if(filter.UserId != null)
            {
                posts = posts.Where(post => post.UserId == filter.UserId);
            }
            if(filter.Date != null)
            {
                posts = posts.Where(post => post.Date.Date == filter.Date?.Date);
            }
            if(filter.Description != null)
            {
                posts = posts.Where(post => post.Description.ToLower().Contains(filter.Description.ToLower()));
            }
            var paginatePost = PageList<Post>.Create(posts, filter.PageNumber, filter.PageSize);
            return paginatePost;
        }

        public async Task Store(Post post)
        {
            var user = await this.unitOfWork.userRepository.Show(post.UserId);
            if(user == null)
            {
                throw new BusinessException("User dose not not exist");
            }
            var userPost = await this.unitOfWork.postRepository.GetUserPost(post.UserId);
            if(userPost.Count() < 10)
            {
                var lastPost = userPost.OrderByDescending(post=> post.Date).FirstOrDefault();
                double totalDays = (DateTime.Now.Date - lastPost.Date).TotalDays;
                if(totalDays < 7)
                {
                    throw new BusinessException($"Not allowed post yet, you can post on {7 - totalDays} days");
                }
            }
            if(post.Description.Contains("sexo"))
            {
                throw new BusinessException("Contentn not allowed");
            }
            await this.unitOfWork.postRepository.Store(post);
            await this.unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> Update(Post post)
        {
            this.unitOfWork.postRepository.Update(post);
            await this.unitOfWork.SaveChangesAsync();
            return true;
        }
  }
}