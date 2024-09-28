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
