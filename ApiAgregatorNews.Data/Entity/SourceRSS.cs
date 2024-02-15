using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace ApiAgregatorNews.Data.Entity
{
    [Table("SourcesRSS")]
    public class SourceRSS
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(255)]
        public string Title { get; set; }
        [StringLength(255)]
        public string Link { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        public string Culture { get; set; }

        public ICollection<Item> Items { get; set; }
    }
}
