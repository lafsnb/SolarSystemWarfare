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

        private Earth earth;
        private bool lasersCooling = false;
        private int spawnTimer;
        private Timer spawnEnemy;
        private Timer timetoMove;
        private Timer laserTimer;
        private Timer enemyFire;

        private IList<Sprite> shipPool = new List<Sprite>();
        IDictionary<string, long> scores;
        IList<Sprite> firingList;


        private Random rand = new Random();

        public MainWindow()
        {
            InitializeComponent();

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

        private void MoveShips()
        {
            for (int counter = 0; counter != shipPool.Count; counter++)
            {

                if (shipPool[counter].X <= -21 || shipPool[counter].X >= 565 ||
                        shipPool[counter].Y <= -20 || shipPool[counter].Y >= 650)
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

        private void SpawnTimerIncrease(Object source, ElapsedEventArgs e)
        {
            if (spawnTimer != 400)
            {
                spawnTimer -= 10;
                spawnEnemy.Interval = spawnTimer;
            }
        }

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

        private void CoolLasersDown(Object source, ElapsedEventArgs e)
        {
            lasersCooling = false;
        }

        private void SpawnEnemies(Object source, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, new runOnUIThread(NewEnemySpawn));
        }


        private void NewEnemySpawn()
        {
            int spawnChoice = rand.Next(1, 5);

            Enemy en;

            //Choice one is PatternSwish
            if (spawnChoice == 1)
            {
                en = new Enemy(0, rand.Next(0, 301), 2, 1, 1, 2000, initEnemyPic());

                en.Pattern = new PatternSwishRight(en, rand.Next(100, 301));

            }
            //Choice two is UpDownRight Pattern
            else if (spawnChoice == 2)
            {
                en = new Enemy(rand.Next(50, 401), 0, 2, 1, 1, 2000, initEnemyPic());

                en.Pattern = new PatternUpDownRight(en, rand.Next(20, 251));

            }
            //Choice three is UpDownLeft Pattern
            else if (spawnChoice == 3)
            {
                en = new Enemy(rand.Next(50, 401), 0, 2, 1, 1, 2000, initEnemyPic());

                en.Pattern = new PatternUpDownLeft(en, rand.Next(20, 251));

            }
            else
            {
                en = new Enemy(525, rand.Next(0, 301), 2, 1, 1, 2000, initEnemyPic());

                en.Pattern = new PatternSwishLeft(en, rand.Next(100, 301));
            }

            shipPool.Add(en);
            Space.Children.Add(shipPool.Last().Rect);

            if (spawnTimer != 400)
            {
                spawnEnemy.Interval = spawnTimer -= 20;
            }


        }

        private void PatternsMove()
        {

            if (earth.Up && earth.Y >= 10)
            {
                earth.MoveY(-earth.Speed);
            }
            if (earth.Down && earth.Y <= 640)
            {
                earth.MoveY(earth.Speed);
            }
            if (earth.Left && earth.X >= 10)
            {
                earth.MoveX(-earth.Speed);
            }
            if (earth.Right && earth.X <= 515)
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

        private void MoveEnemyShip(Object source, ElapsedEventArgs e)
        {
            PatternsMove();

            this.Dispatcher.Invoke(DispatcherPriority.Normal, new runOnUIThread(MoveShips));

        }

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
            }
        }

        private Rectangle InitEarthPic()
        {
            Rectangle earthPic = new Rectangle();
            earthPic.Fill = (ImageBrush)Resources["EarthImage"];
            earthPic.Height = 40;
            earthPic.Width = 50;

            return earthPic;
        }

        private Rectangle initEnemyPic()
        {
            Rectangle enemyPic = new Rectangle();
            enemyPic.Fill = (ImageBrush)Resources["EnemyImage"];

            return enemyPic;
        }

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

        private void EnemyFire(Sprite origin, Direction d)
        {
            Projectiles enemyLaser = new Projectiles(origin.X + 10, origin.Y, 6, 1, initProjPic(true), d, 1);
            Space.Children.Add(enemyLaser.Rect);
            shipPool.Add(enemyLaser);
        }

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

        private void CheatLaserFire()
        {
            Fire(earth, Direction.UP);
        }

        private void CheatEnemyLaserFire()
        {
            foreach (Sprite el in firingList)
            {
                EnemyFire(el, Direction.DOWN);
            }
        }

        private void DisplayScore()
        {
            ScoreLbl.Content = $"Score: {Score.GetScore()}";
        }

        private void GameOver()
        {
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

        private void StartGame()
        {
            earth = new Earth(200, 400, 2, 3, InitEarthPic());

            shipPool.Add(earth);

            Space.Children.Add(earth.Rect);

            spawnTimer = 1500;

            timetoMove = new Timer(5);
            timetoMove.Elapsed += MoveEnemyShip;

            timetoMove.AutoReset = true;
            timetoMove.Enabled = true;

            laserTimer = new Timer(500);
            laserTimer.Elapsed += CoolLasersDown;

            laserTimer.AutoReset = true;
            laserTimer.Enabled = true;

            enemyFire = new Timer(2000);
            enemyFire.Elapsed += EnemiesFire;

            enemyFire.AutoReset = true;
            enemyFire.Enabled = true;

            spawnEnemy = new Timer(spawnTimer);
            spawnEnemy.Elapsed += SpawnEnemies;

            spawnEnemy.AutoReset = true;
            spawnEnemy.Enabled = true;
        }

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
            for (int counter = 0; counter != scores.Count; counter++)
            {

                namesAndScores.Append(string.Format("{0}: {1}\n", scores.Keys.ElementAt(counter),
                                                                scores.Values.ElementAt(counter)));

            }

            Scores.Content = namesAndScores.ToString();

            ScoreLbl.Content = "Score: 0";
        }

        private void CommitHighScore_Click(object sender, RoutedEventArgs e)
        {
            EnterHighScore.Visibility = Visibility.Hidden;
            CommitHighScore.Visibility = Visibility.Hidden;

            double left = (Space.ActualWidth - StartGameBt.ActualWidth) / 2;
            Canvas.SetLeft(StartGameBt, left);
            Canvas.SetTop(StartGameBt, 400);

            Score.WriteToFile(EnterHighScore.Text, scores);

            Score.ResetScore();

            GameOverLabel.Visibility = Visibility.Hidden;

            ShowStartScreen();
        }

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
    }

}
