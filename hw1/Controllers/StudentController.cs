using hw1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks.Dataflow;

namespace hw1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private static List<Students> li = new List<Students>
        {
            new Students {id = 1, name= "tuan", age = 20},
            new Students {id = 2, name= "hung", age = 20},
            new Students {id = 3, name= "dung", age = 20},
            new Students {id = 4, name= "anh", age = 20}
        };

        [HttpGet]
        public ActionResult<IEnumerable<Students>> GetAll()
        {
            return Ok(li);
        }

        [HttpGet("{id}")]
        public ActionResult<Students> GetById(int id) 
        {
            var z = li.FirstOrDefault(s => s.id == id);
            if(z == null)
            {
                   return NotFound();
            }
            return Ok(z);
        }

        [HttpPost]
        public ActionResult<Students> Create(Students s)
        {
            s.id = li.Max(s => s.id) + 1;
            li.Add(s);
            return CreatedAtAction(nameof(GetById), new {id = s.id}, s);
        }

        [HttpPut("{id}")]
        public ActionResult<Students> Update(int id, Students value)
        {
            var z = li.FirstOrDefault(s => s.id == id);
            if(z == null)
            {
                return NotFound();
            }

            z.name = value.name;
            z.age = value.age;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult<Students> Delete(int id)
        {
            var z = li.FirstOrDefault(s => s.id == id);
            if(z == null)
            {
                return NotFound();
            }

            li.Remove(z);
            return NoContent();
        }

    }
}
