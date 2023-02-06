namespace WordlistTool.Core.Transforms.Library;

public class SortByLengthTransform : InMemoryTransform
{
	public SortByLengthTransform(bool descending)
	{
		Descending = descending;
	}

	public bool Descending { get; }


	protected override void Transform(IList<string> list, CancellationToken cancellationToken)
	{
		if (list is not List<string> implementation)
		{
			throw new NotSupportedException();
		}

		implementation.Sort(Descending ? new DescendingComparer() : new AscendingComparer());
	}


	private sealed class AscendingComparer : IComparer<string>
	{
		public int Compare(string? x, string? y) => x.Length.CompareTo(y.Length);
	}

	private sealed class DescendingComparer : IComparer<string>
	{
		public int Compare(string? x, string? y) => y.Length.CompareTo(x.Length);
	}
}
