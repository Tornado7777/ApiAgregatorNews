using ApiAgregatorNews.Dto;
using ApiAgregatorNews.Dto.Requests;
using ApiAgregatorNews.Dto.Status;

namespace ApiAgregatorNews.Services
{
    public interface IItemService
    {
        /// <summary>
        /// Получить новости по подстроке оглавления
        /// </summary>
        /// <param name="substring"></param>
        /// <returns></returns>
        public List<ItemDto> GetItemsBySubstringHead(string substring);
        /// <summary>
        /// Получить список всех новостей из БД
        /// </summary>
        /// <returns></returns>
        public List<ItemDto> GetAllItems();       

    }
}
