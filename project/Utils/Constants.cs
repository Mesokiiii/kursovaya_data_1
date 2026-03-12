using System;

namespace FootballLeague
{
    /// <summary>
    /// Константы приложения
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Очки за победу
        /// </summary>
        public const int POINTS_FOR_WIN = 3;

        /// <summary>
        /// Очки за ничью
        /// </summary>
        public const int POINTS_FOR_DRAW = 1;

        /// <summary>
        /// Очки за поражение
        /// </summary>
        public const int POINTS_FOR_LOSS = 0;

        /// <summary>
        /// Максимальное количество команд в лиге
        /// </summary>
        public const int MAX_TEAMS_IN_LEAGUE = 20;

        /// <summary>
        /// Минимальное количество команд в лиге
        /// </summary>
        public const int MIN_TEAMS_IN_LEAGUE = 4;

        /// <summary>
        /// Формат отображения даты
        /// </summary>
        public const string DATE_FORMAT = "dd.MM.yyyy";

        /// <summary>
        /// Формат отображения времени
        /// </summary>
        public const string TIME_FORMAT = "HH:mm";

        /// <summary>
        /// Путь к файлу данных по умолчанию
        /// </summary>
        public const string DEFAULT_DATA_PATH = "project/Data/data.json";
    }
}