namespace PBL_WEB.Models
{
    public class ListSchedule
    {
        public string IdDoctor { get; set; }
        public List<string> Monday { get; set; }
        public List<string> Tuesday { get; set; }
        public List<string> Wednesday { get; set; }
        public List<string> Thursday { get; set; }
        public List<string> Friday { get; set; }
        public List<string> Saturday { get; set; }

    }
}
