namespace Edu.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;
    using Npgsql;
    using NpgsqlTypes;

    public class UserRepository
    {
        private const string Select = "SELECT \"id\", \"login\", \"password\" " +
                                      "FROM \"user\" " +
                                      "ORDER BY \"id\"";
        private const string SelectById = "SELECT \"id\", \"login\", \"password\" " +
                                          "FROM \"user\" " +
                                          "WHERE \"id\" = @0";       
        private const string SelectByLogin = "SELECT \"id\", \"login\", \"password\" " +
                                             "FROM \"user\" " +
                                             "WHERE \"login\" = @0";
        private const string Insert = "INSERT INTO \"user\"(\"login\", \"password\") " +
                                      "VALUES (@0, @1) " +
                                      "RETURNING \"id\", \"login\", \"password\"";
        private const string Update = "UPDATE \"user\" " +
                                      "SET \"login\" = @1, \"password\" = @2 " +
                                      "WHERE \"id\" = @0 " +
                                      "RETURNING \"id\", \"login\", \"password\"";
        private const string Delete = "DELETE FROM \"user\" " +
                                      "WHERE \"id\" = @0 " +
                                      "RETURNING \"id\", \"login\", \"password\"";

        public async Task<User> Get(long id)
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
                        return new User
                        {
                            Id = reader.GetInt64(0),
                            Login = reader.GetString(1),
                            Password = reader.GetString(2)
                        };
                    }
                }
            }
        }

        public async Task<User> GetByLogin(string login)
        {
            using (var conn = new NpgsqlConnection(Config.ConnectionString))
            {
                await conn.OpenAsync();
                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText = SelectByLogin;
                    comm.Parameters.AddWithValue("0", NpgsqlDbType.Text, login);
                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        if (! await reader.ReadAsync()) return null;
                        return new User
                        {
                            Id = reader.GetInt64(0),
                            Login = reader.GetString(1),
                            Password = reader.GetString(2)
                        };
                    }
                }
            }
        }
        
        public async Task<User[]> GetAll()
        {
            using (var conn = new NpgsqlConnection(Config.ConnectionString))
            {
                await conn.OpenAsync();
                using (var comm = conn.CreateCommand())
                {
                    var result = new List<User>();
                    comm.CommandText = Select;
                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new User()
                            {
                                Id = reader.GetInt64(0),
                                Login = reader.GetString(1),
                                Password = reader.GetString(2)
                            });
                        }
                        return result.ToArray();
                    }
                }
            }
        }

        public async Task<User> Add(User user)
        {
            using (var conn = new NpgsqlConnection(Config.ConnectionString))
            {
                await conn.OpenAsync();
                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText = Insert;
                    comm.Parameters.AddWithValue("0", NpgsqlDbType.Text, user.Login);
                    comm.Parameters.AddWithValue("1", NpgsqlDbType.Text, user.Password);

                    comm.Prepare();
                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        if (!reader.Read()) return null;
                        return new User
                        {
                            Id = reader.GetInt64(0),
                            Login = reader.GetString(1),
                            Password = reader.GetString(2)
                        };
                    }
                }
            }
        }

        public async Task<User> Edit(User user)
        {
            using (var conn = new NpgsqlConnection(Config.ConnectionString))
            {
                await conn.OpenAsync();
                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText = Update;
                    comm.Parameters.AddWithValue("0", NpgsqlDbType.Bigint, user.Id);
                    comm.Parameters.AddWithValue("1", NpgsqlDbType.Text, user.Login);
                    comm.Parameters.AddWithValue("2", NpgsqlDbType.Text, user.Password);
                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        if (!reader.Read()) return null;
                        return new User
                        {
                            Id = reader.GetInt64(0),
                            Login = reader.GetString(1),
                            Password = reader.GetString(2)
                        };
                    }
                }
            }
        }

        public async Task<User> Remove(User user)
        {
            using (var conn = new NpgsqlConnection(Config.ConnectionString))
            {
                await conn.OpenAsync();
                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText = Delete;
                    comm.Parameters.AddWithValue("0", NpgsqlDbType.Bigint, user.Id);
                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        if (!reader.Read()) return null;
                        return new User
                        {
                            Id = reader.GetInt64(0),
                            Login = reader.GetString(1),
                            Password = reader.GetString(2)
                        };
                    }
                }
            }
        }
    }
}