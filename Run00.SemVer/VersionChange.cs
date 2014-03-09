using System;
using System.Collections.Generic;

namespace Run00.SemVer
{
	public class VersionChange
	{
		public Version Old { get; set; }

		public Version New { get; set; }

		public IEnumerable<Difference> Differences { get; set; }

	}
}
