using System.Collections.Concurrent;
using System.Text;
using XiangqiCore.Game;
using XiangqiOpponentGameScraper.Dtos;
using static XiangqiOpponentGameScraper.Services.Logger;

namespace XiangqiOpponentGameScraper.Services;

public class CreatingPgnService
{
	private readonly SemaphoreSlim _semaphore = new(10, 10);
	private readonly XiangqiBuilder _xiangqiBuilder = new();
	private readonly BlockingCollection<GameRecordDto> _gameRecords;

	private string _targetDirectory { get; set; }
	private string _folderName { get; set; }

	private const string PGN_EXTENSION = ".pgn";
	private const string TXT_EXTENSION = ".txt";
	private string FilePathPrefix => Path.Combine(_targetDirectory, _folderName);

	internal CreatingPgnService(BlockingCollection<GameRecordDto> gameRecords, string targetDirectory, string folderName)
	{
		_gameRecords = gameRecords;
		_targetDirectory = targetDirectory;
		_folderName = folderName;
	}

	public async Task ProcessGameRecordsAsync()
	{
		Directory.CreateDirectory(FilePathPrefix);
		int totalGames = 0;

		using ProgressBar progressBar = new();

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

		LogColor($"Finished writing {totalGames} game records!");
	}

	private async Task ProcessBatchAsync(List<GameRecordDto> batch)
	{
		await Parallel.ForEachAsync(batch, new ParallelOptions { MaxDegreeOfParallelism = 10 }, async (gameRecord, cancellationToken) =>
		{
			string fileName = $"{gameRecord.GameName}{PGN_EXTENSION}";
			byte[] gameRecordBytes;

			Encoding gb2312Encoding = CodePagesEncodingProvider.Instance.GetEncoding(936) ?? Encoding.UTF8;

			await _semaphore.WaitAsync(cancellationToken);

			try
			{
				try
				{
					XiangqiGame game = _xiangqiBuilder
						.WithDpxqGameRecord(gameRecord.GameRecord)
						.Build();

					gameRecordBytes = gb2312Encoding.GetBytes(game.ExportGameAsPgnString());
				}
				catch (Exception ex)
				{
					fileName = $"{gameRecord.GameName}{TXT_EXTENSION}";

					LogError($"Error creating pgn file for {gameRecord.GameName}: {ex.Message}");
					Log("Writing the DPXQ game record to a txt file instead...");

					gameRecordBytes = gb2312Encoding.GetBytes(gameRecord.GameRecord);
				}

				string filePath = Path.Combine(FilePathPrefix, fileName);
				await File.WriteAllBytesAsync(filePath, gameRecordBytes, cancellationToken);
			}
			finally
			{
				_semaphore.Release();
			}
		});
	}
}
