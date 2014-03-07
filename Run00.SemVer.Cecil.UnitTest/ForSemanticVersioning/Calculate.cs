using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NuGet;
using Run00.MsTest;

namespace Run00.SemVer.Cecil.UnitTest.ForSemanticVersioning
{
	[DeploymentItem(@"..\..\Artifacts")]
	[TestClass, CategorizeByConventionClass(typeof(Calculate))]
	public class Calculate
	{
		[TestMethod, CategorizeByConvention()]
		public void When_Should()
		{
			//Arrange
			var moqRepository = new Mock<IPackageRepository>(MockBehavior.Strict);
			var moqPackageManager = new Mock<IPackageManager>(MockBehavior.Strict);
			var versioning = (ISemanticVersioning)new SemanticVersioning(moqRepository.Object, moqPackageManager.Object);

			//Act
			var result = versioning.Calculate(null, null);

			//Assert
			Assert.IsNotNull(null);
			moqRepository.VerifyAll();
			moqPackageManager.VerifyAll();
		}
	}
}
