namespace Oxygen.Models
{
    public class CandidateMatch
    {
        public Guid Id { get; set; }
        public Guid CandidateId { get; set; }
        public Guid JobRoleId { get; set; }
        public decimal Score { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
