using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ModelLayer.Entity
{
    public class UserEntity
    {
        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public List<BookEntity> Books { get; set; } // One user can have many books
        public List<AddressEntity> Addresses { get; set; } // One user can have many addresses
        public List<OrderEntity> Orders { get; set; } // One user can place many orders
        public List<WishListEntity> WishLists { get; set; } // One user can have many wishlist ite

    }
}
