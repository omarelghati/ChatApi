namespace ChatApi.Models
{
    public class AddMessageDTO
    {
        public string TargetId { get; set; }

        public string SenderId { get; set; }

        public string Message { get; set; }
    }
}
