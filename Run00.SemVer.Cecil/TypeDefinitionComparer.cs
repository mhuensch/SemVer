//using Mono.Cecil;
//using System.Collections.Generic;

//namespace Run00.SemVer.Cecil
//{
//	public class TypeDefinitionComparer : IDefinitionComparer<TypeDefinition>
//	{
//		IEnumerable<Difference> IDefinitionComparer<TypeDefinition>.Compare(TypeDefinition neo, TypeDefinition plaeo)
//		{
//			var result = new List<Difference>();

//			if (neo.FullName.Equals(plaeo.FullName) == false)
//			{
//				result.Add(new Difference() { Name = neo.Name, Reason = Difference.ChangeReason.Added });
//				result.Add(new Difference() { Name = plaeo.Name, Reason = Difference.ChangeReason.Removed });
//			}

//			return result;
//		}
//	}
//}
