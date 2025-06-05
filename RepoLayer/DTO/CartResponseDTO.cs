using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.DTO
{
    public class CartResponseDTO
    {
        public int CartId { get; set; }
        public int BookId { get; set; }
        public string? BookName { get; set; }
        public string? AuthorName { get; set; }
        public float PricePerUnit { get; set; }
        public int Quantity { get; set; }
        public float TotalPrice { get; set; }
        public string? Image { get; set; }
        public bool isUncarted { get; set; }
    }
}
