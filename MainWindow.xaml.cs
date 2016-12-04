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

        private Earth earth;
        private bool lasersCooling = false;
        private int spawnTimer = 1500;
        private Timer spawnTimerRaise;
        private Timer spawnEnemy;

        private IList<Sprite> shipPool = new List<Sprite>();
        IList<Sprite> firingList;


        private Random rand = new Random();

        public MainWindow()
        {
            Hide();

            Splash splash = new Splash();
            splash.Show();
            System.Threading.Thread.Sleep(5000);
            splash.Close();

            Show();
            InitializeComponent();

            earth = new Earth(200, 400, 2, 3, InitEarthPic());

            shipPool.Add(earth);

            Space.Children.Add(earth.Rect);

            Timer timetoMove = new Timer(5);
            timetoMove.Elapsed += MoveEnemyShip;

            timetoMove.AutoReset = true;
            timetoMove.Enabled = true;

            Timer laserTimer = new Timer(500);
            laserTimer.Elapsed += CoolLasersDown;

            laserTimer.AutoReset = true;
            laserTimer.Enabled = true;

            Timer enemyFire = new Timer(2000);
            enemyFire.Elapsed += EnemiesFire;

            enemyFire.AutoReset = true;
            enemyFire.Enabled = true;

            spawnEnemy = new Timer(spawnTimer);
            spawnEnemy.Elapsed += SpawnEnemies;

            spawnEnemy.AutoReset = true;
            spawnEnemy.Enabled = true;


        }

        private void MoveShips()
        {
            for (int counter = 0; counter != shipPool.Count; counter++)
            {

                if (shipPool[counter].X <= -1 || shipPool[counter].X >= 525 ||
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

            if (earth.Up)
            {
                earth.MoveY(-earth.Speed);
            }
            if (earth.Down)
            {
                earth.MoveY(earth.Speed);
            }
            if (earth.Left)
            {
                earth.MoveX(-earth.Speed);
            }
            if (earth.Right)
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
            catch (InvalidOperationException e) { }

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
            Projectiles enemyLaser = new Projectiles(origin.X + 10, origin.Y, 6, 1, initProjPic(), d, 1);
            Space.Children.Add(enemyLaser.Rect);
            shipPool.Add(enemyLaser);
        }

        /*Tori added method to spawn projectiles next to ship*/
        private void Fire(Sprite origin, Direction d)
        {
            Projectiles Left = new Projectiles(origin.X, origin.Y, 6, 1, initProjPic(), d, 1);
            Space.Children.Add(Left.Rect);
            shipPool.Add(Left);

            Projectiles Right = new Projectiles(origin.X + 45, origin.Y, 6, 1, initProjPic(), d, 1);
            Space.Children.Add(Right.Rect);
            shipPool.Add(Right);
        }

        /*Tori added method to initilize projectile rectangle with projectile image*/
        private Rectangle initProjPic()
        {
            Rectangle projPic = new Rectangle();
            projPic.Fill = (ImageBrush)Resources["ProjImage"];

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
    }

}
