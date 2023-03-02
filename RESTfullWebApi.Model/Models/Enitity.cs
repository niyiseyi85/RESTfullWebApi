using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTfullWebApi.Model.Models
{
  public interface IEntity
  {
    int Id { get; }
    DateTime CreatedDate { get; }
    DateTime? ModifiedDate { get; }
  }

  public class Entity : IEntity
  {
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public int CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public int? ModifiedBy { get; set; }

    public int? lawal { get; set; }

  }
}
