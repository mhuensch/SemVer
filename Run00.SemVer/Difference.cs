
namespace Run00.SemVer
{
	public class Difference
	{
		public enum ChangeReason { None, Added, Removed }

		public string Name { get; set; }

		public ChangeReason Reason { get; set; }
	}
}
