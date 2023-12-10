using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testvezeeta.Presentation_Layer.DTO.AdminDTO
{
    public class NumRequestsDTO
    {
        //{#requests,#PendingRequests,#completedRequests,#CancelledRequests}

        public int Requests { get; set; }
        public int PendingRequests { get; set; }
        public int completedRequests { get; set; }
        public int CancelledRequests { get; set; }

    }
}
