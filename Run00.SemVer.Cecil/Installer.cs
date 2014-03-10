using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Run00.SemVer.Cecil
{
	public class Installer : IWindsorInstaller
	{
		void IWindsorInstaller.Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(
				Component.For<IRunner>().ImplementedBy<Runner>(),
				Component.For<INuGet>().ImplementedBy<NuGet>(),
				Component.For<ISemanticVersioning>().ImplementedBy<SemanticVersioning>(),
				Component.For<NuGetConfiguration>().Instance(new NuGetConfiguration()).LifestyleSingleton()
			);
		}

	}
}
