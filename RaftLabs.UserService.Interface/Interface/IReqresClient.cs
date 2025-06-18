using RaftLabs.UserService.DTO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaftLabs.UserService.Interface.Interface
{
    public interface IReqresClient
    {
        Task<UserDTO?> GetUserByIdAsync(int userId);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
    }
}
