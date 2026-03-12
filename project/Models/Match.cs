namespace FootballLeague
{
    /// <summary>
    /// Модель матча между двумя командами
    /// </summary>
    public class Match
    {
        /// <summary>
        /// Уникальный идентификатор матча
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Номер тура
        /// </summary>
        public int Round { get; set; }
        
        /// <summary>
        /// Команда хозяев поля
        /// </summary>
        public Team HomeTeam { get; set; }
        
        /// <summary>
        /// Команда гостей
        /// </summary>
        public Team AwayTeam { get; set; }
        
        /// <summary>
        /// Голы команды хозяев
        /// </summary>
        public int HomeGoals { get; set; }
        
        /// <summary>
        /// Голы команды гостей
        /// </summary>
        public int AwayGoals { get; set; }
        
        /// <summary>
        /// Флаг завершения матча
        /// </summary>
        public bool IsPlayed { get; set; }
    }
}
