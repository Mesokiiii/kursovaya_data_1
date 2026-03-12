namespace FootballLeague
{
    /// <summary>
    /// Модель команды футбольной лиги
    /// </summary>
    public class Team
    {
        /// <summary>
        /// Уникальный идентификатор команды
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Название команды
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Проверяет корректность данных команды
        /// </summary>
        /// <returns>True, если данные корректны</returns>
        public bool IsValid()
        {
            return Id > 0 && 
                   !string.IsNullOrWhiteSpace(Name) &&
                   Played >= 0 &&
                   Wins >= 0 &&
                   Draws >= 0 &&
                   Losses >= 0 &&
                   GoalsFor >= 0 &&
                   GoalsAgainst >= 0 &&
                   (Wins + Draws + Losses) == Played;
        }
        
        /// <summary>
        /// Количество сыгранных матчей
        /// </summary>
        public int Played { get; set; }
        
        /// <summary>
        /// Количество побед
        /// </summary>
        public int Wins { get; set; }
        
        /// <summary>
        /// Количество ничьих
        /// </summary>
        public int Draws { get; set; }
        
        /// <summary>
        /// Количество поражений
        /// </summary>
        public int Losses { get; set; }
        
        /// <summary>
        /// Забитые голы
        /// </summary>
        public int GoalsFor { get; set; }
        
        /// <summary>
        /// Пропущенные голы
        /// </summary>
        public int GoalsAgainst { get; set; }

        /// <summary>
        /// Разность забитых и пропущенных голов
        /// </summary>
        public int GoalDifference => GoalsFor - GoalsAgainst;
        
        /// <summary>
        /// Очки команды (3 за победу, 1 за ничью)
        /// </summary>
        public int Points => (Wins * Constants.POINTS_FOR_WIN) + (Draws * Constants.POINTS_FOR_DRAW);
    }
}
