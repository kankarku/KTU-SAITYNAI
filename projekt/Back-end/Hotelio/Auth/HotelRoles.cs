namespace Hotelio.Auth
{
    public class HotelRoles
    {
        public const string Owner = nameof(Owner);
        public const string Admin = nameof(Admin);
        public const string Client = nameof(Client);

        public static readonly IReadOnlyCollection<string> All = new[] { Owner, Admin, Client};
    }
}
