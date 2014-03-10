
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Run00.SemVer.Cecil;
using System;
using System.IO;
namespace Run00.SemVer.WindowsConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			var container = new WindsorContainer();
			IRunner runner = null;
			try
			{
				container.Install(
					FromAssembly.This(),
					FromAssembly.InDirectory(new AssemblyFilter(Directory.GetCurrentDirectory()))
				);
				runner = container.Resolve<IRunner>();
				runner.Execute(args);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			finally
			{
				if (runner == null)
					container.Release(runner);
			}
		}
	}
}
