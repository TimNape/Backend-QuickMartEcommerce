namespace PragmaOnce.Service.src.DTOs.TalentHub
{
    public class CandidateReadDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? Title { get; set; }
        public int? MinYearsExperience { get; set; }
        public int? MaxYearsExperience { get; set; }
        public Guid? Citizenship { get; set; }
        public Guid? PermanentResidency { get; set; }

    }

    public class CandidateUpdateDto 
    {
        public string? Title { get; set; }
        public int? MinYearsExperience { get; set; }
        public int? MaxYearsExperience { get; set; }
        public Guid? Citizenship { get; set; }
        public Guid? PermanentResidency { get; set; }
    }

    public class CandidateCreateDto
    {
        public Guid UserId { get; set; }
        public string? Title { get; set; }
        public int? MinYearsExperience { get; set; }
        public int? MaxYearsExperience { get; set; }
        public Guid? Citizenship { get; set; }
        public Guid? PermanentResidency { get; set; }

    }
}
