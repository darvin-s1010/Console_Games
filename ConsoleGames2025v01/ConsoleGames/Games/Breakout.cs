using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace ConsoleGames.Games
{
    public class Breakout : Game
    {
        // PUBLIC PROPERTIES
        public override string Name => "Breakout";
        public override string Description => "Destroy all blocks with a bouncing ball.";
        public override string Rules => "Move paddle with arrow keys. Do not let the ball fall down.";
        public override string Credits => "Darvin Subramaniam";
        public override int Year => 2026;
        public override int LevelMax => 4;
        public override bool TheHigherTheBetter => true;
        public override Score HighScore { get; set; }

        // PRIVATE FIELDS
        private List<Point> blockPositions;
        private List<Size> blockSizes;
        private List<ConsoleColor> blockColors;

        private Point playerPosition;
        private Size playerSize;

        private Point ballPosition;
        private Point ballDirection;
        private Size ballSize;

        private int points;
        private int speed;
        private int lives;

        private int fieldWidth;
        private int fieldHeight;

        private bool gameOver;
        private bool levelCompleted;

        // GAME LOOP
        public override Score Play(int level)
        {
            PrepareLevel(level);

            while (!gameOver && !levelCompleted)
            {
                HandleInput();
                MoveBall();
                CheckCollisions();
                DrawGame();
                Thread.Sleep(speed);
            }

            return CreateScore(level);
        }

        // SETUP
        private void PrepareLevel(int level)
        {
            Console.CursorVisible = false;

            fieldWidth = Console.WindowWidth;
            fieldHeight = Console.WindowHeight;

            points = 0;
            lives = 3;
            gameOver = false;
            levelCompleted = false;

            blockPositions = new List<Point>();
            blockSizes = new List<Size>();
            blockColors = new List<ConsoleColor>();

            GeneratePlayer();
            GenerateBall();
            GenerateBlocks(level);
            SetSpeedByLevel(level);
        }

        private void GeneratePlayer()
        {
            playerSize = new Size(16, 1);
            playerPosition = new Point(
                fieldWidth / 2 - playerSize.Width / 2,
                fieldHeight - 3
            );
        }

        private void GenerateBall()
        {
            ballSize = new Size(1, 1);
            ballPosition = new Point(
                playerPosition.X + playerSize.Width / 2,
                playerPosition.Y - 1
            );
            ballDirection = new Point(1, -1);
        }

        private void SetSpeedByLevel(int level)
        {
            speed = Math.Max(20, 80 - level * 15);
        }

        // BLOCKS
        private void GenerateBlocks(int level)
        {
            Random rnd = new Random();
            int y = 2;

            while (y < fieldHeight / 3)
            {
                int x = 2;

                while (x < fieldWidth - 10)
                {
                    int width = rnd.Next(6, 12);
                    blockPositions.Add(new Point(x, y));
                    blockSizes.Add(new Size(width, 1));
                    blockColors.Add((ConsoleColor)rnd.Next(1, 15));
                    x += width + 3;
                }
                y += 2;
            }
        }

        // GAME LOGIC
        private void MoveBall()
        {
            ballPosition.X += ballDirection.X;
            ballPosition.Y += ballDirection.Y;
        }

        private void CheckCollisions()
        {
            // Seitenwände
            if (ballPosition.X <= 0 || ballPosition.X >= fieldWidth - 1)
                ballDirection.X *= -1;

            // Decke
            if (ballPosition.Y <= 0)
                ballDirection.Y *= -1;

            Rectangle ball = new Rectangle(ballPosition, ballSize);
            Rectangle paddle = new Rectangle(playerPosition, playerSize);

            // Paddle
            if (ball.IntersectsWith(paddle) && ballDirection.Y > 0)
            {
                ballDirection.Y = -1;
            }

            // Boden
            if (ballPosition.Y >= fieldHeight - 1)
            {
                lives--;
                if (lives <= 0)
                {
                    gameOver = true;
                    return;
                }
                GeneratePlayer();
                GenerateBall();
                return;
            }

            // Blöcke
            for (int i = blockPositions.Count - 1; i >= 0; i--)
            {
                Rectangle block = new Rectangle(blockPositions[i], blockSizes[i]);
                if (ball.IntersectsWith(block))
                {
                    blockPositions.RemoveAt(i);
                    blockSizes.RemoveAt(i);
                    blockColors.RemoveAt(i);
                    ballDirection.Y *= -1;
                    points += 10;
                    break;
                }
            }

            if (blockPositions.Count == 0)
                levelCompleted = true;
        }

        private Score CreateScore(int level)
        {
            return new Score
            {
                GameName = Name,
                Level = level,
                Points = points,
                LevelCompleted = levelCompleted
            };
        }

        // INPUT
        private void HandleInput()
        {
            while (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.LeftArrow)
                    playerPosition.X = Math.Max(0, playerPosition.X - 3);
                if (key == ConsoleKey.RightArrow)
                    playerPosition.X = Math.Min(fieldWidth - playerSize.Width, playerPosition.X + 3);
            }
        }

        // DRAWING
        private void DrawGame()
        {
            Console.Clear();
            DrawBlocks();
            DrawPlayer();
            DrawBall();
            DrawScore();
        }

        private void DrawBlocks()
        {
            for (int i = 0; i < blockPositions.Count; i++)
            {
                Console.ForegroundColor = blockColors[i];
                Console.SetCursorPosition(blockPositions[i].X, blockPositions[i].Y);
                Console.Write(new string('█', blockSizes[i].Width));
            }
            Console.ResetColor();
        }

        private void DrawPlayer()
        {
            Console.SetCursorPosition(playerPosition.X, playerPosition.Y);
            Console.Write(new string('■', playerSize.Width));
        }

        private void DrawBall()
        {
            Console.SetCursorPosition(ballPosition.X, ballPosition.Y);
            Console.Write("⬤");
        }

        private void DrawScore()
        {
            Console.SetCursorPosition(2, fieldHeight - 1);
            Console.Write($"Punkte: {points}   Leben: {lives}");
        }
    }
}
