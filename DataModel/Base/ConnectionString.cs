namespace DAL.Base
{
    /// <summary>
    /// Строка подключения к БД
    /// </summary>
    internal static class ConnectionString
    {
        private static readonly string _connectionString = "";

        /// <summary>
        /// Получить
        /// </summary>
        public static string Get()
        {
            return _connectionString;
        }
    }
}
