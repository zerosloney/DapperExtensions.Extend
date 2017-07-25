using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DapperExtensions.Extend;
using ApiDemo.Models;

namespace ApiDemo.Controllers
{
    public class UserController : ApiController
    {
        private IDapperContext context = new DapperContext("db");
        private IRespositoryBase<user> userRespo;

        public UserController()
        {
            userRespo = new RespositoryBase<user>(context);
        }

        [HttpGet]
        public IHttpActionResult GetUsers()
        {
            var list = userRespo.GetList(x => x.Age > 21 && x.Name.StartsWith("7G"), null).ToList();
            return Json(list);
        }
    }
}
