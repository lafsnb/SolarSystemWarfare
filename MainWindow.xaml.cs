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

        private IList<Sprite> shipPool = new List<Sprite>();
        //private IList<Patterns> shipPatterns = new List<Patterns>();

        private Random rand = new Random();

        public MainWindow()
        {
            InitializeComponent();

            earth = new Earth(200, 400, 3, 1, InitEarthPic());

            shipPool.Add(earth);

            Space.Children.Add(earth.Rect);

            Timer timetoMove = new Timer(5);
            timetoMove.Elapsed += MoveEnemyShip;

            timetoMove.AutoReset = true;
            timetoMove.Enabled = true;

            Timer spawnEnemy = new Timer(500);
            spawnEnemy.Elapsed += SpawnEnemies;

            spawnEnemy.AutoReset = true;
            spawnEnemy.Enabled = true;


        }

        private void MoveShips()
        {
            IList<int> removeCount = new List<int>();
            for (int counter = 0; counter != shipPool.Count; counter++)
            {
                if (shipPool[counter].Dead)
                {
                    removeCount.Add(counter);
                    continue;
                }
                else
                {
                    Canvas.SetTop(shipPool[counter].Rect, shipPool[counter].Y);
                    Canvas.SetLeft(shipPool[counter].Rect, shipPool[counter].X);
                }
            }

            foreach (int r in removeCount)
            {
                shipPool.RemoveAt(r);
            }
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
                en = new Enemy(0, rand.Next(0, 301), 2, 1, 1, 1, initEnemyPic());

                en.Pattern = new PatternSwishRight(en, rand.Next(100, 301));

            }
            //Choice two is UpDownRight Pattern
            else if (spawnChoice == 2)
            {
                en = new Enemy(rand.Next(50, 401), 0, 2, 1, 1, 1, initEnemyPic());

                en.Pattern = new PatternUpDownRight(en, rand.Next(20, 251));

            }
            //Choice three is UpDownLeft Pattern
            else if (spawnChoice == 3)
            {
                en = new Enemy(rand.Next(50, 401), 0, 2, 1, 1, 1, initEnemyPic());

                en.Pattern = new PatternUpDownLeft(en, rand.Next(20, 251));

            }
            else
            {
                en = new Enemy(525, rand.Next(0, 301), 2, 1, 1, 1, initEnemyPic());

                en.Pattern = new PatternSwishLeft(en, rand.Next(100, 301));
            }

            shipPool.Add(en);
            Space.Children.Add(shipPool.Last().Rect);

            //Two more choices to be implamented
            //They will be mirrors of the other Patterns
        }

        private void PatternsMove()
        {
            //foreach(Patterns pat in shipPatterns)
            //{
            //    pat.runPattern();
            //}

            foreach (Sprite en in shipPool)
            {
                if (en is Enemy)
                {
                    ((Enemy)en).Pattern.runPattern();

                }
                /*Tori added in order to move laser projectile*/
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
                    earth.MoveY(-earth.Speed);
                    break;

                case Key.Left:
                case Key.A:
                    earth.MoveX(-earth.Speed);
                    break;

                case Key.Right:
                case Key.D:
                    earth.MoveX(earth.Speed);
                    break;

                case Key.Down:
                case Key.S:
                    earth.MoveY(earth.Speed);
                    break;

                /*Tori added key to fire up*/
                case Key.Z:
                case Key.OemComma:
                    Fire(earth, Direction.UP);
                    break;


                /*Tori added key to fire down*/
                case Key.X:
                case Key.OemPeriod:
                    Fire(earth, Direction.DOWN);
                    break;

            }
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

        private Rectangle InitEarthPic()
        {
            Rectangle earthPic = new Rectangle();
            earthPic.Fill = (ImageBrush)Resources["EarthImage"];
            earthPic.Height = 40;
            earthPic.Width = 45;

            return earthPic;
        }

        private Rectangle initEnemyPic()
        {
            Rectangle enemyPic = new Rectangle();
            enemyPic.Fill = (ImageBrush)Resources["EnemyImage"];

            return enemyPic;
        }

        /*Tori added method to initilize projectile rectangle with projectile image*/
        private Rectangle initProjPic()
        {
            Rectangle projPic = new Rectangle();
            projPic.Fill = (ImageBrush)Resources["ProjImage"];

            return projPic;
        }
    }
}
