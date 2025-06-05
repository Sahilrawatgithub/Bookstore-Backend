using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Entity;
using RepositoryLayer.DTO;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        public Task<ResponseDTO<string>> RegisterUserAsync(RegUserDTO request);
        public Task<ResponseDTO<LoginResponseDTO>> LoginAsync(LoginRequestDTO response);

        public Task<ResponseDTO<List<UserEntity>>> GetAllUsersAsync();
        public Task<ResponseDTO<string>> DeleteUserAsync(string email);
        public Task<ResponseDTO<string>> ForgotPasswordAsync(string email);
        public Task<ResponseDTO<string>> ResetPasswordAsync(string email, string newPassword);
    }
}
