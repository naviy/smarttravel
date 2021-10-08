module Luxena
{

	export enum SemanticMemberKind
	{
		Primary = 1,
		Important,
		Secondary,
		Utility,
	}

	export enum SemanticMemberVisibility
	{
		Hidden = 1,
		Reserve,
	}


	export interface ISemanticMember extends SemanticMember { }

	export class SemanticMember extends SemanticObject<SemanticMember>
	{
		_type: SemanticType;
		_enumType: any;
		_enumIsFlags: boolean;
		_enumList: Array<{ Id: string; Name: string; }>;
		_original: SemanticMember;

		private _reference: SemanticEntity;
		_lookupGetter: () => SemanticEntity;
		getReference = () => this._reference || (this._reference = this._lookupGetter && this._lookupGetter());
		_collectionItemEntity: () => SemanticEntity;
		_collectionItemMasterMember: (se: SemanticEntity) => SemanticMember;


		_kind: SemanticMemberKind;
		_visibility: SemanticMemberVisibility;

		_isEntityDate: boolean;
		_isEntityName: boolean;
		_isEntityPosition: boolean;
		_isEntityType: boolean;
		_isMoney: boolean;

		_isCalculated: boolean;
		_isNonsaved: boolean;
		_isReadOnly: boolean;
		_isSubject: boolean;


		_allowFiltering: boolean = true;
		_allowGrouping: boolean = true;
		_sortOrder: string;
		_required: boolean;
		_unique: boolean;
		_format: string;
		_length: number;
		_minLength: number;
		_maxLength: number;
		_lineCount: number;
		_precision: number;
		_defaultValue: any;
		_columnFixed: boolean;
		_columnVisible = true;
		_selectRequired: boolean;
		_useTotalSum: boolean;
		_width: number;

		//_dependencies: SemanticObject[] = [];

		_placeholder: string;

		_optionChanged: boolean = true;


		//#region SemanticTypes

		bool()
		{
			this._type = BoolSemanticType.Bool;
			return this;
		}


		//#region Date

		date()
		{
			this._type = DateSemanticType.Date;
			return this;
		}

		monthAndYear()
		{
			this._type = DateSemanticType.MonthAndYear;
			return this;
		}

		quarterAndYear()
		{
			this._type = DateSemanticType.QuarterAndYear;
			return this;
		}

		year()
		{
			this._type = DateSemanticType.Year;
			return this;
		}


		dateTime()
		{
			this._type = DateSemanticType.DateTime;
			return this;
		}

		dateTime2()
		{
			this._type = DateSemanticType.DateTime2;
			return this;
		}

		time()
		{
			this._type = DateSemanticType.Time;
			return this;
		}

		time2()
		{
			this._type = DateSemanticType.Time;
			return this;
		}

		//#endregion


		//#region Number

		float(precision?: number)
		{
			this._type = NumericSemanticType.Float;
			this._precision = precision;
			return this;
		}

		int()
		{
			this._type = NumericSemanticType.Int;
			return this;
		}

		percent()
		{
			this._type = NumericSemanticType.Percent;
			return this;
		}

		//#endregion
		
		
		enum(enumType: any, ...enumListIds: string[])
		{
			this._type = EnumSemanticType.Enum;
			this._enumType = enumType;
			this._enumIsFlags = enumType._isFlags;

			this._maxLength = enumType["_maxLength"];

			if (enumListIds && enumListIds.length)
				this._enumList = enumListIds.map(a => enumType._items[a]);

			return this;
		}
		
		enumIsFlags(value?: boolean)
		{
			this._enumIsFlags = value !== false;
			return this;
		}

		currencyCode()
		{
			this._type = CurrencyCodeSemanticType.CurrencyCode;
			return this;
		}

		money()
		{
			this._type = MoneySemanticType.Money;
			this._isMoney = true;
			return this;
		}

		defaultMoney()
		{
			this._type = MoneySemanticType.Money;
			this._isMoney = true;
			return this;
		}

		lookup(lookupGetter: () => SemanticEntity)
		{
			this._type = LookupSemanticType.Reference;

			this._lookupGetter = lookupGetter;

			return this;
		}


		string(maxLength?: number, length?: number, minLength?: number)
		{
			this._type = TextSemanticType.String;

			if (maxLength)
				this._maxLength = maxLength;

			if (length)
				this._length = length;

			if (minLength)
				this._minLength = minLength;

			return this;
		}

		text(lineCount?: number)
		{
			this._type = TextSemanticType.Text;

			if (lineCount)
				this._lineCount = lineCount;

			return this;
		}

		codeText(lineCount?: number)
		{
			this._type = TextSemanticType.CodeText;

			if (lineCount)
				this._lineCount = lineCount;

			return this;
		}
		
		password()
		{
			this._type = TextSemanticType.Password;
			return this;
		}

		confirmPassword(passwordField: string)
		{
			this._type = TextSemanticType.Password;
			return this;
		}

		lineCount(value: number)
		{
			this._lineCount = value;
			return this;
		}

		//#endregion


		//#region Tags


		entityDate()
		{
			this._isEntityDate = true;
			this._kind = SemanticMemberKind.Primary;
			this._optionChanged = true;
			return this;
		}

		entityName()
		{
			this._isEntityName = true;
			this._kind = SemanticMemberKind.Primary;
			this._entity._nameMember = this;
			this._optionChanged = true;
			return this;
		}

		entityType()
		{
			this._isEntityType = true;
			this._kind = SemanticMemberKind.Primary;
			this._optionChanged = true;
			return this;
		}

		entityPosition()
		{
			this._isEntityPosition = true;
			this._kind = SemanticMemberKind.Primary;
			this._optionChanged = true;
			return this;
		}

		importent(value?: boolean)
		{
			this._kind = SemanticMemberKind.Important;

			return this;
		}


		secondary()
		{
			this._kind = SemanticMemberKind.Secondary;
			this._optionChanged = true;
			return this;
		}
		
		utility()
		{
			this._kind = SemanticMemberKind.Utility;
			this._optionChanged = true;
			return this;
		}


		hidden()
		{
			this._visibility = SemanticMemberVisibility.Hidden;
			this._optionChanged = true;
			return this;
		}

		reserve()
		{
			this._visibility = SemanticMemberVisibility.Reserve;
			this._optionChanged = true;
			return this;
		}


		prepare()
		{
			if (!this._optionChanged) return;


			if (this._isEntityDate)
			{
				//this._allowGrouping = true;
			}
			else if (this._isEntityName)
			{
			}
			else if (this._isEntityType)
			{
			}
			else if (this._isEntityPosition)
			{
				this._allowGrouping = false;
			}
			else if (this._kind === SemanticMemberKind.Utility)
			{
				this._columnVisible = false;
			}		
				
			
			if (this._visibility === SemanticMemberVisibility.Hidden)
			{
				this._columnVisible = false;
				this._selectRequired = true;
			}
			else if (this._visibility === SemanticMemberVisibility.Reserve)
			{
				this._columnVisible = false;
			}


			this._optionChanged = false;
		}

		//#endregion


		//#region Setters

		//dependencies(value: SemanticObject[])
		//{
		//	if (value)
		//	{
		//		value.forEach(a =>
		//		{
		//			if (this._dependencies.indexOf(a) < 0)
		//				this._dependencies.push(a);
		//		});
		//	}

		//	return this;
		//}

		allowFiltering(value: boolean)
		{
			this._allowFiltering = value;
			return this;
		}

		fixed(value?: boolean)
		{
			this._columnFixed = value !== false;
			return this;
		}

		defaultValue(value: any)
		{
			this._defaultValue = value;
			return this;
		}

		emptyText(value: string)
		{
			this._placeholder = value;
			return this;
		}

		format(value: string)
		{
			this._format = value;
			return this;
		}

		length(value: number, min?: number, max?: number)
		{
			this._length = value;

			if (min)
				this._minLength = min;

			if (max)
				this._maxLength = max;

			return this;
		}

		maxLength(value: number)
		{
			this._maxLength = value;
			return this;
		}

		minLength(value: number)
		{
			this._minLength = value;
			return this;
		}

		precision(value: number)
		{
			this._precision = value;
			return this;
		}

		required(value?: boolean)
		{
			this._required = value !== false;
			return this;
		}

		subject(value?: boolean)
		{
			this._isSubject = value !== false;
			return this;
		}

		title(value: string)
		{
			this._title = value;
			return this;
		}

		titlePrefix(value: string)
		{
			this._title = (value || "") + (this._title || "");
			return this;
		}

		titlePostfix(value: string)
		{
			this._title = (this._title || "") + (value || "");
			return this;
		}

		localizeTitle(value: ILocalization)
		{
			this._localizeTitle(value);
			return this;
		}

		ru(value: string)
		{
			this._localizeTitle({ ru: value });
			return this;
		}

		//en(value: string)
		//{
		//	this._localizeTitle({ en: value });
		//	return this;
		//}

		//ua(value: string)
		//{
		//	this._localizeTitle({ ua: value });
		//	return this;
		//}

		width(value: number)
		{
			this._width = value;
			return this;
		}

		unique(value?: boolean)
		{
			this._unique = value !== false;
			return this;
		}

		calculated(value?: boolean)
		{
			this._isCalculated = value !== false;
			return this;
		}

		nonsaved(value?: boolean)
		{
			this._isNonsaved = value !== false;
			return this;
		}

		readOnly(value?: boolean)
		{
			this._isReadOnly = value !== false;
			return this;
		}

		totalSum(value?: boolean)
		{
			this._useTotalSum = value !== false;
			return this;
		}

		//#endregion
		

		getFilterExpr(value: any, operation?: string)
		{
			return this._type.getFilterExpr(this, value, operation);
		}

		clone(cfg?: ISemanticMember)
		{
			const clone = <SemanticMember>$.extend(new SemanticMember(), this, cfg);
			clone._original = this;

			return clone;
		}

		data(data: any): any
		{
			return this._type.getFromData(this, data);
		}

		get(model: any): any
		{
			return ko.unwrap(this._type.getModel(model, this._name));
		}

		getModel(model: any): any
		{
			return this._type.getModel(model, this._name);
		}

		filter(model, operation?: string): [string, string, any]
		{
			const value = this.get(model);
			if (!value) return undefined;

			return this.getFilterExpr(value, operation);
		}

	}


	export class SemanticCollectionMember<TEntity extends SemanticEntity> extends SemanticMember
	{

		toGrid(
			members: (se: TEntity) => SemanticMembers<TEntity>,
			cfg?: IGridControllerConfigExt)
		{
			if (!this._collectionItemEntity || !this._collectionItemMasterMember)
				throw Error("Свойство " + this._entity._name + "." + this._name + " не является коллекцией");

			const se = this._collectionItemEntity();
			var masterMember = this._collectionItemMasterMember(se);

			const sf = new SemanticGridField(se, () => masterMember, members, cfg);
			sf._name = this._name;

			if (this._title && this._title !== this._name)
				sf.title(this._title);

			return sf;
		}

		toTab(
			ctrl: SemanticController,
			members?: (se: TEntity) => SemanticMembers<TEntity>,
			cfg?: IGridControllerConfigExt)
		{
			var sf = this.toGrid(members, cfg);
			return sf.toTab(ctrl);
		}

	}

}