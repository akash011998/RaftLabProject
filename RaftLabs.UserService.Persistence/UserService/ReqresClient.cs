using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RaftLabs.UserService.DTO.Model;
using RaftLabs.UserService.Interface.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static RaftLabs.UserService.DTO.Model.ApiUserResponseDTO;

namespace RaftLabs.UserService.Persistence.UserService
{
    public class ReqresClient : IReqresClient
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly ILogger<ReqresClient> _logger;

        public ReqresClient(HttpClient httpClient, IMemoryCache cache, ILogger<ReqresClient> logger)
        {
            _httpClient = httpClient;
            _cache = cache;
            _logger = logger;
        }
        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = new List<UserDTO>();
            int page = 1;

            try
            {
                while (true)
                {
                    var response = await _httpClient.GetAsync($"users?page={page}");

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        _logger.LogWarning("Unauthorized request.");
                        Console.WriteLine("Access denied. Please check your credentials.");
                        return users; // return empty or partial list
                    }

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError($"Failed to fetch users. Status Code: {response.StatusCode}");
                        Console.WriteLine($"Failed to fetch users. Status Code: {response.StatusCode}");
                        return users;
                    }

                    var json = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<UserListResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    users.AddRange(apiResponse.Data.Select(d => new UserDTO
                    {
                        Id = d.Id,
                        Email = d.Email,
                        FirstName = d.First_Name,
                        LastName = d.Last_Name
                    }));

                    if (page >= apiResponse.Total_Pages) break;
                    page++;
                }

                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all users.");
                Console.WriteLine("An unexpected error occurred while fetching users. Please try again later.");
                return users; // return what was fetched so far or empty list
            }


        }

        public async Task<UserDTO?> GetUserByIdAsync(int userId)
        {
            string cacheKey = $"user_{userId}";
            if (_cache.TryGetValue(cacheKey, out UserDTO user)) return user;

            try
            {
                var response = await _httpClient.GetAsync($"users/{userId}");
                if (response.StatusCode == HttpStatusCode.NotFound) return null;
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<UserResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                user = new UserDTO
                {
                    Id = apiResponse.Data.Id,
                    Email = apiResponse.Data.Email,
                    FirstName = apiResponse.Data.First_Name,
                    LastName = apiResponse.Data.Last_Name
                };

                _cache.Set(cacheKey, user, TimeSpan.FromMinutes(5));
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user.");
                throw;
            }
        }
    }
}
