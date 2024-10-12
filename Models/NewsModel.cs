namespace DelusiveVortexAspApi.Models
{
    public class NewsModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string Url { get; set; }
        public DateTime PublishDate { get; set; }
        public TimeSpan PublishTime { get; set; }
    }
}
