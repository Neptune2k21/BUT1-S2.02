using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;

namespace SaeDonjon.View
{
    /// <summary>
    /// La classe BooleanToColorConverter convertit une valeur booléenne en couleur.
    /// </summary>
    public class BooleanToColorConverter : IValueConverter
    {
        /// <summary>
        /// Convertit une valeur booléenne en couleur. Si la valeur est true, retourne la couleur jaune.
        /// </summary>
        /// <param name="value">La valeur à convertir.</param>
        /// <param name="targetType">Le type cible de la conversion.</param>
        /// <param name="parameter">Paramètre de conversion facultatif.</param>
        /// <param name="culture">Culture utilisée pour la conversion.</param>
        /// <returns>Retourne Brushes.Yellow si la valeur est true, sinon retourne DependencyProperty.UnsetValue.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isPartOfPath && isPartOfPath)
            {
                return Brushes.Yellow;
            }
            return DependencyProperty.UnsetValue;
        }

        /// <summary>
        /// Conversion inverse non implémentée.
        /// </summary>
        /// <param name="value">La valeur à convertir en arrière.</param>
        /// <param name="targetType">Le type cible de la conversion.</param>
        /// <param name="parameter">Paramètre de conversion facultatif.</param>
        /// <param name="culture">Culture utilisée pour la conversion.</param>
        /// <returns>Retourne une exception NotImplementedException.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
