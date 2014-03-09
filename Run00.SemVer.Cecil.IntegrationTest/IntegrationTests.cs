extern alias AddingClass;
extern alias AddingMethod;
extern alias ChangingComments;
extern alias ControlGroup;
extern alias DeletingClass;
extern alias ChangingGenericType;
extern alias ChangingMethodSignature;
extern alias ChangingNamespace;
extern alias AddingPrivateMethod;
extern alias RefactoringMethod;
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
			var testSample = typeof(RefactoringMethod::Run00.TestSample.Order).Assembly.Location;

			//Act
			var result = versioning.Calculate(new[] { testSample }, "Test.Sample.ControlGroup");

			//Assert
			//Assert.AreEqual(ContractChangeType.Refactor, result.Justification.ChangeType);
			Assert.AreEqual("1.0.1.0", result.New.ToString());
		}

		[TestMethod, CategorizeByConvention]
		public void WhenOnlyCommentsAreChanged_ShouldBeCosmetic()
		{
			//Arrange
			var versioning = BuildVersioning();
			var testSample = typeof(ChangingComments::Run00.TestSample.Order).Assembly.Location;

			//Act
			var result = versioning.Calculate(new[] { testSample }, "Test.Sample.ControlGroup");

			//Assert
			//Assert.AreEqual(ContractChangeType.Cosmetic, result.Justification.ChangeType);
			Assert.AreEqual("1.0.1.0", result.New.ToString());
		}

		[TestMethod, CategorizeByConvention]
		public void WhenClassIsDeleted_ShouldBeBreaking()
		{
			//Arrange
			var versioning = BuildVersioning();
			var testSample = typeof(DeletingClass::Run00.TestSample.Order).Assembly.Location;

			//Act
			var result = versioning.Calculate(new[] { testSample }, "Test.Sample.ControlGroup");

			//Assert
			//Assert.AreEqual(ContractChangeType.Breaking, result.Justification.ChangeType);
			Assert.AreEqual("2.0.0.0", result.New.ToString());
		}

		[TestMethod, CategorizeByConvention]
		public void WhenClassIsAdded_ShouldBeEnhancement()
		{
			//Arrange
			var versioning = BuildVersioning();
			var testSample = typeof(AddingClass::Run00.TestSample.Order).Assembly.Location;

			//Act
			var result = versioning.Calculate(new[] { testSample }, "Test.Sample.ControlGroup");

			//Assert
			//Assert.AreEqual(ContractChangeType.Enhancement, result.Justification.ChangeType);
			Assert.AreEqual("1.1.0.0", result.New.ToString());
		}

		[TestMethod, CategorizeByConvention]
		public void WhenMethodIsAdded_ShouldBeEnhancement()
		{
			//Arrange
			var versioning = BuildVersioning();
			var testSample = typeof(AddingMethod::Run00.TestSample.Order).Assembly.Location;

			//Act
			var result = versioning.Calculate(new[] { testSample }, "Test.Sample.ControlGroup");

			//Assert
			//Assert.AreEqual(ContractChangeType.Enhancement, result.Justification.ChangeType);
			Assert.AreEqual("1.1.0.0", result.New.ToString());
		}

		[TestMethod, CategorizeByConvention]
		public void WhenMethodIsModified_ShouldBeBreaking()
		{
			//Arrange
			var versioning = BuildVersioning();
			var testSample = typeof(ChangingMethodSignature::Run00.TestSample.Order).Assembly.Location;

			//Act
			var result = versioning.Calculate(new[] { testSample }, "Test.Sample.ControlGroup");

			//Assert
			//Assert.AreEqual(ContractChangeType.Breaking, result.Justification.ChangeType);
			Assert.AreEqual("2.0.0.0", result.New.ToString());
		}

		[TestMethod, CategorizeByConvention]
		public void WhenNamespaceIsChanged_ShouldBeBreaking()
		{
			//Arrange
			var versioning = BuildVersioning();
			var testSample = typeof(ChangingNamespace::Run00.NewNamespace.Order).Assembly.Location;

			//Act
			var result = versioning.Calculate(new[] { testSample }, "Test.Sample.ControlGroup");

			//Assert
			//Assert.AreEqual(ContractChangeType.Breaking, result.Justification.ChangeType);
			Assert.AreEqual("2.0.0.0", result.New.ToString());
		}

		[TestMethod, CategorizeByConvention]
		public void WhenGenericIsChanged_ShouldBeBreaking()
		{
			//Arrange
			var versioning = BuildVersioning();
			var testSample = typeof(ChangingGenericType::Run00.TestSample.Order).Assembly.Location;

			//Act
			var result = versioning.Calculate(new[] { testSample }, "Test.Sample.ControlGroup");

			//Assert
			//Assert.AreEqual(ContractChangeType.Breaking, result.Justification.ChangeType);
			Assert.AreEqual("2.0.0.0", result.New.ToString());
		}

		[TestMethod, CategorizeByConvention]
		public void WhenPrivateMethodIsAdded_ShouldBeRefactor()
		{
			//Arrange
			var versioning = BuildVersioning();
			var testSample = typeof(AddingPrivateMethod::Run00.TestSample.Order).Assembly.Location;

			//Act
			var result = versioning.Calculate(new[] { testSample }, "Test.Sample.ControlGroup");

			//Assert
			//Assert.AreEqual(ContractChangeType.Refactor, result.Justification.ChangeType);
			Assert.AreEqual("1.0.1.0", result.New.ToString());
		}

		private ISemanticVersioning BuildVersioning()
		{
			var currentDir = Directory.GetCurrentDirectory();
			var packageRepository = PackageRepositoryFactory.Default.CreateRepository(currentDir);
			var packageManager = new PackageManager(packageRepository, Path.GetTempPath());
			var comparer = new ContractComparer();

			var versioning = (ISemanticVersioning)new SemanticVersioning(packageRepository, packageManager, comparer);
			return versioning;
		}
	}
}
