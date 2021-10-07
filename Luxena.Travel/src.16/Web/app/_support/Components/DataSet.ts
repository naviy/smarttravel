namespace Luxena.Components_
{
	import DataSource = DevExpress.data.DataSource;


	export interface IDataLoadOptions
	{
		select?: string[];
		expand?: string[];
	}


	export abstract class DataSet
	{
		_dataSource: DataSource;


		_details: Array<{
			source: DataSet;
			filterSource: (master: DataSet, detail: DataSet) => boolean;
		}> = [];

		_components: Component[] = [];

		_current: any;
		//_prior: { [name: string]: any; };

		_selecteds: any[];


		dataSource()
		{
			return this._dataSource;
		}

		prepare()
		{
			this._components.forEach(a => a.prepare());

			const loadOptions = DataSet.concatLoadOptions(this._components);
			//$log(loadOptions);

			const ds = this._dataSource;
			ds.select(loadOptions.select);
			(<any>ds.loadOptions()).expand = loadOptions.expand;
		}

		static concatLoadOptions(components: Component[])
		{
			if (!components) return undefined;

			const select = ["Id", "Version"]; //TODO
			const expand = [];

			for (let cmp of components)
			{
				const opt = cmp.loadOptions();
				if (!opt) continue;

				select.register(opt.select);
				expand.register(opt.expand);
			}

			return <IDataLoadOptions>{
				select: select,
				expand: expand,
			};
		}

		detail(source: DataSet, filterSource: (master: DataSet, detail: DataSet) => boolean)
		{
			this._details.push({ source: source, filterSource: filterSource });
			return this;
		}

		current()//data?: any)
		{
			//if (data === undefined)
			return this._current;
			//return this;
		}


		filter(value: any)
		{

			return true;
		}


		select(data: Object | Object[], container?: JQuery)
		{
			if (!data)
			{
				this._selecteds = null;
				this._current = null;
			}
			else if (isArray(data))
			{
				this._selecteds = data;
				this._current = data.length ? data[0] : null;
			}
			else
			{
				this._selecteds = [data];
				this._current = data;
			}


			this.repaint(container);
		}

		repaint(container?: JQuery)
		{
			this._components.forEach(a => a.repaint(container));
		}

		abstract load(id, defaults?): JQueryPromise<any>;
		//abstract loadDefaults(): JQueryPromise<any>;
		abstract update(): JQueryPromise<any>;
		abstract delete(): JQueryPromise<any>;

	}


	export class EntityDataSet extends DataSet
	{
		constructor(public _entity: Semantic.Entity)
		{
			super();
			this._dataSource = new DataSource({
				store: this._entity._store,
			});
		}
		

		load(id): JQueryPromise<any>
		{
			/*this._entity._lookupFields.id*/
			this._dataSource.filter(["Id", "=", id]);
			return this._dataSource.load().done(data => this.select(data));
		}

		update(): JQueryPromise<any>
		{
			return null;
		}

		delete(): JQueryPromise<any>
		{
			return null;
		}

	}

/*

	в компонент

	export class PartyBalanceForm extends Form
	{

		init()
		{
			this.title(sd.Party);

			var dsParties = uiSource(sd.Party);

			var dsBuyPoducts = uiSource(sd.Products, {
				master: dsParty,
				detailField: sd.Product.Customer,
			});

			var dsSellPoducts = uiSource(sd.Products, {
				master: dsParty,
				detailField: sd.Product.Seller,
			});


			this.addItems({
				fields: [
					uiGrid(dsParties),

					uiGrid(dsBuyPoducts)
						.items(se => [ ... ]),
					uiToolbox()
						.master(dsBuyPoducts)
						.items((se: IProduct) => [se.smart]),
	
					uiGrid(dsSellPoducts)
						.items(se => [ ... ]),
					uiFieldSet(uiSource({
						entity: dsSellPoducts.currentEntity,
						master: dsSellPoducts,
					}))
						.items((se: IProduct) => [se.smart]),
				],
			});
		}
	}



	Container: _items => _placeholders
*/



}