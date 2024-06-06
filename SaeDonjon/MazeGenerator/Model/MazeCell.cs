using SaeDonjon.Common;
using System;

namespace SaeDonjon.Model
{
    /// <summary>
    /// La classe MazeCell représente une case unique du labyrinthe.
    /// </summary>
    public class MazeCell : NotificationBase, IComparable<MazeCell>
    {
        #region Champs

        private bool _northWall = true;

        private bool _eastWall = true;

        private bool _southWall = true;

        private bool _westWall = true;

        private CellState _cellState = CellState.Default;

        private CellType _cellType;

        private bool _containsDonjon = false;

        private bool _isPartOfPath = false;

        #endregion

        #region Constructeurs

        /// <summary>
        /// Constructeur par défaut.
        /// </summary>
        public MazeCell() { }

        #endregion

        #region Propriétés

        /// <summary>
        /// Propriété pour le mur nord.
        /// </summary>
        public bool NorthWall
        {
            get => _northWall;
            private set
            {
                _northWall = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Propriété pour le mur est.
        /// </summary>
        public bool EastWall
        {
            get => _eastWall;
            private set
            {
                _eastWall = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Propriété pour le mur sud.
        /// </summary>
        public bool SouthWall
        {
            get => _southWall;
            private set
            {
                _southWall = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Propriété pour le mur ouest.
        /// </summary>
        public bool WestWall
        {
            get => _westWall;
            private set
            {
                _westWall = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Propriété pour l'état de la cellule.
        /// </summary>
        public CellState CellState
        {
            get => _cellState;
            set
            {
                _cellState = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Propriété pour le type de la cellule.
        /// </summary>
        public CellType CellType
        {
            get => _cellType;
            set
            {
                _cellType = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Propriété pour indiquer si la cellule contient le donjon.
        /// </summary>
        public bool ContainsDonjon
        {
            get => _containsDonjon;
            set
            {
                _containsDonjon = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Propriété pour indiquer si la cellule fait partie du chemin.
        /// </summary>
        public bool IsPartOfPath
        {
            get => _isPartOfPath;
            set
            {
                if (_isPartOfPath != value)
                {
                    _isPartOfPath = value;
                    RaisePropertyChanged();
                }
            }
        }

        #endregion

        #region Méthodes

        /// <summary>
        /// Supprime un mur de la cellule dans la direction spécifiée.
        /// </summary>
        /// <param name="cellWall">La direction du mur à supprimer.</param>
        public void RemoveWall(Direction cellWall)
        {
            try
            {
                switch (cellWall)
                {
                    case Direction.North:
                        NorthWall = false;
                        break;
                    case Direction.East:
                        EastWall = false;
                        break;
                    case Direction.South:
                        SouthWall = false;
                        break;
                    case Direction.West:
                        WestWall = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("MazeCell.RemoveWall(Direction cellWall): " + ex.ToString());
            }
        }

        /// <summary>
        /// Restaure un mur de la cellule dans la direction spécifiée.
        /// </summary>
        /// <param name="cellWall">La direction du mur à restaurer.</param>
        public void RestoreWall(Direction cellWall)
        {
            try
            {
                switch (cellWall)
                {
                    case Direction.North:
                        NorthWall = true;
                        break;
                    case Direction.East:
                        EastWall = true;
                        break;
                    case Direction.South:
                        SouthWall = true;
                        break;
                    case Direction.West:
                        WestWall = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("MazeCell.RestoreWall(Direction cellWall): " + ex.ToString());
            }
        }

        /// <summary>
        /// Réinitialise la cellule à son état par défaut.
        /// </summary>
        public void ResetCell()
        {
            NorthWall = true;
            EastWall = true;
            SouthWall = true;
            WestWall = true;
            CellState = CellState.Default;
            CellType = CellType.Default;
        }

        /// <summary>
        /// Vérifie si la cellule a un mur dans la direction spécifiée.
        /// </summary>
        /// <param name="direction">La direction à vérifier.</param>
        /// <returns>Vrai si la cellule a un mur dans cette direction, sinon faux.</returns>
        public bool HasWall(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return NorthWall;
                case Direction.East:
                    return EastWall;
                case Direction.South:
                    return SouthWall;
                case Direction.West:
                    return WestWall;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        /// <summary>
        /// Propriété privée pour indiquer si la cellule fait partie du chemin.
        /// </summary>
        public bool _IsPartOfPath
        {
            get => _isPartOfPath;
            set
            {
                if (_isPartOfPath != value)
                {
                    _isPartOfPath = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Compare cette cellule avec une autre cellule.
        /// </summary>
        /// <param name="other">L'autre cellule à comparer.</param>
        /// <returns>Résultat de la comparaison.</returns>
        public int CompareTo(MazeCell other)
        {
            if (other == null) return 1;
            return this.GetHashCode().CompareTo(other.GetHashCode());
        }

        #endregion
    }
}
