using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Work6
{
	public static class Program
	{
		

		private static Task _newTask;
		private static TodoList _todoList;
		private static readonly CsvTodoSerializer Serializer = new CsvTodoSerializer(Environment.CurrentDirectory + "/todolist.csv");

		static void Main()
		{
			_todoList = Serializer.Deserialize();
			WriteMainView();

			Serializer.Serialize(_todoList);
		}

		public static void WriteMainView()
		{
			while(true)
			{
				Console.Clear();
				var menuList = new List<string> { Strings.newTask, Strings.searchTasksByTags, Strings.lastTasks, Strings.exitEscape, Strings.changeLanguage };

				for(var i = 0; i < menuList.Count; i++)
				{
					Console.WriteLine("{0}> {1}", i, menuList[i]);
				}

				switch(Console.ReadKey().Key)
				{
					case ConsoleKey.D0:	//New task
						EnterTaskTitle();
						break;
					case ConsoleKey.D1:	//Search tasks
						WriteSearchedTasks();
						break;
					case ConsoleKey.D2:	//Last tasks
						WriteLastTasks();
						break;
					case ConsoleKey.D3:	//Exit
						return;
					case ConsoleKey.D4:	//Change language
						Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture.Name == "ru-RU"
							? new CultureInfo("en-US")
							: new CultureInfo("ru-RU");
						continue;
					default:
						continue;
				}
				break;
			}
		}

		private static void EnterTaskTitle()
		{
			Console.Clear();
			Console.WriteLine(Strings.enterTitle);
			var title = Console.ReadLine();
			_newTask = new Task(title);

			EnterTaskDescription();
		}

		private static void EnterTaskDescription()
		{
			Console.Clear();
			Console.WriteLine(Strings.enterDescription);
			_newTask.Description = Console.ReadLine();

			EnterDeadline();
		}

		private static void EnterDeadline()
		{
			Console.Clear();
			Console.WriteLine(Strings.enterDeadline);

			DateTime deadline;
			while(!DateTime.TryParse(Console.ReadLine(), out deadline))
			{
				Console.Clear();
				Console.WriteLine(Strings.badFormatTryAgain);
			}

			_newTask.Deadline = deadline;

			EnterTaskTags();
		}

		private static void EnterTaskTags()
		{
			Console.Clear();
			Console.WriteLine(Strings.enterTagsSpaceSeparated);
			Console.ReadLine().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(t => _newTask.AddTag(t));

			_todoList.AddTask(_newTask);

			WriteMainView();
		}

		private static void WriteLastTasks()
		{
			var tasks = _todoList.Last();

			WriteTasks(tasks);
		}

		private static void WriteSearchedTasks()
		{
			Console.Clear();

			Console.WriteLine(Strings.enterTagsSpaceSeparated);
			var tags = Console.ReadLine().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
			var tasks = _todoList.Search(tags);

			for(var i = 0; i < Console.LargestWindowWidth / 2; i++)
				Console.Write("_");

			Console.WriteLine();
			Console.WriteLine();

			WriteTasks(tasks);
		}

		private static void WriteTasks(IEnumerable<Task> tasks)
		{
			Console.Clear();

			if(tasks.Count() != 0)
			{
				foreach(var task in tasks)
				{
					Console.WriteLine(Strings.title, task.Title);
					Console.WriteLine(Strings.description, task.Description);
					Console.WriteLine(Strings.deadline, task.Deadline.ToShortDateString());
					Console.WriteLine(Strings.tags, String.Join(", ", task.Tags));
					Console.WriteLine();
				}
			}
			else
			{
				Console.WriteLine(Strings.nullResult);
			}

			Console.WriteLine(Strings.pressAnyKeyToContinue);
			Console.ReadKey();
			WriteMainView();
		}
	}
}
