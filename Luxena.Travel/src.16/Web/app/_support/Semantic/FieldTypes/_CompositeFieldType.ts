module Luxena
{

	export abstract class CompositeFieldType extends SemanticFieldType
	{
		_memberColumnVisible = false;
		_isComposite = true;

		addItemsToController(sf: Field, ctrl: SemanticController, action?: (sm: SemanticMember, sc: SemanticComponent<any>) => void)
		{
			sf._components = ctrl.addComponents(sf._members || sf._member._members, sf, null, (sm2, sc2) =>
			{
				if (!this._memberColumnVisible)
					sc2.columnVisible(false);

				if (isField(sc2))
					sc2._type.addItemsToController(sc2, ctrl, action);

				action && action(sm2, <Field>sc2);
			});
		}

		
		getSelectFieldNames(sf: Field)
		{
			return [];
		}

		loadFromData(sf: Field, model: any, data: any): void
		{
		}

		saveToData(sf: Field, model: any, data: any): void
		{
		}


		getDisplayValueVisible(sf: Field, model)
		{
			var widgets = sf._controller.widgets;

			return () =>
			{
				var visible = false;

				sf._components.forEach(sc2 =>
				{
					if (sc2.isComposite())
						visible = true;
					else
					{
						var widget = widgets[sc2.uname()];
						visible = visible || widget && (!widget.valueVisible || widget.valueVisible());
					}
				});

				return visible;
			};
		}

	}
}