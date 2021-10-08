module Luxena
{

	export interface IEntitySemantic extends ISemanticEntity
	{
		Id: SemanticMember;
		Version: SemanticMember;
	}


	export class EntitySemantic extends SemanticEntity implements IEntitySemantic
	{
		Id = this.member().string().utility();
		Version = this.member().int().utility();
	}


	export interface IDomainActionSemantic extends ISemanticEntity
	{
		Id: SemanticMember;
	}


	export class DomainActionSemantic extends SemanticEntity implements IDomainActionSemantic
	{
		Id = this.member().string().utility();
	}


	//export class FloatByDateSemantic extends EntitySemantic
	//{
	//	X = this.member().date();
	//	Y = this.member().float(2);
	//}


	export interface IEntity2Semantic extends IEntitySemantic
	{
		/** Дата создания */
		CreatedOn: SemanticMember;

		/** Создано пользователем */
		CreatedBy: SemanticMember;

		/** Дата изменения */
		ModifiedOn: SemanticMember;

		/** Изменено пользователем */
		ModifiedBy: SemanticMember;

		HistoryTab: Field;
	}

	export class Entity2Semantic extends EntitySemantic implements IEntity2Semantic
	{
		/** Дата создания */
		CreatedOn = this.member()
			.localizeTitle({ ru: "Дата создания" })
			.dateTime2()
			.required()
			.utility();

		/** Создано пользователем */
		CreatedBy = this.member()
			.localizeTitle({ ru: "Создано пользователем" })
			.string()
			.utility();

		/** Дата изменения */
		ModifiedOn = this.member()
			.localizeTitle({ ru: "Дата изменения" })
			.dateTime2()
			.utility();

		/** Изменено пользователем */
		ModifiedBy = this.member()
			.localizeTitle({ ru: "Изменено пользователем" })
			.string()
			.utility();

		HistoryTab: Field;

		constructor()
		{
			super();

			const se = this;

			se.HistoryTab = se.member0().col().field().icon("history").items(
				se.CreatedBy,
				se.CreatedOn,
				se.ModifiedBy,
				se.ModifiedOn
			);
		}
	}


	export interface IEntity3Semantic extends IEntity2Semantic
	{
		/** Название */
		Name: SemanticMember;
	}

	export class Entity3Semantic extends Entity2Semantic implements IEntity3Semantic
	{
		/** Название */
		Name = this.member()
			.localizeTitle({ ru: "Название" })
			.string()
			.entityName();
	}


	export interface IEntity3DSemantic extends IEntity3Semantic
	{
		/** Описание */
		Description: SemanticMember;
	}

	export class Entity3DSemantic extends Entity3Semantic implements IEntity3DSemantic
	{
		/** Описание */
		Description = this.member()
			.localizeTitle({ ru: "Описание" })
			.text(4);
	}


}