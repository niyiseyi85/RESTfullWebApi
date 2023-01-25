using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using RESTfullWebApi.Model.Models;

namespace RESTfullWebApi.Model.Dto.UserDto
{
  public class UserLoginDto
  {    
      public string? Username { get; set; }
      public string? Password { get; set; }
  }
  public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
  {
    public UserLoginDtoValidator()
    {
      RuleFor(x => x.Username)
          .NotEmpty().WithMessage("{PropertyName} is required")
          .NotNull();
      //.MaximumLength(50);
      RuleFor(x => x.Password)
        .NotEmpty().WithMessage("{PropertyName} is required")
        .NotNull();
        
    } 
  }
  public class UserLoginRequestMappingConfig : Profile
  {
    public UserLoginRequestMappingConfig()
    {
      CreateMap<User, UserLoginDto>().ReverseMap();
    }
  }
}
