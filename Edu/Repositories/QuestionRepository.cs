using EducationPlatform.Models;
using Npgsql;
using NpgsqlTypes;
using Edu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPlatform.Repositories
{
    public class QuestionRepository
    {
        private const string SelectByCourse = "SELECT \"id\", \"text\", \"id_course\" " +
                                             "FROM \"question\" " +
                                             "WHERE \"id_course\" = @0 " +
                                             "ORDER BY \"id\"";
        private const string SelectById = "SELECT \"id\", \"text\", \"id_course\" " +
                                             "FROM \"question\" " +
                                             "WHERE \"id\" = @0";

        public async Task<Question[]> GetByCourse(int idCourse)
        {
            using (var conn = new NpgsqlConnection(Config.ConnectionString))
            {
                await conn.OpenAsync();
                using (var comm = conn.CreateCommand())
                {
                    var result = new List<Question>();
                    comm.CommandText = SelectByCourse;
                    comm.Parameters.AddWithValue("0", NpgsqlDbType.Integer, idCourse);
                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new Question()
                            {
                                Id = reader.GetInt32(0),
                                Text = reader.GetString(1),
                                IdCourse = reader.GetInt32(2)
                            });
                        }
                        return result.ToArray();
                    }
                }
            }
        }
        public async Task<Question> Get(int id)
        {
            using (var conn = new NpgsqlConnection(Config.ConnectionString))
            {
                await conn.OpenAsync();
                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText = SelectById;
                    comm.Parameters.AddWithValue("0", NpgsqlDbType.Bigint, id);
                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        if (!reader.Read()) return null;
                        return new Question
                        {
                            Id = reader.GetInt32(0),
                            Text = reader.GetString(1),
                            IdCourse = reader.GetInt32(2)
                        };
                    }
                }
            }
        }
    }

}
