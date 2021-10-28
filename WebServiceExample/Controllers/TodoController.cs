using System;
using System.Collections.Generic;
using AdvancedTodo.Data;
using AdvancedTodo.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebServiceExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoController : ControllerBase
    {
        private ITodosData _todosData;

        public TodoController(ITodosData todosData)
        {
            this._todosData = todosData;
        }

        [HttpGet]
        public ActionResult<IList<Todo>> GetTodos([FromQuery] bool? isCompleted, [FromQuery] int? userId)
        {
            try
            {
                IList<Todo> todos = _todosData.GetTodos();
                IList<Todo> filteredTodos = new List<Todo>();
                if (isCompleted == null && userId==null)
                {
                    filteredTodos = todos;
                }
                else if (userId != null)
                {
                    foreach(Todo todo in todos)
                    {
                        if(todo.UserId==userId)filteredTodos.Add(todo);
                    }
                }
                else
                {
                    foreach (var todo in todos)
                    {
                        if(todo.IsCompleted.Equals(isCompleted))filteredTodos.Add(todo);
                    }
                }
                return Ok(filteredTodos);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public ActionResult<Todo> AddTodo([FromBody] Todo todo)
        {
            try
            {
                _todosData.AddTodo(todo);
                return Created($"/{todo.TodoId}", todo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public ActionResult DeleteTodo([FromRoute] int id)
        {
            try
            {
                _todosData.RemoveTodo(id);
                return Ok(); 
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPatch]
        [Route("{id:int}")]
        public ActionResult UpdateTodo([FromRoute] int id, [FromQuery] string title, [FromQuery] bool isCompleted)
        {
            try
            {
                Todo todoToUpdate = _todosData.Get(id);
                if(title!=null)
                    todoToUpdate.Title = title;
                todoToUpdate.IsCompleted = isCompleted;
                _todosData.Update(todoToUpdate);
                return Ok(); 
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
    }
}