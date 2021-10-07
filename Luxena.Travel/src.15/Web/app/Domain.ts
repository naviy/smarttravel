module Luxena
{
	var endpointSelector = new DevExpress.EndpointSelector(config.endpoints);


	var serviceConfig = $.extend(true, {}, config.services, 
	{
		db: <DevExpress.data.ODataContextOptions>{
			url: endpointSelector.urlFor("db"),

			//jsonp: true,

			version: 4,

			errorHandler: showError,

			beforeSend(request: any)
			{
				//if (request.method === "MERGE")
				//{
				//	request.headers['X-HTTP-Method'] = request.method;
				//	request.method = "PATCH";
				//}
				
				const prms = request.params;
				if (prms && prms.$select)
				{
					if (prms.$select.indexOf(",$usecalculated") >= 0)
					{
						prms.$select = prms.$select.replace(",$usecalculated", "");
						prms.usecalculated = true;
					}

					if (prms.$select.indexOf(",$recalc") >= 0)
					{
						prms.$select = prms.$select.replace(",$recalc", "");
						prms.recalc = true;
					}
				}
			}
		}
	});


	export var db = new Domain(serviceConfig.db);


	export function showError(error: Error | string)
	{
		let msg = "";

		if (typeof error === "string")
		{
			console.error("ERROR: " + error);
		}
		else
		{
			let err = error["errorDetails"] || error;

			if (err && err.message)
			{
				let priorMsg = "";
				while (err)
				{
					if (err.message !== priorMsg)
						msg = `${msg ? "\r\n" : ""}<li>${err.message}</li>${msg}` ;

					priorMsg = err.message;
					err = err["innererror"] || err["internalexception"];
				}
			}

			error.message = msg;

			console.error(error.name + ": " + msg);
		}

		DevExpress.ui.notify({
			type: "error",
			displayTime: 50000,
			closeOnOutsideClick: true,
			contentTemplate: `<ul style="display: table-cell; padding-left: 25px">${msg}</ul>`,
		});
	}

}