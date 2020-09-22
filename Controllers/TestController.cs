using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DatelAPI.Controllers
{
    public class TestController : ApiController
    {

        [Route("Test")]
        public string Get()
        {
            return "Test Successful";
        }
    }
}
