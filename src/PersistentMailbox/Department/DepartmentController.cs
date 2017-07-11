using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PersistentMailbox.Department.Messages;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PersistentMailbox.Department
{
    [Route("api/[controller]")]
    public class DepartmentController : Controller
    {
        private readonly ActorManager _manager;

        public DepartmentController(ActorManager manager)
        {
            _manager = manager;
        }

        // GET: api/values
        [HttpGet]
        public async Task<IEnumerable<DepartmentInfo>> Get()
        {
             var actor = _manager.Get<DepartmentManagerActor>();
            var request = new GetDepartmentsRequest();
            var response = await actor.RequestAsync<GetDepartmentsResponse>(request);
            return response.DepartmentInfos;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] CreateDepartmentMessage value)
        {

            _manager.Get<DepartmentManagerActor>().Tell(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}