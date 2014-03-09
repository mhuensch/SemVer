//using Mono.Cecil;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Run00.SemVer.Cecil
//{
//	public class ContractComparer : IContractComparer
//	{

//		IEnumerable<Difference> IContractComparer.Compare(IMemberDefinition neo, IMemberDefinition paleo)
//		{
//			//TODO: Solve this with DI
//			if (neo is TypeDefinition && paleo is TypeDefinition)
//			{
//				var x = new TypeDefinitionComparer() as IDefinitionComparer<TypeDefinition>;
//				return x.Compare((TypeDefinition)neo, (TypeDefinition)paleo);
//			}
//			else if (neo is MethodDefinition && paleo is MethodDefinition)
//			{
//				var x = new MethodDefinitionComparer() as IDefinitionComparer<MethodDefinition>;
//				return x.Compare((MethodDefinition)neo, (MethodDefinition)paleo);
//			}


//			return Enumerable.Empty<Difference>();
//		}

//	}
//}
