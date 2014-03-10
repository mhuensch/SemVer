using System.Linq;

namespace Run00.SemVer.Cecil
{
	public class Runner : IRunner
	{
		public Runner(ISemanticVersioning versioning, INuGet nuget, NuGetConfiguration config, IReporter reporter)
		{
			_versioning = versioning;
			_nuget = nuget;
			_config = config;
			_reporter = reporter;
		}

		void IRunner.Execute(string[] args)
		{
			//TODO: add parsing for more complex arguments
			var path = args.First();
			_config.PackageSource = args.Skip(1).Take(1).SingleOrDefault();
			_config.InstallPath = args.Skip(2).Take(1).SingleOrDefault();
			
			var manifest = _nuget.ReadManifest(path);
			var assemblies = manifest.Files.Select(f => f.Source);
			var packageId = manifest.Metadata.Id;

			var change = _versioning.Calculate(assemblies, packageId);

			_nuget.SavePackageVersion(path, change.New);
		}

		private readonly ISemanticVersioning _versioning;
		private readonly INuGet _nuget;
		private readonly NuGetConfiguration _config;
		private readonly IReporter _reporter;

	}
}
