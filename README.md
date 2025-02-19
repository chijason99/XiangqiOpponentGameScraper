# 東萍象棋棋譜爬虫
本程序用于从 dpxq.com 爬取指定玩家的象棋（中国象棋）对局记录，并将其保存为 PGN 文件。如果某个对局记录无法成功解析，则会将其保存为 TXT 文件。

## 使用方法
1. 下载exe檔：从发布页面下载 XiangqiOpponentGameScraper.exe 文件。目前该 .exe 文件仅支持 Windows x64 系统。
2. 运行程序：双击 XiangqiOpponentGameScraper.exe 文件运行程序。
3. 安装 Chromium（如需）：如果你的计算机上尚未安装 Playwright 的 Chromium 浏览器，程序会先尝试安装它。
4. 输入棋手名称：在提示时输入你想下戴棋手的名称，必须使用简体中文。
5. 输入文件路径：输入要保存文件的目录路径。
6. 输入对局数量：输入要爬取的对局数量（范围：1 到 500）。
7. 等待完成：程序会显示旋转进度条以指示爬取过程。完成后，你将看到下载的 PGN 文件列表。

## 示例
![XiangqiOpponentGameScrapperDemo-ezgif com-video-to-gif-converter](https://github.com/user-attachments/assets/482077f8-5e95-4c0e-98d4-dc2871c15655)

## 输出结果
- PGN 文件：成功解析的对局记录将保存为 .pgn 文件，存储在指定的目录中。
- TXT 文件：无法成功解析的对局记录将保存为 .txt 文件，存储在指定的目录中。

## 注意事项
- 运行程序时，请确保网络连接稳定。
- 程序运行时间可能较长，具体取决于爬取的对局数量。
- 确保计算机支持显示简体中文，否则可能无法正确显示和打开对局记录。在 Windows 11 中，你可以通过以下方式设置：
   - 设置 -> 时间和语言 -> 语言和区域 -> 管理语言设置，然后将 非 Unicode 程序的语言 更改为 中文（简体）。

## 许可证
本项目采用 MIT 许可证，详细信息请参阅 LICENSE 文件。

## 贡献
欢迎贡献代码！如果你有任何改进建议或发现了 bug，请提交 issue 或 pull request。

## 联系方式
如有任何问题或需要支持，请在 GitHub 仓库中提交 issue。

# XiangqiOpponentGameScraper
This program scrapes Xiangqi (Chinese Chess) game records for a specified player from dpxq.com and saves them as PGN files. If a game record cannot be parsed successfully, it will be saved as a TXT file instead.

## How to Use
1.	Download the Executable: Download the XiangqiOpponentGameScraper.exe file from the releases page. Currently the .exe file only supports windows x64 system.
2.	Run the Program: Double-click the XiangqiOpponentGameScraper.exe file to run the program.
3.	The program will first attempt to install the Chromium browser for Playwright if it is not already installed on your computer.
4.	Enter Player Name: When prompted, enter the player name in  <strong>Simplified Chinese</strong>.
5.	Enter File Path: Enter the file path where you want to save the files.
6.	Enter Number of Games: Enter the number of games you want to scrape (between 100 and 500).
7.	Wait for Completion: The program will display a spinning progress bar while it processes the game records. Once completed, you will see a list of PGN files downloaded.
   
## Example
![XiangqiOpponentGameScrapperDemo-ezgif com-video-to-gif-converter](https://github.com/user-attachments/assets/482077f8-5e95-4c0e-98d4-dc2871c15655)

## Output
- PGN Files: Successfully parsed game records will be saved as .pgn files in the specified directory.
- TXT Files: Game records that cannot be parsed successfully will be saved as .txt files in the specified directory.

## Notes
- Ensure you have a stable internet connection while running the program.
- The program may take some time to complete depending on the number of games being scraped.
- Please make sure your computer supports displaying Simplified Chinese in order to display and open the game records correctly. You can do so in windows 11 by going to Settings -> Time & Language -> Language & Region -> Administrative Language Settings, and changing the Langauge for non-Unicode programs to Chinese Simplified

  ![image](https://github.com/user-attachments/assets/49cc6516-d0d4-44d3-90a0-ec2d92a97582)

## License
This project is licensed under the MIT License. See the LICENSE file for details.

## Contributing
Contributions are welcome! Please open an issue or submit a pull request for any improvements or bug fixes.

## Contact
For any questions or support, please open an issue on the GitHub repository.
