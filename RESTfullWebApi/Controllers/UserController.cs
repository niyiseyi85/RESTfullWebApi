using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTfullWebApi.Common.Common;
using RESTfullWebApi.Services.Services.IOperation;
using RESTfullWebApi.Model.Dto.UserDto;
using RESTfullWebApi.Data;

namespace RESTfullWebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UserController : ControllerBase
  {
    private readonly IUserService _UserService;
    private readonly IValidator<UserDto> _UserDtoValiadator;
    private readonly IValidator<AddUserDto> _AddUserDtoValiadator;
    private readonly IValidator<UserLoginDto> _UserLoginDtoValiadator;
    private readonly DataContext _context;
    public UserController(IUserService UserService, IValidator<UserDto> UserDtoValiadator
      , DataContext context, IValidator<AddUserDto> AddUserDtoValidator, IValidator<UserLoginDto> UserLoginDtoValidator)
    {
      _UserService = UserService;
      _UserDtoValiadator = UserDtoValiadator;
      _AddUserDtoValiadator = AddUserDtoValidator;
      _UserLoginDtoValiadator = UserLoginDtoValidator;
      _context = context;
    }
    /// <summary>
    /// Addind a new User record
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("AddUser")]
    [Produces(typeof(Envelope<ResponseModel>))]
    public async Task<IActionResult> CreateUser([FromBody]AddUserDto request)
    {
      var validateModel = await _AddUserDtoValiadator.ValidateAsync(request);
      if (!validateModel.IsValid)
      {
        return BadRequest();
      }
      var response = await _UserService.CreateUserAsync(request);
      Result res = Result.Combine(response);
      if (res.IsFailure)
        if (response.Equals(null))
          return BadRequest();
      return Ok(response.Value);
    }
    /// <summary>
    /// user login and get token
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("User-Login")]
    [Produces(typeof(Envelope<ResponseModel>))]
    public async Task<IActionResult> LoginUser([FromBody] UserLoginDto request)
    {
      var validateModel = await _UserLoginDtoValiadator.ValidateAsync(request);
      if (!validateModel.IsValid)
      {
        return BadRequest();
      }
      var response = await _UserService.LoginUserAsync(request);
      Result res = Result.Combine(response);
      if (res.IsFailure)
        if (response.Equals(null))
          return BadRequest();
      return Ok(response.Value);
    }
    /// <summary>
    /// update User record
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("UpdateUser")]
    [Produces(typeof(Envelope<ResponseModel>))]
    public async Task<IActionResult> UpdateUser([FromBody] UserDto request)
    {
      var validateModel = await _UserDtoValiadator.ValidateAsync(request);
      if (!validateModel.IsValid)
      {
        return NoContent();
      }
      var response = await _UserService.UpdateUserAsync(request);
      Result res = Result.Combine(response);
      if (res.IsFailure)
        if (response.Equals(null))
          return BadRequest();
      return Ok(response.Value);
    }/// <summary>
    ///  Get All User record
    /// </summary>
    /// <returns></returns>
    [Produces(typeof(Envelope<ResponseModel<List<UserDto>>>))]
    [ProducesDefaultResponseType(typeof(UserDto))]
    [HttpGet]
    public async Task<IActionResult> GetAllUser()
   {
      //return Ok(await _context.Users.ToListAsync());
      var response = await _UserService.GetAllUserAsync();
      Result res = Result.Combine(response);
      if (res.IsFailure)
        if (response.Equals(null))
          return BadRequest();
      return Ok(response.Value);
    }
    /// <summary>
    /// Get User record by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Produces(typeof(Envelope<ResponseModel>))]
    [ProducesDefaultResponseType(typeof(UserDto))]
    [HttpGet("GetUserById/{id:int}")]
    public async Task<IActionResult> GetByIdUser(int id)
    {
      var response = await _UserService.GetUserByIdAsync(id);
      Result res = Result.Combine(response);
      if (res.IsFailure)
        if (response.Equals(null))
          return BadRequest();
      return Ok(response.Value);
    }
    /// <summary>
    /// delete User record
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("DeleteUser/{id}")]
    [Produces(typeof(Envelope<ResponseModel>))]
    public async Task<IActionResult> DeleteUser(int id)
    {
      var response = await _UserService.DeleteUserAsync(id);
      Result res = Result.Combine(response);
      if (res.IsFailure)
        if (response.Equals(null))
          return BadRequest();
      return Ok(response.Value);
    }
  }  
}
