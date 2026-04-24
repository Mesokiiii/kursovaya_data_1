using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Threading.Tasks;
using OxyPlot;

namespace FootballLeague.ViewModels
{
    /// <summary>
    /// Главная ViewModel для MainWindow
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly LeagueManager _leagueManager;

        #region Properties

        private ObservableCollection<Team> _standings;
        public ObservableCollection<Team> Standings
        {
            get => _standings;
            set => SetProperty(ref _standings, value);
        }

        private ObservableCollection<Match> _headToHeadMatches;
        public ObservableCollection<Match> HeadToHeadMatches
        {
            get => _headToHeadMatches;
            set => SetProperty(ref _headToHeadMatches, value);
        }

        private ObservableCollection<Team> _searchResults;
        public ObservableCollection<Team> SearchResults
        {
            get => _searchResults;
            set => SetProperty(ref _searchResults, value);
        }

        private ObservableCollection<Match> _remainingMatches;
        public ObservableCollection<Match> RemainingMatches
        {
            get => _remainingMatches;
            set => SetProperty(ref _remainingMatches, value);
        }

        private ObservableCollection<Team> _predictionStandings;
        public ObservableCollection<Team> PredictionStandings
        {
            get => _predictionStandings;
            set => SetProperty(ref _predictionStandings, value);
        }

        private ObservableCollection<TeamRoundStats> _teamHistory;
        public ObservableCollection<TeamRoundStats> TeamHistory
        {
            get => _teamHistory;
            set => SetProperty(ref _teamHistory, value);
        }

        private ObservableCollection<Team> _teams;
        public ObservableCollection<Team> Teams
        {
            get => _teams;
            set => SetProperty(ref _teams, value);
        }

        private Team _selectedTeam1;
        public Team SelectedTeam1
        {
            get => _selectedTeam1;
            set => SetProperty(ref _selectedTeam1, value);
        }

        private Team _selectedTeam2;
        public Team SelectedTeam2
        {
            get => _selectedTeam2;
            set => SetProperty(ref _selectedTeam2, value);
        }

        private Team _selectedHistoryTeam;
        public Team SelectedHistoryTeam
        {
            get => _selectedHistoryTeam;
            set => SetProperty(ref _selectedHistoryTeam, value);
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private string _loadingText;
        public string LoadingText
        {
            get => _loadingText;
            set => SetProperty(ref _loadingText, value);
        }

        private PlotModel _chartModel;
        public PlotModel ChartModel
        {
            get => _chartModel;
            set => SetProperty(ref _chartModel, value);
        }

        private string _graphInfoText;
        public string GraphInfoText
        {
            get => _graphInfoText;
            set => SetProperty(ref _graphInfoText, value);
        }

        // Search criteria properties
        private string _minGoalsText;
        public string MinGoalsText
        {
            get => _minGoalsText;
            set => SetProperty(ref _minGoalsText, value);
        }

        private string _maxGoalsText;
        public string MaxGoalsText
        {
            get => _maxGoalsText;
            set => SetProperty(ref _maxGoalsText, value);
        }

        private string _minPointsText;
        public string MinPointsText
        {
            get => _minPointsText;
            set => SetProperty(ref _minPointsText, value);
        }

        private string _minWinsText;
        public string MinWinsText
        {
            get => _minWinsText;
            set => SetProperty(ref _minWinsText, value);
        }

        private string _minGoalDiffText;
        public string MinGoalDiffText
        {
            get => _minGoalDiffText;
            set => SetProperty(ref _minGoalDiffText, value);
        }

        private string _nameContainsText;
        public string NameContainsText
        {
            get => _nameContainsText;
            set => SetProperty(ref _nameContainsText, value);
        }

        private ObservableCollection<Team> _selectedGraphTeams;
        public ObservableCollection<Team> SelectedGraphTeams
        {
            get => _selectedGraphTeams;
            set => SetProperty(ref _selectedGraphTeams, value);
        }

        #endregion

        #region Commands

        public ICommand SearchHeadToHeadCommand { get; }
        public ICommand SearchTeamsCommand { get; }
        public ICommand ClearSearchCommand { get; }
        public ICommand CalculatePredictionCommand { get; }
        public ICommand ResetPredictionCommand { get; }
        public ICommand ShowHistoryCommand { get; }
        public ICommand ShowPositionGraphCommand { get; }
        public ICommand ShowPointsGraphCommand { get; }
        public ICommand GenerateDataCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand AboutCommand { get; }

        #endregion

        public MainViewModel()
        {
            _leagueManager = new LeagueManager();
            SelectedGraphTeams = new ObservableCollection<Team>();
            GraphInfoText = "Выберите команды и нажмите кнопку для отображения графика";

            // Initialize commands
            SearchHeadToHeadCommand = new RelayCommand(async _ => await SearchHeadToHeadAsync(), _ => CanSearchHeadToHead());
            SearchTeamsCommand = new RelayCommand(async _ => await SearchTeamsAsync());
            ClearSearchCommand = new RelayCommand(_ => ClearSearch());
            CalculatePredictionCommand = new RelayCommand(async _ => await CalculatePredictionAsync());
            ResetPredictionCommand = new RelayCommand(_ => ResetPrediction());
            ShowHistoryCommand = new RelayCommand(async _ => await ShowHistoryAsync(), _ => SelectedHistoryTeam != null);
            ShowPositionGraphCommand = new RelayCommand(async _ => await ShowPositionGraphAsync());
            ShowPointsGraphCommand = new RelayCommand(async _ => await ShowPointsGraphAsync());
            GenerateDataCommand = new RelayCommand(_ => GenerateData());
            ExitCommand = new RelayCommand(_ => Application.Current.Shutdown());
            AboutCommand = new RelayCommand(_ => ShowAbout());

            // ВЫЗОВИТЕ ЭТО ОДИН РАЗ, ЧТОБЫ СДЕЛАТЬ СКРИНШОТ КОНСОЛИ
            PerformanceTester.RunAllTests();

            InitializeLeague();
        }

        private void InitializeLeague()
        {
            try
            {
                string dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "data.json");

                if (File.Exists(dataPath))
                {
                    DataLoader.LoadFromJson(_leagueManager, dataPath);
                }
                else
                {
                    MessageBox.Show($"Файл data.json не найден по пути:\n{dataPath}\n\nИспользуются тестовые данные.", "Предупреждение");
                    _leagueManager.GenerateMockData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка");
                _leagueManager.GenerateMockData();
            }

            RefreshStandings();
            PopulateTeams();
            RefreshPrediction();
        }

        private void RefreshStandings()
        {
            var standings = _leagueManager.GetStandings();
            Standings = new ObservableCollection<Team>(standings);
        }

        private void PopulateTeams()
        {
            List<Team> sortedTeams = ManualAlgorithms.OrderBy(_leagueManager.Teams, t => t.Name);
            Teams = new ObservableCollection<Team>(sortedTeams);
        }

        private bool CanSearchHeadToHead()
        {
            return SelectedTeam1 != null && SelectedTeam2 != null && SelectedTeam1.Id != SelectedTeam2.Id;
        }

        private async Task SearchHeadToHeadAsync()
        {
            if (SelectedTeam1 == null || SelectedTeam2 == null || SelectedTeam1.Id == SelectedTeam2.Id)
            {
                MessageBox.Show("Выберите разные команды", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ShowLoading("Ищем личные встречи...");
            await Task.Run(() =>
            {
                var headToHead = _leagueManager.GetHeadToHead(SelectedTeam1, SelectedTeam2);
                List<Match> sortedMatches = ManualAlgorithms.OrderBy(headToHead, m => m.Round);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    HeadToHeadMatches = new ObservableCollection<Match>(sortedMatches);
                    HideLoading();
                });
            });
        }

        private async Task SearchTeamsAsync()
        {
            ShowLoading("Ищем команды...");
            await Task.Run(() =>
            {
                try
                {
                    var criteria = new LeagueManager.SearchCriteria();

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (int.TryParse(MinGoalsText, out int minGoals))
                            criteria.MinGoalsFor = minGoals;

                        if (int.TryParse(MaxGoalsText, out int maxGoals))
                            criteria.MaxGoalsFor = maxGoals;

                        if (int.TryParse(MinPointsText, out int minPoints))
                            criteria.MinPoints = minPoints;

                        if (int.TryParse(MinWinsText, out int minWins))
                            criteria.MinWins = minWins;

                        if (int.TryParse(MinGoalDiffText, out int minDiff))
                            criteria.MinGoalDifference = minDiff;

                        if (!string.IsNullOrWhiteSpace(NameContainsText))
                            criteria.NameContains = NameContainsText;
                    });

                    var results = _leagueManager.SearchTeamsByCriteria(criteria);

                    // Многоуровневая сортировка вручную
                    List<Comparison<Team>> comparisons = new List<Comparison<Team>>
                    {
                        (a, b) => b.Points.CompareTo(a.Points),
                        (a, b) => b.GoalDifference.CompareTo(a.GoalDifference),
                        (a, b) => b.GoalsFor.CompareTo(a.GoalsFor)
                    };
                    List<Team> sortedResults = ManualAlgorithms.MultiSort(results, comparisons);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        SearchResults = new ObservableCollection<Team>(sortedResults);
                        HideLoading();
                    });
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show($"Ошибка поиска: {ex.Message}", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        HideLoading();
                    });
                }
            });
        }

        private void ClearSearch()
        {
            MinGoalsText = string.Empty;
            MaxGoalsText = string.Empty;
            MinPointsText = string.Empty;
            MinWinsText = string.Empty;
            MinGoalDiffText = string.Empty;
            NameContainsText = string.Empty;
            SearchResults = null;
        }

        private void RefreshPrediction()
        {
            List<Match> notPlayed = ManualAlgorithms.Where(_leagueManager.Matches, m => !m.IsPlayed);
            List<Match> sortedMatches = ManualAlgorithms.OrderBy(notPlayed, m => m.Round);
            RemainingMatches = new ObservableCollection<Match>(sortedMatches);

            var prediction = _leagueManager.GetStandings();
            PredictionStandings = new ObservableCollection<Team>(prediction);
        }

        private async Task CalculatePredictionAsync()
        {
            ShowLoading("Рассчитываем прогноз...");
            await Task.Run(() =>
            {
                try
                {
                    var remainingMatches = Application.Current.Dispatcher.Invoke(() => RemainingMatches);

                    if (remainingMatches == null || remainingMatches.Count == 0)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            MessageBox.Show("Нет оставшихся матчей для прогноза", "Информация",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                            HideLoading();
                        });
                        return;
                    }

                    var predictionManager = _leagueManager.CreatePredictionCopy();

                    for (int i = 0; i < remainingMatches.Count; i++)
                    {
                        Match match = remainingMatches[i];
                        if (match.HomeGoals >= 0 && match.AwayGoals >= 0)
                        {
                            Match predMatch = ManualAlgorithms.FirstOrDefault(predictionManager.Matches, m => m.Id == match.Id);
                            if (predMatch != null)
                            {
                                predMatch.HomeGoals = match.HomeGoals;
                                predMatch.AwayGoals = match.AwayGoals;
                                predMatch.IsPlayed = true;
                                predictionManager.ProcessMatchResult(predMatch);
                            }
                        }
                    }

                    var predictedStandings = predictionManager.GetStandings();

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        PredictionStandings = new ObservableCollection<Team>(predictedStandings);
                        HideLoading();
                    });
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show($"Ошибка при расчете прогноза: {ex.Message}", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        HideLoading();
                    });
                }
            });
        }

        private void ResetPrediction()
        {
            RefreshPrediction();
        }

        private async Task ShowHistoryAsync()
        {
            if (SelectedHistoryTeam == null) return;

            ShowLoading("Загружаем историю...");
            await Task.Run(() =>
            {
                var history = _leagueManager.GetTeamHistory(SelectedHistoryTeam.Id);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    TeamHistory = new ObservableCollection<TeamRoundStats>(history);
                    HideLoading();
                });
            });
        }

        private async Task ShowPositionGraphAsync()
        {
            if (SelectedGraphTeams == null || SelectedGraphTeams.Count == 0)
            {
                MessageBox.Show("Выберите хотя бы одну команду", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SelectedGraphTeams.Count > 5)
            {
                MessageBox.Show("Выберите не более 5 команд для удобства отображения", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ShowLoading("Генерируем график позиций...");
            await Task.Run(() =>
            {
                var histories = new Dictionary<string, List<TeamRoundStats>>();

                for (int i = 0; i < SelectedGraphTeams.Count; i++)
                {
                    Team team = SelectedGraphTeams[i];
                    var history = _leagueManager.GetTeamHistory(team.Id);
                    if (history.Count > 0)
                    {
                        histories[team.Name] = history;
                    }
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (histories.Count > 0)
                    {
                        ChartModel = ChartHelper.CreatePositionChart(histories);
                        GraphInfoText = $"Отображается график изменения позиций для {histories.Count} команд(ы)";
                    }
                    else
                    {
                        MessageBox.Show("Нет данных для выбранных команд", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        GraphInfoText = "Нет данных для отображения";
                    }
                    HideLoading();
                });
            });
        }

        private async Task ShowPointsGraphAsync()
        {
            if (SelectedGraphTeams == null || SelectedGraphTeams.Count != 1)
            {
                MessageBox.Show("Для графика очков выберите ровно одну команду", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Team team = SelectedGraphTeams[0];

            ShowLoading("Генерируем график очков...");
            await Task.Run(() =>
            {
                var history = _leagueManager.GetTeamHistory(team.Id);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (history.Count > 0)
                    {
                        ChartModel = ChartHelper.CreatePointsChart(history, team.Name);
                        GraphInfoText = $"Отображается график изменения очков для команды {team.Name}";
                    }
                    else
                    {
                        MessageBox.Show("Нет данных для этой команды", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        GraphInfoText = "Нет данных для отображения";
                    }
                    HideLoading();
                });
            });
        }

        private void GenerateData()
        {
            var dialog = new GenerateDataDialog();
            if (dialog.ShowDialog() == true)
            {
                ShowLoading("Генерируем данные...");
                Task.Run(() =>
                {
                    try
                    {
                        string dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "data.json");
                        ScheduleGenerator.GenerateAndSave(dataPath, dialog.TeamCount, dialog.SimulateRounds);

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            HideLoading();
                            MessageBox.Show($"Данные успешно сгенерированы!\n\nКоманд: {dialog.TeamCount}\nТуров для симуляции: {dialog.SimulateRounds}\n\nПерезапустите приложение для загрузки новых данных.",
                                "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        });
                    }
                    catch (Exception ex)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            HideLoading();
                            MessageBox.Show($"Ошибка генерации данных: {ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        });
                    }
                });
            }
        }

        private void ShowAbout()
        {
            MessageBox.Show("Футбольная Лига - Система управления турнирной таблицей\n\n" +
                "Возможности:\n" +
                "• Турнирная таблица с автоматическим расчетом очков\n" +
                "• Поиск по множественным критериям\n" +
                "• Личные встречи команд\n" +
                "• Прогнозирование итоговых мест\n" +
                "• Графики изменения позиций и очков\n" +
                "• Генерация полного расписания (20 команд, 380 матчей)\n\n" +
                "Версия: 2.0 (MVVM)\n" +
                "Курсовая работа по C# (WPF)",
                "О программе", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ShowLoading(string message)
        {
            LoadingText = message;
            IsLoading = true;
        }

        private void HideLoading()
        {
            IsLoading = false;
        }
    }
}
