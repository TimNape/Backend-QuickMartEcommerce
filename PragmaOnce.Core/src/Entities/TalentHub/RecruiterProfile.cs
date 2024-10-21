namespace PragmaOnce.Core.src.Entities.TalentHub
{
    public class RecruiterProfile : BaseEntity
    {
        public Guid UserId { get; set; }

        public Guid? CompanyId { get; set; }

        public RecruiterProfile() { }
        public RecruiterProfile(Guid userId)
        {
            UserId = userId;
        }

        public RecruiterProfile(Guid userId, Guid? companyId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            CompanyId = companyId;
        }
    }
}
