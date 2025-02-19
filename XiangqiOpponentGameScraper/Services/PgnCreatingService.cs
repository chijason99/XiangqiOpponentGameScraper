using System.Collections.Concurrent;
using XiangqiCore.Game;
using XiangqiCore.Move;
using XiangqiCore.Services.PgnSaving;
using XiangqiOpponentGameScraper.Dtos;
using static XiangqiOpponentGameScraper.Services.Logger;

namespace XiangqiOpponentGameScraper.Services;

public class CreatingPgnService
{
	private readonly SemaphoreSlim _semaphore = new(10, 10);
	private readonly BlockingCollection<GameRecordDto> _gameRecords;
	private readonly GameScrapingService _gameScrapingService;
	private readonly IPgnSavingService _pgnSavingService;
	private ConsoleSpinner _spinner;

	private string _targetDirectory { get; set; }
	private string _folderName { get; set; }
	private MoveNotationType _moveNotationType { get; set; }

	private const string PGN_EXTENSION = ".pgn";
	private string FilePathPrefix => Path.Combine(_targetDirectory, _folderName);

	internal CreatingPgnService(
		GameScrapingService gameScrapingService,
		BlockingCollection<GameRecordDto> gameRecords,
		IPgnSavingService pgnSavingService,
		string targetDirectory,
		string folderName,
		MoveNotationType moveNotationType)
	{
		_gameScrapingService = gameScrapingService;
		_pgnSavingService = pgnSavingService;

		gameScrapingService.ScrapeCompleted += StartSpinner;

		_gameRecords = gameRecords;
		_targetDirectory = targetDirectory;
		_folderName = folderName;
		_moveNotationType = moveNotationType;
	}

	public async Task ProcessGameRecordsAsync()
	{
		Directory.CreateDirectory(FilePathPrefix);
		int totalGames = 0;

		List<Task> tasks = [];

		while (!_gameRecords.IsCompleted)
		{
			List<GameRecordDto> batch = [];

			// Take up to 10 records from the collection
			for (int i = 0; i < 10 && _gameRecords.TryTake(out GameRecordDto? gameRecord); i++)
			{
				batch.Add(gameRecord);
			}

			if (batch.Count > 0)
			{
				totalGames += batch.Count;
				tasks.Add(ProcessBatchAsync(batch));
			}
		}

		await Task.WhenAll(tasks);

		StopSpinner();
		CleanUpEvents();

		LogColor($"Finished writing {totalGames} game records!");
	}

	private async Task ProcessBatchAsync(List<GameRecordDto> batch)
	{
		await Parallel.ForEachAsync(batch, new ParallelOptions { MaxDegreeOfParallelism = 10 }, async (gameRecord, cancellationToken) =>
		{
			string fileName = $"{gameRecord.GameName}{PGN_EXTENSION}";
			
			try
			{
				XiangqiBuilder xiangqiBuilder = new();

				XiangqiGame game = xiangqiBuilder
					.WithDpxqGameRecord(gameRecord.GameRecord)
					.Build();

				await _semaphore.WaitAsync(cancellationToken);

				string filePath = Path.Combine(FilePathPrefix, SanitizeFileName(fileName));

				await _pgnSavingService.SaveAsync(
					filePath, 
					game, 
					_moveNotationType, 
					cancellationToken);
			}
			catch (Exception ex)
			{
				LogError($"An unexpected error occurred while processing {fileName}");
			}
			finally
			{
				_semaphore.Release();
			}
		});
	}

	private string SanitizeFileName(string fileName)
	{
		foreach (char c in Path.GetInvalidFileNameChars())
		{
			fileName = fileName.Replace(c, '_');
		}

		return fileName;
	}

	public void StartSpinner()
	{
		_spinner = new ConsoleSpinner();
	}

	public void StopSpinner()
	{
		_spinner.Dispose();
	}

	public void CleanUpEvents()
	{
		_gameScrapingService.ScrapeCompleted -= StartSpinner;
	}
}
