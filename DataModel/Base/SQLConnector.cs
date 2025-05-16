using System.Data;
using Microsoft.Data.SqlClient;
using System.Reflection;

namespace DAL.Base
{
    /// <summary>
    /// Статический класс для выполнения SQL-операций через хранимые процедуры
    /// </summary>
    internal static class SqlConnector
    {
        /// <summary>
        /// Асинхронно выполнить хранимую процедуру и вернуть коллекцию сущностей
        /// </summary>
        public static async Task<IEnumerable<T>> GetAsync<T>(string storedProcedure, object args = null) where T : new()
        {
            using var connection = new SqlConnection(ConnectionString.Get());
            using var command = new SqlCommand(storedProcedure, connection) { CommandType = CommandType.StoredProcedure };

            PrepareParams(command.Parameters, args);

            try
            {
                await connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                return await TranslateAsync<T>(reader);
            }
            catch (Exception e)
            {
                throw new DataAccessException($"Вызов {storedProcedure} : {e.Message}");
            }
        }

        /// <summary>
        /// Асинхронно выполнить хранимую процедуру и модифицировать данные
        /// </summary>
        public static async Task<int> SetAsync(string storedProcedure, object args)
        {
            using var connection = new SqlConnection(ConnectionString.Get());
            using var command = new SqlCommand(storedProcedure, connection) { CommandType = CommandType.StoredProcedure };

            PrepareParams(command.Parameters, args);
            var res = command.Parameters.Add("@RetVal", SqlDbType.Int);
            res.Direction = ParameterDirection.ReturnValue;

            try
            {
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                return (int)res.Value;
            }
            catch (Exception e)
            {
                throw new DataAccessException($"Вызов {storedProcedure} : {e.Message}");
            }
        }

        /// <summary>
        /// Преобразовать данные из SqlDataReader в список объектов
        /// </summary>
        private static async Task<List<T>> TranslateAsync<T>(SqlDataReader reader) where T : new()
        {
            var columns = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();
            var props = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                              .Where(p => p.CanWrite)
                              .ToArray();

            var list = new List<T>();

            while (await reader.ReadAsync())
                list.Add(Read<T>(reader, props, columns));

            return list;
        }

        /// <summary>
        /// Заполнить свойства объекта значениями из текущей записи SqlDataReader
        /// </summary>
        private static T Read<T>(SqlDataReader reader, PropertyInfo[] props, string[] columns) where T : new()
        {
            var item = new T();

            foreach (var name in columns)
            {
                var property = props.FirstOrDefault(p => p.Name == name);

                if (property == null)
                    continue;

                var type = reader[name].GetType();

                if (type == typeof(DBNull))
                {
                    property.SetValue(item, null);
                }
                else if (type == typeof(byte[]) && property.PropertyType == typeof(Stream))
                {
                    property.SetValue(item, reader.GetStream(name));
                }
                else
                {
                    property.SetValue(item, reader[name]);
                }
            }

            return item;
        }

        /// <summary>
        /// Генеририровать имя параметра SQL на основе имени поля объекта
        /// </summary>
        private static string Param(FieldInfo fi)
        {
            var name = fi.Name;
            var ic = name.IndexOf('>');
            name = "@" + name[1..ic];
            return name;
        }

        /// <summary>
        /// Добавить параметры в SqlCommand на основе свойств объекта
        /// </summary>
        private static void PrepareParams(SqlParameterCollection p, object args)
        {
            if (args == null)
                return;

            foreach (var fi in args?.GetType().GetTypeInfo().DeclaredFields)
            {
                p.AddWithValue(Param(fi), fi.GetValue(args) ?? DBNull.Value);
            }
        }
    }
}
