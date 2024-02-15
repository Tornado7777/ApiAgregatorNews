using ApiAgregatorNews.Dto;
using ApiAgregatorNews.Dto.Requests;
using ApiAgregatorNews.Dto.Status;

namespace ApiAgregatorNews.Services
{
    public interface ISourceService
    {
        /// <summary>
        /// Добавляет источник RSS
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        SourceResponse AddSource(string url);
        /// <summary>
        /// Возвращает список текущих истоников
        /// </summary>
        /// <returns></returns>
        SourcesResponse GetSources();

        /// <summary>
        /// Загрузить новости из источников в БД
        /// </summary>
        public RefreshSourcesStatus RefreshSources();
    }
}
