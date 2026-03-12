namespace FootballLeague
{
    /// <summary>
    /// Результат матча для команды
    /// </summary>
    public enum MatchResult
    {
        /// <summary>
        /// Победа
        /// </summary>
        Win,
        
        /// <summary>
        /// Ничья
        /// </summary>
        Draw,
        
        /// <summary>
        /// Поражение
        /// </summary>
        Loss
    }

    /// <summary>
    /// Статус матча
    /// </summary>
    public enum MatchStatus
    {
        /// <summary>
        /// Запланирован
        /// </summary>
        Scheduled,
        
        /// <summary>
        /// Идет
        /// </summary>
        InProgress,
        
        /// <summary>
        /// Завершен
        /// </summary>
        Finished,
        
        /// <summary>
        /// Отменен
        /// </summary>
        Cancelled
    }

    /// <summary>
    /// Тип сортировки турнирной таблицы
    /// </summary>
    public enum SortType
    {
        /// <summary>
        /// По очкам
        /// </summary>
        Points,
        
        /// <summary>
        /// По разности голов
        /// </summary>
        GoalDifference,
        
        /// <summary>
        /// По забитым голам
        /// </summary>
        GoalsFor,
        
        /// <summary>
        /// По названию команды
        /// </summary>
        TeamName
    }
}