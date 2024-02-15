using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAgregatorNews.Dto.Status
{
    public enum SourcesResponceStatus
    {
        Success = 0,
        NotFoundSorceRSS = 1,
        ErrorService = 2,        
    }
}
