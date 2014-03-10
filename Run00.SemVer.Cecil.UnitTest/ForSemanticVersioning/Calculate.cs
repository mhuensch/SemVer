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
			var moqNuGet = new Mock<INuGet>(MockBehavior.Strict);
			var versioning = (ISemanticVersioning)new SemanticVersioning(moqNuGet.Object);

			//Act
			var result = versioning.Calculate(null, null);

			//Assert
			Assert.IsNotNull(null);
			moqNuGet.VerifyAll();
		}
	}
}
