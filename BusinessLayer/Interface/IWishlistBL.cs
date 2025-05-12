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
        public Task<ResponseDTO<string>> WishlistBook(int bookId, int userId);
        public Task<ResponseDTO<List<WishListEntity>>> GetAllWishlistedBooks(int userId);
        public Task<ResponseDTO<string>> RemoveBookFromWishlist(int bookId, int userId);
        public Task<ResponseDTO<string>> ClearWishlist(int userId);

    }
}
