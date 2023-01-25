using AutoMapper;
using FluentValidation;
using RESTfullWebApi.Model.Models;

namespace RESTfullWebApi.Model.Dto.UserDto
{
  public class UserDto
  {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }    
  }
  public class UserDtoValidator : AbstractValidator<UserDto>
  {
    public UserDtoValidator()
    {
      RuleFor(x => x.FirstName)
          .NotEmpty().WithMessage("{PropertyName} is required")
          .NotNull()
          .MaximumLength(50);
      RuleFor(x => x.LastName)
        .NotEmpty().WithMessage("{PropertyName} is required")
        .NotNull()
        .MaximumLength(50);
      RuleFor(x => x.Email)
          .NotEmpty().WithMessage("{PropertyName} is required")
          .NotNull()
          .EmailAddress();
      RuleFor(x => x.Address)
        .NotEmpty().WithMessage("{PropertyName} is required")
        .NotNull()
        .MaximumLength(50);
      RuleFor(x => x.PhoneNumber)
        .NotEmpty().WithMessage("{PropertyName} is required")
        .NotNull();
    }
  }
  public class UserRequestMappingConfig : Profile
  {
    public UserRequestMappingConfig()
    {
      CreateMap<User, UserDto>().ReverseMap();
    }
  }

}
