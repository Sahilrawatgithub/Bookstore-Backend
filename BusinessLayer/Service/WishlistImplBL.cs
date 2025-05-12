using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using ModelLayer.Entity;
using RepositoryLayer.DTO;
using RepositoryLayer.Interface;

namespace BusinessLayer.Service
{
    public class WishlistImplBL: IWishlistBL
    {
        private readonly IWishlistRL _wishlistRL;
        public WishlistImplBL(IWishlistRL wishlistRL)
        {
            _wishlistRL = wishlistRL;
        }
        public async Task<ResponseDTO<string>> WishlistBookAsync(int bookId, int userId)
        {
            return await _wishlistRL.WishlistBookAsync(bookId, userId);
        }
        public async Task<ResponseDTO<List<WishListEntity>>> GetAllWishlistedBooksAsync(int userId)
        {
            return await _wishlistRL.GetAllWishlistedBooksAsync(userId);
        }
        public async Task<ResponseDTO<string>> RemoveBookFromWishlistAsync(int bookId, int userId)
        {
            return await _wishlistRL.RemoveBookFromWishlistAsync(bookId, userId);
        }
        public async Task<ResponseDTO<string>> ClearWishlistAsync(int userId)
        {
            return await _wishlistRL.ClearWishlistAsync(userId);
        }

    }
}
