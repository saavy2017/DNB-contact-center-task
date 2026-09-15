using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CaseManagement.DTO
{
    public class SupportCaseFilterRequest
    {
        public int? ReferenceNumber { get; set; }
        public string? CustomerEmail { get; set; }
        public string? Priority { get; set; }
        public string? Status { get; set; }
    }
}
