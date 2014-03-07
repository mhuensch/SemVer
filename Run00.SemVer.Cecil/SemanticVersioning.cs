using ApiChange.Api.Introspection;
using Mono.Cecil;
using NuGet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Run00.SemVer.Cecil
{
	public class SemanticVersioning : ISemanticVersioning
	{
		public SemanticVersioning(IPackageRepository packageRepository, IPackageManager packageManager)
		{
			_packageRepository = packageRepository;
			_packageManager = packageManager;
		}

		VersionChange ISemanticVersioning.Calculate(IEnumerable<string> assemblies, string packageId)
		{
			var currentTypes = GetAssemblyTypes(assemblies);

			var previousPackage = GetPreviousPackage(packageId);
			var previousAssemblies = GetAssemblies(previousPackage);
			var previousTypes = GetAssemblyTypes(previousAssemblies);

			var changes = GetChanges(currentTypes, previousTypes);
			var newVersion = GetNewVersion(changes, previousPackage.Version.Version);

			return new VersionChange()
			{
				Justification = changes,
				Previous = previousPackage.Version.Version,
				New = newVersion
			};

			//var manifest = Manifest.ReadFrom("path here");
			//manifest.Save()

			//var nupkgFile = new FileInfo(nupkg);

			//var currentPackage = new ZipPackage(nupkgFile.FullName);
			//var currentAssemblies = InstallPackageFiles(currentPackage, nupkgFile.Directory.FullName);

			//var version = GetNewVersion(changeCount, previousPackage.Version.Version);

			//ZipFile.ExtractToDirectory(nupkgFile.FullName, "Extracted");
			//TODO: Repack File with new version
		}

		private IPackage GetPreviousPackage(string packageId)
		{
			return _packageRepository.GetPackages()
				.Where(p => string.Compare(p.Id, packageId, StringComparison.InvariantCultureIgnoreCase) == 0)
				.OrderBy(p => p.Version)
				.LastOrDefault();
		}

		private IEnumerable<string> GetAssemblies(IPackage package)
		{
			if (package == null)
				return Enumerable.Empty<string>();

			var previousPackageDir = _packageManager.PathResolver.GetPackageDirectory(package);

			if (_packageManager.FileSystem.DirectoryExists(previousPackageDir) == false)
				_packageManager.InstallPackage(package, true, false);

			var fullDir = _packageManager.FileSystem.GetFullPath(previousPackageDir);
			var previousAssemblies = package.AssemblyReferences.Select(a => Path.Combine(fullDir, a.Path));
			return previousAssemblies;
		}

		private IEnumerable<TypeDefinition> GetAssemblyTypes(IEnumerable<string> assemblies)
		{
			var result = new List<TypeDefinition>();
			foreach (var assemblyPath in assemblies)
			{
				var file = new FileInfo(assemblyPath);
				var assembly = AssemblyLoader.LoadCecilAssembly(file.FullName);
				result.AddRange(QueryAggregator.PublicApiQueries.ExeuteAndAggregateTypeQueries(assembly));
			}
			return result;
		}

		private Differences GetChanges(IEnumerable<TypeDefinition> currentTypes, IEnumerable<TypeDefinition> previousTypes)
		{
			var collection = new Differences();

			//Add all new and removed types to the collection
			var newDefs = currentTypes
				.Where(c => previousTypes.Any(p => p.Name.Equals(c.Name) && p.Namespace.Equals(c.Namespace)) == false)
				.Select(t => t.Name);
			collection.AddedTypes.AddRange(newDefs);

			var removedDefs = previousTypes
				.Where(p => currentTypes.Any(c => c.IsEqual(p)) == false)
				.Select(t => t.Name);
			collection.RemovedTypes.AddRange(removedDefs);

			//Add changed types
			foreach (TypeDefinition previousType in previousTypes)
			{
				var currentType = currentTypes.Where(c => c.IsEqual(previousType)).FirstOrDefault();
				if (currentType == null)
					continue;

				var typeDiff = TypeDiff.GenerateDiff(previousType, currentType, QueryAggregator.PublicApiQueries);

				if (TypeDiff.None == typeDiff)
					continue;

				collection.ChangedTypes.Add(typeDiff.TypeV1.Name);
			}

			return collection;
		}

		private Version GetNewVersion(Differences diff, Version previousVersion)
		{
			if (diff.RemovedTypes.Count > 0)
				return new Version(previousVersion.Major + 1, 0, 0, 0);

			if (diff.ChangedTypes.Count > 0)
				return new Version(previousVersion.Major + 1, 0, 0, 0);

			if (diff.AddedTypes.Count > 0)
				return new Version(previousVersion.Major, previousVersion.Minor + 1, 0, 0);

			if (diff.UpdatedTypes.Count > 0)
				return new Version(previousVersion.Major, previousVersion.Minor + 1, 0, 0);

			return new Version(previousVersion.Major, previousVersion.Minor, previousVersion.Build + 1, 0);
		}

		private readonly IPackageRepository _packageRepository;
		private readonly IPackageManager _packageManager;
	}
}
