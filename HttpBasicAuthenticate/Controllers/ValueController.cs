using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


using System.Web.Http.Cors;


namespace HttpBasicAuthenticate.Controllers
{
	[HttpBasicAuthenticate]
	public class ValueController : ApiController
	{
		[EnableCors("*", "*", "*")]
		public IEnumerable<User> Get()
		{
			return new List<User>
			{
				new User{ID=1,Name="sunf"},
				new User{ID=2,Name="sunf"},
				new User{ID=3,Name="sunf"},
				new User{ID=4,Name="sunf"},
				new User{ID=5,Name="sunf"}
			};
		}


	}

	public class User
	{
		public int ID { get; set; }
		public string Name { get; set; }
	}
}
