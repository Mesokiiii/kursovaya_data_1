namespace FootballLeague
{
    /// <summary>
    /// Статистика команды по турам для построения графиков
    /// </summary>
    public class TeamRoundStats
    {
        /// <summary>
        /// Идентификатор команды
        /// </summary>
        public int TeamId { get; set; }
        
        /// <summary>
        /// Название команды
        /// </summary>
        public string TeamName { get; set; }
        
        /// <summary>
        /// Номер тура
        /// </summary>
        public int Round { get; set; }
        
        /// <summary>
        /// Количество очков на момент тура
        /// </summary>
        public int Points { get; set; }
        
        /// <summary>
        /// Позиция в турнирной таблице
        /// </summary>
        public int Position { get; set; }
    }
}
