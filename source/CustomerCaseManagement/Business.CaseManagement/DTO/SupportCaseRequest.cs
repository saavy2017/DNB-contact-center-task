namespace Application.CaseManagement.DTO
{
    public class SupportCaseRequest
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
    }
}
