using System.Runtime.CompilerServices;


namespace LxnBase.Data
{

	public partial class RangeRequest
	{
		[AlternateSignature]
		public extern void SetDefaultSort(string sort);

		public void SetDefaultSort(string sort, bool dir)
		{
			if (Sort != null) return;

			Sort = sort;
			Dir = dir ? "ASC" : "DESC";
		}
	}

}
