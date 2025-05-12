using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ModelLayer.Entity
{
    public class BookEntity
    {
        [Key]
        public int BookId { get; set; }
        [ForeignKey("User")]
        public int AuthorID { get; set; } 
        public string BookName { get; set; }
        public string BookImage { get; set; }
        public string Description { get; set; }
        public string AuthorName { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        [JsonIgnore]
        public UserEntity? User { get; set; }

    }
}
