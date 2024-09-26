using System.Collections.Concurrent;
using System.Text;
using XiangqiOpponentGameScraper.Dtos;
using XiangqiOpponentGameScraper.Services;
using static XiangqiOpponentGameScraper.Services.Logger;

// Set the console encoding to UTF-8
Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

try
{
	string playerName = GetPlayerName();
	string downloadPath = GetDownloadPath();
	int targetNumberOfGames = GetTargetNumberOfGames();

	BlockingCollection<GameRecordDto> gameRecords = [];
	string folderName = $"{playerName}_game_records_{DateTime.Now.Ticks}";

	GameScrapingService gameScrapingService = new(playerName, gameRecords, targetNumberOfGames);
	CreatingPgnService creatingPgnService = new(gameRecords, downloadPath, folderName);

	Task scrappingTask = Task.Run(gameScrapingService.ScrapeGamesAsync);
	Task creatingPgnFileTask = Task.Run(creatingPgnService.ProcessGameRecordsAsync);

	await Task.WhenAll(scrappingTask, creatingPgnFileTask);

	Log("Finished!");
	PromptExit();

}
catch (Exception ex)
{
	LogError($"An unexpected error occurred: {ex.Message}");
	PromptExit(true);
}

string GetPlayerName()
{
	Console.WriteLine("Enter the player name (in Simplified Chinese): ");

	string playerName = Console.ReadLine().Trim();

	if (string.IsNullOrWhiteSpace(playerName))
	{
		Console.WriteLine("Player name cannot be empty");

		PromptExit();
	}

	if (playerName.Length < 2)
	{
		Console.WriteLine("Player name must be at least 2 characters long");

		PromptExit();
	}

	Console.WriteLine();

	return playerName;
}

string GetDownloadPath()
{
	Console.WriteLine("Please enter the download path for the game records");
	string downloadPath = Console.ReadLine().Trim();

	if (string.IsNullOrWhiteSpace(downloadPath))
	{
		Console.WriteLine("Download path cannot be empty");

		PromptExit();
	}

	if (!Directory.Exists(downloadPath))
	{
		Console.WriteLine("Download path does not exist");

		PromptExit();
	}

	Console.WriteLine();
	return downloadPath;
}

int GetTargetNumberOfGames()
{
	Console.WriteLine("Please enter the target number of the games you wish to scrape (between 1 - 500):");
	string target = Console.ReadLine().Trim();
	const int DEFAULT_TARGET= 100;
	const int MAX_TARGET = 500;

	bool isNumber = int.TryParse(target, out int targetNumberOfGames );

	if (string.IsNullOrWhiteSpace(target) || 
		!isNumber || 
		targetNumberOfGames <= 0 || 
		targetNumberOfGames > MAX_TARGET)
	{
		Console.WriteLine("Invalid number provided. Will default to scrape 100 games.");

		return DEFAULT_TARGET;
	}

	Console.WriteLine();
	return targetNumberOfGames;
}

void PromptExit(bool autoExit = false)
{
	Log("Press any key to exit...");
	Console.ReadKey();

	if (autoExit)
		Environment.Exit(0);
}