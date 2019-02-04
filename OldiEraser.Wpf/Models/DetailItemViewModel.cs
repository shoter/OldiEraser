namespace OldiEraser.Wpf.Models
{
    public class DetailItemViewModel
    {
        public string Path { get; set; }
        public long DaysToLive{ get; set; }

        public DetailItemViewModel(string path, long daysToLive)
        {
            Path = path;
            DaysToLive = daysToLive;
        }
    }
}
