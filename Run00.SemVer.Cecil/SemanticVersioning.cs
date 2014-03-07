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
		void ISemanticVersioning.UpdateManifest(string manifestFile, string outputDirectory)
		{
			throw new NotImplementedException();
		}

		private IEnumerable<string> InstallPackageFiles(IPackage package, params string[] sources)
		{
			var packageManager = new PackageManager(
				new AggregateRepository(PackageRepositoryFactory.Default,
					sources,
					ignoreFailingRepositories: true
				),
				_outputFoler
			);

			packageManager.InstallPackage(package, true, false);

			return package.AssemblyReferences.Select(a => Path.Combine(_outputFoler, package.Id + "." + package.Version, a.Path));
		}

		private IEnumerable<TypeDefinition> GetAssemblyTypes(IEnumerable<string> assemblies)
		{
			var result = new List<TypeDefinition>();
			foreach (var assemblyPath in assemblies)
			{
				var assembly = AssemblyLoader.LoadCecilAssembly(assemblyPath);
				result.AddRange(QueryAggregator.PublicApiQueries.ExeuteAndAggregateTypeQueries(assembly));
			}
			return result;
		}

		private IPackage FindPreviousPackage(IPackage currentPackage)
		{
			var packages = new List<IPackage>();
			foreach (var source in _sources)
			{
				var sourceRepository = PackageRepositoryFactory.Default.CreateRepository(source);
				var package = sourceRepository
					.GetPackages()
					.Where(p => string.Compare(p.Id, currentPackage.Id, StringComparison.InvariantCultureIgnoreCase) == 0);
				packages.AddRange(package);
			}
			return packages.OrderBy(p => p.Version).LastOrDefault();
		}

		private Differences GetChanges(IEnumerable<TypeDefinition> currentTypes, IEnumerable<TypeDefinition> previousTypes)
		{
			var collection = new Differences();

			//Add all new and removed types to the collection
			var newDefs = currentTypes
				.Where(c => previousTypes.Any(p => p.IsEqual(c)) == false)
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

		private Version GetNewVersion(Differences diff, Version currentVersion)
		{
			if (diff.RemovedTypes.Count > 0)
				return new Version(currentVersion.Major + 1, 0, 0, 0);

			if (diff.ChangedTypes.Count > 0)
				return new Version(currentVersion.Major + 1, 0, 0, 0);

			if (diff.AddedTypes.Count > 0)
				return new Version(currentVersion.Major, currentVersion.Minor + 1, 0, 0);

			if (diff.UpdatedTypes.Count > 0)
				return new Version(currentVersion.Major, currentVersion.Minor + 1, 0, 0);

			return new Version(currentVersion.Major, currentVersion.Minor, currentVersion.Build + 1, 0);
		}

		private readonly string _outputFoler = "Packages";
		private readonly string[] _sources;
	}
}
