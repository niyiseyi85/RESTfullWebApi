using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTfullWebApi.Model.ResponseModel
{
  public static class UserResponseModel
  {
    public static class Message
    {
      public const string UserCreationSuccessfull = "User record is created successfully ";
      public const string UserUpdateSuccessfull = "User record is successfully update";
      public const string UserDeleteSuccessfull = "User record is successfully Deleted";
      public const string UserByIdSuccessfull = "get User record by id successfully ";

    }
    public static class ErrorMessage
    {
      public const string UserCreationFailed = "User record creation failed ";
      public const string UserExist = "User record already exist";
      public const string UserUpdateFailed = " failed to update User record";
      public const string UserNotExist = "User dose not exist";
      public const string UserDeleteFailed = "User record delete failed ";
      public const string UserByIdFailed = "get User record by id failed ";
    }
  }
}