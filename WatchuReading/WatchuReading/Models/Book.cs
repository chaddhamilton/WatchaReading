using System;
using System.Collections.Generic;

namespace WatchuReading.Models
{
    public class Book : IBusinessEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Rating { get; set; }
        public List<Comment> Comments { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool ShowMenu { get; set; } = false;
        public string LineItem { get; set; }
        public int AddedBy { get; set; } = 0;
        public string PublishedDate { get; set; }
    }

    public class Comment : IBusinessEntity
    {
        public int Id { get; set; }
        public string Opinion { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool ShowMenu { get; set; }

    }
}
