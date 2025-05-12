using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Entity;
using RepositoryLayer.DTO;

namespace RepositoryLayer.Interface
{
    public interface IBookRL
    {
        public Task<ResponseDTO<string>> UploadBook(AddBookReqDTO request,int userId);
        public Task<ResponseDTO<BookEntity>> ViewBookById(int bookId);
        public Task<ResponseDTO<List<BookEntity>>> GetAllBooks();

    }
}
