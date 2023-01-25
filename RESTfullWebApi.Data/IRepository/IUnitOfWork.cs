using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerRecord.Data.Data.IRepository;

namespace RESTfullWebApi.Data.IRepository
{
  public interface IUnitOfWork : IDisposable
  {
    IUserRepository UserRepository { get; }
    Task SaveAsync();
  }
}
