using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace ADAssignment.Managers
{
    public class AzureKeyVaultManager
    {
        private async Task<string> AuthenticateVault(string authority, string resource, string scope)
        {
            var clientCredential = new ClientCredential("68d54b26-7fdb-4754-8330-8b5b02788bcb",
                "Er69Ic]9jByKpejEHzi8x=x:=j@/KxKN");
            var authenticationContext = new AuthenticationContext(authority);
            var result = await authenticationContext.AcquireTokenAsync(resource, clientCredential);
            return result.AccessToken;
        }

        public string GetSecret(string secretId)
        {
            var keyVaultClient = new KeyVaultClient(AuthenticateVault);
            var result = keyVaultClient
                .GetSecretAsync(secretId);

            var secret = result.Result.Value;

            return secret;
        }
    }
}