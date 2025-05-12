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
    public class WishListEntity
    {
        [Key]
        public int WishListId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Book")]
        public int BookId { get; set; }
        public bool IsCarted { get; set; } = false;
        [JsonIgnore]
        public UserEntity User { get; set; }
        [JsonIgnore]
        public BookEntity Book { get; set; }

    }
}
