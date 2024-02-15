using ApiAgregatorNews.Dto.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAgregatorNews.Dto.Requests
{
    public class SourcesResponse
    {
        public SourcesResponceStatus Status { get; set; }
        public List<SourceDto> SourcesDto { get; set; }
    }
}
