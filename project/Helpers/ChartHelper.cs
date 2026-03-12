using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;

namespace FootballLeague
{
    /// <summary>
    /// Помощник для создания графиков статистики команд
    /// </summary>
    public class ChartHelper
    {
        /// <summary>
        /// Создает график изменения очков команды по турам
        /// </summary>
        /// <param name="history">История статистики команды</param>
        /// <param name="teamName">Название команды</param>
        /// <returns>Модель графика</returns>
        public static PlotModel CreatePointsChart(List<TeamRoundStats> history, string teamName)
        {
            if (history == null || !history.Any())
                return new PlotModel { Title = "Нет данных для отображения" };

            var model = new PlotModel 
            { 
                Title = $"Изменение очков: {teamName}",
                Background = OxyColors.White
            };

            // Настройка осей
            model.Axes.Add(new LinearAxis 
            { 
                Position = AxisPosition.Bottom, 
                Title = "Тур",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                MajorGridlineColor = OxyColor.FromRgb(230, 230, 230),
                MinorGridlineColor = OxyColor.FromRgb(245, 245, 245)
            });
            
            model.Axes.Add(new LinearAxis 
            { 
                Position = AxisPosition.Left, 
                Title = "Очки",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                MajorGridlineColor = OxyColor.FromRgb(230, 230, 230),
                MinorGridlineColor = OxyColor.FromRgb(245, 245, 245)
            });

            // Создание серии данных
            var series = new LineSeries
            {
                Title = teamName,
                Color = OxyColors.Blue,
                StrokeThickness = 2,
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerFill = OxyColors.Blue,
                MarkerStroke = OxyColors.DarkBlue
            };

            // Добавление точек данных
            foreach (var stat in history.OrderBy(s => s.Round))
            {
                series.Points.Add(new DataPoint(stat.Round, stat.Points));
            }

            model.Series.Add(series);
            return model;
        }

        /// <summary>
        /// Создает график сравнения позиций команд
        /// </summary>
        /// <param name="histories">Истории нескольких команд</param>
        /// <returns>Модель графика</returns>
        public static PlotModel CreatePositionChart(Dictionary<string, List<TeamRoundStats>> histories)
        {
            var model = new PlotModel 
            { 
                Title = "Изменение позиций в турнирной таблице",
                Background = OxyColors.White
            };

            model.Axes.Add(new LinearAxis 
            { 
                Position = AxisPosition.Bottom, 
                Title = "Тур"
            });
            
            model.Axes.Add(new LinearAxis 
            { 
                Position = AxisPosition.Left, 
                Title = "Позиция",
                StartPosition = 1,
                EndPosition = 0 // Инвертируем ось (1 место вверху)
            });

            var colors = new[] { OxyColors.Blue, OxyColors.Red, OxyColors.Green, OxyColors.Orange, OxyColors.Purple };
            int colorIndex = 0;

            foreach (var teamHistory in histories)
            {
                var series = new LineSeries
                {
                    Title = teamHistory.Key,
                    Color = colors[colorIndex % colors.Length],
                    StrokeThickness = 2,
                    MarkerType = MarkerType.Circle,
                    MarkerSize = 3
                };

                foreach (var stat in teamHistory.Value.OrderBy(s => s.Round))
                {
                    series.Points.Add(new DataPoint(stat.Round, stat.Position));
                }

                model.Series.Add(series);
                colorIndex++;
            }

            return model;
        }
    }
}
