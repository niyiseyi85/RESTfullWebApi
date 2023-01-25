using IdentityServer4.Models;

namespace RESTfullWebApi
{
  public static class IdentityServerConfig
  {
    public const string RoleIdentityResource = "role";
    public const string ApiResourceName = "BetDossierAPI";
    public static IEnumerable<ApiResource> GetApis(IConfiguration config) //= new List<ApiResource>
    {
      var scopes = config
                .GetSection("Identity:Scopes")
                .Get<IEnumerable<string>>()
                //.Select(x => new Scope(x))
                .ToArray();

      var secret = config["Identity:ApiSecret"];

      yield return new ApiResource
      {
        Name = ApiResourceName,
        DisplayName = ApiResourceName,
        Description = $"{ApiResourceName} Access",
        UserClaims = { RoleIdentityResource },
        ApiSecrets = { new Secret(config["Identity:ApiSecret"].Sha256()) },
        Scopes = scopes
      };

      //new ApiResource("api1", "My API")
    }

    public static IEnumerable<Client> GetClients(IConfiguration config) //= new List<Client>
    {
      var clients = config
                .GetSection("Identity:Clients")
                .Get<List<Client>>();

      var clientSecret = new Secret(config["Identity:ClientSecret"].Sha256());

      clients.ForEach(x =>
              {
                x.AllowedGrantTypes = GrantTypes.ResourceOwnerPassword;
                x.ClientSecrets = new List<Secret> { clientSecret
    };
                x.AllowOfflineAccess = true;
                x.RefreshTokenUsage = TokenUsage.OneTimeOnly;
                x.UpdateAccessTokenClaimsOnRefresh = true;
                //x.AccessTokenLifetime = int.Parse(config["Identity:Clients:TokenLife"]);
                //x.AccessTokenLifetime = 2592000;
                x.IncludeJwtId = true;
              });

      return clients;
      //new Client
      //        {
      //            ClientId = "client",
      //            AllowedGrantTypes = GrantTypes.ClientCredentials,
      //            ClientSecrets = { new Secret("secret".Sha256()) },
      //            AllowedScopes = { "api1" }
      //        }
    }

    public static IEnumerable<IdentityResource> GetIdentityResources()// = new List<IdentityResource>
    {
      yield return new IdentityResources.OpenId();
      yield return new IdentityResources.Profile();
      yield return new IdentityResources.Email();

      yield return new IdentityResource
      {
        Name = RoleIdentityResource,
        DisplayName = RoleIdentityResource,
        UserClaims = { RoleIdentityResource }
      };
      //new IdentityResources.OpenId(),
      //        new IdentityResources.Profile(),
      }
    }

  }
