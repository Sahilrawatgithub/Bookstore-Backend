using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ModelLayer.Enums;


namespace ModelLayer.Entity
{
    public class AddressEntity
    {
        [Key]
        public int AddressId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public AddressType Type { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        
        [JsonIgnore]
        public UserEntity User { get; set; }

    }
}
