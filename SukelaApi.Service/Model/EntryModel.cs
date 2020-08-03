
namespace SukelaApi.Service.Model
{
    public class Entry
    {
        public long Id { get; set; }
        public string Content { get; set; }

        public string EntryUrl { get; set; }
        public string EntryDate { get; set; }

        public string Author { get; set; }
        public string AuthorUrl { get; set; }
    }
}
