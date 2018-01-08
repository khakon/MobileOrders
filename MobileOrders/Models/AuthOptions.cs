using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileOrders.Models
{
	public class AuthOptions
	{
		public const string ISSUER = "Souz"; // издатель токена
		public const string AUDIENCE = "weborders2017.com.ua"; // потребитель токена
		const string KEY = "weborders2017_com_ua";   // ключ для шифрации
		public const int LIFETIME = 180 * 24 * 60; // время жизни токена - в минутах
		public static SymmetricSecurityKey GetSymmetricSecurityKey()
		{
			return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
		}
	}
}
