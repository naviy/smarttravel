using Ext.form;


namespace LxnBase.UI
{
	public class FieldActions
	{
		public FieldActions(Field field, FieldActionGetActionsDelegate getActions)
		{
			_field = field;
			_getActions = getActions;
		}

		public Field Field
		{
			get { return _field; }
		}

		public FieldActionGetActionsDelegate GetActions
		{
			get { return _getActions; }
		}

		private readonly Field _field;
		private readonly FieldActionGetActionsDelegate _getActions;
	}
}