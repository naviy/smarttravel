module Luxena
{
	export var language: string = "ru";// navigator.language || navigator.browserLanguage || "ru";
//	if (language == "ru-RU" || language == "ua" || language == "uk" || language == "uk-UA")
//		language = "ru";

	Globalize.culture(language);

	
	export var app: DevExpress.framework.html.HtmlApplication;

	
	$(() =>
	{
		DevExpress.devices.current({ platform: "generic" });


		app = new DevExpress.framework.html.HtmlApplication({
			namespace: Views,
			layoutSet: DevExpress.framework.html.layoutSets[config.layoutSet || "desktop"],
			mode: "webSite",
			navigation: config.menu,
			commandMapping: config.commandMapping,
		});

		$(window).unload(() => app.saveState());

		app.router.register(":view/:id", { view: config.startupView || "home", id: undefined });
		app.navigate();
	});

}