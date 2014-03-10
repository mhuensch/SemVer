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
			var moqNuget = new Mock<INuGet>(MockBehavior.Strict);

			//Act
			var result = new SemanticVersioning(moqNuget.Object);

			//Assert
			Assert.IsNotNull(result);
			moqNuget.VerifyAll();
		}
	}
}
