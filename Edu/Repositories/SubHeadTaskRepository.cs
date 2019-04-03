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
    public class SubHeadTaskRepository
    {
        private const string Select = "SELECT \"id\", \"name\" , \"description\" ,  \"id_coursetask\" " +
                                    "FROM \"subheadtask\" " +
                                    "ORDER BY \"id\"";
        private const string SelectById = "SELECT \"id\", \"name\", \"description\" , \"id_coursetask\" " +
                                          "FROM \"subheadtask\" " +
                                          "WHERE \"id\" = @0";
        private const string SelectByCourse = "SELECT \"id\", \"name\", \"description\" , \"id_coursetask\" " +
                                         "FROM \"coursetask\" " +
                                          "WHERE \"id_coursetask\" = @0 " +
                                             "ORDER BY \"id\"";


        public async Task<SubHeadTask[]> GetByCourse(int idCourseTask)
        {
            using (var conn = new NpgsqlConnection(Config.ConnectionString))
            {
                await conn.OpenAsync();
                using (var comm = conn.CreateCommand())
                {
                    var result = new List<SubHeadTask>();
                    comm.CommandText = SelectByCourse;
                    comm.Parameters.AddWithValue("0", NpgsqlDbType.Integer, idCourseTask);
                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new SubHeadTask()
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.GetString(2),
                                IdCourseTask = reader.GetInt32(3)
                            });
                        }
                        return result.ToArray();
                    }
                }
            }
        }


        public async Task<SubHeadTask[]> GetAll()
        {
            using (var conn = new NpgsqlConnection(Config.ConnectionString))
            {
                await conn.OpenAsync();
                using (var comm = conn.CreateCommand())
                {
                    var result = new List<SubHeadTask>();
                    comm.CommandText = Select;
                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new SubHeadTask()
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.GetString(2),
                                IdCourseTask = reader.GetInt32(3)
                            });
                        }
                        return result.ToArray();
                    }
                }
            }
        }
        public async Task<SubHeadTask> Get(int id)
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
                        return new SubHeadTask
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            IdCourseTask = reader.GetInt32(3)
                        };
                    }
                }
            }
        }
    }
}
