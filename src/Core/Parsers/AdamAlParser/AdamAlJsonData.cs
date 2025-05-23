using Newtonsoft.Json;




namespace Luxena.Travel.Parsers
{



	//===g






	public class AdamAlJsonData
	{

		//---g


		// Tickets
		
		[JsonProperty("type_of_issue")]
		public string TypeOfIssue { get; set; }

		[JsonProperty("date_of_issue")]
		public string DateOfIssue {get; set;}

		[JsonProperty("booking_number")]
		public string BookingNumber {get; set;}

		[JsonProperty("agent_ID")]
		public string AgentID{get; set;}

		[JsonProperty("passenger_title")]
		public string PassengerTitle{get; set;}

		[JsonProperty("passenger_name")]
		public string PassengerName {get; set;}

		[JsonProperty("passenger_surname")]
		public string PassengerSurname {get; set;}

		[JsonProperty("route")]
		public string Route {get; set;}

		[JsonProperty("baggage")]
		public string Baggage{get; set;}

		[JsonProperty("seat")]
		public string Seat {get; set;}

		[JsonProperty("flight_number")]
		public string FlightNumber {get; set;}

		[JsonProperty("departure_date")]
		public string DepartureDate{get; set;}

		[JsonProperty("arrival_date")]
		public string ArrivalDate{get; set;}

		[JsonProperty("grand_total")]
		public string GrandTotal{get; set;}

		[JsonProperty("booking_date")]
		public string BookingDate{get; set;}

		[JsonProperty("ticketing_date")]
		public string TicketingDate{get; set;}



		// Hotel
		

		[JsonProperty("guests")]
		public string Guests{get; set;}

		[JsonProperty("city")]
		public string City{get; set;}

		[JsonProperty("room_type")]
		public string RoomType {get; set;}

		[JsonProperty("meal_type")]
		public string MealType {get; set;}

		[JsonProperty("hotel_name")]
		public string HotelName {get; set;}

		[JsonProperty("checkin_date")]
		public string CheckinDate {get; set;}

		[JsonProperty("checkout_date")]
		public string CheckoutDate {get; set;}



		// Insurance


		[JsonProperty("area")]
		public string Area {get; set;}

		[JsonProperty("country")]
		public string Country{get; set;}

		[JsonProperty("insurance_type")]
		public string InsuranceType {get; set;}

		[JsonProperty("start_date")]
		public string StartDate{get; set;}

		[JsonProperty("end_date")]
		public string EndDate {get; set;}

		[JsonProperty("city_from")]
		public string CityFrom {get; set;}

		[JsonProperty("city_to")]
		public string CityTo {get; set;}

		[JsonProperty("service_type")]
		public string ServiceType{get; set;}

		[JsonProperty("vehicle_type")]
		public string VehicleType{get; set;}



		//---g

	}






	//===g



}