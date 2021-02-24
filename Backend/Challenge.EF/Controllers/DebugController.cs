using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;

namespace Challenge.EF.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class DebugController : ControllerBase
	{
		[HttpPost]
		public async Task<IActionResult> GetToken()
		{
			var clientHandler = new HttpClientHandler();
			clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
			{
				return true;
			};
			var client = new HttpClient(clientHandler);

			var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5002");
			if (disco.IsError)
			{
				Console.WriteLine(disco.Error);
				return Unauthorized();
			}

			var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
			{
				Address = disco.TokenEndpoint,

				ClientId = "api",
				ClientSecret = "secret",
				Scope = "api"
			});

			if (tokenResponse.IsError)
			{
				Console.WriteLine(tokenResponse.Error);
				return Unauthorized();
			}

			return Ok(tokenResponse.AccessToken);
		}
	}
}