using CustomerRecord.Data.Data.IRepository;
using RESTfullWebApi.Data.IRepository;
using RESTfullWebApi.Model.Models;

namespace RESTfullWebApi.Data.Repository
{
  public class UserRepository : RepositoryGeneric<User>, IUserRepository
  {
    private readonly DataContext context;

    public UserRepository(DataContext context) : base(context)
    {
      this.context = context;
    }
    public void UpdateUser(User user)
    {
      this.Update(user);
    }
    public async Task<User> AddUser(User user)
    {
      
      return await this.Add(user);
    }
  }
}
