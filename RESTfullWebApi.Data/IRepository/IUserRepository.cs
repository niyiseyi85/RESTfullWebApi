using RESTfullWebApi.Model.Models;

namespace RESTfullWebApi.Data.IRepository
{
  public interface IUserRepository : IRepositoryGeneric<User> 
  {
    Task<User> AddUser(User user);
    void UpdateUser(User User);
  }
}
