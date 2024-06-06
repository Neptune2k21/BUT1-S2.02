using SaeDonjon.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SaeDonjon.Model
{
    public class Maze : NotificationBase
    {
        #region Champs

        // Collection de cellules du labyrinthe
        private ObservableCollection<MazeCell> _mazeCells;

        // Pile utilisée pour la génération du labyrinthe
        private Stack<MazeCell> _mazeGeneratorStack;

        private MazeState _mazeState = MazeState.Default;

        private Random _randomNumberGenerator;

        private Donjon _donjon;

        #endregion

        #region Constructeurs

        /// <summary>
        /// Constructeur par défaut du labyrinthe.
        /// </summary>
        public Maze()
        {
            try
            {
                _randomNumberGenerator = new Random();
                ResetMaze();
            }
            catch (Exception ex)
            {
                throw new Exception("Maze(): " + ex.ToString());
            }
        }

        #endregion

        #region Propriétés

        /// <summary>
        /// Largeur du labyrinthe en cellules.
        /// </summary>
        public int MazeWidthCells => Constants.MazeWidth;

        /// <summary>
        /// Hauteur du labyrinthe en cellules.
        /// </summary>
        public int MazeHeightCells => Constants.MazeHeight;

        /// <summary>
        /// Collection observable des cellules du labyrinthe.
        /// </summary>
        public ObservableCollection<MazeCell> MazeCells
        {
            get
            {
                if (_mazeCells == null)
                {
                    _mazeCells = new ObservableCollection<MazeCell>();
                    while (_mazeCells.Count != Constants.MazeWidth * Constants.MazeHeight)
                    {
                        _mazeCells.Add(new MazeCell());
                    }
                }
                return _mazeCells;
            }
            private set
            {
                _mazeCells = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// État actuel du labyrinthe.
        /// </summary>
        public MazeState MazeState
        {
            get => _mazeState;
            private set
            {
                _mazeState = value;
                RaisePropertyChanged();
                RaisePropertyChanged("CanGenerateMaze");
                RaisePropertyChanged("CanResetMaze");
            }
        }

        /// <summary>
        /// Indique si le labyrinthe peut être généré.
        /// </summary>
        public bool CanGenerateMaze => MazeState == MazeState.Default;

        /// <summary>
        /// Indique si le labyrinthe peut être réinitialisé.
        /// </summary>
        public bool CanResetMaze => MazeState == MazeState.MazeGenerated;

        #endregion

        #region Méthodes

        /// <summary>
        /// Réinitialise le labyrinthe.
        /// </summary>
        public void ResetMaze()
        {
            try
            {
                if (CanResetMaze)
                {
                    MazeCells = new ObservableCollection<MazeCell>();
                    while (MazeCells.Count != Constants.MazeWidth * Constants.MazeHeight)
                    {
                        MazeCells.Add(new MazeCell());
                    }
                    MazeState = MazeState.Default;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Maze.ResetMaze(): " + ex.ToString());
            }
        }

        /// <summary>
        /// Génère un nouveau labyrinthe de manière asynchrone.
        /// </summary>
        public async Task GenerateNewMaze()
        {
            try
            {
                if (CanGenerateMaze)
                {
                    MazeState = MazeState.MazeGenerating;
                    ResetMaze();
                    _mazeGeneratorStack = new Stack<MazeCell>();

                    MazeCell startCell = ChooseRandomCell();
                    startCell.CellState = CellState.Visited;

                    await GenerateNewMaze(startCell);

                    if (MazeCells.Count > 0)
                    {
                        MazeCells.First().CellType = CellType.Start;
                        MazeCells.Last().CellType = CellType.End;

                        PlaceDonjonAtStart();
                    }

                    MazeState = MazeState.MazeGenerated;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Maze.GenerateNewMaze(): " + ex.ToString());
            }
        }

        /// <summary>
        /// Génère un nouveau labyrinthe à partir de la cellule actuelle de manière asynchrone.
        /// </summary>
        /// <param name="currentCell">La cellule actuelle.</param>
        private async Task GenerateNewMaze(MazeCell currentCell)
        {
            try
            {
                await Task.Delay(Constants.MazeGenerationDelayMilliSeconds);

                if (MazeCells.Any(x => x.CellState == CellState.Default) || _mazeGeneratorStack.Count > 0)
                {
                    int cellIndex = MazeCells.IndexOf(currentCell);

                    int northNeighbourIndex = cellIndex - Constants.MazeWidth;
                    int eastNeighbourIndex = cellIndex + 1;
                    int southNeighbourIndex = cellIndex + Constants.MazeHeight;
                    int westNeighbourIndex = cellIndex - 1;

                    bool northEdge = cellIndex < Constants.MazeWidth;
                    bool eastEdge = ((cellIndex + 1) % Constants.MazeWidth) == 0;
                    bool westEdge = (cellIndex % Constants.MazeWidth) == 0;
                    bool southEdge = (cellIndex + Constants.MazeWidth) >= (Constants.MazeWidth * Constants.MazeHeight);

                    List<MazeCell> unvisitedNeighbours = new List<MazeCell>();
                    if (!northEdge && IsCellIndexValid(northNeighbourIndex) && MazeCells[northNeighbourIndex].CellState == CellState.Default)
                    {
                        unvisitedNeighbours.Add(MazeCells[northNeighbourIndex]);
                    }
                    if (!eastEdge && IsCellIndexValid(eastNeighbourIndex) && MazeCells[eastNeighbourIndex].CellState == CellState.Default)
                    {
                        unvisitedNeighbours.Add(MazeCells[eastNeighbourIndex]);
                    }
                    if (!southEdge && IsCellIndexValid(southNeighbourIndex) && MazeCells[southNeighbourIndex].CellState == CellState.Default)
                    {
                        unvisitedNeighbours.Add(MazeCells[southNeighbourIndex]);
                    }
                    if (!westEdge && IsCellIndexValid(westNeighbourIndex) && MazeCells[westNeighbourIndex].CellState == CellState.Default)
                    {
                        unvisitedNeighbours.Add(MazeCells[westNeighbourIndex]);
                    }

                    if (unvisitedNeighbours.Count > 0)
                    {
                        MazeCell selectedNeighbour = unvisitedNeighbours.ElementAt(_randomNumberGenerator.Next(unvisitedNeighbours.Count));

                        int selectedNeightbourIndex = MazeCells.IndexOf(selectedNeighbour);
                        if (selectedNeightbourIndex == northNeighbourIndex)
                        {
                            currentCell.RemoveWall(Direction.North);
                            selectedNeighbour.RemoveWall(Direction.South);
                        }
                        else if (selectedNeightbourIndex == eastNeighbourIndex)
                        {
                            currentCell.RemoveWall(Direction.East);
                            selectedNeighbour.RemoveWall(Direction.West);
                        }
                        else if (selectedNeightbourIndex == southNeighbourIndex)
                        {
                            currentCell.RemoveWall(Direction.South);
                            selectedNeighbour.RemoveWall(Direction.North);
                        }
                        else if (selectedNeightbourIndex == westNeighbourIndex)
                        {
                            currentCell.RemoveWall(Direction.West);
                            selectedNeighbour.RemoveWall(Direction.East);
                        }

                        _mazeGeneratorStack.Push(currentCell);
                        selectedNeighbour.CellState = CellState.Visited;

                        await GenerateNewMaze(selectedNeighbour);
                    }
                    else
                    {
                        currentCell.CellState = CellState.Empty;

                        MazeCell previousCell = _mazeGeneratorStack.Pop();
                        previousCell.CellState = CellState.Empty;

                        await GenerateNewMaze(previousCell);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Maze.GenerateNewMaze(MazeCell currentCell): " + ex.ToString());
            }
        }

        /// <summary>
        /// Génère un labyrinthe imparfait avec un certain nombre de boucles de manière asynchrone.
        /// </summary>
        /// <param name="numberOfLoopsToAdd">Nombre de boucles à ajouter.</param>
        public async Task GenerateImperfectMaze(int numberOfLoopsToAdd)
        {
            try
            {
                await GenerateNewMaze();
                AddLoopsToMaze(numberOfLoopsToAdd);
            }
            catch (Exception ex)
            {
                throw new Exception("Maze.GenerateImperfectMaze(): " + ex.ToString());
            }
        }

        /// <summary>
        /// Ajoute des boucles au labyrinthe.
        /// </summary>
        /// <param name="numberOfLoopsToAdd">Nombre de boucles à ajouter.</param>
        private void AddLoopsToMaze(int numberOfLoopsToAdd)
        {
            for (int i = 0; i < numberOfLoopsToAdd; i++)
            {
                MazeCell cell = ChooseRandomCell();
                List<Direction> directions = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();

                while (directions.Count > 0)
                {
                    Direction direction = directions.ElementAt(_randomNumberGenerator.Next(directions.Count));
                    directions.Remove(direction);

                    int neighbourIndex = GetNeighbourIndex(cell, direction);
                    if (IsCellIndexValid(neighbourIndex))
                    {
                        MazeCell neighbour = MazeCells[neighbourIndex];
                        if (cell.HasWall(direction) && neighbour.HasWall(GetOppositeDirection(direction)))
                        {
                            cell.RemoveWall(direction);
                            neighbour.RemoveWall(GetOppositeDirection(direction));
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Obtient l'index du voisin d'une cellule dans une direction donnée.
        /// </summary>
        /// <param name="cell">La cellule dont on veut obtenir le voisin.</param>
        /// <param name="direction">La direction du voisin.</param>
        /// <returns>L'index du voisin.</returns>
        public int GetNeighbourIndex(MazeCell cell, Direction direction)
        {
            int index = MazeCells.IndexOf(cell);
            switch (direction)
            {
                case Direction.North:
                    return index - Constants.MazeWidth;
                case Direction.East:
                    return index + 1;
                case Direction.South:
                    return index + Constants.MazeWidth;
                case Direction.West:
                    return index - 1;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Obtient la direction opposée à une direction donnée.
        /// </summary>
        /// <param name="direction">La direction.</param>
        /// <returns>La direction opposée.</returns>
        private Direction GetOppositeDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return Direction.South;
                case Direction.East:
                    return Direction.West;
                case Direction.South:
                    return Direction.North;
                case Direction.West:
                    return Direction.East;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Choisit une cellule aléatoire dans le labyrinthe.
        /// </summary>
        /// <returns>La cellule aléatoire choisie.</returns>
        private MazeCell ChooseRandomCell()
        {
            int cellIndex = _randomNumberGenerator.Next(Constants.MazeHeight * Constants.MazeWidth);
            if (IsCellIndexValid(cellIndex))
            {
                return MazeCells.ElementAt(cellIndex);
            }
            else
            {
                throw new Exception("Impossible de choisir une cellule aléatoire.");
            }
        }

        /// <summary>
        /// Vérifie si un index de cellule est valide.
        /// </summary>
        /// <param name="cellIndex">L'index de la cellule.</param>
        /// <returns>Vrai si l'index est valide, sinon faux.</returns>
        private bool IsCellIndexValid(int cellIndex)
        {
            return cellIndex >= 0 && cellIndex < MazeCells.Count;
        }

        /// <summary>
        /// Place le Donjon à la cellule de départ.
        /// </summary>
        private void PlaceDonjonAtStart()
        {
            var startCell = MazeCells.First();
            _donjon = new Donjon();
            startCell.CellType = CellType.Donjon;
        }

        #endregion
    }
}
