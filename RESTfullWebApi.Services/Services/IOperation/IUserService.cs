using CSharpFunctionalExtensions;
using RESTfullWebApi.Common.Common;
using RESTfullWebApi.Model.Dto.UserDto;

namespace RESTfullWebApi.Services.Services.IOperation
{
  public interface IUserService
  {

    Task<Result<ResponseModel<string>>> LoginUserAsync(UserLoginDto request);
    Task<Result<ResponseModel>> CreateUserAsync(AddUserDto request);
    Task<Result<ResponseModel>> UpdateUserAsync(UserDto request);
    Task<Result<ResponseModel<List<UserDto>>>> GetAllUserAsync();
    Task<Result<ResponseModel<UserDto>>> GetUserByIdAsync(int id);
    Task<Result<ResponseModel>> DeleteUserAsync(int id);
  }
}
