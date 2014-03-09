using Mono.Cecil;
using System.Collections.Generic;

namespace Run00.SemVer.Cecil
{
	public class PackageDefinition
	{
		public ICollection<TypeDefinition> TypeDefinitions { get; set; }
		public ICollection<MethodDefinition> MethodDefinitions { get; set; }
		public ICollection<FieldDefinition> FieldDefinitions { get; set; }

		public PackageDefinition()
		{
			TypeDefinitions = new List<TypeDefinition>();
			MethodDefinitions = new List<MethodDefinition>();
			FieldDefinitions = new List<FieldDefinition>();
		}
	}
}
