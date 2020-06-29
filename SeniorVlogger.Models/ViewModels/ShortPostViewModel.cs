namespace SeniorVlogger.Models.ViewModels
{
    public class ShortPostViewModel
    {
        public ShortPostViewModel(int id, string slug, string title)
        {
            Id = id;
            Slug = slug;
            Title = title;
        }

        public int Id { get; set; }

        public string Slug { get; set; }

        public string Title { get; set; }
    }
}
