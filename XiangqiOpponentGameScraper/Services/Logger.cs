﻿namespace XiangqiOpponentGameScraper.Services;

// Credit to https://github.com/nilaoda/BBDown/blob/master/BBDown.Core/Logger.cs
public static class Logger
{
	public static void Log(object text, bool enter = true)
	{
		Console.Write(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss.fff]") + " - " + text);
		if (enter) Console.WriteLine();
	}

	public static void LogError(object text)
	{
		Console.Write(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss.fff]") + " - ");
		Console.ForegroundColor = ConsoleColor.Red;
		Console.Write(text);
		Console.ResetColor();
		Console.WriteLine();
	}

	public static void LogColor(object text, bool time = true)
	{
		if (time)
			Console.Write(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss.fff]") + " - ");

		Console.ForegroundColor = ConsoleColor.Cyan;
		
		if (time)
			Console.Write(text);
		else
			Console.Write("                            " + text);

		Console.ResetColor();
		Console.WriteLine();
	}

	public static void LogWarn(object text, bool time = true)
	{
		if (time)
			Console.Write(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss.fff]") + " - ");

		Console.ForegroundColor = ConsoleColor.DarkYellow;
		
		if (time)
			Console.Write(text);
		else
			Console.Write("                            " + text);
		
		Console.ResetColor();
		Console.WriteLine();
	}
}
