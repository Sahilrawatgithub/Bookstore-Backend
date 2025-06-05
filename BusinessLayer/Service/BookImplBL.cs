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
    public class BookImplBL:IBookBL
    {
        private readonly IBookRL _bookRL;
        public BookImplBL(IBookRL bookRl)
        {
            _bookRL = bookRl;
        }
        public async Task<ResponseDTO<string>> UploadBookAsync(AddBookReqDTO request,int userId, string imageFileName)
        {
            return await _bookRL.UploadBookAsync(request,userId,imageFileName);
        }
        public async Task<ResponseDTO<BookEntity>> ViewBookByIdAsync(int bookId)
        {
            return await _bookRL.ViewBookByIdAsync(bookId);
        }
        public async Task<ResponseDTO<List<BookEntity>>> GetAllBooksAsync()
        {
            return await _bookRL.GetAllBooksAsync();
        }
    }
}
