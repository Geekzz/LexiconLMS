namespace LMS.Shared.DTOs
{
    public class CourseDocumentDto
    {
        public int CourseDocumentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime UploadedAt { get; set; }

        //public Guid FileName { get; set; } // Reference to the actual file saved on the server
    }
}