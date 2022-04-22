namespace SocialMedia.Domain.CustomEntities
{
    public class Metadata
    {
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public bool HasPrevPage { get; set; }
        public bool HasNextPage { get; set; }

        public string NextPageUrl { get; set; }
        public string PrevPageUrl { get; set; }
    }
}