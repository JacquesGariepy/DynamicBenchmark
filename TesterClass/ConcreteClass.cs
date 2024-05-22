namespace Concrete
{
    public class ConcreteClass
    {

        public void TestDto()
        {
            var dto = new NewDto { Id = 1, Name = "Name" };
        }

        public void TestDto2()
        {
            var dto = new NewDtoRecord(1, "Name");
        }

        public void TestSortedSetWithUserComparer()
        {
            try
            {
                var users = new SortedSet<User>
                {
                    new User { Username = "User3" },
                    new User { Username = "User1" },
                    new User { Username = "User2" },
                    new User { Username = "User4" },
                };

                var sortedSet = new SortedSet<User>(users, new UserComparer());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void TestListWithSort()
        {
            try
            {
                var users = new List<User>
                {
                    new User { Username = "User3" },
                    new User { Username = "User1" },
                    new User { Username = "User2" },
                    new User { Username = "User4" },
                };

                users.Sort(new UserComparer());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void TestListWithLinqSort()
        {
            try
            {
                var users = new List<User>
                {
                    new User { Username = "User3" },
                    new User { Username = "User1" },
                    new User { Username = "User2" },
                    new User { Username = "User4" },
                };

                var sortedUsers = users.OrderBy(u => u.Username).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public string TestToString()
        {
            var newDto = new NewDto();
            return newDto.ToString();
        }

        public string TestNameOf()
        {
            var newDto = new NewDto();
            return nameof(newDto);
        }
    }
    public class NewDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public record NewDtoRecord(int Id, string Name);

    public class User
    {
        public string Username { get; set; }
    }

    public class UserComparer : IComparer<User>
    {
        public int Compare(User? x, User? y)
        {
            return x.Username.CompareTo(y.Username);
        }
    }
}