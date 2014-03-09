using System.Collections.Generic;

namespace Run00.SemVer.Cecil
{
	internal interface IDefinitionComparer<T>
	{
		IEnumerable<Difference> Compare(T neo, T paleo);
	}
}
