using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet;
using Run00.MsTest;
using System.IO;

namespace Run00.SemVer.Cecil.IntegrationTest
{
	[DeploymentItem(@"..\..\Artifacts")]
	[TestClass, CategorizeByConventionClass(typeof(IntegrationTest))]
	public class IntegrationTest
	{
		[TestMethod, CategorizeByConvention]
		public void WhenOnlyCodeBlockIsChanged_ShouldBeRefactor()
		{
			//Arrange
			var versioning = BuildVersioning();
			var testSample = typeof(TestSample.IOrderService).Assembly.Location;

			//Act
			var result = versioning.Calculate(new[] { testSample }, "Test.Sample.Refactor");

			//Assert
			//Assert.AreEqual(ContractChangeType.Refactor, result.Justification.ChangeType);
			Assert.AreEqual("1.0.1.0", result.New.ToString());
		}

		[TestMethod, CategorizeByConvention]
		public void WhenOnlyCommentsAreChanged_ShouldBeCosmetic()
		{
			//Arrange
			var versioning = BuildVersioning();
			var testSample = typeof(TestSample.IOrderService).Assembly.Location;

			//Act
			var result = versioning.Calculate(new[] { testSample }, "Test.Sample.Comments");

			//Assert
			//Assert.AreEqual(ContractChangeType.Cosmetic, result.Justification.ChangeType);
			Assert.AreEqual("1.0.1.0", result.New.ToString());
		}

		[TestMethod, CategorizeByConvention]
		public void WhenClassIsDeleted_ShouldBeBreaking()
		{
			//Arrange
			var versioning = BuildVersioning();
			var testSample = typeof(TestSample.IOrderService).Assembly.Location;

			//Act
			var result = versioning.Calculate(new[] { testSample }, "Test.Sample.Deleted");

			//Assert
			//Assert.AreEqual(ContractChangeType.Breaking, result.Justification.ChangeType);
			Assert.AreEqual("2.0.0.0", result.New.ToString());
		}

		[TestMethod, CategorizeByConvention]
		public void WhenClassIsAdded_ShouldBeEnhancement()
		{
			//Arrange
			var versioning = BuildVersioning();
			var testSample = typeof(TestSample.IOrderService).Assembly.Location;

			//Act
			var result = versioning.Calculate(new[] { testSample }, "Test.Sample.Adding");

			//Assert
			//Assert.AreEqual(ContractChangeType.Enhancement, result.Justification.ChangeType);
			Assert.AreEqual("1.1.0.0", result.New.ToString());
		}

		[TestMethod, CategorizeByConvention]
		public void WhenMethodIsModified_ShouldBeBreaking()
		{
			//Arrange
			var versioning = BuildVersioning();
			var testSample = typeof(TestSample.IOrderService).Assembly.Location;

			//Act
			var result = versioning.Calculate(new[] { testSample }, "Test.Sample.Modifying");

			//Assert
			//Assert.AreEqual(ContractChangeType.Breaking, result.Justification.ChangeType);
			Assert.AreEqual("2.0.0.0", result.New.ToString());
		}

		[TestMethod, CategorizeByConvention]
		public void WhenNamespaceIsChanged_ShouldBeBreaking()
		{
			//Arrange
			var versioning = BuildVersioning();
			var testSample = typeof(TestSample.IOrderService).Assembly.Location;

			//Act
			var result = versioning.Calculate(new[] { testSample }, "Test.Sample.Namespace");

			//Assert
			//Assert.AreEqual(ContractChangeType.Breaking, result.Justification.ChangeType);
			Assert.AreEqual("2.0.0.0", result.New.ToString());
		}

		[TestMethod, CategorizeByConvention]
		public void WhenGenericIsChanged_ShouldBeBreaking()
		{
			//Arrange
			var versioning = BuildVersioning();
			var testSample = typeof(TestSample.IOrderService).Assembly.Location;

			//Act
			var result = versioning.Calculate(new[] { testSample }, "Test.Sample.Generic");

			//Assert
			//Assert.AreEqual(ContractChangeType.Breaking, result.Justification.ChangeType);
			Assert.AreEqual("2.0.0.0", result.New.ToString());
		}

		[TestMethod, CategorizeByConvention]
		public void WhenPrivateMethodIsAdded_ShouldBeRefactor()
		{
			//Arrange
			var versioning = BuildVersioning();
			var testSample = typeof(TestSample.IOrderService).Assembly.Location;

			//Act
			var result = versioning.Calculate(new[] { testSample }, "Test.Sample.Private");

			//Assert
			//Assert.AreEqual(ContractChangeType.Refactor, result.Justification.ChangeType);
			Assert.AreEqual("1.0.1.0", result.New.ToString());
		}

		private ISemanticVersioning BuildVersioning()
		{
			var currentDir = Directory.GetCurrentDirectory();
			var packageRepository = PackageRepositoryFactory.Default.CreateRepository(currentDir);
			var packageManager = new PackageManager(packageRepository, Path.GetTempPath());
			var versioning = (ISemanticVersioning)new SemanticVersioning(packageRepository, packageManager);
			return versioning;
		}
	}
}
