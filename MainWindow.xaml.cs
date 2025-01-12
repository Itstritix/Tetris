using System;
using System.Diagnostics;     // Pour Stopwatch
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading; // Pour DispatcherTimer
using Tetris.Blocks;

namespace Tetris
{
    public partial class MainWindow : Window
    {
        // Images tuiles
        private readonly ImageSource[] tileImages =
        {
            new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileRed.png", UriKind.Relative))
        };

        // Images blocs
        private readonly ImageSource[] blockImages =
        {
            new BitmapImage(new Uri("Assets/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative))
        };

        // Matrice d'images pour le Canvas
        private readonly Image[,] imageControls;

        // Paramètres de vitesse du jeu
        private readonly int maxDelay = 1000;
        private readonly int minDelay = 75;
        private readonly int delayDecrease = 25;

        // État du jeu
        private GameState gameState = new GameState();

        // ===============================
        // =========== TIMER =============
        // ===============================
        private DispatcherTimer gameClockTimer; // Pour rafraîchir l'affichage du temps
        private Stopwatch stopwatch;            // Chronomètre pour mesurer le temps écoulé

        public MainWindow()
        {
            InitializeComponent();

            // Initialisation du canvas
            imageControls = SetupGameCanvas(gameState.GameGrid);

            // Initialisation du timer
            gameClockTimer = new DispatcherTimer();
            gameClockTimer.Interval = TimeSpan.FromSeconds(1);
            gameClockTimer.Tick += GameClockTimer_Tick;
        }

        private Image[,] SetupGameCanvas(GameGrid grid)
        {
            Image[,] controls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };

                    // Position
                    Canvas.SetTop(imageControl, (r - 2) * cellSize + 10);
                    Canvas.SetLeft(imageControl, c * cellSize);

                    GameCanvas.Children.Add(imageControl);
                    controls[r, c] = imageControl;
                }
            }

            return controls;
        }

        // ===============================
        // ========== ÉVÉNEMENTS =========
        // ===============================

        private async void Play_Click(object sender, RoutedEventArgs e)
        {
            // On cache le menu de démarrage
            StartMenu.Visibility = Visibility.Hidden;

            // Réinitialise l'état du jeu
            gameState = new GameState();

            // (Ré)initialiser le chrono
            stopwatch = new Stopwatch();
            stopwatch.Start();

            // Lance le timer qui rafraîchit le TimerText
            gameClockTimer.Start();

            // Lance la boucle de jeu
            await GameLoop();
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            // Masque le menu Game Over
            GameOverMenu.Visibility = Visibility.Hidden;

            // Réinitialiser l'état du jeu
            gameState = new GameState();

            // Remettre le chrono à zéro
            stopwatch = new Stopwatch();
            stopwatch.Start();
            gameClockTimer.Start();

            // Relancer la boucle
            await GameLoop();
        }

        // ===============================
        // ========== BOUCLE JEU =========
        // ===============================
        private async Task GameLoop()
        {
            Draw(gameState);

            while (!gameState.GameOver)
            {
                int delay = Math.Max(minDelay, maxDelay - (gameState.Score * delayDecrease));
                await Task.Delay(delay);

                gameState.MoveBlockDown();
                Draw(gameState);
            }

            // Quand la partie est terminée
            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreText.Text = $"Score: {gameState.Score}";

            // Arrêter le chrono
            stopwatch.Stop();
            gameClockTimer.Stop();
        }

        // ===============================
        // == DESSIN & MISE À JOUR UI ===
        // ===============================
        private void Draw(GameState state)
        {
            DrawGrid(state.GameGrid);
            DrawGhostBlock(state.CurrentBlock);
            DrawBlock(state.CurrentBlock);
            DrawNextBlock(state.BlockQueue);
            DrawHeldBlock(state.HeldBlock);

            ScoreText.Text = $"Score: {state.Score}";
        }

        private void DrawGrid(GameGrid grid)
        {
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r, c];
                    imageControls[r, c].Opacity = 1;
                    imageControls[r, c].Source = tileImages[id];
                }
            }
        }

        private void DrawBlock(Block block)
        {
            foreach (Position p in block.TilePositions())
            {
                imageControls[p.Row, p.Column].Opacity = 1;
                imageControls[p.Row, p.Column].Source = tileImages[block.Id];
            }
        }

        private void DrawNextBlock(BlockQueue blockQueue)
        {
            Block next = blockQueue.NextBlock;
            NextImage.Source = blockImages[next.Id];
        }

        private void DrawHeldBlock(Block heldBlock)
        {
            if (heldBlock == null)
            {
                HoldImage.Source = blockImages[0];
            }
            else
            {
                HoldImage.Source = blockImages[heldBlock.Id];
            }
        }

        private void DrawGhostBlock(Block block)
        {
            int dropDistance = gameState.BlockDropDistance();
            foreach (Position p in block.TilePositions())
            {
                imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
                imageControls[p.Row + dropDistance, p.Column].Source = tileImages[block.Id];
            }
        }

        // ===============================
        // ====== MISE À JOUR TIMER ======
        // ===============================
        private void GameClockTimer_Tick(object sender, EventArgs e)
        {
            if (stopwatch != null && stopwatch.IsRunning)
            {
                // Format "mm:ss"
                TimerText.Text = stopwatch.Elapsed.ToString(@"mm\:ss");
            }
        }

        // ===============================
        // ====== GESTION DU CLAVIER =====
        // ===============================
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.Left:
                    gameState.MoveBlockLeft();
                    break;
                case Key.Right:
                    gameState.MoveBlockRight();
                    break;
                case Key.Down:
                    gameState.MoveBlockDown();
                    break;
                case Key.Up:
                    gameState.RotateBlockCW();
                    break;
                case Key.Z:
                    gameState.RotateBlockCCW();
                    break;
                case Key.C:
                    gameState.HoldBlock();
                    break;
                case Key.Space:
                    gameState.DropBlock();
                    break;
                default:
                    return;
            }

            Draw(gameState);
        }
    }
}
