namespace PragmaOnce.Service.src.DTOs.TalentHub
{
    public class RecruiterReadDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? CompanyId { get; set; }
        public string? Title { get; set; }
    }

    public class RecruiterUpdateDto
    {
        public Guid? CompanyId { get; set; }
    }

    public class RecruiterCreateDto
    {
        public Guid UserId { get; set; }
        public Guid? CompanyId { get; set; }
    }
}
