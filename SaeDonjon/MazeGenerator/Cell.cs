using SaeDonjon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGenerator
{
    /// <summary>
    /// La classe Cell représente une cellule dans le générateur de labyrinthe.
    /// </summary>
    public class Cell
    {
        public int X { get; }
        public int Y { get; }
        public CellState State { get; set; }

        /// <summary>
        /// Constructeur de la classe Cell.
        /// </summary>
        /// <param name="x">Coordonnée X de la cellule.</param>
        /// <param name="y">Coordonnée Y de la cellule.</param>
        public Cell(int x, int y)
        {
            X = x;
            Y = y;
            State = CellState.Default;
        }
    }
}