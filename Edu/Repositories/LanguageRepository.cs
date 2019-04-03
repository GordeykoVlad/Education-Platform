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
    public class LanguageRepository
    {
        private const string Select = "SELECT \"id\", \"name\" , \"description\" , \"image\" " +
                                    "FROM \"language\" " +
                                    "ORDER BY \"id\"";
        private const string SelectById = "SELECT \"id\", \"name\", \"description\" , \"image\" " +
                                          "FROM \"language\" " +
                                          "WHERE \"id\" = @0";

        public async Task<Language[]> GetAll()
        {
            using (var conn = new NpgsqlConnection(Config.ConnectionString))
            {
                await conn.OpenAsync();
                using (var comm = conn.CreateCommand())
                {
                    var result = new List<Language>();
                    comm.CommandText = Select;
                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new Language()
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.GetString(2),
                                Image = reader.GetString(3)
                            });
                        }
                        return result.ToArray();
                    }
                }
            }
        }
        public async Task<Language> Get(int id)
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
                        return new Language
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            Image = reader.GetString(3)
                        };
                    }
                }
            }
        }

    }
}
