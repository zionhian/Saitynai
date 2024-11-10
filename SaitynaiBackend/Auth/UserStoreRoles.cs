namespace SaitynaiBackend.Auth
{
    public static class UserStoreRoles
    {
        public const string Admin = nameof(Admin);
        public const string User = nameof(User);
        public const string Publisher = nameof(Publisher);
        public static readonly IReadOnlyCollection<string> All = new[] { Admin, Publisher,User };
    }
}
