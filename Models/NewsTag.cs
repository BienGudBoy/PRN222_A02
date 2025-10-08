using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NguyenVanBien_SE18C.NET_A02.Models
{
    [Table("NewsTag")]
    public class NewsTag
    {
        [Key, Column(Order = 0)]
        [StringLength(20)]
        public string NewsArticleID { get; set; } = string.Empty;

        [Key, Column(Order = 1)]
        public int TagID { get; set; }

        // Navigation properties
        [ForeignKey("NewsArticleID")]
        public virtual NewsArticle? NewsArticle { get; set; }

        [ForeignKey("TagID")]
        public virtual Tag? Tag { get; set; }
    }
}
