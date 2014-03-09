using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NuGet;
using Run00.MsTest;

namespace Run00.SemVer.Cecil.UnitTest.ForSemanticVersioning
{
	[TestClass, CategorizeByConventionClass(typeof(Constructor))]
	public class Constructor
	{
		[TestMethod, CategorizeByConvention()]
		public void WhenValidParameters_ShouldConstruct()
		{
			//Arrange
			var moqRepository = new Mock<IPackageRepository>(MockBehavior.Strict);
			var moqPackageManager = new Mock<IPackageManager>(MockBehavior.Strict);

			//Act
			var result = new SemanticVersioning(moqRepository.Object, moqPackageManager.Object);

			//Assert
			Assert.IsNotNull(result);
			moqRepository.VerifyAll();
			moqPackageManager.VerifyAll();
		}
	}
}
