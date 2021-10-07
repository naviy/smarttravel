module Luxena.Components
{

	export class Html extends SemanticComponent<Html>
	{
		constructor(public _html: string)
		{
			super();
		}

		render(container: JQuery)
		{
			if (this._html)
				container.append(this._html);
		}
	}

}