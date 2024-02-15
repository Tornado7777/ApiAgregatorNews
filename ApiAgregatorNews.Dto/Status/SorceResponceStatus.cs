
namespace ApiAgregatorNews.Dto.Status
{
    public enum SourceResponceStatus
    {
        Success = 0,
        NotAddSorceRSS = 1,
        ErrorRSSFromNet = 2,
        ErrorDB = 3,
        ErrorXML = 4,
        EmptyURL = 5,
        ErrorService = 6
    }
}
