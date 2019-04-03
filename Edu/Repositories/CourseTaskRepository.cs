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
    public class CourseTaskRepository
    {
        private const string Select = "SELECT \"id\", \"name\" , \"description\" ,  \"id_course\" " +
                                    "FROM \"coursetask\" " +
                                    "ORDER BY \"id\"";
        private const string SelectById = "SELECT \"id\", \"name\", \"description\" , \"id_course\" " +
                                          "FROM \"coursetask\" " +
                                          "WHERE \"id\" = @0";
        private const string SelectByCourse = "SELECT \"id\", \"name\", \"description\" , \"id_course\" " +
                                         "FROM \"course\" " +
                                          "WHERE \"id_course\" = @0 " +
                                             "ORDER BY \"id\"";


        public async Task<CourseTask[]> GetByCourse(int idCourse)
        {
            using (var conn = new NpgsqlConnection(Config.ConnectionString))
            {
                await conn.OpenAsync();
                using (var comm = conn.CreateCommand())
                {
                    var result = new List<CourseTask>();
                    comm.CommandText = SelectByCourse;
                    comm.Parameters.AddWithValue("0", NpgsqlDbType.Integer, idCourse);
                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new CourseTask()
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.GetString(2),
                                IdCourse = reader.GetInt32(3)
                            });
                        }
                        return result.ToArray();
                    }
                }
            }
        }


        public async Task<CourseTask[]> GetAll()
        {
            using (var conn = new NpgsqlConnection(Config.ConnectionString))
            {
                await conn.OpenAsync();
                using (var comm = conn.CreateCommand())
                {
                    var result = new List<CourseTask>();
                    comm.CommandText = Select;
                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new CourseTask()
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.GetString(2),
                                IdCourse = reader.GetInt32(3)
                            });
                        }
                        return result.ToArray();
                    }
                }
            }
        }
        public async Task<CourseTask> Get(int id)
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
                        return new CourseTask
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            IdCourse = reader.GetInt32(3)
                        };
                    }
                }
            }
        }
    }
}
