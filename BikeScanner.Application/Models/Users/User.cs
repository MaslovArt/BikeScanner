using BikeScanner.Domain.Models;

namespace BikeScanner.Application.Models.Users
{
    public record User
    {
        public long UserId { get; set; }
        public string Login { get; set; }
        public AccountState State { get; set; }
    }
}

