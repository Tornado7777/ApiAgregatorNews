using ApiAgregatorNews.Dto.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAgregatorNews.Dto.Requests
{
    public class RefreshSourcesResponse
    {
        public RefreshSourcesStatus Status { get; set; }
    }
}
