using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.IO;
using System.Threading.Tasks;

namespace FootballLeague
{
    public partial class MainWindow : Window
    {
        private LeagueManager leagueManager;

        public MainWindow()
        {
            InitializeComponent();
            InitializeLeague();
        }

        private void InitializeLeague()
        {
            leagueManager = new LeagueManager();
            
            try
            {
                // Ищем data.json в нескольких местах
                string[] possiblePaths = new string[]
                {
                    System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "project", "Data", "data.json"),
                    System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "project", "Data", "data.json"),
                    "project/Data/data.json"
                };

                string dataPath = null;
                foreach (var path in possiblePaths)
                {
                    string fullPath = System.IO.Path.GetFullPath(path);
                    if (File.Exists(fullPath))
                    {
                        dataPath = fullPath;
                        break;
                    }
                }

                if (dataPath != null)
                {
                    DataLoader.LoadFromJson(leagueManager, dataPath);
                }
                else
                {
                    MessageBox.Show("Файл data.json не найден в:\n" + string.Join("\n", possiblePaths) + "\n\nИспользуются тестовые данные.", "Предупреждение");
                    leagueManager.GenerateMockData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка");
                leagueManager.GenerateMockData();
            }
            
            RefreshStandings();
            PopulateTeamComboBoxes();
            RefreshPrediction();
        }

        private void RefreshStandings()
        {
            var standings = leagueManager.GetStandings();
            StandingsGrid.ItemsSource = new ObservableCollection<Team>(standings);
        }

        private void PopulateTeamComboBoxes()
        {
            var teams = leagueManager.Teams.OrderBy(t => t.Name).ToList();
            Team1ComboBox.ItemsSource = teams;
            Team2ComboBox.ItemsSource = teams;
            Team1ComboBox.DisplayMemberPath = "Name";
            Team2ComboBox.DisplayMemberPath = "Name";
            
            TeamHistoryComboBox.ItemsSource = teams;
            TeamHistoryComboBox.DisplayMemberPath = "Name";
            
            TeamGraphComboBox.ItemsSource = teams;
            TeamGraphComboBox.DisplayMemberPath = "Name";
        }

        private void SearchHeadToHeadButton_Click(object sender, RoutedEventArgs e)
        {
            if (Team1ComboBox.SelectedItem is Team team1 && Team2ComboBox.SelectedItem is Team team2)
            {
                if (team1.Id == team2.Id)
                {
                    MessageBox.Show("Выберите разные команды", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                ShowLoading("Ищем личные встречи...");
                Task.Run(() =>
                {
                    var headToHead = leagueManager.GetHeadToHead(team1, team2);
                    Dispatcher.Invoke(() =>
                    {
                        HeadToHeadGrid.ItemsSource = new ObservableCollection<Match>(headToHead.OrderBy(m => m.Round));
                        HideLoading();
                    });
                });
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(MinGoalsTextBox.Text, out int minGoals))
            {
                ShowLoading("Ищем команды...");
                Task.Run(() =>
                {
                    var results = leagueManager.SearchTeams(t => t.GoalsFor >= minGoals);
                    Dispatcher.Invoke(() =>
                    {
                        SearchGrid.ItemsSource = new ObservableCollection<Team>(results.OrderByDescending(t => t.GoalsFor));
                        HideLoading();
                    });
                });
            }
            else
            {
                MessageBox.Show("Введите корректное число", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RefreshPrediction()
        {
            var prediction = leagueManager.GetStandings();
            PredictionGrid.ItemsSource = new ObservableCollection<Team>(prediction);
        }

        private void ShowHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (TeamHistoryComboBox.SelectedItem is Team team)
            {
                ShowLoading("Загружаем историю...");
                Task.Run(() =>
                {
                    var history = leagueManager.GetTeamHistory(team.Id);
                    Dispatcher.Invoke(() =>
                    {
                        HistoryGrid.ItemsSource = new ObservableCollection<TeamRoundStats>(history);
                        HideLoading();
                    });
                });
            }
        }

        private void ShowGraphButton_Click(object sender, RoutedEventArgs e)
        {
            if (TeamGraphComboBox.SelectedItem is Team team)
            {
                ShowLoading("Генерируем график...");
                Task.Run(() =>
                {
                    var history = leagueManager.GetTeamHistory(team.Id);
                    Dispatcher.Invoke(() =>
                    {
                        if (history.Count > 0)
                        {
                            PointsPlot.Model = ChartHelper.CreatePointsChart(history, team.Name);
                        }
                        else
                        {
                            MessageBox.Show("Нет данных для этой команды", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        HideLoading();
                    });
                });
            }
        }

        private void ShowLoading(string message)
        {
            LoadingText.Text = message;
            LoadingPanel.Visibility = Visibility.Visible;
        }

        private void HideLoading()
        {
            LoadingPanel.Visibility = Visibility.Collapsed;
        }
    }
}
