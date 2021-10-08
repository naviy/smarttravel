//module Luxena.layouts
//{
//
//	DX.framework.html.OverlayLayoutControllerBase = DX.framework.html.DefaultLayoutController.inherit({
//		ctor(options)
//		{
//			options = options || {};
//
//			this.callBase(options);
//
//			if (!options.childController)
//			{
//				this._ensureChildController("SimpleLayoutController", "SimpleLayout");
//				this.childController = new DX.framework.html.SimpleLayoutController();
//			}
//			else
//				this.childController = options.childController;
//
//			this.contentContainerSelector = options.contentContainerSelector;
//		},
//
//		_initChildController(options)
//		{
//			const $targetViewPort = this._$mainLayout.find(this.contentContainerSelector);
//
//			this.childController.init($.extend({}, options, { $viewPort: $targetViewPort }));
//
//			["viewRendered", "viewShowing", "viewReleased"].forEach(callbacksPropertyName =>
//			{
//				this.childController.on(callbacksPropertyName, args =>
//					this.fireEvent(callbacksPropertyName, [args])
//				);
//			});
//		},
//
//		_ensureChildController(controllerName, layoutName)
//		{
//			if (!DX.framework.html[controllerName])
//				throw new Error(controllerName + " is not found but it is required by the '" + this.name + "' layout for specified platform and device. Make sure the " + layoutName + ".* files are referenced in your main *.html file or specify other platform and device.");
//		},
//
//		_base()
//		{
//			return DX.framework.html.DefaultLayoutController.prototype;
//		},
//
//		_showContainerWidget: DX.abstract,
//		_hideContainerWidget: DX.abstract,
//
//		init(options)
//		{
//			options = options || {};
//			this.callBase(options);
//			this._initChildController(options);
//		},
//
//		activate($target)
//		{
//			this.childController.activate();
//			this._base().activate.call(this, $target);
//			var result = this._showContainerWidget($target);
//			return result;
//		},
//
//		deactivate()
//		{
//			const result = this._hideContainerWidget();
//
//			result.done(() =>
//			{
//				this._base().deactivate.call(this);
//				this.childController.deactivate();
//			});
//
//			return result;
//		},
//
//		showView(viewInfo, direction)
//		{
//			return this.childController.showView(viewInfo, direction);
//		}
//	});
//
//
//	DX.framework.html.PopupLayoutController = DX.framework.html.OverlayLayoutControllerBase.inherit({
//		ctor(options)
//		{
//			options = options || {};
//			options.name = options.name || "popup";
//			options.contentContainerSelector = options.contentContainerSelector || ".child-controller-content";
//
//			this.isOverlay = true;
//			this._targetContainer = options.targetContainer;
//			this.callBase(options);
//		},
//
//		init(options)
//		{
//			this.callBase(options);
//			this._popup = this._$mainLayout.find(".popup-container").dxPopup("instance");
//			if (this._targetContainer)
//				this._popup.option("container", this._targetContainer);
//		},
//
//		_showContainerWidget()
//		{
//			return this._popup.show();
//		},
//
//		_hideContainerWidget()
//		{
//			return this._popup.hide();
//		}
//	});
//
//	var layoutSets = DX.framework.html.layoutSets;
//
//	["agent"].forEach(name =>
//	{
//		layoutSets[name] = layoutSets[name] || [];
//
//		layoutSets[name].forEach(a => a.modal = false);
//
//		layoutSets[name].push({
//			modal: true,
//			platform: "generic",
//			controller: new DX.framework.html.PopupLayoutController()
//		});
//	});
//
//}