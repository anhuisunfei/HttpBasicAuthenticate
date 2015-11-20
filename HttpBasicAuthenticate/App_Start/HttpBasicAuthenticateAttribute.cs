using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Net.Http;
namespace HttpBasicAuthenticate
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
	public class HttpBasicAuthenticateAttribute : AuthorizeAttribute
	{
		public override void OnAuthorization(HttpActionContext actionContext)
		{
			var identity = ParseAuthorizationHeader(actionContext);
			if (identity == null)
			{
				Challenge(actionContext);
				return;
			}


			if (!OnAuthorizeUser(identity.Name, identity.Password, actionContext))
			{
				Challenge(actionContext);
				return;
			}
		}

		/// <summary>
		/// 认证用户
		/// </summary>
		protected virtual bool OnAuthorizeUser(string username, string password, System.Web.Http.Controllers.HttpActionContext actionContext)
		{
			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
				return false;

			return true;
		}


		/// <summary>
		/// 解析Authorization请求头
		/// </summary>
		protected virtual BasicAuthenticationIdentity ParseAuthorizationHeader(HttpActionContext actionContext)
		{
			string authHeader = null;
			var auth = actionContext.Request.Headers.Authorization;
			if (auth != null && auth.Scheme == "Basic")
				authHeader = auth.Parameter;

			if (string.IsNullOrEmpty(authHeader))
				return null;

			authHeader = Encoding.Default.GetString(Convert.FromBase64String(authHeader));

			var tokens = authHeader.Split(':');
			if (tokens.Length < 2)
				return null;

			return new BasicAuthenticationIdentity(tokens[0], tokens[1]);
		}


		/// <summary>
		/// 发送认证询问请求
		/// </summary>
		void Challenge(HttpActionContext actionContext)
		{
			var host = actionContext.Request.RequestUri.DnsSafeHost;
			actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
			actionContext.Response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", host));
		}
	}

	/// <summary>
	/// 表示基本认证用户
	/// </summary>
	public class BasicAuthenticationIdentity : GenericIdentity
	{
		public BasicAuthenticationIdentity(string name, string password)
			: base(name, "Basic")
		{
			this.Password = password;
		}

		public string Password { get; set; }
	}
}