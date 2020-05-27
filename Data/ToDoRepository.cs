using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Task.Helpers;

namespace Task.Data
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly DataContext _context;

        public ToDoRepository(DataContext context)
        {
            _context = context;
        }

        public void Add(ToDo entity)
        {
            _context.Add(entity);
            
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<ToDo> GetToDo(int todoid, int userId)
        {
            var todo = await _context.ToDos.FirstOrDefaultAsync(u => u.Id == todoid && u.UserId == userId);

            return todo;
        }

        public async Task<PagedList<ToDo>> GetToDos(PageParams pageParams, int userId)
        {
            var todos = _context.ToDos.Where(x=>x.UserId == userId).ToList().OrderByDescending(x => x.DateTime).GroupBy(x => x.DateTime >= DateTime.Now).ToList();
            var todosToReturn = new List<ToDo>();
            foreach (IGrouping<bool, ToDo> group in todos)
            {
                if (group.Key == true)
                {
                    foreach (var item in group.OrderBy(x=>x.DateTime))
                    {
                        todosToReturn.Add(item);
                    }
                }
                else
                {
                    foreach (var item in group)
                    {
                        todosToReturn.Add(item);
                    }
                }

            }
            return await PagedList<ToDo>.Create(todosToReturn, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<IEnumerable<ToDo>> GetTimeOverToDos(int userId)
        {
            var todos = await  _context.ToDos.Where(x=> x.UserId == userId && x.DateTime < DateTime.Now).ToListAsync();

            return  todos;
        }

        public async Task<Image> GetImage(int id, int userId)
        {
            var photo = await _context.Images.FirstOrDefaultAsync(p => p.ToDo.UserId == userId && p.Id == id);

            return photo;
        }
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }


    }
}