using System.Runtime.InteropServices;

namespace WordlistTool.Core.Transforms.Library;

public class SortTransform : InMemoryTransform
{
	public SortTransform(bool descending)
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

		var inner = CollectionsMarshal.AsSpan(implementation);
		inner.Sort();

		if (Descending)
		{
			inner.Reverse();
		}
	}
}
