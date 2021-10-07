module Luxena.Components
{

	export class Card extends Container<Card>
	{

		_isComposite = true;


		render(container: JQuery)
		{
			container.addClass("card card-fieldset");

			this._components.forEach(sc2 =>
			{
				if (this._indentLabelItems)
					sc2.indentLabel();

				if (this._labelAsHeaderItems)
					sc2.labelAsHeader();
				else if (this._unlabelItems)
					sc2.unlabel();
				else if (this._hideLabelItems)
					sc2.hideLabel();

				sc2.render(container);
			});
		}

	}

}