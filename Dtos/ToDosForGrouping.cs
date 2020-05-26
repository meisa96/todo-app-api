using System.Collections.Generic;

namespace Task.Dtos

{
    public class ToDosForGrouping
    {
        public List<ToDoForListDto> RecentToDos { get; set; }
        public List<ToDoForListDto> TimeOverToDos { get; set; }
    }
}