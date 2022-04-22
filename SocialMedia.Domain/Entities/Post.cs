using System;
using System.Collections.Generic;
using SocialMedia.Domain.Interfaces;

#nullable disable

namespace SocialMedia.Domain.Entities
{
    public partial class Post:  Entity, IEntity
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
        }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
