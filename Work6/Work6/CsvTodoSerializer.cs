using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Work6
{
	public class CsvTodoSerializer
	{
		private readonly string _filePath;

		public CsvTodoSerializer(string filePath)
		{
			_filePath = filePath;

			if(!File.Exists(filePath))
				Serialize(new TodoList());
		}

		public void Serialize(TodoList todolist)
		{
			using(var writer = new StreamWriter(_filePath))
			{
				writer.WriteLine("Title;Description;Deadline;Tags");

				foreach(var task in todolist.Tasks)
				{
					var tags = string.Join(" ", task.Tags);

					writer.WriteLine("{0};{1};{2};{3}", task.Title, task.Description, task.Deadline, tags);
				}
			}
		}

		public TodoList Deserialize()
		{
			var todolist = new TodoList();
			var fileLines = new List<string>();

			using(var reader = new StreamReader(_filePath))
			{
				string line;
				while((line = reader.ReadLine()) != null)
				{
					fileLines.Add(line);
				}
			}

			var columns = fileLines[0].Split(';').ToList();

			for(var i = 1; i < fileLines.Count; i++)
			{
				var segments = fileLines[i].Split(';').ToList();

				var title = segments[columns.IndexOf("Title")];
				var description = segments[columns.IndexOf("Description")];

				DateTime deadline;
				DateTime.TryParse(segments[columns.IndexOf("Deadline")], out deadline);

				var tags = segments[columns.IndexOf("Tags")].Split(' ').ToList();


				var task = new Task(title) {Description = description, Deadline = deadline};

				foreach(var tag in tags)
					task.AddTag(tag);

				todolist.AddTask(task);
			}

			return todolist;
		}
	}
}
