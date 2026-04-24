using System.Linq;
using System.Windows.Controls;
using FootballLeague.ViewModels;

namespace FootballLeague.Views.Components
{
    public partial class ChartsView : UserControl
    {
        public ChartsView()
        {
            InitializeComponent();
            TeamGraphListBox.SelectionChanged += TeamGraphListBox_SelectionChanged;
        }

        private void TeamGraphListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                viewModel.SelectedGraphTeams.Clear();
                foreach (Team team in TeamGraphListBox.SelectedItems)
                {
                    viewModel.SelectedGraphTeams.Add(team);
                }
            }
        }
    }
}
