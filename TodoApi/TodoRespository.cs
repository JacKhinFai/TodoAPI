using System.Xml;
using Microsoft.VisualBasic;
using TodoApi.Models;
using Newtonsoft.Json;

namespace TodoApi.Services
{
    public class TodoRespository
    {
        private List<TodoItem> todoItems = new List<TodoItem>();
        private const string FilePath = "todos.json"; // Path to the JSON file
        private int nextId = 1;
        public List<TodoItem> GetAll() => todoItems;
        public List<TodoItem> GetFilterSort(string? sortby, bool? completed, DateTime? dueDate)
        {
            var filteredTodo = todoItems.Where(todoItems => (!completed.HasValue || todoItems.Completed == completed.Value) &&
                                                (!dueDate.HasValue || todoItems.DueDate?.Date == dueDate.Value.Date)).ToList();
            filteredTodo = sortby?.ToLower() switch
            {
                "name" => filteredTodo.OrderBy(x => x.Name).ToList(),
                "duedate" => filteredTodo.OrderBy(x => x.DueDate).ToList(),
                "completed" => filteredTodo.OrderBy(x => x.Completed).ToList(),
                _ => filteredTodo.OrderBy(x => x.Id).ToList()
            };
            return filteredTodo.ToList();
        }

        private List<TodoItem> LoadFromFile()
        {
            if (File.Exists(FilePath))
            {
                var jsonData = File.ReadAllText(FilePath);
                return JsonConvert.DeserializeObject<List<TodoItem>>(jsonData) ?? new List<TodoItem>();
                //nextId = todoItems.Any() ? todoItems.Max(x => x.Id) + 1 : 1;
            }
            return new List<TodoItem>();
        }
        public TodoItem? GetById(int id) => todoItems.FirstOrDefault(t => t.Id == id);
        public TodoItem Add(TodoItem todo)
        {
            LoadFromFile();
            todo.Id = nextId++;
            todoItems.Add(todo);
            SavetoFile();
            return todo;
        }
        public bool Update(int id, TodoItem updatedTodo)
        {
            LoadFromFile();
            var existingTodo = GetById(id);
            if (existingTodo == null)
            {
                return false;
            }
            existingTodo.Name = updatedTodo.Name;
            existingTodo.Description = updatedTodo.Description;
            existingTodo.DueDate = updatedTodo.DueDate;
            existingTodo.Completed = updatedTodo.Completed;
            SavetoFile();
            return true;
        }
        public bool Delete(int id)
        {
            LoadFromFile();
            var todo = GetById(id);
            if (todo == null)
            {
                return false;
            }
            todoItems.Remove(todo);
            SavetoFile();
            return true;
        }
        private void SavetoFile()
        {
            var jsonFile = JsonConvert.SerializeObject(new {todoItems, Newtonsoft.Json.Formatting.Indented});
            File.WriteAllText(FilePath, jsonFile);
        }
    }
}