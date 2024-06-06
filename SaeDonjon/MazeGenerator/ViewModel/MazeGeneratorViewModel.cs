using SaeDonjon.Common;
using SaeDonjon.Model;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SaeDonjon.ViewModel
{
    public class MazeGeneratorViewModel : NotificationBase
    {
        #region Champs

        // Indique si le labyrinthe est imparfait
        private bool _isImperfect = false;

        private Donjon donjon;

        private MazeCell _startCell;

        private MazeCell _endCell;

        #endregion

        #region Constructeurs

        /// <summary>
        /// Constructeur par défaut de MazeGeneratorViewModel.
        /// </summary>
        public MazeGeneratorViewModel()
        {
            try
            {
                Maze = new Maze();
                donjon = new Donjon(Maze);
                Maze.PropertyChanged += OnMazePropertyChanged;

                GenerateMazeCommand = new DelegateCommand(OnGenerateMaze, CanGenerateMaze);
                ResetMazeCommand = new DelegateCommand(OnResetMaze, CanResetMaze);
                GenerateImperfectMazeCommand = new DelegateCommand(OnGenerateImperfectMaze, CanGenerateMaze);
                FindShortestPathCommand = new DelegateCommand(OnFindShortestPath, CanFindShortestPath);

                InitializeStartAndEndCells();
            }
            catch (Exception ex)
            {
                throw new Exception("MazeGeneratorViewModel(): " + ex.ToString());
            }
        }

        #endregion

        #region Propriétés

        /// <summary>
        /// Cellule de départ du labyrinthe.
        /// </summary>
        public MazeCell StartCell
        {
            get { return _startCell; }
            set
            {
                if (_startCell != value)
                {
                    _startCell = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Cellule de fin du labyrinthe.
        /// </summary>
        public MazeCell EndCell
        {
            get { return _endCell; }
            set
            {
                if (_endCell != value)
                {
                    _endCell = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Instance du labyrinthe.
        /// </summary>
        public Maze Maze { get; }

        /// <summary>
        /// Largeur du labyrinthe.
        /// </summary>
        public int MazeWidth => Constants.MazeWidth;

        /// <summary>
        /// Hauteur du labyrinthe.
        /// </summary>
        public int MazeHeight => Constants.MazeHeight;

        /// <summary>
        /// Indique si le labyrinthe est imparfait.
        /// </summary>
        public bool IsImperfect
        {
            get => _isImperfect;
            set
            {
                if (_isImperfect != value)
                {
                    _isImperfect = value;
                    RaisePropertyChanged();
                }
            }
        }

        // Commandes pour générer, réinitialiser et "ESSAIE de" trouver le chemin le plus court dans le labyrinthe
        public DelegateCommand GenerateMazeCommand { get; private set; }
        public DelegateCommand ResetMazeCommand { get; private set; }
        public DelegateCommand GenerateImperfectMazeCommand { get; private set; }
        public DelegateCommand FindShortestPathCommand { get; private set; }

        #endregion

        #region Méthodes

        /// <summary>
        /// Génère un nouveau labyrinthe de manière asynchrone.
        /// </summary>
        public async void OnGenerateMaze(object arg)
        {
            try
            {
                if (Maze != null)
                {
                    await Maze.GenerateNewMaze();
                    InitializeStartAndEndCells();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("MazeGeneratorViewModel.OnGenerateMaze(object arg): " + ex.ToString());
            }
        }

        /// <summary>
        /// Génère un labyrinthe imparfait de manière asynchrone.
        /// </summary>
        public async void OnGenerateImperfectMaze(object arg)
        {
            try
            {
                if (Maze != null)
                {
                    await Maze.GenerateImperfectMaze(numberOfLoopsToAdd: 20);
                    InitializeStartAndEndCells();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("MazeGeneratorViewModel.OnGenerateImperfectMaze(object arg): " + ex.ToString());
            }
        }

        /// <summary>
        /// Vérifie si la commande pour trouver le chemin le plus court peut être exécutée.
        /// </summary>
        public bool CanFindShortestPath(object arg)
        {
            return Maze != null && donjon != null && _startCell != null && _endCell != null;
        }

        /// <summary>
        /// Trouve le chemin le plus court entre la cellule de départ et la cellule de fin.
        /// </summary>
        public void OnFindShortestPath(object arg)
        {
            try
            {
                if (StartCell == null || EndCell == null)
                {
                    throw new InvalidOperationException("StartCell ou EndCell est null.");
                }

                var path = donjon.FindShortestPath(_startCell, _endCell);

                foreach (var cell in Maze.MazeCells)
                {
                    cell.IsPartOfPath = false;
                }

                if (path != null)
                {
                    foreach (var cell in path)
                    {
                        cell.IsPartOfPath = true;
                    }
                }

                RaisePropertyChanged(nameof(Maze));
            }
            catch (Exception ex)
            {
                throw new Exception("MazeGeneratorViewModel.OnFindShortestPath(object arg): " + ex.ToString());
            }
        }

        /// <summary>
        /// Vérifie si la commande pour générer le labyrinthe peut être exécutée.
        /// </summary>
        public bool CanGenerateMaze(object arg)
        {
            return Maze != null && Maze.CanGenerateMaze;
        }

        /// <summary>
        /// Réinitialise le labyrinthe.
        /// </summary>
        public void OnResetMaze(object arg)
        {
            try
            {
                if (Maze != null)
                {
                    Maze.ResetMaze();
                    InitializeStartAndEndCells();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("MazeGeneratorViewModel.OnResetMaze(object arg): " + ex.ToString());
            }
        }

        /// <summary>
        /// Vérifie si la commande pour réinitialiser le labyrinthe peut être exécutée.
        /// </summary>
        public bool CanResetMaze(object arg)
        {
            return Maze != null && Maze.CanResetMaze;
        }

        /// <summary>
        /// Gestionnaire d'événements pour les changements de propriété du labyrinthe.
        /// </summary>
        private void OnMazePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                GenerateMazeCommand.RaiseCanExecuteChanged();
                ResetMazeCommand.RaiseCanExecuteChanged();
                FindShortestPathCommand.RaiseCanExecuteChanged();
            }
            catch (Exception ex)
            {
                throw new Exception("MazeGeneratorViewModel.OnMazePropertyChanged(object sender, PropertyChangedEventArgs e): " + ex.ToString());
            }
        }

        /// <summary>
        /// Initialise les cellules de départ et de fin du labyrinthe.
        /// </summary>
        private void InitializeStartAndEndCells()
        {
            if (Maze.MazeCells == null || Maze.MazeCells.Count == 0)
                return;

            _startCell = Maze.MazeCells.First();
            if (StartCell != null)
                StartCell.CellType = CellType.Start;

            _endCell = Maze.MazeCells.Last();
            if (EndCell != null)
                EndCell.CellType = CellType.End;
        }

        #endregion
    }
}
