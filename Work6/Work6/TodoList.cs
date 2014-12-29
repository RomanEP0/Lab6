using System.Collections.Generic;
using System.Linq;

namespace Work6
{
	public class TodoList
	{
		public TodoList()
		{
			_tasks = new List<Task>();
		}

		private readonly List<Task> _tasks;
		public IEnumerable<Task> Tasks { get { return _tasks; } }

		public IEnumerable<Task> Search(IEnumerable<string> tags)
		{
			return _tasks.OrderBy(t => t.Deadline).Where(task => task.Tags.Any(tags.Contains));
		}

		public IEnumerable<Task> Last()
		{
			return _tasks.OrderBy(t => t.Deadline).Take(5);
		}

		public void AddTask(Task task)
		{
			_tasks.Add(task);
		}

		public void RemoveTask(Task task)
		{
			_tasks.Remove(task);
		}
	}
}
