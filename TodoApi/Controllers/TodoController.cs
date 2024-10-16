using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly TodoRespository todoRespository;
        public TodoController()
        {
            todoRespository = new TodoRespository();
        }
        // POST
        [HttpPost]
        public IActionResult CreateTodoItem(TodoItem newTodo)
        {
            // TITLE
            if (newTodo.Name == null)
            {
                return BadRequest("Name is required.");
            }
            var todo = todoRespository.Add(newTodo);
            return CreatedAtAction(nameof(GetTodoItem), new { id = todo.Id }, todo);
        }
        // GET
        [HttpGet]
        public ActionResult<List<TodoItem>> GetAllTodos([FromQuery] string? sortby, 
        [FromQuery] bool? completed, [FromQuery] DateTime? dueDate)
        {
            //var todos = todoRespository.GetAll();
            var todos = todoRespository.GetFilterSort(sortby, completed, dueDate);
            return Ok(todos);
        }
        // GET
        [HttpGet("{id}")]
        public IActionResult GetTodoItem(int id)
        {
            var todo = todoRespository.GetById(id);
            if (todo == null)
            {
                return NotFound();
            }
            return Ok(todo);
        }
        // PUT
        [HttpPut("{id}")]
        public IActionResult UpdateTodoItem(int id, TodoItem updatedTodo)
        {
            if (!todoRespository.Update(id, updatedTodo))
            {
                return NotFound();
            }
            return NoContent();
        }
        // DELETE
        [HttpDelete("{id}")]
        public IActionResult DeleteTodoItem(int id)
        {
            if (!todoRespository.Delete(id))
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
