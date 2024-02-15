using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAgregatorNews.Dto.Status
{
    public enum RefreshSourcesStatus
    {
        Success = 0,
        ErrorGetRSSFromNet = 1,
        ErrorSaveDB = 2,
        ErrorService =3,
    }
}
