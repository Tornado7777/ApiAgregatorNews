using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiAgregatorNews.Dto
{
    public class ItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string PubDate { get; set; }
        public string Description { get; set; }

        public string SourceName { get; set; }
    }
}
