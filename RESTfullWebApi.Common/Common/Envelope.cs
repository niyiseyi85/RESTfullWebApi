using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTfullWebApi.Common.Common
{
  public class Envelope<T>
  {
    public T Result { get; }

    public string ErrorMessage { get; }

    public List<string> ErrorMessages { get; }

    public bool HasError { get; }

    public DateTime TimeGenerated { get; }

    protected internal Envelope(T result, string errorMessage, bool hasError)
    {
      Result = result;
      ErrorMessage = errorMessage;
      HasError = hasError;
      TimeGenerated = DateTime.UtcNow;
    }

    protected internal Envelope(T result, List<string> errorMessages, bool hasError)
    {
      Result = result;
      ErrorMessages = errorMessages;
      HasError = hasError;
      TimeGenerated = DateTime.UtcNow;
    }
  }
}
