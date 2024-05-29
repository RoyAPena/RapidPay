using SharedKernel;

namespace RapidPay.Domain.User
{
    public sealed class User : Entity
    {
        private User(Guid id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
        }

        private User()
        {
        }

        public string Username { get; private set; }
        public string Password { get; private set; }

        public static User Create(string userName, string password)
        {
            var user = new User(Guid.NewGuid(), userName, password);
            
            return user;
        }
    }
}