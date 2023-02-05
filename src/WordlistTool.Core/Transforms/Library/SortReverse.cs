namespace WordlistTool.Core.Transforms.Library;

public class SortReverseTransform : InMemoryTransform
{
	protected override void Transform(IList<string> list, CancellationToken cancellationToken)
	{
		if (list is List<string> implementation)
		{
			implementation.Reverse();
		}
		else
		{
			for (int i = 0; i < list.Count; i++)
			{
				int reverseIndex = list.Count - i - 1;
				(list[reverseIndex], list[i]) = (list[i], list[reverseIndex]);
			}
		}
	}
}
