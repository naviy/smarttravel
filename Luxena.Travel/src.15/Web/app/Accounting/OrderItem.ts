module Luxena.Views
{
	registerEntityControllers(sd.OrderItem, se => ({

		list: [
			se.Order,
			se.Position,
			se.Text,
			se.GrandTotal,
		],

		view: [
			se.Order,
			se.Product,
			se.LinkType,
			se.Text,
			se.Quantity,
			se.Price,
			se.Discount,
			se.GrandTotal,
		],
		
		edit: [
			se.Order,
			se.Product,
			se.Text,
			se.Quantity,
			se.Price,
			se.Discount,
			se.GrandTotal,
		],
		
	}));

}