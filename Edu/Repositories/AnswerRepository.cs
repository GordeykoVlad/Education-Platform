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
    public class AnswerRepository
    {
        private const string SelectByQuestion = "SELECT \"id\", \"text\", \"id_question\", \"is_correct\" " +
                                             "FROM \"answer\" " +
                                             "WHERE \"id_question\" = @0 " +
                                             "ORDER BY \"id\"";
        private const string SelectById = "SELECT \"id\", \"text\", \"id_question\", \"is_correct\" " +
                                             "FROM \"answer\" " +
                                             "WHERE \"id\" = @0";


        public async Task<Answer[]> GetByQuestion(int idQuestion)
        {
            using (var conn = new NpgsqlConnection(Config.ConnectionString))
            {
                await conn.OpenAsync();
                using (var comm = conn.CreateCommand())
                {
                    var result = new List<Answer>();
                    comm.CommandText = SelectByQuestion;
                    comm.Parameters.AddWithValue("0", NpgsqlDbType.Integer, idQuestion);
                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new Answer()
                            {
                                Id = reader.GetInt32(0),
                                Text = reader.GetString(1),
                                IdQuestion = reader.GetInt32(2),
                                IsCorrect =reader.GetBoolean(3)
                              });
                        }
                        return result.ToArray();
                    }
                }
            }
        }
        public async Task<Answer> Get(int id)
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
                        return new Answer
                        {
                            Id = reader.GetInt32(0),
                            Text = reader.GetString(1),
                            IdQuestion = reader.GetInt32(2),
                            IsCorrect = reader.GetBoolean(3)
                        };
                    }
                }
            }
        }

        }
}
