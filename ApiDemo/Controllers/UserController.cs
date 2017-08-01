using ApiDemo.Models;
using DapperExtensions.Extend;
using System.Web.Http;
using Dapper;

namespace ApiDemo.Controllers
{
    [RoutePrefix("users2")]
    public class UserController : ApiController
    {
        public IRespositoryBase<user> userRespo { get; set; }

        [Route("all")]
        [HttpGet]
        public IHttpActionResult GetUsers()
        {
            //var list = userRespo.GetList(x => x.Age > 21, null);
            var list = userRespo.Context.DB.Query<user>("select * from user");
            return Json(new { data = list });
        }

        [Route("add")]
        [HttpGet]
        public IHttpActionResult CreateUser()
        {
            var u = new user() { Id = 1111111111, Name = "fd13", Age = 26 };
            //指定主键Id
            var b = userRespo.Insert(u);
            //不指定主键Id
            userRespo.Insert(u, x => x.Id);
            return Json(b);
        }

        [HttpGet]
        [Route("update")]
        public IHttpActionResult UpdateUser()
        {
            var f1 = new DbFiled<user>(x => x.Age, 27);
            var b = userRespo.Update(x => x.Id == 56907279991046144, f1);
            return Json(b);
        }

        [HttpGet]
        [Route("one")]
        public IHttpActionResult GetUser()
        {
            var b = userRespo.Get(x => x.Id, 1111111111);
            return Json(b);
        }

        [HttpGet]
        [Route("page")]
        public IHttpActionResult GetUserPaging()
        {
            var total = 0;
            Sorting<user>[] sorts = new Sorting<user>[] { new Sorting<user>(x => x.Id, SortType.Desc) };
            var b = userRespo.GetPage(x => x.Id > 1, sorts, 1, 5, false, ref total);
            return Json(b);
        }
    }
}
