using Microsoft.Playwright;
using System.Collections.Concurrent;
using XiangqiOpponentGameScraper.Dtos;
using static XiangqiOpponentGameScraper.Services.Logger;

namespace XiangqiOpponentGameScraper.Services;

internal class GameScrapingService
{
	private const string DPXQ_URL = "http://www.dpxq.com/hldcg/search";

	public GameScrapingService(string playerName, BlockingCollection<GameRecordDto> gameRecords, int targetNumberOfGames = 50)
    {
		ArgumentException.ThrowIfNullOrWhiteSpace(playerName, nameof(playerName));

		TargetNumberOfGames = targetNumberOfGames;
		PlayerName = playerName;
		_gameRecords = gameRecords;
	}

	public int TotalNumberOfRecords { get; set; } = 0;
	public int TargetNumberOfGames { get; init; }
    public string PlayerName { get; set; }

	private readonly BlockingCollection<GameRecordDto> _gameRecords;
    public async Task ScrapeGamesAsync()
	{
		using IPlaywright playwright = await Playwright.CreateAsync();

		await using IBrowser browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
		{
			Headless = true,
		});

		Log("Launching browser...");

		IPage page = await browser.NewPageAsync();

		await page.GotoAsync(DPXQ_URL);

		Log("Going to dpxq.com...");

		await page
			.GetByRole(AriaRole.Link, new() { Name = "高级搜索" })
			.Nth(1)
			.ClickAsync();

		await page.FrameLocator("iframe[name=\"search_diy\"]").Locator("input[name=\"red\"]").ClickAsync();
		await page.FrameLocator("iframe[name=\"search_diy\"]").Locator("input[name=\"red\"]").FillAsync(PlayerName);
		await page.FrameLocator("iframe[name=\"search_diy\"]").Locator("input[name=\"black\"]").ClickAsync();
		await page.FrameLocator("iframe[name=\"search_diy\"]").Locator("input[name=\"black\"]").FillAsync(PlayerName);

		await page.FrameLocator("iframe[name=\"search_diy\"]")
			.GetByRole(AriaRole.Button, new() { Name = "搜索" })
			.ClickAsync();

		Log($"Searching for games from {PlayerName}...");

		await page.WaitForTimeoutAsync(500);

		bool isLastGameRecord = false;

		while (!isLastGameRecord)
		{
			var gameRows = await page.FrameLocator("iframe[name=\"search_end_pos\"]")
				.GetByRole(AriaRole.Link, new()
				{
					Name = PlayerName
				})
				.AllAsync();

			if (gameRows.Count == 0)
			{
				isLastGameRecord = true;
				break;
			}

			foreach (var gameLocation in gameRows)
			{
				await gameLocation.ClickAsync();

				string gameName = $"Game_{TotalNumberOfRecords + 1}_{await gameLocation.InnerTextAsync()}";

				LogColor($"{gameName} found...");

				foreach (char invalidChar in Path.GetInvalidFileNameChars())
				{
					gameName = gameName.Replace(invalidChar, '_');
				}

				await page.WaitForTimeoutAsync(300);

				await page.FrameLocator("iframe[name=\"name_dhtmlxq_search_view\"]").GetByRole(AriaRole.Button, new() { Name = "导出" }).ClickAsync();
				await page.FrameLocator("iframe[name=\"name_dhtmlxq_search_view\"]").GetByRole(AriaRole.Link, new() { Name = "文本" }).Last.ClickAsync();
				await page.FrameLocator("iframe[name=\"name_dhtmlxq_search_view\"]").GetByRole(AriaRole.Link, new() { Name = "◎简单文本" }).ClickAsync();
				await page.FrameLocator("iframe[name=\"name_dhtmlxq_search_view\"]").GetByRole(AriaRole.Link, new() { Name = "确认导出" }).ClickAsync();
				await page.FrameLocator("iframe[name=\"name_dhtmlxq_search_view\"]").GetByRole(AriaRole.Button, new() { Name = "确 定" }).ClickAsync();

				string gameRecord = await page.FrameLocator("iframe[name=\"search_end_pos\"]")
					.Locator("#imgs")
					.InnerTextAsync();

				_gameRecords.Add(new GameRecordDto(gameName, gameRecord));

				//Interlocked.Increment(ref TotalNumberOfRecords);
				TotalNumberOfRecords++;

				if (TotalNumberOfRecords >= TargetNumberOfGames)
					break;
			}

			await page.FrameLocator("iframe[name=\"search_end_pos\"]").GetByRole(AriaRole.Link, new() { Name = ">", Exact = true }).ClickAsync();
			await page.WaitForTimeoutAsync(300);

			if (TotalNumberOfRecords >= TargetNumberOfGames)
				break;
		}

		_gameRecords.CompleteAdding();

		Log($"Total number of records found: {TotalNumberOfRecords}");

		await page.CloseAsync();
	}
}
