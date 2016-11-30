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
        public delegate void MovingShips();

        Enemy en;
        Enemy en2;
        Enemy en3;
        Enemy en4;
        Enemy en5;

        Patterns enPat1;
        Patterns enPat2;
        Patterns enPat3;
        Patterns enPat4;
        Patterns enPat5;

        Earth earth;

        bool goRight = true;
        bool goLeft = false;

        IList<Sprite> shipPool = new List<Sprite>();

        public MainWindow()
        {
            InitializeComponent();

            en = new Enemy(20, 20, 1, 1, new Rectangle(), Enemy1, 1, 1);
            en2 = new Enemy(60, 50, 1, 1, new Rectangle(), Enemy2, 1, 1);
            en3 = new Enemy(100, 80, 1, 1, new Rectangle(), Enemy3, 1, 1);
            en4 = new Enemy(140, 110, 1, 1, new Rectangle(), Enemy4, 1, 1);
            en5 = new Enemy(180, 140, 1, 1, new Rectangle(), Enemy5, 1, 1);

            enPat1 = new Patterns(en);
            enPat2 = new Patterns(en2);
            enPat3 = new Patterns(en3);
            enPat4 = new Patterns(en4);
            enPat5 = new Patterns(en5);

            earth = new Earth(200, 400, 1, 1, new Rectangle(), EarthPic);            

            shipPool.Add(en);
            shipPool.Add(earth);
            shipPool.Add(en2);
            shipPool.Add(en3);
            shipPool.Add(en4);
            shipPool.Add(en5);

            Canvas.SetLeft(en.Icon, 20);
            Canvas.SetTop(en.Icon, 20);

            Timer timetoMove = new Timer(5);
            timetoMove.Elapsed += MoveEnemyShip;

            timetoMove.AutoReset = true;
            timetoMove.Enabled = true;



        }

        private void MoveShips()
        {
            IList<int> removeCount = new List<int>();
            for (int counter = 0; counter != shipPool.Count; counter++)
            {
                if (shipPool[counter] == null)
                {
                    removeCount.Add(counter);
                }
                else
                {
                    Canvas.SetTop(shipPool[counter].Icon, shipPool[counter].Y);
                    Canvas.SetLeft(shipPool[counter].Icon, shipPool[counter].X);
                }
            }

            foreach (int r in removeCount)
            {
                shipPool.RemoveAt(r);
            }
        }

        private void MoveEnemyShip(Object source, ElapsedEventArgs e)
        {
            enPat1.SwishSway();
            enPat2.SwishSway();
            enPat3.SwishSway();
            enPat4.SwishSway();
            enPat5.SwishSway();

            this.Dispatcher.Invoke(DispatcherPriority.Send, new MovingShips(MoveShips));

        }



        private void MovePatternBackAndForth(Sprite sprite)
        {
            if (goRight)
            {
                sprite.MoveY(.5);
                sprite.MoveX(2);
            } else if (goLeft)
            {
                sprite.MoveY(.5);
                en.MoveX(-2);
            }
            if (sprite.X == 400)
            {
                goRight = false;
                goLeft = true;
            } else if (sprite.X == 20)
            {
                goRight = true;
                goLeft = false;
            }
            if (sprite.Y == 500)
            {
                sprite = null;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Up:
                case Key.W:
                    earth.MoveY(-1.5);
                    break;

                case Key.Left:
                case Key.A:
                    earth.MoveX(-1.5);
                    break;

                case Key.Right:
                case Key.D:
                    earth.MoveX(1.5);
                    break;

                case Key.Down:
                case Key.S:
                    earth.MoveY(1.5);
                    break;
            }
        }
    }
}
