using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Windows.Threading;

namespace SolarSystemWarfare
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml aka GameEngine
    /// </summary>
    public partial class MainWindow : Window
    {
        public delegate void runOnUIThread();

        public string highScores;

        private System.Threading.EventWaitHandle pause = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.ManualReset);

        private Earth earth;
        private bool lasersCooling = false;
        private bool gameStarted = false;
        private bool gamePaused = false;
        double screenWidth = SystemParameters.VirtualScreenWidth;
        double screenHeight = SystemParameters.VirtualScreenHeight;
        private int upDownSpawnLocation = ((int)SystemParameters.VirtualScreenWidth / 3) - 100;
        private int swishSpawnLocation = ((int)SystemParameters.VirtualScreenWidth / 3) - 200;
        private int patternHowFarDown = ((int)SystemParameters.VirtualScreenHeight) - (int)(SystemParameters.VirtualScreenHeight / 2.5);
        private int swishHowFarX = ((int)SystemParameters.VirtualScreenWidth / 3) - 25;
        private GameTimer spawnEnemy;
        private int spawnEnemySpeed;
        private GameTimer timetoMove;
        private int timetoMoveSpeed = 5;
        private GameTimer laserTimer;
        private int laserTimerSpeed = 500;
        private GameTimer enemyFire;
        private int enemyFireSpeed = 2000;


        private IList<Sprite> shipPool = new List<Sprite>();
        IDictionary<string, long> scores;
        IList<Sprite> firingList;


        private Random rand = new Random();

        public MainWindow()
        {
            InitializeComponent();

            Panel1.Width = screenWidth / 3;
            Panel1.Height = screenHeight;
            Panel2.Width = screenWidth / 3;
            Panel2.Height = screenHeight;
            Space.Width = screenWidth / 3;
            Space.Height = screenHeight;

            TopWindow.MinHeight = screenHeight;
            TopWindow.MinWidth = screenWidth;
            TopWindow.Height = screenHeight;
            TopWindow.Width = screenWidth;

            Hide();

            Splash splash = new Splash();
            splash.Show();
            System.Threading.Thread.Sleep(5000);
            splash.Close();

            Show();

            earth = new Earth(200, 400, 2, 3, InitEarthPic());

            ShowStartScreen();

            //StartGame();
        }


        // Position updating methods

        /*
         * 
         * Main method to update ship positions, runs every 10 ms.
         * Also removes ships from shipPool who are dead, and removes them from the Canvas.
         * 
         */
        private void MoveShips()
        {
            for (int counter = 0; counter != shipPool.Count; counter++)
            {

                if (shipPool[counter].X <= -21 || shipPool[counter].X >= Space.ActualWidth ||
                        shipPool[counter].Y <= -20 || shipPool[counter].Y >= Space.ActualHeight - 80)
                {

                    shipPool[counter].Destroy();
                    Console.WriteLine("Ship removed");
                }
                else
                {
                    Canvas.SetTop(shipPool[counter].Rect, shipPool[counter].Y);
                    Canvas.SetLeft(shipPool[counter].Rect, shipPool[counter].X);
                }
            }

            RemoveShips.CollisionCheck(shipPool);

            foreach (Sprite el in shipPool)
            {
                if (el.Dead)
                {
                    Space.Children.Remove(el.Rect);
                }
            }

            RemoveShips.remove(shipPool);

            DisplayScore();

            if (earth.Durability == 2)
            {
                Heart1.Fill = (ImageBrush)Resources["EmptyHeart"];
            }
            else if (earth.Durability == 1)
            {
                Heart2.Fill = (ImageBrush)Resources["EmptyHeart"];
            }

            if (earth.Dead)
            {
                GameOver();
            }
        }
        /*
        * 
        * Method that changes positions for all Sprites in the shipPool.
        * Catches if an exception occurs while we are looping, exception occurs from list being updated during the foreach loop.
        * This exception only occurs evey few mins, and the impact is very minimal if at all.
        * 
        */
        private void PatternsMove()
        {

            if (earth.Up && earth.Y >= 10)
            {
                earth.MoveY(-earth.Speed);
            }
            if (earth.Down && earth.Y <= Space.ActualHeight - 125)
            {
                earth.MoveY(earth.Speed);
            }
            if (earth.Left && earth.X >= 10)
            {
                earth.MoveX(-earth.Speed);
            }
            if (earth.Right && earth.X <= Space.ActualWidth - 55)
            {
                earth.MoveX(earth.Speed);
            }
            if (earth.Firing && !lasersCooling)
            {
                lasersCooling = true;
                this.Dispatcher.Invoke(DispatcherPriority.Normal, new runOnUIThread(CheatLaserFire));
            }

            try
            {
                foreach (Sprite en in shipPool)
                {
                    if (en is Enemy)
                    {
                        ((Enemy)en).Pattern.runPattern();
                    }
                    else if (en is Projectiles)
                    {
                        if (((Projectiles)en).Direction == Direction.UP)
                        {
                            en.MoveY(-en.Speed);
                        }
                        else if (((Projectiles)en).Direction == Direction.DOWN)
                        {
                            en.MoveY(en.Speed);
                        }
                    }
                }
            }
            catch (InvalidOperationException) { }
        }

        /*
         * 
         * Starts MoveShips on the UI thread.
         * 
         */

        private void MoveEnemyShip(Object source, ElapsedEventArgs e)
        {
            PatternsMove();

            this.Dispatcher.Invoke(DispatcherPriority.Normal, new runOnUIThread(MoveShips));

        }

        // Sprite Spawning methods

        // Laser Spawning methods

        /*
         * 
         * Adds enemies to firingList from shipPool, then start CheatEnemyLaserFire on UI thread.
         *
         */

        private void EnemiesFire(Object source, ElapsedEventArgs e)
        {
            firingList = new List<Sprite>();

            foreach (Sprite el in shipPool)
            {
                if (el is Enemy)
                {
                    firingList.Add(el);
                }
            }

            this.Dispatcher.Invoke(DispatcherPriority.Normal, new runOnUIThread(CheatEnemyLaserFire));
        }

        /*
         * 
         * Creates a projectile for an Enemy and adds it to the shipPool.
         * 
         */

        private void EnemyFire(Sprite origin, Direction d)
        {
            Projectiles enemyLaser = new Projectiles(origin.X + 10, origin.Y, 6, 1, initProjPic(true), d, 1);
            Space.Children.Add(enemyLaser.Rect);
            shipPool.Add(enemyLaser);
        }

        /*
         * 
         * Creates two projectiles for Earth, and adds them to the shipPool.
         * 
         */

        /*Tori added method to spawn projectiles next to ship*/
        private void Fire(Sprite origin, Direction d)
        {
            Projectiles Left = new Projectiles(origin.X, origin.Y, 6, 1, initProjPic(false), d, 1);
            Space.Children.Add(Left.Rect);
            shipPool.Add(Left);

            Projectiles Right = new Projectiles(origin.X + 45, origin.Y, 6, 1, initProjPic(false), d, 1);
            Space.Children.Add(Right.Rect);
            shipPool.Add(Right);
        }

        /*
         * 
         * Not a spawning method, but it does run every 500 ms to allow Earth to fire lasers again.
         * 
         */

        private void CoolLasersDown(Object source, ElapsedEventArgs e)
        {
            lasersCooling = false;
            laserTimer.Interval = laserTimerSpeed;
        }

        /*
         * 
         * Runs Fire method on UI thread
         * Couldn't figureout how to create a delegate with parameters,
         * so I just made this to run Fire on UI thread through Dispatcher.
         * 
         */
        private void CheatLaserFire()
        {
            Fire(earth, Direction.UP);
        }

        /*
         * 
         * Runs EnemyFire method on UI thread
         * Couldn't figureout how to create a delegate with parameters,
         * so I just made this to run EnemyFire on UI thread through Dispatcher.
         * 
         */

        private void CheatEnemyLaserFire()
        {
            foreach (Sprite el in firingList)
            {
                EnemyFire(el, Direction.DOWN);
            }
        }

        //Enemy Spawning Methods

        /*
         * 
         * Starts NewEnemySpawn on the UI thread.
         * 
         */

        private void SpawnEnemies(Object source, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, new runOnUIThread(NewEnemySpawn));
        }

        /*
         * 
         * Spawns a new enemy in a random pattern, out of four patterns.
         * Spawns the enemy in a random location for the specified pattern.
         * 
         */

        private void NewEnemySpawn()
        {
            int spawnChoice = rand.Next(1, 5);

            Enemy en;

            //Choice one is PatternSwishRight
            if (spawnChoice == 1)
            {
                en = new Enemy(0, rand.Next(0, swishSpawnLocation), 2, 1, 1, 2000, initEnemyPic());

                en.Pattern = new PatternSwishRight(en, rand.Next(100, patternHowFarDown), swishHowFarX);

            }
            //Choice two is UpDownRight Pattern
            else if (spawnChoice == 2)
            {
                en = new Enemy(rand.Next(50, upDownSpawnLocation), 0, 2, 1, 1, 2000, initEnemyPic());

                en.Pattern = new PatternUpDownRight(en, rand.Next(20, patternHowFarDown));

            }
            //Choice three is UpDownLeft Pattern
            else if (spawnChoice == 3)
            {
                en = new Enemy(rand.Next(50, upDownSpawnLocation), 0, 2, 1, 1, 2000, initEnemyPic());

                en.Pattern = new PatternUpDownLeft(en, rand.Next(20, patternHowFarDown));

            }
            //Choice four is PatternSwishLeft
            else
            {
                en = new Enemy(SystemParameters.VirtualScreenWidth / 3, rand.Next(0, swishSpawnLocation), 2, 1, 1, 2000, initEnemyPic());

                en.Pattern = new PatternSwishLeft(en, rand.Next(100, patternHowFarDown), swishHowFarX);
            }

            // Add enemy to the shipPool and to the Canvas.

            shipPool.Add(en);
            Space.Children.Add(shipPool.Last().Rect);

            // Increase difficulty by decreasing spawn time, up to 300 ms.

            if (spawnEnemySpeed != 300)
            {
                spawnEnemy.Interval = spawnEnemySpeed -= 20;
            }


        }

        // Listeners

        /*
         * 
         * Listens for a keyDown, if a key that is supported is hit down
         * then the ship will start moving in that direction, or start firing lasers.
         * 
         */

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                case Key.W:
                    earth.Up = true;
                    break;

                case Key.Left:
                case Key.A:
                    earth.Left = true;
                    break;

                case Key.Right:
                case Key.D:
                    earth.Right = true;
                    break;

                case Key.Down:
                case Key.S:
                    earth.Down = true;
                    break;

                case Key.Space:
                    earth.Firing = true;
                    break;

                case Key.Escape:
                case Key.F10:
                    if (gameStarted)
                    {
                        if (gamePaused)
                        {
                            ResumeGame();
                        } else
                        {
                            PauseGame();
                        }
                        
                    }
                    break;
            }
        }

        /*
         * 
         * Listens for a keyUp, if a key that is supported is unpressed
         * then the ship will stop moving in that direction, or stop firing lasers.
         * 
         */

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                case Key.W:
                    earth.Up = false;
                    break;

                case Key.Left:
                case Key.A:
                    earth.Left = false;
                    break;

                case Key.Right:
                case Key.D:
                    earth.Right = false;
                    break;

                case Key.Down:
                case Key.S:
                    earth.Down = false;
                    break;

                /*Tori added key to fire up*/
                case Key.Space:
                    earth.Firing = false;
                    break;
            }
        }

        /*
         * 
         * When the game ends the user will have the chance to enter their name, and have it commited to disk.
         * That happens through this method, that listens for the Enter Score button being pressed.
         * 
         */

        private void CommitHighScore_Click(object sender, RoutedEventArgs e)
        {
            EnterHighScore.Visibility = Visibility.Hidden;
            CommitHighScore.Visibility = Visibility.Hidden;

            double left = (Space.ActualWidth - StartGameBt.ActualWidth) / 2;
            Canvas.SetLeft(StartGameBt, left);
            Canvas.SetTop(StartGameBt, 400);

            if (string.IsNullOrWhiteSpace(EnterHighScore.Text))
            {
                Score.WriteToFile("Player1", scores);
            }
            else
            {
                Score.WriteToFile(EnterHighScore.Text, scores);
            }

            Score.ResetScore();

            GameOverLabel.Visibility = Visibility.Hidden;

            ShowStartScreen();
        }

        /*
         * 
         * Listener for the button that starts the game, the Start Game button.
         * 
         */

        private void StartGameBt_Click(object sender, RoutedEventArgs e)
        {
            GameOverLabel.Visibility = Visibility.Hidden;
            StartGameBt.Visibility = Visibility.Hidden;
            GameTitle.Visibility = Visibility.Hidden;

            Heart1.Fill = (ImageBrush)Resources["FullHeart"];
            Heart2.Fill = (ImageBrush)Resources["FullHeart"];
            Heart3.Fill = (ImageBrush)Resources["FullHeart"];

            Heart1.Visibility = Visibility.Visible;
            Heart2.Visibility = Visibility.Visible;
            Heart3.Visibility = Visibility.Visible;

            StartGame();
        }

        // UI initializers

        /*
         * 
         * Initializes earths picture.
         * 
         */

        private Rectangle InitEarthPic()
        {
            Rectangle earthPic = new Rectangle();
            earthPic.Fill = (ImageBrush)Resources["EarthImage"];
            earthPic.Height = 40;
            earthPic.Width = 50;

            return earthPic;
        }

        /*
         * 
         * Initializes the picture for an enemy ship.
         * 
         */

        private Rectangle initEnemyPic()
        {
            Rectangle enemyPic = new Rectangle();
            enemyPic.Fill = (ImageBrush)Resources["EnemyImage"];

            return enemyPic;
        }

        /*
         * 
         * Initializes a projectiles picture, initializes different pictures
         * for Earth and Enemy.
         *
         */

        /*Tori added method to initilize projectile rectangle with projectile image*/
        private Rectangle initProjPic(bool enemy)
        {
            Rectangle projPic = new Rectangle();
            if (enemy)
            {
                projPic.Fill = (ImageBrush)Resources["ProjImage"];
            }
            else
            {
                projPic.Fill = (ImageBrush)Resources["ProjEarthImage"];
            }
            return projPic;
        }

        /*
         * 
         * Updates the score counter
         * 
         */
        private void DisplayScore()
        {
            ScoreLbl.Content = $"Score: {Score.GetScore()}";
        }

        /*
         * 
         * Stops all game timers, removes all Sprites from the Canvas, shipPool
         * and puts up the GameOverLabel with the enter name box and Enter Score button
         * 
         */

        private void GameOver()
        {
            gameStarted = false;

            spawnEnemy.Stop();
            timetoMove.Stop();
            laserTimer.Stop();
            enemyFire.Stop();

            spawnEnemy.Enabled = false;
            timetoMove.Enabled = false;
            laserTimer.Enabled = false;
            enemyFire.Enabled = false;

            for (int counter = 0; counter != shipPool.Count; counter++)
            {
                Space.Children.Remove(shipPool[counter].Rect);
            }

            shipPool.Clear();

            double left = (Space.ActualWidth - GameOverLabel.ActualWidth) / 2;
            Canvas.SetLeft(GameOverLabel, left);
            Canvas.SetTop(GameOverLabel, 300);

            left = (Space.ActualWidth - EnterHighScore.ActualWidth) / 2;
            Canvas.SetLeft(EnterHighScore, left - 60);
            Canvas.SetTop(EnterHighScore, 400);

            left = (Space.ActualWidth - CommitHighScore.ActualWidth) / 2;
            Canvas.SetLeft(CommitHighScore, left + 60);
            Canvas.SetTop(CommitHighScore, 400);

            EnterHighScore.Visibility = Visibility.Visible;
            CommitHighScore.Visibility = Visibility.Visible;
            GameOverLabel.Visibility = Visibility.Visible;

            Heart1.Visibility = Visibility.Hidden;
            Heart2.Visibility = Visibility.Hidden;
            Heart3.Visibility = Visibility.Hidden;
        }

        /*
         * 
         * Initializes all the timers to start the game,
         * and initializes the earth.
         * 
         */

        private void StartGame()
        {
            gameStarted = true;

            earth = new Earth(200, 400, 2, 3, InitEarthPic());

            shipPool.Add(earth);

            Space.Children.Add(earth.Rect);

            spawnEnemySpeed = 1500;

            timetoMove = new GameTimer(timetoMoveSpeed);
            timetoMove.Elapsed += MoveEnemyShip;

            timetoMove.AutoReset = true;
            timetoMove.Enabled = true;

            laserTimer = new GameTimer(laserTimerSpeed);
            laserTimer.Elapsed += CoolLasersDown;

            laserTimer.AutoReset = true;
            laserTimer.Enabled = true;

            enemyFire = new GameTimer(enemyFireSpeed);
            enemyFire.Elapsed += EnemiesFire;

            enemyFire.AutoReset = true;
            enemyFire.Enabled = true;

            spawnEnemy = new GameTimer(spawnEnemySpeed);
            spawnEnemy.Elapsed += SpawnEnemies;

            spawnEnemy.AutoReset = true;
            spawnEnemy.Enabled = true;
        }

        /*
         * 
         * Initializes the welcome screen
         * 
         */

        private void ShowStartScreen()
        {

            double left = (Space.ActualWidth - GameTitle.ActualWidth) / 2;
            Canvas.SetLeft(GameTitle, left);
            Canvas.SetTop(GameTitle, 300);

            GameTitle.Visibility = Visibility.Visible;

            left = (Space.ActualWidth - StartGameBt.ActualWidth) / 2;
            Canvas.SetLeft(StartGameBt, left);
            Canvas.SetTop(StartGameBt, 400);

            StartGameBt.Visibility = Visibility.Visible;

            scores = Score.ReadFromFile();
            StringBuilder namesAndScores = new StringBuilder();
            for (int counter = 0; counter != scores.Count && counter != 15; counter++)
            {

                namesAndScores.Append(string.Format("{0}. {1}: {2}\n", counter + 1, scores.Keys.ElementAt(counter),
                                                                scores.Values.ElementAt(counter).ToString()));

            }

            Scores.Content = namesAndScores.ToString();

            ScoreLbl.Content = "Score: 0";
        }

        private void ResumeGame()
        {
            gamePaused = false;

            MenuPopup.IsOpen = false;

            spawnEnemy.Start();
            timetoMove.Start();
            laserTimer.Start();
            enemyFire.Start();
        }

        private void PauseGame()
        {
            gamePaused = true;

            MenuPopup.IsOpen = true;

            spawnEnemy.Pause();
            timetoMove.Pause();
            laserTimer.Pause();
            enemyFire.Pause();
        }

        private void Resume_Click(object sender, RoutedEventArgs e)
        {
            ResumeGame();
        }

        private void ExitGame_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}