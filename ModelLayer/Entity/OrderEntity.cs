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
    public class OrderEntity
    {
        [Key]
        public int OrderId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Book")]
        public int BookId { get; set; }
        [ForeignKey("Address")]
        public int AddressId { get; set; }
        public DateTime OrderDate { get; set; }
        [JsonIgnore]
        public UserEntity User { get; set; }
        [JsonIgnore]
        public BookEntity Book { get; set; }
        [JsonIgnore]
        public AddressEntity Address { get; set; }

    }
}
