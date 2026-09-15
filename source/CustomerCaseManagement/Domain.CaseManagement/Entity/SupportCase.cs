using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.CaseManagement.Entity
{
    public class SupportCase
    {
        public int Id { get; set; }
        public int ReferenceNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
