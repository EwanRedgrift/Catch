/*
                       *                             *                    *                            *                          *            *
  *     _            _          _                    _             _               _           _                    _             _      
       / /\    *    /\*\       / /\    *         * /\ \         * /\ \     *      /\*\      * // \                /\*\     *     /\ \    *
  *   / / *\  *    / *\ \     / /  \              /  \ \         /  \ \          /  \ \      / /  \              /  \ \         /  \ \   
     / / /\ \__   / /\ \ \   / / /\ \       *    / /\ \ \       / /\ \ \        / /\ \ \    / / /\ \      *     / /\ \ \       / /\ \ \  
    / / /\ \___\ / / /\ \_\ / / /\ \ \          / / /\ \ \     / / /\ \_\      / / /\ \_\  / / /\ \ \          / / /\ \ \     / / /\ \_\ 
    \ \ \ \/___// / /_/ / // / / *\ \*\        / / /  \ \_\   / /_/_ \/_/*    / / /_/ / / / / /  \ \ \   *    / / /  \ \_\   / /_/_ \/_/ 
     \ \ \     / / /__\/ // / /___/ /\ \      / / /    \/_/  / /____/\       / / /__\/ / / / /___/ /\ \      / / /    \/_/  / /____/\    
 _    \ \*\   / / /_____// / /_____/ /\ \    / / /     *    / /\____\/      / / /_____/ / / /_____/ /\ \    / / /      *   / /\____\/    
/_/\__/ / /  / / /      / /_________/\ \ \  / / /________  / / /______     / / /\ \ \  / /_________/\ \ \  / / /________  / / /______    *
\ \/___/ /  / / /   *  / / /_       __\ \_\/ / /_________\/ / /_______\   / / /  \ \ \/ / /_       __\ \_\/ / /_________\/ / /_______\   
 \_____\/   \/_/       \_\___\  *  /____/_/\/____________/\/__________/   \/_/    \_\/\_\___\   * /____/_/\/___________/\/__________/   
                      *                           *                                               *
*

Space Race
By Ewan Redgrift
5/23/24

Game rules:
- A max count down of 10000 miliseconds per turn.
- Move player by clicking and dragging, if mouse button is not being held down, the player will not move.
- 2 turns in total.
- After player 1's turn, the hero will turn red and be sent back to the start, indicating the start of player 2's turn.

- Objective: obtain as many points as possible.
    - Gold meteorite's give players 3000 points
    - Normal meteorites removes 2000 points from players and send them back to their start point. 
    - Reaching the end causes all remaining time (in miliseconds) to be added with their permanent points and calculates a final score.
    - If time runs out, final socre is equal to players permanent points.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace Catch
{
    public partial class Form1 : Form
    {
        //Time indicator
        Rectangle timeIndicator = new Rectangle(0, 0, 600, 10);
        int timeIndicatorWidth = 600;

        //Hero variables
        Rectangle hero = new Rectangle(280, 540, 40, 10);
        int heroSpeed = 10;

        //Ball variables
        int ballSizeMax = 15;
        int ballSizeMin = 7;
        int ballSpeed = 8;

        //List of balls
        List<bool> leftOrRight = new List<bool>();
        List<Rectangle> ballList = new List<Rectangle>();
        List<int> ballSpeeds = new List<int>(); 
        List<int> ballSizeList = new List<int>();
        List<string> ballColours = new List<string>();

        //Turn stuff
        int time = 10000;
        int scoreBoost = 0;
        int p1FinalScore = 0;
        int p2FinalScore = 0;
        string turn = "p1";
        bool gameOver = false;
        bool gameStart = true;

        //Control
        bool mousePressed = false;

        //Brushes
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush grayBrush = new SolidBrush(Color.Gray);
        SolidBrush goldBrush = new SolidBrush(Color.Gold);
        SolidBrush blueBrush = new SolidBrush(Color.Blue);
        SolidBrush redBrush = new SolidBrush (Color.Red);

        //Random
        Random randGen = new Random();
        int randValue = 0;

        //Sound
        SoundPlayer boomSound = new SoundPlayer(Properties.Resources.Explosion);
        SoundPlayer rewardSound = new SoundPlayer(Properties.Resources.Coin);

        //random
        int freeZone = 75;
        public Form1()
        {
            InitializeComponent();
        }
        private void CreateNewBall(int y, string colour)
        {
            int ballSize = ballSizeList[ballSizeList.Count - 1];

            if (leftOrRight[leftOrRight.Count() - 1] == true)
            {
                Rectangle ball = new Rectangle(0, y, ballSize, ballSize);
                ballList.Add(ball);
                ballColours.Add(colour);
                ballSpeeds.Add(randGen.Next(5, 15));
            }
            else
            {
                Rectangle ball = new Rectangle(this.Width - ballSize, y, ballSize, ballSize);
                ballList.Add(ball);
                ballColours.Add(colour);
                ballSpeeds.Add(randGen.Next(5, 15));
            }
        }

        private void gameTime_Tick(object sender, EventArgs e)
        {
            //move balls down the screen
            for (int i = 0; i < ballList.Count(); i++)
            {
                int ballSize = ballSizeList[i];

                if (leftOrRight[i] == true) //Moving right
                {
                    //Get new x pos
                    int x = ballList[i].X + ballSpeeds[i];

                    //update the ball object
                    ballList[i] = new Rectangle(x, ballList[i].Y, ballSize, ballSize);
                }
                else // moving left
                {
                    //Get new x pos
                    int x = ballList[i].X - ballSpeeds[i];

                    //update the ball object
                    ballList[i] = new Rectangle(x, ballList[i].Y, ballSize, ballSize);
                }

            }

            //create new ball if it is time
            randValue = randGen.Next(0, 1000);

            if (randValue < 335)
            {
                //Choose ball's direction
                int randLeftOrRight = randGen.Next(1, 3);

                if (randLeftOrRight == 1)
                {
                    leftOrRight.Add(true);
                }
                else
                {
                    leftOrRight.Add(false);
                }

                int randBallSize = randGen.Next(ballSizeMin, ballSizeMax);
                ballSizeList.Add(randBallSize);

                int randY = randGen.Next(freeZone, this.Height - freeZone - randBallSize);

                if (randValue > 175)
                {
                    CreateNewBall(randY, "white");

                }
                else if (randValue < 175 && randValue > 20)
                {
                    CreateNewBall(randY, "gray");
                }
                else
                {
                    CreateNewBall(randY, "gold");
                }
            }

            //remove ball from list if it has gone off the screen
            for (int i = 0; i < ballList.Count(); i++)
            {
                if (ballList[i].X < 0 || ballList[i].X > this.Width)
                {
                    ballList.RemoveAt(i);
                    ballColours.RemoveAt(i);
                    ballSpeeds.RemoveAt(i);
                    leftOrRight.RemoveAt(i);
                    ballSizeList.RemoveAt(i);
                }
            }

            //check for collision between ball and player
            for (int i = 0; i < ballList.Count(); i++)
            {
                if (ballList[i].IntersectsWith(hero))
                {
                    if (ballColours[i] == "gold")
                    {
                        scoreBoost += 3000;
                        
                        rewardSound.Play();
                    }
                    else
                    {
                        hero.X = 280;
                        hero.Y = 540;
                        mousePressed = false;

                        scoreBoost -= 2000;

                        boomSound.Play();
                    }

                    ballList.RemoveAt(i);
                    ballColours.RemoveAt(i);
                    ballSpeeds.RemoveAt(i);
                    leftOrRight.RemoveAt(i);
                    ballSizeList.RemoveAt(i);
                }
            }

            //If player touches the top
            if (hero.Y == 0)
            {
                rewardSound.Play();

                if (turn == "p1")
                {
                    SwithToP2();
                }
                else
                {
                    DetermineWinner();
                }
            }

            //Decreases time
            time -= 20;

            //Checks if time is zero
            if (time <= 0)
            {
                if (turn == "p1") //Swithces to p2
                {
                    SwithToP2();
                }

                else if (turn == "p2") //Determines winner
                {
                    DetermineWinner();
                }
            }

            //redraw the screen
            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //Time indicator
            SolidBrush timeIndicatorBrush = new SolidBrush(CalculateCustomColour());
            timeIndicator.Width = (int)(600 * ((double)time / 10000));
            e.Graphics.FillRectangle(timeIndicatorBrush, timeIndicator);

            //update labels
            scoreLabel.Text = $"Time: {time}";
            
            if (scoreBoost != 0 && scoreBoost > 1)
            {
                scoreBoostLabel.Text = $"+{scoreBoost}";
            }
            if (scoreBoost != 0 && scoreBoost < 1)
            {
                scoreBoostLabel.Text = $"{scoreBoost}";
            }

            if (turn == "p2")
            {
                p1ScoreLabel.Text = $"Player 1 Score: {p1FinalScore}";
            }

            //draw hero
            if (turn == "p1")
            {
                e.Graphics.FillRectangle(blueBrush, hero);
            }
            if (turn == "p2")
            {
                e.Graphics.FillRectangle(redBrush, hero);
            }


            //draw balls
            for (int i = 0; i < ballList.Count(); i++)
            {
                if (ballColours[i] == "white")
                {
                    e.Graphics.FillEllipse(whiteBrush, ballList[i]);
                }
                else if (ballColours[i] == "gray")
                {
                    e.Graphics.FillEllipse(grayBrush, ballList[i]);
                }
                else
                {
                    e.Graphics.FillEllipse(goldBrush, ballList[i]);
                }
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.X >= hero.X && e.X <= hero.X + hero.Width && e.Y >= hero.Y && e.Y <= hero.Y + hero.Width) //Only allows to be pressed down when cusor is over square
            {
                mousePressed = true;
            }
        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            mousePressed = false;
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mousePressed == true)
            {
                hero.X = e.X - hero.Width / 2;
                hero.Y = e.Y - hero.Height / 2;

                if (hero.Right > this.Width)
                {
                    hero.X = this.Width - hero.Width;
                }
                if (hero.Top < 0)
                {
                    hero.Y = 0;
                }
                if (hero.Left < 0)
                {
                    hero.X = 0;
                }
                if (hero.Bottom > this.Height)
                {
                    hero.Y = this.Height - hero.Height;
                }

                Refresh();
            }

        }

        private void SwithToP2()
        {
            p1FinalScore = time + scoreBoost;

            turn = "p2";
            time = 10000;
            scoreBoost = 0;

            hero.X = 280;
            hero.Y = 540;

            mousePressed = false;

            scoreBoostLabel.Text = null;
            p1ScoreLabel.Text = $"Player 1 Score: {p1FinalScore}";
        }

        private void DetermineWinner()
        {
            p2FinalScore = time + scoreBoost;

            titleLabel.Visible = true;
            titleLabel.Text = $"Player 2 score: {p2FinalScore}\n\n";

            if (p1FinalScore > p2FinalScore)
            {
                titleLabel.Text += "Player 1 wins";
            }
            else if (p1FinalScore < p2FinalScore)
            {
                titleLabel.Text += "Player 2 wins";
            }
            else
            {
                titleLabel.Text += "Tie!";
            }

            titleLabel.Text += "\n\nPress ESC to restart";

            gameOver = true;
            gameTimer.Stop();
        }

        private Color CalculateCustomColour()
        {
            double percentageRemaining = (double)time / 10000;

            //Figiures out the colours based on remaining time
            int red = (int)(255 * (1 - percentageRemaining));
            int green = (int)(255 * percentageRemaining);
            int blue = 0; // No blue component for simplicity

            // Ensure colour components are within valid range
            red = Math.Max(0, Math.Min(255, red));
            green = Math.Max(0, Math.Min(255, green));

            return Color.FromArgb(red, green, blue);
        }

        private void RestartGame()
        {
            // Reset game variables here
            time = 10000;
            scoreBoost = 0;
            p1FinalScore = 0;
            p2FinalScore = 0;
            turn = "p1";
            ballList.Clear();
            ballColours.Clear();
            ballSpeeds.Clear();
            leftOrRight.Clear();
            ballSizeList.Clear();
            gameStart = false;
            gameOver = false;
            // Reset any other necessary variables or game state
            p1ScoreLabel.Text = null;
            titleLabel.Text = null;
            scoreBoostLabel.Text = null;

            // Reset hero position
            hero.X = 280;
            hero.Y = 540;

            // Start the game timer
            gameTimer.Start();
            Refresh();
        }

        private void StartGame()
        {
            titleLabel.Text = null;
            gameStart = true;

            // Put hero at stat
            hero.X = 280;
            hero.Y = 540;

            // start the game timer
            gameTimer.Start();
            Refresh();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && gameOver == true)
            {
                RestartGame();
            }
            else if (e.KeyCode == Keys.Escape && gameStart == true)
            {
                StartGame();
            }
        }
    }
}