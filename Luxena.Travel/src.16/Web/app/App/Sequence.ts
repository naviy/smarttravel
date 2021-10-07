module Luxena.Views
{

	registerEntityControllers(sd.Sequence, se => [
		se.Name,
		se.Format,
		se.Discriminator,
		se.Current,
		se.Timestamp,
	]);

}