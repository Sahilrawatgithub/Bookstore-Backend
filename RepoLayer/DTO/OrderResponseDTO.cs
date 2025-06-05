using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.DTO
{
    public class OrderResponseDTO
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }

        public string BookName { get; set; }
        public string AuthorName { get; set; }
        public string BookImage { get; set; }
        public float Price { get; set; }
    }
}
