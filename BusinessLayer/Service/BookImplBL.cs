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
        public async Task<ResponseDTO<string>> UploadBook(AddBookReqDTO request,int userId)
        {
            return await _bookRL.UploadBook(request,userId);
        }
        public async Task<ResponseDTO<BookEntity>> ViewBookById(int bookId)
        {
            return await _bookRL.ViewBookById(bookId);
        }
        public async Task<ResponseDTO<List<BookEntity>>> GetAllBooks()
        {
            return await _bookRL.GetAllBooks();
        }
    }
}
