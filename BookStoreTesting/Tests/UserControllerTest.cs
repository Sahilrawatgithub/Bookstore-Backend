using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Controllers;
using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Entity;
using Moq;
using NUnit.Framework;
using RepositoryLayer.DTO;
using RepositoryLayer.Interface;

namespace BookStoreTesting.Tests
{
    public class UserControllerTest
    {
        private Mock<IUserBL> _mockUserBL;
        private UserController _controller;

        [SetUp]
        public void Setup()
        {
            _mockUserBL = new Mock<IUserBL>();
            _controller = new UserController(_mockUserBL.Object);
        }

        [Test]
        public async Task RegisterUserAsync_ReturnsOk_WhenSuccessful()
        {
            
            var userRequest = new RegUserDTO { Email = "test@test.com", Password = "Test123" };
            var response = new ResponseDTO<string> { Success = true, Message = "User Registered Successfully" };

            _mockUserBL.Setup(x => x.RegisterUserAsync(userRequest)).ReturnsAsync(response);

            var result = await _controller.RegisterUserAsync(userRequest);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            var resultValue = okResult?.Value as ResponseDTO<string>; 

            Assert.That(resultValue?.Success, Is.True);
        }

        [Test]
        public async Task LoginAsync_ReturnsOk_WhenCredentialsAreValid()
        {
            var email = "test@test.com";
            var password = "Test123";
            var response = new ResponseDTO<LoginResponseDTO>
            {
                Success = true,
                Message = "Login successful",
                Data = new LoginResponseDTO { Token = "dummy_token", Email = email }
            };

            _mockUserBL.Setup(x => x.LoginAsync(email, password)).ReturnsAsync(response);

            var result = await _controller.LoginAsync(email, password);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            var resultValue = okResult?.Value as ResponseDTO<LoginResponseDTO>;
            Assert.That(resultValue?.Success, Is.True);
        }

        [Test]
        public async Task GetAllUsersAsync_ReturnsOk_WhenUsersExist()
        {
            var users = new List<UserEntity> { new UserEntity { Email = "user1@test.com" } };
            var response = new ResponseDTO<List<UserEntity>>
            {
                Success = true,
                Message = "Users fetched",
                Data = users
            };

            _mockUserBL.Setup(x => x.GetAllUsersAsync()).ReturnsAsync(response);

            var result = await _controller.GetAllUsersAsync();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            var resultValue = okResult?.Value as ResponseDTO<List<UserEntity>>;

            Assert.That(resultValue?.Success, Is.True);
            Assert.That(resultValue?.Data.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task DeleteUserAsync_ReturnsOk_WhenDeletionSuccessful()
        {
            var email = "delete@test.com";
            var response = new ResponseDTO<string>
            {
                Success = true,
                Message = "User deleted successfully"
            };

            _mockUserBL.Setup(x => x.DeleteUserAsync(email)).ReturnsAsync(response);

            var result = await _controller.DeleteUserAsync(email);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            var resultValue = okResult?.Value as ResponseDTO<string>;

            Assert.That(resultValue?.Success, Is.True);
        }

        [Test]
        public async Task ForgotPasswordAsync_ReturnsOk_WhenEmailIsSent()
        {
            var email = "forgot@test.com";
            var response = new ResponseDTO<string>
            {
                Success = true,
                Message = "Reset link sent"
            };

            _mockUserBL.Setup(x => x.ForgotPasswordAsync(email)).ReturnsAsync(response);

            var result = await _controller.ForgotPasswordAsync(email);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            var resultValue = okResult?.Value as ResponseDTO<string>;

            Assert.That(resultValue?.Success, Is.True);
        }

        [Test]
        public async Task ResetPasswordAsync_ReturnsOk_WhenPasswordResetIsSuccessful()
        {
            var email = "reset@test.com";
            var newPassword = "NewPass@123";
            var response = new ResponseDTO<string>
            {
                Success = true,
                Message = "Password reset successful"
            };

            _mockUserBL.Setup(x => x.ResetPasswordAsync(email, newPassword)).ReturnsAsync(response);

            var result = await _controller.ResetPasswordAsync(email, newPassword);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            var resultValue = okResult?.Value as ResponseDTO<string>;

            Assert.That(resultValue?.Success, Is.True);
        }


    }
}
