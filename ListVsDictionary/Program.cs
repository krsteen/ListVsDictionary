using System.Diagnostics;

namespace MyNamespace
{
    public static class Program
    {
        private const int MAX_USER_COUNT = 10000;
        private static List<AdUser> AdUsers = new List<AdUser>();
        private static List<SapUser> SapUsers = new List<SapUser>();

        static void InitUsers()
        {
            for (int i = 0; i < MAX_USER_COUNT; i++)
            {
                var username = $"Username{i}";
                AdUsers.Add(new AdUser() { Id = i, Username = username });
                SapUsers.Add(new SapUser() { Id = i, Username = username, IsDepartmentLeader = i % 10 == 0 });
            }
        }

        public static void Main(params string[] args)
        {
            InitUsers();

            //Run all variants
            MergeUsersUsingDictionaryApproach();
            MergeUsersUsingHashSetApproach();
            MergeUsersUsingListApproach();

            //Run again
            MergeUsersUsingDictionaryApproach();
            MergeUsersUsingHashSetApproach();
            MergeUsersUsingListApproach();
        }

        static void MergeUsersUsingDictionaryApproach()
        {
            var spElapsed = Stopwatch.StartNew();

            var departmentLeadersDictionary = SapUsers.Where(sapUser => sapUser.IsDepartmentLeader)
              .ToDictionary(sapUser=> sapUser.Username, sapUser => sapUser);

            var usersWithAllProps = AdUsers.Select(adUser => new UserWithAllProps()
            {
                Id = adUser.Id,
                Username = adUser.Username,
                IsAdmin = adUser.IsAdmin,
                IsDepartmentLeader = departmentLeadersDictionary.ContainsKey(adUser.Username)
            }).ToList();

            spElapsed.Stop();
            Console.WriteLine($"Dictionary: {spElapsed.ElapsedMilliseconds} ms");
        }


        static void MergeUsersUsingHashSetApproach()
        {
            var spElapsed = Stopwatch.StartNew();

            var departmentLeadersHashSet = SapUsers.Where(sapUser => sapUser.IsDepartmentLeader)
                .Select(sapUser => sapUser.Username).ToHashSet();

            var usersWithAllProps = AdUsers.Select(adUser => new UserWithAllProps()
            {
                Id = adUser.Id,
                Username = adUser.Username,
                IsAdmin = adUser.IsAdmin,
                IsDepartmentLeader = departmentLeadersHashSet.Contains(adUser.Username)
            }).ToList();

            spElapsed.Stop();
            Console.WriteLine($"HashSet: {spElapsed.ElapsedMilliseconds} ms");
        }

        static void MergeUsersUsingListApproach()
        {
            var spElapsed = Stopwatch.StartNew();

            var usersWithAllProps = AdUsers.Select(adUser => new UserWithAllProps()
            {
                Id = adUser.Id,
                Username = adUser.Username,
                IsAdmin = adUser.IsAdmin,
                IsDepartmentLeader = SapUsers.FirstOrDefault(sapUser => sapUser.Username == adUser.Username).IsDepartmentLeader //This is offcourse dumb since firstordefault might return null
            }).ToList();

            spElapsed.Stop();
            Console.WriteLine($"List: {spElapsed.ElapsedMilliseconds} ms");
        }

        
    }
}
