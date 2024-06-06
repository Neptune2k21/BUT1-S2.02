using SaeDonjon.ViewModel;
using System.Windows;

namespace SaeDonjon.View
{
    public partial class MazeGeneratorView : Window
    {
        public MazeGeneratorView()
        {
            InitializeComponent();
            DataContext = new MazeGeneratorViewModel();            
        }

        private void FindShortestPathButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MazeGeneratorViewModel;
            viewModel?.FindShortestPathCommand.Execute(null);
        }
    }
}
