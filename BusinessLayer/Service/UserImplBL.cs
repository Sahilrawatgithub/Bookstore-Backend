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
    public class UserImplBL : IUserBL
    {
        private readonly IUserRL userRl;
        public UserImplBL(IUserRL userRl)
        {
            this.userRl = userRl;
        }
        public async Task<ResponseDTO<string>> RegisterUserAsync(RegUserDTO request)
        {
            return await userRl.RegisterUserAsync(request);
        }
        public async Task<ResponseDTO<LoginResponseDTO>> LoginAsync(LoginRequestDTO response)
        {
            return await userRl.LoginAsync(response);
        }
        public async Task<ResponseDTO<List<UserEntity>>> GetAllUsersAsync()
        {
            return await userRl.GetAllUsersAsync();
        }
        public async Task<ResponseDTO<string>> DeleteUserAsync(string email)
        {
            return await userRl.DeleteUserAsync(email);
        }
        public async Task<ResponseDTO<string>> ForgotPasswordAsync(string email)
        {
            return await userRl.ForgotPasswordAsync(email);
        }
        public async Task<ResponseDTO<string>> ResetPasswordAsync(string email, string newPassword)
        {
            return await userRl.ResetPasswordAsync(email, newPassword);
        }
    }
    
}
