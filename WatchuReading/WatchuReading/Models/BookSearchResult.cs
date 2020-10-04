﻿using System;
using System.Collections.Generic;

namespace WatchuReading.Models
{
    public class Doc
    {
        public string title_suggest { get; set; }
        public List<string> edition_key { get; set; }
        public List<string> isbn { get; set; }
        public bool has_fulltext { get; set; }
        public List<string> text { get; set; }
        public List<string> author_name { get; set; }
        public List<string> seed { get; set; }
        public List<string> oclc { get; set; }
        public List<string> author_key { get; set; }
        public List<string> subject { get; set; }
        public string title { get; set; }
        public List<string> publish_date { get; set; }
        public string type { get; set; }
        public int ebook_count_i { get; set; }
        public List<string> publish_place { get; set; }
        public int edition_count { get; set; }
        public string overdrive_s { get; set; }
        public string key { get; set; }
        public List<string> id_goodreads { get; set; }
        public List<string> id_overdrive { get; set; }
        public List<string> publisher { get; set; }
        public List<string> language { get; set; }
        public List<string> lccn { get; set; }
        public int last_modified_i { get; set; }
        public List<string> id_librarything { get; set; }
        public string cover_edition_key { get; set; }
        public List<int> publish_year { get; set; }
        public int first_publish_year { get; set; }
    }

    //OPENLIBRARY.org
    public class BookSearchResult
    {
        public int start { get; set; }
        public int num_found { get; set; }
        public int numFound { get; set; }
        public List<Doc> docs { get; set; }
    }
}
