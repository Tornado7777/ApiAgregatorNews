using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAgregatorNews.Dto.Status
{
    public enum ItemsLidStatus
    {
        Success = 0,
        NotFoundItems = 1,
        ErrorRead = 2,
        EmptyText = 3,
        ServiceError = 4
    }
}
