using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAgregatorNews.Data.Entity
{
    [Table("Items")]
    public class Item
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(255)]
        public string Title { get; set; }
        [StringLength(255)]
        public string Link { get; set; }
        [StringLength(255)]
        public string PubDate { get; set; }
        [StringLength(1023)]
        public string Description { get; set; }

        public int? SourceRSSId {get; set; }
        public SourceRSS SourceRSS { get; set; }

    }
}
