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
    public class CartEntity
    {
        [Key]
        public int CartId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Book")]
        public int BookId { get; set; }
        public int Quantity { get; set; }

        public bool IsUncarted { get; set; } = false;
        public bool IsOrdered { get; set; } = false;

        [JsonIgnore]
        public UserEntity User { get; set; }
        [JsonIgnore]
        public BookEntity Book { get; set; }
    }
}
