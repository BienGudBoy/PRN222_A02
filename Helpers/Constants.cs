namespace NguyenVanBien_SE18C.NET_A02.Helpers
{
    public static class UserRoles
    {
        public const int Staff = 1;
        public const int Lecturer = 2;
        public const int Admin = 3;
    }

    public static class SessionHelper
    {
        public const string SessionKeyAccountId = "AccountId";
        public const string SessionKeyAccountEmail = "AccountEmail";
        public const string SessionKeyAccountName = "AccountName";
        public const string SessionKeyAccountRole = "AccountRole";
    }
}
