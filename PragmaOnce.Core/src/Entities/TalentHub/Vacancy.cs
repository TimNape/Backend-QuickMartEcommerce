namespace PragmaOnce.Core.src.Entities.TalentHub
{
    public class Vacancy : BaseEntity
    {
        public string? Title { get; set; }

        public Guid? CompanyId { get; set; }

        public Guid? RecruiterProfileId { get; set; }
        public Vacancy() { }

        public Vacancy(string title, Guid? companyId, Guid? recruiterProfileId)
        {
            Id = Guid.NewGuid();
            Title = title;
            CompanyId = companyId;
            RecruiterProfileId = recruiterProfileId;
        }
    }
}
