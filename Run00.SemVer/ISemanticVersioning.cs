using System.Collections.Generic;

namespace Run00.SemVer
{
	public interface ISemanticVersioning
	{
		VersionChange Calculate(IEnumerable<string> assemblies, string packageId);
	}
}
