using System;
using System.Collections.Generic;

namespace Work6
{
	public class Task
	{
		public Task(string title)
		{
			Title = title;
			_tags = new List<string>();
		}

		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime Deadline { get; set; }
		private readonly List<string> _tags;
		public IEnumerable<string> Tags { get { return _tags; } }

		public void AddTag(string tag)
		{
			_tags.Add(tag);
		}

		public void RemoveTag(string tag)
		{
			_tags.Remove(tag);
		}
	}
}
