using Microsoft.EntityFrameworkCore;

namespace BrickTrickWeb
{
    
    /// <summary>
    ///  Данная часть класса отвечает за реализацию БД
    /// </summary>
    internal partial class DBapplicationClass : DbContext
    {
        
        private DbSet<User> Users => Set<User>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=wwwroot/appDb.db");
        }
    }
    /// <summary>
    ///  Данная часть класса отвечает за методы базы данных 
    /// </summary>
    internal partial class DBapplicationClass 
    {
        public User? TakeThisUser(int userId) => Users.ToList()[userId];
        public User? TakeThisUser(string password, string email) => Users.Where(specUser => specUser.Email == email & specUser.Password == password).ToList().FirstOrDefault();
        public List<User> TakeMeAllUsers() => Users.ToList();
        public void AddUser(User user) { Users.Add(user); SaveChanges(); }
        public void RemoveUser(User user) { Users.Remove(user); SaveChanges(); }
    }
    /// <summary>
    ///  Данная часть класса отвечает за спец. методы
    /// </summary>
    internal partial class DBapplicationClass
    {
        public bool CheckExistThisUser(User user)
        {
            return CheckExistThisUser(user.Password, user.Email);
        }
        public bool CheckExistThisUser(string password, string email)
        {
            User? _user = TakeThisUser(password, email);
            return (_user != null) ? true : false;
        }
    }
    internal class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
