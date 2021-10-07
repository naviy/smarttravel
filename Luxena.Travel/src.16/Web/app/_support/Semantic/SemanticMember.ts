module Luxena
{
	
	export enum SemanticMemberKind
	{
		Primary = 1,
		Important,
		Secondary,
		Utility,
	}



	export interface ISemanticMember extends SemanticMember { }

	export class SemanticMember extends SemanticObject<SemanticMember>
	{
		_type: SemanticFieldType;
		_enumType: any;
		_enumIsFlags: boolean;
		_enumList: Array<{ Id: string; Name: string; }>;
		_original: SemanticMember;
		_members: SemanticMembers<any>;

		private _lookupEntity: SemanticEntity;
		_lookupGetter: () => SemanticEntity;
		getLookupEntity = () => this._lookupEntity || (this._lookupEntity = this._lookupGetter && this._lookupGetter());
		_collectionItemEntity: () => SemanticEntity;
		_collectionItemMasterMember: (se: SemanticEntity) => SemanticMember;


		_kind: SemanticMemberKind;

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
		_groupIndex: number;
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

		//_isCard: boolean;
		_placeholder: string;

		_optionChanged: boolean = true;


		//#region SemanticTypes

		bool()
		{
			this._type = FieldTypes.Bool.Bool;
			return this.initType();
		}

		boolSet(members?: SemanticMembers<any>)
		{
			this._type = FieldTypes.BoolSet.BoolSet;
			this._members = members;
			return this.initType();
		}

		//#region Date

		date()
		{
			this._type = FieldTypes.DateTime.Date;
			return this.initType();
		}

		monthAndYear()
		{
			this._type = FieldTypes.DateTime.MonthAndYear;
			return this.initType();
		}

		quarterAndYear()
		{
			this._type = FieldTypes.DateTime.QuarterAndYear;
			return this.initType();
		}

		year()
		{
			this._type = FieldTypes.DateTime.Year;
			return this.initType();
		}


		dateTime()
		{
			this._type = FieldTypes.DateTime.DateTime;
			return this.initType();
		}

		dateTime2()
		{
			this._type = FieldTypes.DateTime.DateTime2;
			return this.initType();
		}

		time()
		{
			this._type = FieldTypes.DateTime.Time;
			return this;
		}

		time2()
		{
			this._type = FieldTypes.DateTime.Time;
			return this.initType();
		}

		//#endregion


		//#region Number

		float(precision?: number)
		{
			this._type = FieldTypes.Numeric.Float;
			this._precision = precision;
			return this.initType();
		}

		int()
		{
			this._type = FieldTypes.Numeric.Int;
			return this.initType();
		}

		percent()
		{
			this._type = FieldTypes.Numeric.Percent;
			return this.initType();
		}

		//#endregion
		
		
		enum(enumType: any, ...enumListIds: string[])
		{
			this._type = FieldTypes.Enum.Enum;
			this._enumType = enumType;
			this._enumIsFlags = enumType._isFlags;

			this._maxLength = enumType["_maxLength"];

			if (enumListIds && enumListIds.length)
				this._enumList = enumListIds.map(a => enumType._items[a]);

			return this.initType();
		}

		enumIsFlags(value?: boolean)
		{
			this._enumIsFlags = value !== false;
			return this;
		}


		currencyCode()
		{
			this._type = FieldTypes.CurrencyCode.CurrencyCode;
			return this.initType();
		}

		money()
		{
			this._type = FieldTypes.Money.Money;
			this._isMoney = true;
			return this.initType();
		}

		defaultMoney()
		{
			this._type = FieldTypes.Money.Money;
			this._isMoney = true;
			return this.initType();
		}

		lookup(lookupGetter: () => SemanticEntity)
		{
			this._type = FieldTypes.Lookup.Reference;

			this._lookupGetter = lookupGetter;

			return this.initType();
		}

		col(members?: SemanticMembers<any>)
		{
			this._type = FieldTypes.FieldColumn.FieldColumn;

			if (members)
				this._members = members;

			return this.initType();
		}

		row(members?: SemanticMembers<any>)
		{
			this._type = FieldTypes.FieldRow.FieldRow;

			if (members)
				this._members = members;

			return this.initType();
		}


		//#region Text

		string(maxLength?: number, length?: number, minLength?: number)
		{
			this._type = FieldTypes.Text.String;

			if (maxLength)
				this._maxLength = maxLength;

			if (length)
				this._length = length;

			if (minLength)
				this._minLength = minLength;

			return this.initType();
		}

		text(lineCount?: number)
		{
			this._type = FieldTypes.Text.Text;

			if (lineCount)
				this._lineCount = lineCount;

			return this.initType();
		}

		codeText(lineCount?: number)
		{
			this._type = FieldTypes.Text.CodeText;

			if (lineCount)
				this._lineCount = lineCount;

			return this.initType();
		}

		password()
		{
			this._type = FieldTypes.Text.Password;
			return this.initType();
		}

		confirmPassword(passwordField: string)
		{
			this._type = FieldTypes.Text.Password;
			return this.initType();
		}

		lineCount(value: number)
		{
			this._lineCount = value;
			return this.initType();
		}

		email()
		{
			this._type = FieldTypes.Text.Email;

			if (!this._length)
				this._length = 18;

			return this.initType();
		}

		phone()
		{
			this._type = FieldTypes.Text.Phone;

			if (!this._length)
				this._length = 14;

			return this.initType();
		}

		fax()
		{
			this._type = FieldTypes.Text.Fax;

			if (!this._length)
				this._length = 14;

			return this.initType();
		}

		address(lineCount?: number)
		{
			this._type = FieldTypes.Text.Address;

			if (!this._length)
				this._length = 24;

			if (lineCount)
				this._lineCount = lineCount;

			return this.initType();
		}

		hyperlink()
		{
			this._type = FieldTypes.Text.Hyperlink;

			if (!this._length)
				this._length = 18;

			return this.initType();
		}

		//#endregion

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
			return this.field().hidden();
		}

		reserved()
		{
			return this.field().reserved();
		}

		header1()
		{
			return this.field().header1();
		}

		header2()
		{
			return this.field().header2();
		}

		header3()
		{
			return this.field().header3();
		}

		header4()
		{
			return this.field().header4();
		}

		header5()
		{
			return this.field().header5();
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

		//card(value?: boolean)
		//{
		//	this._isCard = value !== false;
		//	return this;
		//}

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

		fit()
		{
			this._length = 0;
			return this;
		}

		format(value: string)
		{
			this._format = value;
			return this;
		}

		groupIndex(value: number)
		{
			this._groupIndex = value;
			return this;
		}
		
		ungroup()
		{
			this._groupIndex = -1;
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

		items(...members: SemanticMembers<any>[])
		{
			this._members = members;
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

		sortOrder(value?: string)
		{
			this._sortOrder = value || "asc";
			return this;
		}

		totalSum(value?: boolean)
		{
			this._useTotalSum = value !== false;
			return this;
		}

		//#endregion
		

		initType()
		{
			this._type.initMember(this);
			return this;
		}

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

		getModel(model: any): KnockoutObservable<any>
		{
			return this._type.getModel(model, this._name);
		}

		setModel(model: any, value: any)
		{
			return this._type.setModel(model, this._name, value);
		}

		filter(model, operation?: string): [string, string, any]
		{
			const value = this.get(model);
			if (!value) return undefined;

			return this.getFilterExpr(value, operation);
		}


		field()
		{
			return new Field(this);
		}


		toTab()
		{
			return {
				title: this._title,
				template: this._name,
			};
		}


		getIconHtml(icon?: string, withTitle?: boolean)
		{
			return super.getIconHtml(icon || this._icon || this._type._icon, withTitle);
		}

		getLength()
		{
			const sm = this;
			let minLength = sm._minLength;
			let maxLength = sm._maxLength;
			let length = sm._length;

			if (length === undefined)
				length = sm._maxLength;

			if (length === undefined)
			{
				const ref = sm.getLookupEntity();
				if (ref)
				{
					const name = ref._nameMember;
					if (!name)
					{
						$log_(sm);
						throw Error("SemanticMember._nameMember is null");
					}

					length = name._length;
					minLength = name._minLength;
					maxLength = name._maxLength;
				}
			}

			if (length === undefined)
				length = sm._type && sm._type.length;

			return {
				length: length,
				min: minLength || sm._minLength,
				max: maxLength || sm._maxLength,
			};
		}
	}
	
}