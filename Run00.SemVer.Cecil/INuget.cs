using NuGet;
using System;
using System.Collections.Generic;

namespace Run00.SemVer.Cecil
{
	public interface INuGet
	{
		IPackage GetLatestPackage(string packageId);

		IEnumerable<string> GetAssemblies(IPackage package);

		Manifest ReadManifest(string path);

		void SavePackageVersion(string path, Version version);

	}
}
