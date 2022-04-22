using System;
using System.Collections.Generic;
using System.Linq;

namespace SocialMedia.Domain.CustomEntities
{
    public class PageList<T>: List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPage { get; private set; }
        public int PageSize { get; set; }
        public int TotalCount { get; private set; }

        public bool HasPrevPage => this.CurrentPage > 1;
        public bool HasNextPage => this.CurrentPage < this.TotalPage;

        public int? PrevPage => this.HasPrevPage ? this.CurrentPage - 1 : (int?)null;
        public int? NextPage => this.HasNextPage ? this.CurrentPage + 1 : (int?)null; 

        public PageList(List<T> items, int count, int pageNumber, int pageSize)        
        {
            this.TotalCount = count;
            this.PageSize = pageSize;
            this.CurrentPage = pageNumber;   
            this.TotalPage = (int)Math.Ceiling(count/ (double)pageSize);
            AddRange(items);
        }

        public static PageList<T> Create(IEnumerable<T> items, int pageNumber, int pageSize)
        {
            int count = items.Count();
            var _items = items.Skip((int)pageSize * (pageNumber -1 )).Take(pageSize).ToList();
            return new PageList<T>(_items, count, pageNumber, pageSize);

        }
    }
}