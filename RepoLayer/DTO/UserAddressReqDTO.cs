using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ModelLayer.Entity;
using ModelLayer.Enums;

namespace RepositoryLayer.DTO
{
    public class UserAddressReqDTO
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public AddressType Type { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
    }
}
