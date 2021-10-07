module Luxena.Views
{

	sd.GdsAgent._lookupItemTemplate = r => `<b class="span-150">${r.Person}</b> ${r.Codes}`;


	registerEntityControllers(sd.GdsAgent, se => ({

		members: [
			se.Person,
			se.Origin,
			se.OfficeCode,
			se.Code,
			se.Office,
		],

		actions: [
			sd.GdsAgent_ApplyToUnassigned.toAction(prms => ({
				 GdsAgentId: prms.id
			})),
		],

	}));


	registerEntityControllers(sd.GdsAgent_ApplyToUnassigned, se => ({
		edit: [
			se.GdsAgent,
			se.DateFrom,
			se.DateTo,
			se.ProductCount,
		],
	}));

}