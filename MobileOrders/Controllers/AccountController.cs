using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrdersRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Newtonsoft.Json;
using MobileOrders.Models;
using Microsoft.IdentityModel.Tokens;

namespace MobileOrders.Controllers
{
    [Produces("application/json")]
    public class AccountController : Controller
    {
		private OrdersDbContext db;
		public AccountController(OrdersDbContext ordersContext)
		{
			db = ordersContext;
		}
		[HttpPost("api/token")]
		public async Task Token(LoginModel model)
		{
			string mobileID = Request.Form["mobileID"];
			string code = Request.Form["agentCode"];
			string password = Request.Form["password"];
			try
			{
				mobileID = Request.Form["mobileID"];
				code = Request.Form["agentCode"];
				password = Request.Form["password"];
			}
			catch
			{
				mobileID = model.mobileID;
				code = model.agentCode;
				password = model.password;
			}
			if (!db.MobileUsers.Any(s=>s.AgentId == code && s.AgentPassword.ToLower() == password.ToString().ToLower()))
			{
				Response.StatusCode = 404;
				await Response.WriteAsync("Page not found."); ;
				return;
			}
			if(db.MobileUsers.Any(s=>s.AgentMobileId == mobileID))
			{
				var mobileAgent = db.MobileUsers.FirstOrDefault(s => s.AgentMobileId == mobileID);
				mobileAgent.AgentMobileId = String.Empty;
				db.SaveChanges();
			}
			var mobileUser = db.MobileUsers.SingleOrDefault(s => s.AgentId == code);
			mobileUser.AgentMobileId = mobileID;
			db.SaveChanges();
			var identity = GetIdentity(mobileID);
			var now = DateTime.UtcNow;
			// создаем JWT-токен
			var jwt = new JwtSecurityToken(
					issuer: AuthOptions.ISSUER,
					audience: AuthOptions.AUDIENCE,
					notBefore: now,
					claims: identity.Claims,
					expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
					signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
			var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

			var response = new
			{
				access_token = encodedJwt,
				username = identity.Name
			};

			// сериализация ответа
			Response.ContentType = "application/json";
			await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
		}
		#region Helpers
		private ClaimsIdentity GetIdentity(string username)
		{
			var claims = new List<Claim>
				{
					new Claim(ClaimsIdentity.DefaultNameClaimType, username),
					new Claim(ClaimsIdentity.DefaultRoleClaimType, "agent")
				};
			ClaimsIdentity claimsIdentity =
			new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
				ClaimsIdentity.DefaultRoleClaimType);
			return claimsIdentity;
		}
		#endregion
	}
}