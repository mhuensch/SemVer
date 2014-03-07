
namespace Run00.SemVer
{
	public interface ISemanticVersioning
	{
		void UpdateManifest(string manifestFile, string outputDirectory);
	}
}
