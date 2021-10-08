using System;

using Luxena.Travel.Services;


namespace Luxena.Travel
{

	public abstract class AviaDocumentEditForm : ProductEditForm
	{

		public override void Open()
		{
			if (Script.IsValue(_args.IdToLoad))
				AviaService.GetAviaDocument(_args.IdToLoad, _args.Type, true, OnLoad, null);
			else
				OnLoad(null);
		}

		protected override string GetNameBy(object data)
		{
			AviaDocumentDto dto = (AviaDocumentDto)data;
			return string.Format("{0}-{1}", dto.AirlinePrefixCode, dto.Number);
		}

	}

}