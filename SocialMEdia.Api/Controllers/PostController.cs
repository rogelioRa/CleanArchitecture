using System.Net;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Domain.DTOs;
using SocialMedia.Domain.Entities;
using SocialMedia.Domain.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using SocialMedia.Domain.QueryFilters;
using Newtonsoft.Json;
using SocialMedia.Domain.CustomEntities;
using SocialMedia.Infrastructure.Interfaces;
using SocialMedia.Domain.Responses;
using SocialMedia.Infrastructure.Factory;
using SocialMedia.Domain.Services;

namespace SocialMEdia.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService postService;
        private readonly IMapper mapper;
        private readonly IUriService urlService;
        private readonly IUserService userService;
        
        public PostController(IPostService postService, IMapper mapper, IUriService urlService, IUserService userService)
        {
            this.postService = postService;
            this.mapper = mapper;
            this.urlService = urlService;
            this.userService = userService;
        }
        
        /// <summary>
        /// Retrive all post
        /// </summary>
        /// <param name="filter">filter prop to paginate post</param>
        /// <returns></returns>
        [HttpGet(Name = nameof(Get))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<PostDTO>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult Get([FromQuery]PostQueryFilters filter)
        {
            var posts = this.postService.Get(filter);
            var response = new ResponseFactory<IEnumerable<PostDTO>, Post>(mapper).GetResponse(posts);
            // var postDto = mapper.Map<PostDTO>(post);
            // var response = new ApiResponse<PostDTO>(postDto);
            // response.metadata = new Metadata()
            // {
            //     CurrentPage =  posts.CurrentPage,
            //     TotalPage =  posts.TotalPage,
            //     PageSize =  posts.PageSize,
            //     TotalCount =  posts.TotalCount,
            //     HasPrevPage =  posts.HasPrevPage,
            //     HasNextPage =  posts.HasNextPage,
            //     NextPageUrl = this.urlService.GetUrl(filter, Url.RouteUrl(nameof(Get))).ToString(),
            //     PrevPageUrl = this.urlService.GetUrl(filter, Url.RouteUrl(nameof(Get))).ToString()
            // };
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var post = await this.postService.Show(id);
            var response = new ResponseFactory<PostDTO, Post>(mapper).GetResponse(post);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Store(PostDTO postDto)
        {
            var post = mapper.Map<Post>(postDto);
            await this.postService.Store(post);
            var response = new ResponseFactory<PostDTO, Post>(mapper).GetResponse(post);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PostDTO postDto)
        {
            var post = mapper.Map<Post>(postDto);
            post.Id = id;
            var result = await this.postService.Update(post);
            var response = new ResponseFactory<bool, Post>(mapper).GetResponse(result);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await this.postService.Delete(id);
            var response = new ResponseFactory<bool, Post>(mapper).GetResponse(result);
            return Ok(response);
        }
    }
}
