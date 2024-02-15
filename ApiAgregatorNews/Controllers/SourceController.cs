using ApiAgregatorNews.Dto;
using ApiAgregatorNews.Dto.Requests;
using ApiAgregatorNews.Dto.Status;
using ApiAgregatorNews.Services;
using Microsoft.AspNetCore.Mvc;


namespace ApiAgregatorNews.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SourceController : ControllerBase
    {

        #region Services

        private readonly ISourceService _sourceService;

        #endregion


        public SourceController(
            ISourceService sourceService)
        {
            _sourceService = sourceService;
        }

        /// <summary>
        /// Метод добовляет два источника в БД
        /// </summary>
        /// <param name="text"></param>
        /// <param name="raitingSort"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Start")]
        [ProducesResponseType(typeof(SourcesResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SourceResponse), StatusCodes.Status200OK)]
        public IActionResult Start()
        {
            var sourceResponce1 = AddSourceFunc("http://www.ria.ru/export/rss2/index.xml");
            var sourcesResponse = new SourcesResponse 
            { 
                Status = SourcesResponceStatus.Success,
                SourcesDto = new List<SourceDto>()
            };
            if(sourceResponce1.Status != SourceResponceStatus.Success)
            {
                return BadRequest(sourceResponce1);
            }
            sourcesResponse.SourcesDto.Add(sourceResponce1.SourceDto);
            var sourceResponce2 = AddSourceFunc("http://www.dp.ru/exportnews.xml");
            if (sourceResponce2.Status != SourceResponceStatus.Success)
            {
                return BadRequest(sourceResponce2);
            }
            sourcesResponse.SourcesDto.Add(sourceResponce2.SourceDto);
            return Ok(sourcesResponse);
        }

        [HttpGet]
        [Route("GetSources")]
        [ProducesResponseType(typeof(SourcesResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SourcesResponse), StatusCodes.Status200OK)]
        public IActionResult GetAllItems()
        {
            var sourcesResponse = new SourcesResponse();
                           
            try
            {
                sourcesResponse = _sourceService.GetSources();
                if (sourcesResponse.SourcesDto != null && sourcesResponse.SourcesDto.Count > 0)
                {
                    sourcesResponse.Status = SourcesResponceStatus.Success;
                }
                else
                {
                    sourcesResponse.Status = SourcesResponceStatus.NotFoundSorceRSS;
                }
            }
            catch
            {
                return BadRequest(new SourcesResponse
                {

                    Status = SourcesResponceStatus.ErrorService
                });
            }
            
            return Ok(sourcesResponse);
        }

        /// <summary>
        /// Метод обновляет данные из интерена для сохраненных в БД источников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("RefereshSources")]
        [ProducesResponseType(typeof(RefreshSourcesResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RefreshSourcesResponse), StatusCodes.Status200OK)]
        public IActionResult RefreshSorces()
        {
            var refreshSourcesResponse = new RefreshSourcesResponse();

            try
            {
                refreshSourcesResponse.Status = _sourceService.RefreshSources();                
            }
            catch
            {
                return BadRequest(new RefreshSourcesResponse
                {

                    Status = RefreshSourcesStatus.ErrorService
                });
            }

            return Ok(refreshSourcesResponse);
        }



        /// <summary>
        /// Метод добовляет источник в БД
        /// </summary>
        /// <param name="text"></param>
        /// <param name="raitingSort"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("AddSource")]
        [ProducesResponseType(typeof(SourceResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SourceResponse), StatusCodes.Status200OK)]
        public IActionResult AddSource([FromQuery] string url)
        {
            var sourceResponce = AddSourceFunc(url);
            if (sourceResponce.Status != SourceResponceStatus.Success)
            {
                return BadRequest(sourceResponce);
            }
            return Ok(sourceResponce);
        }

        private SourceResponse AddSourceFunc(string url)
        {
            var sourceResponse = new SourceResponse();
            //проверка на наличие текста в запросе
            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    sourceResponse = _sourceService.AddSource(url);
                    if (sourceResponse.SourceDto != null)
                    {
                        sourceResponse.Status = SourceResponceStatus.Success;
                    }
                    else
                    {
                        return sourceResponse;
                    }
                }
                catch
                {
                    return new SourceResponse
                    {

                        Status = SourceResponceStatus.ErrorService
                    };
                }
            }
            else
            {
                sourceResponse.Status = SourceResponceStatus.EmptyURL;
                return sourceResponse;
            }

            return sourceResponse;
        }

       
    }
}
