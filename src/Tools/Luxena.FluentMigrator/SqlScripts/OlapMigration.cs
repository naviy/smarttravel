using FluentMigrator;


namespace Luxena.Travel.DbMigrator.Olap
{

	[Profile("OLAP")]
	public class OlapMigration : ForwardOnlyMigration
	{

		public override void Up()
		{
			foreach (var script in new[]
			{
				"olap_airline_dim.sql",
				"olap_direction_dim.sql",
				"olap_airport_dim.sql",
				"olap_backdeparture_dim.sql",
				"olap_bookeroffice_dim.sql",
				"olap_carrier_dim.sql",
				"olap_country_dim.sql",
				"olap_currency_dim.sql",
				"olap_currency_rate.sql",
				"olap_customer_dim.sql",
				"olap_departuredate_dim.sql",
				"olap_document.sql",
				"olap_fare_currency_dim.sql",
				"olap_fare_segment_dim.sql",
				"olap_flighttype_dim.sql",
				"olap_gds_dim.sql",
				"olap_intermediary_dim.sql",
				"olap_issuedate_dim.sql",
				"olap_itinerary_dim.sql",
				"olap_number_dim.sql",
				"olap_order_dim.sql",
				"olap_owner_dim.sql",
				"olap_passenger_dim.sql",
				"olap_paymenttype_dim.sql",
				"olap_segment_dim.sql",
				"olap_segmentclass_dim.sql",
				"olap_seller_dim.sql",
				"olap_settlement_dim.sql",
				"olap_ticketeroffice_dim.sql",
				"olap_ticketingiataoffice_dim.sql",
				"olap_ticketingiataoffice_dim.sql",
				"olap_tourcode_dim.sql",
				"olap_transaction_dim.sql",
				"olap_type_dim.sql",
			})
			{
				Execute.EmbeddedScript(script);
			}
		}

	}

}
