using ApiAgregatorNews.Data;
using ApiAgregatorNews.Dto;
using ApiAgregatorNews.Dto.Requests;
using System.Data;
using System.Linq;



namespace ApiAgregatorNews.Services.Impl
{
    public class ItemService : IItemService
    {
        #region Services

        private readonly IServiceScopeFactory _serviceScopeFactory;

        #endregion


        public ItemService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public List<ItemDto> GetAllItems()
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            ApiAgregatorNewsDbContext context = scope.ServiceProvider.GetRequiredService<ApiAgregatorNewsDbContext>();

            var items = context.Items.ToList();
            var sourceRSS = context.SourcesRSS.ToList();

            if (!items.Any())
            {
                return null;
            }
            var itemsDto = new List<ItemDto>();
            foreach (var item in items)
            {

                //здесь также можно сделать проверку статуса статьи
                if (item == null)
                {
                    return null;
                }
                itemsDto.Add(new ItemDto
                {
                    Id = item.Id,
                    Title = item.Title,
                    Link = item.Link,
                    Description = item.Description,
                    PubDate = item.PubDate,
                    SourceName = sourceRSS.FirstOrDefault(x => x.Id == item.SourceRSSId)?.Title ?? string.Empty
                });
            }
            return itemsDto;
        }

        public List<ItemDto> GetItemsBySubstringHead(string text)
        {
            
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            ApiAgregatorNewsDbContext context = scope.ServiceProvider.GetRequiredService<ApiAgregatorNewsDbContext>();
            var items = (from Items in context.Items
                            where Items.Title.Contains(text) select Items).ToList();
            var sourceRSS = context.SourcesRSS.ToList();

            if (!items.Any())
            {
                return null;
            }
            var itemsDto = new List<ItemDto>();
            foreach (var item in items)
            {

                //здесь также можно сделать проверку статуса статьи
                if (item == null)
                {
                    return null;
                }
                itemsDto.Add(new ItemDto
                {
                    Id = item.Id,
                    Title = item.Title,
                    Link = item.Link,
                    Description = item.Description,
                    PubDate = item.PubDate,
                    SourceName = sourceRSS.FirstOrDefault(x => x.Id == item.SourceRSSId)?.Title ?? string.Empty
                });
            }
            return itemsDto;
        }

       
    }
}
