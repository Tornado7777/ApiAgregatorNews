using ApiAgregatorNews.Dto;
using ApiAgregatorNews.Dto.Requests;
using ApiAgregatorNews.Dto.Status;
using ApiAgregatorNews.Services;
using Microsoft.AspNetCore.Mvc;


namespace ApiAgregatorNews.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {

        #region Services

        private readonly IItemService _itemService;

        #endregion


        public ItemController(
            IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        [Route("GetAllItems")]
        [ProducesResponseType(typeof(ItemsResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ItemsResponse), StatusCodes.Status200OK)]
        public IActionResult GetAllItems()
        {
            var itemsResponse = new ItemsResponse();
                           
            try
            {
                itemsResponse.Items = _itemService.GetAllItems();
                if (itemsResponse.Items != null && itemsResponse.Items.Count > 0)
                {
                    itemsResponse.Status = ItemsLidStatus.Success;
                }
                else
                {
                    itemsResponse.Status = ItemsLidStatus.NotFoundItems;
                }
            }
            catch
            {
                return BadRequest(new ItemsResponse
                {

                    Status = ItemsLidStatus.ErrorRead
                });
            }
            
            return Ok(itemsResponse);
        }

        

        

        /// <summary>
        /// Метод возвращает лиды статьи начиная с текста который в поиске по заданное кол-во (сейчас 100).
        /// Статьи сортируются по дате в обратном порядке по умолчанию или по рейтингу(raitingSort = true).
        /// </summary>
        /// <param name="text"></param>
        /// <param name="raitingSort"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetItemsBySubstringTitle")]
        [ProducesResponseType(typeof(ItemsResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ItemsResponse), StatusCodes.Status200OK)]
        public IActionResult GetItemssBySubstringTitle([FromQuery] string text)
        {
            var itemsResponse = new ItemsResponse();
            //проверка на наличие текста в запросе
            if (!string.IsNullOrEmpty(text))
            {
                try
                {
                    itemsResponse.Items = _itemService.GetItemsBySubstringHead(text);
                    if (itemsResponse.Items != null && itemsResponse.Items.Count > 0)
                    {
                        itemsResponse.Status = ItemsLidStatus.Success;
                    }
                    else
                    {
                        itemsResponse.Status = ItemsLidStatus.NotFoundItems;
                    }
                }
                catch
                {
                    return BadRequest(new ItemsResponse
                    {

                        Status = ItemsLidStatus.ErrorRead
                    });
                }
            }
            else
            {
                itemsResponse.Status = ItemsLidStatus.EmptyText;
            }       

            return Ok(itemsResponse);
        }

       
    }
}
