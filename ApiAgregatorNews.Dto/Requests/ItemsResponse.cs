using ApiAgregatorNews.Dto.Status;

namespace ApiAgregatorNews.Dto.Requests
{
    public class ItemsResponse
    {
        public ItemsLidStatus Status { get; set; }
        public List<ItemDto> Items { get; set;}
    }
}
