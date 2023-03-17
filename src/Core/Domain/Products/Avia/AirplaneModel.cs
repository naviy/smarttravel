using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;




namespace Luxena.Travel.Domain
{



	//===g






	[RU("Модель самолёта", "Модели самолётов (типы судов)")]
	[SupervisorPrivileges]
	public partial class AirplaneModel : Entity3
	{

		//---g



		[RU("IATA код"), MaxLength(3), Required]
		public virtual string IataCode { get; set; }

		[RU("ICAO код"), MaxLength(4)]
		public virtual string IcaoCode { get; set; }



		//---g



		public class Service : Entity3Service<AirplaneModel>
		{

			#region Modify


			public AirplaneModel Resolve(AirplaneModel r)
			{

				if (!db.IsNew(r))
					return r;


				if (r.Name.No())
				{
					if (r.IataCode.Yes())
						return By(a => a.IataCode == r.IataCode);

					r.Name = r.IataCode;
				}

				
				return Save(r);

			}



			[DebuggerStepThrough]
			public static AirplaneModel operator +(AirplaneModel r, Service service)
			{
				return r == null ? null : service.Resolve(r);
			}


			#endregion

		}



		//---g

	}






	//===g



}