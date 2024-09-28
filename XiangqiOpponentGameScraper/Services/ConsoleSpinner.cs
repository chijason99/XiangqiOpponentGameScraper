namespace XiangqiOpponentGameScraper.Services;

public class ConsoleSpinner : IDisposable
{
	private const string animation = @"|/-\";
	private readonly Timer timer;
	private int animationIndex = 0;
	private bool disposed = false;

	public ConsoleSpinner()
	{
		timer = new Timer(TimerHandler);
		ResetTimer();
	}

	private void TimerHandler(object? state)
	{
		lock (timer)
		{
			if (disposed) return;

			Console.Write(animation[animationIndex++ % animation.Length]);
			Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);

			ResetTimer();
		}
	}

	private void ResetTimer()
	{
		timer.Change(TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(-1));
	}

	public void Dispose()
	{
		lock (timer)
		{
			disposed = true;
			Console.Write(' ');
			Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
		}
	}
}
