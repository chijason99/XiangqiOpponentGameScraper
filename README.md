# XiangqiOpponentGameScraper
This program scrapes Xiangqi (Chinese Chess) game records for a specified player from dpxq.com and saves them as PGN files. If a game record cannot be parsed successfully, it will be saved as a TXT file instead.

How to Use
1.	Download the Executable: Download the XiangqiOpponentGameScraper.exe file from the releases page.
2.	Run the Program: Double-click the XiangqiOpponentGameScraper.exe file to run the program.
3.	Enter Player Name: When prompted, enter the player name in  <strong>Simplified Chinese</strong>.
4.	Enter File Path: Enter the file path where you want to save the files.
5.	Enter Number of Games: Enter the number of games you want to scrape (between 100 and 500).
6.	Wait for Completion: The program will display a spinning progress bar while it processes the game records. Once completed, you will see a list of PGN files downloaded.
   
Example
```bash
Enter the player name: 王天一
Enter the file path to save the files: C:\Users\YourName\Documents\XiangqiGames
Enter the number of games to scrape (100-500): 50
```

Output
•	PGN Files: Successfully parsed game records will be saved as .pgn files in the specified directory.
•	TXT Files: Game records that cannot be parsed successfully will be saved as .txt files in the specified directory.
Notes
•	Ensure you have a stable internet connection while running the program.
•	The program may take some time to complete depending on the number of games being scraped.
License
This project is licensed under the MIT License. See the LICENSE file for details.
Contributing
Contributions are welcome! Please open an issue or submit a pull request for any improvements or bug fixes.
Contact
For any questions or support, please open an issue on the GitHub repository.
---
