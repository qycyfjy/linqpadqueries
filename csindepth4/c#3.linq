<Query Kind="Program">
  <Namespace>System.Runtime.CompilerServices</Namespace>
  <Namespace>System.Runtime.InteropServices</Namespace>
</Query>

void Main()
{
	var a1 = new[] { 1, 2, 3, 4 };
	int[] a2 = { 5, 6, 7, 8 };

	var users = new List<User>{
			new User{Name = "Jim", Age=20},
			new User{Name = "Mike", Age = 23},
		};
	users.Dump();

	var anonymousUser = new { Name = "Anonymous", Age = 34 };
	anonymousUser.Dump();

	Expression<Func<int, int, int>> f = (a, b) => a + b;
	f.Dump();

	var nums = new List<int>(100);
	for (int i = 0; i < 100; i++)
	{
		nums.Add(Random.Shared.Next(10000));
	}
	var evens = from num in nums
				where num % 2 == 0
				select num;
	evens.Dump();
}



class User
{
	public string Name { get; init; }
	public int Age { get; set; }

	public User() { }

	public User(string name, int age)
	{
		Name = name;
		Age = age;
	}
}