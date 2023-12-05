<Query Kind="Program">
  <Namespace>System.Runtime.CompilerServices</Namespace>
  <Namespace>System.Runtime.InteropServices</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

void Main()
{
	var pack = new Pack();

	unsafe
	{
		pack.Buf[4] = 255;
	}

	pack.Dump();

	var gen10 = Generate(10);
	var gen10Enumerator = gen10.GetEnumerator();
	var gen10List = new List<int>();
	while (gen10Enumerator.MoveNext())
	{
		gen10List.Add(gen10Enumerator.Current);
	}
	gen10List.Dump();

	var s = new Nullable<int>(10);
	s.GetType().Dump();

	GenericCounter<int>.Count += 1;
	GenericCounter<int>.Count.Dump();
	GenericCounter<string>.Count.Dump();
	typeof(List<int>).Dump();
	typeof(Dictionary<,>).Dump();

	var achievementMgr = new AchievementManager();
	var player = new Player { Name = "1024" };
	player.PlayerDie += achievementMgr.FirstDieAchievement;

	var t = Task.Run(() =>
	{
		Thread.Sleep(Random.Shared.Next(3000));
		player.Die();
	});
	t.Wait();
}

IEnumerable<int> Generate(int n)
{
	try
	{
		int i = 0;
		while (i < n)
		{
			yield return i;
			++i;
		}
	}
	finally
	{
		Console.WriteLine("iter over");
	}
}

unsafe struct Pack
{
	public fixed byte Buf[0x10];
}

class Clazz
{
	public static void DumpFileName(
	[CallerMemberName] string memberName = "",
	[CallerFilePath] string fileName = "",
	[CallerLineNumber] int sourceLine = 0)
	{
		$"{memberName} at {fileName}:{sourceLine}".Dump();
	}
}

class BaseClazz
{
	public void Foo()
	{
		Console.WriteLine("Base.Foo()");
	}

	public virtual void Bar()
	{
		Console.WriteLine("Base.Bar()");
	}
}

class DerivedClazz : BaseClazz
{
	public new void Foo()
	{
		Console.WriteLine("Derived.Foo()");
	}

	public override void Bar()
	{
		Console.WriteLine("Derived.Bar()");
	}
}

class Player
{
	public delegate void PlayerDieEventHandler(Player p);
	public event PlayerDieEventHandler PlayerDie;
	public string Name { get; init; }
	public Player()
	{
		this.PlayerDie += (p) =>
		{
			Console.WriteLine($"{Name} Dead.");
		};
	}
	public void Die()
	{
		PlayerDie.Invoke(this);
	}
}

class AchievementManager
{
	public void FirstDieAchievement(Player p)
	{
		Console.WriteLine($"Die Die Die");
	}
}

class GenericCounter<T>
{
	static GenericCounter()
	{
		Console.WriteLine($"GenericCounter<{typeof(T)}> Constructed.");
	}
	public static int Count { get; set; }
}

partial class Utils
{
	partial void Foo(ref int s);
	public partial bool Bar();
}

partial class Utils
{
	public partial bool Bar() { return false; }
}