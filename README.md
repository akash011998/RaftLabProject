
RaftLabs 


This solution is built to fetch and manage user data from an external system, simulating integration with public APIs. 
We use [https://reqres.in](https://reqres.in) as the public API source.

The project is structured using Clean Architecture to promote separation of concerns, testability, and maintainability.


1. Solution Structure

RaftLabs.UserService.ConsoleApp       -> Console app to demonstrate the service usage
RaftLabs.UserService.DTO              -> Contains DTOs and configurations
RaftLabs.UserService.Interface        -> Service interfaces
RaftLabs.UserService.Persistence      -> API client implementation and logic
RaftLabs.UserService.UnitTest         -> xUnit tests for the services


2. Features

API Client Implementation

- GET `/api/users?page={page}` – Get paginated user list  
- GET `/api/users/{userId}` – Get user by ID  
- Uses `HttpClient` properly with `async/await`

Data Modeling & Mapping

- Models like UserDTO` map API JSON response to C#.

Service Layer

Implemented ExternalUserService with:

C#
Task<UserDTO> GetUserByIdAsync(int userId)
Task<IEnumerable<UserDTO>> GetAllUsersAsync() 


Configuration

- Base URL (`https://reqres.in/api/`) is configurable via appsettings.json in the Console project.

Error Handling

   Handles:
  - API failures (timeouts, unreachable server)
  - HTTP non-success codes like 404
  - JSON deserialization issues
 Logs and throws meaningful exceptions.


Added Bonus Features 

- Clean Architecture
- Unit Testing using xUnit
- Proper error handling

c#
dotnet run --project RaftLabs.UserService.ConsoleApp

Run Unit Tests

dotnet test

=====================================
Screen Recording Video Link:
https://drive.google.com/file/d/1_VOdVWHPPl3-EoepZgjbePY0pwR50-XG/view?usp=sharing
