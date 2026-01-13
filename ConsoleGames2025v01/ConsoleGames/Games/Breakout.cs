using System;
using System.Collections.Generic;
using System.Drawing;

namespace ConsoleGames.Games
{
    public class Breakout : Game
    {
        public override string Name => "Breakout";
        public override string Description => "Destroy all blocks with a bouncing ball.";
        public override string Rules => "Move the paddle left and right. Do not let the ball fall down.";
        public override string Credits => "Darvin Subramaniam";
        public override int Year => 2026;
        public override int LevelMax => 4;
        public override bool TheHigherTheBetter => true;
        public override Score HighScore { get; set; }

        private List<Point> blockPositions;      // Position jedes Blocks
        private List<Size> blockSizes;           // Größe jedes Blocks
        private List<ConsoleColor> blockColors;  // Farbe jedes Blocks

        private Point playerPosition;            // Paddle Position
        private Size playerSize;                 // Paddle Größe
        private Point ballPosition;              // Ball Position
        private Point ballDirection;             // Ball Bewegungsrichtung

        private int points;                      // Aktuelle Punktzahl
        private int speed;                       // Spielgeschwindigkeit

        private int fieldWidth;                  // Breite des Spielfelds
        private int fieldHeight;                 // Höhe des Spielfelds

        private bool gameOver;                   // Spiel beendet
        private bool levelCompleted;             // Level abgeschlossen

        public override Score Play(int level)
        {
            Score score = new Score();
            score.LevelCompleted = false;

            PrepareLevel(level);

            while (true)
            {
                HandleInput();

                MoveBall();

                CheckCollisions();

                DrawGame();

                if (IsGameOver())
                    break;

                if (IsLevelCompleted())
                    break;

                System.Threading.Thread.Sleep(50);
            }

            return CreateScore(level);
        }

        private void PrepareLevel(int level)
        {
            // Spielfeldgröße anpassen
            fieldWidth = Console.WindowWidth;
            fieldHeight = Console.WindowHeight;

            // Punkte und Status zurücksetzen
            points = 0;
            gameOver = false;
            levelCompleted = false;

            // Blöcke 
            blockPositions = new List<Point>();
            blockSizes = new List<Size>();
            blockColors = new List<ConsoleColor>();

            // Player, Ball und Blöcke erzeugen
            GeneratePlayer();
            GenerateBall();
            GenerateBlocks(level);

            // Geschwindigkeit nach Level setzen
            SetSpeedByLevel(level);
        }

        private void GenerateBlocks(int level)
        {
            // TODO: Blöcke zufällig erzeugen
        }

        private void GeneratePlayer()
        {
            // Paddle erzeugen
            playerSize = new Size(15, 1);
            playerPosition = new Point(fieldWidth / 2 - playerSize.Width / 2, fieldHeight - 2);
        }

        private void GenerateBall()
        {
            // Ball Startposition und Richtung setzen
        }

        private void SetSpeedByLevel(int level)
        {
            // Geschwindigkeit an Level anpassen
        }

        private void MoveBall()
        {
            //Ballbewegung berechnen
        }

        private void CheckCollisions()
        {
            //Kollisionen mit Paddle, Wänden und Blöcken prüfen
        }

        private bool IsGameOver()
        {
            //Prüfen, ob Ball unten raus gefallen ist
            return false;
        }

        private bool IsLevelCompleted()
        {
            //Prüfen, ob alle Blöcke zerstört sind
            return false;
        }

        private Score CreateScore(int level)
        {
            //Score-Objekt erstellen und zurückgeben
            return null;
        }

        private void HandleInput()
        {
            // Paddle Bewegung ohne Verzögerung
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.LeftArrow)
                {
                    playerPosition.X -= 2; // schneller bewegen
                    if (playerPosition.X < 0) playerPosition.X = 0;
                }
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    playerPosition.X += 2; // schneller bewegen
                    if (playerPosition.X + playerSize.Width > fieldWidth)
                        playerPosition.X = fieldWidth - playerSize.Width;
                }
            }
        }


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
            // Blöcke zeichnen
        }

        private void DrawPlayer()
        {
            // Paddle zeichnen
            if (playerPosition.X + playerSize.Width >= fieldWidth)
            {
                playerPosition.X = fieldWidth - playerSize.Width;
            }

            Console.SetCursorPosition(playerPosition.X, playerPosition.Y);
            Console.Write(new string('■', playerSize.Width));
        }

        private void DrawBall()
        {
            //Ball zeichnen
        }

        private void DrawScore()
        {
            //Punkte und Levelanzeige
        }
    }
}
