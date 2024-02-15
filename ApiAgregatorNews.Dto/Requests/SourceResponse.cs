using ApiAgregatorNews.Dto.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAgregatorNews.Dto.Requests
{
    public class SourceResponse
    {
        public SourceResponceStatus Status { get; set; }
        public SourceDto SourceDto { get; set; }
    }
}
