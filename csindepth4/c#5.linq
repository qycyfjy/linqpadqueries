<Query Kind="Program">
  <NuGetReference>Microsoft.Office.Interop.Excel</NuGetReference>
  <NuGetReference>Microsoft.Office.Interop.Outlook</NuGetReference>
  <NuGetReference>Microsoft.Office.Interop.PowerPoint</NuGetReference>
  <NuGetReference>Microsoft.Office.Interop.Word</NuGetReference>
  <Namespace>Excel = Microsoft.Office.Interop.Excel</Namespace>
  <Namespace>Outlook = Microsoft.Office.Interop.Outlook</Namespace>
  <Namespace>PowerPoint = Microsoft.Office.Interop.PowerPoint</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Runtime.CompilerServices</Namespace>
  <Namespace>System.Runtime.InteropServices</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Word = Microsoft.Office.Interop.Word</Namespace>
</Query>

void Main()
{
	//var task = Task.Run(async () =>
	//{
	//	(await GetWebPageLength("https://example.com/")).Dump();
	//});
	//task.Wait();

	//Task.Run(async () =>
	//{
	//	var t1 = ThrowAsync();
	//	try
	//	{
	//		await t1;
	//	}
	//	catch (Exception e)
	//	{
	//		e.Dump();
	//		t1.Exception.Dump();
	//	}
	//	var t2 = ThrowAsync();
	//	try
	//	{
	//		t2.Wait();
	//	}
	//	catch (Exception e)
	//	{
	//		e.Dump();
	//		t2.Exception.Dump();
	//	}
	//}).Wait();
	//
	//	try
	//	{
	//		var t1 = ThrowTest();
	//		Console.WriteLine("exception?");
	//		t1.Wait();
	//	}
	//	catch (Exception e)
	//	{
	//		e.Dump();
	//	}
	//
	//	try
	//	{
	//		var t2 = ThrowTestAsync();
	//		Console.WriteLine("exception?");
	//		t2.Wait();
	//	}
	//	catch (Exception e)
	//	{
	//		e.Dump();
	//	}

	Task.Run(async () =>
	{
		using var ss = StringStream("你好世界");
		var bbb = new ByteByByte(ss);
		var buffer = new byte[10];
		for (int i = 0; i < 10; i++)
		{
			var b = await bbb.FetchAsync();
			if (b is null)
			{
				break;
			}
			buffer[i] = b.Value;
		}
		buffer.Dump();
	}).Wait();
}

async Task<int> GetWebPageLength(string url)
{
	var client = new HttpClient();
	var res = await client.GetStringAsync(url);
	return res.Length;
}

Task ThrowTest()
{
	Console.WriteLine("now throw a exception");
	throw new Exception("ex");
}

async Task ThrowTestAsync()
{
	Console.WriteLine("now throw a exception");
	throw new Exception("ex");
}

async Task ThrowAsync()
{
	await Task.Delay(1000);
	throw new Exception("123");
}

class ByteByByte : IDisposable
{
	private byte[] _buffer;
	private int _readPos;
	private int _bufferedCount;
	private Stream _stream;
	bool disposedValue;

	public ByteByByte(Stream stream)
	{
		_stream = stream;
		_readPos = 0;
		_buffer = new byte[8 * 1024];
	}

	public async ValueTask<byte?> FetchAsync()
	{
		if (_readPos == _bufferedCount)
		{
			_bufferedCount = await _stream.ReadAsync(_buffer, 0, _buffer.Length).ConfigureAwait(false);
			if (_bufferedCount == 0)
			{
				return null;
			}
			_readPos = 0;
		}
		return _buffer[_readPos++];
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				_stream.Dispose();
			}
			_buffer = null;
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}

Stream StringStream(string s)
{
	var bytes = Encoding.UTF8.GetBytes(s);
	var ms = new MemoryStream(bytes);
	return ms;
}