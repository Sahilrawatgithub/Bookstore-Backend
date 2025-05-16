using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BookStore.Controllers;
using BusinessLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Entity;
using Moq;
using NUnit.Framework;
using RepositoryLayer.DTO;

namespace BookStoreTesting.Tests
{
    public class BookControllerTest
    {
        private Mock<IBookBL> _mockBookBL;
        private BookController _controller;

        [SetUp]
        public void Setup()
        {
            _mockBookBL = new Mock<IBookBL>();
            _controller = new BookController(_mockBookBL.Object);

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("Id", "1")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userClaims }
            };
        }

        [Test]
        public async Task UploadBook_ReturnsOk_WhenSuccessful()
        {
            
            var request = new AddBookReqDTO
            {
                BookName = "book",
                AuthorName = "sahil",
                BookImage = "sample",
                Description = "sample",
                Quantity = 10,
                Price = 180
            };

            var response = new ResponseDTO<string>
            {
                Success = true,
                Message = "Book uploaded successfully"
            };

            _mockBookBL.Setup(x => x.UploadBookAsync(request, 1)).ReturnsAsync(response);

            var result = await _controller.UploadBook(request);
            
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            var resultValue = okResult?.Value as ResponseDTO<string>;
            Assert.That(resultValue?.Success, Is.True);
            Assert.That(resultValue?.Message, Is.EqualTo("Book uploaded successfully"));
        }

        [Test]
        public async Task ViewBookById_ReturnsOk_WhenBookExists()
        {
            
            int bookId = 1;
            var bookEntity = new BookEntity { BookId = bookId, BookName = "Test Book" };
            var response = new ResponseDTO<BookEntity> 
            {
                Success = true,
                Message = "Book retrieved successfully",
                Data = bookEntity
            };

            _mockBookBL.Setup(x => x.ViewBookByIdAsync(bookId)).ReturnsAsync(response);

            
            var result = await _controller.ViewBookById(bookId);

            
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            var resultValue = okResult?.Value as ResponseDTO<BookEntity>;
            Assert.That(resultValue?.Success, Is.True);
            Assert.That(resultValue?.Data?.BookId, Is.EqualTo(bookId));
        }

        [Test]
        public async Task GetAllBooks_ReturnsOk_WithBookList()
        {
            
            var bookList = new List<BookEntity>
    {
        new BookEntity { BookId = 1, BookName = "Book 1", AuthorName = "Author 1" },
        new BookEntity { BookId = 2, BookName = "Book 2", AuthorName = "Author 2" }
    };
            var response = new ResponseDTO<List<BookEntity>>
            {
                Success = true,
                Message = "Books retrieved successfully",
                Data = bookList
            };

            _mockBookBL.Setup(x => x.GetAllBooksAsync()).ReturnsAsync(response);

            
            var result = await _controller.GetAllBooks();

            
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            var resultValue = okResult?.Value as ResponseDTO<List<BookEntity>>;  

            Assert.That(resultValue?.Success, Is.True);
            Assert.That(resultValue?.Data?.Count, Is.EqualTo(2));
            Assert.That(resultValue?.Data?[0].BookName, Is.EqualTo("Book 1"));
            Assert.That(resultValue?.Data?[1].AuthorName, Is.EqualTo("Author 2"));
        }
    }
}
