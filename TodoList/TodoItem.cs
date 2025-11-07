namespace TodoList;

public class TodoItem
{
	public TodoItem(string text)
	{
		Text = text;
		IsDone = false;
		LastUpdate = DateTime.Now;
	}

	public string Text { get; private set; }
	public bool IsDone { get; private set; }
	public DateTime LastUpdate { get; private set; }

	public void MarkDone()
	{
		IsDone = true;
		LastUpdate = DateTime.Now;
	}

	public void UpdateText(string newText)
	{
		Text = newText;
		LastUpdate = DateTime.Now;
	}

	public string GetShortInfo()
	{
		var preview = Text.Length <= 30 ? Text : Text.Substring(0, 27) + "...";
		var status = IsDone ? "Выполнена" : "Не выполнена";
		return $"{preview} | {status} | {LastUpdate:dd.MM.yyyy HH:mm}";
	}

	public string GetFullInfo()
	{
		return
			$"Текст: {Text}\nСтатус: {(IsDone ? "Выполнена" : "Не выполнена")}\nДата изменения: {LastUpdate:dd.MM.yyyy HH:mm}";
	}
}