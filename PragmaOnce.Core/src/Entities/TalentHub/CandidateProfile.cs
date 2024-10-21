namespace PragmaOnce.Core.src.Entities.TalentHub
{
    public class CandidateProfile : BaseEntity
    {
        public Guid UserId { get; set; }
        public string? Title { get; set; }

        public int? MinYearsExperience { get; set; }

        public int? MaxYearsExperience { get; set; }

        public Guid? Citizenship { get; set; }
        public Guid? PermanentResidency { get; set; }
        public CandidateProfile() { }
        public CandidateProfile(Guid userId, string title)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Title = title;
        }


    }
}
