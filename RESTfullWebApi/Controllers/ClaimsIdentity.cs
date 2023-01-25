using System.Security.Claims;

namespace RESTfullWebApi.Controllers
{
  internal class ClaimsIdentity : System.Security.Claims.ClaimsIdentity
  {
    private Claim[] claims;

    public ClaimsIdentity(Claim[] claims)
    {
      this.claims = claims;
    }
  }
}