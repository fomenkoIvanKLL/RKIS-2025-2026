namespace TodoList;

public class TodoList
{
	public TodoItem[] items;

	public TodoList(int capacity = 10)
	{
		items = new TodoItem[capacity];
		Count = 0;
	}

	public int Count { get; private set; }

	public void Add(TodoItem item)
	{
		if (Count >= items.Length) IncreaseArray(items, item);
		items[Count++] = item;
	}

	public void Delete(int index)
	{
		if (index < 0 || index >= Count)
			throw new ArgumentOutOfRangeException("Неверный индекс задачи");

		for (var i = index; i < Count - 1; i++) items[i] = items[i + 1];
		Count--;
	}

	public void View(bool showIndex, bool showStatus, bool showDate)
	{

		var header = "";
		if (showIndex) header += "№".PadRight(6);
		if (showStatus) header += "Статус".PadRight(10);
		if (showDate) header += "Дата".PadRight(16);
		header += "Задача";

		Console.WriteLine(header);
		Console.WriteLine(new string('-', header.Length));

		for (var i = 0; i < Count; i++)
		{
			var line = "";
			if (showIndex) line += $"{i + 1}".PadRight(6);
			if (showStatus) line += $"{items[i].Status}".PadRight(10);
			if (showDate) line += $"{items[i].LastUpdate:dd.MM.yyyy HH:mm}".PadRight(16);

			var preview = items[i].Text.Length <= 30 ? items[i].Text : items[i].Text.Substring(0, 27) + "...";
			line += preview;

			Console.WriteLine(line);
		}
	}

	public TodoItem GetItem(int index)
	{
		if (index < 0 || index >= Count)
			throw new ArgumentOutOfRangeException("Неверный индекс задачи");
		return items[index];
	}

	private void IncreaseArray(TodoItem[] currentItems, TodoItem newItem)
	{
		var newItems = new TodoItem[currentItems.Length * 2];
		Array.Copy(currentItems, newItems, Count);
		items = newItems;
	}
}