using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;

namespace API.Utils
{
	[ApiController]
	[Route("[controller]")]
	public class DebugTokenController : ControllerBase
	{
		[HttpGet]
		public async Task<IActionResult> GetToken()
		{
			var clientHandler = new HttpClientHandler
			{
				ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
			};
			var client = new HttpClient(clientHandler);

			var disco = await client.GetDiscoveryDocumentAsync(
				Environment.GetEnvironmentVariable("IDENTITY_SERVER") ?? "https://localhost:5002");
			
			if (disco.IsError)
			{
				return BadRequest(disco.Error);
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
				return Unauthorized(tokenResponse.Error);
			}

			return Ok($"Bearer {tokenResponse.AccessToken}");
		}
	}
}