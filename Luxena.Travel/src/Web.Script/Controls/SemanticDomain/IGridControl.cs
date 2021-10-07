using Ext;


namespace Luxena.Travel
{

	public interface IGridControl 
	{
		void LoadData(object[] data);

		object[] GetData();

		bool IsModified();

		Component Widget { get; }
	}

}