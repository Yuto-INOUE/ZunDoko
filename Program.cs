using System.Collections;

namespace ZunDoko
{
	public enum ZunDoko
	{
		Zun,
		Doko,
	}

	public static class ZunDokoExtensions
	{
		public static string AsZunDokoString(this ZunDoko zunDoko) =>
			zunDoko switch
			{
				ZunDoko.Zun => "ズン",
				ZunDoko.Doko => "ドコ",
				_ => throw new ArgumentOutOfRangeException(nameof(zunDoko), zunDoko, null)
			};
	}

	public class ZunDokoList : IEnumerable<ZunDoko>
	{
		private readonly List<ZunDoko> _results;
		private static readonly Random s_random = new ();
		private static readonly ZunDoko[] s_successResult = new[]
		{
			ZunDoko.Zun, ZunDoko.Zun, ZunDoko.Zun, ZunDoko.Zun, ZunDoko.Doko,
		};

		public ZunDokoList(IEnumerable<ZunDoko> results)
		{
			_results = results.ToList();
		}

		public IEnumerator<ZunDoko> GetEnumerator() => _results.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public static ZunDokoList Create()
		{
			lock (s_random)
			{
				var result = Enumerable.Range(0, 5)
					.Select(_ => (s_random.Next() % 2) == 0 ? ZunDoko.Zun : ZunDoko.Doko)
					.ToArray();
				return new ZunDokoList(result);
			}
		}

		public bool IsKiyoshi => s_successResult.SequenceEqual(_results);
	}

	public static class Program
	{
		public static void Main(string[] args)
		{
			var trialCount = 0;
			while (true)
			{
				trialCount++;
				var zunDokoList = ZunDokoList.Create();
				Console.Write(string.Join(" ", zunDokoList.Select(z => z.AsZunDokoString())));

				if (zunDokoList.IsKiyoshi)
				{
					Console.WriteLine("  キヨシ！");
					break;
				}

				Console.WriteLine("  キヨシではありません");
			}

			Console.WriteLine($"お疲れ様でした。試行回数: {trialCount}回");
		}
	}
}
