using System;

using Luxena.Base.Serialization;


namespace Luxena.Base.Data
{

	[DataContract]
	public class OperationStatus
	{
		public OperationStatus() {}

		public OperationStatus(string disableInfo)
		{
			DisableInfo = disableInfo;
		}

		public static OperationStatus Enabled()
		{
			return new OperationStatus { Visible = true };
		}

		public static OperationStatus Hidden()
		{
			return new OperationStatus { Visible = false };
		}

		public bool IsEnabled => Visible && string.IsNullOrEmpty(DisableInfo);

		public bool IsDisabled => Visible && !string.IsNullOrEmpty(DisableInfo);

		public bool IsHidden => !Visible && string.IsNullOrEmpty(DisableInfo);

		public bool Visible { get; set; }

		public string DisableInfo { get; set; }


		public static implicit operator bool(OperationStatus me)
		{
			return me != null && me.IsEnabled;
		}

		public static implicit operator OperationStatus(bool me)
		{
			return me ? Enabled() : Hidden();
		}


		public void Assert()
		{
			if (IsEnabled) return;

			throw new OperationDeniedException(DisableInfo);
		}

		public void Assert(string exceptionMessage)
		{
			if (IsEnabled) return;

			throw new OperationDeniedException(
				(exceptionMessage != null ? exceptionMessage + "\r\n" : null) + DisableInfo
			);
		}

		public void Assert(Func<string> exceptionMessage)
		{
			if (IsEnabled) return;

			throw new OperationDeniedException(
				(exceptionMessage != null ? exceptionMessage() + "\r\n" : null) + DisableInfo
			);
		}

	}

}