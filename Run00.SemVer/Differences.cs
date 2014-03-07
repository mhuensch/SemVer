using System.Collections.Generic;

namespace Run00.SemVer
{
	public class Differences
	{
		public ICollection<string> AddedTypes { get; private set; }
		public ICollection<string> RemovedTypes { get; private set; }
		public ICollection<string> ChangedTypes { get; private set; }
		public ICollection<string> UpdatedTypes { get; private set; }

		public Differences()
		{
			AddedTypes = new List<string>();
			RemovedTypes = new List<string>();
			ChangedTypes = new List<string>();
			UpdatedTypes = new List<string>();
		}
	}
}
