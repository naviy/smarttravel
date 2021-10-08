module Luxena.Components
{

	export abstract class Container<TContainer> extends SemanticComponent<TContainer>
	{
		_items: SemanticMembers<any>;
		_components: SemanticComponent<any>[];


		items(...value: SemanticMembers<any>[])
		{
			this._items = value;
			return <TContainer><any>this;
		}

		addItemsToController(action?: (sm: SemanticMember, sc: SemanticComponent<any>) => void)
		{
			this._components = this._controller.addComponents(this._items, this, null, action);
		}

	}

}