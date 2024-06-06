using MazeGenerator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SaeDonjon.Model
{
    public class Donjon
    {
        // Représente le graphe du donjon avec les cellules et leurs voisins
        private Dictionary<MazeCell, Dictionary<MazeCell, int>> _graph;

        // Nom du donjon
        public string Name { get; set; }

        // Trésor du donjon
        public int Treasure { get; set; }

        // Représente le labyrinthe associé au donjon
        private Maze _maze;

        /// <summary>
        /// Constructeur par défaut du donjon.
        /// </summary>
        public Donjon()
        {
            Name = "Donjon";
            Treasure = 100;
        }

        /// <summary>
        /// Constructeur du donjon avec un labyrinthe donné.
        /// </summary>
        /// <param name="Maze">Le labyrinthe à utiliser pour construire le donjon.</param>
        public Donjon(Maze Maze)
        {
            _maze = Maze ?? throw new ArgumentNullException(nameof(Maze));
            BuildGraphFromMaze(Maze);
        }

        /// <summary>
        /// Construit le graphe du donjon à partir du labyrinthe donné.
        /// </summary>
        /// <param name="Maze">Le labyrinthe à partir duquel construire le graphe.</param>
        private void BuildGraphFromMaze(Maze Maze)
        {
            _graph = new Dictionary<MazeCell, Dictionary<MazeCell, int>>();

            foreach (var cell in Maze.MazeCells)
            {
                _graph[cell] = new Dictionary<MazeCell, int>();
                foreach (var direction in Enum.GetValues(typeof(Direction)).Cast<Direction>())
                {
                    if (!cell.HasWall(direction))
                    {
                        int weight = 1;
                        int neighborIndex = Maze.GetNeighbourIndex(cell, direction);
                        MazeCell neighbor = Maze.MazeCells[neighborIndex];
                        _graph[cell][neighbor] = weight;
                    }
                }
            }
        }

        /// <summary>
        /// Trouve le chemin le plus court entre deux cellules du labyrinthe.
        /// </summary>
        /// <param name="start">Cellule de départ.</param>
        /// <param name="end">Cellule d'arrivée.</param>
        /// <returns>Liste des cellules représentant le chemin le plus court.</returns>
        public List<MazeCell> FindShortestPath(MazeCell start, MazeCell end)
        {
            if (start == null)
                throw new ArgumentNullException(nameof(start), "La cellule de départ ne peut pas être nulle.");

            if (end == null)
                throw new ArgumentNullException(nameof(end), "La cellule d'arrivée ne peut pas être nulle.");

            if (_maze == null || _maze.MazeCells == null)
                throw new InvalidOperationException("Le labyrinthe n'est pas correctement initialisé.");

            var distances = new Dictionary<MazeCell, int>();
            var previous = new Dictionary<MazeCell, MazeCell>();
            var priorityQueue = new SortedSet<(int distance, MazeCell cell)>();

            foreach (var node in _graph.Keys)
            {
                distances[node] = int.MaxValue;
                priorityQueue.Add((int.MaxValue, node));
            }
            distances[start] = 0;
            priorityQueue.Remove((int.MaxValue, start));
            priorityQueue.Add((0, start));

            while (priorityQueue.Any())
            {
                var (currentDistance, celluleActuelle) = priorityQueue.Min;
                priorityQueue.Remove((currentDistance, celluleActuelle));

                if (celluleActuelle == end)
                {
                    var chemin = new List<MazeCell>();
                    while (previous.ContainsKey(celluleActuelle))
                    {
                        chemin.Add(celluleActuelle);
                        celluleActuelle = previous[celluleActuelle];
                    }
                    chemin.Reverse();
                    return chemin;
                }

                if (_graph.TryGetValue(celluleActuelle, out var neighbors))
                {
                    foreach (var kvp in neighbors)
                    {
                        var neighbor = kvp.Key;
                        var weight = kvp.Value;
                        int newDistance = currentDistance + weight;
                        if (newDistance < distances[neighbor])
                        {
                            priorityQueue.Remove((distances[neighbor], neighbor));
                            distances[neighbor] = newDistance;
                            previous[neighbor] = celluleActuelle;
                            priorityQueue.Add((newDistance, neighbor));
                        }
                    }
                }
            }
            return new List<MazeCell>(); // Retourne une liste vide si le chemin n'est pas trouvé
        }
    }
}
