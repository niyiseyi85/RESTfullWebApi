using AutoMapper;
using CSharpFunctionalExtensions;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RESTfullWebApi.Common.Common;
using RESTfullWebApi.Data.IRepository;
using RESTfullWebApi.Model.ResponseModel;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using RESTfullWebApi.Model.Models;
using RESTfullWebApi.Data;
using RESTfullWebApi.Model.Dto.UserDto;

namespace RESTfullWebApi.Services.Services.IOperation
{
  public class UserService : IUserService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IConfiguration config;
    public UserService(IUnitOfWork unitOfWork, IMapper mapper, DataContext context,IConfiguration configuration)
    {
      _unitOfWork = unitOfWork;
      _mapper = mapper;
      _context = context;
      config = configuration;
    }
    public async Task<Result<ResponseModel>> CreateUserAsync(AddUserDto request)
    {
      var response = new ResponseModel();
      var UserExist = await _context.Users.FirstOrDefaultAsync(x => x.FirstName == request.FirstName);
      if (UserExist == null)
      {
        var user = _mapper.Map<User>(request);        
        try
        {
          CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);          
          user.PasswordSalt = passwordSalt;
          user.PasswordHash = passwordHash;
          await _unitOfWork.UserRepository.AddUser(user);
          await _unitOfWork.SaveAsync();
          response.IsSuccessful = true;
          response.Message = UserResponseModel.Message.UserCreationSuccessfull;
        }
        catch (Exception ex)
        {
          return Result.Failure<ResponseModel>($"{UserResponseModel.ErrorMessage.UserCreationFailed} - {ex.Message} : {ex.InnerException}");
        }
      }
      else
      {
        response.IsSuccessful = false;
        response.Message = UserResponseModel.ErrorMessage.UserExist;
      }
      return response;
    }
    public async Task<Result<ResponseModel>> UpdateUserAsync ( UserDto request)
    {
      var response = new ResponseModel();
      try
      {
        var UserExist = await _unitOfWork.UserRepository.FirstOrDefault(x => x.Id == request.Id);
        if(UserExist != null)
        {
          _mapper.Map(request, UserExist);
          _unitOfWork.UserRepository.UpdateUser(UserExist);
          await _unitOfWork.SaveAsync();
          response.IsSuccessful= true;
          response.Message = UserResponseModel.Message.UserUpdateSuccessfull;
        }
      }
      catch(Exception ex)
      {
        return Result.Failure<ResponseModel>($"{UserResponseModel.ErrorMessage.UserUpdateFailed} - {ex.Message} : {ex.InnerException}");
      }
      return response;
    }
    public async Task<Result<ResponseModel<List<UserDto>>>> GetAllUserAsync()
    {
      var response = new ResponseModel<List<UserDto>>();
      var User = await _unitOfWork.UserRepository.GetAll();
      var result = _mapper.Map<List<UserDto>>(User);
      response.Data = result;
      return response;
    }
    public async Task<Result<ResponseModel<UserDto>>> GetUserByIdAsync(int id)
    {
      var response = new ResponseModel<UserDto>();
      var User = await _unitOfWork.UserRepository.FirstOrDefault(x => x.Id == id);
      if (User == null)
      {
        response.IsSuccessful = false;
        response.Message = UserResponseModel.ErrorMessage.UserNotExist;
      }
      else
      {
        try
        {
          var result = _mapper.Map<UserDto>(User);
          response.IsSuccessful = true;
          response.Message = UserResponseModel.Message.UserByIdSuccessfull;
          response.Data = result;
        }
        catch(Exception ex)
        {
          return Result.Failure<ResponseModel<UserDto>>($"{UserResponseModel.ErrorMessage.UserByIdFailed} - {ex.Message} : {ex.InnerException}");
        }
      }
      return response;
    }
    public async Task<Result<ResponseModel>> DeleteUserAsync(int id)
    {
      var response = new ResponseModel();
      var User = await _unitOfWork.UserRepository.FirstOrDefault(x => x.Id == id);
      if (User == null)
      {
        response.IsSuccessful = false;
        response.Message = UserResponseModel.ErrorMessage.UserNotExist;
      }
      else
      {
        try
        {
           _unitOfWork.UserRepository.Remove(id);
          await _unitOfWork.SaveAsync();
          response.IsSuccessful = true;
          response.Message = UserResponseModel.Message.UserDeleteSuccessfull;
        }
        catch (Exception ex)
        {
          return Result.Failure<ResponseModel>($"{UserResponseModel.ErrorMessage.UserDeleteFailed} - {ex.Message} : {ex.InnerException}");
        }
      }
      return response;
    }
    public async Task<Result<ResponseModel<string>>> LoginUserAsync(UserLoginDto request)
    {
      var response = new ResponseModel<string>();
      var user = await _unitOfWork.UserRepository.FirstOrDefault(x => x.Username == request.Username || x.Email == request.Username);
      if (user is null)
      {
        return Result.Failure < ResponseModel<string>>($"user not is valid");
      }
      if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
      {
        return Result.Failure<ResponseModel<string>>($"password not correct");
      }      
      var token = CreateToken();
      response.Data = token;
      response.IsSuccessful = true;
      return response;
    }
    private string CreateToken()
    {
      string key = config["Identity:Key"];
      string audience = config["Identity:ValidAudience"];
      string issuer = config["Identity:ValidIssuer"];

      JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
      var tokenKey = Encoding.ASCII.GetBytes(key);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new[] { new Claim("UserName", "Admin") }),
        Expires = DateTime.UtcNow.AddHours(1),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature),
        Audience = audience,
        Issuer = issuer
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      return (tokenHandler.WriteToken(token));
      
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
      using (var hmac = new HMACSHA512())
      {
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
      }
    }

    private bool VerifyPasswordHash(string password, byte[] paswordHAsh, byte[] passwordSalt)
    {
      using (var hmac = new HMACSHA512(passwordSalt))
      {
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(paswordHAsh);
      }
    }
  }
}
