using System;
using System.Globalization;
using System.Windows.Data;

namespace FootballLeague
{
    /// <summary>
    /// Конвертер для отображения счета матча
    /// </summary>
    public class ScoreConverter : IValueConverter
    {
        public object Convert(object value, object targetType, object parameter, CultureInfo culture)
        {
            if (value is Match match)
            {
                if (match.IsPlayed)
                    return $"{match.HomeGoals} : {match.AwayGoals}";
                else
                    return "vs";
            }
            return "N/A";
        }

        public object ConvertBack(object value, object targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Конвертер для форматирования позиции команды
    /// </summary>
    public class PositionConverter : IValueConverter
    {
        public object Convert(object value, object targetType, object parameter, CultureInfo culture)
        {
            if (value is int position)
            {
                return $"{position} место";
            }
            return "N/A";
        }

        public object ConvertBack(object value, object targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Конвертер для форматирования разности голов
    /// </summary>
    public class GoalDifferenceConverter : IValueConverter
    {
        public object Convert(object value, object targetType, object parameter, CultureInfo culture)
        {
            if (value is int difference)
            {
                if (difference > 0)
                    return $"+{difference}";
                else
                    return difference.ToString();
            }
            return "0";
        }

        public object ConvertBack(object value, object targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
