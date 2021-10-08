module Luxena
{

	export class SemanticDomain
	{

		entity<TSemanticEntity extends SemanticEntity>(se: TSemanticEntity): TSemanticEntity
		{
			se.init();
			return se;
		}

		entityByOData(data, targetEntity?: SemanticEntity): SemanticEntity
		{
			if (targetEntity && !targetEntity._isAbstract)
				return targetEntity;

			let name = <string>ko.unwrap(data["_Type"] || data["@odata.type"]);
			name = name && name.replace(`#${config.serverNamespace}.`, "");

			if (name)
			{
				const entity = this[name];
				if (entity)
					return entity;
			}

			if (!targetEntity)
				return null;

			return targetEntity._getBaseEntity && targetEntity._getBaseEntity() || targetEntity;
		}

	}

}