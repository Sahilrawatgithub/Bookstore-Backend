using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Entity;
using RepositoryLayer.DTO;

namespace BusinessLayer.Interface
{
    public interface IWishlistBL
    {
        public Task<ResponseDTO<string>> WishlistBookAsync(int bookId, int userId);
        public Task<ResponseDTO<List<WishListEntity>>> GetAllWishlistedBooksAsync(int userId);
        public Task<ResponseDTO<string>> RemoveBookFromWishlistAsync(int bookId, int userId);
        public Task<ResponseDTO<string>> ClearWishlistAsync(int userId);

    }
}
