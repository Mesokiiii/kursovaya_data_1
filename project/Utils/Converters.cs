using System.Globalization;
using System.Windows.Data;

namespace FootballLeague
{
    public class ScoreConverter : IValueConverter
    {
        public object Convert(object value, object targetType, object parameter, CultureInfo culture)
        {
            if (value is Match match)
            {
                return $"{match.HomeGoals} : {match.AwayGoals}";
            }
            return "";
        }

        public object ConvertBack(object value, object targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
