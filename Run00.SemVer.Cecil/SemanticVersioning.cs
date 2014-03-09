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
		public SemanticVersioning(IPackageRepository packageRepository, IPackageManager packageManager, IContractComparer comparer)
		{
			_packageRepository = packageRepository;
			_packageManager = packageManager;
			_comparer = comparer;
		}

		VersionChange ISemanticVersioning.Calculate(IEnumerable<string> assemblies, string packageId)
		{
			var neoDefinitions = GetPackageDefinition(assemblies);

			var paleoPackage = GetLatestPackage(packageId);
			var paleoAssemblies = GetAssemblies(paleoPackage);
			var paleoDefinitions = GetPackageDefinition(paleoAssemblies);


			var changes = GetDifferences(neoDefinitions, paleoDefinitions);
			var newVersion = GetNewVersion(changes, paleoPackage.Version.Version);

			return new VersionChange()
			{
				Differences = changes,
				Old = paleoPackage.Version.Version,
				New = newVersion
			};

			//var manifest = Manifest.ReadFrom("path here");
			//manifest.Save()
			//var currentPackage = new ZipPackage(nupkgFile.FullName);
			//ZipFile.ExtractToDirectory(nupkgFile.FullName, "Extracted");
		}

		private IPackage GetLatestPackage(string packageId)
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

			var dir = _packageManager.PathResolver.GetPackageDirectory(package);

			if (_packageManager.FileSystem.DirectoryExists(dir) == false)
				_packageManager.InstallPackage(package, true, false);

			var fullDir = _packageManager.FileSystem.GetFullPath(dir);
			var assemblies = package.AssemblyReferences.Select(a => Path.Combine(fullDir, a.Path));
			return assemblies;
		}

		private PackageDefinition GetPackageDefinition(IEnumerable<string> assemblies)
		{
			var result = new PackageDefinition();
			foreach (var assemblyPath in assemblies)
			{
				var file = new FileInfo(assemblyPath);

				var assembly = AssemblyDefinition.ReadAssembly(file.FullName);
				var types = assembly.Modules.SelectMany(m => m.Types).Where(t => t.IsPublic);
				result.TypeDefinitions.AddRange(types);

				foreach (var type in types)
					Add(type, result);
			}
			return result;
		}

		public PackageDefinition Add(TypeDefinition type, PackageDefinition package)
		{
			var exposedTypes = new List<TypeDefinition>();

			var methods = type.Methods.Where(m => m.IsPublic);
			exposedTypes.AddRange(methods
				.SelectMany(m => m.Parameters.Select(p => p.ParameterType.Resolve()).Union(new[] { m.ReturnType.Resolve() }))
				.Where(m => m != null && package.TypeDefinitions.Select(t => t.FullName).Contains(m.FullName) == false));
			package.MethodDefinitions.AddRange(methods);

			var fields = type.Fields.Where(m => m.IsPublic);
			exposedTypes.AddRange(fields
				.Select(m => m.FieldType.Resolve())
				.Where(m => m != null && package.TypeDefinitions.Select(t => t.FullName).Contains(m.FullName) == false));
			package.FieldDefinitions.AddRange(fields);

			package.TypeDefinitions.AddRange(exposedTypes.Distinct());

			foreach (var t in exposedTypes)
				Add(t, package);

			return package;
		}

		private List<Difference> GetDifferences(PackageDefinition neoPackage, PackageDefinition paleoPackage)
		{
			var result = new List<Difference>();
			var neos = GetDefs(neoPackage);
			var paleos = GetDefs(paleoPackage);

			var addedKeys = neos.Keys.Where(c => paleos.Keys.Contains(c) == false);
			result.AddRange(
				from k in addedKeys
				let m = neos[k]
				select new Difference() { Name = m.FullName, Reason = Difference.ChangeReason.Added });

			var removedKeys = paleos.Keys.Where(c => neos.Keys.Contains(c) == false);
			result.AddRange(
				from k in removedKeys
				let m = paleos[k]
				select new Difference() { Name = m.FullName, Reason = Difference.ChangeReason.Removed });

			//var matchingKeys = paleos.Keys.Where(c => neos.Keys.Contains(c));
			//result.AddRange(matchingKeys.SelectMany(k => _comparer.Compare(neos[k], paleos[k])));

			return result;
		}

		private Dictionary<string, IMemberDefinition> GetDefs(PackageDefinition package)
		{
			return package
				.TypeDefinitions.Cast<IMemberDefinition>()
				.Union(package.MethodDefinitions.Cast<IMemberDefinition>())
				.ToDictionary(t => t.FullName, t => t);
		}

		private Version GetNewVersion(IEnumerable<Difference> diff, Version paleoVersion)
		{
			if (diff.Where(d => d.Reason == Difference.ChangeReason.Removed).Any())
				return new Version(paleoVersion.Major + 1, 0, 0, 0);

			if (diff.Where(d => d.Reason == Difference.ChangeReason.Added).Any())
				return new Version(paleoVersion.Major, paleoVersion.Minor + 1, 0, 0);

			return new Version(paleoVersion.Major, paleoVersion.Minor, paleoVersion.Build + 1, 0);
		}

		private readonly IContractComparer _comparer;
		private readonly IPackageRepository _packageRepository;
		private readonly IPackageManager _packageManager;
	}
}
