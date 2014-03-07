using System;

namespace Run00.SemVer
{
	public class VersionChange
	{
		public Version Previous { get; set; }
		public Version New { get; set; }
		public Differences Justification { get; set; }
	}
}
