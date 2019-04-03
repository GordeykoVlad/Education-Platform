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
    public class CourseRepository
    {
        private const string Select = "SELECT \"id\", \"name\" , \"description\" , \"image\" , \"id_language\" " +
                                    "FROM \"course\" " +
                                    "ORDER BY \"id\"";
        private const string SelectById = "SELECT \"id\", \"name\", \"description\" , \"image\" , \"id_language\" " +
                                          "FROM \"course\" " +
                                          "WHERE \"id\" = @0";
        private const string SelectByLanguage = "SELECT \"id\", \"name\", \"description\" , \"image\" , \"id_language\" " +
                                         "FROM \"course\" " +
                                          "WHERE \"id_language\" = @0 " +
                                             "ORDER BY \"id\"";


        public async Task<Course[]> GetByLanguage(int idLanguage)
        {
            using (var conn = new NpgsqlConnection(Config.ConnectionString))
            {
                await conn.OpenAsync();
                using (var comm = conn.CreateCommand())
                {
                    var result = new List<Course>();
                    comm.CommandText = SelectByLanguage;
                    comm.Parameters.AddWithValue("0", NpgsqlDbType.Integer, idLanguage);
                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new Course()
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.GetString(2),
                                Image = reader.GetString(3),
                                IdLanguage = reader.GetInt32(4)
                            });
                        }
                        return result.ToArray();
                    }
                }
            }
        }


        public async Task<Course[]> GetAll()
        {
            using (var conn = new NpgsqlConnection(Config.ConnectionString))
            {
                await conn.OpenAsync();
                using (var comm = conn.CreateCommand())
                {
                    var result = new List<Course>();
                    comm.CommandText = Select;
                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new Course()
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.GetString(2),
                                Image = reader.GetString(3),
                                IdLanguage = reader.GetInt32(4)
                            });
                        }
                        return result.ToArray();
                    }
                }
            }
        }
        public async Task<Course> Get(int id)
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
                        return new Course
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            Image = reader.GetString(3),
                            IdLanguage = reader.GetInt32(4)
                        };
                    }
                }
            }
        }

    }
}
