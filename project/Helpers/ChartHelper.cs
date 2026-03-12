using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;

namespace FootballLeague
{
    public class ChartHelper
    {
        public static PlotModel CreatePointsChart(List<TeamRoundStats> history, string teamName)
        {
            var model = new PlotModel { Title = $"Изменение очков: {teamName}" };

            model.Axes.Add(new LinearAxis 
            { 
                Position = AxisPosition.Bottom, 
                Title = "Тур",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            });
            
            model.Axes.Add(new LinearAxis 
            { 
                Position = AxisPosition.Left, 
                Title = "Очки",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            });

            var series = new LineSeries
            {
                Title = teamName,
                Color = OxyColors.Blue,
                StrokeThickness = 2,
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerFill = OxyColors.Blue
            };

            foreach (var stat in history.OrderBy(s => s.Round))
            {
                series.Points.Add(new DataPoint(stat.Round, stat.Points));
            }

            model.Series.Add(series);
            return model;
        }
    }
}
