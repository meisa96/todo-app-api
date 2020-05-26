using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Task.Data;
using Task.Dtos;
using Task.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task.Helpers;

namespace Task.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ToDosController : ControllerBase
    {
        private readonly IToDoRepository _repo;
        private readonly IMapper _mapper;

        public ToDosController(IToDoRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }
        [HttpPost]
        public async Task<IActionResult> AddToDo(ToDoForAddDto todoForAddDto)
        {
            int UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            todoForAddDto.UserId = UserId;

            var todoToAdd = _mapper.Map<ToDo>(todoForAddDto);

            _repo.Add(todoToAdd);
            
            if (await _repo.SaveAll())
                return Ok(todoToAdd.Id);

            throw new Exception($"Add Todo failed on save");
        }



        [HttpGet]
        public async Task<IActionResult> GetToDos([FromQuery]PageParams pageParams)
        {
            int UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var todos = await _repo.GetToDos(pageParams, UserId);

            var todosToReturn = _mapper.Map<IEnumerable<ToDoForListDto>>(todos);

            Response.AddPagination(todos.CurrentPage,todos.PageSize,
                todos.TotalCount,todos.TotalPages);


            return Ok(todosToReturn);
        }

        [HttpGet("GetTimeOverToDos")]
        public async Task<IActionResult> GetTimeOverToDos()
        {
            int UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var todos = await _repo.GetTimeOverToDos(UserId);

            var todosToReturn = _mapper.Map<IEnumerable<ToDoForListDto>>(todos);

            return Ok(todosToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetToDo(int id)
        {
            int UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var todo = await _repo.GetToDo(id, UserId);

            return Ok(todo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToDo(int id, ToDoForUpdateDto todoForUpdateDto)
        {
            int UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var todoFromRepo = await _repo.GetToDo(id, UserId);

            _mapper.Map(todoForUpdateDto, todoFromRepo);

            if (await _repo.SaveAll())
                return NoContent();
            
            throw new Exception($"Updating Todo {id} failed on save");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            int UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var todo = await _repo.GetToDo(id, UserId);
            _repo.Delete(todo);

            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Failed to delete todo");
        }


    }
}