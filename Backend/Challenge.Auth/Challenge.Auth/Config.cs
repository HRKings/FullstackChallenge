﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace Challenge.Auth
{
	public class Config
	{
		// scopes define the resources in your system
		public static IEnumerable<IdentityResource> GetIdentityResources()
		{
			return new List<IdentityResource>
			{
				new IdentityResources.OpenId(),
				new IdentityResources.Profile()
			};
		}

		public static IEnumerable<ApiScope> GetApiScopes()
		{
			return new List<ApiScope>
			{
				new ApiScope("api", "API access")
			};
		}

		// clients want to access resources (aka scopes)
		public static IEnumerable<Client> GetClients()
		{
			// client credentials client
			return new List<Client>
			{
				new Client
				{
					ClientId = "api",
					ClientSecrets = {new Secret("secret".Sha256())},
					ClientName = "API Access",
					AllowedGrantTypes = GrantTypes.ClientCredentials,
					AllowedScopes = {"api"}
				},
				new Client
				{
					ClientId = "angular",
					ClientSecrets = {new Secret("secret")},
					ClientName = "Challenge Angular",
					AllowedGrantTypes = GrantTypes.Code,
					RequirePkce = true,
					RequireClientSecret = false,
					AllowedScopes = {"openid", "profile", "api"},
					RedirectUris =
					{
						$"{Environment.GetEnvironmentVariable("FRONTEND_PATH")}/auth-callback",
						$"{Environment.GetEnvironmentVariable("FRONTEND_PATH")}/silent-refresh.html"
					},
					PostLogoutRedirectUris = {Environment.GetEnvironmentVariable("FRONTEND_PATH")},
					AllowAccessTokensViaBrowser = true
				}
			};
		}

		public static List<TestUser> GetUsers()
		{
			return new List<TestUser>
			{
				new TestUser
				{
					SubjectId = "1",
					Username = "test",
					Password = "1234",

					Claims = new List<Claim>
					{
						new Claim("name", "Test"),
						new Claim(JwtClaimTypes.GivenName, "Testing"),
						new Claim(JwtClaimTypes.FamilyName, "test"),
						new Claim(JwtClaimTypes.Email, "test@test.com"),
						new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean)
					}
				}
			};
		}
	}
}