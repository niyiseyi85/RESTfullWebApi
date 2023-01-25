using CustomerRecord.Data;
using CustomerRecord.Data.Data.IRepository;
using RESTfullWebApi.Data.IRepository;
using System;
using System.Threading.Tasks;

namespace RESTfullWebApi.Data.Repository
{
  public class UnitOfWork : IUnitOfWork
  {
    private readonly DataContext _context;
    private IUserRepository _userRepository;
    public UnitOfWork(DataContext context, IUserRepository userRepository)
    {
      _context = context;
      _userRepository = userRepository;
    }

    public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);    
    public async Task SaveAsync()
    {
      await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
      _context.Dispose();
      GC.SuppressFinalize(this);
    }
  }
}
