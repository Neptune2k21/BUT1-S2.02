using System;
using System.Windows.Input;

namespace SaeDonjon.Common
{
    /// <summary>
    /// La classe DelegateCommand représente une implémentation de l'interface ICommand.
    /// Elle est utilisée par les classes qui ont besoin d'exécuter des commandes.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        #region Champs

        private Action<object> _execute;
        private Func<object, bool> _canExecute;

        #endregion

        #region Constructeurs

        /// <summary>
        /// Constructeur.
        /// Crée une nouvelle DelegateCommand avec l'action fournie.
        /// </summary>
        /// <param name="execute">Action à exécuter.</param>
        public DelegateCommand(Action<object> execute)
        {
            _execute = execute;
            _canExecute = (x) => { return true; };
        }

        /// <summary>
        /// Constructeur.
        /// Crée une nouvelle DelegateCommand avec l'action et la condition fournies.
        /// </summary>
        /// <param name="execute">Action à exécuter.</param>
        /// <param name="canExecute">Fonction déterminant si l'action peut être exécutée.</param>
        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion

        #region Événements

        public event EventHandler CanExecuteChanged;

        #endregion

        #region Propriétés
        #endregion

        #region Méthodes

        /// <summary>
        /// La méthode CanExecute est appelée pour indiquer si cette commande peut être exécutée.
        /// </summary>
        /// <param name="parameter">Paramètre de la commande.</param>
        /// <returns>Retourne vrai si la commande peut être exécutée, sinon faux.</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        /// <summary>
        /// La méthode RaiseCanExecuteChanged est appelée pour évaluer si cette commande peut être exécutée.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// La commande Execute est appelée pour exécuter cette commande.
        /// </summary>
        /// <param name="parameter">Paramètre de la commande.</param>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        #endregion
    }
}
