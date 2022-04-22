using System;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Domain.DTOs
{
    public class PostDTO
    {
        /// <summary>
        /// Identity property 
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}