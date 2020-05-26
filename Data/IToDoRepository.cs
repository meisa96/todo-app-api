using System.Collections.Generic;
using System.Threading.Tasks;
using Task.Models;
using Task.Helpers;

namespace Task.Data
{
    public interface IToDoRepository
    {
         void Add(ToDo entity);
         void Delete<T>(T entity) where T: class;
         Task<bool> SaveAll();
         Task<PagedList<ToDo>> GetToDos(PageParams pageParams,int userId);
        Task<IEnumerable<ToDo>> GetTimeOverToDos(int userId);
        Task<ToDo> GetToDo(int todoid,int userId);
        Task<Image> GetImage(int id,int userId);
    }
}