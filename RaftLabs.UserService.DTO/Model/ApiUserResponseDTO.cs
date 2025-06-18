using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaftLabs.UserService.DTO.Model
{
    public class ApiUserResponseDTO
    {
        public class ApiUserData
        {
            public int? Id { get; set; }
            public string? Email { get; set; }
            public string? First_Name { get; set; }
            public string? Last_Name { get; set; }
        }

        public class UserResponse
        {
            public ApiUserData? Data { get; set; }
        }

        public class UserListResponse
        {
            public int Page { get; set; }
            public int Total_Pages { get; set; }
            public List<ApiUserData>? Data { get; set; }
        }
    }
}
