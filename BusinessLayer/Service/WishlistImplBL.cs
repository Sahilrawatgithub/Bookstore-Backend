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
        public async Task<ResponseDTO<string>> WishlistBook(int bookId, int userId)
        {
            return await _wishlistRL.WishlistBook(bookId, userId);
        }
        public async Task<ResponseDTO<List<WishListEntity>>> GetAllWishlistedBooks(int userId)
        {
            return await _wishlistRL.GetAllWishlistedBooks(userId);
        }
        public async Task<ResponseDTO<string>> RemoveBookFromWishlist(int bookId, int userId)
        {
            return await _wishlistRL.RemoveBookFromWishlist(bookId, userId);
        }
        public async Task<ResponseDTO<string>> ClearWishlist(int userId)
        {
            return await _wishlistRL.ClearWishlist(userId);
        }

    }
}
