using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;

namespace Challenge_EF.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class DebugController : ControllerBase
	{
		[HttpPost]
		public async Task<IActionResult> GetToken()
		{
			var clientHandler = new HttpClientHandler();
			clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
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

				ClientId = "client",
				ClientSecret = "secret",
				Scope = "api_challenge"
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