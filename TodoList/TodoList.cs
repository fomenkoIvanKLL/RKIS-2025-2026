using System;

namespace TodoList
{
    public class TodoList
    {
        private TodoItem[] items;
        private int count;

        public TodoList(int capacity = 10)
        {
            items = new TodoItem[capacity];
            count = 0;
        }

        public void Add(TodoItem item)
        {
            if (count >= items.Length)
            {
                IncreaseArray();
            }
            items[count++] = item;
        }

        public void Delete(int index)
        {
            if (index < 0 || index >= count)
                throw new ArgumentOutOfRangeException("Неверный индекс задачи");

            for (int i = index; i < count - 1; i++)
            {
                items[i] = items[i + 1];
            }
            count--;
        }

        public void View(bool showIndex, bool showDone, bool showDate)
        {
            if (count == 0)
            {
                Console.WriteLine("Список задач пуст");
                return;
            }

            string header = "";
            if (showIndex) header += "№".PadRight(6);
            if (showDone) header += "Статус".PadRight(10);
            if (showDate) header += "Дата".PadRight(16);
            header += "Задача";
            
            Console.WriteLine(header);
            Console.WriteLine(new string('-', header.Length));

            for (int i = 0; i < count; i++)
            {
                if (!showDone && items[i].IsDone)
                    continue;

                string line = "";
                if (showIndex) line += $"{i + 1}".PadRight(6);
                if (showDone) line += $"{(items[i].IsDone ? "Сделано" : "Не сд.")}".PadRight(10);
                if (showDate) line += $"{items[i].LastUpdate:dd.MM.yyyy HH:mm}".PadRight(16);
                
                string preview = items[i].Text.Length <= 30 ? 
                    items[i].Text : items[i].Text.Substring(0, 27) + "...";
                line += preview;
                
                Console.WriteLine(line);
            }
        }

        public TodoItem GetItem(int index)
        {
            if (index < 0 || index >= count)
                throw new ArgumentOutOfRangeException("Неверный индекс задачи");
            return items[index];
        }

        public int Count => count;

        private void IncreaseArray()
        {
            TodoItem[] newItems = new TodoItem[items.Length * 2];
            Array.Copy(items, newItems, count);
            items = newItems;
        }
    }
}