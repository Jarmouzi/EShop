namespace EShop.Services.Model.TypeSafe
{
    public class TS
    {

        public static class Roles
        {
            public const string Admin = "Admin";
            public const string User = "User";
            public const string Contributor = "Contributor";
            public const string Guest = "Guest";
        }

        public static class Contoller
        {
            public const string Product = "Product";
            public const string Supplier = "Supplier";
            public const string Module = "Module";
            public const string Category = "Category";
        }

        public static class Permissions
        {
            public const int None = 0;
            public const int Read = 1;
            public const int Write = 2;
            public const int Update = 3;
            public const int Delete = 4;

        }

        public static class Policies
        {
            public const string ReadPolicy = "ReadPolicy";
            public const string ReadAndWritePolicy = "AddAndReadPolicy";
            public const string FullControlPolicy = "FullControlPolicy";

            public const string GenericPolicy = "GenericPolicy";
        }
    }
}
