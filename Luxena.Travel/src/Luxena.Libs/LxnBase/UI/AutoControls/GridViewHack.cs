using System.Runtime.CompilerServices;

using Ext;
using Ext.grid;


namespace LxnBase.UI.AutoControls
{
	[Imported]
	public class GridViewHack : GridView
	{
		[IntrinsicProperty]
		public ColumnModel Cm
		{
			get { return null; }
			set { }
		}

		[IntrinsicProperty]
		public Ext.menu.Menu Hmenu
		{
			get { return null; }
			set { }
		}

		[IntrinsicProperty]
		public Element MainHd
		{
			get { return null; }
			set { }
		}

		[IntrinsicProperty]
		public int HdCtxIndex
		{
			get { return 0; }
			set { }
		}
	}
}