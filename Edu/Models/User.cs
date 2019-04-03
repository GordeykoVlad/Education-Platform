namespace Edu.Models
{
    public class User
    {
        public static object Identity { get; internal set; }
        public long Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}