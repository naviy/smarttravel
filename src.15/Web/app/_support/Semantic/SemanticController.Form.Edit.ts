module Luxena
{

	export interface IEditFormControllerConfig extends FormControllerConfig
	{
	}


	export abstract class BaseEditFormController<TConfig extends IEditFormControllerConfig> extends FormController<TConfig>
	{

		editMode = true;

		validators: { [memberName: string]: Object; } = {};


		getScope(): any
		{
			const scope = super.getScope();

			scope.validators = this.validators;

			return scope;
		}

	}


	export class EditFormController extends BaseEditFormController<IEditFormControllerConfig>
	{

		getRedirectUriToEntityType(entityTypeName: string)
		{
			return { view: entityTypeName + "Edit", id: this.getId() };
		}

		getMenuItems(): any[]
		{
			return [
				{
					icon: "save",
					text: "Сохранить",
					onExecute: () => this.save(),
				}, {
					icon: "ellipsis-v",
					items: [
						{
							icon: "check-square-o",
							text: "проверить",
							onExecute: () => this.validate(),
						}, {
							icon: "refresh",
							text: "Обновить",
							onExecute: () => this.refresh(),
						}, {
							text: "$log(model)",
							onExecute: () => $log_(ko.unwrap2(this.model)),
						},
					],
				}
			];
		}


		loadData(onLoaded?: () => void)
		{
			const cfg = this.config;
			const se = cfg.entity;

			if (this.getId())
			{
				super.loadData(onLoaded);
			}
			else if (se._store)
			{
				let prms = this.params || {};
				delete prms.view;

				this.loadFromData(prms);
				prms = this.saveToData();

				for (let name in prms)
				{
					if (!prms.hasOwnProperty(name)) continue;

					const prm = prms[name];
					if (prm === null || prm == undefined)
						delete prms[name];
				}

				const ds = this.dataSourceConfig = this.getDataSourceConfig();

				se.loadDefaults(prms, <string[]>ds.select)
					.done(defaults =>
					{
						this.loadFromData(defaults, true);
						onLoaded && onLoaded();
					})
					.fail(() => this.modelIsReady.resolve());
			}
			else
			{
				this.loadFromData({}, true);
				onLoaded && onLoaded();
			}
		}


		validate(): boolean
		{
			var validateGroup = DevExpress.validationEngine.getGroupConfig("edit-form");

			if (!validateGroup) return true;

			var validateResult = validateGroup.validate();

			if (!validateResult.isValid)
				console.log("validateResult: ", validateResult);

			return validateResult.isValid;
		}


		saveToData(): any
		{
			var data = this.data || {};

			this.config.entity._members.forEach(sm =>
			{
				if (sm._isNonsaved)
					sm._type.removeFromData(sm, data);
			});

			this.fields.forEach(sf =>
			{
				if (!sf.member._isNonsaved)
					sf.saveToData(this.model, data);
			});

			return data;
		}

		save(): void
		{
			if (!this.validate())
				return;

			this.loadingMessage("Сохранение...");
			this.modelIsLoading(true);

			this.config.entity
				.save(this.getId(), this.saveToData())
				.done(() =>
				{
					this.modelIsLoading(false);
					app.back();
				})
				.fail(() => this.modelIsLoading(false));
		}


		isRecalculating: boolean;

		recalc(propertyName?: string): void
		{
			if (this.isRecalculating) return;

			this.isRecalculating = true;

			//$log("recalc");
			const data = this.saveToData();
			const se = this.config.entity;
			se.recalc({
				id: this.getId(),
				data: data,
				propertyName: propertyName,
				select: <string[]>this.dataSourceConfig.select,
			})
				.done((id, data) =>
				{
					//s$log(data);
					try
					{
						this.fields.forEach(sf => sf.loadFromData(this.model, data));
					}
					finally
					{
						this.isRecalculating = false;
					}
				})
				.fail(() => this.isRecalculating = false);
		}
	}



}