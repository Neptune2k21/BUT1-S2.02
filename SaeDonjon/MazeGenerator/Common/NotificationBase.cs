using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SaeDonjon.Common
{
    /// <summary>
    /// La classe NotificationBase représente une implémentation de l'interface INotifyPropertyChanged.
    /// Elle est utilisée par les classes qui ont besoin de déclencher un événement de changement de propriété.
    /// </summary>
    public class NotificationBase : INotifyPropertyChanged
    {
        #region Champs
        #endregion

        #region Constructeurs
        #endregion

        #region Événements

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Propriétés
        #endregion

        #region Méthodes

        /// <summary>
        /// La méthode RaisePropertyChanged est appelée pour déclencher un événement de changement de propriété.
        /// </summary>
        /// <param name="propertyname">Nom de la propriété ayant changé.</param>
        protected void RaisePropertyChanged([CallerMemberName] string propertyname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        #endregion
    }
}
