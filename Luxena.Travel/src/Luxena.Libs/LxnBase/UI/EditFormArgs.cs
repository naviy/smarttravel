using System;
using System.Collections;

using LxnBase.Data;
using SysType = System.Type;

namespace LxnBase.UI
{
	public sealed class EditFormArgs
	{
		public EditFormArgs(object id, string type, Dictionary fieldValues, RangeRequest @params, GenericOneArgDelegate onSave,
			AnonymousDelegate onCancel, LoadMode mode, bool isCopy)
		{
			IdToLoad = id;
			Type = type;
			FieldValues = fieldValues;
			RangeRequest = @params;
			OnSave = onSave;
			OnCancel = onCancel;
			Mode = mode;
			IsCopy = isCopy;

			
		}

		public object IdToLoad;
		public object Id { get { return IsCopy ? null : IdToLoad; } }
		public bool IsNew { get { return IsCopy || !Script.IsValue(IdToLoad); } }
		public object Instance;
		public string Type;
		public Dictionary FieldValues;
		public RangeRequest RangeRequest;
		public GenericOneArgDelegate OnSave;
		public AnonymousDelegate OnCancel;
		public LoadMode Mode;
		public bool IsCopy;
	}
}