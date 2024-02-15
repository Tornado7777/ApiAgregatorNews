using ApiAgregatorNews.Data;
using ApiAgregatorNews.Data.Entity;
using ApiAgregatorNews.Dto;
using ApiAgregatorNews.Dto.Requests;
using ApiAgregatorNews.Dto.Status;
using Azure;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Xml;



namespace ApiAgregatorNews.Services.Impl
{
    public class SourceService : ISourceService
    {
        #region Services

        private readonly IServiceScopeFactory _serviceScopeFactory;

        #endregion


        public SourceService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public SourceResponse AddSource(string url)
        {
            
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            ApiAgregatorNewsDbContext context = scope.ServiceProvider.GetRequiredService<ApiAgregatorNewsDbContext>();

            var sourceResponse = new SourceResponse();
            var sourceRSS = GetSourceRSSFromNet(url);

            if(sourceRSS == null)
            {
                sourceResponse.Status = SourceResponceStatus.ErrorRSSFromNet;
                return sourceResponse;
            }

            sourceResponse.SourceDto = new SourceDto
            {
                Title = sourceRSS.Title,
                Link = sourceRSS.Link,
                Description = sourceRSS.Description
            };

            context.SourcesRSS.Add(sourceRSS);
            try
            {
                context.SaveChanges();
            }
            catch
            {
                sourceResponse.Status = SourceResponceStatus.ErrorDB;
            }
            sourceResponse.Status = SourceResponceStatus.Success;
            return sourceResponse;
        }


        public SourcesResponse GetSources()
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            ApiAgregatorNewsDbContext context = scope.ServiceProvider.GetRequiredService<ApiAgregatorNewsDbContext>();

            var sources = context.SourcesRSS.ToList();
            var sorcesResponce = new SourcesResponse();

            if (!sources.Any())
            {
                sorcesResponce.Status = SourcesResponceStatus.NotFoundSorceRSS;
                return sorcesResponce;
            }
            var sourcesDto = new List<SourceDto>();
            foreach (var source in sources)
            {

                //здесь также можно сделать проверку статуса статьи
                if (source == null)
                {
                    return null;
                }
                sourcesDto.Add(new SourceDto
                {
                    Id = source.Id,
                    Title = source.Title,
                    Link = source.Link,
                    Description = source.Description,
                    
                });
            }
            sorcesResponce.Status = SourcesResponceStatus.Success;
            sorcesResponce.SourcesDto = sourcesDto;
            return sorcesResponce; 
        }

        public RefreshSourcesStatus RefreshSources()
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            ApiAgregatorNewsDbContext context = scope.ServiceProvider.GetRequiredService<ApiAgregatorNewsDbContext>();

            var sources = context.SourcesRSS.ToList();
            if(sources != null && sources.Count > 0)
            {
                foreach (var source in sources)
                {

                    var sourceNet = GetSourceRSSFromNet(source.Link);
                    var sourceItems = context.Items.Where(x => x.SourceRSSId == source.Id).ToList();
                    if(sourceNet != null && sourceNet.Items != null && sourceNet.Items.Count() > 0)
                    {
                         var diff = sourceNet.Items.Except(sourceItems, new ItemsComparer());
                        if(diff != null && diff.Count() > 0)
                        {
                            foreach(var item in diff)
                            {
                                source.Items.Add(item);
                            }
                            context.SourcesRSS.Update(source);
                            try
                            {
                                context.SaveChanges();
                            }
                            catch
                            {
                               return  RefreshSourcesStatus.ErrorSaveDB;
                            }
                        }

                    }
                    else
                    {
                        return RefreshSourcesStatus.ErrorGetRSSFromNet;
                    }
                }
            }

            return RefreshSourcesStatus.Success;
            
        }

        private SourceRSS? GetSourceRSSFromNet(string url)
        {
            string GetElem(XmlNode chanElem, string text)
            {
                if (chanElem[text] != null)
                {
                    return chanElem[text].InnerText;
                }
                return string.Empty;
            }

            WebRequest request = WebRequest.Create(url);

            WebResponse response = request.GetResponse();
            XmlDocument doc = new XmlDocument();

            var channel = new SourceRSS();
            try
            {
                doc.Load(response.GetResponseStream());
                XmlElement rssElem = doc["rss"];
                if (rssElem == null) return null;
                XmlElement chanElem = rssElem["channel"];

                if (chanElem != null)
                {
                    channel.Title = GetElem(chanElem, "title") ?? "";
                    channel.Link = url;
                    channel.Description = GetElem(chanElem, "description") ?? "";

                    if (chanElem["language"] != null)
                    {
                        channel.Culture = CultureInfo.CreateSpecificCulture(chanElem["language"].InnerText).Name;
                    }
                    else
                    {
                        channel.Culture = CultureInfo.CurrentCulture.Name;
                    }
                    channel.Items = new List<Item>();
                    XmlNodeList itemElems = chanElem.GetElementsByTagName("item");
                    foreach (XmlElement itemElem in itemElems)
                    {
                        Item item = new Item();

                        item.Title = GetElem(itemElem, "title") ?? "";
                        item.Link = GetElem(itemElem, "link") ?? "";
                        item.Description = GetElem(itemElem, "description") ?? "";
                        item.PubDate = GetElem(itemElem, "pubDate") ?? "";
                        channel.Items.Add(item);
                    }

                }

            }
            catch (XmlException)
            {
                return null;
            }
            return channel;
        }

        private class ItemsComparer : IEqualityComparer<Item>
        {
            public bool Equals(Item x, Item y)
            {
                if(x != null && y != null 
                    && x.Title == y.Title
                    && x.Link == y.Link
                    && x.PubDate == y.PubDate 
                    && x.Description == y.Description)
                {
                    return true;
                }
                return false;
            }
            public int GetHashCode([DisallowNull] Item obj)
            {
                return (obj.Title ?? "" + obj.Link ?? "" + obj.PubDate ?? "" + obj.Description ?? "").GetHashCode();
            }
        }
    }
}
