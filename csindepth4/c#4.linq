<Query Kind="Program">
  <NuGetReference>Microsoft.Office.Interop.Excel</NuGetReference>
  <NuGetReference>Microsoft.Office.Interop.Outlook</NuGetReference>
  <NuGetReference>Microsoft.Office.Interop.PowerPoint</NuGetReference>
  <NuGetReference>Microsoft.Office.Interop.Word</NuGetReference>
  <Namespace>Excel = Microsoft.Office.Interop.Excel</Namespace>
  <Namespace>Outlook = Microsoft.Office.Interop.Outlook</Namespace>
  <Namespace>PowerPoint = Microsoft.Office.Interop.PowerPoint</Namespace>
  <Namespace>System.Runtime.CompilerServices</Namespace>
  <Namespace>System.Runtime.InteropServices</Namespace>
  <Namespace>Word = Microsoft.Office.Interop.Word</Namespace>
</Query>

using System.Dynamic;

void Main()
{
	IEnumerable<string> e = new List<string> { "1", "2", "3", "4" };
	IEnumerable<object> eo = e;

	Action<object> f = (o) => { };
	Action<string> fs = f;

	var app = new Excel.Application();
	app.Visible = true;
	var workbook = app.Workbooks.Add();
	Excel.Worksheet sheet = app.ActiveSheet;
	var start = sheet.Cells[1, 1];
	var end = sheet.Cells[1, 20];
	sheet.Range[start, end].Value = Enumerable.Range(1, 20).ToArray();
	workbook.SaveAs(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "cominterop.xlsx"));
	app.Application.Quit();

	dynamic s = "hello world";
	var upperS = s.ToUpper() as string;
	upperS.Dump();

	dynamic obj = new ExpandoObject();
	obj.name = "foo";
	Action<string> action = s => { Console.WriteLine(s); };
	obj.foo = action;
	obj.foo(obj.name);

	IDictionary<string, object> dict = obj;
	dict["bar"] = "bar";
	obj.foo(dict["bar"] as string);

	dynamic so = new SimpleDynamic();
	so.foo(123, "hello");
	Console.WriteLine(so.bar);
}

class SimpleDynamic : DynamicObject
{
	public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
	{
		Console.WriteLine($"dynamic call {binder.Name}({string.Join(", ", args)})");
		result = null;
		return true;
	}

	public override bool TryGetMember(GetMemberBinder binder, out object result)
	{
		Console.WriteLine($"dynamic get {binder.Name}");
		result = "Success";
		return true;
	}
}

