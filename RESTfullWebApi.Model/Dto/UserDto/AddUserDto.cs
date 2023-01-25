using AutoMapper;
using FluentValidation;
using RESTfullWebApi.Model.Models;

namespace RESTfullWebApi.Model.Dto.UserDto
{
  public class AddUserDto
  {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
  }
  public class AddUserDtoValidator : AbstractValidator<AddUserDto>
  {
    public AddUserDtoValidator()
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
  public class AddUserRequestMappingConfig : Profile
  {
    public AddUserRequestMappingConfig()
    {
      CreateMap<User, AddUserDto>().ReverseMap();
    }
  }

}
