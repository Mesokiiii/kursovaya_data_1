using System.Windows;
using System.Windows.Controls;

namespace FootballLeague
{
    public partial class GenerateDataDialog : Window
    {
        public int TeamCount { get; private set; }
        public int SimulateRounds { get; private set; }

        public GenerateDataDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (TeamCountComboBox.SelectedItem is ComboBoxItem teamItem &&
                SimulateRoundsComboBox.SelectedItem is ComboBoxItem roundItem)
            {
                TeamCount = int.Parse(teamItem.Tag.ToString());
                SimulateRounds = int.Parse(roundItem.Tag.ToString());
                
                DialogResult = true;
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
