

namespace PragmaOnce.Service.src.DTOs.TalentHub
{
    public class VacancyReadDto
    {
        public Guid Id { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? RecruiterProfileId { get; set; }
    }

    public class VacancyCreateDto
    {
        public Guid? CompanyId { get; set; }
        public Guid? RecruiterProfileId { get; set; }
    }

    public class VacancyUpdateDto
    {
        public Guid? RecruiterProfileId { get; set; }
        public string? Title { get; set; }
    }
}
