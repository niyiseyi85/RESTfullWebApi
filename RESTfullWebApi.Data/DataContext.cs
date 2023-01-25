using Microsoft.EntityFrameworkCore;
using RESTfullWebApi.Model.Models;

namespace RESTfullWebApi.Data
{
  public class DataContext : DbContext
  {
    
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }
    public virtual DbSet<User> Users { set; get; }
  }
}
