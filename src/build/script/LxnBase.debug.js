//! LxnBase.debug.js
//

////////////////////////////////////////////////////////////////////////////////

Ext.ns('LxnBase.Data');

LxnBase.Data.WebServiceProxy = function (service, method) {
	LxnBase.Data.WebServiceProxy.superclass.constructor.call(this);
	this._service = service;
	this._method = method;
};

Ext.extend(LxnBase.Data.WebServiceProxy, Ext.data.DataProxy,
{
	useGet: false,
	wrapper: null,
	arguments: null,

	setResponse: function (response) {
		this._response = response;
	},

	load: function (params, reader, callback, scope, arg) {
		if (arg && params && !arg.params)
			arg.params = params;

		if (this.fireEvent("beforeload", this, params) === false) {
			callback.call(scope || this, null, arg, false);
			return;
		}

		var handleResponse = ss.Delegate.create(this, function (response) {
			if (typeof response.Start == 'number')
				arg.params.start = response.Start;

			if (response.Sort)
				arg.params.sort = response.Sort;

			if (response.Dir)
				arg.params.dir = response.Dir;

			var records;
			try {
				records = reader.readRecords(response);
			} catch (e) {
				this.fireEvent("loadexception", this, response, arg, e);
				callback.call(scope, null, arg, false);
				return;
			}
			this.fireEvent("load", this, response, arg);
			callback.call(scope || window, records, arg, true);
		});

		if (this._response) {
			handleResponse(this._response);
			this._response = null;

			return;
		}

		Ext.apply(arg, this.arguments);

		this._service.invoke(this._method, arg, this.useGet, this.wrapper,

			handleResponse,

			ss.Delegate.create(this, function(error) {
				this.fireEvent("loadexception", this, null, arg);
				callback.call(scope || window, null, arg, false);
			})
		);
	},

	_request: null,
	_method: null,
	_response: null
});

LxnBase.Data.RangeReader = function(meta, recordType){
	meta = meta || {};
	LxnBase.Data.RangeReader.superclass.constructor.call(this, meta, recordType || meta.fields);
};

LxnBase.Data.RangeReader = Ext.extend(Ext.data.JsonReader, {
	readRecords: function(o) {
		var sid = this.meta.id;
		var root = this.createAccessor(this.meta.root)(o);

		var records = [];
		var totalRecords;

		if (this.meta.totalProperty) {
			totalRecords = this.createAccessor(this.meta.totalProperty)(o);
			if (isNaN(totalRecords)) {
				throw 'Invalid total records value';
			}
		}

		for (var i = 0; i < root.length; i++) {
			var n = root[i];
			var id = ((sid || sid === 0) && n[sid] !== undefined && n[sid] !== "" ? n[sid] : null);
			var record = new this.recordType(this.readData(n), id);
			record.json = n;
			records[records.length] = record;
		}

		return {
			records: records,
			totalRecords: totalRecords === null ? records.length : totalRecords
		};
	},

	readData: function(n) {
		var fields = this.recordType.prototype.fields;
		var data = {};
		for (var j = 0, jlen = fields.length; j < jlen; j++) {
			var field = fields.items[j];
			var pos = field.mapping !== undefined && field.mapping !== null ? field.mapping : j;
			data[field.name] = n[pos] !== undefined ? field.convert(n[pos], n) : field.defaultValue;
		}
		return data;
	}
});

LxnBase.Data.GenericStore = function(config) {
	if (config && config.baseParams)
		this.baseParams = config.baseParams;
	else {
		this.baseParams = {
			start: null,
			limit: null,
			sort: null,
			dir: null,
			searchMode: null,
			query: null,
			conditions: null
		};
	}

	LxnBase.Data.GenericStore.superclass.constructor.call(this, config);
}

Ext.extend(LxnBase.Data.GenericStore, Ext.data.Store, {
	get_start: function() { return this.baseParams.start; },
	set_start: function(value) { this.baseParams.start = value; },

	get_limit: function() { return this.baseParams.limit; },
	set_limit: function(value) { this.baseParams.limit = value; },

	get_sortField: function() { return this.baseParams.sort; },
	set_sortField: function(value) { this.baseParams.sort = value; },

	get_sortDirection: function() { return this.baseParams.dir; },
	set_sortDirection: function(value) { this.baseParams.dir = value; },

	get_searchMode: function() { return this.baseParams.searchMode; },
	set_searchMode: function(value) { this.baseParams.searchMode = value; },

	get_suggestionQuery: function() { return this.baseParams.query; },
	set_suggestionQuery: function(value) { this.baseParams.query = value; },

	get_filter: function() { return this.baseParams.filter; },
	set_filter: function(value) { this.baseParams.filter = value; },

	load: function(options) {
		options = options || {};
		if(this.fireEvent("beforeload", this, options) !== false) {
				this.storeOptions(options);

				var p = this.baseParams || {};
				for (var paramName in options.params)
					p[paramName] = options.params[paramName];
				options.params = p;

				if(this.sortInfo && this.remoteSort) {
					var pn = this.paramNames;
					p[pn["sort"]] = this.sortInfo.field;
					p[pn["dir"]] = this.sortInfo.direction;
				}

				this.proxy.load(p, this.reader, this.loadRecords, this, options);
				return true;
		} else {
			return false;
		}
	}
});

////////////////////////////////////////////////////////////////////////////////

Ext.ns('LxnBase.UI.Controls.ColumnFilters');

LxnBase.UI.pathToImages = "img/";

LxnBase.UI.Controls.ColumnFilters.FilterMenuItem = function(cfg) {
	LxnBase.UI.Controls.ColumnFilters.FilterMenuItem.superclass.constructor.call(this, cfg);

	if (cfg && cfg.menuIcon)
		this.menuIcon = cfg.menuIcon;

	if (cfg && cfg.editor)
		this.editor = cfg.editor;

	if (cfg && cfg.editorEvents)
		this.editorEvents = cfg.editorEvents;
}

LxnBase.UI.Controls.ColumnFilters.FilterMenuItem = Ext.extend(Ext.menu.BaseItem, {
	itemCls: "x-menu-item",
	hideOnClick: false,

	initComponent: function() {
		if (!this.menuIcon)
			this.menuIcon = LxnBase.UI.pathToImages + "find.png";

		LxnBase.UI.Controls.ColumnFilters.FilterMenuItem.superclass.initComponent.call(this);

		this.addEvents("keyup");
		this.addEvents("keydown");
		this.addEvents("change");

		this.editor = this.editor || new Ext.form.TextField();

		if (this.text) {
			this.editor.setValue(this.text);
		}
	},

	onRender: function(container) {
		var s = container.createChild({
			cls: this.itemCls,
			html: '<img src="' + this.menuIcon + '" style="margin: 3px 0px 0px 2px; vertical-align: top;" />'
		});

		Ext.apply(this.config, {width: 125});

		this.editor.render(s);

		if (this.editorEvents) {
			for (var i = 0; i < this.editorEvents.length; i++)
				this.editor.on(this.editorEvents[i], this._onChange, this);
		}

		this.editor.on("keydown", this._onKeyDown, this);
		this.editor.on("keyup", this._onKeyUp, this);

		this.el = s;
		this.relayEvents(this.editor.el, ["keyup"]);
		this.relayEvents(this.editor.el, ["keydown"]);

		if (Ext.isGecko) {
			s.setStyle('overflow', 'auto');
		}

		LxnBase.UI.Controls.ColumnFilters.FilterMenuItem.superclass.onRender.call(this, container);
	},

	getValue: function() {
		return this.editor.getValue();
	},

	setValue: function(value) {
		this.editor.setValue(value);
	},

	isValid: function(preventMark) {
		return this.editor.isValid(preventMark);
	},

	_onChange: function() {
		this.fireEvent("change", this);
	},

	_onKeyUp: function(field, e) {
		var key = e.getKey();

		this.fireEvent("keyup", this);

		if (e == e.ENTER) {
			e.stopEvent();
			e.preventDefault();

			this._onChange();
		}
	},

	_onKeyDown: function(field, e) {
		this.fireEvent("keydown", this);
	}
});


////////////////////////////////////////////////////////////////////////////////

Ext.ns('Ext.ux.layout');

Ext.ux.layout.CenterLayout = Ext.extend(Ext.layout.FitLayout, {
	// private
	setItemSize: function(item, size){
		this.container.addClass('ux-layout-center');
		item.addClass('ux-layout-center-item');
		if(item && size.height > 0){
			if(item.width){
				size.width = item.width;
			}
			item.setSize(size);
		}
	}
});
Ext.Container.LAYOUTS['ux.center'] = Ext.ux.layout.CenterLayout;

Ext.ux.layout.RowLayout = Ext.extend(Ext.layout.ContainerLayout, {
	// private
	monitorResize:true,

	// private
	isValidParent: function(c, target){
		return c.getEl().dom.parentNode == this.innerCt.dom;
	},

	// private
	onLayout: function(ct, target){
		var rs = ct.items.items, len = rs.length, r, i;

		if(!this.innerCt){
			target.addClass('ux-row-layout-ct');
			this.innerCt = target.createChild({cls:'x-row-inner'});
		}
		this.renderAll(ct, this.innerCt);

		var size = target.getViewSize();

		if(size.width < 1 && size.height < 1){ // display none?
			return;
		}

		var h = size.height - target.getPadding('tb'),
				ph = h;

		this.innerCt.setSize({height:h});

		// some rows can be percentages while others are fixed
		// so we need to make 2 passes

		for(i = 0; i < len; i++){
			r = rs[i];
			if(!r.rowHeight){
				ph -= (r.getSize().height + r.getEl().getMargins('tb'));
			}
		}

		ph = ph < 0 ? 0 : ph;

		for(i = 0; i < len; i++){
			r = rs[i];
			if(r.rowHeight){
				r.setSize({height: Math.floor(r.rowHeight*ph) - r.getEl().getMargins('tb')});
			}
		}
	}
});
Ext.Container.LAYOUTS['ux.row'] = Ext.ux.layout.RowLayout;


////////////////////////////////////////////////////////////////////////////////

if (this.JSON && !this.JSON.hasMsFix) {
	var reIsoDate = /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)Z$/;
	var reParseMsDate = /\/Date\((-?[0-9]+)([\+\-][0-9]+)?\)\//;
	var reFixMsDate = /(^|[^\\])\"(\/Date\(-?[0-9]+([\+\-][0-9]+)?\))\/\"/g;
	
	var parse = JSON.parse;
	var stringify = JSON.stringify;
	
	var hasStringifyIssue = parse(stringify({ a: 1 }, function (key, value) { if (key == 'a') return 2; return value; })).a == 1;

	JSON.parse = function(json) {
		try {
			var res = parse(json,
				function(key, value) {
					if (typeof value === 'string') {
						if (reIsoDate.exec(value)) {
							return new Date(value);
						}
						var groups = reParseMsDate.exec(value);
						if (groups) {
							if (groups[2]) {
								return new Date((+groups[1]) + (+groups[2])*60000);
							}
							var d = new Date(+groups[1]);
							return new Date(d.getUTCFullYear(), d.getUTCMonth(), d.getUTCDate(), d.getUTCHours(), d.getUTCMinutes(), d.getUTCSeconds(), d.getUTCMilliseconds());
						}
					}
					return value;
				});
			return res;
		} catch (e) {
			throw new Error("JSON content could not be parsed");
			return null;
		}
	};

	JSON.stringify = function(json) {
		if (json === undefined) {
			return json;
		}

		if (hasStringifyIssue) {
			json = parse(stringify(json));
		}

		return stringify(json, function(key, value) {
			if (typeof value == "string" && reIsoDate.exec(value)) {
				var d = new Date(value);

				var s = '/Date(' + Date.UTC(d.getFullYear(), d.getMonth(), d.getDate(), d.getHours(), d.getMinutes(), d.getSeconds()) + ')/';

				if (hasStringifyIssue) {
					this[key] = s;
				}

				return s;
			}
			return value;
		}).replace(reFixMsDate, '$1"\\$2\\/"');
	};

	JSON.hasMsFix = true;
}


////////////////////////////////////////////////////////////////////////////////

(function($) {

Type.registerNamespace('LxnBase');

////////////////////////////////////////////////////////////////////////////////
// LxnBase.BaseRes

LxnBase.BaseRes = { 
    atoGrid_UpdateRowMsg1: '\u0421\u043e\u0445\u0440\u0430\u043d\u0435\u043d\u044b \u0438\u0437\u043c\u0435\u043d\u0435\u043d\u0438\u044f \u0432 {0} \u0441\u0442\u0440\u043e\u043a\u0435',
    atoGrid_UpdateRowMsg2: '\u0421\u043e\u0445\u0440\u0430\u043d\u0435\u043d\u044b \u0438\u0437\u043c\u0435\u043d\u0435\u043d\u0438\u044f \u0432 {0} \u0441\u0442\u0440\u043e\u043a\u0430\u0445',
    autoGrid_ActionNotPermitted_Msg: '\u0423 \u0432\u0430\u0441 \u043d\u0435\u0434\u043e\u0441\u0442\u0430\u0442\u043e\u0447\u043d\u043e \u043f\u0440\u0430\u0432 \u0434\u043b\u044f \u0434\u0430\u043d\u043d\u043e\u0433\u043e \u0434\u0435\u0439\u0441\u0442\u0432\u0438\u044f. \u0415\u0441\u043b\u0438 \u0432\u0430\u043c \u043d\u0435\u043e\u0431\u0445\u043e\u0434\u0438\u043c\u044b \u043f\u0440\u0430\u0432\u0430, \u043e\u0431\u0440\u0430\u0442\u0438\u0442\u0435\u0441\u044c \u043a \u0430\u0434\u043c\u0438\u043d\u0438\u0441\u0442\u0440\u0430\u0442\u043e\u0440\u0443 \u0441\u0438\u0441\u0442\u0435\u043c\u044b',
    autoGrid_ContinueDelete_Msg: '\u041f\u0440\u043e\u0434\u043e\u043b\u0436\u0438\u0442\u044c \u0443\u0434\u0430\u043b\u0435\u043d\u0438\u0435 \u043e\u0441\u0442\u0430\u043b\u044c\u043d\u044b\u0445 \u043e\u0431\u044a\u0435\u043a\u0442\u043e\u0432?',
    autoGrid_DeleteCompletedMsg1: '\u0443\u0434\u0430\u043b\u0435\u043d\u0430 {0} \u0441\u0442\u0440\u043e\u043a\u0430',
    autoGrid_DeleteCompletedMsg2: '\u0443\u0434\u0430\u043b\u0435\u043d\u044b {0} \u0441\u0442\u0440\u043e\u043a\u0438',
    autoGrid_DeleteCompletedMsg3: '\u0443\u0434\u0430\u043b\u0435\u043d\u043e {0} \u0441\u0442\u0440\u043e\u043a',
    autoGrid_DeleteConstrainedFailed_Msg: '\u041e\u0431\u044a\u0435\u043a\u0442 \u043d\u0435 \u043c\u043e\u0436\u0435\u0442 \u0431\u044b\u0442\u044c \u0443\u0434\u0430\u043b\u0435\u043d, \u0442\u0430\u043a \u043a\u0430\u043a \u0438\u043c\u0435\u044e\u0442\u0441\u044f \u0441\u0432\u044f\u0437\u0430\u043d\u043d\u044b\u0435 \u0441 \u043d\u0438\u043c \u0434\u0430\u043d\u043d\u044b\u0435.',
    autoGrid_DeleteFailed_Msg: '\u0421\u043b\u0435\u0434\u0443\u044e\u0449\u0438\u0435 \u043e\u0431\u044a\u0435\u043a\u0442\u044b \u043d\u0435 \u043c\u043e\u0433\u0443\u0442 \u0431\u044b\u0442\u044c \u0443\u0434\u0430\u043b\u0435\u043d\u044b, \u0442\u0430\u043a \u043a\u0430\u043a \u0438\u043c\u0435\u044e\u0442\u0441\u044f \u0441\u0432\u044f\u0437\u0430\u043d\u043d\u044b\u0435 \u0441 \u043d\u0438\u043c\u0438 \u0434\u0430\u043d\u043d\u044b\u0435:',
    autoGrid_DeleteMsg1: '{0} \u0441\u0442\u0440\u043e\u043a\u0430 \u0431\u0443\u0434\u0435\u0442 \u0443\u0434\u0430\u043b\u0435\u043d\u0430. \u041f\u0440\u043e\u0434\u043e\u043b\u0436\u0438\u0442\u044c?',
    autoGrid_DeleteMsg2: '{0} \u0441\u0442\u0440\u043e\u043a\u0438 \u0431\u0443\u0434\u0443\u0442 \u0443\u0434\u0430\u043b\u0435\u043d\u044b. \u041f\u0440\u043e\u0434\u043e\u043b\u0436\u0438\u0442\u044c?',
    autoGrid_DeleteMsg3: '{0} \u0441\u0442\u0440\u043e\u043a \u0431\u0443\u0434\u0435\u0442 \u0443\u0434\u0430\u043b\u0435\u043d\u043e. \u041f\u0440\u043e\u0434\u043e\u043b\u0436\u0438\u0442\u044c?',
    autoGrid_DispayMsg: '\u041e\u0442\u043e\u0431\u0440\u0430\u0436\u0430\u044e\u0442\u0441\u044f {0} - {1} \u0438\u0437 {2}',
    autoGrid_EmptyMsg: '\u0414\u0430\u043d\u043d\u044b\u0435 \u043e\u0442\u0441\u0443\u0442\u0441\u0442\u0432\u0443\u044e\u0442',
    autoGrid_NotDisplay_Msg: '(\u043d\u0435 \u043e\u0442\u043e\u0431\u0440\u0430\u0436\u0430\u0435\u0442\u0441\u044f \u0432 \u0442\u0435\u043a\u0443\u0449\u0435\u0439 \u0444\u0438\u043b\u044c\u0442\u0440\u0430\u0446\u0438\u0438)',
    autoGrid_RefreshData: '\u043e\u0431\u043d\u043e\u0432\u043b\u044f\u0442\u044c \u0434\u0430\u043d\u043d\u044b\u0435 \u043f\u0440\u0438 \u0438\u0437\u043c\u0435\u043d\u0435\u043d\u0438\u0438',
    autoGrid_RejectChanges: '\u041e\u0442\u043c\u0435\u043d\u0438\u0442\u044c \u0438\u0437\u043c\u0435\u043d\u0435\u043d\u0438\u0435',
    autoGrid_RepeatUpdate: '\u041f\u043e\u0432\u0442\u043e\u0440\u0438\u0442\u044c \u0441\u043e\u0445\u0440\u0430\u043d\u0435\u043d\u0438\u0435',
    autoGrid_ReplaceToAdmin_Msg: '\u0414\u043b\u044f \u0443\u0434\u0430\u043b\u0435\u043d\u0438\u044f \u043e\u0431\u044a\u0435\u043a\u0442\u0430 \u0441 \u0437\u0430\u043c\u0435\u043d\u043e\u0439 \u043e\u0431\u0440\u0430\u0442\u0438\u0442\u0435\u0441\u044c \u043a \u0430\u0434\u043c\u0438\u043d\u0438\u0441\u0442\u0440\u0430\u0442\u043e\u0440\u0443.',
    autoGrid_ResetFilter_Title: '\u043e\u0442\u043c\u0435\u043d\u0438\u0442\u044c \u0444\u0438\u043b\u044c\u0442\u0440',
    autoGrid_ReturnToEditing: '\u0432\u0435\u0440\u043d\u0443\u0442\u044c\u0441\u044f \u043a \u0440\u0435\u0434\u0430\u043a\u0442\u0438\u0440\u043e\u0432\u0430\u043d\u0438\u044e',
    autoGrid_SaveChanges: '\u0441\u043e\u0445\u0440\u0430\u043d\u0438\u0442\u044c \u0438\u0437\u043c\u0435\u043d\u0435\u043d\u0438\u044f',
    autoGrid_SimpleFilter_Title: '\u0424\u0438\u043b\u044c\u0442\u0440:',
    autoGrid_SuspendUpdate: '\u041e\u0442\u043b\u043e\u0436\u0438\u0442\u044c \u0441\u043e\u0445\u0440\u0430\u043d\u0435\u043d\u0438\u0435',
    autoGrid_UnsavedChangesMsg: '\u0415\u0441\u0442\u044c \u043d\u0435\u0441\u043e\u0445\u0440\u0430\u043d\u0435\u043d\u043d\u044b\u0435 \u0438\u0437\u043c\u0435\u043d\u0435\u043d\u0438\u044f',
    autoList_AutoCommit: '\u0430\u0432\u0442\u043e\u0441\u043e\u0445\u0440\u0430\u043d\u0435\u043d\u0438\u0435',
    cancel: '\u041e\u0442\u043c\u0435\u043d\u0438\u0442\u044c',
    cancel_Lower: '\u043e\u0442\u043c\u0435\u043d\u0438\u0442\u044c',
    caption_Separator: ':',
    clear: '\u041e\u0447\u0438\u0441\u0442\u0438\u0442\u044c',
    close: '\u0417\u0430\u043a\u0440\u044b\u0442\u044c',
    confirmation: '\u041f\u043e\u0434\u0442\u0432\u0435\u0440\u0436\u0434\u0435\u043d\u0438\u0435',
    copy: '\u041a\u043e\u043f\u0438\u0440\u043e\u0432\u0430\u0442\u044c',
    copy_Lower: '\u043a\u043e\u043f\u0438\u0440\u043e\u0432\u0430\u0442\u044c',
    created: '\u0434\u043e\u0431\u0430\u0432\u043b\u0435\u043d\u043e',
    createItem: '\u0421\u043e\u0437\u0434\u0430\u0442\u044c',
    createItem_Lower: '\u0441\u043e\u0437\u0434\u0430\u0442\u044c',
    createObjectMsg: "\u0414\u043e\u0431\u0430\u0432\u043b\u0435\u043d \u043d\u043e\u0432\u044b\u0439 '{0}': {1}",
    creation: '\u0441\u043e\u0437\u0434\u0430\u043d\u0438\u0435',
    deleted: '\u0443\u0434\u0430\u043b\u0435\u043d\u043e',
    delete_Confirmation: '\u041e\u0431\u044a\u0435\u043a\u0442 \u0431\u0443\u0434\u0435\u0442 \u0443\u0434\u0430\u043b\u0435\u043d. \u041f\u0440\u043e\u0434\u043e\u043b\u0436\u0438\u0442\u044c?',
    edit: '\u0418\u0437\u043c\u0435\u043d\u0438\u0442\u044c',
    editing: '\u0440\u0435\u0434\u0430\u043a\u0442\u0438\u0440\u043e\u0432\u0430\u043d\u0438\u0435',
    edit_Lower: '\u0438\u0437\u043c\u0435\u043d\u0438\u0442\u044c',
    error: '\u041e\u0448\u0438\u0431\u043a\u0430',
    export_Action: '\u042d\u043a\u0441\u043f\u043e\u0440\u0442',
    export_All: '\u0432\u0441\u0435 ({0})',
    export_ExceptSelection: '\u0432\u0441\u0435 \u043a\u0440\u043e\u043c\u0435 \u0432\u044b\u0434\u0435\u043b\u0435\u043d\u043d\u044b\u0445 ({0})',
    export_Selection: '\u0432\u044b\u0434\u0435\u043b\u0435\u043d\u043d\u044b\u0435 ({0})',
    filterOperator_Contains: '\u0421\u043e\u0434\u0435\u0440\u0436\u0438\u0442',
    filterOperator_EndsWith: '\u0417\u0430\u043a\u0430\u043d\u0447\u0438\u0432\u0430\u0435\u0442\u0441\u044f \u043d\u0430',
    filterOperator_Equals: '\u0420\u0430\u0432\u043d\u043e',
    filterOperator_Greater: '\u0411\u043e\u043b\u044c\u0448\u0435',
    filterOperator_GreaterOrEquals: '\u0411\u043e\u043b\u044c\u0448\u0435 \u0440\u0430\u0432\u043d\u043e',
    filterOperator_IsIn: '\u0421\u0440\u0435\u0434\u0438 \u0437\u043d\u0430\u0447\u0435\u043d\u0438\u0439',
    filterOperator_IsNull: '\u041f\u0443\u0441\u0442\u043e',
    filterOperator_Less: '\u041c\u0435\u043d\u044c\u0448\u0435',
    filterOperator_LessOrEquals: '\u041c\u0435\u043d\u044c\u0448\u0435 \u0440\u0430\u0432\u043d\u043e',
    filterOperator_None: '\u041e\u0442\u0441\u0443\u0442\u0441\u0432\u0443\u0435\u0442',
    filterOperator_StartsWith: '\u041d\u0430\u0447\u0438\u043d\u0430\u0435\u0442\u0441\u044f \u0441',
    filter_AfterDate: '\u0421',
    filter_BeforeDate: '\u041f\u043e',
    filter_Conjunction: '\u0438',
    filter_False: '\u041d\u0435\u0442',
    filter_GeneralFilterMsg: "\u043e\u0434\u043d\u043e \u0438\u0437 \u043f\u043e\u043b\u0435\u0439 \u0441\u043e\u0434\u0435\u0440\u0436\u0438\u0442 '{0}'",
    filter_Not: '\u043d\u0435',
    filter_OnDate: '\u0412',
    filter_Title: '\u0424\u0438\u043b\u044c\u0442\u0440',
    filter_True: '\u0414\u0430',
    list: '\u0441\u043f\u0438\u0441\u043e\u043a',
    loading: '\u0417\u0430\u0433\u0440\u0443\u0437\u043a\u0430...',
    messages_Title: '\u0421\u043e\u043e\u0431\u0449\u0435\u043d\u0438\u044f',
    noData_Text: '\u043d\u0435\u0442 \u0434\u0430\u043d\u043d\u044b\u0445',
    propertyFilterCondition_Not: '\u041d\u0435...',
    remove: '\u0423\u0434\u0430\u043b\u0438\u0442\u044c',
    remove_Lower: '\u0443\u0434\u0430\u043b\u0438\u0442\u044c',
    replace: '\u0417\u0430\u043c\u0435\u043d\u0438\u0442\u044c',
    replaceDelete: '\u0423\u0434\u0430\u043b\u0435\u043d\u0438\u0435 \u0441 \u0437\u0430\u043c\u0435\u043d\u043e\u0439',
    replaceForm_DeleteAfterReplace: '\u0423\u0434\u0430\u043b\u044f\u0442\u044c \u043f\u043e\u0441\u043b\u0435 \u0437\u0430\u043c\u0435\u043d\u044b',
    replaceForm_DependentObjects: '\u0417\u0430\u0432\u0438\u0441\u0438\u043c\u044b\u0435 \u043e\u0431\u044a\u0435\u043a\u0442\u044b',
    replaceForm_ObjectForReplace: '\u041d\u0435\u0432\u043e\u0437\u043c\u043e\u0436\u043d\u043e \u0443\u0434\u0430\u043b\u0438\u0442\u044c {0}, \u0442\u0430\u043a \u043a\u0430\u043a \u0438\u043c\u0435\u044e\u0442\u0441\u044f \u0437\u0430\u0432\u0438\u0441\u0438\u043c\u044b\u0435 \u0434\u0430\u043d\u043d\u044b\u0435.',
    replaceForm_ReplaceComplete_Msg: '{0} \u0443\u0434\u0430\u043b\u0435\u043d\u043e \u0441 \u0437\u0430\u043c\u0435\u043d\u043e\u0439 \u043d\u0430 {1}',
    replaceForm_ReplacingObject: '\u041f\u0440\u043e\u0434\u043e\u043b\u0436\u0438\u0442\u044c \u0443\u0434\u0430\u043b\u0435\u043d\u0438\u0435 \u0441 \u0437\u0430\u043c\u0435\u043d\u043e\u0439 \u0441\u0432\u044f\u0437\u0435\u0439 \u043d\u0430:',
    salutation: '\u041e\u0431\u0440\u0430\u0449\u0435\u043d\u0438\u0435',
    salutation_Text_List: '\u041e\u0431\u0440\u0430\u0449\u0435\u043d\u0438\u044f',
    save: '\u0421\u043e\u0445\u0440\u0430\u043d\u0438\u0442\u044c',
    saveChenges_Msg: '\u0414\u0430\u043d\u043d\u044b\u0435 \u0431\u044b\u043b\u0438 \u0438\u0437\u043c\u0435\u043d\u0435\u043d\u044b. \u0421\u043e\u0445\u0440\u0430\u043d\u0438\u0442\u044c \u0438\u0437\u043c\u0435\u043d\u0435\u043d\u0438\u044f?',
    saveObjectValidationErrorMsg: '\u0412\u0432\u0435\u0434\u0435\u043d\u044b \u043d\u0435\u043a\u043e\u0440\u0440\u0435\u043a\u0442\u043d\u044b\u0435 \u0434\u0430\u043d\u043d\u044b\u0435',
    save_Lower: 'c\u043e\u0445\u0440\u0430\u043d\u0438\u0442\u044c',
    select_Lower: '\u0432\u044b\u0431\u0440\u0430\u0442\u044c',
    unknownError: '\u041d\u0435\u0432\u043e\u0437\u043c\u043e\u0436\u043d\u043e \u0432\u044b\u043f\u043e\u043b\u043d\u0438\u0442\u044c \u043e\u043f\u0435\u0440\u0430\u0446\u0438\u044e. \u041f\u043e\u0436\u0430\u043b\u0443\u0439\u0441\u0442\u0430, \u043e\u0431\u0440\u0430\u0442\u0438\u0442\u0435\u0441\u044c \u043a \u0441\u0438\u0441\u0442\u0435\u043c\u043d\u043e\u043c\u0443 \u0430\u0434\u043c\u0438\u043d\u0438\u0441\u0442\u0440\u0430\u0442\u043e\u0440\u0443',
    unreadMessageCount_Msg1: '{0} \u043d\u0435\u043f\u0440\u043e\u0447\u0438\u0442\u0430\u043d\u043d\u043e\u0435 \u0441\u043e\u043e\u0431\u0449\u0435\u043d\u0438\u0435',
    unreadMessageCount_Msg2: '{0} \u043d\u0435\u043f\u0440\u043e\u0447\u0438\u0442\u0430\u043d\u043d\u044b\u0445 \u0441\u043e\u043e\u0431\u0449\u0435\u043d\u0438\u044f',
    unreadMessageCount_Msg3: '{0} \u043d\u0435\u043f\u0440\u043e\u0447\u0438\u0442\u0430\u043d\u043d\u044b\u0445 \u0441\u043e\u043e\u0431\u0449\u0435\u043d\u0438\u0439',
    updated: '\u0438\u0437\u043c\u0435\u043d\u0435\u043d\u043e',
    updateObjectMsg: "\u0421\u043e\u0445\u0440\u0430\u043d\u0435\u043d '{0}': {1}",
    valiadationStatusHideError_Msg: '\u041d\u0430\u0436\u043c\u0438\u0442\u0435 \u0441\u043d\u043e\u0432\u0430, \u0447\u0442\u043e\u0431\u044b \u0441\u043a\u0440\u044b\u0442\u044c \u0441\u043f\u0438\u0441\u043e\u043a',
    valiadationStatusSowError_Msg: '\u0424\u043e\u0440\u043c\u0430 \u0441\u043e\u0434\u0435\u0440\u0436\u0438\u0442 \u043e\u0448\u0438\u0431\u043a\u0438 (\u043d\u0430\u0436\u043c\u0438\u0442\u0435 \u0437\u0434\u0435\u0441\u044c \u0434\u043b\u044f \u043f\u0440\u043e\u0441\u043c\u043e\u0442\u0440\u0430...)',
    view: '\u041f\u0440\u043e\u0441\u043c\u043e\u0442\u0440',
    warning: '\u041f\u0440\u0435\u0434\u0443\u043f\u0440\u0435\u0436\u0434\u0435\u043d\u0438\u0435',
    webServiceAborted: "The server method '{0}' aborted.",
    webServiceFailed: "The server method '{0}' failed with the following error: {1}",
    webServiceFailedNoMsg: "The server method '{0}' failed.",
    webServiceInvalidJsonWrapper: "The server method '{0}' returned invalid data. The '{1}' property is missing from the JSON wrapper.",
    webServiceParseError: "The server method '{0}' parse error.",
    webServiceResponse_Error_Msg: '\u041f\u0440\u043e\u0438\u0437\u043e\u0448\u043b\u0430 \u043e\u0448\u0438\u0431\u043a\u0430. \u0420\u0435\u043a\u043e\u043c\u0435\u043d\u0434\u0443\u0435\u043c \u0432\u0430\u043c \u0437\u0430\u043a\u0440\u044b\u0442\u044c \u043e\u043a\u043d\u043e \u0438 \u0437\u0430\u0433\u0440\u0443\u0437\u0438\u0442\u044c \u043f\u0440\u0438\u043b\u043e\u0436\u0435\u043d\u0438\u0435 \u0432 \u043d\u043e\u0432\u043e\u043c. \u0415\u0441\u043b\u0438 \u043e\u0448\u0438\u0431\u043a\u0430 \u0431\u0443\u0434\u0435\u0442 \u043f\u043e\u0432\u0442\u043e\u0440\u044f\u0442\u044c\u0441\u044f \u043e\u0431\u0440\u0430\u0442\u0438\u0442\u0435\u0441\u044c \u043a \u0430\u0434\u043c\u0438\u043d\u0438\u0441\u0442\u0440\u0430\u0442\u043e\u0440\u0443.',
    webServiceResponse_Error_Title: '\u041a\u0440\u0438\u0442\u0438\u0447\u0435\u0441\u043a\u0430\u044f \u043e\u0448\u0438\u0431\u043a\u0430',
    webServiceTimedOut: "The server method '{0}' timed out."
};


////////////////////////////////////////////////////////////////////////////////
// LxnBase.MessageType

LxnBase.MessageType = function() { 
    /// <field name="info" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="warn" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="error" type="Number" integer="true" static="true">
    /// </field>
};
LxnBase.MessageType.prototype = {
    info: 0, 
    warn: 1, 
    error: 2
}
LxnBase.MessageType.registerEnum('LxnBase.MessageType', false);


////////////////////////////////////////////////////////////////////////////////
// LxnBase.EnumUtility

LxnBase.EnumUtility = function LxnBase_EnumUtility() {
}
LxnBase.EnumUtility.localize = function LxnBase_EnumUtility$localize(type, value, resources) {
    /// <param name="type" type="Type">
    /// </param>
    /// <param name="value" type="ss.Enum">
    /// </param>
    /// <param name="resources" type="Type">
    /// </param>
    /// <returns type="String"></returns>
    if (value == null) {
        return null;
    }
    var name = LxnBase.EnumUtility.toString(type, value);
    var typeName = type.get_name().substr(0, 1).toLowerCase() + type.get_name().substr(1);
    var text = resources[typeName + '_' + name];
    return (String.isNullOrEmpty(text)) ? name : text;
}
LxnBase.EnumUtility.toString = function LxnBase_EnumUtility$toString(type, value) {
    /// <param name="type" type="Type">
    /// </param>
    /// <param name="value" type="ss.Enum">
    /// </param>
    /// <returns type="String"></returns>
    var d = type;
    var $dict1 = d;
    for (var $key2 in $dict1) {
        var pair = { key: $key2, value: $dict1[$key2] };
        if (pair.value === value) {
            return LxnBase.StringUtility.capitalize(pair.key);
        }
    }
    return null;
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.MessageRegister

LxnBase.MessageRegister = function LxnBase_MessageRegister() {
    /// <field name="__newMessage" type="Function" static="true">
    /// </field>
}
LxnBase.MessageRegister.add_newMessage = function LxnBase_MessageRegister$add_newMessage(value) {
    /// <param name="value" type="Function" />
    LxnBase.MessageRegister.__newMessage = ss.Delegate.combine(LxnBase.MessageRegister.__newMessage, value);
}
LxnBase.MessageRegister.remove_newMessage = function LxnBase_MessageRegister$remove_newMessage(value) {
    /// <param name="value" type="Function" />
    LxnBase.MessageRegister.__newMessage = ss.Delegate.remove(LxnBase.MessageRegister.__newMessage, value);
}
LxnBase.MessageRegister.error = function LxnBase_MessageRegister$error(message, details, handled) {
    /// <param name="message" type="String">
    /// </param>
    /// <param name="details" type="String">
    /// </param>
    /// <param name="handled" type="Boolean">
    /// </param>
    if (LxnBase.MessageRegister.__newMessage != null) {
        LxnBase.MessageRegister.__newMessage(null, new LxnBase.MessageRegisterEventArgs(2, message, details, null, handled));
    }
}
LxnBase.MessageRegister.warn = function LxnBase_MessageRegister$warn(message, details) {
    /// <param name="message" type="String">
    /// </param>
    /// <param name="details" type="String">
    /// </param>
    if (LxnBase.MessageRegister.__newMessage != null) {
        LxnBase.MessageRegister.__newMessage(null, new LxnBase.MessageRegisterEventArgs(1, message, details));
    }
}
LxnBase.MessageRegister.info = function LxnBase_MessageRegister$info(messageCaption, message, details) {
    /// <param name="messageCaption" type="String">
    /// </param>
    /// <param name="message" type="String">
    /// </param>
    /// <param name="details" type="String">
    /// </param>
    if (LxnBase.MessageRegister.__newMessage != null) {
        LxnBase.MessageRegister.__newMessage(null, new LxnBase.MessageRegisterEventArgs(0, message, details, messageCaption));
    }
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.MessageRegisterEventArgs

LxnBase.MessageRegisterEventArgs = function LxnBase_MessageRegisterEventArgs(type, message, details, messageCaption, handled) {
    /// <param name="type" type="LxnBase.MessageType">
    /// </param>
    /// <param name="message" type="String">
    /// </param>
    /// <param name="details" type="String">
    /// </param>
    /// <param name="messageCaption" type="String">
    /// </param>
    /// <param name="handled" type="Boolean">
    /// </param>
    /// <field name="_messageCaption$1" type="String">
    /// </field>
    /// <field name="_message$1" type="String">
    /// </field>
    /// <field name="_details$1" type="String">
    /// </field>
    /// <field name="_type$1" type="LxnBase.MessageType">
    /// </field>
    /// <field name="_handled$1" type="Boolean">
    /// </field>
    LxnBase.MessageRegisterEventArgs.initializeBase(this);
    this._messageCaption$1 = messageCaption;
    this._message$1 = message;
    this._details$1 = details;
    this._type$1 = type;
    this._handled$1 = handled;
}
LxnBase.MessageRegisterEventArgs.prototype = {
    
    get_type: function LxnBase_MessageRegisterEventArgs$get_type() {
        /// <value type="LxnBase.MessageType"></value>
        return this._type$1;
    },
    set_type: function LxnBase_MessageRegisterEventArgs$set_type(value) {
        /// <value type="LxnBase.MessageType"></value>
        this._type$1 = value;
        return value;
    },
    
    get_messageCaption: function LxnBase_MessageRegisterEventArgs$get_messageCaption() {
        /// <value type="String"></value>
        return this._messageCaption$1;
    },
    set_messageCaption: function LxnBase_MessageRegisterEventArgs$set_messageCaption(value) {
        /// <value type="String"></value>
        this._messageCaption$1 = value;
        return value;
    },
    
    get_message: function LxnBase_MessageRegisterEventArgs$get_message() {
        /// <value type="String"></value>
        return this._message$1;
    },
    set_message: function LxnBase_MessageRegisterEventArgs$set_message(value) {
        /// <value type="String"></value>
        this._message$1 = value;
        return value;
    },
    
    get_details: function LxnBase_MessageRegisterEventArgs$get_details() {
        /// <value type="String"></value>
        return this._details$1;
    },
    set_details: function LxnBase_MessageRegisterEventArgs$set_details(value) {
        /// <value type="String"></value>
        this._details$1 = value;
        return value;
    },
    
    get_handled: function LxnBase_MessageRegisterEventArgs$get_handled() {
        /// <value type="Boolean"></value>
        return this._handled$1;
    },
    set_handled: function LxnBase_MessageRegisterEventArgs$set_handled(value) {
        /// <value type="Boolean"></value>
        this._handled$1 = value;
        return value;
    },
    
    _messageCaption$1: null,
    _message$1: null,
    _details$1: null,
    _type$1: 0,
    _handled$1: false
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.NumberUtility

LxnBase.NumberUtility = function LxnBase_NumberUtility() {
}
LxnBase.NumberUtility.isNumber = function LxnBase_NumberUtility$isNumber(obj) {
    /// <param name="obj" type="Object">
    /// </param>
    /// <returns type="Boolean"></returns>
    return typeof(obj) === 'number' && isFinite(obj);
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.StringUtility

LxnBase.StringUtility = function LxnBase_StringUtility() {
}
LxnBase.StringUtility.getNumberText = function LxnBase_StringUtility$getNumberText(number, textFormat1, textFormat2, textFormat3) {
    /// <param name="number" type="Number" integer="true">
    /// </param>
    /// <param name="textFormat1" type="String">
    /// </param>
    /// <param name="textFormat2" type="String">
    /// </param>
    /// <param name="textFormat3" type="String">
    /// </param>
    /// <returns type="String"></returns>
    var twoLastDigits = number % 100;
    if (5 <= twoLastDigits && twoLastDigits <= 20) {
        return String.format(textFormat3, number);
    }
    var lastDigit = number % 10;
    if (lastDigit === 1) {
        return String.format(textFormat1, number);
    }
    if (lastDigit === 2 || lastDigit === 3 || lastDigit === 4) {
        return String.format(textFormat2, number);
    }
    return String.format(textFormat3, number);
}
LxnBase.StringUtility.capitalize = function LxnBase_StringUtility$capitalize(value) {
    /// <param name="value" type="String">
    /// </param>
    /// <returns type="String"></returns>
    if (String.isNullOrEmpty(value)) {
        return value;
    }
    return value.substr(0, 1).toUpperCase() + value.substr(1);
}
LxnBase.StringUtility.toString = function LxnBase_StringUtility$toString(obj) {
    /// <param name="obj" type="Object">
    /// </param>
    /// <returns type="String"></returns>
    if (ss.isNullOrUndefined(obj)) {
        return '';
    }
    if (Type.canCast(obj, Date)) {
        return (obj).format('d.m.Y');
    }
    if (('Text' in obj)) {
        return obj.Text;
    }
    return '';
}


Type.registerNamespace('LxnBase.Data');

////////////////////////////////////////////////////////////////////////////////
// LxnBase.Data.DocumentExportMode

LxnBase.Data.DocumentExportMode = function() { 
    /// <field name="all" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="selected" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="exceptSelected" type="Number" integer="true" static="true">
    /// </field>
};
LxnBase.Data.DocumentExportMode.prototype = {
    all: 0, 
    selected: 1, 
    exceptSelected: 2
}
LxnBase.Data.DocumentExportMode.registerEnum('LxnBase.Data.DocumentExportMode', false);


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Data.FilterOperator

LxnBase.Data.FilterOperator = function() { 
    /// <field name="none" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="equals" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="isNull" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="startsWith" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="contains" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="endsWith" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="less" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="lessOrEquals" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="greaterOrEquals" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="greater" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="isIn" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="isIdIn" type="Number" integer="true" static="true">
    /// </field>
};
LxnBase.Data.FilterOperator.prototype = {
    none: 0, 
    equals: 1, 
    isNull: 2, 
    startsWith: 3, 
    contains: 4, 
    endsWith: 5, 
    less: 6, 
    lessOrEquals: 7, 
    greaterOrEquals: 8, 
    greater: 9, 
    isIn: 10, 
    isIdIn: 11
}
LxnBase.Data.FilterOperator.registerEnum('LxnBase.Data.FilterOperator', false);


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Data.ObjectInfo

LxnBase.Data.ObjectInfo = function LxnBase_Data_ObjectInfo() {
    /// <field name="Id" type="Object">
    /// </field>
    /// <field name="Text" type="String">
    /// </field>
    /// <field name="Type" type="String">
    /// </field>
    /// <field name="IdPos" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="TextPos" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="TypePos" type="Number" integer="true" static="true">
    /// </field>
}
LxnBase.Data.ObjectInfo.create = function LxnBase_Data_ObjectInfo$create(type, text, id) {
    /// <param name="type" type="String">
    /// </param>
    /// <param name="text" type="String">
    /// </param>
    /// <param name="id" type="Object">
    /// </param>
    /// <returns type="LxnBase.Data.ObjectInfo"></returns>
    var info = new LxnBase.Data.ObjectInfo();
    info.Type = type;
    info.Text = text;
    info.Id = id;
    return info;
}
LxnBase.Data.ObjectInfo.copy = function LxnBase_Data_ObjectInfo$copy(source) {
    /// <param name="source" type="LxnBase.Data.ObjectInfo">
    /// </param>
    /// <returns type="LxnBase.Data.ObjectInfo"></returns>
    if (source == null) {
        return null;
    }
    var info = new LxnBase.Data.ObjectInfo();
    info.Type = source.Type;
    info.Text = source.Text;
    info.Id = source.Id;
    return info;
}
LxnBase.Data.ObjectInfo.equals = function LxnBase_Data_ObjectInfo$equals(obj1, obj2) {
    /// <param name="obj1" type="LxnBase.Data.ObjectInfo">
    /// </param>
    /// <param name="obj2" type="LxnBase.Data.ObjectInfo">
    /// </param>
    /// <returns type="Boolean"></returns>
    return obj1 != null && obj2 != null && obj1.Id === obj2.Id;
}
LxnBase.Data.ObjectInfo.prototype = {
    Id: null,
    Text: null,
    Type: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Data.DeleteOperationResponse

LxnBase.Data.DeleteOperationResponse = function LxnBase_Data_DeleteOperationResponse() {
    /// <field name="Success" type="Boolean">
    /// </field>
    /// <field name="RangeResponse" type="LxnBase.Data.RangeResponse">
    /// </field>
    /// <field name="UndeletableObjects" type="Array" elementType="Object">
    /// </field>
}
LxnBase.Data.DeleteOperationResponse.prototype = {
    Success: false,
    RangeResponse: null,
    UndeletableObjects: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Data.DocumentExportArgs

LxnBase.Data.DocumentExportArgs = function LxnBase_Data_DocumentExportArgs() {
    /// <field name="Mode" type="LxnBase.Data.DocumentExportMode">
    /// </field>
    /// <field name="Request" type="LxnBase.Data.RangeRequest">
    /// </field>
    /// <field name="SelectedDocuments" type="Array" elementType="Object">
    /// </field>
}
LxnBase.Data.DocumentExportArgs.prototype = {
    Mode: 0,
    Request: null,
    SelectedDocuments: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Data.ItemResponse

LxnBase.Data.ItemResponse = function LxnBase_Data_ItemResponse() {
    /// <field name="Item" type="Object">
    /// </field>
    /// <field name="RangeResponse" type="LxnBase.Data.RangeResponse">
    /// </field>
    /// <field name="Errors" type="Object">
    /// </field>
}
LxnBase.Data.ItemResponse.prototype = {
    Item: null,
    RangeResponse: null,
    Errors: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Data.OperationPermissions

LxnBase.Data.OperationPermissions = function LxnBase_Data_OperationPermissions() {
    /// <field name="CanUpdate" type="LxnBase.Data.OperationStatus">
    /// </field>
    /// <field name="CanDelete" type="LxnBase.Data.OperationStatus">
    /// </field>
    /// <field name="CustomActionPermissions" type="Object">
    /// </field>
}
LxnBase.Data.OperationPermissions.prototype = {
    CanUpdate: null,
    CanDelete: null,
    CustomActionPermissions: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Data.OperationStatus

LxnBase.Data.OperationStatus = function LxnBase_Data_OperationStatus() {
    /// <field name="IsEnabled" type="Boolean">
    /// </field>
    /// <field name="IsDisabled" type="Boolean">
    /// </field>
    /// <field name="IsHidden" type="Boolean">
    /// </field>
    /// <field name="Visible" type="Boolean">
    /// </field>
    /// <field name="DisableInfo" type="String">
    /// </field>
}
LxnBase.Data.OperationStatus.prototype = {
    IsEnabled: false,
    IsDisabled: false,
    IsHidden: false,
    Visible: false,
    DisableInfo: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Data.PropertyFilter

LxnBase.Data.PropertyFilter = function LxnBase_Data_PropertyFilter() {
    /// <field name="InternalPath" type="String">
    /// </field>
    /// <field name="Property" type="String">
    /// </field>
    /// <field name="Conditions" type="Array" elementType="PropertyFilterCondition">
    /// </field>
}
LxnBase.Data.PropertyFilter.prototype = {
    InternalPath: null,
    Property: null,
    Conditions: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Data.PropertyFilterCondition

LxnBase.Data.PropertyFilterCondition = function LxnBase_Data_PropertyFilterCondition() {
    /// <field name="Not" type="Boolean">
    /// </field>
    /// <field name="Operator" type="LxnBase.Data.FilterOperator">
    /// </field>
    /// <field name="Value" type="Object">
    /// </field>
}
LxnBase.Data.PropertyFilterCondition.prototype = {
    Not: false,
    Operator: 0,
    Value: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Data.RangeRequest

LxnBase.Data.RangeRequest = function LxnBase_Data_RangeRequest() {
    /// <field name="ClassName" type="String">
    /// </field>
    /// <field name="NamedFilters" type="Array" elementType="String">
    /// </field>
    /// <field name="Filters" type="Array" elementType="PropertyFilter">
    /// </field>
    /// <field name="query" type="String">
    /// </field>
    /// <field name="GeneralFilter" type="String">
    /// </field>
    /// <field name="VisibleProperties" type="Array" elementType="String">
    /// </field>
    /// <field name="HiddenProperties" type="Array" elementType="String">
    /// </field>
    /// <field name="start" type="Number" integer="true">
    /// </field>
    /// <field name="limit" type="Number" integer="true">
    /// </field>
    /// <field name="sort" type="String">
    /// </field>
    /// <field name="dir" type="String">
    /// </field>
    /// <field name="PositionableObjectId" type="Object">
    /// </field>
}
LxnBase.Data.RangeRequest.prototype = {
    ClassName: null,
    NamedFilters: null,
    Filters: null,
    query: null,
    GeneralFilter: null,
    VisibleProperties: null,
    HiddenProperties: null,
    start: 0,
    limit: 0,
    sort: null,
    dir: null,
    PositionableObjectId: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Data.RangeResponse

LxnBase.Data.RangeResponse = function LxnBase_Data_RangeResponse() {
    /// <field name="Start" type="Number" integer="true">
    /// </field>
    /// <field name="Sort" type="String">
    /// </field>
    /// <field name="Dir" type="String">
    /// </field>
    /// <field name="TotalCount" type="Number" integer="true">
    /// </field>
    /// <field name="List" type="Array" elementType="Object">
    /// </field>
    /// <field name="SelectedRow" type="Number" integer="true">
    /// </field>
}
LxnBase.Data.RangeResponse.prototype = {
    Start: 0,
    Sort: null,
    Dir: null,
    TotalCount: 0,
    List: null,
    SelectedRow: 0
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Data.ConfigManager

LxnBase.Data.ConfigManager = function LxnBase_Data_ConfigManager() {
    /// <field name="_listConfigs" type="Object" static="true">
    /// </field>
    /// <field name="_viewConfigs" type="Object" static="true">
    /// </field>
    /// <field name="_editConfigs" type="Object" static="true">
    /// </field>
}
LxnBase.Data.ConfigManager.getListConfig = function LxnBase_Data_ConfigManager$getListConfig(className, listConfigLoaded) {
    /// <param name="className" type="String">
    /// </param>
    /// <param name="listConfigLoaded" type="Function">
    /// </param>
    if (Object.keyExists(LxnBase.Data.ConfigManager._listConfigs, className)) {
        listConfigLoaded(LxnBase.Data.ConfigManager._listConfigs[className]);
    }
    else {
        LxnBase.Services.GenericService.GetRangeConfig(className, function(result) {
            LxnBase.Data.ConfigManager._listConfigs[className] = result;
            listConfigLoaded(result);
        }, null);
    }
}
LxnBase.Data.ConfigManager.getViewConfig = function LxnBase_Data_ConfigManager$getViewConfig(className, itemConfigLoaded) {
    /// <param name="className" type="String">
    /// </param>
    /// <param name="itemConfigLoaded" type="Function">
    /// </param>
    if (Object.keyExists(LxnBase.Data.ConfigManager._viewConfigs, className)) {
        itemConfigLoaded(LxnBase.Data.ConfigManager._viewConfigs[className]);
    }
    else {
        LxnBase.Services.GenericService.GetItemConfig(className, true, function(result) {
            LxnBase.Data.ConfigManager._viewConfigs[className] = result;
            itemConfigLoaded(result);
        }, null);
    }
}
LxnBase.Data.ConfigManager.getEditConfig = function LxnBase_Data_ConfigManager$getEditConfig(className, itemConfigLoaded) {
    /// <param name="className" type="String">
    /// </param>
    /// <param name="itemConfigLoaded" type="Function">
    /// </param>
    if (Object.keyExists(LxnBase.Data.ConfigManager._editConfigs, className)) {
        itemConfigLoaded(LxnBase.Data.ConfigManager._editConfigs[className]);
    }
    else {
        LxnBase.Services.GenericService.GetItemConfig(className, false, function(result) {
            LxnBase.Data.ConfigManager._editConfigs[className] = result;
            itemConfigLoaded(result);
        }, null);
    }
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Data.ObjectPropertyNames

LxnBase.Data.ObjectPropertyNames = function LxnBase_Data_ObjectPropertyNames() {
    /// <field name="idPropertyName" type="String" static="true">
    /// </field>
    /// <field name="versionPropertyName" type="String" static="true">
    /// </field>
    /// <field name="textPropertyName" type="String" static="true">
    /// </field>
    /// <field name="typePropertyName" type="String" static="true">
    /// </field>
    /// <field name="referencePropertyName" type="String" static="true">
    /// </field>
    /// <field name="objectClassPropertyName" type="String" static="true">
    /// </field>
}


Type.registerNamespace('LxnBase.Services');

////////////////////////////////////////////////////////////////////////////////
// LxnBase.Services.TypeEnum

LxnBase.Services.TypeEnum = function() { 
    /// <field name="object" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="number" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="list" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="bool" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="string" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="date" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="custom" type="Number" integer="true" static="true">
    /// </field>
};
LxnBase.Services.TypeEnum.prototype = {
    object: 0, 
    number: 1, 
    list: 2, 
    bool: 3, 
    string: 4, 
    date: 5, 
    custom: 6
}
LxnBase.Services.TypeEnum.registerEnum('LxnBase.Services.TypeEnum', false);


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Services.ColumnConfig

LxnBase.Services.ColumnConfig = function LxnBase_Services_ColumnConfig() {
    /// <field name="Name" type="String">
    /// </field>
    /// <field name="Caption" type="String">
    /// </field>
    /// <field name="Type" type="LxnBase.Services.TypeEnum">
    /// </field>
    /// <field name="IsRequired" type="Boolean">
    /// </field>
    /// <field name="IsReadOnly" type="Boolean">
    /// </field>
    /// <field name="IsPersistent" type="Boolean">
    /// </field>
    /// <field name="IsReference" type="Boolean">
    /// </field>
    /// <field name="ListWidth" type="Number">
    /// </field>
    /// <field name="Hidden" type="Boolean">
    /// </field>
    /// <field name="DefaultValue" type="Object">
    /// </field>
}
LxnBase.Services.ColumnConfig.prototype = {
    Name: null,
    Caption: null,
    Type: 0,
    IsRequired: false,
    IsReadOnly: false,
    IsPersistent: false,
    IsReference: false,
    ListWidth: 0,
    Hidden: false,
    DefaultValue: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Services.ClassColumnConfig

LxnBase.Services.ClassColumnConfig = function LxnBase_Services_ClassColumnConfig() {
    /// <field name="Clazz" type="String">
    /// </field>
    /// <field name="FilterType" type="LxnBase.Services.TypeEnum">
    /// </field>
    /// <field name="Length" type="Number" integer="true">
    /// </field>
    /// <field name="RenderAsString" type="Boolean">
    /// </field>
    LxnBase.Services.ClassColumnConfig.initializeBase(this);
}
LxnBase.Services.ClassColumnConfig.prototype = {
    Clazz: null,
    FilterType: 0,
    Length: 0,
    RenderAsString: false
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Services.ClassDefinition

LxnBase.Services.ClassDefinition = function LxnBase_Services_ClassDefinition() {
    /// <field name="ClassId" type="String">
    /// </field>
    /// <field name="Caption" type="String">
    /// </field>
    /// <field name="ListCaption" type="String">
    /// </field>
}
LxnBase.Services.ClassDefinition.prototype = {
    ClassId: null,
    Caption: null,
    ListCaption: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Services.CustomTypeColumnConfig

LxnBase.Services.CustomTypeColumnConfig = function LxnBase_Services_CustomTypeColumnConfig() {
    /// <field name="TypeName" type="String">
    /// </field>
    LxnBase.Services.CustomTypeColumnConfig.initializeBase(this);
}
LxnBase.Services.CustomTypeColumnConfig.prototype = {
    TypeName: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Services.DateTimeColumnConfig

LxnBase.Services.DateTimeColumnConfig = function LxnBase_Services_DateTimeColumnConfig() {
    /// <field name="FormatString" type="String">
    /// </field>
    LxnBase.Services.DateTimeColumnConfig.initializeBase(this);
}
LxnBase.Services.DateTimeColumnConfig.prototype = {
    FormatString: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Services.GenericService

LxnBase.Services.GenericService = function LxnBase_Services_GenericService() {
    /// <field name="service" type="LxnBase.Net.WebService" static="true">
    /// </field>
}
LxnBase.Services.GenericService.GetRange = function LxnBase_Services_GenericService$GetRange(className, pparams, onSuccess, onError) {
    /// <param name="className" type="String">
    /// </param>
    /// <param name="pparams" type="LxnBase.Data.RangeRequest">
    /// </param>
    /// <param name="onSuccess" type="Function">
    /// </param>
    /// <param name="onError" type="Function">
    /// </param>
    LxnBase.Services.GenericService.service.invoke('GetRange', { className: className, params: pparams }, false, null, onSuccess, onError);
}
LxnBase.Services.GenericService.getRangeProxy = function LxnBase_Services_GenericService$getRangeProxy(className) {
    /// <param name="className" type="String">
    /// </param>
    /// <returns type="LxnBase.Data.WebServiceProxy"></returns>
    var proxy = new LxnBase.Data.WebServiceProxy(LxnBase.Services.GenericService.service, 'GetRange');
    proxy.arguments = { className: className };
    return proxy;
}
LxnBase.Services.GenericService.Refresh = function LxnBase_Services_GenericService$Refresh(className, ids, visibleProperties, hiddenProperties, onSuccess, onError) {
    /// <param name="className" type="String">
    /// </param>
    /// <param name="ids" type="Array" elementType="Object">
    /// </param>
    /// <param name="visibleProperties" type="Array" elementType="String">
    /// </param>
    /// <param name="hiddenProperties" type="Array" elementType="String">
    /// </param>
    /// <param name="onSuccess" type="Function">
    /// </param>
    /// <param name="onError" type="Function">
    /// </param>
    LxnBase.Services.GenericService.service.invoke('Refresh', { className: className, ids: ids, visibleProperties: visibleProperties, hiddenProperties: hiddenProperties }, false, null, onSuccess, onError);
}
LxnBase.Services.GenericService.GetRangeConfig = function LxnBase_Services_GenericService$GetRangeConfig(className, onSuccess, onError) {
    /// <param name="className" type="String">
    /// </param>
    /// <param name="onSuccess" type="Function">
    /// </param>
    /// <param name="onError" type="Function">
    /// </param>
    LxnBase.Services.GenericService.service.invoke('GetRangeConfig', { className: className }, false, null, onSuccess, onError);
}
LxnBase.Services.GenericService.Get = function LxnBase_Services_GenericService$Get(className, id, viewMode, onSuccess, onError) {
    /// <param name="className" type="String">
    /// </param>
    /// <param name="id" type="Object">
    /// </param>
    /// <param name="viewMode" type="Boolean">
    /// </param>
    /// <param name="onSuccess" type="Function">
    /// </param>
    /// <param name="onError" type="Function">
    /// </param>
    LxnBase.Services.GenericService.service.invoke('Get', { className: className, id: id, viewMode: viewMode }, false, null, onSuccess, onError);
}
LxnBase.Services.GenericService.GetItemConfig = function LxnBase_Services_GenericService$GetItemConfig(className, viewMode, onSuccess, onError) {
    /// <param name="className" type="String">
    /// </param>
    /// <param name="viewMode" type="Boolean">
    /// </param>
    /// <param name="onSuccess" type="Function">
    /// </param>
    /// <param name="onError" type="Function">
    /// </param>
    LxnBase.Services.GenericService.service.invoke('GetItemConfig', { className: className, viewMode: viewMode }, false, null, onSuccess, onError);
}
LxnBase.Services.GenericService.Suggest = function LxnBase_Services_GenericService$Suggest(className, pparams, onSuccess, onError) {
    /// <param name="className" type="String">
    /// </param>
    /// <param name="pparams" type="LxnBase.Data.RangeRequest">
    /// </param>
    /// <param name="onSuccess" type="Function">
    /// </param>
    /// <param name="onError" type="Function">
    /// </param>
    LxnBase.Services.GenericService.service.invoke('Suggest', { className: className, params: pparams }, false, null, onSuccess, onError);
}
LxnBase.Services.GenericService.suggestProxy = function LxnBase_Services_GenericService$suggestProxy(className) {
    /// <param name="className" type="String">
    /// </param>
    /// <returns type="LxnBase.Data.WebServiceProxy"></returns>
    var proxy = new LxnBase.Data.WebServiceProxy(LxnBase.Services.GenericService.service, 'Suggest');
    proxy.arguments = { className: className };
    return proxy;
}
LxnBase.Services.GenericService.CanUpdate = function LxnBase_Services_GenericService$CanUpdate(className, id, onSuccess, onError) {
    /// <param name="className" type="String">
    /// </param>
    /// <param name="id" type="Object">
    /// </param>
    /// <param name="onSuccess" type="Function">
    /// </param>
    /// <param name="onError" type="Function">
    /// </param>
    LxnBase.Services.GenericService.service.invoke('CanUpdate', { className: className, id: id }, false, null, onSuccess, onError);
}
LxnBase.Services.GenericService.Update = function LxnBase_Services_GenericService$Update(className, id, version, data, pparams, onSuccess, onError) {
    /// <param name="className" type="String">
    /// </param>
    /// <param name="id" type="Object">
    /// </param>
    /// <param name="version" type="Object">
    /// </param>
    /// <param name="data" type="Object">
    /// </param>
    /// <param name="pparams" type="LxnBase.Data.RangeRequest">
    /// </param>
    /// <param name="onSuccess" type="Function">
    /// </param>
    /// <param name="onError" type="Function">
    /// </param>
    LxnBase.Services.GenericService.service.invoke('Update', { className: className, id: id, version: version, data: data, params: pparams }, false, null, onSuccess, onError);
}
LxnBase.Services.GenericService.CanDelete = function LxnBase_Services_GenericService$CanDelete(className, ids, onSuccess, onError) {
    /// <param name="className" type="String">
    /// </param>
    /// <param name="ids" type="Array" elementType="Object">
    /// </param>
    /// <param name="onSuccess" type="Function">
    /// </param>
    /// <param name="onError" type="Function">
    /// </param>
    LxnBase.Services.GenericService.service.invoke('CanDelete', { className: className, ids: ids }, false, null, onSuccess, onError);
}
LxnBase.Services.GenericService.Delete = function LxnBase_Services_GenericService$Delete(className, ids, pparams, onSuccess, onError) {
    /// <param name="className" type="String">
    /// </param>
    /// <param name="ids" type="Array" elementType="Object">
    /// </param>
    /// <param name="pparams" type="LxnBase.Data.RangeRequest">
    /// </param>
    /// <param name="onSuccess" type="Function">
    /// </param>
    /// <param name="onError" type="Function">
    /// </param>
    LxnBase.Services.GenericService.service.invoke('Delete', { className: className, ids: ids, params: pparams }, false, null, onSuccess, onError);
}
LxnBase.Services.GenericService.CanList = function LxnBase_Services_GenericService$CanList(className, onSuccess, onError) {
    /// <param name="className" type="String">
    /// </param>
    /// <param name="onSuccess" type="Function">
    /// </param>
    /// <param name="onError" type="Function">
    /// </param>
    LxnBase.Services.GenericService.service.invoke('CanList', { className: className }, false, null, onSuccess, onError);
}
LxnBase.Services.GenericService.GetDependencies = function LxnBase_Services_GenericService$GetDependencies(className, id, onSuccess, onError) {
    /// <param name="className" type="String">
    /// </param>
    /// <param name="id" type="Object">
    /// </param>
    /// <param name="onSuccess" type="Function">
    /// </param>
    /// <param name="onError" type="Function">
    /// </param>
    LxnBase.Services.GenericService.service.invoke('GetDependencies', { className: className, id: id }, false, null, onSuccess, onError);
}
LxnBase.Services.GenericService.CanReplace = function LxnBase_Services_GenericService$CanReplace(className, id, onSuccess, onError) {
    /// <param name="className" type="String">
    /// </param>
    /// <param name="id" type="Object">
    /// </param>
    /// <param name="onSuccess" type="Function">
    /// </param>
    /// <param name="onError" type="Function">
    /// </param>
    LxnBase.Services.GenericService.service.invoke('CanReplace', { className: className, id: id }, false, null, onSuccess, onError);
}
LxnBase.Services.GenericService.Replace = function LxnBase_Services_GenericService$Replace(className, oldId, newId, deleteOld, onSuccess, onError) {
    /// <param name="className" type="String">
    /// </param>
    /// <param name="oldId" type="Object">
    /// </param>
    /// <param name="newId" type="Object">
    /// </param>
    /// <param name="deleteOld" type="Boolean">
    /// </param>
    /// <param name="onSuccess" type="Function">
    /// </param>
    /// <param name="onError" type="Function">
    /// </param>
    LxnBase.Services.GenericService.service.invoke('Replace', { className: className, oldId: oldId, newId: newId, deleteOld: deleteOld }, false, null, onSuccess, onError);
}
LxnBase.Services.GenericService.Export = function LxnBase_Services_GenericService$Export(className, args, onSuccess, onError) {
    /// <param name="className" type="String">
    /// </param>
    /// <param name="args" type="LxnBase.Data.DocumentExportArgs">
    /// </param>
    /// <param name="onSuccess" type="Function">
    /// </param>
    /// <param name="onError" type="Function">
    /// </param>
    LxnBase.Services.GenericService.service.invoke('Export', { className: className, args: args }, false, null, onSuccess, onError);
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Services.ItemConfig

LxnBase.Services.ItemConfig = function LxnBase_Services_ItemConfig() {
    /// <field name="Columns" type="Array" elementType="ColumnConfig">
    /// </field>
    /// <field name="Caption" type="String">
    /// </field>
    /// <field name="ListCaption" type="String">
    /// </field>
    /// <field name="IsListAllowed" type="LxnBase.Data.OperationStatus">
    /// </field>
    /// <field name="IsCreationAllowed" type="LxnBase.Data.OperationStatus">
    /// </field>
    /// <field name="IsCopyingAllowed" type="LxnBase.Data.OperationStatus">
    /// </field>
    /// <field name="IsEditAllowed" type="LxnBase.Data.OperationStatus">
    /// </field>
    /// <field name="IsRemovingAllowed" type="LxnBase.Data.OperationStatus">
    /// </field>
    /// <field name="CustomActionPermissions" type="Object">
    /// </field>
}
LxnBase.Services.ItemConfig.prototype = {
    Columns: null,
    Caption: null,
    ListCaption: null,
    IsListAllowed: null,
    IsCreationAllowed: null,
    IsCopyingAllowed: null,
    IsEditAllowed: null,
    IsRemovingAllowed: null,
    CustomActionPermissions: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Services.ListColumnConfig

LxnBase.Services.ListColumnConfig = function LxnBase_Services_ListColumnConfig() {
    /// <field name="Items" type="Array" elementType="Object">
    /// </field>
    LxnBase.Services.ListColumnConfig.initializeBase(this);
}
LxnBase.Services.ListColumnConfig.prototype = {
    Items: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Services.ListConfig

LxnBase.Services.ListConfig = function LxnBase_Services_ListConfig() {
    /// <field name="Columns" type="Array" elementType="ColumnConfig">
    /// </field>
    /// <field name="Caption" type="String">
    /// </field>
    /// <field name="Filterable" type="Boolean">
    /// </field>
    /// <field name="IsCreationAllowed" type="LxnBase.Data.OperationStatus">
    /// </field>
    /// <field name="IsCopyingAllowed" type="LxnBase.Data.OperationStatus">
    /// </field>
    /// <field name="IsEditAllowed" type="LxnBase.Data.OperationStatus">
    /// </field>
    /// <field name="IsRemovingAllowed" type="LxnBase.Data.OperationStatus">
    /// </field>
    /// <field name="IsQuickEditAllowed" type="Boolean">
    /// </field>
    /// <field name="SingleSelect" type="Boolean">
    /// </field>
    /// <field name="CustomActionPermissions" type="Object">
    /// </field>
}
LxnBase.Services.ListConfig.prototype = {
    Columns: null,
    Caption: null,
    Filterable: false,
    IsCreationAllowed: null,
    IsCopyingAllowed: null,
    IsEditAllowed: null,
    IsRemovingAllowed: null,
    IsQuickEditAllowed: false,
    SingleSelect: false,
    CustomActionPermissions: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Services.NumberColumnConfig

LxnBase.Services.NumberColumnConfig = function LxnBase_Services_NumberColumnConfig() {
    /// <field name="IsInteger" type="Boolean">
    /// </field>
    LxnBase.Services.NumberColumnConfig.initializeBase(this);
}
LxnBase.Services.NumberColumnConfig.prototype = {
    IsInteger: false
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Services.TextColumnConfig

LxnBase.Services.TextColumnConfig = function LxnBase_Services_TextColumnConfig() {
    /// <field name="Length" type="Number" integer="true">
    /// </field>
    /// <field name="Lines" type="Number" integer="true">
    /// </field>
    LxnBase.Services.TextColumnConfig.initializeBase(this);
}
LxnBase.Services.TextColumnConfig.prototype = {
    Length: 0,
    Lines: 0
}


Type.registerNamespace('LxnBase.Knockout');

////////////////////////////////////////////////////////////////////////////////
// LxnBase.Knockout.EmailBindingHandler

LxnBase.Knockout.EmailBindingHandler = function LxnBase_Knockout_EmailBindingHandler() {
    /// <field name="_protocolCheck$1" type="RegExp" static="true">
    /// </field>
    LxnBase.Knockout.EmailBindingHandler.initializeBase(this);
}
LxnBase.Knockout.EmailBindingHandler.prototype = {
    
    update: function LxnBase_Knockout_EmailBindingHandler$update(element, valueAccessor, allBindingsAccessor, viewModel) {
        /// <param name="element" type="Object" domElement="true">
        /// </param>
        /// <param name="valueAccessor" type="System.Func`1">
        /// </param>
        /// <param name="allBindingsAccessor" type="System.Func`1">
        /// </param>
        /// <param name="viewModel" type="Object">
        /// </param>
        var text = ko.utils.unwrapObservable(valueAccessor());
        var href = (String.isNullOrEmpty(text)) ? null : ((LxnBase.Knockout.EmailBindingHandler._protocolCheck$1.test(text)) ? text : 'mailto:' + text);
        $(element).attr('href', href).text(text);
    }
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Knockout.FixedBindingHandler

LxnBase.Knockout.FixedBindingHandler = function LxnBase_Knockout_FixedBindingHandler() {
    LxnBase.Knockout.FixedBindingHandler.initializeBase(this);
}
LxnBase.Knockout.FixedBindingHandler.prototype = {
    
    update: function LxnBase_Knockout_FixedBindingHandler$update(element, valueAccessor, allBindingsAccessor, viewModel) {
        /// <param name="element" type="Object" domElement="true">
        /// </param>
        /// <param name="valueAccessor" type="System.Func`1">
        /// </param>
        /// <param name="allBindingsAccessor" type="System.Func`1">
        /// </param>
        /// <param name="viewModel" type="Object">
        /// </param>
        var value = ko.utils.unwrapObservable(valueAccessor());
        var obj = $(element).text((LxnBase.NumberUtility.isNumber(value)) ? value.format('n') : value.toString());
        if (value < 0) {
            obj.addClass('negative');
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Knockout.LinkBindingHandler

LxnBase.Knockout.LinkBindingHandler = function LxnBase_Knockout_LinkBindingHandler() {
    /// <field name="_protocolCheck$1" type="RegExp" static="true">
    /// </field>
    LxnBase.Knockout.LinkBindingHandler.initializeBase(this);
}
LxnBase.Knockout.LinkBindingHandler.prototype = {
    
    update: function LxnBase_Knockout_LinkBindingHandler$update(element, valueAccessor, allBindingsAccessor, viewModel) {
        /// <param name="element" type="Object" domElement="true">
        /// </param>
        /// <param name="valueAccessor" type="System.Func`1">
        /// </param>
        /// <param name="allBindingsAccessor" type="System.Func`1">
        /// </param>
        /// <param name="viewModel" type="Object">
        /// </param>
        var text = ko.utils.unwrapObservable(valueAccessor());
        var href = (String.isNullOrEmpty(text)) ? null : ((LxnBase.Knockout.LinkBindingHandler._protocolCheck$1.test(text)) ? text : 'http://' + text);
        $(element).attr('href', href).text(text);
    }
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Knockout.ResBindingHandler

LxnBase.Knockout.ResBindingHandler = function LxnBase_Knockout_ResBindingHandler() {
    /// <field name="_resources$1" type="Array" static="true">
    /// </field>
    LxnBase.Knockout.ResBindingHandler.initializeBase(this);
}
LxnBase.Knockout.ResBindingHandler.get_resources = function LxnBase_Knockout_ResBindingHandler$get_resources() {
    /// <value type="Array"></value>
    return LxnBase.Knockout.ResBindingHandler._resources$1;
}
LxnBase.Knockout.ResBindingHandler.prototype = {
    
    update: function LxnBase_Knockout_ResBindingHandler$update(element, valueAccessor, allBindingsAccessor, viewModel) {
        /// <param name="element" type="Object" domElement="true">
        /// </param>
        /// <param name="valueAccessor" type="System.Func`1">
        /// </param>
        /// <param name="allBindingsAccessor" type="System.Func`1">
        /// </param>
        /// <param name="viewModel" type="Object">
        /// </param>
        var name = valueAccessor();
        if (String.isNullOrEmpty(name)) {
            return;
        }
        var $enum1 = ss.IEnumerator.getEnumerator(LxnBase.Knockout.ResBindingHandler._resources$1);
        while ($enum1.moveNext()) {
            var resource = $enum1.current;
            var value = resource[name];
            if (!String.isNullOrEmpty(value)) {
                $(element).text(value);
                return;
            }
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Knockout.StringTemplate

LxnBase.Knockout.StringTemplate = function LxnBase_Knockout_StringTemplate(id, text) {
    /// <param name="id" type="String">
    /// </param>
    /// <param name="text" type="String">
    /// </param>
    /// <field name="_templates" type="Object" static="true">
    /// </field>
    /// <field name="_id" type="String">
    /// </field>
    /// <field name="_text" type="String">
    /// </field>
    /// <field name="_data" type="Object">
    /// </field>
    this._id = id;
    this._text = text;
    this._data = {};
    LxnBase.Knockout.StringTemplate._templates[id] = this;
}
LxnBase.Knockout.StringTemplate.get = function LxnBase_Knockout_StringTemplate$get(id) {
    /// <param name="id" type="String">
    /// </param>
    /// <returns type="LxnBase.Knockout.StringTemplate"></returns>
    return LxnBase.Knockout.StringTemplate._templates[id];
}
LxnBase.Knockout.StringTemplate.prototype = {
    
    renderTo: function LxnBase_Knockout_StringTemplate$renderTo(el, model) {
        /// <param name="el" type="Object" domElement="true">
        /// </param>
        /// <param name="model" type="Object">
        /// </param>
        /// <returns type="Object" domElement="true"></returns>
        el.setAttribute('data-bind', "template: '" + this._id + "'");
        ko.applyBindings(model, el);
        return el;
    },
    
    text: function LxnBase_Knockout_StringTemplate$text(value) {
        /// <param name="value" type="String">
        /// </param>
        /// <returns type="String"></returns>
        return (!arguments.length) ? this._text : this._text = value;
    },
    
    data: function LxnBase_Knockout_StringTemplate$data(key, value) {
        /// <param name="key" type="String">
        /// </param>
        /// <param name="value" type="Object">
        /// </param>
        /// <returns type="Object"></returns>
        return (arguments.length === 1) ? this._data[key] : this._data[key] = value;
    },
    
    _id: null,
    _text: null,
    _data: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Knockout.ViewBindingHandler

LxnBase.Knockout.ViewBindingHandler = function LxnBase_Knockout_ViewBindingHandler() {
    LxnBase.Knockout.ViewBindingHandler.initializeBase(this);
}
LxnBase.Knockout.ViewBindingHandler.prototype = {
    
    init: function LxnBase_Knockout_ViewBindingHandler$init(element, valueAccessor, allBindingsAccessor, viewModel) {
        /// <param name="element" type="Object" domElement="true">
        /// </param>
        /// <param name="valueAccessor" type="System.Func`1">
        /// </param>
        /// <param name="allBindingsAccessor" type="System.Func`1">
        /// </param>
        /// <param name="viewModel" type="Object">
        /// </param>
        $(element).attr('href', 'javascript:void(0)');
    },
    
    update: function LxnBase_Knockout_ViewBindingHandler$update(element, valueAccessor, allBindingsAccessor, viewModel) {
        /// <param name="element" type="Object" domElement="true">
        /// </param>
        /// <param name="valueAccessor" type="System.Func`1">
        /// </param>
        /// <param name="allBindingsAccessor" type="System.Func`1">
        /// </param>
        /// <param name="viewModel" type="Object">
        /// </param>
        var value = ko.mapping.toJS(valueAccessor());
        if (Type.canCast(value, Array)) {
            var arr = value;
            $(element).text(arr[LxnBase.Data.ObjectInfo.TextPos]).click(function() {
                LxnBase.UI.FormsRegistry.viewObject(arr[LxnBase.Data.ObjectInfo.TypePos], arr[LxnBase.Data.ObjectInfo.IdPos]);
            });
        }
        else {
            var obj = value;
            $(element).text(obj.Text).click(function() {
                LxnBase.UI.FormsRegistry.viewObject(obj.Type, obj.Id);
            });
        }
    }
}


Type.registerNamespace('LxnBase.Net');

////////////////////////////////////////////////////////////////////////////////
// LxnBase.Net.WebServiceError

LxnBase.Net.WebServiceError = function LxnBase_Net_WebServiceError() {
    /// <field name="Message" type="String">
    /// </field>
    /// <field name="ExceptionType" type="String">
    /// </field>
    /// <field name="ExceptionDetail" type="String">
    /// </field>
    /// <field name="StackTrace" type="String">
    /// </field>
    /// <field name="StatusText" type="String">
    /// </field>
    /// <field name="StatusCode" type="Number" integer="true">
    /// </field>
}
LxnBase.Net.WebServiceError.prototype = {
    Message: null,
    ExceptionType: null,
    ExceptionDetail: null,
    StackTrace: null,
    StatusText: null,
    StatusCode: 0
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Net.WebServiceFailureArgs

LxnBase.Net.WebServiceFailureArgs = function LxnBase_Net_WebServiceFailureArgs(error, method) {
    /// <param name="error" type="LxnBase.Net.WebServiceError">
    /// </param>
    /// <param name="method" type="String">
    /// </param>
    /// <field name="_error" type="LxnBase.Net.WebServiceError">
    /// </field>
    /// <field name="_method" type="String">
    /// </field>
    /// <field name="_handled" type="Boolean">
    /// </field>
    this._error = error;
    this._method = method;
}
LxnBase.Net.WebServiceFailureArgs.prototype = {
    
    get_error: function LxnBase_Net_WebServiceFailureArgs$get_error() {
        /// <value type="LxnBase.Net.WebServiceError"></value>
        return this._error;
    },
    
    get_method: function LxnBase_Net_WebServiceFailureArgs$get_method() {
        /// <value type="String"></value>
        return this._method;
    },
    
    get_handled: function LxnBase_Net_WebServiceFailureArgs$get_handled() {
        /// <value type="Boolean"></value>
        return this._handled;
    },
    set_handled: function LxnBase_Net_WebServiceFailureArgs$set_handled(value) {
        /// <value type="Boolean"></value>
        this._handled = value;
        return value;
    },
    
    toString: function LxnBase_Net_WebServiceFailureArgs$toString() {
        /// <returns type="String"></returns>
        return this._error.Message;
    },
    
    _error: null,
    _method: null,
    _handled: false
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.Net.WebService

LxnBase.Net.WebService = function LxnBase_Net_WebService(path) {
    /// <param name="path" type="String">
    /// </param>
    /// <field name="root" type="String" static="true">
    /// </field>
    /// <field name="__failure" type="Function" static="true">
    /// </field>
    /// <field name="_path" type="String">
    /// </field>
    /// <field name="_timeOut" type="Number" integer="true">
    /// </field>
    /// <field name="_wrapper" type="String">
    /// </field>
    this._path = path;
}
LxnBase.Net.WebService.add_failure = function LxnBase_Net_WebService$add_failure(value) {
    /// <param name="value" type="Function" />
    LxnBase.Net.WebService.__failure = ss.Delegate.combine(LxnBase.Net.WebService.__failure, value);
}
LxnBase.Net.WebService.remove_failure = function LxnBase_Net_WebService$remove_failure(value) {
    /// <param name="value" type="Function" />
    LxnBase.Net.WebService.__failure = ss.Delegate.remove(LxnBase.Net.WebService.__failure, value);
}
LxnBase.Net.WebService.prototype = {
    
    get_timeOut: function LxnBase_Net_WebService$get_timeOut() {
        /// <value type="Number" integer="true"></value>
        return this._timeOut;
    },
    set_timeOut: function LxnBase_Net_WebService$set_timeOut(value) {
        /// <value type="Number" integer="true"></value>
        this._timeOut = value;
        return value;
    },
    
    get_wrapper: function LxnBase_Net_WebService$get_wrapper() {
        /// <value type="String"></value>
        return this._wrapper;
    },
    set_wrapper: function LxnBase_Net_WebService$set_wrapper(value) {
        /// <value type="String"></value>
        this._wrapper = value;
        return value;
    },
    
    invoke: function LxnBase_Net_WebService$invoke(method, args, useGet, wrapper, onSuccess, onError) {
        /// <param name="method" type="String">
        /// </param>
        /// <param name="args" type="Object">
        /// </param>
        /// <param name="useGet" type="Boolean">
        /// </param>
        /// <param name="wrapper" type="String">
        /// </param>
        /// <param name="onSuccess" type="Function">
        /// </param>
        /// <param name="onError" type="Function">
        /// </param>
        var options = {};
        options.url = LxnBase.Net.WebService.root + this._path + '/' + method;
        options.data = JSON.stringify(args);
        options.dataType = 'text';
        options.timeout = this._timeOut;
        if (useGet) {
            options.type = 'GET';
        }
        else {
            options.type = 'POST';
            options.contentType = 'application/json; charset=utf-8';
        }
        $.ajax(options).success(ss.Delegate.create(this, function(data) {
            if (ss.isNullOrUndefined(onSuccess)) {
                return;
            }
            var result = JSON.parse(data);
            wrapper = (wrapper || this._wrapper);
            onSuccess((!wrapper) ? result : result[wrapper]);
        })).error(function(request, textStatus, e) {
            var error = new LxnBase.Net.WebServiceError();
            switch (textStatus) {
                case 'abort':
                    error.Message = String.format(LxnBase.BaseRes.webServiceAborted, method);
                    break;
                case 'timeout':
                    error.Message = String.format(LxnBase.BaseRes.webServiceTimedOut, method);
                    break;
                case 'parsererror':
                    error.Message = String.format(LxnBase.BaseRes.webServiceResponse_Error_Msg, method);
                    break;
                case 'error':
                    if (!String.isNullOrEmpty(request.responseText)) {
                        error = JSON.parse(request.responseText);
                    }
                    else {
                        error.Message = String.format(LxnBase.BaseRes.webServiceFailedNoMsg, method);
                    }
                    break;
            }
            error.StatusCode = request.status;
            error.StatusText = textStatus;
            var failureArgs = new LxnBase.Net.WebServiceFailureArgs(error, method);
            if (LxnBase.Net.WebService.__failure != null) {
                LxnBase.Net.WebService.__failure(failureArgs);
            }
            if (onError != null) {
                onError(failureArgs);
            }
            if (textStatus !== 'abort') {
                LxnBase.MessageRegister.error(failureArgs.toString(), failureArgs.get_error().ExceptionType, failureArgs.get_handled());
            }
        });
    },
    
    _path: null,
    _timeOut: 60000,
    _wrapper: 'd'
}


Type.registerNamespace('LxnBase.UI');

////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.IKeyHandler

LxnBase.UI.IKeyHandler = function() { 
};
LxnBase.UI.IKeyHandler.prototype = {
    handleKeyEvent : null,
    restoreFocus : null
}
LxnBase.UI.IKeyHandler.registerInterface('LxnBase.UI.IKeyHandler');


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.LoadMode

LxnBase.UI.LoadMode = function() { 
    /// <field name="local" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="remote" type="Number" integer="true" static="true">
    /// </field>
};
LxnBase.UI.LoadMode.prototype = {
    local: 0, 
    remote: 1
}
LxnBase.UI.LoadMode.registerEnum('LxnBase.UI.LoadMode', false);


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.Versioned

LxnBase.UI.$create_Versioned = function LxnBase_UI_Versioned() { return {}; }


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.ActionFactory

LxnBase.UI.ActionFactory = function LxnBase_UI_ActionFactory() {
}
LxnBase.UI.ActionFactory.createListAction = function LxnBase_UI_ActionFactory$createListAction(type, text) {
    /// <param name="type" type="String">
    /// </param>
    /// <param name="text" type="String">
    /// </param>
    /// <returns type="Ext.Action"></returns>
    return new Ext.Action(new Ext.ActionConfig().text(text).handler(function() {
        LxnBase.UI.FormsRegistry.listObjects(type);
    }).toDictionary());
}
LxnBase.UI.ActionFactory.createViewAction = function LxnBase_UI_ActionFactory$createViewAction(type, id, text) {
    /// <param name="type" type="String">
    /// </param>
    /// <param name="id" type="Object">
    /// </param>
    /// <param name="text" type="String">
    /// </param>
    /// <returns type="Ext.Action"></returns>
    return new Ext.Action(new Ext.ActionConfig().text(text).handler(function() {
        LxnBase.UI.FormsRegistry.viewObject(type, id);
    }).toDictionary());
}
LxnBase.UI.ActionFactory.createAction3 = function LxnBase_UI_ActionFactory$createAction3(viewPage, text) {
    /// <param name="viewPage" type="String">
    /// </param>
    /// <param name="text" type="String">
    /// </param>
    /// <returns type="Ext.Action"></returns>
    return new Ext.Action(new Ext.ActionConfig().text(text).handler(function() {
        eval("window.open('http://travel3/#/" + viewPage + "', 'LuxenaTravel3');");
    }).toDictionary());
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.BaseClassEditForm

LxnBase.UI.BaseClassEditForm = function LxnBase_UI_BaseClassEditForm(args, instanceConfig) {
    /// <param name="args" type="LxnBase.UI.EditFormArgs">
    /// </param>
    /// <param name="instanceConfig" type="LxnBase.Services.ItemConfig">
    /// </param>
    /// <field name="saveButton" type="Ext.Button">
    /// </field>
    /// <field name="cancelButton" type="Ext.Button">
    /// </field>
    /// <field name="_args$1" type="LxnBase.UI.EditFormArgs">
    /// </field>
    /// <field name="_instanceConfig$1" type="LxnBase.Services.ItemConfig">
    /// </field>
    /// <field name="_instance$1" type="Object">
    /// </field>
    /// <field name="_localMode$1" type="Boolean">
    /// </field>
    LxnBase.UI.BaseClassEditForm.initializeBase(this);
    this.init(args, instanceConfig);
}
LxnBase.UI.BaseClassEditForm.prototype = {
    
    get_args: function LxnBase_UI_BaseClassEditForm$get_args() {
        /// <value type="LxnBase.UI.EditFormArgs"></value>
        return this._args$1;
    },
    
    get_instanceConfig: function LxnBase_UI_BaseClassEditForm$get_instanceConfig() {
        /// <value type="LxnBase.Services.ItemConfig"></value>
        return this._instanceConfig$1;
    },
    set_instanceConfig: function LxnBase_UI_BaseClassEditForm$set_instanceConfig(value) {
        /// <value type="LxnBase.Services.ItemConfig"></value>
        this._instanceConfig$1 = value;
        return value;
    },
    
    get_instance: function LxnBase_UI_BaseClassEditForm$get_instance() {
        /// <value type="Object"></value>
        return this._instance$1;
    },
    set_instance: function LxnBase_UI_BaseClassEditForm$set_instance(value) {
        /// <value type="Object"></value>
        this._instance$1 = value;
        return value;
    },
    
    get_localMode: function LxnBase_UI_BaseClassEditForm$get_localMode() {
        /// <value type="Boolean"></value>
        return this._localMode$1;
    },
    
    init: function LxnBase_UI_BaseClassEditForm$init(args, itemConfig) {
        /// <param name="args" type="LxnBase.UI.EditFormArgs">
        /// </param>
        /// <param name="itemConfig" type="LxnBase.Services.ItemConfig">
        /// </param>
        this._args$1 = args;
        this._instanceConfig$1 = itemConfig;
        this._localMode$1 = ss.isValue(this._args$1.mode) && !this._args$1.mode;
        if (this._args$1.onSave != null) {
            this.add_saved(this._args$1.onSave);
        }
        if (this._args$1.onCancel != null) {
            this.add_canceled(this._args$1.onCancel);
        }
    },
    
    open: function LxnBase_UI_BaseClassEditForm$open() {
        this.onWindowOpen();
    },
    
    onWindowOpen: function LxnBase_UI_BaseClassEditForm$onWindowOpen() {
        if (this._localMode$1) {
            this.onInstanceLoaded(this._args$1.fieldValues);
        }
        else if (ss.isValue(this._args$1.idToLoad)) {
            this.loadInstance(ss.Delegate.create(this, this.onInstanceLoaded));
        }
        else {
            this.onInstanceLoaded(null);
        }
    },
    
    onInstanceLoaded: function LxnBase_UI_BaseClassEditForm$onInstanceLoaded(result) {
        /// <param name="result" type="Object">
        /// </param>
        if (!this.get_args().isCopy) {
            this._instance$1 = result;
        }
        else {
            this.get_args().fieldValues = result;
        }
        var title = String.format('{0} ({1})', this.getCaption(), (this.get_args().get_isNew()) ? LxnBase.BaseRes.creation : LxnBase.BaseRes.editing);
        this.get_window().setTitle(title);
        this.loadForm();
        this.setFieldValues();
        this.onLoaded();
        this.get_window().show();
    },
    
    getCaption: function LxnBase_UI_BaseClassEditForm$getCaption() {
        /// <returns type="String"></returns>
        return this.get_instanceConfig().Caption;
    },
    
    loadForm: function LxnBase_UI_BaseClassEditForm$loadForm() {
        this.set_fields(this.addFields());
        var buttons = this.addButtons();
        this.initComponentSequence(this.get_fields(), buttons);
        this.onLoadForm();
    },
    
    addButtons: function LxnBase_UI_BaseClassEditForm$addButtons() {
        /// <returns type="Array" elementType="Button"></returns>
        this.saveButton = this.get_form().addButton(LxnBase.BaseRes.save, ss.Delegate.create(this, this.save));
        this.cancelButton = this.get_form().addButton(LxnBase.BaseRes.cancel, ss.Delegate.create(this, this.cancel));
        return [ this.saveButton, this.cancelButton ];
    },
    
    initComponentSequence: function LxnBase_UI_BaseClassEditForm$initComponentSequence(fields, buttons) {
        /// <param name="fields" type="Array" elementType="Field">
        /// </param>
        /// <param name="buttons" type="Array" elementType="Button">
        /// </param>
        var list = [];
        list.addRange(fields);
        list.addRange(buttons);
        this.set_componentSequence(list);
    },
    
    onLoadForm: function LxnBase_UI_BaseClassEditForm$onLoadForm() {
    },
    
    onLoaded: function LxnBase_UI_BaseClassEditForm$onLoaded() {
    },
    
    getInstancePropertyValue: function LxnBase_UI_BaseClassEditForm$getInstancePropertyValue(fieldName) {
        /// <param name="fieldName" type="String">
        /// </param>
        /// <returns type="Object"></returns>
        return (this._instance$1 != null) ? this._instance$1[fieldName] : this.getArgsValue(fieldName);
    },
    
    getArgsValue: function LxnBase_UI_BaseClassEditForm$getArgsValue(fieldName) {
        /// <param name="fieldName" type="String">
        /// </param>
        /// <returns type="Object"></returns>
        if (this._args$1.fieldValues != null && !ss.isNullOrUndefined(this._args$1.fieldValues[fieldName])) {
            return this._args$1.fieldValues[fieldName];
        }
        return null;
    },
    
    getFieldConfig: function LxnBase_UI_BaseClassEditForm$getFieldConfig(fieldName) {
        /// <param name="fieldName" type="String">
        /// </param>
        /// <returns type="LxnBase.Services.ColumnConfig"></returns>
        for (var i = 0; i < this._instanceConfig$1.Columns.length; i++) {
            if (this._instanceConfig$1.Columns[i].Name === fieldName) {
                return this._instanceConfig$1.Columns[i];
            }
        }
        return null;
    },
    
    createEditor: function LxnBase_UI_BaseClassEditForm$createEditor(fieldName) {
        /// <param name="fieldName" type="String">
        /// </param>
        /// <returns type="Ext.form.Field"></returns>
        return LxnBase.UI.AutoControls.ControlFactory.createEditor(this.getFieldConfig(fieldName));
    },
    
    getCustomActionStatus: function LxnBase_UI_BaseClassEditForm$getCustomActionStatus(actionName) {
        /// <param name="actionName" type="String">
        /// </param>
        /// <returns type="LxnBase.Data.OperationStatus"></returns>
        var permissions = this.get_instanceConfig().CustomActionPermissions;
        if (permissions != null && Object.keyExists(permissions, actionName)) {
            return permissions[actionName];
        }
        var status = new LxnBase.Data.OperationStatus();
        status.Visible = false;
        return status;
    },
    
    startSave: function LxnBase_UI_BaseClassEditForm$startSave() {
        this.saveButton.disable();
    },
    
    failSave: function LxnBase_UI_BaseClassEditForm$failSave(result) {
        /// <param name="result" type="Object">
        /// </param>
        try {
            LxnBase.UI.BaseClassEditForm.callBaseMethod(this, 'failSave', [ result ]);
        }
        finally {
            this.saveButton.enable();
        }
    },
    
    saveButton: null,
    cancelButton: null,
    _args$1: null,
    _instanceConfig$1: null,
    _instance$1: null,
    _localMode$1: false
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.BaseClassViewForm

LxnBase.UI.BaseClassViewForm = function LxnBase_UI_BaseClassViewForm(tabId, id, type) {
    /// <param name="tabId" type="String">
    /// </param>
    /// <param name="id" type="Object">
    /// </param>
    /// <param name="type" type="String">
    /// </param>
    /// <field name="_itemConfig$6" type="LxnBase.Services.ItemConfig">
    /// </field>
    /// <field name="_type$6" type="String">
    /// </field>
    /// <field name="_id$6" type="Object">
    /// </field>
    /// <field name="_instance$6" type="Object">
    /// </field>
    /// <field name="_permissions$6" type="LxnBase.Data.OperationPermissions">
    /// </field>
    /// <field name="_editButton$6" type="Ext.Action">
    /// </field>
    /// <field name="_deleteButton$6" type="Ext.Action">
    /// </field>
    /// <field name="_copyButton$6" type="Ext.Action">
    /// </field>
    LxnBase.UI.BaseClassViewForm.initializeBase(this, [ new Ext.PanelConfig().closable(true).autoScroll(true).layout('fit').title(LxnBase.BaseRes.loading).cls('autoView').bodyCssClass('view-body').toDictionary(), tabId ]);
    this._type$6 = type;
    this._id$6 = id;
    LxnBase.Data.ConfigManager.getViewConfig(type, ss.Delegate.create(this, function(config) {
        this._itemConfig$6 = config;
        LxnBase.UI.BaseClassViewForm.setActionStatus(this._copyButton$6, this._itemConfig$6.IsCopyingAllowed);
        LxnBase.UI.BaseClassViewForm.setActionStatus(this._editButton$6, this._itemConfig$6.IsEditAllowed);
        LxnBase.UI.BaseClassViewForm.setActionStatus(this._deleteButton$6, this._itemConfig$6.IsRemovingAllowed);
        this.onLoadConfig();
        this.getInstance();
    }));
}
LxnBase.UI.BaseClassViewForm.setActionStatus = function LxnBase_UI_BaseClassViewForm$setActionStatus(action, status) {
    /// <param name="action" type="Ext.Action">
    /// </param>
    /// <param name="status" type="LxnBase.Data.OperationStatus">
    /// </param>
    if (ss.isNullOrUndefined(status)) {
        return;
    }
    action.setHidden(!status.Visible);
    action.setDisabled(status.IsDisabled);
    action.tooltip = status.DisableInfo;
}
LxnBase.UI.BaseClassViewForm.prototype = {
    
    initComponent: function LxnBase_UI_BaseClassViewForm$initComponent() {
        this._editButton$6 = new Ext.Action(new Ext.ActionConfig().text(LxnBase.BaseRes.edit_Lower).handler(ss.Delegate.create(this, this.edit)).hidden(true).toDictionary());
        this._copyButton$6 = new Ext.Action(new Ext.ActionConfig().text(LxnBase.BaseRes.copy_Lower).handler(ss.Delegate.create(this, this.copy)).hidden(true).toDictionary());
        this._deleteButton$6 = new Ext.Action(new Ext.ActionConfig().text(LxnBase.BaseRes.remove_Lower).handler(ss.Delegate.create(this, this.deleteInstance)).hidden(true).toDictionary());
        var list = [];
        list.addRange([ this._editButton$6, this._copyButton$6, this._deleteButton$6 ]);
        this.onInitToolBar(list);
        this.tbar = new Ext.Toolbar(list);
        LxnBase.UI.BaseClassViewForm.callBaseMethod(this, 'initComponent');
    },
    
    onInitToolBar: function LxnBase_UI_BaseClassViewForm$onInitToolBar(toolbarItems) {
        /// <param name="toolbarItems" type="Array">
        /// </param>
    },
    
    get_id: function LxnBase_UI_BaseClassViewForm$get_id() {
        /// <value type="Object"></value>
        return this._id$6;
    },
    
    get_instanceType: function LxnBase_UI_BaseClassViewForm$get_instanceType() {
        /// <value type="String"></value>
        return this._type$6;
    },
    
    get_instance: function LxnBase_UI_BaseClassViewForm$get_instance() {
        /// <value type="Object"></value>
        return this._instance$6;
    },
    
    get_permissions: function LxnBase_UI_BaseClassViewForm$get_permissions() {
        /// <value type="LxnBase.Data.OperationPermissions"></value>
        return this._permissions$6;
    },
    
    get_itemConfig: function LxnBase_UI_BaseClassViewForm$get_itemConfig() {
        /// <value type="LxnBase.Services.ItemConfig"></value>
        return this._itemConfig$6;
    },
    
    onLoadConfig: function LxnBase_UI_BaseClassViewForm$onLoadConfig() {
    },
    
    load: function LxnBase_UI_BaseClassViewForm$load(result) {
        /// <param name="result" type="Object">
        /// </param>
        this._instance$6 = result;
        this._updatePermissions$6();
        this.onLoad();
    },
    
    refresh: function LxnBase_UI_BaseClassViewForm$refresh(result) {
        /// <param name="result" type="Object">
        /// </param>
        if (result == null) {
            return;
        }
        this._instance$6 = result;
        this._updatePermissions$6();
        this.onRefresh();
    },
    
    updateActionsStatus: function LxnBase_UI_BaseClassViewForm$updateActionsStatus() {
        LxnBase.UI.BaseClassViewForm.setActionStatus(this._copyButton$6, this._itemConfig$6.IsCopyingAllowed);
        LxnBase.UI.BaseClassViewForm.setActionStatus(this._editButton$6, this._permissions$6.CanUpdate || this._itemConfig$6.IsEditAllowed);
        LxnBase.UI.BaseClassViewForm.setActionStatus(this._deleteButton$6, this._permissions$6.CanDelete || this._itemConfig$6.IsRemovingAllowed);
        this.setToolbarVisibility();
    },
    
    onRefresh: function LxnBase_UI_BaseClassViewForm$onRefresh() {
        this.body.clean();
        this.onLoad();
    },
    
    edit: function LxnBase_UI_BaseClassViewForm$edit() {
        this.onEdit();
    },
    
    onEdit: function LxnBase_UI_BaseClassViewForm$onEdit() {
        LxnBase.UI.FormsRegistry.editObject(this._type$6, this._id$6, null, ss.Delegate.create(this, function(result) {
            var response = result;
            this.load((ss.isNullOrUndefined(response.Item)) ? result : response.Item);
        }), null);
    },
    
    copy: function LxnBase_UI_BaseClassViewForm$copy() {
        this.onCopy();
    },
    
    onCopy: function LxnBase_UI_BaseClassViewForm$onCopy() {
        var dictionary = {};
        var $enum1 = ss.IEnumerator.getEnumerator(this._itemConfig$6.Columns);
        while ($enum1.moveNext()) {
            var t = $enum1.current;
            dictionary[t.Name] = this._instance$6[t.Name];
        }
        LxnBase.UI.FormsRegistry.editObject(this._type$6, null, dictionary, ss.Delegate.create(this, function(result) {
            var objId = (result).Item.Id;
            LxnBase.UI.FormsRegistry.viewObject(this._type$6, objId);
        }), null);
    },
    
    deleteInstance: function LxnBase_UI_BaseClassViewForm$deleteInstance() {
        this.onDeleteInstance();
    },
    
    onDeleteInstance: function LxnBase_UI_BaseClassViewForm$onDeleteInstance() {
        LxnBase.UI.MessageBoxWrap.confirm(LxnBase.BaseRes.confirmation, LxnBase.BaseRes.delete_Confirmation, ss.Delegate.create(this, function(button, text) {
            if (button !== 'yes') {
                return;
            }
            LxnBase.Services.GenericService.Delete(this._type$6, [ this._id$6 ], null, ss.Delegate.create(this, function(result) {
                var response = result;
                if (response.Success) {
                    this.close();
                    LxnBase.MessageRegister.info(this._itemConfig$6.ListCaption, LxnBase.BaseRes.deleted + ' ' + this.title);
                }
                else {
                    this._onDeleteFailed$6();
                }
            }), null);
        }));
    },
    
    onActivate: function LxnBase_UI_BaseClassViewForm$onActivate(isFirst) {
        /// <param name="isFirst" type="Boolean">
        /// </param>
        if (!isFirst) {
            this.getInstance();
        }
    },
    
    setToolbarVisibility: function LxnBase_UI_BaseClassViewForm$setToolbarVisibility() {
        var hideToolbar = true;
        var list = this.getTopToolbar().items;
        var count = list.getCount();
        var isSeparator = false;
        var isFirst = true;
        for (var i = 0; i < count; i++) {
            var item = list.itemAt(i);
            if (Type.canCast(item, Ext.Toolbar.Separator)) {
                if (!i || i === count - 1 || isSeparator || isFirst) {
                    item.setVisible(false);
                }
                isSeparator = true;
            }
            else {
                isSeparator = false;
            }
            if (!item.hidden) {
                isFirst = false;
            }
        }
        for (var i = 0; i < count; i++) {
            if (!(list.itemAt(i)).hidden) {
                hideToolbar = false;
            }
        }
        if (hideToolbar) {
            this.getTopToolbar().setVisible(false);
        }
    },
    
    getCustomActionStatus: function LxnBase_UI_BaseClassViewForm$getCustomActionStatus(actionName) {
        /// <param name="actionName" type="String">
        /// </param>
        /// <returns type="LxnBase.Data.OperationStatus"></returns>
        var permissions = this.get_itemConfig().CustomActionPermissions;
        if (permissions != null && Object.keyExists(permissions, actionName)) {
            return permissions[actionName];
        }
        var status = new LxnBase.Data.OperationStatus();
        status.Visible = false;
        return status;
    },
    
    _updatePermissions$6: function LxnBase_UI_BaseClassViewForm$_updatePermissions$6() {
        var permissions = this._instance$6.Permissions;
        if (!ss.isNullOrUndefined(permissions)) {
            this._permissions$6 = permissions;
        }
        if (ss.isNullOrUndefined(this._permissions$6)) {
            this._permissions$6 = new LxnBase.Data.OperationPermissions();
        }
        this.updateActionsStatus();
    },
    
    _onDeleteFailed$6: function LxnBase_UI_BaseClassViewForm$_onDeleteFailed$6() {
        LxnBase.Services.GenericService.CanReplace(this._type$6, this._id$6, ss.Delegate.create(this, function(result) {
            if (result) {
                var form = new LxnBase.UI.ReplaceForm(this._type$6, this._id$6);
                form.add_saved(ss.Delegate.create(this, function() {
                    this.close();
                }));
                form.open();
            }
            else {
                LxnBase.UI.MessageBoxWrap.show(LxnBase.BaseRes.warning, LxnBase.BaseRes.autoGrid_DeleteConstrainedFailed_Msg + '<br/>' + LxnBase.BaseRes.autoGrid_ReplaceToAdmin_Msg, Ext.MessageBox.WARNING, Ext.MessageBox.OK);
            }
        }), null);
    },
    
    _itemConfig$6: null,
    _type$6: null,
    _id$6: null,
    _instance$6: null,
    _permissions$6: null,
    _editButton$6: null,
    _deleteButton$6: null,
    _copyButton$6: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.BaseEditForm

LxnBase.UI.BaseEditForm = function LxnBase_UI_BaseEditForm() {
    /// <field name="__saved" type="Function">
    /// </field>
    /// <field name="__canceled" type="Function">
    /// </field>
    /// <field name="__close" type="Function">
    /// </field>
    /// <field name="_window" type="Ext.Window">
    /// </field>
    /// <field name="_form" type="LxnBase.UI.Form">
    /// </field>
    /// <field name="_errorPanel" type="Ext.Panel">
    /// </field>
    /// <field name="_statusBar" type="Ext.ux.StatusBar">
    /// </field>
    /// <field name="_fields" type="Array" elementType="Field">
    /// </field>
    /// <field name="_componentSequence" type="Array" elementType="Component">
    /// </field>
    /// <field name="_errors" type="Ext.util.MixedCollection">
    /// </field>
    /// <field name="_isSaved" type="Boolean">
    /// </field>
    /// <field name="_isCanceled" type="Boolean">
    /// </field>
    /// <field name="_isSubscribeOnStatusClick" type="Boolean">
    /// </field>
    /// <field name="_navigationManager" type="LxnBase.UI.FieldNavigationManager">
    /// </field>
    /// <field name="_actionManager" type="LxnBase.UI.FieldActionManager">
    /// </field>
    this._errors = new Ext.util.MixedCollection();
    this._form = new LxnBase.UI.Form(LxnBase.UI.BaseEditForm._createFormConfig().toDictionary());
    this._window = new Ext.Window(this._createWindowConfig().toDictionary());
    this._actionManager = new LxnBase.UI.FieldActionManager();
}
LxnBase.UI.BaseEditForm._createFormConfig = function LxnBase_UI_BaseEditForm$_createFormConfig() {
    /// <returns type="Ext.form.FormPanelConfig"></returns>
    return new Ext.form.FormPanelConfig().autoScroll(true).bodyBorder(false).border(false).labelWidth(100).labelAlign('right').buttonAlign('center');
}
LxnBase.UI.BaseEditForm.isFieldMdified = function LxnBase_UI_BaseEditForm$isFieldMdified(field) {
    /// <param name="field" type="Ext.form.Field">
    /// </param>
    /// <returns type="Boolean"></returns>
    return field != null && field.isDirty();
}
LxnBase.UI.BaseEditForm._onErrorPanelClick = function LxnBase_UI_BaseEditForm$_onErrorPanelClick(e) {
    /// <param name="e" type="Object">
    /// </param>
    var arg = e;
    var element = arg.getTarget('li');
    if (!ss.isNullOrUndefined(element)) {
        Ext.ComponentMgr.get(element.id.split('errorFieldId-')[1]).focus();
    }
    arg.stopEvent();
}
LxnBase.UI.BaseEditForm.prototype = {
    
    add_saved: function LxnBase_UI_BaseEditForm$add_saved(value) {
        /// <param name="value" type="Function" />
        this.__saved = ss.Delegate.combine(this.__saved, value);
    },
    remove_saved: function LxnBase_UI_BaseEditForm$remove_saved(value) {
        /// <param name="value" type="Function" />
        this.__saved = ss.Delegate.remove(this.__saved, value);
    },
    
    __saved: null,
    
    add_canceled: function LxnBase_UI_BaseEditForm$add_canceled(value) {
        /// <param name="value" type="Function" />
        this.__canceled = ss.Delegate.combine(this.__canceled, value);
    },
    remove_canceled: function LxnBase_UI_BaseEditForm$remove_canceled(value) {
        /// <param name="value" type="Function" />
        this.__canceled = ss.Delegate.remove(this.__canceled, value);
    },
    
    __canceled: null,
    
    add_close: function LxnBase_UI_BaseEditForm$add_close(value) {
        /// <param name="value" type="Function" />
        this.__close = ss.Delegate.combine(this.__close, value);
    },
    remove_close: function LxnBase_UI_BaseEditForm$remove_close(value) {
        /// <param name="value" type="Function" />
        this.__close = ss.Delegate.remove(this.__close, value);
    },
    
    __close: null,
    
    get_window: function LxnBase_UI_BaseEditForm$get_window() {
        /// <value type="Ext.Window"></value>
        return this._window;
    },
    
    get_form: function LxnBase_UI_BaseEditForm$get_form() {
        /// <value type="LxnBase.UI.Form"></value>
        return this._form;
    },
    
    get_fields: function LxnBase_UI_BaseEditForm$get_fields() {
        /// <value type="Array" elementType="Field"></value>
        return this._fields;
    },
    set_fields: function LxnBase_UI_BaseEditForm$set_fields(value) {
        /// <value type="Array" elementType="Field"></value>
        this._fields = value;
        return value;
    },
    
    get_componentSequence: function LxnBase_UI_BaseEditForm$get_componentSequence() {
        /// <value type="Array" elementType="Component"></value>
        return this._componentSequence;
    },
    set_componentSequence: function LxnBase_UI_BaseEditForm$set_componentSequence(value) {
        /// <value type="Array" elementType="Component"></value>
        this._componentSequence = value;
        return value;
    },
    
    get_isCanceled: function LxnBase_UI_BaseEditForm$get_isCanceled() {
        /// <value type="Boolean"></value>
        return this._isCanceled;
    },
    
    get_isSaved: function LxnBase_UI_BaseEditForm$get_isSaved() {
        /// <value type="Boolean"></value>
        return this._isSaved;
    },
    
    _createWindowConfig: function LxnBase_UI_BaseEditForm$_createWindowConfig() {
        /// <returns type="Ext.WindowConfig"></returns>
        this._errorPanel = new Ext.Panel(new Ext.PanelConfig().cls('errors').hidden(true).listeners({ render: ss.Delegate.create(this, function() {
            this._errorPanel.getEl().on('click', LxnBase.UI.BaseEditForm._onErrorPanelClick);
        }) }).toDictionary());
        this._statusBar = new Ext.ux.StatusBar(new Ext.ux.StatusBarConfig().ctCls('status-bar').toDictionary());
        return new Ext.WindowConfig().baseCls('x-panel').cls('window-edit').resizable(false).listeners({ afterrender: ss.Delegate.create(this, this._onWindowAfterRender), show: ss.Delegate.create(this, this._onWindowShow), close: ss.Delegate.create(this, this._onWindowClose) }).modal(true).keys([ { key: 13, fn: ss.Delegate.create(this, this._onKeyEnterPress), scope: this } ]).items([ this._form, this._errorPanel ]).bbar(this._statusBar);
    },
    
    onValidate: function LxnBase_UI_BaseEditForm$onValidate() {
        /// <returns type="Boolean"></returns>
        return true;
    },
    
    onSaved: function LxnBase_UI_BaseEditForm$onSaved(result) {
        /// <param name="result" type="Object">
        /// </param>
        this._isSaved = true;
        this._window.close();
    },
    
    save: function LxnBase_UI_BaseEditForm$save() {
        if (this.validate()) {
            this.onSave();
        }
    },
    
    completeSave: function LxnBase_UI_BaseEditForm$completeSave(result) {
        /// <param name="result" type="Object">
        /// </param>
        if (this.__saved != null) {
            this.__saved(result);
        }
        this.onSaved(result);
    },
    
    failSave: function LxnBase_UI_BaseEditForm$failSave(result) {
        /// <param name="result" type="Object">
        /// </param>
    },
    
    cancel: function LxnBase_UI_BaseEditForm$cancel() {
        this._isCanceled = true;
        this._window.close();
    },
    
    registerField: function LxnBase_UI_BaseEditForm$registerField(field) {
        /// <param name="field" type="Ext.form.Field">
        /// </param>
        if (this._fields == null) {
            this._fields = new Array(0);
        }
        this._fields[this._fields.length] = field;
    },
    
    registerFocusComponent: function LxnBase_UI_BaseEditForm$registerFocusComponent(component) {
        /// <param name="component" type="Ext.Component">
        /// </param>
        if (this._componentSequence == null) {
            this._componentSequence = new Array(0);
        }
        this._componentSequence[this._componentSequence.length] = component;
    },
    
    validate: function LxnBase_UI_BaseEditForm$validate() {
        /// <returns type="Boolean"></returns>
        if (this._form.getForm().isValid() && this.onValidate()) {
            return true;
        }
        this._showErrors();
        return false;
    },
    
    isModified: function LxnBase_UI_BaseEditForm$isModified() {
        /// <returns type="Boolean"></returns>
        if (this._fields != null) {
            for (var i = 0; i < this._fields.length; i++) {
                if (LxnBase.UI.BaseEditForm.isFieldMdified(this._fields[i])) {
                    return true;
                }
            }
        }
        return false;
    },
    
    applyActions: function LxnBase_UI_BaseEditForm$applyActions(field, getActions) {
        /// <param name="field" type="Ext.form.Field">
        /// </param>
        /// <param name="getActions" type="Function">
        /// </param>
        this._actionManager.add(field, getActions);
    },
    
    refreshActions: function LxnBase_UI_BaseEditForm$refreshActions() {
        this._actionManager.refresh();
    },
    
    handleKeyEvent: function LxnBase_UI_BaseEditForm$handleKeyEvent(keyEvent) {
        /// <param name="keyEvent" type="jQueryEvent">
        /// </param>
        /// <returns type="Boolean"></returns>
        return this._navigationManager.handleKeyEvent(keyEvent) || this._actionManager.handleKeyEvent(keyEvent);
    },
    
    restoreFocus: function LxnBase_UI_BaseEditForm$restoreFocus() {
        this._navigationManager.restoreFocus();
    },
    
    _onWindowAfterRender: function LxnBase_UI_BaseEditForm$_onWindowAfterRender() {
        this._navigationManager = LxnBase.UI.FieldNavigationManager.create(this._componentSequence);
        this._actionManager.init(this._navigationManager, this.get_form());
        if (this._componentSequence != null) {
            for (var i = 0; i < this._componentSequence.length; i++) {
                var component = this._componentSequence[i];
                if (!component.hidden) {
                    this._window.defaultButton = component;
                    return;
                }
            }
        }
    },
    
    _onWindowShow: function LxnBase_UI_BaseEditForm$_onWindowShow() {
        if (this._fields != null) {
            for (var i = 0; i < this._fields.length; i++) {
                var field = this._fields[i];
                field.on('invalid', ss.Delegate.create(this, this._onFieldValidation));
                field.on('valid', ss.Delegate.create(this, this._onFieldValidation));
            }
        }
        LxnBase.UI.EventsManager.registerKeyDownHandler(this, !this._window.modal);
        this._statusBar.clearStatus();
        this.refreshWindowShadow();
    },
    
    _onWindowClose: function LxnBase_UI_BaseEditForm$_onWindowClose() {
        if (!this._isSaved && this.__canceled != null) {
            this.__canceled();
        }
        LxnBase.UI.EventsManager.unregisterKeyDownHandler(this);
        if (this.__close != null) {
            this.__close();
        }
    },
    
    _onFieldValidation: function LxnBase_UI_BaseEditForm$_onFieldValidation(field, msg) {
        /// <param name="field" type="Ext.form.Field">
        /// </param>
        /// <param name="msg" type="String">
        /// </param>
        if (!this._isSubscribeOnStatusClick) {
            (this._statusBar.statusEl).getEl().on('click', ss.Delegate.create(this, function() {
                if (!this._errorPanel.hidden) {
                    this._hideErrors();
                }
                else if (this._errors.getCount() > 0) {
                    this._showErrors();
                }
            }));
            this._isSubscribeOnStatusClick = true;
        }
        if (!ss.isNullOrUndefined(msg)) {
            this._errors.add(field.id, { field: field, message: msg });
        }
        else {
            this._errors.removeKey(field.id);
        }
        this._updateErrorList();
    },
    
    _showErrors: function LxnBase_UI_BaseEditForm$_showErrors() {
        if (!this._errorPanel.hidden) {
            return;
        }
        this._errorPanel.setWidth(this._form.getBox().width);
        this._errorPanel.show();
        this._statusBar.setText(LxnBase.BaseRes.valiadationStatusHideError_Msg);
        this.refreshWindowShadow();
    },
    
    _hideErrors: function LxnBase_UI_BaseEditForm$_hideErrors() {
        if (!this._errorPanel.hidden) {
            this._errorPanel.hide();
            this._statusBar.setText(LxnBase.BaseRes.valiadationStatusSowError_Msg);
            this.refreshWindowShadow();
        }
    },
    
    _updateErrorList: function LxnBase_UI_BaseEditForm$_updateErrorList() {
        if (this._errors.getCount() > 0) {
            var html = '<ul>';
            this._errors.each(function(obj) {
                var field = obj.field;
                var message = obj.message;
                html += String.format("<li id='errorFieldId-{2}'><a href='javascript:void(0)'>{0}: {1}</a></li>", field.fieldLabel, message, field.id);
            });
            new Ext.Template(html).overwrite(this._errorPanel.body);
            if (this._errorPanel.hidden && this._statusBar.getText() !== LxnBase.BaseRes.valiadationStatusSowError_Msg) {
                this._statusBar.setStatus(new Ext.ux.StatusBarConfig().text(LxnBase.BaseRes.valiadationStatusSowError_Msg).iconCls('error-icon').toDictionary());
            }
            else if (!this._errorPanel.hidden && this._statusBar.getText() !== LxnBase.BaseRes.valiadationStatusHideError_Msg) {
                this._statusBar.setStatus(new Ext.ux.StatusBarConfig().text(LxnBase.BaseRes.valiadationStatusHideError_Msg).iconCls('error-icon').toDictionary());
            }
        }
        else {
            new Ext.Template('<div></div>').overwrite(this._errorPanel.body);
            this._errorPanel.hide();
            this._statusBar.clearStatus();
            this.refreshWindowShadow();
        }
        if (!this._errorPanel.hidden) {
            this.refreshWindowShadow();
        }
    },
    
    _onKeyEnterPress: function LxnBase_UI_BaseEditForm$_onKeyEnterPress(key, e) {
        /// <param name="key" type="Object">
        /// </param>
        /// <param name="e" type="Ext.EventObject">
        /// </param>
        if (!e.shiftKey && e.ctrlKey && !e.altKey) {
            e.stopEvent();
            this.save();
        }
    },
    
    refreshWindowShadow: function LxnBase_UI_BaseEditForm$refreshWindowShadow() {
        if (this._form.ownerCt != null && Type.canCast(this._form.ownerCt, Ext.Window)) {
            this._form.ownerCt.getEl().enableShadow(true);
        }
    },
    
    _window: null,
    _form: null,
    _errorPanel: null,
    _statusBar: null,
    _fields: null,
    _componentSequence: null,
    _isSaved: false,
    _isCanceled: false,
    _isSubscribeOnStatusClick: false,
    _navigationManager: null,
    _actionManager: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.EditFormArgs

LxnBase.UI.EditFormArgs = function LxnBase_UI_EditFormArgs(id, type, fieldValues, pparams, onSave, onCancel, mode, isCopy) {
    /// <param name="id" type="Object">
    /// </param>
    /// <param name="type" type="String">
    /// </param>
    /// <param name="fieldValues" type="Object">
    /// </param>
    /// <param name="pparams" type="LxnBase.Data.RangeRequest">
    /// </param>
    /// <param name="onSave" type="Function">
    /// </param>
    /// <param name="onCancel" type="Function">
    /// </param>
    /// <param name="mode" type="LxnBase.UI.LoadMode">
    /// </param>
    /// <param name="isCopy" type="Boolean">
    /// </param>
    /// <field name="idToLoad" type="Object">
    /// </field>
    /// <field name="instance" type="Object">
    /// </field>
    /// <field name="type" type="String">
    /// </field>
    /// <field name="fieldValues" type="Object">
    /// </field>
    /// <field name="rangeRequest" type="LxnBase.Data.RangeRequest">
    /// </field>
    /// <field name="onSave" type="Function">
    /// </field>
    /// <field name="onCancel" type="Function">
    /// </field>
    /// <field name="mode" type="LxnBase.UI.LoadMode">
    /// </field>
    /// <field name="isCopy" type="Boolean">
    /// </field>
    this.idToLoad = id;
    this.type = type;
    this.fieldValues = fieldValues;
    this.rangeRequest = pparams;
    this.onSave = onSave;
    this.onCancel = onCancel;
    this.mode = mode;
    this.isCopy = isCopy;
}
LxnBase.UI.EditFormArgs.prototype = {
    idToLoad: null,
    
    get_id: function LxnBase_UI_EditFormArgs$get_id() {
        /// <value type="Object"></value>
        return (this.isCopy) ? null : this.idToLoad;
    },
    
    get_isNew: function LxnBase_UI_EditFormArgs$get_isNew() {
        /// <value type="Boolean"></value>
        return this.isCopy || !ss.isValue(this.idToLoad);
    },
    
    instance: null,
    type: null,
    fieldValues: null,
    rangeRequest: null,
    onSave: null,
    onCancel: null,
    mode: 0,
    isCopy: false
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.EventsManager

LxnBase.UI.EventsManager = function LxnBase_UI_EventsManager() {
    /// <field name="_handlerList" type="Array" static="true">
    /// </field>
}
LxnBase.UI.EventsManager.registerKeyDownHandler = function LxnBase_UI_EventsManager$registerKeyDownHandler(handler, allowBubbling) {
    /// <param name="handler" type="Object">
    /// </param>
    /// <param name="allowBubbling" type="Boolean">
    /// </param>
    if (LxnBase.UI.EventsManager._handlerList.length > 0) {
        var eventHandler = (LxnBase.UI.EventsManager._handlerList[LxnBase.UI.EventsManager._handlerList.length - 1]);
        eventHandler.set_focusedElement(document.activeElement);
    }
    LxnBase.UI.EventsManager._handlerList.add(new LxnBase.UI.KeyEventHandler(handler, allowBubbling));
}
LxnBase.UI.EventsManager.unregisterKeyDownHandler = function LxnBase_UI_EventsManager$unregisterKeyDownHandler(handler) {
    /// <param name="handler" type="Object">
    /// </param>
    if (ss.isNullOrUndefined(handler)) {
        LxnBase.UI.EventsManager._handlerList.removeAt(LxnBase.UI.EventsManager._handlerList.length - 1);
    }
    else {
        for (var i = LxnBase.UI.EventsManager._handlerList.length - 1; i >= 0; i--) {
            if ((LxnBase.UI.EventsManager._handlerList[i]).get_handler() === handler) {
                LxnBase.UI.EventsManager._handlerList.removeAt(i);
                break;
            }
        }
    }
    if (LxnBase.UI.EventsManager._handlerList.length > 0) {
        var element = (LxnBase.UI.EventsManager._handlerList[LxnBase.UI.EventsManager._handlerList.length - 1]).get_focusedElement();
        if (element != null) {
            element.focus();
        }
    }
}
LxnBase.UI.EventsManager._onKeyDown = function LxnBase_UI_EventsManager$_onKeyDown(e) {
    /// <param name="e" type="jQueryEvent">
    /// </param>
    for (var i = LxnBase.UI.EventsManager._handlerList.length - 1; i >= 0; i--) {
        var eventHandler = LxnBase.UI.EventsManager._handlerList[i];
        var handler = eventHandler.get_handler();
        if (handler.handleKeyEvent && handler.handleKeyEvent(e) || !eventHandler.get_allowBubbling()) {
            break;
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.FieldActionManager

LxnBase.UI.FieldActionManager = function LxnBase_UI_FieldActionManager() {
    /// <field name="_navigationManager" type="LxnBase.UI.FieldNavigationManager">
    /// </field>
    /// <field name="_fieldActions" type="Array">
    /// </field>
    /// <field name="_menu" type="Ext.menu.Menu">
    /// </field>
    /// <field name="_actionButtonId" type="String">
    /// </field>
    /// <field name="_isInitialized" type="Boolean">
    /// </field>
    this._fieldActions = [];
    this._menu = new Ext.menu.Menu(new Ext.menu.MenuConfig().cls('simple-menu testMenu').listeners({ beforeshow: ss.Delegate.create(this, function() {
        this._menu.tryActivate(0);
    }) }).toDictionary());
}
LxnBase.UI.FieldActionManager.prototype = {
    
    get__actionButton: function LxnBase_UI_FieldActionManager$get__actionButton() {
        /// <value type="jQueryObject"></value>
        return $(String.format('#{0}', this._actionButtonId));
    },
    
    init: function LxnBase_UI_FieldActionManager$init(navigationManager, form) {
        /// <param name="navigationManager" type="LxnBase.UI.FieldNavigationManager">
        /// </param>
        /// <param name="form" type="LxnBase.UI.Form">
        /// </param>
        this._navigationManager = navigationManager;
        navigationManager.add_focusChanged(ss.Delegate.create(this, function() {
            this.refresh();
        }));
        form.on('beforedestroy', ss.Delegate.create(this, function() {
            this._menu.destroy();
        }));
        this._actionButtonId = String.format('{0}-field-action-button', form.id);
        this._isInitialized = true;
    },
    
    add: function LxnBase_UI_FieldActionManager$add(field, getActions) {
        /// <param name="field" type="Ext.form.Field">
        /// </param>
        /// <param name="getActions" type="Function">
        /// </param>
        field.on('changeValue', ss.Delegate.create(this, this.refresh));
        this._fieldActions.add(new LxnBase.UI.FieldActions(field, getActions));
    },
    
    refresh: function LxnBase_UI_FieldActionManager$refresh() {
        if (!this._isInitialized || !this._fieldActions.length) {
            return;
        }
        this._hideActionButton();
        var $enum1 = ss.IEnumerator.getEnumerator(this._fieldActions);
        while ($enum1.moveNext()) {
            var fieldAction = $enum1.current;
            var fieldActions = fieldAction;
            if (fieldActions.get_field() === this._navigationManager.get_current()) {
                fieldActions.get_getActions()(fieldActions.get_field(), ss.Delegate.create(this, function(actions) {
                    this._displayActions(fieldActions.get_field(), actions);
                }));
                break;
            }
        }
    },
    
    handleKeyEvent: function LxnBase_UI_FieldActionManager$handleKeyEvent(keyEvent) {
        /// <param name="keyEvent" type="jQueryEvent">
        /// </param>
        /// <returns type="Boolean"></returns>
        if (keyEvent.which !== Ext.EventObject.SPACE || !keyEvent.ctrlKey || keyEvent.altKey || keyEvent.shiftKey) {
            return false;
        }
        if (!this.get__actionButton().length) {
            return false;
        }
        this._onActionButtonClick();
        return true;
    },
    
    _displayActions: function LxnBase_UI_FieldActionManager$_displayActions(field, actions) {
        /// <param name="field" type="Ext.form.Field">
        /// </param>
        /// <param name="actions" type="Array" elementType="Action">
        /// </param>
        if (actions != null && actions.length > 0) {
            this._setActions(actions);
            this._createActionButton(field);
        }
    },
    
    _hideActionButton: function LxnBase_UI_FieldActionManager$_hideActionButton() {
        this.get__actionButton().remove();
    },
    
    _createActionButton: function LxnBase_UI_FieldActionManager$_createActionButton(field) {
        /// <param name="field" type="Ext.form.Field">
        /// </param>
        this.get__actionButton().remove();
        var fieldElement = $('#' + field.id);
        var button = $('<div></div>').attr('id', this._actionButtonId).addClass('field-action');
        fieldElement.parent().append(button);
        var delta = fieldElement.outerHeight() / 2 - button.outerHeight() / 2;
        var top = fieldElement.position().top + delta;
        var left = fieldElement.position().left + fieldElement.outerWidth() - button.outerWidth() - delta;
        button.css('top', top + 'px');
        button.css('left', left + 'px');
        button.click(ss.Delegate.create(this, function() {
            this._onActionButtonClick();
        }));
    },
    
    _onActionButtonClick: function LxnBase_UI_FieldActionManager$_onActionButtonClick() {
        if (!this.get__actionButton().length) {
            return;
        }
        this._menu.show(this.get__actionButton()[0], 'tr-br?');
    },
    
    _setActions: function LxnBase_UI_FieldActionManager$_setActions(actions) {
        /// <param name="actions" type="Array" elementType="Action">
        /// </param>
        this._menu.removeAll();
        for (var i = 0; i < actions.length; i++) {
            var action = actions[i];
            this._menu.addMenuItem(new Ext.menu.ItemConfig().text(action.getText(null)).handler(ss.Delegate.create(action, action.execute)).toDictionary());
        }
    },
    
    _navigationManager: null,
    _menu: null,
    _actionButtonId: null,
    _isInitialized: false
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.FieldActions

LxnBase.UI.FieldActions = function LxnBase_UI_FieldActions(field, getActions) {
    /// <param name="field" type="Ext.form.Field">
    /// </param>
    /// <param name="getActions" type="Function">
    /// </param>
    /// <field name="_field" type="Ext.form.Field">
    /// </field>
    /// <field name="_getActions" type="Function">
    /// </field>
    this._field = field;
    this._getActions = getActions;
}
LxnBase.UI.FieldActions.prototype = {
    
    get_field: function LxnBase_UI_FieldActions$get_field() {
        /// <value type="Ext.form.Field"></value>
        return this._field;
    },
    
    get_getActions: function LxnBase_UI_FieldActions$get_getActions() {
        /// <value type="Function"></value>
        return this._getActions;
    },
    
    _field: null,
    _getActions: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.FieldNavigationManager

LxnBase.UI.FieldNavigationManager = function LxnBase_UI_FieldNavigationManager(components) {
    /// <param name="components" type="Array" elementType="Component">
    /// </param>
    /// <field name="__focusChanged" type="Function">
    /// </field>
    /// <field name="_controlSequence" type="Array">
    /// </field>
    /// <field name="_currentFocused" type="Ext.Component">
    /// </field>
    this._controlSequence = [];
    this._controlSequence.addRange(components);
    for (var i = 0; i < this._controlSequence.length; i++) {
        this._registerFocus(this._controlSequence[i]);
    }
}
LxnBase.UI.FieldNavigationManager.create = function LxnBase_UI_FieldNavigationManager$create(components) {
    /// <param name="components" type="Array" elementType="Component">
    /// </param>
    /// <returns type="LxnBase.UI.FieldNavigationManager"></returns>
    return new LxnBase.UI.FieldNavigationManager(components);
}
LxnBase.UI.FieldNavigationManager._focus = function LxnBase_UI_FieldNavigationManager$_focus(component) {
    /// <param name="component" type="Ext.Component">
    /// </param>
    if (component == null) {
        return;
    }
    component.focus();
}
LxnBase.UI.FieldNavigationManager.prototype = {
    
    add_focusChanged: function LxnBase_UI_FieldNavigationManager$add_focusChanged(value) {
        /// <param name="value" type="Function" />
        this.__focusChanged = ss.Delegate.combine(this.__focusChanged, value);
    },
    remove_focusChanged: function LxnBase_UI_FieldNavigationManager$remove_focusChanged(value) {
        /// <param name="value" type="Function" />
        this.__focusChanged = ss.Delegate.remove(this.__focusChanged, value);
    },
    
    __focusChanged: null,
    
    get_current: function LxnBase_UI_FieldNavigationManager$get_current() {
        /// <value type="Ext.Component"></value>
        return this._currentFocused;
    },
    
    handleKeyEvent: function LxnBase_UI_FieldNavigationManager$handleKeyEvent(keyEvent) {
        /// <param name="keyEvent" type="jQueryEvent">
        /// </param>
        /// <returns type="Boolean"></returns>
        if (keyEvent.which !== Ext.EventObject.ENTER || keyEvent.ctrlKey || keyEvent.altKey) {
            return false;
        }
        if (keyEvent.target.tagName.toLowerCase() === 'button' && !keyEvent.shiftKey) {
            return true;
        }
        var component = this._currentFocused;
        if (Type.canCast(component, Ext.form.TextArea) && !keyEvent.shiftKey) {
            return true;
        }
        if (Type.canCast(component, Ext.Button)) {
            if (keyEvent.shiftKey) {
                keyEvent.stopPropagation();
                keyEvent.preventDefault();
                LxnBase.UI.FieldNavigationManager._focus(this._previous(component));
            }
            else {
                var element = component.getEl().child((component).buttonSelector);
                if (document.activeElement !== element.dom) {
                    LxnBase.UI.FieldNavigationManager._focus(this._previous(component));
                }
            }
        }
        else {
            this._changeFocus(keyEvent, component);
        }
        return true;
    },
    
    restoreFocus: function LxnBase_UI_FieldNavigationManager$restoreFocus() {
        LxnBase.UI.FieldNavigationManager._focus(this._currentFocused || this._controlSequence[0]);
    },
    
    _registerFocus: function LxnBase_UI_FieldNavigationManager$_registerFocus(component) {
        /// <param name="component" type="Ext.Component">
        /// </param>
        var handler = ss.Delegate.create(this, function() {
            this._currentFocused = component;
            this._onFocusChanged();
        });
        if (Type.canCast(component, Ext.form.Field)) {
            component.on('focus', handler);
        }
        else if (Type.canCast(component, Ext.Button)) {
            var btn = component.getEl().child((component).buttonSelector);
            btn.on('focus', handler);
        }
        else {
            component.getEl().on('focus', handler);
        }
    },
    
    _changeFocus: function LxnBase_UI_FieldNavigationManager$_changeFocus(keyEvent, current) {
        /// <param name="keyEvent" type="jQueryEvent">
        /// </param>
        /// <param name="current" type="Ext.Component">
        /// </param>
        var noAdditionalKey = !keyEvent.shiftKey && !keyEvent.ctrlKey && !keyEvent.altKey;
        var shiftKey = keyEvent.shiftKey && !keyEvent.ctrlKey && !keyEvent.altKey;
        if (noAdditionalKey || shiftKey) {
            keyEvent.preventDefault();
            keyEvent.stopPropagation();
            if (Type.canCast(current, Ext.form.TriggerField)) {
                current.triggerBlur();
            }
            if (shiftKey) {
                LxnBase.UI.FieldNavigationManager._focus(this._previous(current));
            }
            else {
                LxnBase.UI.FieldNavigationManager._focus(this._next(current));
            }
        }
    },
    
    _next: function LxnBase_UI_FieldNavigationManager$_next(component) {
        /// <param name="component" type="Ext.Component">
        /// </param>
        /// <returns type="Ext.Component"></returns>
        var index = this._controlSequence.indexOf(component);
        for (var i = index + 1; i < this._controlSequence.length; i++) {
            var next = this._controlSequence[i];
            if (!next.hidden && !next.disabled && !(Type.canCast(next, Ext.form.DisplayField))) {
                return next;
            }
        }
        return null;
    },
    
    _previous: function LxnBase_UI_FieldNavigationManager$_previous(component) {
        /// <param name="component" type="Ext.Component">
        /// </param>
        /// <returns type="Ext.Component"></returns>
        var index = this._controlSequence.indexOf(component);
        for (var i = index - 1; i >= 0; i--) {
            var previous = this._controlSequence[i];
            if (!previous.hidden && !previous.disabled && !(Type.canCast(previous, Ext.form.DisplayField))) {
                return previous;
            }
        }
        return null;
    },
    
    _onFocusChanged: function LxnBase_UI_FieldNavigationManager$_onFocusChanged() {
        if (this.__focusChanged != null) {
            this.__focusChanged(this._currentFocused);
        }
    },
    
    _currentFocused: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.Form

LxnBase.UI.Form = function LxnBase_UI_Form(config) {
    /// <param name="config" type="Object">
    /// </param>
    LxnBase.UI.Form.initializeBase(this, [ config ]);
}
LxnBase.UI.Form.prototype = {
    
    hideItem: function LxnBase_UI_Form$hideItem(field) {
        /// <param name="field" type="Ext.form.Field">
        /// </param>
        if (field.rendered) {
            var formItem = field.getEl().findParent('.x-form-item');
            var element = new Ext.Element(formItem);
            element.addClass('x-hide-display');
        }
        else {
            field.itemCls = 'x-hide-display';
        }
        field.setVisible(false);
    },
    
    showItem: function LxnBase_UI_Form$showItem(field) {
        /// <param name="field" type="Ext.form.Field">
        /// </param>
        if (field.rendered) {
            var formItem = field.getEl().findParent('.x-form-item');
            var element = new Ext.Element(formItem);
            element.removeClass('x-hide-display');
        }
        field.setVisible(true);
    }
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.FormsRegistry

LxnBase.UI.FormsRegistry = function LxnBase_UI_FormsRegistry() {
    /// <field name="_noFormExceptionFormat" type="String" static="true">
    /// </field>
    /// <field name="_registerExceptionFormat" type="String" static="true">
    /// </field>
    /// <field name="_lists" type="Object" static="true">
    /// </field>
    /// <field name="_defaultList" type="Function" static="true">
    /// </field>
    /// <field name="_views" type="Object" static="true">
    /// </field>
    /// <field name="_defaultView" type="Function" static="true">
    /// </field>
    /// <field name="_edits" type="Object" static="true">
    /// </field>
    /// <field name="_defaultEdit" type="Function" static="true">
    /// </field>
    /// <field name="_selects" type="Object" static="true">
    /// </field>
    /// <field name="_defaultSelect" type="Function" static="true">
    /// </field>
}
LxnBase.UI.FormsRegistry.registerList = function LxnBase_UI_FormsRegistry$registerList(type, callback) {
    /// <param name="type" type="String">
    /// </param>
    /// <param name="callback" type="Function">
    /// </param>
    if (Object.keyExists(LxnBase.UI.FormsRegistry._lists, type)) {
        throw new Error(String.format("{0} form is alreay registered for type '{1}'", 'List', type));
    }
    LxnBase.UI.FormsRegistry._lists[type] = callback;
}
LxnBase.UI.FormsRegistry.registerDefaultList = function LxnBase_UI_FormsRegistry$registerDefaultList(callback) {
    /// <param name="callback" type="Function">
    /// </param>
    LxnBase.UI.FormsRegistry._defaultList = callback;
}
LxnBase.UI.FormsRegistry.registerView = function LxnBase_UI_FormsRegistry$registerView(type, callback, hasForm) {
    /// <param name="type" type="String">
    /// </param>
    /// <param name="callback" type="Function">
    /// </param>
    /// <param name="hasForm" type="Boolean">
    /// </param>
    if (Object.keyExists(LxnBase.UI.FormsRegistry._views, type)) {
        throw new Error(String.format("{0} form is alreay registered for type '{1}'", 'View', type));
    }
    var viewAction = new LxnBase.UI.ViewAction();
    viewAction.callback = callback;
    if (!ss.isUndefined(hasForm)) {
        viewAction.hasForm = hasForm;
    }
    LxnBase.UI.FormsRegistry._views[type] = viewAction;
}
LxnBase.UI.FormsRegistry.registerDefaultView = function LxnBase_UI_FormsRegistry$registerDefaultView(callback) {
    /// <param name="callback" type="Function">
    /// </param>
    LxnBase.UI.FormsRegistry._defaultView = callback;
}
LxnBase.UI.FormsRegistry.registerEdit = function LxnBase_UI_FormsRegistry$registerEdit(type, callback) {
    /// <param name="type" type="String">
    /// </param>
    /// <param name="callback" type="Function">
    /// </param>
    if (Object.keyExists(LxnBase.UI.FormsRegistry._edits, type)) {
        throw new Error(String.format("{0} form is alreay registered for type '{1}'", 'Edit', type));
    }
    LxnBase.UI.FormsRegistry._edits[type] = callback;
}
LxnBase.UI.FormsRegistry.registerDefaultEdit = function LxnBase_UI_FormsRegistry$registerDefaultEdit(callback) {
    /// <param name="callback" type="Function">
    /// </param>
    LxnBase.UI.FormsRegistry._defaultEdit = callback;
}
LxnBase.UI.FormsRegistry.registerSelect = function LxnBase_UI_FormsRegistry$registerSelect(type, callback) {
    /// <param name="type" type="String">
    /// </param>
    /// <param name="callback" type="Function">
    /// </param>
    if (Object.keyExists(LxnBase.UI.FormsRegistry._selects, type)) {
        throw new Error(String.format("{0} form is alreay registered for type '{1}'", 'Select', type));
    }
    LxnBase.UI.FormsRegistry._selects[type] = callback;
}
LxnBase.UI.FormsRegistry.registerDefaultSelect = function LxnBase_UI_FormsRegistry$registerDefaultSelect(callback) {
    /// <param name="callback" type="Function">
    /// </param>
    LxnBase.UI.FormsRegistry._defaultSelect = callback;
}
LxnBase.UI.FormsRegistry.listObjects = function LxnBase_UI_FormsRegistry$listObjects(type, baseRequest, newTab) {
    /// <param name="type" type="String">
    /// </param>
    /// <param name="baseRequest" type="LxnBase.Data.RangeRequest">
    /// </param>
    /// <param name="newTab" type="Boolean">
    /// </param>
    var list = LxnBase.UI.FormsRegistry._lists[type] || LxnBase.UI.FormsRegistry._defaultList;
    if (ss.isNullOrUndefined(list)) {
        throw new Error(String.format("There is no registered {0} form for type '{1}'", 'list', type));
    }
    list(new LxnBase.UI.ListArgs(type, baseRequest), newTab);
}
LxnBase.UI.FormsRegistry.viewObject = function LxnBase_UI_FormsRegistry$viewObject(type, id, newTab) {
    /// <param name="type" type="String">
    /// </param>
    /// <param name="id" type="Object">
    /// </param>
    /// <param name="newTab" type="Boolean">
    /// </param>
    var view = (ss.isNullOrUndefined(LxnBase.UI.FormsRegistry._views[type])) ? LxnBase.UI.FormsRegistry._defaultView : (LxnBase.UI.FormsRegistry._views[type]).callback;
    if (ss.isNullOrUndefined(view)) {
        throw new Error(String.format("There is no registered {0} form for type '{1}'", 'view', type));
    }
    view(type, id, newTab);
}
LxnBase.UI.FormsRegistry.hasViewForm = function LxnBase_UI_FormsRegistry$hasViewForm(type) {
    /// <param name="type" type="String">
    /// </param>
    /// <returns type="Boolean"></returns>
    return ss.isNullOrUndefined(LxnBase.UI.FormsRegistry._views[type]) || (LxnBase.UI.FormsRegistry._views[type]).hasForm;
}
LxnBase.UI.FormsRegistry.editObject = function LxnBase_UI_FormsRegistry$editObject(type, id, values, onSave, onCancel, pparams, mode, isCopy) {
    /// <param name="type" type="String">
    /// </param>
    /// <param name="id" type="Object">
    /// </param>
    /// <param name="values" type="Object">
    /// </param>
    /// <param name="onSave" type="Function">
    /// </param>
    /// <param name="onCancel" type="Function">
    /// </param>
    /// <param name="pparams" type="LxnBase.Data.RangeRequest">
    /// </param>
    /// <param name="mode" type="LxnBase.UI.LoadMode">
    /// </param>
    /// <param name="isCopy" type="Boolean">
    /// </param>
    var edit = LxnBase.UI.FormsRegistry._edits[type] || LxnBase.UI.FormsRegistry._defaultEdit;
    if (ss.isNullOrUndefined(edit)) {
        throw new Error(String.format("There is no registered {0} form for type '{1}'", 'edit', type));
    }
    edit(new LxnBase.UI.EditFormArgs(id, type, values, (ss.isValue(pparams)) ? pparams : null, onSave, onCancel, (ss.isValue(mode)) ? mode : 1, isCopy));
}
LxnBase.UI.FormsRegistry.selectObjects = function LxnBase_UI_FormsRegistry$selectObjects(type, baseRequest, singleSelect, onSelect, onCancel) {
    /// <param name="type" type="String">
    /// </param>
    /// <param name="baseRequest" type="LxnBase.Data.RangeRequest">
    /// </param>
    /// <param name="singleSelect" type="Boolean">
    /// </param>
    /// <param name="onSelect" type="Function">
    /// </param>
    /// <param name="onCancel" type="Function">
    /// </param>
    var select = LxnBase.UI.FormsRegistry._selects[type] || LxnBase.UI.FormsRegistry._defaultSelect;
    if (ss.isNullOrUndefined(select)) {
        throw new Error(String.format("There is no registered {0} form for type '{1}'", 'select', type));
    }
    select(new LxnBase.UI.SelectArgs(type, baseRequest, singleSelect, onSelect, onCancel));
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.ViewAction

LxnBase.UI.ViewAction = function LxnBase_UI_ViewAction() {
    /// <field name="callback" type="Function">
    /// </field>
    /// <field name="hasForm" type="Boolean">
    /// </field>
    this.hasForm = true;
}
LxnBase.UI.ViewAction.prototype = {
    callback: null,
    hasForm: false
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.Infos

LxnBase.UI.Infos = function LxnBase_UI_Infos() {
    /// <field name="_container" type="Ext.Element" static="true">
    /// </field>
    /// <field name="_timeout" type="Number" integer="true" static="true">
    /// </field>
}
LxnBase.UI.Infos.init = function LxnBase_UI_Infos$init() {
    LxnBase.UI.Infos._container = Ext.DomHelper.insertFirst(document.body, { id: 'msg-div' }, true);
    LxnBase.MessageRegister.add_newMessage(LxnBase.UI.Infos._onNewMessage);
}
LxnBase.UI.Infos._onNewMessage = function LxnBase_UI_Infos$_onNewMessage(sender, e) {
    /// <param name="sender" type="Object">
    /// </param>
    /// <param name="e" type="LxnBase.MessageRegisterEventArgs">
    /// </param>
    if (!!e.get_type()) {
        return;
    }
    var caption;
    var message = '';
    var details = '';
    var separator = '';
    if (ss.isNullOrUndefined(e.get_messageCaption())) {
        caption = e.get_message();
        message = e.get_details();
    }
    else {
        caption = e.get_messageCaption();
        if (!ss.isNullOrUndefined(e.get_message())) {
            message = e.get_message();
            separator = '<br/>';
        }
        if (!ss.isNullOrUndefined(e.get_details())) {
            details = "<div class='details'>" + e.get_details() + '</div';
            message += separator;
        }
    }
    message += details;
    var html = ([ "<div class='msg'><div class='x-box-tl'><div class='x-box-tr'><div class='x-box-tc'></div></div></div><div class='x-box-ml'><div class='x-box-mr'><div class='x-box-mc'><h3>", caption, '</h3>', message, "</div></div></div><div class='x-box-bl'><div class='x-box-br'><div class='x-box-bc'></div></div></div></div>" ]).join('');
    var msg = Ext.DomHelper.append(LxnBase.UI.Infos._container, { html: html }, true);
    msg.slideIn('t', null).pause(3).ghost('t', { remove: true });
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.KeyEventHandler

LxnBase.UI.KeyEventHandler = function LxnBase_UI_KeyEventHandler(handler, allowBubbling) {
    /// <param name="handler" type="LxnBase.UI.IKeyHandler">
    /// </param>
    /// <param name="allowBubbling" type="Boolean">
    /// </param>
    /// <field name="_handler" type="LxnBase.UI.IKeyHandler">
    /// </field>
    /// <field name="_allowBubbling" type="Boolean">
    /// </field>
    /// <field name="_focusedElement" type="Object" domElement="true">
    /// </field>
    this._handler = handler;
    this._allowBubbling = allowBubbling;
}
LxnBase.UI.KeyEventHandler.prototype = {
    
    get_handler: function LxnBase_UI_KeyEventHandler$get_handler() {
        /// <value type="LxnBase.UI.IKeyHandler"></value>
        return this._handler;
    },
    set_handler: function LxnBase_UI_KeyEventHandler$set_handler(value) {
        /// <value type="LxnBase.UI.IKeyHandler"></value>
        this._handler = value;
        return value;
    },
    
    get_allowBubbling: function LxnBase_UI_KeyEventHandler$get_allowBubbling() {
        /// <value type="Boolean"></value>
        return this._allowBubbling;
    },
    set_allowBubbling: function LxnBase_UI_KeyEventHandler$set_allowBubbling(value) {
        /// <value type="Boolean"></value>
        this._allowBubbling = value;
        return value;
    },
    
    get_focusedElement: function LxnBase_UI_KeyEventHandler$get_focusedElement() {
        /// <value type="Object" domElement="true"></value>
        return this._focusedElement;
    },
    set_focusedElement: function LxnBase_UI_KeyEventHandler$set_focusedElement(value) {
        /// <value type="Object" domElement="true"></value>
        this._focusedElement = value;
        return value;
    },
    
    _handler: null,
    _allowBubbling: false,
    _focusedElement: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.ListArgs

LxnBase.UI.ListArgs = function LxnBase_UI_ListArgs(type, baseRequest) {
    /// <param name="type" type="String">
    /// </param>
    /// <param name="baseRequest" type="LxnBase.Data.RangeRequest">
    /// </param>
    /// <field name="type" type="String">
    /// </field>
    /// <field name="baseRequest" type="LxnBase.Data.RangeRequest">
    /// </field>
    if (baseRequest != null) {
        baseRequest.ClassName = type;
    }
    this.type = type;
    this.baseRequest = baseRequest;
}
LxnBase.UI.ListArgs.prototype = {
    type: null,
    baseRequest: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.LxnMenu

LxnBase.UI.LxnMenu = function LxnBase_UI_LxnMenu(config) {
    /// <param name="config" type="Object">
    /// </param>
    LxnBase.UI.LxnMenu.initializeBase(this, [ config ]);
}
LxnBase.UI.LxnMenu.prototype = {
    
    tryActivateItem: function LxnBase_UI_LxnMenu$tryActivateItem(itemIndex) {
        /// <param name="itemIndex" type="Number" integer="true">
        /// </param>
        this.tryActivate(itemIndex, itemIndex + 1);
    }
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.MessageBoxWrap

LxnBase.UI.MessageBoxWrap = function LxnBase_UI_MessageBoxWrap() {
}
LxnBase.UI.MessageBoxWrap.show = function LxnBase_UI_MessageBoxWrap$show(title, msg, icon, buttons, func) {
    /// <param name="title" type="String">
    /// </param>
    /// <param name="msg" type="String">
    /// </param>
    /// <param name="icon" type="String">
    /// </param>
    /// <param name="buttons" type="Object">
    /// </param>
    /// <param name="func" type="Function">
    /// </param>
    var config = title;
    if (Type.canCast(title, String)) {
        var dictionary = { title: title, msg: msg, icon: icon, buttons: buttons };
        if (!ss.isNullOrUndefined(func)) {
            dictionary['fn'] = func;
        }
        config = dictionary;
    }
    var fn = config.fn;
    config.fn = LxnBase.UI.MessageBoxWrap._getCallbackDelegate(fn);
    var messageBox = Ext.MessageBox.show(config);
    LxnBase.UI.EventsManager.registerKeyDownHandler(messageBox, false);
}
LxnBase.UI.MessageBoxWrap.confirm = function LxnBase_UI_MessageBoxWrap$confirm(title, msg, fn) {
    /// <param name="title" type="String">
    /// </param>
    /// <param name="msg" type="String">
    /// </param>
    /// <param name="fn" type="Function">
    /// </param>
    var messageBox = Ext.MessageBox.confirm(title, msg, LxnBase.UI.MessageBoxWrap._getCallbackDelegate(fn));
    LxnBase.UI.EventsManager.registerKeyDownHandler(messageBox, false);
}
LxnBase.UI.MessageBoxWrap.prompt = function LxnBase_UI_MessageBoxWrap$prompt(title, msg, fn) {
    /// <param name="title" type="String">
    /// </param>
    /// <param name="msg" type="String">
    /// </param>
    /// <param name="fn" type="Function">
    /// </param>
    var messageBox = Ext.MessageBox.prompt(title, msg, LxnBase.UI.MessageBoxWrap._getCallbackDelegate(fn));
    LxnBase.UI.EventsManager.registerKeyDownHandler(messageBox, false);
}
LxnBase.UI.MessageBoxWrap._getCallbackDelegate = function LxnBase_UI_MessageBoxWrap$_getCallbackDelegate(fn) {
    /// <param name="fn" type="Function">
    /// </param>
    /// <returns type="ss.Delegate"></returns>
    if (ss.isNullOrUndefined(fn)) {
        return function() {
            LxnBase.UI.EventsManager.unregisterKeyDownHandler();
        };
    }
    return function(button, text) {
        LxnBase.UI.EventsManager.unregisterKeyDownHandler();
        fn(button, text);
    };
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.Messages

LxnBase.UI.Messages = function LxnBase_UI_Messages(config) {
    /// <param name="config" type="Object">
    /// </param>
    /// <field name="_maxMessageCount" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="_panel" type="Ext.Panel">
    /// </field>
    /// <field name="_region" type="Ext.lib.Region">
    /// </field>
    /// <field name="_containerDiv" type="Object" domElement="true">
    /// </field>
    /// <field name="_unreadMessageCount" type="Number" integer="true">
    /// </field>
    /// <field name="_messageCountElement" type="Object" domElement="true">
    /// </field>
    if (!Object.keyExists(config, 'title')) {
        config['title'] = LxnBase.BaseRes.messages_Title;
    }
    this._panel = new Ext.Panel(config);
    this._panel.addListener('expand', ss.Delegate.create(this, this._clearMessageCounter));
    LxnBase.MessageRegister.add_newMessage(ss.Delegate.create(this, this._onNewMessage));
}
LxnBase.UI.Messages._getMessageElement = function LxnBase_UI_Messages$_getMessageElement(e) {
    /// <param name="e" type="LxnBase.MessageRegisterEventArgs">
    /// </param>
    /// <returns type="Object" domElement="true"></returns>
    var container = document.createElement('div');
    container.className = 'message';
    var icon = document.createElement('div');
    var className = 'messageIcon ';
    if (!e.get_type()) {
        className += 'info';
    }
    else if (e.get_type() === 1) {
        className += 'warn';
    }
    else if (e.get_type() === 2) {
        className += 'error';
    }
    icon.className = className;
    container.appendChild(icon);
    var timestamp = document.createElement('div');
    timestamp.className = 'messageTimestamp';
    timestamp.innerHTML = Date.get_now().format('H:i:s');
    container.appendChild(timestamp);
    var messageCaption = document.createElement('div');
    messageCaption.className = 'messageCaption';
    if (!ss.isNullOrUndefined(e.get_messageCaption())) {
        messageCaption.innerHTML = e.get_messageCaption();
        if (!ss.isNullOrUndefined(e.get_message())) {
            messageCaption.innerHTML += ': ' + e.get_message();
        }
    }
    else {
        messageCaption.innerHTML = e.get_message();
    }
    container.appendChild(messageCaption);
    if (!String.isNullOrEmpty(e.get_details())) {
        var messageDetails = document.createElement('div');
        messageDetails.className = 'messageDetails';
        messageDetails.innerHTML = e.get_details();
        container.appendChild(messageDetails);
    }
    return container;
}
LxnBase.UI.Messages.prototype = {
    
    get_widget: function LxnBase_UI_Messages$get_widget() {
        /// <value type="Ext.Component"></value>
        return this._panel;
    },
    
    setLayoutRegion: function LxnBase_UI_Messages$setLayoutRegion(region) {
        /// <param name="region" type="Ext.lib.Region">
        /// </param>
        this._region = region;
        this._messageCountElement = document.createElement('div');
        this._messageCountElement.className = 'message-count';
        var collapsedHeader = this._region.getCollapsedEl().dom;
        collapsedHeader.addEventListener('click', ss.Delegate.create(this, function() {
            this._clearMessageCounter();
        }), false);
        collapsedHeader.insertBefore(this._messageCountElement, collapsedHeader.firstChild);
    },
    
    _onNewMessage: function LxnBase_UI_Messages$_onNewMessage(sender, e) {
        /// <param name="sender" type="Object">
        /// </param>
        /// <param name="e" type="LxnBase.MessageRegisterEventArgs">
        /// </param>
        if (e.get_handled()) {
            return;
        }
        if (this._containerDiv == null) {
            this._containerDiv = document.createElement('div');
            this._panel.body.dom.appendChild(this._containerDiv);
        }
        var newMessage = LxnBase.UI.Messages._getMessageElement(e);
        var lastMessage = (!this._containerDiv.children.length) ? null : this._containerDiv.children[0];
        if (lastMessage == null) {
            this._containerDiv.appendChild(newMessage);
        }
        else {
            this._containerDiv.insertBefore(newMessage, lastMessage);
        }
        if (this._containerDiv.children.length > 50) {
            this._containerDiv.removeChild(this._containerDiv.children[this._containerDiv.children.length - 1]);
        }
        if (this._messageCountElement != null && this._panel.collapsed) {
            ++this._unreadMessageCount;
            this._messageCountElement.innerHTML = LxnBase.StringUtility.getNumberText(this._unreadMessageCount, LxnBase.BaseRes.unreadMessageCount_Msg1, LxnBase.BaseRes.unreadMessageCount_Msg2, LxnBase.BaseRes.unreadMessageCount_Msg3);
        }
    },
    
    _clearMessageCounter: function LxnBase_UI_Messages$_clearMessageCounter() {
        this._unreadMessageCount = 0;
        if (this._messageCountElement != null) {
            this._messageCountElement.innerHTML = '';
        }
    },
    
    _panel: null,
    _region: null,
    _containerDiv: null,
    _unreadMessageCount: 0,
    _messageCountElement: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.PropertyFilterExtention

LxnBase.UI.PropertyFilterExtention = function LxnBase_UI_PropertyFilterExtention() {
}
LxnBase.UI.PropertyFilterExtention.createFilter = function LxnBase_UI_PropertyFilterExtention$createFilter(propertyName, op, value, not) {
    /// <param name="propertyName" type="String">
    /// </param>
    /// <param name="op" type="LxnBase.Data.FilterOperator">
    /// </param>
    /// <param name="value" type="Object">
    /// </param>
    /// <param name="not" type="Boolean">
    /// </param>
    /// <returns type="LxnBase.Data.PropertyFilter"></returns>
    var condition = new LxnBase.Data.PropertyFilterCondition();
    condition.Not = not;
    condition.Operator = op;
    condition.Value = value;
    var filter = new LxnBase.Data.PropertyFilter();
    filter.Property = propertyName;
    filter.Conditions = [ condition ];
    return filter;
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.ReplaceForm

LxnBase.UI.ReplaceForm = function LxnBase_UI_ReplaceForm(type, id) {
    /// <param name="type" type="String">
    /// </param>
    /// <param name="id" type="Object">
    /// </param>
    /// <field name="_type$1" type="String">
    /// </field>
    /// <field name="_id$1" type="Object">
    /// </field>
    /// <field name="_messageBox$1" type="Ext.BoxComponent">
    /// </field>
    /// <field name="_replacingObjectSelector$1" type="LxnBase.UI.Controls.ObjectSelector">
    /// </field>
    /// <field name="_replaceButton$1" type="Ext.Button">
    /// </field>
    /// <field name="_cancelButton$1" type="Ext.Button">
    /// </field>
    /// <field name="_objectForReplace$1" type="LxnBase.Data.ObjectInfo">
    /// </field>
    /// <field name="_itemConfig$1" type="LxnBase.Services.ItemConfig">
    /// </field>
    LxnBase.UI.ReplaceForm.initializeBase(this);
    this._type$1 = type;
    this._id$1 = id;
    this.get_window().setTitle(LxnBase.BaseRes.replaceDelete);
    this.get_window().cls += ' replace-form';
    this.get_form().labelWidth = true;
    this._createFormItems$1();
    this._replaceButton$1 = this.get_form().addButton(LxnBase.BaseRes.replace, ss.Delegate.create(this, this.save));
    this._cancelButton$1 = this.get_form().addButton(LxnBase.BaseRes.cancel, ss.Delegate.create(this, this.cancel));
    this.set_fields([ this._replacingObjectSelector$1.get_widget() ]);
    this.set_componentSequence([ this._replacingObjectSelector$1.get_widget(), this._replaceButton$1, this._cancelButton$1 ]);
}
LxnBase.UI.ReplaceForm.prototype = {
    
    open: function LxnBase_UI_ReplaceForm$open() {
        LxnBase.Data.ConfigManager.getViewConfig(this._type$1, ss.Delegate.create(this, function(config) {
            this._itemConfig$1 = config;
            LxnBase.Services.GenericService.GetDependencies(this._type$1, this._id$1, ss.Delegate.create(this, this._onLoad$1), null);
        }));
    },
    
    _onLoad$1: function LxnBase_UI_ReplaceForm$_onLoad$1(result) {
        /// <param name="result" type="Object">
        /// </param>
        this._objectForReplace$1 = result;
        this._messageBox$1.autoEl = { html: this._renderMessage$1() };
        this.get_window().show();
    },
    
    _renderMessage$1: function LxnBase_UI_ReplaceForm$_renderMessage$1() {
        /// <returns type="String"></returns>
        var text = String.format('<b>{0}</b>', this._objectForReplace$1.Text);
        var builder = new ss.StringBuilder();
        builder.append(String.format(LxnBase.BaseRes.replaceForm_ObjectForReplace, text));
        builder.append('<br/><br/>');
        builder.append(LxnBase.BaseRes.replaceForm_ReplacingObject);
        return builder.toString();
    },
    
    _createFormItems$1: function LxnBase_UI_ReplaceForm$_createFormItems$1() {
        this._messageBox$1 = new Ext.BoxComponent(new Ext.BoxComponentConfig().cls('x-form-item delete-message').toDictionary());
        this._replacingObjectSelector$1 = new LxnBase.UI.Controls.ObjectSelector(new LxnBase.UI.Controls.ObjectSelectorConfig().setClass(this._type$1).selectOnFocus(true).allowBlank(false).width(250).name('replacingObject').hideLabel(true).labelSeparator(''));
        this.get_form().add(this._messageBox$1);
        this.get_form().add(this._replacingObjectSelector$1.get_widget());
    },
    
    onSave: function LxnBase_UI_ReplaceForm$onSave() {
        LxnBase.Services.GenericService.Replace(this._type$1, this._id$1, this._replacingObjectSelector$1.getObjectInfo().Id, true, ss.Delegate.create(this, this.completeSave), null);
    },
    
    onSaved: function LxnBase_UI_ReplaceForm$onSaved(result) {
        /// <param name="result" type="Object">
        /// </param>
        LxnBase.MessageRegister.info(this._itemConfig$1.ListCaption, String.format(LxnBase.BaseRes.replaceForm_ReplaceComplete_Msg, this._objectForReplace$1.Text, this._replacingObjectSelector$1.getObjectInfo().Text));
        LxnBase.UI.ReplaceForm.callBaseMethod(this, 'onSaved', [ result ]);
    },
    
    _type$1: null,
    _id$1: null,
    _messageBox$1: null,
    _replacingObjectSelector$1: null,
    _replaceButton$1: null,
    _cancelButton$1: null,
    _objectForReplace$1: null,
    _itemConfig$1: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.SelectArgs

LxnBase.UI.SelectArgs = function LxnBase_UI_SelectArgs(type, baseRequest, singleSelect, onSelect, onCancel) {
    /// <param name="type" type="String">
    /// </param>
    /// <param name="baseRequest" type="LxnBase.Data.RangeRequest">
    /// </param>
    /// <param name="singleSelect" type="Boolean">
    /// </param>
    /// <param name="onSelect" type="Function">
    /// </param>
    /// <param name="onCancel" type="Function">
    /// </param>
    /// <field name="type" type="String">
    /// </field>
    /// <field name="baseRequest" type="LxnBase.Data.RangeRequest">
    /// </field>
    /// <field name="singleSelect" type="Boolean">
    /// </field>
    /// <field name="onSelect" type="Function">
    /// </field>
    /// <field name="onCancel" type="Function">
    /// </field>
    this.type = type;
    this.baseRequest = baseRequest;
    this.singleSelect = singleSelect;
    this.onSelect = onSelect;
    this.onCancel = onCancel;
}
LxnBase.UI.SelectArgs.prototype = {
    type: null,
    baseRequest: null,
    singleSelect: false,
    onSelect: null,
    onCancel: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.Tab

LxnBase.UI.Tab = function LxnBase_UI_Tab(config, tabIdentifier) {
    /// <param name="config" type="Object">
    /// </param>
    /// <param name="tabIdentifier" type="String">
    /// </param>
    /// <field name="_tabIdentifier$5" type="String">
    /// </field>
    LxnBase.UI.Tab.initializeBase(this, [ config ]);
    this._tabIdentifier$5 = tabIdentifier;
}
LxnBase.UI.Tab.prototype = {
    
    initComponent: function LxnBase_UI_Tab$initComponent() {
        LxnBase.UI.Tab.callBaseMethod(this, 'initComponent');
        var isFirst = true;
        this.on('activate', ss.Delegate.create(this, function() {
            this.onActivate(isFirst);
            isFirst = false;
        }));
        this.on('deactivate', ss.Delegate.create(this, this.onDeactivate));
    },
    
    get_tabIdentifier: function LxnBase_UI_Tab$get_tabIdentifier() {
        /// <value type="String"></value>
        return this._tabIdentifier$5;
    },
    set_tabIdentifier: function LxnBase_UI_Tab$set_tabIdentifier(value) {
        /// <value type="String"></value>
        this._tabIdentifier$5 = value;
        return value;
    },
    
    restoreFocus: function LxnBase_UI_Tab$restoreFocus() {
    },
    
    handleKeyEvent: function LxnBase_UI_Tab$handleKeyEvent(keyEvent) {
        /// <param name="keyEvent" type="jQueryEvent">
        /// </param>
        /// <returns type="Boolean"></returns>
        return false;
    },
    
    close: function LxnBase_UI_Tab$close() {
        LxnBase.UI.Tabs.close(this);
    },
    
    beforeActivate: function LxnBase_UI_Tab$beforeActivate(pparams) {
        /// <param name="pparams" type="Object">
        /// </param>
    },
    
    onActivate: function LxnBase_UI_Tab$onActivate(isFirst) {
        /// <param name="isFirst" type="Boolean">
        /// </param>
    },
    
    onDeactivate: function LxnBase_UI_Tab$onDeactivate() {
    },
    
    _tabIdentifier$5: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.Tabs

LxnBase.UI.Tabs = function LxnBase_UI_Tabs() {
    /// <field name="_panel" type="Ext.TabPanel" static="true">
    /// </field>
}
LxnBase.UI.Tabs.init = function LxnBase_UI_Tabs$init(config) {
    /// <param name="config" type="Object">
    /// </param>
    config['deferredRender'] = false;
    config['activeTab'] = 0;
    config['enableTabScroll'] = true;
    config['minTabWidth'] = 100;
    LxnBase.UI.Tabs._panel = new Ext.TabPanel(config);
    LxnBase.UI.EventsManager.registerKeyDownHandler(LxnBase.UI.Tabs, false);
}
LxnBase.UI.Tabs.get_widget = function LxnBase_UI_Tabs$get_widget() {
    /// <value type="Ext.Component"></value>
    return LxnBase.UI.Tabs._panel;
}
LxnBase.UI.Tabs.open = function LxnBase_UI_Tabs$open(newTab, tabIdentifier, callback, onActivateParams) {
    /// <param name="newTab" type="Boolean">
    /// </param>
    /// <param name="tabIdentifier" type="String">
    /// </param>
    /// <param name="callback" type="Function">
    /// </param>
    /// <param name="onActivateParams" type="Object">
    /// </param>
    var tab = null;
    if (!newTab) {
        tab = LxnBase.UI.Tabs.findTab(tabIdentifier);
    }
    if (tab != null) {
        tab.beforeActivate(onActivateParams);
        LxnBase.UI.Tabs._panel.setActiveTab(tab.id);
    }
    else {
        tab = callback(tabIdentifier);
        if (tab != null) {
            LxnBase.UI.Tabs._panel.add(tab);
            LxnBase.UI.Tabs._panel.setActiveTab(tab.id);
        }
    }
}
LxnBase.UI.Tabs.handleKeyEvent = function LxnBase_UI_Tabs$handleKeyEvent(keyEvent) {
    /// <param name="keyEvent" type="jQueryEvent">
    /// </param>
    /// <returns type="Boolean"></returns>
    var key = keyEvent.which;
    var isAltKey = !keyEvent.ctrlKey && !keyEvent.shiftKey && keyEvent.altKey;
    var isCtrlKey = keyEvent.ctrlKey && !keyEvent.shiftKey && !keyEvent.altKey;
    if (key === Ext.EventObject.LEFT && isAltKey) {
        keyEvent.preventDefault();
        keyEvent.stopPropagation();
        var tabs = LxnBase.UI.Tabs._panel.items;
        var count = tabs.getCount();
        var pos = tabs.indexOf(LxnBase.UI.Tabs._panel.getActiveTab()) - 1;
        if (pos < 0) {
            pos = count - 1;
        }
        LxnBase.UI.Tabs._panel.setActiveTab(pos);
        return true;
    }
    if (key === Ext.EventObject.RIGHT && isAltKey) {
        keyEvent.preventDefault();
        keyEvent.stopPropagation();
        var tabs = LxnBase.UI.Tabs._panel.items;
        var count = tabs.getCount();
        var pos = tabs.indexOf(LxnBase.UI.Tabs._panel.getActiveTab()) + 1;
        if (pos === count) {
            pos = 0;
        }
        LxnBase.UI.Tabs._panel.setActiveTab(pos);
        return true;
    }
    if (key === Ext.EventObject.W && isCtrlKey) {
        keyEvent.preventDefault();
        keyEvent.stopPropagation();
        var tab = LxnBase.UI.Tabs._panel.getActiveTab();
        if (tab.closable) {
            LxnBase.UI.Tabs.close(tab);
        }
        return true;
    }
    if (isAltKey && (key >= Ext.EventObject.NUM_ZERO && key <= Ext.EventObject.NUM_NINE || key >= Ext.EventObject.ZERO && key <= Ext.EventObject.NINE)) {
        keyEvent.preventDefault();
        keyEvent.stopPropagation();
        var tabs = LxnBase.UI.Tabs._panel.items;
        var count = tabs.getCount();
        var zeroKey = (key <= Ext.EventObject.NINE) ? Ext.EventObject.ZERO : Ext.EventObject.NUM_ZERO;
        var pos = (key === zeroKey) ? 9 : key - zeroKey - 1;
        if (pos < count) {
            LxnBase.UI.Tabs._panel.setActiveTab(pos);
        }
        return true;
    }
    var activeTab = LxnBase.UI.Tabs._panel.activeTab;
    activeTab.handleKeyEvent(keyEvent);
    return false;
}
LxnBase.UI.Tabs.findTab = function LxnBase_UI_Tabs$findTab(tabId) {
    /// <param name="tabId" type="String">
    /// </param>
    /// <returns type="LxnBase.UI.Tab"></returns>
    var tabs = LxnBase.UI.Tabs._panel.items;
    for (var i = 0; i < tabs.getCount(); i++) {
        var tab = tabs.itemAt(i);
        if (tab.get_tabIdentifier() === tabId) {
            return tab;
        }
    }
    return null;
}
LxnBase.UI.Tabs.close = function LxnBase_UI_Tabs$close(tab) {
    /// <param name="tab" type="Ext.Component">
    /// </param>
    LxnBase.UI.Tabs._panel.remove(tab);
}


Type.registerNamespace('LxnBase.UI.AutoControls');

////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.AutoControls.ListMode

LxnBase.UI.AutoControls.ListMode = function() { 
    /// <field name="list" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="select" type="Number" integer="true" static="true">
    /// </field>
};
LxnBase.UI.AutoControls.ListMode.prototype = {
    list: 0, 
    select: 1
}
LxnBase.UI.AutoControls.ListMode.registerEnum('LxnBase.UI.AutoControls.ListMode', false);


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.AutoControls.IReportProvider

LxnBase.UI.AutoControls.IReportProvider = function() { 
};
LxnBase.UI.AutoControls.IReportProvider.prototype = {
    loadReport : null
}
LxnBase.UI.AutoControls.IReportProvider.registerInterface('LxnBase.UI.AutoControls.IReportProvider');


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.AutoControls.AutoGrid

LxnBase.UI.AutoControls.AutoGrid = function LxnBase_UI_AutoControls_AutoGrid(args, config) {
    /// <param name="args" type="LxnBase.UI.AutoControls.AutoGridArgs">
    /// </param>
    /// <param name="config" type="Ext.grid.EditorGridPanelConfig">
    /// </param>
    /// <field name="__onSelect$7" type="Function">
    /// </field>
    /// <field name="__onCancelSelect$7" type="Function">
    /// </field>
    /// <field name="_maxMessageDetailItems$7" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="_listRowNumberLimit$7" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="_selectRowNumberLimit$7" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="_args" type="LxnBase.UI.AutoControls.AutoGridArgs">
    /// </field>
    /// <field name="_baseRequest$7" type="LxnBase.Data.RangeRequest">
    /// </field>
    /// <field name="_filterPlugin$7" type="LxnBase.UI.AutoControls.GridFilterPlugin">
    /// </field>
    /// <field name="_createAction$7" type="Ext.Action">
    /// </field>
    /// <field name="_editAction$7" type="Ext.Action">
    /// </field>
    /// <field name="_copyAction$7" type="Ext.Action">
    /// </field>
    /// <field name="_deleteAction$7" type="Ext.Action">
    /// </field>
    /// <field name="_exportAction$7" type="Ext.Action">
    /// </field>
    /// <field name="_exportSelectionItem$7" type="Ext.menu.Item">
    /// </field>
    /// <field name="_selectAction$7" type="Ext.Action">
    /// </field>
    /// <field name="_resetFilterButton$7" type="Ext.Button">
    /// </field>
    /// <field name="_generalFilterField$7" type="Ext.form.TextField">
    /// </field>
    /// <field name="_pagingToolbar$7" type="Ext.PagingToolbar">
    /// </field>
    /// <field name="_currentPage$7" type="Number" integer="true">
    /// </field>
    /// <field name="_totalPages$7" type="Number" integer="true">
    /// </field>
    /// <field name="_selectedRowNumber$7" type="Number" integer="true">
    /// </field>
    this._selectedRowNumber$7 = -1;
    LxnBase.UI.AutoControls.AutoGrid.initializeBase(this, [ config.cls('autoList').border(false).bodyBorder(false).frame(true).stripeRows(true).header(false).loadMask(true).autoScroll(true).autoHeight(false).clicksToEdit(2).deferRowRender(false).region('center').custom('_args', args).toDictionary() ]);
}
LxnBase.UI.AutoControls.AutoGrid._convertParameter$7 = function LxnBase_UI_AutoControls_AutoGrid$_convertParameter$7(value, columnConfig) {
    /// <param name="value" type="Object">
    /// </param>
    /// <param name="columnConfig" type="LxnBase.Services.ColumnConfig">
    /// </param>
    /// <returns type="String"></returns>
    switch (columnConfig.Type) {
        case 4:
        case 0:
            return String.format("'{0}'", value);
        case 3:
            if (value) {
                return LxnBase.BaseRes.filter_True;
            }
            return LxnBase.BaseRes.filter_False;
        case 5:
            var dateRenderer = LxnBase.UI.AutoControls.ControlFactory.createRenderer(columnConfig);
            return String.format("'{0}'", dateRenderer(value));
        case 2:
            var values = (value);
            var listRenderer = LxnBase.UI.AutoControls.ControlFactory.createRenderer(columnConfig);
            var result = '';
            if (values.length === 1) {
                result = String.format("'{0}'", listRenderer(values[0]));
            }
            else {
                var separator = '';
                var $enum1 = ss.IEnumerator.getEnumerator(values);
                while ($enum1.moveNext()) {
                    var t = $enum1.current;
                    result += String.format("{0}'{1}'", separator, listRenderer(Type.getInstanceType(t)));
                    separator = ', ';
                }
                result = String.format('({0})', result);
            }
            return result;
    }
    return value;
}
LxnBase.UI.AutoControls.AutoGrid.prototype = {
    
    add_onSelect: function LxnBase_UI_AutoControls_AutoGrid$add_onSelect(value) {
        /// <param name="value" type="Function" />
        this.__onSelect$7 = ss.Delegate.combine(this.__onSelect$7, value);
    },
    remove_onSelect: function LxnBase_UI_AutoControls_AutoGrid$remove_onSelect(value) {
        /// <param name="value" type="Function" />
        this.__onSelect$7 = ss.Delegate.remove(this.__onSelect$7, value);
    },
    
    __onSelect$7: null,
    
    add_onCancelSelect: function LxnBase_UI_AutoControls_AutoGrid$add_onCancelSelect(value) {
        /// <param name="value" type="Function" />
        this.__onCancelSelect$7 = ss.Delegate.combine(this.__onCancelSelect$7, value);
    },
    remove_onCancelSelect: function LxnBase_UI_AutoControls_AutoGrid$remove_onCancelSelect(value) {
        /// <param name="value" type="Function" />
        this.__onCancelSelect$7 = ss.Delegate.remove(this.__onCancelSelect$7, value);
    },
    
    __onCancelSelect$7: null,
    
    get_createAction: function LxnBase_UI_AutoControls_AutoGrid$get_createAction() {
        /// <value type="Ext.Action"></value>
        return this._createAction$7;
    },
    
    get_editAction: function LxnBase_UI_AutoControls_AutoGrid$get_editAction() {
        /// <value type="Ext.Action"></value>
        return this._editAction$7;
    },
    
    get_copyAction: function LxnBase_UI_AutoControls_AutoGrid$get_copyAction() {
        /// <value type="Ext.Action"></value>
        return this._copyAction$7;
    },
    
    get_deleteAction: function LxnBase_UI_AutoControls_AutoGrid$get_deleteAction() {
        /// <value type="Ext.Action"></value>
        return this._deleteAction$7;
    },
    
    get_exportAction: function LxnBase_UI_AutoControls_AutoGrid$get_exportAction() {
        /// <value type="Ext.Action"></value>
        return this._exportAction$7;
    },
    
    initComponent: function LxnBase_UI_AutoControls_AutoGrid$initComponent() {
        this._baseRequest$7 = this._args.baseRequest || new LxnBase.Data.RangeRequest();
        this._baseRequest$7.ClassName = this._args.type;
        if (!this._args.nonPaged && (!ss.isNullOrUndefined(this._baseRequest$7.limit) || !this._baseRequest$7.limit)) {
            this._baseRequest$7.limit = (!this._args.mode) ? 25 : 10;
        }
        this.tbar = this._createActionToolbar$7();
        if (!ss.isValue(this.store)) {
            this.store = this._createStore$7();
        }
        if (!ss.isValue(this.selModel)) {
            this.selModel = this._createSelectionModel$7();
        }
        if (!ss.isValue(this._args.columnsConfig)) {
            this._args.columnsConfig = this._getColumnsConfig$7();
        }
        var cols = [];
        cols.addRange(this._args.columnsConfig);
        this.colModel = this._createColumnModel$7(this._args.columnsConfig);
        if (!ss.isValue(this.view)) {
            this.view = new LxnBase.UI.AutoControls.AutoGridView(new Ext.grid.GridViewConfig().forceFit(true).toDictionary());
        }
        (this.view).setColumns(cols);
        this._initStore$7(this.store);
        this._initSelectionModel$7(this.selModel);
        if (!this._args.nonPaged) {
            this._filterPlugin$7 = new LxnBase.UI.AutoControls.GridFilterPlugin(this);
            this.plugins = [ this._filterPlugin$7 ];
            this.bbar = this._createPagingToolbar$7();
        }
        this.on('render', ss.Delegate.create(this, this._onRender$7));
        LxnBase.UI.AutoControls.AutoGrid.callBaseMethod(this, 'initComponent');
    },
    
    get_baseRequest: function LxnBase_UI_AutoControls_AutoGrid$get_baseRequest() {
        /// <value type="LxnBase.Data.RangeRequest"></value>
        return this._baseRequest$7;
    },
    
    get_args: function LxnBase_UI_AutoControls_AutoGrid$get_args() {
        /// <value type="LxnBase.UI.AutoControls.AutoGridArgs"></value>
        return this._args;
    },
    
    get_listConfig: function LxnBase_UI_AutoControls_AutoGrid$get_listConfig() {
        /// <value type="LxnBase.Services.ListConfig"></value>
        return this._args.listConfig;
    },
    
    get_gridView: function LxnBase_UI_AutoControls_AutoGrid$get_gridView() {
        /// <value type="Ext.grid.GridView"></value>
        return this.getView();
    },
    
    get_columnModel: function LxnBase_UI_AutoControls_AutoGrid$get_columnModel() {
        /// <value type="Ext.grid.ColumnModel"></value>
        return this.getColumnModel();
    },
    
    get_selectionModel: function LxnBase_UI_AutoControls_AutoGrid$get_selectionModel() {
        /// <value type="Ext.grid.RowSelectionModel"></value>
        return this.getSelectionModel();
    },
    
    reload: function LxnBase_UI_AutoControls_AutoGrid$reload(reinitPaging) {
        /// <param name="reinitPaging" type="Boolean">
        /// </param>
        this.stopEditing();
        this._baseRequest$7.VisibleProperties = this._getVisibleProperties$7();
        if (reinitPaging) {
            this.store.reload({ params: { start: 0 } });
        }
        else {
            this.store.reload();
        }
    },
    
    refresh: function LxnBase_UI_AutoControls_AutoGrid$refresh() {
        var records = this.store.getRange();
        if (!records.length) {
            return;
        }
        var ids = new Array(records.length);
        for (var i = 0; i < ids.length; ++i) {
            if (records[i].id == null) {
                continue;
            }
            ids[i] = records[i].id;
        }
        LxnBase.Services.GenericService.Refresh(this._args.type, ids, this._getVisibleProperties$7(), this._args.forcedProperties, ss.Delegate.create(this, function(result) {
            if (result == null) {
                return;
            }
            var objs = result;
            for (var i = 0; i < objs.length; ++i) {
                var obj = objs[i];
                if (obj == null) {
                    (records[i].data).__deleted = true;
                }
                else {
                    records[i].data = (this.store.reader).readData(obj);
                }
                records[i].commit();
            }
        }), null);
    },
    
    reloadGrid: function LxnBase_UI_AutoControls_AutoGrid$reloadGrid(request) {
        /// <param name="request" type="LxnBase.Data.RangeRequest">
        /// </param>
        request.VisibleProperties = this._getVisibleProperties$7();
        request.HiddenProperties = this._args.forcedProperties;
        request.limit = this._baseRequest$7.limit;
        this.store.reload({ params: request });
    },
    
    updateRecord: function LxnBase_UI_AutoControls_AutoGrid$updateRecord(values, record) {
        /// <param name="values" type="Object">
        /// </param>
        /// <param name="record" type="Ext.data.Record">
        /// </param>
        var obj = values;
        var recordFields = record.fields.getRange();
        var $enum1 = ss.IEnumerator.getEnumerator(recordFields);
        while ($enum1.moveNext()) {
            var t = $enum1.current;
            var name = t.name;
            if (Object.keyExists(obj, name)) {
                record.data[name] = obj[name];
            }
        }
        record.commit();
    },
    
    getColumnConfig: function LxnBase_UI_AutoControls_AutoGrid$getColumnConfig(columnIndex) {
        /// <param name="columnIndex" type="Number" integer="true">
        /// </param>
        /// <returns type="LxnBase.Services.ColumnConfig"></returns>
        return this.tryGetColumnConfigByName((this.colModel).getColumnId(columnIndex));
    },
    
    getColumnConfigByName: function LxnBase_UI_AutoControls_AutoGrid$getColumnConfigByName(name) {
        /// <param name="name" type="String">
        /// </param>
        /// <returns type="LxnBase.Services.ColumnConfig"></returns>
        var config = this.tryGetColumnConfigByName(name);
        if (config == null) {
            throw new Error('Unknown column name: ' + name);
        }
        return config;
    },
    
    tryGetColumnConfigByName: function LxnBase_UI_AutoControls_AutoGrid$tryGetColumnConfigByName(name) {
        /// <param name="name" type="String">
        /// </param>
        /// <returns type="LxnBase.Services.ColumnConfig"></returns>
        var columnConfigs = this.get_listConfig().Columns;
        var $enum1 = ss.IEnumerator.getEnumerator(columnConfigs);
        while ($enum1.moveNext()) {
            var config = $enum1.current;
            if (config.Name === name) {
                return config;
            }
        }
        return null;
    },
    
    handleKeyEvent: function LxnBase_UI_AutoControls_AutoGrid$handleKeyEvent(keyEvent) {
        /// <param name="keyEvent" type="jQueryEvent">
        /// </param>
        /// <returns type="Boolean"></returns>
        var eventDictionary = Ext.EventObject;
        var key = keyEvent.which;
        var isAdditionalKey = keyEvent.ctrlKey || keyEvent.shiftKey || keyEvent.altKey;
        var isCtrlKey = keyEvent.ctrlKey && !keyEvent.shiftKey && !keyEvent.altKey;
        if (key === eventDictionary['UP']) {
            keyEvent.preventDefault();
            keyEvent.stopPropagation();
            this._restoreFocus$7();
            return true;
        }
        if (key === eventDictionary['DOWN'] && !isAdditionalKey) {
            keyEvent.preventDefault();
            keyEvent.stopPropagation();
            this._restoreFocus$7();
            return true;
        }
        if (key === eventDictionary['F2'] && !isAdditionalKey) {
            if (this.get_selectionModel().getSelections().length !== 1) {
                return true;
            }
            keyEvent.preventDefault();
            keyEvent.stopPropagation();
            this.startEditing(this.store.indexOf(this.get_selectionModel().getSelected()), 1);
            return true;
        }
        if (key === eventDictionary['ESC']) {
            keyEvent.preventDefault();
            keyEvent.stopPropagation();
            this._restoreFocus$7();
            return true;
        }
        if (key === eventDictionary['F'] && isCtrlKey) {
            keyEvent.preventDefault();
            keyEvent.stopPropagation();
            this._generalFilterField$7.focus();
            return true;
        }
        if (!this._args.mode) {
            if (key === eventDictionary['INSERT'] && !isAdditionalKey) {
                keyEvent.preventDefault();
                keyEvent.stopPropagation();
                if (!this.get_listConfig().IsCreationAllowed.IsDisabled && this.get_listConfig().IsCreationAllowed.Visible) {
                    this._create$7();
                }
                return true;
            }
            if (key === eventDictionary['DELETE'] && isCtrlKey) {
                keyEvent.preventDefault();
                keyEvent.stopPropagation();
                if (!this.get_listConfig().IsRemovingAllowed.IsDisabled && this.get_listConfig().IsRemovingAllowed.Visible) {
                    this._remove$7();
                }
                return true;
            }
            if (key === eventDictionary['ENTER'] && !isAdditionalKey) {
                keyEvent.preventDefault();
                keyEvent.stopPropagation();
                this._view$7();
                return true;
            }
            if (key === eventDictionary['ENTER'] && !keyEvent.ctrlKey && keyEvent.shiftKey && !keyEvent.altKey) {
                keyEvent.preventDefault();
                keyEvent.stopPropagation();
                if (!this.get_selectionModel().getSelections().length) {
                    return true;
                }
                if (!this.get_listConfig().IsEditAllowed.IsDisabled && this.get_listConfig().IsEditAllowed.Visible) {
                    this._edit$7();
                }
                return true;
            }
            if (key === eventDictionary['D'] && isCtrlKey) {
                keyEvent.preventDefault();
                keyEvent.stopPropagation();
                if (!this.get_listConfig().IsCreationAllowed.IsDisabled && this.get_listConfig().IsCreationAllowed.Visible) {
                    this._copy$7();
                }
                return true;
            }
        }
        else {
            if (key === eventDictionary['ENTER'] && !isAdditionalKey) {
                keyEvent.preventDefault();
                keyEvent.stopPropagation();
                this._select$7();
                return true;
            }
        }
        if (key === eventDictionary['PAGE_UP'] && !isAdditionalKey) {
            keyEvent.preventDefault();
            keyEvent.stopPropagation();
            if (this._currentPage$7 > 1) {
                this._pagingToolbar$7.changePage(this._currentPage$7 - 1);
            }
            return true;
        }
        if (key === eventDictionary['PAGE_DOWN'] && !isAdditionalKey) {
            keyEvent.preventDefault();
            keyEvent.stopPropagation();
            if (this._currentPage$7 < this._totalPages$7) {
                this._pagingToolbar$7.changePage(this._currentPage$7 + 1);
            }
            return true;
        }
        if (key === eventDictionary['HOME'] && isCtrlKey) {
            keyEvent.preventDefault();
            keyEvent.stopPropagation();
            if (this._currentPage$7 !== 1) {
                this._pagingToolbar$7.changePage(1);
            }
            this.get_selectionModel().selectRow(0);
            return true;
        }
        if (key === eventDictionary['END'] && isCtrlKey) {
            keyEvent.preventDefault();
            keyEvent.stopPropagation();
            if (this._currentPage$7 !== this._totalPages$7) {
                this._pagingToolbar$7.changePage(this._totalPages$7);
            }
            this.get_selectionModel().selectRow(this.store.getCount() - 1);
            return true;
        }
        if (key === eventDictionary['HOME'] && !isAdditionalKey) {
            keyEvent.preventDefault();
            keyEvent.stopPropagation();
            this.get_selectionModel().selectRow(0);
            return true;
        }
        if (key === eventDictionary['END'] && !isAdditionalKey) {
            keyEvent.preventDefault();
            keyEvent.stopPropagation();
            this.get_selectionModel().selectRow(this.store.getCount() - 1);
            return true;
        }
        if (key === eventDictionary['R'] && isCtrlKey) {
            keyEvent.preventDefault();
            keyEvent.stopPropagation();
            this.reload(false);
            return true;
        }
        if (key === eventDictionary['G'] && isCtrlKey) {
            keyEvent.preventDefault();
            keyEvent.stopPropagation();
            var field = this._pagingToolbar$7['field'];
            field.focus();
            return true;
        }
        return false;
    },
    
    reloadWithData: function LxnBase_UI_AutoControls_AutoGrid$reloadWithData(values) {
        /// <param name="values" type="LxnBase.Data.RangeResponse">
        /// </param>
        (this.store.proxy).setResponse(values);
        this.reload(false);
    },
    
    getSelectedIds: function LxnBase_UI_AutoControls_AutoGrid$getSelectedIds() {
        /// <returns type="Array" elementType="Object"></returns>
        var records = this.get_selectionModel().getSelections();
        var ids = [];
        var $enum1 = ss.IEnumerator.getEnumerator(records);
        while ($enum1.moveNext()) {
            var t = $enum1.current;
            ids.add(t.id);
        }
        return ids;
    },
    
    getSelectedItems: function LxnBase_UI_AutoControls_AutoGrid$getSelectedItems() {
        /// <returns type="Array" elementType="ObjectInfo"></returns>
        var records = this.get_selectionModel().getSelections();
        var list = new Array(records.length);
        for (var i = 0; i < records.length; i++) {
            list[i] = this._getInfo$7(records[i]);
        }
        return list;
    },
    
    _getInfo$7: function LxnBase_UI_AutoControls_AutoGrid$_getInfo$7(record) {
        /// <param name="record" type="Ext.data.Record">
        /// </param>
        /// <returns type="LxnBase.Data.ObjectInfo"></returns>
        var info = new LxnBase.Data.ObjectInfo();
        info.Id = record.id;
        info.Text = this._resolveReferenceText$7(record);
        info.Type = this._resolveType$7(record);
        return info;
    },
    
    _onRender$7: function LxnBase_UI_AutoControls_AutoGrid$_onRender$7() {
        this.get_gridView().on('refresh', ss.Delegate.create(this, function() {
            if (!this.store.getCount()) {
                return;
            }
            if (this._selectedRowNumber$7 !== -1) {
                this.get_selectionModel().selectRow(this._selectedRowNumber$7);
                return;
            }
            var index = (this.get_selectionModel()['lastActive'] || 0);
            if (!!index) {
                if (index >= this.store.getCount()) {
                    index = this.store.getCount() - 1;
                }
                this.get_selectionModel().selectRow(index);
                this.get_gridView().focusRow(index);
            }
        }));
        this._load$7();
    },
    
    _load$7: function LxnBase_UI_AutoControls_AutoGrid$_load$7() {
        if (this._args.nonPaged) {
            LxnBase.Services.GenericService.GetRange(this._args.type, null, ss.Delegate.create(this, function(result) {
                this.store.loadData((result).List);
            }), null);
        }
        else {
            this._baseRequest$7.VisibleProperties = this._getVisibleProperties$7();
            this._baseRequest$7.HiddenProperties = this._args.forcedProperties;
            this.store.load();
        }
    },
    
    _createStore$7: function LxnBase_UI_AutoControls_AutoGrid$_createStore$7() {
        /// <returns type="Ext.data.Store"></returns>
        var gridStore;
        var columnConfigs = this.get_listConfig().Columns;
        var dataFields = [];
        dataFields.add('Id');
        dataFields.add('__class');
        dataFields.add('Version');
        var $enum1 = ss.IEnumerator.getEnumerator(columnConfigs);
        while ($enum1.moveNext()) {
            var t = $enum1.current;
            dataFields.add(t.Name);
        }
        if (this._args.nonPaged) {
            gridStore = new Ext.data.ArrayStore(new Ext.data.ArrayStoreConfig().id(0).fields(dataFields).toDictionary());
        }
        else {
            var proxy = LxnBase.Services.GenericService.getRangeProxy(this._args.type);
            proxy.on('load', ss.Delegate.create(this, this._onDataProxyLoaded$7));
            gridStore = new LxnBase.Data.GenericStore(new Ext.data.StoreConfig().baseParams(this._baseRequest$7).proxy(proxy).reader(new LxnBase.Data.RangeReader({ id: 0, root: 'List', totalProperty: 'TotalCount' }, dataFields)).remoteSort(true).toDictionary());
        }
        return gridStore;
    },
    
    _initStore$7: function LxnBase_UI_AutoControls_AutoGrid$_initStore$7(gridStore) {
        /// <param name="gridStore" type="Ext.data.Store">
        /// </param>
        gridStore.on('update', ss.Delegate.create(this, function(s, record, operation) {
            if (operation !== Ext.data.Record.EDIT) {
                return;
            }
            this._update$7(record);
        }));
    },
    
    _createSelectionModel$7: function LxnBase_UI_AutoControls_AutoGrid$_createSelectionModel$7() {
        /// <returns type="Ext.grid.AbstractSelectionModel"></returns>
        var model = new Ext.grid.CheckboxSelectionModel();
        model.singleSelect = this.get_listConfig().SingleSelect;
        return model;
    },
    
    _initSelectionModel$7: function LxnBase_UI_AutoControls_AutoGrid$_initSelectionModel$7(model) {
        /// <param name="model" type="Ext.grid.AbstractSelectionModel">
        /// </param>
        model.on('selectionchange', ss.Delegate.create(this, this._onSelectonChanged$7));
    },
    
    _createColumnModel$7: function LxnBase_UI_AutoControls_AutoGrid$_createColumnModel$7(columnsConfig) {
        /// <param name="columnsConfig" type="Array">
        /// </param>
        /// <returns type="Ext.grid.ColumnModel"></returns>
        if (!(this.selModel).singleSelect) {
            columnsConfig.insert(0, this.selModel);
        }
        return new Ext.grid.ColumnModel(new Ext.grid.ColumnModelConfig().columns(columnsConfig).listeners({ hiddenchange: ss.Delegate.create(this, function(columnModel, columnIndex, isHidden) {
            if (this._generalFilterField$7.getValue() != null && this._applyGeneralFilter$7(false)) {
                this.reload(true);
            }
            else if (!isHidden) {
                this.reload(false);
            }
        }) }).toDictionary());
    },
    
    _getColumnsConfig$7: function LxnBase_UI_AutoControls_AutoGrid$_getColumnsConfig$7() {
        /// <returns type="Array"></returns>
        var columnConfigs = this.get_listConfig().Columns;
        var cols = [];
        var isVisiblePropertiesPassed = !ss.isNullOrUndefined(this._baseRequest$7.VisibleProperties);
        var $enum1 = ss.IEnumerator.getEnumerator(columnConfigs);
        while ($enum1.moveNext()) {
            var config = $enum1.current;
            if (this._args.mode === 1 && !config.Type) {
                (config).RenderAsString = true;
            }
            var renderer = LxnBase.UI.AutoControls.ControlFactory.createRenderer(config);
            if (!this._args.mode && renderer == null && config.IsReference && LxnBase.UI.FormsRegistry.hasViewForm(this._args.type)) {
                renderer = LxnBase.UI.AutoControls.ControlFactory.createRefrenceRenderer(config, this._args.type);
            }
            var editor = null;
            if (!this._args.mode && !config.IsReadOnly && this.get_listConfig().IsQuickEditAllowed) {
                editor = LxnBase.UI.AutoControls.ControlFactory.createEditor(config, true);
            }
            var cfg = new Ext.grid.ColumnConfig().id(config.Name).header(config.Caption).sortable(config.IsPersistent).dataIndex(config.Name).hidden((isVisiblePropertiesPassed) ? !this._baseRequest$7.VisibleProperties.contains(config.Name) : config.Hidden);
            if (renderer != null) {
                cfg.renderer(renderer);
            }
            if (editor != null) {
                cfg.editor(editor);
            }
            if (!!config.ListWidth) {
                cfg.width(config.ListWidth);
            }
            cols.add(cfg.toDictionary());
        }
        return cols;
    },
    
    _createActionToolbar$7: function LxnBase_UI_AutoControls_AutoGrid$_createActionToolbar$7() {
        /// <returns type="Object"></returns>
        var list = [];
        if (!this._args.mode) {
            if (this.get_listConfig().IsCreationAllowed.Visible) {
                this._createAction$7 = new Ext.Action(new Ext.ActionConfig().text(LxnBase.BaseRes.createItem_Lower).handler(ss.Delegate.create(this, this._create$7)).disabled(this.get_listConfig().IsCreationAllowed.IsDisabled).custom('tooltip', this.get_listConfig().IsCreationAllowed.DisableInfo).toDictionary());
                list.add(this._createAction$7);
            }
            if (this.get_listConfig().IsEditAllowed.Visible) {
                this._editAction$7 = new Ext.Action(new Ext.ActionConfig().text(LxnBase.BaseRes.edit_Lower).disabled(true).handler(ss.Delegate.create(this, this._edit$7)).custom('tooltip', this.get_listConfig().IsEditAllowed.DisableInfo).toDictionary());
                list.add(this._editAction$7);
            }
            if (this.get_listConfig().IsCopyingAllowed.Visible) {
                this._copyAction$7 = new Ext.Action(new Ext.ActionConfig().text(LxnBase.BaseRes.copy_Lower).disabled(true).handler(ss.Delegate.create(this, this._copy$7)).custom('tooltip', this.get_listConfig().IsCreationAllowed.DisableInfo).toDictionary());
                list.add(this._copyAction$7);
            }
            if (this.get_listConfig().IsRemovingAllowed.Visible) {
                this._deleteAction$7 = new Ext.Action(new Ext.ActionConfig().text(LxnBase.BaseRes.remove_Lower).disabled(true).handler(ss.Delegate.create(this, this._remove$7)).custom('tooltip', this.get_listConfig().IsRemovingAllowed.DisableInfo).toDictionary());
                list.add(this._deleteAction$7);
            }
            this._args.onCreateActionToolbar(list, this);
            if (!this._args.nonPaged) {
                this._resetFilterButton$7 = new Ext.Button(new Ext.ButtonConfig().text(LxnBase.BaseRes.autoGrid_ResetFilter_Title).disabled(true).handler(ss.Delegate.create(this, this._resetFilter$7)).toDictionary());
                if (list.length > 0) {
                    list.add('-');
                }
                list.add(this._resetFilterButton$7);
            }
            this._exportSelectionItem$7 = new Ext.menu.Item(new Ext.menu.ItemConfig().text(LxnBase.BaseRes.export_Selection).handler(ss.Delegate.create(this, function() {
                this._gridExport$7(1);
            })).disabled(true).toDictionary());
            this._exportAction$7 = new Ext.Action(new Ext.ActionConfig().iconCls('export').disabled(false).custom('menu', new Ext.menu.Menu(new Ext.menu.MenuConfig().items([ new Ext.menu.Item(new Ext.menu.ItemConfig().text(LxnBase.BaseRes.export_All).handler(ss.Delegate.create(this, function() {
                this._gridExport$7(0);
            })).toDictionary()), this._exportSelectionItem$7, new Ext.menu.Item(new Ext.menu.ItemConfig().text(LxnBase.BaseRes.export_ExceptSelection).handler(ss.Delegate.create(this, function() {
                this._gridExport$7(2);
            })).toDictionary()) ]).listeners({ beforeshow: ss.Delegate.create(this, function(component) {
                var menu = component;
                var totalCount = this.store.getTotalCount();
                var selectedCount = this.get_selectionModel().getSelections().length;
                var item = (menu.items).itemAt(0);
                item.setText(String.format(LxnBase.BaseRes.export_All, totalCount));
                item = (menu.items).itemAt(1);
                item.setText(String.format(LxnBase.BaseRes.export_Selection, selectedCount));
                item = (menu.items).itemAt(2);
                item.setText(String.format(LxnBase.BaseRes.export_ExceptSelection, totalCount - selectedCount));
            }) }).toDictionary())).toDictionary());
            list.add(this._exportAction$7);
        }
        var spacePos = (!list.length) ? 0 : list.length - 1;
        if (this.get_listConfig().Filterable) {
            this._generalFilterField$7 = new Ext.form.TextField(new Ext.form.TextFieldConfig().value(this._baseRequest$7.GeneralFilter).enableKeyEvents(true).listeners({ keydown: ss.Delegate.create(this, function(objthis, e) {
                var key = e.getKey();
                if (key === Ext.EventObject.ENTER) {
                    e.stopEvent();
                    this._applyGeneralFilter$7(true);
                }
                else if (key === Ext.EventObject.ESC) {
                    objthis.setValue(this._baseRequest$7.GeneralFilter);
                }
            }), change: ss.Delegate.create(this, function() {
                this._applyGeneralFilter$7(true);
            }) }).toDictionary());
            var component = new Ext.BoxComponent(new Ext.BoxComponentConfig().autoEl({ tag: 'div' }).cls('filter').toDictionary());
            list.addRange([ component, this._generalFilterField$7 ]);
        }
        if (this._args.mode === 1) {
            list.add('->');
            this._selectAction$7 = new Ext.Action(new Ext.ActionConfig().text(LxnBase.BaseRes.select_Lower).handler(ss.Delegate.create(this, this._select$7)).disabled(true).toDictionary());
            var cancelAction = new Ext.Action(new Ext.ActionConfig().text(LxnBase.BaseRes.cancel_Lower).handler(ss.Delegate.create(this, function() {
                if (this.__onCancelSelect$7 != null) {
                    this.__onCancelSelect$7();
                }
            })).toDictionary());
            list.add(this._selectAction$7);
            list.add(cancelAction);
        }
        if (!list.length) {
            return null;
        }
        if (!this._args.mode && list.length !== spacePos) {
            list.insert(spacePos, '->');
        }
        var toolbar = new Ext.Toolbar(list);
        toolbar.cls = 'autoGridToolbar';
        return toolbar;
    },
    
    _createPagingToolbar$7: function LxnBase_UI_AutoControls_AutoGrid$_createPagingToolbar$7() {
        /// <returns type="Ext.PagingToolbar"></returns>
        var pagingToolbarConfig = new Ext.PagingToolbarConfig().pageSize(this._baseRequest$7.limit).store(this.store).displayInfo(true).displayMsg(LxnBase.BaseRes.autoGrid_DispayMsg).emptyMsg(LxnBase.BaseRes.autoGrid_EmptyMsg);
        this._pagingToolbar$7 = new Ext.PagingToolbar(pagingToolbarConfig.toDictionary());
        this._pagingToolbar$7.on('change', ss.Delegate.create(this, function(obj, changeEvent) {
            this._currentPage$7 = changeEvent['activePage'];
            this._totalPages$7 = changeEvent['pages'];
        }));
        return this._pagingToolbar$7;
    },
    
    _create$7: function LxnBase_UI_AutoControls_AutoGrid$_create$7() {
        LxnBase.UI.FormsRegistry.editObject(this._args.type, null, null, ss.Delegate.create(this, function(result) {
            var response = result;
            var obj = response.Item;
            var rangeResponse = response.RangeResponse;
            var reference = obj['__reference'];
            if (Type.canCast(reference, Date)) {
                reference = (reference).format('dd.MM.yyyy');
            }
            var message = LxnBase.BaseRes.created + ' ' + reference;
            if (ss.isNullOrUndefined(rangeResponse.SelectedRow)) {
                LxnBase.MessageRegister.info(this.get_listConfig().Caption, message, LxnBase.BaseRes.autoGrid_NotDisplay_Msg);
                return;
            }
            LxnBase.MessageRegister.info(this.get_listConfig().Caption, message);
            this.reloadWithData(rangeResponse);
        }), null, this._baseRequest$7);
    },
    
    _edit$7: function LxnBase_UI_AutoControls_AutoGrid$_edit$7() {
        var record = this.get_selectionModel().getSelected();
        var type = this._resolveType$7(record);
        LxnBase.Services.GenericService.CanUpdate(type, record.id, ss.Delegate.create(this, function(res) {
            var status = res;
            if (!status.IsEnabled) {
                var msg = (ss.isNullOrUndefined(status.DisableInfo)) ? LxnBase.BaseRes.autoGrid_ActionNotPermitted_Msg : status.DisableInfo;
                LxnBase.UI.MessageBoxWrap.show({ title: LxnBase.BaseRes.error, msg: msg, icon: Ext.MessageBox.ERROR, buttons: Ext.MessageBox.OK });
                return;
            }
            LxnBase.UI.FormsRegistry.editObject(type, record.id, null, ss.Delegate.create(this, function(result) {
                var response = result;
                var obj = response.Item;
                var rangeResponse = response.RangeResponse;
                var reference = obj['__reference'];
                if (Type.canCast(reference, Date)) {
                    reference = (reference).format('d.m.Y');
                }
                var message = LxnBase.BaseRes.updated + ' ' + reference;
                if (ss.isNullOrUndefined(rangeResponse.SelectedRow)) {
                    LxnBase.MessageRegister.info(this.get_listConfig().Caption, message, LxnBase.BaseRes.autoGrid_NotDisplay_Msg);
                }
                else {
                    LxnBase.MessageRegister.info(this.get_listConfig().Caption, message);
                }
                this.reloadWithData(rangeResponse);
            }), null, this._baseRequest$7);
        }), null);
    },
    
    _copy$7: function LxnBase_UI_AutoControls_AutoGrid$_copy$7() {
        var selected = this.get_selectionModel().getSelected();
        var type = this._resolveType$7(selected);
        LxnBase.UI.FormsRegistry.editObject(type, selected.id, null, ss.Delegate.create(this, function(result) {
            var response = result;
            var obj = response.Item;
            var rangeResponse = response.RangeResponse;
            var reference = obj['__reference'];
            if (Type.canCast(reference, Date)) {
                reference = (reference).format('d.m.Y');
            }
            var message = LxnBase.BaseRes.created + ' ' + reference;
            if (ss.isNullOrUndefined(rangeResponse.SelectedRow)) {
                LxnBase.MessageRegister.info(this.get_listConfig().Caption, message, LxnBase.BaseRes.autoGrid_NotDisplay_Msg);
                return;
            }
            LxnBase.MessageRegister.info(this.get_listConfig().Caption, message);
            this.reloadWithData(rangeResponse);
        }), null, this._baseRequest$7, 1, true);
    },
    
    _view$7: function LxnBase_UI_AutoControls_AutoGrid$_view$7() {
        var record = this.get_selectionModel().getSelected();
        if (record == null) {
            return;
        }
        var type = this._resolveType$7(record);
        if (LxnBase.UI.FormsRegistry.hasViewForm(type)) {
            LxnBase.UI.FormsRegistry.viewObject(type, record.id);
        }
    },
    
    _remove$7: function LxnBase_UI_AutoControls_AutoGrid$_remove$7() {
        var selected = this.get_selectionModel().getSelections();
        if (selected == null || !selected.length) {
            return;
        }
        var ids = new Array(selected.length);
        for (var i = 0; i < selected.length; i++) {
            ids[i] = (selected[i]).id;
        }
        LxnBase.Services.GenericService.CanDelete(this._args.type, ids, ss.Delegate.create(this, function(res) {
            var status = res;
            if (!status.IsEnabled) {
                var msg = (ss.isNullOrUndefined(status.DisableInfo)) ? LxnBase.BaseRes.autoGrid_ActionNotPermitted_Msg : status.DisableInfo;
                LxnBase.UI.MessageBoxWrap.show({ title: LxnBase.BaseRes.error, msg: msg, icon: Ext.MessageBox.ERROR, buttons: Ext.MessageBox.OK });
                return;
            }
            LxnBase.UI.MessageBoxWrap.confirm(LxnBase.BaseRes.confirmation, LxnBase.StringUtility.getNumberText(selected.length, LxnBase.BaseRes.autoGrid_DeleteMsg1, LxnBase.BaseRes.autoGrid_DeleteMsg2, LxnBase.BaseRes.autoGrid_DeleteMsg3), ss.Delegate.create(this, function(button, text) {
                if (button === 'yes') {
                    this._removeRows$7();
                }
            }));
        }), null);
    },
    
    _gridExport$7: function LxnBase_UI_AutoControls_AutoGrid$_gridExport$7(mode) {
        /// <param name="mode" type="LxnBase.Data.DocumentExportMode">
        /// </param>
        var args = new LxnBase.Data.DocumentExportArgs();
        args.Mode = mode;
        args.Request = this.store.baseParams;
        if (mode === 2 || mode === 1) {
            var records = this.get_selectionModel().getSelections();
            var ids = [];
            var $enum1 = ss.IEnumerator.getEnumerator(records);
            while ($enum1.moveNext()) {
                var t = $enum1.current;
                ids.add(t.id);
            }
            args.SelectedDocuments = ids;
        }
        LxnBase.UI.AutoControls.ReportLoader.load(String.format('export/{0}/Export_{1}.xls', this._args.type, Date.get_now().format('Y-m-d_H-i')), { exportParams: JSON.stringify(args) });
    },
    
    _select$7: function LxnBase_UI_AutoControls_AutoGrid$_select$7() {
        var selections = this.get_selectionModel().getSelections();
        var ids = [];
        var $enum1 = ss.IEnumerator.getEnumerator(selections);
        while ($enum1.moveNext()) {
            var t = $enum1.current;
            ids.add(t.id);
        }
        if (this.__onSelect$7 != null) {
            this.__onSelect$7(ids);
        }
    },
    
    _removeRows$7: function LxnBase_UI_AutoControls_AutoGrid$_removeRows$7() {
        var selectedRecords = this.get_selectionModel().getSelections();
        if (selectedRecords == null) {
            return;
        }
        var selectedIds = new Array(selectedRecords.length);
        for (var i = 0; i < selectedRecords.length; i++) {
            selectedIds[i] = selectedRecords[i].id;
        }
        LxnBase.Services.GenericService.Delete(this._args.type, selectedIds, this._baseRequest$7, ss.Delegate.create(this, function(result) {
            var response = result;
            if (response.Success) {
                this._onDeleteSuccess$7(response.RangeResponse, selectedRecords);
            }
            else if (selectedRecords.length === 1) {
                this._onSingleDeleteFailed$7(selectedRecords[0]);
            }
            else {
                this._onMultipleDeleteFailed$7(response.UndeletableObjects, selectedRecords);
            }
        }), null);
    },
    
    _onDeleteSuccess$7: function LxnBase_UI_AutoControls_AutoGrid$_onDeleteSuccess$7(result, deleted) {
        /// <param name="result" type="LxnBase.Data.RangeResponse">
        /// </param>
        /// <param name="deleted" type="Array" elementType="Record">
        /// </param>
        var message = null;
        if (deleted.length <= 5) {
            message = this._recordListToString$7(deleted, ', ');
        }
        if (String.isNullOrEmpty(message)) {
            LxnBase.MessageRegister.info(this.get_listConfig().Caption + ': ' + LxnBase.StringUtility.getNumberText(deleted.length, LxnBase.BaseRes.autoGrid_DeleteCompletedMsg1, LxnBase.BaseRes.autoGrid_DeleteCompletedMsg2, LxnBase.BaseRes.autoGrid_DeleteCompletedMsg3));
        }
        else {
            LxnBase.MessageRegister.info(this.get_listConfig().Caption, LxnBase.BaseRes.deleted + ' ' + message);
        }
        this.reloadWithData(result);
    },
    
    _onSingleDeleteFailed$7: function LxnBase_UI_AutoControls_AutoGrid$_onSingleDeleteFailed$7(record) {
        /// <param name="record" type="Ext.data.Record">
        /// </param>
        var type = this._resolveType$7(record);
        LxnBase.Services.GenericService.CanReplace(type, record.id, ss.Delegate.create(this, function(result) {
            if (result) {
                var form = new LxnBase.UI.ReplaceForm(type, record.id);
                form.add_saved(ss.Delegate.create(this, function() {
                    this.reload(false);
                }));
                form.open();
            }
            else {
                LxnBase.UI.MessageBoxWrap.show(LxnBase.BaseRes.warning, LxnBase.BaseRes.autoGrid_DeleteConstrainedFailed_Msg + '<br/>' + LxnBase.BaseRes.autoGrid_ReplaceToAdmin_Msg, Ext.MessageBox.WARNING, Ext.MessageBox.OK);
            }
        }), null);
    },
    
    _onMultipleDeleteFailed$7: function LxnBase_UI_AutoControls_AutoGrid$_onMultipleDeleteFailed$7(objects, records) {
        /// <param name="objects" type="Array" elementType="Object">
        /// </param>
        /// <param name="records" type="Array" elementType="Record">
        /// </param>
        var newLine = '<br>';
        var msg = new ss.StringBuilder(LxnBase.BaseRes.autoGrid_DeleteFailed_Msg + newLine + newLine);
        msg.append("<div class='undeletable'>");
        for (var i = 0; i < objects.length; i++) {
            if (i === 10) {
                msg.append('<div>...</div>');
                break;
            }
            var val = objects[i];
            msg.append(String.format('<div>{0}</div>', val[LxnBase.Data.ObjectInfo.TextPos]));
        }
        msg.append('</div>');
        if (objects.length === records.length) {
            LxnBase.UI.MessageBoxWrap.show(LxnBase.BaseRes.warning, msg.toString(), Ext.MessageBox.WARNING, Ext.MessageBox.OK);
        }
        else {
            msg.append(newLine);
            msg.append(LxnBase.BaseRes.autoGrid_ContinueDelete_Msg);
            LxnBase.UI.MessageBoxWrap.show(LxnBase.BaseRes.warning, msg.toString(), Ext.MessageBox.WARNING, Ext.MessageBox.YESNO, ss.Delegate.create(this, function(button, text1) {
                if (button !== 'yes') {
                    return;
                }
                var $enum1 = ss.IEnumerator.getEnumerator(objects);
                while ($enum1.moveNext()) {
                    var val = $enum1.current;
                    var $enum2 = ss.IEnumerator.getEnumerator(records);
                    while ($enum2.moveNext()) {
                        var record = $enum2.current;
                        if (val[LxnBase.Data.ObjectInfo.IdPos] === record.id) {
                            this.get_selectionModel().deselectRow(this.store.indexOf(record));
                            break;
                        }
                    }
                }
                this._removeRows$7();
            }));
        }
    },
    
    _update$7: function LxnBase_UI_AutoControls_AutoGrid$_update$7(record) {
        /// <param name="record" type="Ext.data.Record">
        /// </param>
        var newData = record.getChanges();
        var oldData = record.modified;
        var $dict1 = oldData;
        for (var $key2 in $dict1) {
            var old = { key: $key2, value: $dict1[$key2] };
            if (Type.canCast(newData[old.key], String) && String.isNullOrEmpty(newData[old.key])) {
                newData[old.key] = null;
            }
            if (old.value === newData[old.key]) {
                delete newData[old.key];
            }
        }
        if (!Object.getKeyCount(newData)) {
            record.reject(true);
            return;
        }
        var type = this._resolveType$7(record);
        var version = record.get('Version');
        LxnBase.Services.GenericService.Update(type, record.id, version, newData, this._baseRequest$7, ss.Delegate.create(this, function(result) {
            var response = result;
            var obj = response.Item;
            this.updateRecord(obj, record);
        }), ss.Delegate.create(this, function(args) {
            this._onAutoCommitFailed$7(args, record);
        }));
    },
    
    _applyGeneralFilter$7: function LxnBase_UI_AutoControls_AutoGrid$_applyGeneralFilter$7(reload) {
        /// <param name="reload" type="Boolean">
        /// </param>
        /// <returns type="Boolean"></returns>
        var rangeRequest = this.store.baseParams;
        var filter = this._generalFilterField$7.getValue();
        var properties = this._getVisibleProperties$7();
        if (rangeRequest.GeneralFilter === filter && rangeRequest.VisibleProperties.length === properties.length) {
            var isEquals = true;
            var $enum1 = ss.IEnumerator.getEnumerator(rangeRequest.VisibleProperties);
            while ($enum1.moveNext()) {
                var t = $enum1.current;
                if (!properties.contains(t)) {
                    isEquals = false;
                }
            }
            if (isEquals) {
                return false;
            }
        }
        rangeRequest.GeneralFilter = filter;
        rangeRequest.VisibleProperties = properties;
        if (reload) {
            this.reload(true);
        }
        return true;
    },
    
    _getVisibleProperties$7: function LxnBase_UI_AutoControls_AutoGrid$_getVisibleProperties$7() {
        /// <returns type="Array"></returns>
        var properties = [];
        var $enum1 = ss.IEnumerator.getEnumerator(this._args.listConfig.Columns);
        while ($enum1.moveNext()) {
            var t = $enum1.current;
            if (ss.isNullOrUndefined(this.get_columnModel().getColumnById(t.Name))) {
                continue;
            }
            var isHidden = this.get_columnModel().getColumnById(t.Name).hidden;
            if (ss.isUndefined(isHidden) || !isHidden) {
                properties.add(t.Name);
            }
        }
        return properties;
    },
    
    _resetFilter$7: function LxnBase_UI_AutoControls_AutoGrid$_resetFilter$7() {
        this._baseRequest$7.NamedFilters = null;
        this._baseRequest$7.GeneralFilter = null;
        this._baseRequest$7.Filters = null;
        this._generalFilterField$7.setValue();
        this._filterPlugin$7.resetFilter();
        this.reload(true);
    },
    
    _onAutoCommitFailed$7: function LxnBase_UI_AutoControls_AutoGrid$_onAutoCommitFailed$7(args, record) {
        /// <param name="args" type="LxnBase.Net.WebServiceFailureArgs">
        /// </param>
        /// <param name="record" type="Ext.data.Record">
        /// </param>
        args.set_handled(true);
        var button = new Ext.Button(new Ext.ButtonConfig().text('OK').toDictionary());
        var label = new Ext.form.Label(new Ext.form.LabelConfig().text(args.get_error().Message).style('font-weight: bold').toDictionary());
        var repeatRadio = new Ext.form.Radio(new Ext.form.RadioConfig().boxLabel(LxnBase.BaseRes.autoGrid_RepeatUpdate).style('padding-top: 10px').checked_(true).name('updateOption').toDictionary());
        var rejectRadio = new Ext.form.Radio(new Ext.form.RadioConfig().boxLabel(LxnBase.BaseRes.autoGrid_RejectChanges).name('updateOption').toDictionary());
        var window = new Ext.Window(new Ext.WindowConfig().width(350).modal(true).plain(true).baseCls('x-panel').bodyStyle('padding: 10px').resizable(false).title(LxnBase.BaseRes.error).items([ label, { items: [ repeatRadio, rejectRadio ], bodyStyle: 'padding: 10px' } ]).buttons([ button ]).listeners({ close: LxnBase.UI.EventsManager.unregisterKeyDownHandler }).buttonAlign('center').toDictionary());
        button.on('click', ss.Delegate.create(this, function() {
            if (repeatRadio.checked) {
                this._update$7(record);
            }
            else {
                this.store.rejectChanges();
            }
            window.close();
        }));
        window.show();
        LxnBase.UI.EventsManager.registerKeyDownHandler(window, false);
    },
    
    _onSelectonChanged$7: function LxnBase_UI_AutoControls_AutoGrid$_onSelectonChanged$7(selectionModel) {
        /// <param name="selectionModel" type="Ext.grid.AbstractSelectionModel">
        /// </param>
        var length = this.get_selectionModel().getSelections().length;
        if (this._deleteAction$7 != null && !this.get_listConfig().IsRemovingAllowed.IsDisabled) {
            if (!length) {
                this._deleteAction$7.disable();
            }
            else {
                this._deleteAction$7.enable();
            }
        }
        if (this._editAction$7 != null && !this.get_listConfig().IsEditAllowed.IsDisabled) {
            if (length !== 1) {
                this._editAction$7.disable();
            }
            else {
                this._editAction$7.enable();
            }
        }
        if (this._copyAction$7 != null && !this.get_listConfig().IsCreationAllowed.IsDisabled) {
            if (length !== 1) {
                this._copyAction$7.disable();
            }
            else {
                this._copyAction$7.enable();
            }
        }
        if (this._selectAction$7 != null) {
            if (!length) {
                this._selectAction$7.disable();
            }
            else {
                this._selectAction$7.enable();
            }
        }
        if (this._exportSelectionItem$7 != null) {
            if (!length) {
                this._exportSelectionItem$7.disable();
            }
            else {
                this._exportSelectionItem$7.enable();
            }
        }
    },
    
    _onDataProxyLoaded$7: function LxnBase_UI_AutoControls_AutoGrid$_onDataProxyLoaded$7(responseProxy, responseObject, arg) {
        /// <param name="responseProxy" type="Object">
        /// </param>
        /// <param name="responseObject" type="Object">
        /// </param>
        /// <param name="arg" type="Object">
        /// </param>
        if (this._resetFilterButton$7 != null) {
            if (!String.isNullOrEmpty(this._baseRequest$7.GeneralFilter) || (this._baseRequest$7.Filters != null && this._baseRequest$7.Filters.length > 0)) {
                this._resetFilterButton$7.enable();
                var button = (this._resetFilterButton$7.getEl().child(this._resetFilterButton$7.buttonSelector)).dom;
                button.setAttribute(this._resetFilterButton$7.tooltipType, this._getCurrentFilterDescription$7());
            }
            else {
                this._resetFilterButton$7.disable();
                var button = (this._resetFilterButton$7.getEl().child(this._resetFilterButton$7.buttonSelector)).dom;
                button.setAttribute(this._resetFilterButton$7.tooltipType, '');
            }
        }
        this.store.setDefaultSort(this._baseRequest$7.sort, this._baseRequest$7.dir);
        var response = responseObject;
        if (!ss.isNullOrUndefined(response.SelectedRow)) {
            this._selectedRowNumber$7 = response.SelectedRow;
        }
        else {
            this._selectedRowNumber$7 = -1;
        }
        this._baseRequest$7.PositionableObjectId = null;
    },
    
    _getCurrentFilterDescription$7: function LxnBase_UI_AutoControls_AutoGrid$_getCurrentFilterDescription$7() {
        /// <returns type="String"></returns>
        var builder = new ss.StringBuilder();
        var separator = '';
        if (!String.isNullOrEmpty(this._baseRequest$7.GeneralFilter)) {
            builder.append(String.format(LxnBase.BaseRes.filter_GeneralFilterMsg, this._baseRequest$7.GeneralFilter));
            separator = ' ' + LxnBase.BaseRes.filter_Conjunction;
        }
        if (this._baseRequest$7.Filters != null && this._baseRequest$7.Filters.length > 0) {
            var $enum1 = ss.IEnumerator.getEnumerator(this._baseRequest$7.Filters);
            while ($enum1.moveNext()) {
                var filter = $enum1.current;
                var columnConfig = this.tryGetColumnConfigByName(filter.Property);
                if (columnConfig == null) {
                    continue;
                }
                var $enum2 = ss.IEnumerator.getEnumerator(filter.Conditions);
                while ($enum2.moveNext()) {
                    var condition = $enum2.current;
                    var notStr = (condition.Not) ? ' ' + LxnBase.BaseRes.filter_Not : '';
                    switch (condition.Operator) {
                        case 2:
                            builder.append(String.format("{0} '{1}'{2} {3}", separator, columnConfig.Caption, notStr, LxnBase.EnumUtility.localize(LxnBase.Data.FilterOperator, condition.Operator, LxnBase.BaseRes).toLowerCase()));
                            break;
                        case 1:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 9:
                        case 8:
                            builder.append(String.format("{0} '{1}'{2} {3} {4}", separator, columnConfig.Caption, notStr, LxnBase.EnumUtility.localize(LxnBase.Data.FilterOperator, condition.Operator, LxnBase.BaseRes).toLowerCase(), LxnBase.UI.AutoControls.AutoGrid._convertParameter$7(condition.Value, columnConfig)));
                            break;
                        case 10:
                            var operatorStr = ((condition.Value).length === 1) ? LxnBase.EnumUtility.localize(LxnBase.Data.FilterOperator, 1, LxnBase.BaseRes).toLowerCase() : LxnBase.EnumUtility.localize(LxnBase.Data.FilterOperator, condition.Operator, LxnBase.BaseRes).toLowerCase();
                            builder.append(String.format("{0} '{1}'{2} {3} {4}", separator, columnConfig.Caption, notStr, operatorStr, LxnBase.UI.AutoControls.AutoGrid._convertParameter$7(condition.Value, columnConfig)));
                            break;
                    }
                }
                separator = ' ' + LxnBase.BaseRes.filter_Conjunction;
            }
        }
        if (builder.isEmpty) {
            return '';
        }
        return builder.toString();
    },
    
    _recordListToString$7: function LxnBase_UI_AutoControls_AutoGrid$_recordListToString$7(selected, separator) {
        /// <param name="selected" type="Array" elementType="Record">
        /// </param>
        /// <param name="separator" type="String">
        /// </param>
        /// <returns type="String"></returns>
        var refColumn = this._getReferenceColumn$7();
        if (refColumn == null) {
            return null;
        }
        var builder = new ss.StringBuilder();
        var sep = '';
        var $enum1 = ss.IEnumerator.getEnumerator(selected);
        while ($enum1.moveNext()) {
            var t = $enum1.current;
            var value = t.get(refColumn);
            if (Type.canCast(value, Array)) {
                value = (value)[LxnBase.Data.ObjectInfo.TextPos];
            }
            if (Type.canCast(value, Date)) {
                value = (value).format('d.m.Y');
            }
            builder.append(sep + value);
            sep = separator;
        }
        if (builder.isEmpty) {
            return null;
        }
        return builder.toString();
    },
    
    _getReferenceColumn$7: function LxnBase_UI_AutoControls_AutoGrid$_getReferenceColumn$7() {
        /// <returns type="String"></returns>
        var $enum1 = ss.IEnumerator.getEnumerator(this.get_listConfig().Columns);
        while ($enum1.moveNext()) {
            var config = $enum1.current;
            if (config.IsReference) {
                return config.Name;
            }
        }
        return null;
    },
    
    _restoreFocus$7: function LxnBase_UI_AutoControls_AutoGrid$_restoreFocus$7() {
        var selectedRecord = this.get_selectionModel().getSelected();
        if (ss.isNullOrUndefined(selectedRecord)) {
            if (this.store.getCount() > 0) {
                this.get_selectionModel().selectRow(0);
                this.get_gridView().focusRow(0);
            }
        }
        else {
            this.get_gridView().focusRow(this.store.indexOf(selectedRecord));
        }
    },
    
    _resolveType$7: function LxnBase_UI_AutoControls_AutoGrid$_resolveType$7(record) {
        /// <param name="record" type="Ext.data.Record">
        /// </param>
        /// <returns type="String"></returns>
        var type = record.data.__class;
        if (ss.isNullOrUndefined(type)) {
            type = this._args.type;
        }
        return type;
    },
    
    _resolveReferenceText$7: function LxnBase_UI_AutoControls_AutoGrid$_resolveReferenceText$7(record) {
        /// <param name="record" type="Ext.data.Record">
        /// </param>
        /// <returns type="String"></returns>
        var refColumn = this._getReferenceColumn$7();
        if (refColumn == null) {
            return null;
        }
        return record.get(refColumn);
    },
    
    _args: null,
    _baseRequest$7: null,
    _filterPlugin$7: null,
    _createAction$7: null,
    _editAction$7: null,
    _copyAction$7: null,
    _deleteAction$7: null,
    _exportAction$7: null,
    _exportSelectionItem$7: null,
    _selectAction$7: null,
    _resetFilterButton$7: null,
    _generalFilterField$7: null,
    _pagingToolbar$7: null,
    _currentPage$7: 0,
    _totalPages$7: 0
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.AutoControls.AutoGridArgs

LxnBase.UI.AutoControls.AutoGridArgs = function LxnBase_UI_AutoControls_AutoGridArgs() {
    /// <field name="__createActionToolbar" type="Function">
    /// </field>
    /// <field name="type" type="String">
    /// </field>
    /// <field name="listConfig" type="LxnBase.Services.ListConfig">
    /// </field>
    /// <field name="nonPaged" type="Boolean">
    /// </field>
    /// <field name="baseRequest" type="LxnBase.Data.RangeRequest">
    /// </field>
    /// <field name="forcedProperties" type="Array" elementType="String">
    /// </field>
    /// <field name="autoCommit" type="Boolean">
    /// </field>
    /// <field name="mode" type="LxnBase.UI.AutoControls.ListMode">
    /// </field>
    /// <field name="columnsConfig" type="Array">
    /// </field>
    this.mode = 0;
}
LxnBase.UI.AutoControls.AutoGridArgs.prototype = {
    
    add_createActionToolbar: function LxnBase_UI_AutoControls_AutoGridArgs$add_createActionToolbar(value) {
        /// <param name="value" type="Function" />
        this.__createActionToolbar = ss.Delegate.combine(this.__createActionToolbar, value);
    },
    remove_createActionToolbar: function LxnBase_UI_AutoControls_AutoGridArgs$remove_createActionToolbar(value) {
        /// <param name="value" type="Function" />
        this.__createActionToolbar = ss.Delegate.remove(this.__createActionToolbar, value);
    },
    
    __createActionToolbar: null,
    type: null,
    listConfig: null,
    nonPaged: false,
    baseRequest: null,
    forcedProperties: null,
    autoCommit: true,
    columnsConfig: null,
    
    onCreateActionToolbar: function LxnBase_UI_AutoControls_AutoGridArgs$onCreateActionToolbar(toolbarItems, autoGrid) {
        /// <param name="toolbarItems" type="Array">
        /// </param>
        /// <param name="autoGrid" type="LxnBase.UI.AutoControls.AutoGrid">
        /// </param>
        if (this.__createActionToolbar != null) {
            this.__createActionToolbar(toolbarItems, autoGrid);
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.AutoControls.AutoGridView

LxnBase.UI.AutoControls.AutoGridView = function LxnBase_UI_AutoControls_AutoGridView(config) {
    /// <param name="config" type="Object">
    /// </param>
    /// <field name="_columns$2" type="Array" elementType="Column">
    /// </field>
    /// <field name="_s$2" type="Array" elementType="String">
    /// </field>
    this._s$2 = [ 'CreatedOn', 'CreatedBy', 'ModifiedOn', 'ModifiedBy', 'Id' ];
    LxnBase.UI.AutoControls.AutoGridView.initializeBase(this, [ config ]);
}
LxnBase.UI.AutoControls.AutoGridView.prototype = {
    
    beforeColMenuShow: function LxnBase_UI_AutoControls_AutoGridView$beforeColMenuShow() {
        var colModel = this.cm;
        var colMenu = this.colMenu;
        if ((colMenu.items).length > 0) {
            return;
        }
        var main = [];
        var system = [];
        for (var i = 0; i < this._columns$2.length; i++) {
            var col = this._columns$2[i];
            var index = colModel.findColumnIndex(col.dataIndex);
            var checkItem = new Ext.menu.CheckItem(new Ext.menu.CheckItemConfig().text(col.header).itemId('col-' + col.id).hideOnClick(false).toDictionary());
            checkItem.setChecked(!colModel.isHidden(index));
            if (this._s$2.contains(col.id)) {
                system.insert(this._s$2.indexOf(col.id), checkItem);
            }
            else {
                main.add(checkItem);
            }
        }
        main.sort(function(x, y) {
            return (x).text.compareTo((y).text);
        });
        colMenu.add(main);
        if (system.length > 0) {
            colMenu.add('-');
            colMenu.add(system);
        }
    },
    
    setColumns: function LxnBase_UI_AutoControls_AutoGridView$setColumns(columns) {
        /// <param name="columns" type="Array" elementType="Column">
        /// </param>
        this._columns$2 = columns;
    },
    
    getRowClass: function LxnBase_UI_AutoControls_AutoGridView$getRowClass(record, index, rowParams, store) {
        /// <param name="record" type="Ext.data.Record">
        /// </param>
        /// <param name="index" type="Number">
        /// </param>
        /// <param name="rowParams" type="Object">
        /// </param>
        /// <param name="store" type="Ext.data.Store">
        /// </param>
        /// <returns type="String"></returns>
        var cls = (ss.isValue((record.data).__deleted)) ? 'deleted' : '';
        var custom = this.getCustomRowClass(record);
        return (cls + ' ' + custom).trim();
    },
    
    getCustomRowClass: function LxnBase_UI_AutoControls_AutoGridView$getCustomRowClass(record) {
        /// <param name="record" type="Ext.data.Record">
        /// </param>
        /// <returns type="String"></returns>
        return null;
    },
    
    _columns$2: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.AutoControls.ControlFactory

LxnBase.UI.AutoControls.ControlFactory = function LxnBase_UI_AutoControls_ControlFactory() {
    /// <field name="_minWidth" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="_maxWidth" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="_defaultTextFieldWidth" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="_defaultNumberFieldWidth" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="_customRenderers" type="Object" static="true">
    /// </field>
    /// <field name="_customEditors" type="Object" static="true">
    /// </field>
}
LxnBase.UI.AutoControls.ControlFactory.createRenderer = function LxnBase_UI_AutoControls_ControlFactory$createRenderer(columnConfig) {
    /// <param name="columnConfig" type="LxnBase.Services.ColumnConfig">
    /// </param>
    /// <returns type="ss.Delegate"></returns>
    if (columnConfig.Type === 3) {
        return LxnBase.UI.AutoControls.ControlFactory._boolRenderer;
    }
    if (columnConfig.Type === 1) {
        return LxnBase.UI.AutoControls.ControlFactory._getNumberRenderer(columnConfig);
    }
    if (columnConfig.Type === 5) {
        return LxnBase.UI.AutoControls.ControlFactory._getDateRenderer(columnConfig);
    }
    if (columnConfig.Type === 2) {
        return LxnBase.UI.AutoControls.ControlFactory._getListRenderer(columnConfig);
    }
    if (!columnConfig.Type) {
        return LxnBase.UI.AutoControls.ControlFactory._getClassRenderer(columnConfig);
    }
    if (columnConfig.Type === 6) {
        var config = columnConfig;
        return LxnBase.UI.AutoControls.ControlFactory._customRenderers[config.TypeName];
    }
    return null;
}
LxnBase.UI.AutoControls.ControlFactory.createBooleanRenderer = function LxnBase_UI_AutoControls_ControlFactory$createBooleanRenderer() {
    /// <returns type="ss.Delegate"></returns>
    var booleanConfig = new LxnBase.Services.ColumnConfig();
    booleanConfig.Type = 3;
    return LxnBase.UI.AutoControls.ControlFactory.createRenderer(booleanConfig);
}
LxnBase.UI.AutoControls.ControlFactory.createEditor = function LxnBase_UI_AutoControls_ControlFactory$createEditor(columnConfig, isListMode) {
    /// <param name="columnConfig" type="LxnBase.Services.ColumnConfig">
    /// </param>
    /// <param name="isListMode" type="Boolean">
    /// </param>
    /// <returns type="Ext.form.Field"></returns>
    var field = null;
    switch (columnConfig.Type) {
        case 1:
            var cfg = columnConfig;
            var config = new Ext.form.NumberFieldConfig().width(87).allowDecimals(!cfg.IsInteger).decimalPrecision(2).allowBlank(!columnConfig.IsRequired).selectOnFocus(true);
            field = (cfg.IsInteger) ? new Ext.form.NumberField(config.toDictionary()) : new LxnBase.UI.Controls.DecimalField(config.toDictionary());
            break;
        case 3:
            if (ss.isNullOrUndefined(isListMode) || !isListMode) {
                field = new Ext.form.Checkbox(new Ext.form.CheckboxConfig().toDictionary());
            }
            else {
                var falseVal = [ false, LxnBase.BaseRes.filter_False ];
                var trueVal = [ true, LxnBase.BaseRes.filter_True ];
                field = new Ext.form.ComboBox(new Ext.form.ComboBoxConfig().store(new Ext.data.ArrayStore(new Ext.data.ArrayStoreConfig().fields([ 'Id', 'Name' ]).data([ falseVal, trueVal ]).toDictionary())).mode('local').width(50).displayField('Name').valueField('Id').editable(false).hideLabel(false).triggerAction('all').allowBlank(!columnConfig.IsRequired).pageSize(0).selectOnFocus(true).toDictionary());
            }
            break;
        case 4:
            var config = new Ext.form.TextFieldConfig().selectOnFocus(true).allowBlank(!columnConfig.IsRequired);
            var maxLength = (columnConfig).Length;
            if (maxLength > 0) {
                config.maxLength(maxLength);
                if (!(columnConfig).Lines) {
                    var width = maxLength * 6;
                    if (width < 70) {
                        width = 70;
                    }
                    else if (width > 230) {
                        width = 230;
                    }
                    config.width(width);
                }
            }
            else {
                config.width(180);
            }
            if (!!(columnConfig).Lines) {
                config.width(230);
                config.height(100);
                field = new Ext.form.TextArea(config.toDictionary());
            }
            else {
                field = new Ext.form.TextField(config.toDictionary());
            }
            break;
        case 5:
            var config = new Ext.form.DateFieldConfig().allowBlank(!columnConfig.IsRequired).selectOnFocus(true);
            if (String.isNullOrEmpty((columnConfig).FormatString)) {
                config.format('d.m.Y');
            }
            else {
                config.format((columnConfig).FormatString);
            }
            field = new Ext.form.DateField(config.toDictionary());
            break;
        case 0:
            field = LxnBase.UI.AutoControls.ControlFactory.getPersistentEditor(columnConfig).get_widget();
            break;
        case 2:
            field = new Ext.form.ComboBox(new Ext.form.ComboBoxConfig().store(new Ext.data.ArrayStore(new Ext.data.ArrayStoreConfig().fields([ 'Id', 'Text' ]).data((columnConfig).Items).toDictionary())).mode('local').displayField('Text').valueField('Id').editable(false).hideLabel(false).hideTrigger(false).triggerAction('all').allowBlank(!columnConfig.IsRequired).pageSize(0).width(180).selectOnFocus(true).toDictionary());
            break;
        case 6:
            var typeName = (columnConfig).TypeName;
            if (Object.keyExists(LxnBase.UI.AutoControls.ControlFactory._customEditors, typeName)) {
                var editor = LxnBase.UI.AutoControls.ControlFactory._customEditors[typeName];
                field = editor(columnConfig, isListMode);
            }
            break;
    }
    if (field == null) {
        var config = new Ext.form.TextFieldConfig().selectOnFocus(true);
        field = new Ext.form.TextField(config.toDictionary());
    }
    field.name = columnConfig.Name;
    field.fieldLabel = columnConfig.Caption;
    if (!ss.isNullOrUndefined(columnConfig.DefaultValue) && ss.isNullOrUndefined(field.value)) {
        field.setValue(columnConfig.DefaultValue);
    }
    return field;
}
LxnBase.UI.AutoControls.ControlFactory.createRefrenceRenderer = function LxnBase_UI_AutoControls_ControlFactory$createRefrenceRenderer(config, type) {
    /// <param name="config" type="LxnBase.Services.ColumnConfig">
    /// </param>
    /// <param name="type" type="String">
    /// </param>
    /// <returns type="Function"></returns>
    return function(value, metadata, record, rowIndex, colIndex, store) {
        if (value == null) {
            return null;
        }
        var values = new Array(3);
        values[LxnBase.Data.ObjectInfo.IdPos] = record.id;
        values[LxnBase.Data.ObjectInfo.TextPos] = value;
        var tp = record.data.__class;
        values[LxnBase.Data.ObjectInfo.TypePos] = (ss.isNullOrUndefined(tp)) ? type : tp;
        return LxnBase.UI.Controls.ObjectLink.renderArray(values);
    };
}
LxnBase.UI.AutoControls.ControlFactory.getPersistentEditor = function LxnBase_UI_AutoControls_ControlFactory$getPersistentEditor(cfg) {
    /// <param name="cfg" type="LxnBase.Services.ClassColumnConfig">
    /// </param>
    /// <returns type="LxnBase.UI.Controls.ObjectSelector"></returns>
    var width = 180;
    if (!!cfg.Length) {
        width = cfg.Length * 6;
        if (width < 70) {
            width = 70;
        }
        else if (width > 230) {
            width = 230;
        }
    }
    var config = new LxnBase.UI.Controls.ObjectSelectorConfig().setClass(cfg.Clazz).hideLabel(false).width(width).selectOnFocus(true).allowBlank(!cfg.IsRequired).name(cfg.Name).fieldLabel(cfg.Caption);
    return new LxnBase.UI.Controls.ObjectSelector(config);
}
LxnBase.UI.AutoControls.ControlFactory.registerCustomRenderer = function LxnBase_UI_AutoControls_ControlFactory$registerCustomRenderer(typeName, renderer) {
    /// <param name="typeName" type="String">
    /// </param>
    /// <param name="renderer" type="Function">
    /// </param>
    LxnBase.UI.AutoControls.ControlFactory._customRenderers[typeName] = renderer;
}
LxnBase.UI.AutoControls.ControlFactory.registerCustomEditor = function LxnBase_UI_AutoControls_ControlFactory$registerCustomEditor(typeName, editor) {
    /// <param name="typeName" type="String">
    /// </param>
    /// <param name="editor" type="Function">
    /// </param>
    LxnBase.UI.AutoControls.ControlFactory._customEditors[typeName] = editor;
}
LxnBase.UI.AutoControls.ControlFactory._boolRenderer = function LxnBase_UI_AutoControls_ControlFactory$_boolRenderer(value) {
    /// <param name="value" type="Object">
    /// </param>
    /// <returns type="Object"></returns>
    if (value) {
        return "<div class='checkBoxDisabled checked'></div>";
    }
    return '';
}
LxnBase.UI.AutoControls.ControlFactory._getDateRenderer = function LxnBase_UI_AutoControls_ControlFactory$_getDateRenderer(config) {
    /// <param name="config" type="LxnBase.Services.DateTimeColumnConfig">
    /// </param>
    /// <returns type="Function"></returns>
    return function(value) {
        return Ext.util.Format.date(value, config.FormatString);
    };
}
LxnBase.UI.AutoControls.ControlFactory._getClassRenderer = function LxnBase_UI_AutoControls_ControlFactory$_getClassRenderer(config) {
    /// <param name="config" type="LxnBase.Services.ClassColumnConfig">
    /// </param>
    /// <returns type="Function"></returns>
    return function(value) {
        if (value == null) {
            return null;
        }
        var values = Type.safeCast(value, Array);
        if (values != null) {
            if (config.RenderAsString) {
                return values[LxnBase.Data.ObjectInfo.TextPos];
            }
            if (ss.isNullOrUndefined(values[LxnBase.Data.ObjectInfo.TypePos])) {
                values[LxnBase.Data.ObjectInfo.TypePos] = config.Clazz;
            }
            return LxnBase.UI.Controls.ObjectLink.renderArray(values);
        }
        var info = value;
        if (config.RenderAsString) {
            return info.Text;
        }
        if (ss.isNullOrUndefined(info.Type)) {
            info.Type = config.Clazz;
        }
        return LxnBase.UI.Controls.ObjectLink.renderInfo(info);
    };
}
LxnBase.UI.AutoControls.ControlFactory._getListRenderer = function LxnBase_UI_AutoControls_ControlFactory$_getListRenderer(config) {
    /// <param name="config" type="LxnBase.Services.ListColumnConfig">
    /// </param>
    /// <returns type="Function"></returns>
    return function(value) {
        if (ss.isNullOrUndefined(value)) {
            return null;
        }
        if (Type.canCast(value, Array)) {
            return (value)[LxnBase.Data.ObjectInfo.TextPos];
        }
        for (var i = 0; i < config.Items.length; i++) {
            var values = config.Items[i];
            if (!ss.isNullOrUndefined(values[LxnBase.Data.ObjectInfo.IdPos]) && values[LxnBase.Data.ObjectInfo.IdPos] === value) {
                return values[LxnBase.Data.ObjectInfo.TextPos];
            }
        }
        return value;
    };
}
LxnBase.UI.AutoControls.ControlFactory._getNumberRenderer = function LxnBase_UI_AutoControls_ControlFactory$_getNumberRenderer(config) {
    /// <param name="config" type="LxnBase.Services.NumberColumnConfig">
    /// </param>
    /// <returns type="Function"></returns>
    return function(value) {
        if (ss.isNullOrUndefined(value)) {
            return '';
        }
        var str = (config.IsInteger) ? value.toString() : (value).format('N2');
        return String.format("<div style='text-align: right'>{0}</div>", str);
    };
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.AutoControls.GridFilterConfig

LxnBase.UI.AutoControls.GridFilterConfig = function LxnBase_UI_AutoControls_GridFilterConfig() {
    /// <field name="_caption" type="String">
    /// </field>
    /// <field name="_filter" type="LxnBase.UI.Controls.ColumnFilters.BaseFilter">
    /// </field>
    /// <field name="_internalPath" type="String">
    /// </field>
}
LxnBase.UI.AutoControls.GridFilterConfig.prototype = {
    
    setCaption: function LxnBase_UI_AutoControls_GridFilterConfig$setCaption(caption) {
        /// <param name="caption" type="String">
        /// </param>
        /// <returns type="LxnBase.UI.AutoControls.GridFilterConfig"></returns>
        this._caption = caption;
        return this;
    },
    
    setFilter: function LxnBase_UI_AutoControls_GridFilterConfig$setFilter(filter) {
        /// <param name="filter" type="LxnBase.UI.Controls.ColumnFilters.BaseFilter">
        /// </param>
        /// <returns type="LxnBase.UI.AutoControls.GridFilterConfig"></returns>
        this._filter = filter;
        return this;
    },
    
    setDataPath: function LxnBase_UI_AutoControls_GridFilterConfig$setDataPath(dataPath) {
        /// <param name="dataPath" type="String">
        /// </param>
        /// <returns type="LxnBase.UI.AutoControls.GridFilterConfig"></returns>
        this._internalPath = dataPath;
        return this;
    },
    
    get_caption: function LxnBase_UI_AutoControls_GridFilterConfig$get_caption() {
        /// <value type="String"></value>
        return this._caption;
    },
    set_caption: function LxnBase_UI_AutoControls_GridFilterConfig$set_caption(value) {
        /// <value type="String"></value>
        this._caption = value;
        return value;
    },
    
    get_filter: function LxnBase_UI_AutoControls_GridFilterConfig$get_filter() {
        /// <value type="LxnBase.UI.Controls.ColumnFilters.BaseFilter"></value>
        return this._filter;
    },
    set_filter: function LxnBase_UI_AutoControls_GridFilterConfig$set_filter(value) {
        /// <value type="LxnBase.UI.Controls.ColumnFilters.BaseFilter"></value>
        this._filter = value;
        return value;
    },
    
    get_internalPath: function LxnBase_UI_AutoControls_GridFilterConfig$get_internalPath() {
        /// <value type="String"></value>
        return this._internalPath;
    },
    set_internalPath: function LxnBase_UI_AutoControls_GridFilterConfig$set_internalPath(value) {
        /// <value type="String"></value>
        this._internalPath = value;
        return value;
    },
    
    _caption: null,
    _filter: null,
    _internalPath: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.AutoControls.GridFilterPlugin

LxnBase.UI.AutoControls.GridFilterPlugin = function LxnBase_UI_AutoControls_GridFilterPlugin(grid) {
    /// <param name="grid" type="LxnBase.UI.AutoControls.AutoGrid">
    /// </param>
    /// <field name="_grid" type="LxnBase.UI.AutoControls.AutoGrid">
    /// </field>
    /// <field name="_customFilters" type="Object" static="true">
    /// </field>
    /// <field name="_filterCheckItems" type="Object">
    /// </field>
    this._filterCheckItems = {};
    this._grid = grid;
}
LxnBase.UI.AutoControls.GridFilterPlugin.registerCustomFilters = function LxnBase_UI_AutoControls_GridFilterPlugin$registerCustomFilters(typeName, configs) {
    /// <param name="typeName" type="String">
    /// </param>
    /// <param name="configs" type="Array" elementType="GridFilterConfig">
    /// </param>
    LxnBase.UI.AutoControls.GridFilterPlugin._customFilters[typeName] = configs;
}
LxnBase.UI.AutoControls.GridFilterPlugin._getSimpleFilter = function LxnBase_UI_AutoControls_GridFilterPlugin$_getSimpleFilter(columnConfig) {
    /// <param name="columnConfig" type="LxnBase.Services.ColumnConfig">
    /// </param>
    /// <returns type="LxnBase.UI.Controls.ColumnFilters.BaseFilter"></returns>
    var type = columnConfig.Type;
    if (type === 3) {
        return new LxnBase.UI.Controls.ColumnFilters.BooleanFilter();
    }
    if (type === 4) {
        return new LxnBase.UI.Controls.ColumnFilters.StringFilter();
    }
    if (type === 1) {
        return new LxnBase.UI.Controls.ColumnFilters.NumberFilter();
    }
    if (type === 5) {
        return new LxnBase.UI.Controls.ColumnFilters.DateFilter();
    }
    if (!type) {
        var config = new LxnBase.Services.ColumnConfig();
        config.Type = (columnConfig).FilterType;
        return LxnBase.UI.AutoControls.GridFilterPlugin._getSimpleFilter(config);
    }
    if (type === 2) {
        return new LxnBase.UI.Controls.ColumnFilters.ListFilter();
    }
    return null;
}
LxnBase.UI.AutoControls.GridFilterPlugin._findPropertyFilter = function LxnBase_UI_AutoControls_GridFilterPlugin$_findPropertyFilter(filters, propertyName, internalPath) {
    /// <param name="filters" type="Array" elementType="PropertyFilter">
    /// </param>
    /// <param name="propertyName" type="String">
    /// </param>
    /// <param name="internalPath" type="String">
    /// </param>
    /// <returns type="LxnBase.Data.PropertyFilter"></returns>
    if (filters == null) {
        return null;
    }
    for (var i = 0; i < filters.length; i++) {
        if (filters[i].Property === propertyName && LxnBase.UI.AutoControls.GridFilterPlugin._isPathEquals(filters[i].InternalPath, internalPath)) {
            return filters[i];
        }
    }
    return null;
}
LxnBase.UI.AutoControls.GridFilterPlugin._deletePropertyFilter = function LxnBase_UI_AutoControls_GridFilterPlugin$_deletePropertyFilter(request, propertyName, internalPath) {
    /// <param name="request" type="LxnBase.Data.RangeRequest">
    /// </param>
    /// <param name="propertyName" type="String">
    /// </param>
    /// <param name="internalPath" type="String">
    /// </param>
    if (request.Filters == null) {
        return;
    }
    var filters = [];
    for (var i = 0; i < request.Filters.length; i++) {
        if (request.Filters[i].Property !== propertyName || !LxnBase.UI.AutoControls.GridFilterPlugin._isPathEquals(request.Filters[i].InternalPath, internalPath)) {
            filters[filters.length] = request.Filters[i];
        }
    }
    request.Filters = filters;
}
LxnBase.UI.AutoControls.GridFilterPlugin._isPathEquals = function LxnBase_UI_AutoControls_GridFilterPlugin$_isPathEquals(path1, path2) {
    /// <param name="path1" type="String">
    /// </param>
    /// <param name="path2" type="String">
    /// </param>
    /// <returns type="Boolean"></returns>
    if (String.isNullOrEmpty(path1) && String.isNullOrEmpty(path2)) {
        return true;
    }
    if (path1 === path2) {
        return true;
    }
    return false;
}
LxnBase.UI.AutoControls.GridFilterPlugin.prototype = {
    
    init: function LxnBase_UI_AutoControls_GridFilterPlugin$init(component) {
        /// <param name="component" type="Ext.Component">
        /// </param>
        component.on('render', ss.Delegate.create(this, this._onGridRender));
    },
    
    resetFilter: function LxnBase_UI_AutoControls_GridFilterPlugin$resetFilter() {
        var $dict1 = this._filterCheckItems;
        for (var $key2 in $dict1) {
            var entry = { key: $key2, value: $dict1[$key2] };
            var items = (entry.value);
            for (var i = 0; i < items.length; i++) {
                (items[i]).setChecked(false, true);
            }
        }
    },
    
    get__gridView: function LxnBase_UI_AutoControls_GridFilterPlugin$get__gridView() {
        /// <value type="LxnBase.UI.AutoControls.GridViewHack"></value>
        return this._grid.get_gridView();
    },
    
    _onGridRender: function LxnBase_UI_AutoControls_GridFilterPlugin$_onGridRender(component) {
        /// <param name="component" type="Ext.Component">
        /// </param>
        this._initFilterMenu();
        this.get__gridView().hmenu.on('beforeshow', ss.Delegate.create(this, function() {
            LxnBase.UI.EventsManager.registerKeyDownHandler(this.get__gridView().hmenu, false);
            this._showFilterMenu();
        }));
        this.get__gridView().hmenu.on('beforehide', LxnBase.UI.EventsManager.unregisterKeyDownHandler);
        this.get__gridView().cm.on('columnmoved', ss.Delegate.create(this, function() {
            this._refreshGridColumnHeaders();
        }));
        this._grid.store.on('load', ss.Delegate.create(this, function() {
            this.resetFilter();
            this._refreshGridColumnHeaders();
        }));
    },
    
    _initFilterMenu: function LxnBase_UI_AutoControls_GridFilterPlugin$_initFilterMenu() {
        this.get__gridView().hmenu.addSeparator();
        for (var i = 0; i < this._grid.get_listConfig().Columns.length; i++) {
            var columnConfig = this._grid.get_listConfig().Columns[i];
            var items = this._createMenuItems(columnConfig);
            for (var j = 0; j < items.length; j++) {
                this.get__gridView().hmenu.add(items[j]);
            }
            this._filterCheckItems[columnConfig.Name] = items;
        }
    },
    
    _createMenuItems: function LxnBase_UI_AutoControls_GridFilterPlugin$_createMenuItems(config) {
        /// <param name="config" type="LxnBase.Services.ColumnConfig">
        /// </param>
        /// <returns type="Array" elementType="Item"></returns>
        var type = config.Type;
        if (type !== 6) {
            var filter = LxnBase.UI.AutoControls.GridFilterPlugin._getSimpleFilter(config);
            if (filter == null) {
                return null;
            }
            filter.set_columnConfig(config);
            filter.add_changed(ss.Delegate.create(this, this._onFilterChanged));
            return [ new Ext.menu.CheckItem({ text: LxnBase.BaseRes.filter_Title, hideOnClick: false, menu: filter.getFilterMenu(), hidden: true, filter: filter, listeners: { checkchange: ss.Delegate.create(this, this._updateGridWithFilter) } }) ];
        }
        var cfg = config;
        if (!ss.isNullOrUndefined(cfg.TypeName)) {
            var items = [];
            var filterConfigs = LxnBase.UI.AutoControls.GridFilterPlugin._customFilters[cfg.TypeName];
            for (var i = 0; i < filterConfigs.length; i++) {
                var filter = filterConfigs[i].get_filter().create();
                filter.set_columnConfig(config);
                filter.set_internalPath(filterConfigs[i].get_internalPath());
                filter.add_changed(ss.Delegate.create(this, this._onFilterChanged));
                items.add(new Ext.menu.CheckItem({ text: filterConfigs[i].get_caption(), hideOnClick: false, menu: filter.getFilterMenu(), filter: filter, listeners: { checkchange: ss.Delegate.create(this, this._updateGridWithFilter) } }));
            }
            return items;
        }
        return null;
    },
    
    _refreshGridColumnHeaders: function LxnBase_UI_AutoControls_GridFilterPlugin$_refreshGridColumnHeaders() {
        var rangeRequest = this._grid.store.baseParams;
        for (var i = 0; i < this._grid.get_listConfig().Columns.length; i++) {
            var name = this._grid.get_listConfig().Columns[i].Name;
            var colIndex = this._grid.get_columnModel().findColumnIndex(name);
            if (colIndex >= 0) {
                var menuItems = this._filterCheckItems[name];
                var isFiltered = false;
                for (var j = 0; j < menuItems.length; j++) {
                    var filter = menuItems[j].filter;
                    if (LxnBase.UI.AutoControls.GridFilterPlugin._findPropertyFilter(rangeRequest.Filters, name, filter.get_internalPath()) != null) {
                        this._updateFilter(name, menuItems[j]);
                        isFiltered = true;
                        break;
                    }
                }
                this._setColumnFiltered(parseInt(colIndex), isFiltered);
            }
        }
    },
    
    _showFilterMenu: function LxnBase_UI_AutoControls_GridFilterPlugin$_showFilterMenu() {
        var config = this._grid.getColumnConfig(this.get__gridView().hdCtxIndex);
        var $dict1 = this._filterCheckItems;
        for (var $key2 in $dict1) {
            var entry = { key: $key2, value: $dict1[$key2] };
            for (var i = 0; i < (entry.value).length; i++) {
                (entry.value)[i].hide();
            }
        }
        if (ss.isNullOrUndefined(this._filterCheckItems[config.Name])) {
            return;
        }
        var menuItems = this._filterCheckItems[config.Name];
        for (var i = 0; i < menuItems.length; i++) {
            menuItems[i].show();
        }
    },
    
    _updateFilter: function LxnBase_UI_AutoControls_GridFilterPlugin$_updateFilter(name, menuItem) {
        /// <param name="name" type="String">
        /// </param>
        /// <param name="menuItem" type="Ext.menu.Item">
        /// </param>
        var filter = menuItem.filter;
        var propertyFilter = LxnBase.UI.AutoControls.GridFilterPlugin._findPropertyFilter((this._grid.store.baseParams).Filters, name, filter.get_internalPath());
        if (propertyFilter == null || propertyFilter.Conditions == null || !propertyFilter.Conditions.length) {
            return;
        }
        filter.set_conditions(propertyFilter.Conditions);
        (menuItem).setChecked(true, true);
    },
    
    _onFilterChanged: function LxnBase_UI_AutoControls_GridFilterPlugin$_onFilterChanged(sender, e) {
        /// <param name="sender" type="Object">
        /// </param>
        /// <param name="e" type="ss.EventArgs">
        /// </param>
        var filter = sender;
        var items = this._filterCheckItems[filter.get_columnConfig().Name];
        for (var i = 0; i < items.length; i++) {
            if (items[i].filter === filter) {
                (items[i]).setChecked(filter.get_conditions() != null, true);
                this._updateGridWithFilter(items[i], filter.get_conditions() != null);
                break;
            }
        }
    },
    
    _updateGridWithFilter: function LxnBase_UI_AutoControls_GridFilterPlugin$_updateGridWithFilter(menuItem, isChecked) {
        /// <param name="menuItem" type="Ext.menu.CheckItem">
        /// </param>
        /// <param name="isChecked" type="Boolean">
        /// </param>
        var filter = menuItem.filter;
        if (isChecked && filter.get_conditions() != null) {
            this._applyFilter(filter);
            this._setColumnFiltered(this.get__gridView().hdCtxIndex, true);
            this._grid.reload(true);
        }
        else {
            var rangeRequest = this._grid.store.baseParams;
            var propertyFilter = LxnBase.UI.AutoControls.GridFilterPlugin._findPropertyFilter(rangeRequest.Filters, filter.get_columnConfig().Name, filter.get_internalPath());
            if (propertyFilter != null) {
                LxnBase.UI.AutoControls.GridFilterPlugin._deletePropertyFilter(rangeRequest, filter.get_columnConfig().Name, filter.get_internalPath());
                this._setColumnFiltered(this.get__gridView().hdCtxIndex, false);
                this._grid.reload(true);
            }
        }
    },
    
    _applyFilter: function LxnBase_UI_AutoControls_GridFilterPlugin$_applyFilter(filter) {
        /// <param name="filter" type="LxnBase.UI.Controls.ColumnFilters.BaseFilter">
        /// </param>
        var rangeRequest = this._grid.store.baseParams;
        var filters = rangeRequest.Filters;
        var propertyFilter = LxnBase.UI.AutoControls.GridFilterPlugin._findPropertyFilter(filters, filter.get_columnConfig().Name, filter.get_internalPath());
        if (propertyFilter == null) {
            propertyFilter = new LxnBase.Data.PropertyFilter();
            propertyFilter.Property = filter.get_columnConfig().Name;
            propertyFilter.InternalPath = filter.get_internalPath();
            if (filters == null) {
                rangeRequest.Filters = [ propertyFilter ];
            }
            else {
                filters[filters.length] = propertyFilter;
            }
        }
        propertyFilter.Conditions = filter.get_conditions();
    },
    
    _setColumnFiltered: function LxnBase_UI_AutoControls_GridFilterPlugin$_setColumnFiltered(columnIndex, isFitered) {
        /// <param name="columnIndex" type="Number" integer="true">
        /// </param>
        /// <param name="isFitered" type="Boolean">
        /// </param>
        var filterClassName = 'filterCls';
        var cellHeaders = this.get__gridView().mainHd.select('td');
        var headerCell = cellHeaders.item(columnIndex);
        if (isFitered) {
            headerCell.addClass(filterClassName);
        }
        else {
            headerCell.removeClass(filterClassName);
        }
    },
    
    _grid: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.AutoControls.RecordMeta

LxnBase.UI.AutoControls.RecordMeta = function LxnBase_UI_AutoControls_RecordMeta() {
    /// <field name="__deleted" type="Boolean">
    /// </field>
    LxnBase.UI.AutoControls.RecordMeta.initializeBase(this);
}
LxnBase.UI.AutoControls.RecordMeta.prototype = {
    __deleted: false
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.AutoControls.ReportLoader

LxnBase.UI.AutoControls.ReportLoader = function LxnBase_UI_AutoControls_ReportLoader() {
    /// <field name="_instance" type="LxnBase.UI.AutoControls.IReportProvider" static="true">
    /// </field>
}
LxnBase.UI.AutoControls.ReportLoader.get_instance = function LxnBase_UI_AutoControls_ReportLoader$get_instance() {
    /// <value type="LxnBase.UI.AutoControls.IReportProvider"></value>
    return LxnBase.UI.AutoControls.ReportLoader._instance;
}
LxnBase.UI.AutoControls.ReportLoader.set_instance = function LxnBase_UI_AutoControls_ReportLoader$set_instance(value) {
    /// <value type="LxnBase.UI.AutoControls.IReportProvider"></value>
    LxnBase.UI.AutoControls.ReportLoader._instance = value;
    return value;
}
LxnBase.UI.AutoControls.ReportLoader.load = function LxnBase_UI_AutoControls_ReportLoader$load(url, parameters) {
    /// <param name="url" type="String">
    /// </param>
    /// <param name="parameters" type="Object">
    /// </param>
    LxnBase.UI.AutoControls.ReportLoader.get_instance().loadReport(url, parameters);
}


Type.registerNamespace('LxnBase.UI.AutoForms');

////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.AutoForms.AutoEditForm

LxnBase.UI.AutoForms.AutoEditForm = function LxnBase_UI_AutoForms_AutoEditForm(args, itemConfig) {
    /// <param name="args" type="LxnBase.UI.EditFormArgs">
    /// </param>
    /// <param name="itemConfig" type="LxnBase.Services.ItemConfig">
    /// </param>
    /// <field name="_instanceDictionary$2" type="Object">
    /// </field>
    LxnBase.UI.AutoForms.AutoEditForm.initializeBase(this, [ args, itemConfig ]);
}
LxnBase.UI.AutoForms.AutoEditForm.prototype = {
    
    loadInstance: function LxnBase_UI_AutoForms_AutoEditForm$loadInstance(onLoaded) {
        /// <param name="onLoaded" type="Function">
        /// </param>
        LxnBase.Services.GenericService.Get(this.get_args().type, this.get_args().idToLoad, false, onLoaded, null);
    },
    
    onLoadForm: function LxnBase_UI_AutoForms_AutoEditForm$onLoadForm() {
        this.get_window().setWidth(this._calculateWindowWidth$2(this.get_fields()));
    },
    
    onLoaded: function LxnBase_UI_AutoForms_AutoEditForm$onLoaded() {
        if (this.get_instance() != null) {
            this._instanceDictionary$2 = this.get_instance();
        }
    },
    
    addFields: function LxnBase_UI_AutoForms_AutoEditForm$addFields() {
        /// <returns type="Array" elementType="Field"></returns>
        var fields = new Array(this.get_instanceConfig().Columns.length);
        for (var i = 0; i < this.get_instanceConfig().Columns.length; i++) {
            fields[i] = LxnBase.UI.AutoControls.ControlFactory.createEditor(this.get_instanceConfig().Columns[i]);
            this.get_form().add(fields[i]);
        }
        return fields;
    },
    
    setFieldValues: function LxnBase_UI_AutoForms_AutoEditForm$setFieldValues() {
        var columns = this.get_instanceConfig().Columns;
        for (var i = 0; i < columns.length; i++) {
            var value = this.getInstancePropertyValue(columns[i].Name);
            if (!ss.isNullOrUndefined(value)) {
                this.get_fields()[i].setValue(value);
            }
        }
    },
    
    onSave: function LxnBase_UI_AutoForms_AutoEditForm$onSave() {
        var data = this.getFieldValues();
        var version = (this.get_instance() != null) ? this._instanceDictionary$2['Version'] : null;
        if (this.get_localMode()) {
            data['Id'] = this.get_args().get_id();
            this.completeSave(data);
        }
        else if (!!Object.getKeyCount(data)) {
            LxnBase.Services.GenericService.Update(this.get_args().type, this.get_args().get_id(), version, data, this.get_args().rangeRequest, ss.Delegate.create(this, this.completeSave), null);
        }
        else {
            this.cancel();
        }
    },
    
    onSaved: function LxnBase_UI_AutoForms_AutoEditForm$onSaved(result) {
        /// <param name="result" type="Object">
        /// </param>
        var response = result;
        if (!this.get_localMode() && ss.isNullOrUndefined(this.get_args().rangeRequest)) {
            var text = response.Item.__reference || this.get_instanceConfig().Caption;
            LxnBase.MessageRegister.info(this.get_instanceConfig().ListCaption, ((this.get_args().get_isNew()) ? LxnBase.BaseRes.created : LxnBase.BaseRes.updated) + ' ' + text);
        }
        LxnBase.UI.AutoForms.AutoEditForm.callBaseMethod(this, 'onSaved', [ result ]);
    },
    
    getFieldValues: function LxnBase_UI_AutoForms_AutoEditForm$getFieldValues() {
        /// <returns type="Object"></returns>
        var data = {};
        for (var i = 0; i < this.get_fields().length; i++) {
            this.getFieldValue(this.get_fields()[i], data);
        }
        return data;
    },
    
    getFieldValue: function LxnBase_UI_AutoForms_AutoEditForm$getFieldValue(field, values) {
        /// <param name="field" type="Ext.form.Field">
        /// </param>
        /// <param name="values" type="Object">
        /// </param>
        var value = field.getValue();
        if (!ss.isValue(value) || String.isNullOrEmpty(value.toString()) && value !== 0) {
            value = null;
        }
        if (this._instanceDictionary$2 == null || this.get_localMode() || field.isDirty() && this._instanceDictionary$2[field.name] !== value) {
            values[field.name] = value;
        }
    },
    
    _calculateWindowWidth$2: function LxnBase_UI_AutoForms_AutoEditForm$_calculateWindowWidth$2(formFields) {
        /// <param name="formFields" type="Array" elementType="Field">
        /// </param>
        /// <returns type="Number"></returns>
        var width = 0;
        for (var i = 0; i < formFields.length; i++) {
            if (!!formFields[i].width && width < formFields[i].width) {
                width = formFields[i].width;
            }
        }
        return this.get_form().labelWidth + width + 50;
    },
    
    _instanceDictionary$2: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.AutoForms.AutoFormCallbacks

LxnBase.UI.AutoForms.AutoFormCallbacks = function LxnBase_UI_AutoForms_AutoFormCallbacks() {
}
LxnBase.UI.AutoForms.AutoFormCallbacks.registerAsDefaults = function LxnBase_UI_AutoForms_AutoFormCallbacks$registerAsDefaults() {
    LxnBase.UI.FormsRegistry.registerDefaultList(LxnBase.UI.AutoForms.AutoFormCallbacks.listObjects);
    LxnBase.UI.FormsRegistry.registerDefaultView(LxnBase.UI.AutoForms.AutoFormCallbacks.viewObject);
    LxnBase.UI.FormsRegistry.registerDefaultEdit(LxnBase.UI.AutoForms.AutoFormCallbacks.editObject);
    LxnBase.UI.FormsRegistry.registerDefaultSelect(LxnBase.UI.AutoForms.AutoFormCallbacks.selectObjects);
}
LxnBase.UI.AutoForms.AutoFormCallbacks.listObjects = function LxnBase_UI_AutoForms_AutoFormCallbacks$listObjects(args, newTab) {
    /// <param name="args" type="LxnBase.UI.ListArgs">
    /// </param>
    /// <param name="newTab" type="Boolean">
    /// </param>
    LxnBase.UI.Tabs.open(newTab, args.type, function(tabId) {
        return new LxnBase.UI.AutoForms.AutoListTab(tabId, args);
    }, args.baseRequest);
}
LxnBase.UI.AutoForms.AutoFormCallbacks.viewObject = function LxnBase_UI_AutoForms_AutoFormCallbacks$viewObject(type, id, newTab) {
    /// <param name="type" type="String">
    /// </param>
    /// <param name="id" type="Object">
    /// </param>
    /// <param name="newTab" type="Boolean">
    /// </param>
    LxnBase.UI.Tabs.open(newTab, id, function(tabId) {
        return new LxnBase.UI.AutoForms.AutoViewForm(tabId, id, type);
    });
}
LxnBase.UI.AutoForms.AutoFormCallbacks.editObject = function LxnBase_UI_AutoForms_AutoFormCallbacks$editObject(args) {
    /// <param name="args" type="LxnBase.UI.EditFormArgs">
    /// </param>
    LxnBase.Data.ConfigManager.getEditConfig(args.type, function(config) {
        var form = new LxnBase.UI.AutoForms.AutoEditForm(args, config);
        form.open();
    });
}
LxnBase.UI.AutoForms.AutoFormCallbacks.selectObjects = function LxnBase_UI_AutoForms_AutoFormCallbacks$selectObjects(args) {
    /// <param name="args" type="LxnBase.UI.SelectArgs">
    /// </param>
    LxnBase.Data.ConfigManager.getListConfig(args.type, function(config) {
        var form = new LxnBase.UI.AutoForms.AutoSelectForm(args, config);
        form.open();
    });
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.AutoForms.AutoListTab

LxnBase.UI.AutoForms.AutoListTab = function LxnBase_UI_AutoForms_AutoListTab(tabId, args) {
    /// <param name="tabId" type="String">
    /// </param>
    /// <param name="args" type="LxnBase.UI.ListArgs">
    /// </param>
    /// <field name="_args$6" type="LxnBase.UI.ListArgs">
    /// </field>
    /// <field name="_autoGrid$6" type="LxnBase.UI.AutoControls.AutoGrid">
    /// </field>
    /// <field name="_updateOnActivation$6" type="Boolean">
    /// </field>
    LxnBase.UI.AutoForms.AutoListTab.initializeBase(this, [ new Ext.PanelConfig().closable(true).autoScroll(true).layout('fit').title(LxnBase.BaseRes.loading).toDictionary(), tabId ]);
    if (args == null) {
        throw new Error('Args cannot be null');
    }
    this._args$6 = args;
    LxnBase.Data.ConfigManager.getListConfig(this._args$6.type, ss.Delegate.create(this, this._load$6));
}
LxnBase.UI.AutoForms.AutoListTab.prototype = {
    
    get_listArgs: function LxnBase_UI_AutoForms_AutoListTab$get_listArgs() {
        /// <value type="LxnBase.UI.ListArgs"></value>
        return this._args$6;
    },
    
    get_autoGrid: function LxnBase_UI_AutoForms_AutoListTab$get_autoGrid() {
        /// <value type="LxnBase.UI.AutoControls.AutoGrid"></value>
        return this._autoGrid$6;
    },
    
    beforeActivate: function LxnBase_UI_AutoForms_AutoListTab$beforeActivate(pparams) {
        /// <param name="pparams" type="Object">
        /// </param>
        this._updateOnActivation$6 = ss.isNullOrUndefined(pparams);
        if (this._updateOnActivation$6) {
            return;
        }
        this._autoGrid$6.reloadGrid(pparams);
    },
    
    onActivate: function LxnBase_UI_AutoForms_AutoListTab$onActivate(isFirst) {
        /// <param name="isFirst" type="Boolean">
        /// </param>
        if (!isFirst && this._updateOnActivation$6) {
            this._autoGrid$6.refresh();
        }
    },
    
    handleKeyEvent: function LxnBase_UI_AutoForms_AutoListTab$handleKeyEvent(keyEvent) {
        /// <param name="keyEvent" type="jQueryEvent">
        /// </param>
        /// <returns type="Boolean"></returns>
        return this._autoGrid$6.handleKeyEvent(keyEvent);
    },
    
    _load$6: function LxnBase_UI_AutoForms_AutoListTab$_load$6(listConfig) {
        /// <param name="listConfig" type="LxnBase.Services.ListConfig">
        /// </param>
        this.setTitle((String.isNullOrEmpty(listConfig.Caption)) ? this._args$6.type : listConfig.Caption);
        this.add(this._initGrid$6(listConfig));
        this.onLoad();
        this.doLayout();
    },
    
    _initGrid$6: function LxnBase_UI_AutoForms_AutoListTab$_initGrid$6(listConfig) {
        /// <param name="listConfig" type="LxnBase.Services.ListConfig">
        /// </param>
        /// <returns type="Ext.Component"></returns>
        var args = new LxnBase.UI.AutoControls.AutoGridArgs();
        args.type = this._args$6.type;
        args.listConfig = listConfig;
        args.baseRequest = this._args$6.baseRequest;
        var config = new Ext.grid.EditorGridPanelConfig();
        this.onInitGrid(args, config);
        this._autoGrid$6 = new LxnBase.UI.AutoControls.AutoGrid(args, config);
        return this._autoGrid$6;
    },
    
    onInitGrid: function LxnBase_UI_AutoForms_AutoListTab$onInitGrid(config, editorGridPanelConfig) {
        /// <param name="config" type="LxnBase.UI.AutoControls.AutoGridArgs">
        /// </param>
        /// <param name="editorGridPanelConfig" type="Ext.grid.EditorGridPanelConfig">
        /// </param>
    },
    
    onLoad: function LxnBase_UI_AutoForms_AutoListTab$onLoad() {
    },
    
    _args$6: null,
    _autoGrid$6: null,
    _updateOnActivation$6: true
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.AutoForms.AutoSelectForm

LxnBase.UI.AutoForms.AutoSelectForm = function LxnBase_UI_AutoForms_AutoSelectForm(args, config) {
    /// <param name="args" type="LxnBase.UI.SelectArgs">
    /// </param>
    /// <param name="config" type="LxnBase.Services.ListConfig">
    /// </param>
    /// <field name="_args" type="LxnBase.UI.SelectArgs">
    /// </field>
    /// <field name="_listConfig" type="LxnBase.Services.ListConfig">
    /// </field>
    /// <field name="_window" type="Ext.Window">
    /// </field>
    /// <field name="_grid" type="LxnBase.UI.AutoControls.AutoGrid">
    /// </field>
    /// <field name="_width" type="Number" integer="true">
    /// </field>
    /// <field name="_height" type="Number" integer="true">
    /// </field>
    /// <field name="_isSelect" type="Boolean">
    /// </field>
    this._args = args;
    this._listConfig = config;
    this._listConfig.SingleSelect = this._args.singleSelect;
}
LxnBase.UI.AutoForms.AutoSelectForm.prototype = {
    
    get_width: function LxnBase_UI_AutoForms_AutoSelectForm$get_width() {
        /// <value type="Number" integer="true"></value>
        return this._width;
    },
    set_width: function LxnBase_UI_AutoForms_AutoSelectForm$set_width(value) {
        /// <value type="Number" integer="true"></value>
        this._width = value;
        return value;
    },
    
    get_height: function LxnBase_UI_AutoForms_AutoSelectForm$get_height() {
        /// <value type="Number" integer="true"></value>
        return this._height;
    },
    set_height: function LxnBase_UI_AutoForms_AutoSelectForm$set_height(value) {
        /// <value type="Number" integer="true"></value>
        this._height = value;
        return value;
    },
    
    get_listConfig: function LxnBase_UI_AutoForms_AutoSelectForm$get_listConfig() {
        /// <value type="LxnBase.Services.ListConfig"></value>
        return this._listConfig;
    },
    
    open: function LxnBase_UI_AutoForms_AutoSelectForm$open() {
        this._initGrid();
        this._window = new Ext.Window(new Ext.WindowConfig().title(this._listConfig.Caption || this._args.type).items(this._grid).layout('fit').width(this._width).height(this._height).listeners({ close: ss.Delegate.create(this, this._onWindowClose) }).modal(true).toDictionary());
        this._grid.add_onSelect(ss.Delegate.create(this, function(arg1) {
            if (this._args.onSelect != null) {
                this._args.onSelect(arg1);
            }
            this._isSelect = true;
            this._window.close();
        }));
        this._grid.add_onCancelSelect(ss.Delegate.create(this._window, this._window.close));
        LxnBase.UI.EventsManager.registerKeyDownHandler(this, false);
        this._window.show();
    },
    
    _initGrid: function LxnBase_UI_AutoForms_AutoSelectForm$_initGrid() {
        var args = new LxnBase.UI.AutoControls.AutoGridArgs();
        args.type = this._args.type;
        args.listConfig = this._listConfig;
        args.baseRequest = this._args.baseRequest;
        args.mode = 1;
        var config = new Ext.grid.EditorGridPanelConfig();
        this.onInitGrid(args, config);
        this._grid = new LxnBase.UI.AutoControls.AutoGrid(args, config);
    },
    
    onInitGrid: function LxnBase_UI_AutoForms_AutoSelectForm$onInitGrid(config, editorGridPanelConfig) {
        /// <param name="config" type="LxnBase.UI.AutoControls.AutoGridArgs">
        /// </param>
        /// <param name="editorGridPanelConfig" type="Ext.grid.EditorGridPanelConfig">
        /// </param>
    },
    
    _onWindowClose: function LxnBase_UI_AutoForms_AutoSelectForm$_onWindowClose() {
        if (!this._isSelect && this._args.onCancel != null) {
            this._args.onCancel();
        }
        LxnBase.UI.EventsManager.unregisterKeyDownHandler(this);
    },
    
    handleKeyEvent: function LxnBase_UI_AutoForms_AutoSelectForm$handleKeyEvent(keyEvent) {
        /// <param name="keyEvent" type="jQueryEvent">
        /// </param>
        /// <returns type="Boolean"></returns>
        return this._grid.handleKeyEvent(keyEvent);
    },
    
    restoreFocus: function LxnBase_UI_AutoForms_AutoSelectForm$restoreFocus() {
    },
    
    _args: null,
    _listConfig: null,
    _window: null,
    _grid: null,
    _width: 700,
    _height: 400,
    _isSelect: false
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.AutoForms.AutoViewForm

LxnBase.UI.AutoForms.AutoViewForm = function LxnBase_UI_AutoForms_AutoViewForm(tabId, id, type) {
    /// <param name="tabId" type="String">
    /// </param>
    /// <param name="id" type="Object">
    /// </param>
    /// <param name="type" type="String">
    /// </param>
    LxnBase.UI.AutoForms.AutoViewForm.initializeBase(this, [ tabId, id, type ]);
}
LxnBase.UI.AutoForms.AutoViewForm.prototype = {
    
    getInstance: function LxnBase_UI_AutoForms_AutoViewForm$getInstance() {
        LxnBase.Services.GenericService.Get(this.get_instanceType(), this.get_id(), true, ss.Delegate.create(this, this.load), ss.Delegate.create(this, this._onInstanceFailure$7));
    },
    
    onLoad: function LxnBase_UI_AutoForms_AutoViewForm$onLoad() {
        var columns = this.get_itemConfig().Columns;
        var caption = null;
        var template = new ss.StringBuilder();
        var $enum1 = ss.IEnumerator.getEnumerator(columns);
        while ($enum1.moveNext()) {
            var t = $enum1.current;
            var val = this.get_instance()[t.Name];
            if (t.IsReference) {
                if (Type.canCast(val, String)) {
                    caption = val;
                }
                else if (Type.canCast(val, Array)) {
                    caption = (val)[LxnBase.Data.ObjectInfo.TextPos];
                }
            }
            if (ss.isNullOrUndefined(val) || t.Hidden || t.Type === 3 && !val) {
                continue;
            }
            var renderer = LxnBase.UI.AutoControls.ControlFactory.createRenderer(t);
            var text = ((renderer == null) ? val : renderer(val));
            text = text.split('\n').join('<br/>');
            template.append("<div class='viewItem'>");
            template.append(String.format("<div class='itemCaption'>{0}</div>", t.Caption + ':'));
            template.append(String.format("<div class='itemValue'>{0}</div>", text));
            template.append('</div>');
        }
        if (caption == null) {
            caption = this.get_itemConfig().Caption;
        }
        this.setTitle(caption);
        new Ext.Template(template.toString()).overwrite(this.body, this.get_instance());
        this.doLayout();
    },
    
    _onInstanceFailure$7: function LxnBase_UI_AutoForms_AutoViewForm$_onInstanceFailure$7(args) {
        /// <param name="args" type="LxnBase.Net.WebServiceFailureArgs">
        /// </param>
        this.close();
    }
}


Type.registerNamespace('LxnBase.UI.Controls.ColumnFilters');

////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.Controls.ColumnFilters.BaseFilter

LxnBase.UI.Controls.ColumnFilters.BaseFilter = function LxnBase_UI_Controls_ColumnFilters_BaseFilter() {
    /// <field name="__changed" type="Function">
    /// </field>
    /// <field name="_filterMenuClass" type="String" static="true">
    /// </field>
    /// <field name="_columnConfig" type="LxnBase.Services.ColumnConfig">
    /// </field>
    /// <field name="_internalPath" type="String">
    /// </field>
}
LxnBase.UI.Controls.ColumnFilters.BaseFilter.prototype = {
    
    add_changed: function LxnBase_UI_Controls_ColumnFilters_BaseFilter$add_changed(value) {
        /// <param name="value" type="Function" />
        this.__changed = ss.Delegate.combine(this.__changed, value);
    },
    remove_changed: function LxnBase_UI_Controls_ColumnFilters_BaseFilter$remove_changed(value) {
        /// <param name="value" type="Function" />
        this.__changed = ss.Delegate.remove(this.__changed, value);
    },
    
    __changed: null,
    
    get_columnConfig: function LxnBase_UI_Controls_ColumnFilters_BaseFilter$get_columnConfig() {
        /// <value type="LxnBase.Services.ColumnConfig"></value>
        return this._columnConfig;
    },
    set_columnConfig: function LxnBase_UI_Controls_ColumnFilters_BaseFilter$set_columnConfig(value) {
        /// <value type="LxnBase.Services.ColumnConfig"></value>
        this._columnConfig = value;
        return value;
    },
    
    get_internalPath: function LxnBase_UI_Controls_ColumnFilters_BaseFilter$get_internalPath() {
        /// <value type="String"></value>
        return this._internalPath;
    },
    set_internalPath: function LxnBase_UI_Controls_ColumnFilters_BaseFilter$set_internalPath(value) {
        /// <value type="String"></value>
        this._internalPath = value;
        return value;
    },
    
    invokeChanged: function LxnBase_UI_Controls_ColumnFilters_BaseFilter$invokeChanged() {
        if (this.__changed != null) {
            this.__changed(this, ss.EventArgs.Empty);
        }
    },
    
    _columnConfig: null,
    _internalPath: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.Controls.ColumnFilters.BooleanFilter

LxnBase.UI.Controls.ColumnFilters.BooleanFilter = function LxnBase_UI_Controls_ColumnFilters_BooleanFilter() {
    /// <field name="_value$1" type="Boolean">
    /// </field>
    /// <field name="_items$1" type="Array" elementType="CheckItem">
    /// </field>
    /// <field name="_suppressEvent$1" type="Boolean">
    /// </field>
    LxnBase.UI.Controls.ColumnFilters.BooleanFilter.initializeBase(this);
}
LxnBase.UI.Controls.ColumnFilters.BooleanFilter.prototype = {
    
    create: function LxnBase_UI_Controls_ColumnFilters_BooleanFilter$create() {
        /// <returns type="LxnBase.UI.Controls.ColumnFilters.BaseFilter"></returns>
        return new LxnBase.UI.Controls.ColumnFilters.BooleanFilter();
    },
    
    get_conditions: function LxnBase_UI_Controls_ColumnFilters_BooleanFilter$get_conditions() {
        /// <value type="Array" elementType="PropertyFilterCondition"></value>
        var value = new LxnBase.Data.PropertyFilterCondition();
        value.Operator = 1;
        value.Value = this._value$1;
        return [ value ];
    },
    set_conditions: function LxnBase_UI_Controls_ColumnFilters_BooleanFilter$set_conditions(value) {
        /// <value type="Array" elementType="PropertyFilterCondition"></value>
        if (value != null && value[0].Value) {
            this._value$1 = true;
            this._suppressEvent$1 = true;
            this._items$1[0].setChecked(true, true);
        }
        else {
            this._value$1 = false;
            this._suppressEvent$1 = true;
            this._items$1[1].setChecked(true, true);
        }
        return value;
    },
    
    getFilterMenu: function LxnBase_UI_Controls_ColumnFilters_BooleanFilter$getFilterMenu() {
        /// <returns type="Ext.menu.Menu"></returns>
        var groupId = Ext.id();
        this._items$1 = [ new Ext.menu.CheckItem(new Ext.menu.CheckItemConfig().text(LxnBase.BaseRes.filter_True).group(groupId).checked_(this._value$1).listeners({ checkchange: ss.Delegate.create(this, function() {
            this._checkItemClicked$1(true);
        }) }).toDictionary()), new Ext.menu.CheckItem(new Ext.menu.CheckItemConfig().text(LxnBase.BaseRes.filter_False).group(groupId).checked_(!this._value$1).listeners({ checkchange: ss.Delegate.create(this, function() {
            this._checkItemClicked$1(false);
        }) }).toDictionary()) ];
        return new Ext.menu.Menu(new Ext.menu.MenuConfig().cls('filterMenu').items(this._items$1).toDictionary());
    },
    
    _checkItemClicked$1: function LxnBase_UI_Controls_ColumnFilters_BooleanFilter$_checkItemClicked$1(value) {
        /// <param name="value" type="Boolean">
        /// </param>
        if (this._suppressEvent$1) {
            this._suppressEvent$1 = false;
            return;
        }
        if (value === this._value$1) {
            return;
        }
        this._value$1 = value;
        this.invokeChanged();
    },
    
    _value$1: false,
    _items$1: null,
    _suppressEvent$1: false
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.Controls.ColumnFilters.DateFilter

LxnBase.UI.Controls.ColumnFilters.DateFilter = function LxnBase_UI_Controls_ColumnFilters_DateFilter() {
    /// <field name="_menu$1" type="Ext.menu.Menu">
    /// </field>
    /// <field name="_afterItem$1" type="Ext.menu.CheckItem">
    /// </field>
    /// <field name="_beforeItem$1" type="Ext.menu.CheckItem">
    /// </field>
    /// <field name="_onItem$1" type="Ext.menu.CheckItem">
    /// </field>
    /// <field name="_afterDateMenu$1" type="Ext.menu.DateMenu">
    /// </field>
    /// <field name="_beforeDateMenu$1" type="Ext.menu.DateMenu">
    /// </field>
    /// <field name="_onDateMenu$1" type="Ext.menu.DateMenu">
    /// </field>
    /// <field name="_after$1" type="Object">
    /// </field>
    /// <field name="_before$1" type="Object">
    /// </field>
    /// <field name="_on$1" type="Object">
    /// </field>
    LxnBase.UI.Controls.ColumnFilters.DateFilter.initializeBase(this);
}
LxnBase.UI.Controls.ColumnFilters.DateFilter._setDate$1 = function LxnBase_UI_Controls_ColumnFilters_DateFilter$_setDate$1(dateMenu, date) {
    /// <param name="dateMenu" type="Ext.menu.DateMenu">
    /// </param>
    /// <param name="date" type="Date">
    /// </param>
    if (!ss.isNull(date)) {
        dateMenu.picker.setValue(date);
    }
}
LxnBase.UI.Controls.ColumnFilters.DateFilter.prototype = {
    
    create: function LxnBase_UI_Controls_ColumnFilters_DateFilter$create() {
        /// <returns type="LxnBase.UI.Controls.ColumnFilters.BaseFilter"></returns>
        return new LxnBase.UI.Controls.ColumnFilters.DateFilter();
    },
    
    get_conditions: function LxnBase_UI_Controls_ColumnFilters_DateFilter$get_conditions() {
        /// <value type="Array" elementType="PropertyFilterCondition"></value>
        if (this._on$1 != null) {
            var onValue = new LxnBase.Data.PropertyFilterCondition();
            onValue.Value = this._on$1;
            onValue.Operator = 1;
            return [ onValue ];
        }
        if (this._after$1 == null && this._before$1 == null) {
            return null;
        }
        var result = [];
        if (this._before$1 != null) {
            var beforeValue = new LxnBase.Data.PropertyFilterCondition();
            beforeValue.Value = this._before$1;
            beforeValue.Operator = 7;
            result.add(beforeValue);
        }
        if (this._after$1 != null) {
            var afterValue = new LxnBase.Data.PropertyFilterCondition();
            afterValue.Value = this._after$1;
            afterValue.Operator = 8;
            result.add(afterValue);
        }
        return result;
    },
    set_conditions: function LxnBase_UI_Controls_ColumnFilters_DateFilter$set_conditions(value) {
        /// <value type="Array" elementType="PropertyFilterCondition"></value>
        if (value == null || !value.length) {
            return;
        }
        for (var i = 0; i < value.length; i++) {
            if (value[i].Operator === 8 || value[i].Operator === 9) {
                this._after$1 = value[i].Value;
                this._afterItem$1.setChecked(true, true);
                this._afterDateMenu$1.picker.setValue(this._after$1);
            }
            else if (value[i].Operator === 7 || value[i].Operator === 6) {
                this._before$1 = value[i].Value;
                this._beforeItem$1.setChecked(true, true);
                this._beforeDateMenu$1.picker.setValue(this._before$1);
            }
            else if (value[i].Operator === 1) {
                this._on$1 = value[i].Value;
                this._onItem$1.setChecked(true, true);
                this._onDateMenu$1.picker.setValue(this._on$1);
            }
            else {
                throw new Error('Argument error');
            }
        }
        return value;
    },
    
    getFilterMenu: function LxnBase_UI_Controls_ColumnFilters_DateFilter$getFilterMenu() {
        /// <returns type="Ext.menu.Menu"></returns>
        this._beforeDateMenu$1 = new Ext.menu.DateMenu();
        this._beforeDateMenu$1.on('select', ss.Delegate.create(this, this._beforeDateSelect$1));
        LxnBase.UI.Controls.ColumnFilters.DateFilter._setDate$1(this._beforeDateMenu$1, this._before$1);
        this._beforeItem$1 = new Ext.menu.CheckItem({ text: LxnBase.BaseRes.filter_BeforeDate, hideOnClick: false, checked: this._before$1 != null, menu: this._beforeDateMenu$1 });
        this._beforeItem$1.on('checkchange', ss.Delegate.create(this, this._beforeItemChecked$1));
        this._afterDateMenu$1 = new Ext.menu.DateMenu();
        this._afterDateMenu$1.on('select', ss.Delegate.create(this, this._afterDateSelect$1));
        LxnBase.UI.Controls.ColumnFilters.DateFilter._setDate$1(this._afterDateMenu$1, this._after$1);
        this._afterItem$1 = new Ext.menu.CheckItem({ text: LxnBase.BaseRes.filter_AfterDate, hideOnClick: false, checked: this._after$1 != null, menu: this._afterDateMenu$1 });
        this._afterItem$1.on('checkchange', ss.Delegate.create(this, this._afterItemChecked$1));
        this._onDateMenu$1 = new Ext.menu.DateMenu();
        this._onDateMenu$1.on('select', ss.Delegate.create(this, this._onDateSelect$1));
        LxnBase.UI.Controls.ColumnFilters.DateFilter._setDate$1(this._onDateMenu$1, this._on$1);
        this._onItem$1 = new Ext.menu.CheckItem({ text: LxnBase.BaseRes.filter_OnDate, hideOnClick: false, checked: this._on$1 != null, menu: this._onDateMenu$1 });
        this._onItem$1.on('checkchange', ss.Delegate.create(this, this._onItemChecked$1));
        this._menu$1 = new Ext.menu.Menu({ items: [ this._afterItem$1, this._beforeItem$1, '-', this._onItem$1 ] });
        return this._menu$1;
    },
    
    _beforeDateSelect$1: function LxnBase_UI_Controls_ColumnFilters_DateFilter$_beforeDateSelect$1(picker, date) {
        /// <param name="picker" type="Ext.DatePicker">
        /// </param>
        /// <param name="date" type="Date">
        /// </param>
        this._before$1 = date;
        this._beforeItem$1.setChecked(true, true);
        this._clearOnDate$1();
        this.invokeChanged();
    },
    
    _afterDateSelect$1: function LxnBase_UI_Controls_ColumnFilters_DateFilter$_afterDateSelect$1(picker, date) {
        /// <param name="picker" type="Ext.DatePicker">
        /// </param>
        /// <param name="date" type="Date">
        /// </param>
        this._after$1 = date;
        this._afterItem$1.setChecked(true, true);
        this._clearOnDate$1();
        this.invokeChanged();
    },
    
    _onDateSelect$1: function LxnBase_UI_Controls_ColumnFilters_DateFilter$_onDateSelect$1(picker, date) {
        /// <param name="picker" type="Ext.DatePicker">
        /// </param>
        /// <param name="date" type="Date">
        /// </param>
        this._on$1 = date;
        this._onItem$1.setChecked(true, true);
        this._after$1 = null;
        this._afterItem$1.setChecked(false, true);
        this._before$1 = null;
        this._beforeItem$1.setChecked(false, true);
        this.invokeChanged();
    },
    
    _beforeItemChecked$1: function LxnBase_UI_Controls_ColumnFilters_DateFilter$_beforeItemChecked$1(checkItem, chckd) {
        /// <param name="checkItem" type="Ext.menu.CheckItem">
        /// </param>
        /// <param name="chckd" type="Boolean">
        /// </param>
        if (chckd) {
            checkItem.setChecked(false, true);
        }
        else {
            this._beforeItem$1 = null;
            this.invokeChanged();
        }
    },
    
    _afterItemChecked$1: function LxnBase_UI_Controls_ColumnFilters_DateFilter$_afterItemChecked$1(checkItem, chckd) {
        /// <param name="checkItem" type="Ext.menu.CheckItem">
        /// </param>
        /// <param name="chckd" type="Boolean">
        /// </param>
        if (chckd) {
            checkItem.setChecked(false, true);
        }
        else {
            this._after$1 = null;
            this.invokeChanged();
        }
    },
    
    _onItemChecked$1: function LxnBase_UI_Controls_ColumnFilters_DateFilter$_onItemChecked$1(checkItem, chckd) {
        /// <param name="checkItem" type="Ext.menu.CheckItem">
        /// </param>
        /// <param name="chckd" type="Boolean">
        /// </param>
        if (chckd) {
            checkItem.setChecked(false, true);
        }
        else {
            this._on$1 = null;
            this.invokeChanged();
        }
    },
    
    _clearOnDate$1: function LxnBase_UI_Controls_ColumnFilters_DateFilter$_clearOnDate$1() {
        this._on$1 = null;
        this._onItem$1.setChecked(false, true);
    },
    
    _menu$1: null,
    _afterItem$1: null,
    _beforeItem$1: null,
    _onItem$1: null,
    _afterDateMenu$1: null,
    _beforeDateMenu$1: null,
    _onDateMenu$1: null,
    _after$1: null,
    _before$1: null,
    _on$1: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.Controls.ColumnFilters.EditorFilter

LxnBase.UI.Controls.ColumnFilters.EditorFilter = function LxnBase_UI_Controls_ColumnFilters_EditorFilter() {
    /// <field name="_editorClassName" type="String" static="true">
    /// </field>
    /// <field name="_defaultOperator" type="LxnBase.Data.FilterOperator" static="true">
    /// </field>
    /// <field name="_not$1" type="Boolean">
    /// </field>
    /// <field name="_operator$1" type="LxnBase.Data.FilterOperator">
    /// </field>
    /// <field name="_value$1" type="Object">
    /// </field>
    LxnBase.UI.Controls.ColumnFilters.EditorFilter.initializeBase(this);
    this._operator$1 = LxnBase.UI.Controls.ColumnFilters.EditorFilter._defaultOperator;
}
LxnBase.UI.Controls.ColumnFilters.EditorFilter.prototype = {
    
    get_not: function LxnBase_UI_Controls_ColumnFilters_EditorFilter$get_not() {
        /// <value type="Boolean"></value>
        return this._not$1;
    },
    set_not: function LxnBase_UI_Controls_ColumnFilters_EditorFilter$set_not(value) {
        /// <value type="Boolean"></value>
        this._not$1 = value;
        return value;
    },
    
    get_operator: function LxnBase_UI_Controls_ColumnFilters_EditorFilter$get_operator() {
        /// <value type="LxnBase.Data.FilterOperator"></value>
        return this._operator$1;
    },
    set_operator: function LxnBase_UI_Controls_ColumnFilters_EditorFilter$set_operator(value) {
        /// <value type="LxnBase.Data.FilterOperator"></value>
        this._operator$1 = value;
        return value;
    },
    
    get_value: function LxnBase_UI_Controls_ColumnFilters_EditorFilter$get_value() {
        /// <value type="Object"></value>
        return this._value$1;
    },
    set_value: function LxnBase_UI_Controls_ColumnFilters_EditorFilter$set_value(value) {
        /// <value type="Object"></value>
        this._value$1 = value;
        return value;
    },
    
    get_conditions: function LxnBase_UI_Controls_ColumnFilters_EditorFilter$get_conditions() {
        /// <value type="Array" elementType="PropertyFilterCondition"></value>
        if (this.get_operator() !== 2 && this._value$1 == null) {
            return null;
        }
        var filterValue = new LxnBase.Data.PropertyFilterCondition();
        filterValue.Not = this._not$1;
        filterValue.Operator = this.get_operator();
        filterValue.Value = this._value$1;
        return [ filterValue ];
    },
    set_conditions: function LxnBase_UI_Controls_ColumnFilters_EditorFilter$set_conditions(value) {
        /// <value type="Array" elementType="PropertyFilterCondition"></value>
        if (value == null || !value.length) {
            this._not$1 = false;
            this._value$1 = null;
            this.set_operator(LxnBase.UI.Controls.ColumnFilters.EditorFilter._defaultOperator);
        }
        else {
            this._not$1 = value[0].Not;
            this._value$1 = value[0].Value;
            this.set_operator(value[0].Operator);
            this.get_notItem().setChecked(this._not$1, true);
            for (var i = 0; i < this.get_checkItems().length; i++) {
                if (this.get_checkItems()[i].initialConfig.value === this.get_operator()) {
                    this.get_checkItems()[i].setChecked(true, true);
                    break;
                }
            }
            this.get_editor().setValue(this.get_value());
        }
        return value;
    },
    
    createCheckItem: function LxnBase_UI_Controls_ColumnFilters_EditorFilter$createCheckItem(groupId, filterOperator, listeners) {
        /// <param name="groupId" type="String">
        /// </param>
        /// <param name="filterOperator" type="LxnBase.Data.FilterOperator">
        /// </param>
        /// <param name="listeners" type="Object">
        /// </param>
        /// <returns type="Ext.menu.CheckItem"></returns>
        var item = new Ext.menu.CheckItem({ text: LxnBase.EnumUtility.localize(LxnBase.Data.FilterOperator, filterOperator, LxnBase.BaseRes), value: filterOperator, group: groupId, checked: this.get_operator() === filterOperator, hideOnClick: false, listeners: listeners });
        return item;
    },
    
    checkItemClicked: function LxnBase_UI_Controls_ColumnFilters_EditorFilter$checkItemClicked(checkItem, cchecked) {
        /// <param name="checkItem" type="Ext.menu.CheckItem">
        /// </param>
        /// <param name="cchecked" type="Boolean">
        /// </param>
        if (!cchecked) {
            return;
        }
        this.set_operator(checkItem.initialConfig.value);
        this.checkEditor();
        this.invokeChanged();
    },
    
    checkEditor: function LxnBase_UI_Controls_ColumnFilters_EditorFilter$checkEditor() {
        if (this.get_operator() === 2) {
            this.get_editor().disable();
        }
        else {
            this.get_editor().enable();
        }
    },
    
    _not$1: false,
    _operator$1: 0,
    _value$1: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.Controls.ColumnFilters.ListFilter

LxnBase.UI.Controls.ColumnFilters.ListFilter = function LxnBase_UI_Controls_ColumnFilters_ListFilter() {
    /// <field name="_not$1" type="Boolean">
    /// </field>
    /// <field name="_value$1" type="Array">
    /// </field>
    /// <field name="_notItem$1" type="Ext.menu.CheckItem">
    /// </field>
    /// <field name="_checkItems$1" type="Array">
    /// </field>
    this._value$1 = [];
    LxnBase.UI.Controls.ColumnFilters.ListFilter.initializeBase(this);
}
LxnBase.UI.Controls.ColumnFilters.ListFilter.prototype = {
    
    create: function LxnBase_UI_Controls_ColumnFilters_ListFilter$create() {
        /// <returns type="LxnBase.UI.Controls.ColumnFilters.BaseFilter"></returns>
        return new LxnBase.UI.Controls.ColumnFilters.ListFilter();
    },
    
    get_conditions: function LxnBase_UI_Controls_ColumnFilters_ListFilter$get_conditions() {
        /// <value type="Array" elementType="PropertyFilterCondition"></value>
        if (this._value$1 == null || !this._value$1.length) {
            return null;
        }
        var filterValue = new LxnBase.Data.PropertyFilterCondition();
        var valueList = [];
        var tempList = this._value$1;
        for (var i = 0; i < tempList.length; i++) {
            valueList[i] = tempList[i];
        }
        filterValue.Not = this._not$1;
        filterValue.Value = valueList;
        filterValue.Operator = 10;
        return [ filterValue ];
    },
    set_conditions: function LxnBase_UI_Controls_ColumnFilters_ListFilter$set_conditions(value) {
        /// <value type="Array" elementType="PropertyFilterCondition"></value>
        if (value == null || !value.length) {
            this._not$1 = false;
            this._value$1 = [];
        }
        else {
            this._not$1 = value[0].Not;
            this._value$1 = [];
            var tempList = value[0].Value;
            for (var i = 0; i < tempList.length; i++) {
                this._value$1[i] = tempList[i];
            }
            this._notItem$1.setChecked(this._not$1, true);
            for (var i = 0; i < this._checkItems$1.length; i++) {
                var checkItem = this._checkItems$1[i];
                for (var j = 0; j < this._value$1.length; j++) {
                    if (checkItem.initialConfig.checkItemId === this._value$1[j]) {
                        checkItem.setChecked(true, true);
                        break;
                    }
                }
            }
        }
        return value;
    },
    
    getFilterMenu: function LxnBase_UI_Controls_ColumnFilters_ListFilter$getFilterMenu() {
        /// <returns type="Ext.menu.Menu"></returns>
        this._notItem$1 = new Ext.menu.CheckItem(new Ext.menu.CheckItemConfig().text(LxnBase.BaseRes.propertyFilterCondition_Not).checked_(this._not$1).listeners({ checkchange: ss.Delegate.create(this, function(sender, cchecked) {
            this._not$1 = cchecked;
            this.invokeChanged();
        }) }).hideOnClick(false).toDictionary());
        this._checkItems$1 = [];
        var config = this.get_columnConfig();
        for (var i = 0; i < config.Items.length; i++) {
            var item = config.Items[i];
            this._checkItems$1.add(new Ext.menu.CheckItem(new Ext.menu.CheckItemConfig().text(item[LxnBase.Data.ObjectInfo.TextPos]).checked_(this._isChecked$1(item[LxnBase.Data.ObjectInfo.IdPos])).listeners({ checkchange: ss.Delegate.create(this, this._checkChanged$1) }).hideOnClick(false).custom('checkItemId', item[LxnBase.Data.ObjectInfo.IdPos]).toDictionary()));
        }
        var items = [];
        items.add(this._notItem$1);
        items.add(new Ext.menu.Separator());
        items.addRange(this._checkItems$1);
        return new Ext.menu.Menu({ items: items });
    },
    
    _checkChanged$1: function LxnBase_UI_Controls_ColumnFilters_ListFilter$_checkChanged$1(checkItem, isChecked) {
        /// <param name="checkItem" type="Ext.menu.CheckItem">
        /// </param>
        /// <param name="isChecked" type="Boolean">
        /// </param>
        var id = checkItem.initialConfig.checkItemId;
        if (!isChecked) {
            for (var i = 0; i < this._value$1.length; i++) {
                if (this._value$1[i] === id) {
                    this._value$1.removeAt(i);
                    break;
                }
            }
        }
        else {
            this._value$1.add(id);
        }
        this.invokeChanged();
    },
    
    _isChecked$1: function LxnBase_UI_Controls_ColumnFilters_ListFilter$_isChecked$1(id) {
        /// <param name="id" type="Object">
        /// </param>
        /// <returns type="Boolean"></returns>
        for (var i = 0; i < this._value$1.length; i++) {
            if (this._value$1[i] === id) {
                return true;
            }
        }
        return false;
    },
    
    _not$1: false,
    _notItem$1: null,
    _checkItems$1: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.Controls.ColumnFilters.NumberFilter

LxnBase.UI.Controls.ColumnFilters.NumberFilter = function LxnBase_UI_Controls_ColumnFilters_NumberFilter() {
    /// <field name="_menu$2" type="Ext.menu.Menu">
    /// </field>
    /// <field name="_editor$2" type="Ext.form.NumberField">
    /// </field>
    /// <field name="_checkItems$2" type="Array">
    /// </field>
    /// <field name="_notItem$2" type="Ext.menu.CheckItem">
    /// </field>
    this._checkItems$2 = [];
    LxnBase.UI.Controls.ColumnFilters.NumberFilter.initializeBase(this);
}
LxnBase.UI.Controls.ColumnFilters.NumberFilter.prototype = {
    
    get_editor: function LxnBase_UI_Controls_ColumnFilters_NumberFilter$get_editor() {
        /// <value type="Ext.form.Field"></value>
        return this._editor$2;
    },
    
    get_checkItems: function LxnBase_UI_Controls_ColumnFilters_NumberFilter$get_checkItems() {
        /// <value type="Array" elementType="CheckItem"></value>
        return this._checkItems$2;
    },
    
    get_notItem: function LxnBase_UI_Controls_ColumnFilters_NumberFilter$get_notItem() {
        /// <value type="Ext.menu.CheckItem"></value>
        return this._notItem$2;
    },
    
    create: function LxnBase_UI_Controls_ColumnFilters_NumberFilter$create() {
        /// <returns type="LxnBase.UI.Controls.ColumnFilters.BaseFilter"></returns>
        return new LxnBase.UI.Controls.ColumnFilters.NumberFilter();
    },
    
    getFilterMenu: function LxnBase_UI_Controls_ColumnFilters_NumberFilter$getFilterMenu() {
        /// <returns type="Ext.menu.Menu"></returns>
        var groupId = Ext.id();
        var listeners = { checkchange: ss.Delegate.create(this, this.checkItemClicked) };
        this._initEditor$2();
        this._notItem$2 = new Ext.menu.CheckItem({ text: LxnBase.BaseRes.propertyFilterCondition_Not, checked: this.get_not(), hideOnClick: false, listeners: { checkchange: ss.Delegate.create(this, function(sender, cchecked) {
            this.set_not(cchecked);
            this.invokeChanged();
        }) } });
        this._checkItems$2 = [];
        this._checkItems$2.add(this.createCheckItem(groupId, 1, listeners));
        this._checkItems$2.add(this.createCheckItem(groupId, 9, listeners));
        this._checkItems$2.add(this.createCheckItem(groupId, 6, listeners));
        this._checkItems$2.add(this.createCheckItem(groupId, 8, listeners));
        this._checkItems$2.add(this.createCheckItem(groupId, 7, listeners));
        this._checkItems$2.add(this.createCheckItem(groupId, 2, listeners));
        var items = [];
        items.add(this._notItem$2);
        items.add(new Ext.menu.Separator());
        items.addRange(this._checkItems$2);
        items.add(new Ext.menu.Separator());
        items.add(new LxnBase.UI.Controls.ColumnFilters.FilterMenuItem({ editor: this._editor$2 }));
        this._menu$2 = new Ext.menu.Menu({ cls: 'filterMenu', items: items });
        return this._menu$2;
    },
    
    _initEditor$2: function LxnBase_UI_Controls_ColumnFilters_NumberFilter$_initEditor$2() {
        this._editor$2 = new Ext.form.NumberField({ cls: 'filterEditor', enableKeyEvents: true });
        this._editor$2.setValue(this.get_value());
        this._editor$2.on('keypress', ss.Delegate.create(this, function(objthis, e) {
            var value = this._editor$2.getValue();
            var key = e.getKey();
            if (key === Ext.EventObject.ENTER && value !== this.get_value()) {
                this.set_value(value);
                this._menu$2.hide(true);
                this.invokeChanged();
            }
            else if (key === Ext.EventObject.ESC) {
                this._editor$2.setValue(this.get_value());
                this._menu$2.hide(true);
            }
        }));
        this._editor$2.on('change', ss.Delegate.create(this, function() {
            var value = this._editor$2.getValue();
            if (this.get_value() !== value) {
                this.set_value(value);
                this.invokeChanged();
            }
        }));
        this.checkEditor();
    },
    
    _menu$2: null,
    _editor$2: null,
    _notItem$2: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.Controls.ColumnFilters.StringFilter

LxnBase.UI.Controls.ColumnFilters.StringFilter = function LxnBase_UI_Controls_ColumnFilters_StringFilter() {
    /// <field name="_menu$2" type="Ext.menu.Menu">
    /// </field>
    /// <field name="_editor$2" type="Ext.form.TextField">
    /// </field>
    /// <field name="_checkItems$2" type="Array">
    /// </field>
    /// <field name="_notItem$2" type="Ext.menu.CheckItem">
    /// </field>
    this._checkItems$2 = [];
    LxnBase.UI.Controls.ColumnFilters.StringFilter.initializeBase(this);
}
LxnBase.UI.Controls.ColumnFilters.StringFilter.prototype = {
    
    get_editor: function LxnBase_UI_Controls_ColumnFilters_StringFilter$get_editor() {
        /// <value type="Ext.form.Field"></value>
        return this._editor$2;
    },
    
    get_checkItems: function LxnBase_UI_Controls_ColumnFilters_StringFilter$get_checkItems() {
        /// <value type="Array" elementType="CheckItem"></value>
        return this._checkItems$2;
    },
    
    get_notItem: function LxnBase_UI_Controls_ColumnFilters_StringFilter$get_notItem() {
        /// <value type="Ext.menu.CheckItem"></value>
        return this._notItem$2;
    },
    
    create: function LxnBase_UI_Controls_ColumnFilters_StringFilter$create() {
        /// <returns type="LxnBase.UI.Controls.ColumnFilters.BaseFilter"></returns>
        return new LxnBase.UI.Controls.ColumnFilters.StringFilter();
    },
    
    getFilterMenu: function LxnBase_UI_Controls_ColumnFilters_StringFilter$getFilterMenu() {
        /// <returns type="Ext.menu.Menu"></returns>
        var groupId = Ext.id();
        var listeners = { checkchange: ss.Delegate.create(this, this.checkItemClicked) };
        this._initEditor$2();
        this._notItem$2 = new Ext.menu.CheckItem({ text: LxnBase.BaseRes.propertyFilterCondition_Not, checked: this.get_not(), hideOnClick: false, listeners: { checkchange: ss.Delegate.create(this, function(sender, cchecked) {
            this.set_not(cchecked);
            this.invokeChanged();
        }) } });
        this._checkItems$2 = [];
        this._checkItems$2.add(this.createCheckItem(groupId, 1, listeners));
        var item = this.createCheckItem(groupId, 3, listeners);
        item.setChecked(true, true);
        this.set_operator(3);
        this._checkItems$2.add(item);
        this._checkItems$2.add(this.createCheckItem(groupId, 5, listeners));
        this._checkItems$2.add(this.createCheckItem(groupId, 4, listeners));
        this._checkItems$2.add(this.createCheckItem(groupId, 2, listeners));
        var items = [];
        items.add(this._notItem$2);
        items.add(new Ext.menu.Separator());
        items.addRange(this._checkItems$2);
        items.add(new Ext.menu.Separator());
        items.add(new LxnBase.UI.Controls.ColumnFilters.FilterMenuItem({ editor: this._editor$2 }));
        this._menu$2 = new Ext.menu.Menu({ cls: 'filterMenu', items: items });
        return this._menu$2;
    },
    
    _initEditor$2: function LxnBase_UI_Controls_ColumnFilters_StringFilter$_initEditor$2() {
        this._editor$2 = new Ext.form.TextField({ cls: 'filterEditor', enableKeyEvents: true });
        this._editor$2.setValue(this.get_value());
        this._editor$2.on('keypress', ss.Delegate.create(this, function(objthis, e) {
            var value = this._editor$2.getValue();
            var key = e.getKey();
            if (key === Ext.EventObject.ENTER && value !== this.get_value()) {
                this.set_value(value);
                this._menu$2.hide(true);
                this.invokeChanged();
            }
            else if (key === Ext.EventObject.ESC) {
                this._editor$2.setValue(this.get_value());
                this._menu$2.hide(true);
            }
        }));
        this._editor$2.on('change', ss.Delegate.create(this, function() {
            var value = this._editor$2.getValue();
            if (this.get_value() !== value) {
                this.set_value(value);
                this.invokeChanged();
            }
        }));
        this.checkEditor();
    },
    
    _menu$2: null,
    _editor$2: null,
    _notItem$2: null
}


Type.registerNamespace('LxnBase.UI.Controls');

////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.Controls.ComboBox

LxnBase.UI.Controls.ComboBox = function LxnBase_UI_Controls_ComboBox(config) {
    /// <param name="config" type="Object">
    /// </param>
    /// <field name="_values$7" type="Array">
    /// </field>
    /// <field name="_backgroundLoading$7" type="Boolean">
    /// </field>
    LxnBase.UI.Controls.ComboBox.initializeBase(this, [ config ]);
}
LxnBase.UI.Controls.ComboBox._normalizeValue$7 = function LxnBase_UI_Controls_ComboBox$_normalizeValue$7(val) {
    /// <param name="val" type="Object">
    /// </param>
    /// <returns type="Array"></returns>
    if (val != null && !(Type.canCast(val, Array))) {
        var values = new Array(3);
        values[LxnBase.Data.ObjectInfo.IdPos] = (val).Id;
        values[LxnBase.Data.ObjectInfo.TextPos] = (val).Text;
        values[LxnBase.Data.ObjectInfo.TypePos] = (val).Type;
        return values;
    }
    return val;
}
LxnBase.UI.Controls.ComboBox.prototype = {
    
    get_backgroundLoading: function LxnBase_UI_Controls_ComboBox$get_backgroundLoading() {
        /// <value type="Boolean"></value>
        return this._backgroundLoading$7;
    },
    
    setValue: function LxnBase_UI_Controls_ComboBox$setValue(val) {
        /// <param name="val" type="Object">
        /// </param>
        var oldValues = this._values$7;
        if (ss.isNullOrUndefined(val)) {
            LxnBase.UI.Controls.ComboBox.callBaseMethod(this, 'setValue', [ val ]);
            this._values$7 = null;
            this.lastQuery = null;
            if (oldValues !== val) {
                this.fireEvent('changeValue', this, val, oldValues);
            }
            return;
        }
        var values;
        if (Type.canCast(val, Array)) {
            values = val;
        }
        else if (!ss.isNullOrUndefined((val).Id) && !ss.isNullOrUndefined((val).Text)) {
            values = LxnBase.UI.Controls.ComboBox._normalizeValue$7(val);
        }
        else {
            LxnBase.UI.Controls.ComboBox.callBaseMethod(this, 'setValue', [ val ]);
            return;
        }
        LxnBase.UI.Controls.ComboBox.callBaseMethod(this, 'setValue', [ values[LxnBase.Data.ObjectInfo.TextPos] ]);
        this._values$7 = values;
        this.fireEvent('changeValue', this, this._values$7, oldValues);
    },
    
    getValue: function LxnBase_UI_Controls_ComboBox$getValue() {
        /// <returns type="Array"></returns>
        var text = this.getRawValue();
        if ((text == null || !text) && !ss.isUndefined(this.value)) {
            this.setValue(null);
        }
        if (!ss.isNullOrUndefined(this._values$7)) {
            return this._values$7;
        }
        return null;
    },
    
    getObjectInfo: function LxnBase_UI_Controls_ComboBox$getObjectInfo() {
        /// <returns type="LxnBase.Data.ObjectInfo"></returns>
        var array = this.getValue();
        if (array == null) {
            return null;
        }
        var info = new LxnBase.Data.ObjectInfo();
        info.Id = array[LxnBase.Data.ObjectInfo.IdPos];
        info.Text = array[LxnBase.Data.ObjectInfo.TextPos];
        info.Type = array[LxnBase.Data.ObjectInfo.TypePos];
        return info;
    },
    
    getSelectedId: function LxnBase_UI_Controls_ComboBox$getSelectedId() {
        /// <returns type="Object"></returns>
        if (this._values$7 == null) {
            return null;
        }
        return this._values$7[LxnBase.Data.ObjectInfo.IdPos];
    },
    
    getTrigger: function LxnBase_UI_Controls_ComboBox$getTrigger() {
        /// <returns type="Ext.Element"></returns>
        if (!this.rendered) {
            return null;
        }
        var trigger = (this.getEl().parent()).select('.selector-trigger', true);
        if (trigger != null) {
            return trigger.elements[0];
        }
        return null;
    },
    
    initComponent: function LxnBase_UI_Controls_ComboBox$initComponent() {
        LxnBase.UI.Controls.ComboBox.callBaseMethod(this, 'initComponent');
        this.addEvents('changeValue');
    },
    
    onLoad: function LxnBase_UI_Controls_ComboBox$onLoad() {
        if (this._backgroundLoading$7) {
            this._backgroundLoading$7 = false;
            var st = this.store;
            if (st.getCount() === 1) {
                this.setValue(st.getAt(0).data);
            }
            else {
                var text = this.lastSelectionText;
                (this.el.dom).value = text || '';
                this.applyEmptyText();
            }
        }
        else {
            LxnBase.UI.Controls.ComboBox.callBaseMethod(this, 'onLoad');
        }
    },
    
    onSelect: function LxnBase_UI_Controls_ComboBox$onSelect(record, index) {
        /// <param name="record" type="Ext.data.Record">
        /// </param>
        /// <param name="index" type="Object">
        /// </param>
        if (this.fireEvent('beforeselect', this, record.data)) {
            this.setValue(record.data);
            this.collapse();
            this.fireEvent('select', this, record.data);
        }
    },
    
    onKeyUp: function LxnBase_UI_Controls_ComboBox$onKeyUp(e) {
        /// <param name="e" type="Ext.EventObject">
        /// </param>
        if (e.getKey() === e.F2) {
            return;
        }
        if (this.editable && String.isNullOrEmpty(this.getRawValue())) {
            this.setValue(null);
        }
        LxnBase.UI.Controls.ComboBox.callBaseMethod(this, 'onKeyUp', [ e ]);
    },
    
    beforeBlur: function LxnBase_UI_Controls_ComboBox$beforeBlur() {
        var info = this.getObjectInfo();
        var val = this.getRawValue();
        if (info != null && val === info.Text) {
            return;
        }
        var rec = this.findRecord(this.displayField, val);
        if (ss.isNullOrUndefined(rec) && this.forceSelection) {
            if (!String.isNullOrEmpty(val) && val !== this.emptyText && val !== this.lastQuery) {
                this._backgroundLoading$7 = true;
                this.doQuery(val, true);
            }
            else {
                this.clearValue();
            }
        }
        else {
            if (!ss.isNullOrUndefined(rec)) {
                val = rec.get(this.valueField || this.displayField);
            }
            this.setValue(val);
        }
    },
    
    _values$7: null,
    _backgroundLoading$7: false
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.Controls.DecimalField

LxnBase.UI.Controls.DecimalField = function LxnBase_UI_Controls_DecimalField(config) {
    /// <param name="config" type="Object">
    /// </param>
    LxnBase.UI.Controls.DecimalField.initializeBase(this, [ config ]);
    this.decimalSeparator = ',';
    this.baseChars = '0123456789.';
    this.decimalPrecision = 2;
}
LxnBase.UI.Controls.DecimalField.prototype = {
    
    setValue: function LxnBase_UI_Controls_DecimalField$setValue(v) {
        /// <param name="v" type="Object">
        /// </param>
        if (ss.isNullOrUndefined(v)) {
            v = '';
        }
        else {
            v = (Type.getInstanceType(v) === Number) ? v : Number.parse((v).replaceAll(this.decimalSeparator, '.'));
            v = (isNaN(v)) ? '' : (v).toFixed(2).replaceAll('.', this.decimalSeparator);
        }
        this.value = v;
        this.setRawValue(v);
        if (this.rendered) {
            this.validate();
        }
    }
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.Controls.ObjectLink

LxnBase.UI.Controls.ObjectLink = function LxnBase_UI_Controls_ObjectLink() {
}
LxnBase.UI.Controls.ObjectLink.render = function LxnBase_UI_Controls_ObjectLink$render(id, text, type) {
    /// <param name="id" type="Object">
    /// </param>
    /// <param name="text" type="Object">
    /// </param>
    /// <param name="type" type="String">
    /// </param>
    /// <returns type="String"></returns>
    return String.format("<a href='#' class='object-link' onclick='{0}.viewObject(event, {1}, {2})'>{3}</a>", (LxnBase.UI.Controls.ObjectLink).get_fullName(), JSON.stringify(type), JSON.stringify(id), text);
}
LxnBase.UI.Controls.ObjectLink.renderInfo = function LxnBase_UI_Controls_ObjectLink$renderInfo(info) {
    /// <param name="info" type="LxnBase.Data.ObjectInfo">
    /// </param>
    /// <returns type="String"></returns>
    if (ss.isNullOrUndefined(info)) {
        return '';
    }
    return LxnBase.UI.Controls.ObjectLink.render(info.Id, info.Text, info.Type);
}
LxnBase.UI.Controls.ObjectLink.renderArray = function LxnBase_UI_Controls_ObjectLink$renderArray(values) {
    /// <param name="values" type="Array" elementType="Object">
    /// </param>
    /// <returns type="String"></returns>
    return LxnBase.UI.Controls.ObjectLink.render(values[LxnBase.Data.ObjectInfo.IdPos], values[LxnBase.Data.ObjectInfo.TextPos], values[LxnBase.Data.ObjectInfo.TypePos]);
}
LxnBase.UI.Controls.ObjectLink.viewObject = function LxnBase_UI_Controls_ObjectLink$viewObject(e, type, id) {
    /// <param name="e" type="ElementEvent">
    /// </param>
    /// <param name="type" type="String">
    /// </param>
    /// <param name="id" type="Object">
    /// </param>
    if (e.ctrlKey) {
        e.cancelBubble = true;
        e.returnValue = false;
    }
    LxnBase.UI.FormsRegistry.viewObject(type, id, e.ctrlKey);
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.Controls.ObjectSelector

LxnBase.UI.Controls.ObjectSelector = function LxnBase_UI_Controls_ObjectSelector(objectSelectorConfig) {
    /// <param name="objectSelectorConfig" type="LxnBase.UI.Controls.ObjectSelectorConfig">
    /// </param>
    /// <field name="_comboBox" type="LxnBase.UI.Controls.ComboBox">
    /// </field>
    /// <field name="_config" type="LxnBase.UI.Controls.ObjectSelectorConfig">
    /// </field>
    /// <field name="_editButton" type="Ext.Element">
    /// </field>
    /// <field name="_minListWidth" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="_actionPropertyName" type="String" static="true">
    /// </field>
    /// <field name="_isCaptionPropertyName" type="String" static="true">
    /// </field>
    this._config = objectSelectorConfig;
    this._config.mode('remote');
    this._render();
}
LxnBase.UI.Controls.ObjectSelector._onBeforeLoadData = function LxnBase_UI_Controls_ObjectSelector$_onBeforeLoadData(store, options) {
    /// <param name="store" type="Ext.data.Store">
    /// </param>
    /// <param name="options" type="Object">
    /// </param>
    var query = store.baseParams.query;
    if (!String.isNullOrEmpty(query)) {
        return;
    }
    var response = new LxnBase.Data.RangeResponse();
    response.List = new Array(0);
    response.TotalCount = 0;
    var proxy = store.proxy;
    proxy.setResponse(response);
}
LxnBase.UI.Controls.ObjectSelector.prototype = {
    
    get_widget: function LxnBase_UI_Controls_ObjectSelector$get_widget() {
        /// <value type="LxnBase.UI.Controls.ComboBox"></value>
        return this._comboBox;
    },
    
    get_config: function LxnBase_UI_Controls_ObjectSelector$get_config() {
        /// <value type="LxnBase.UI.Controls.ObjectSelectorConfig"></value>
        return this._config;
    },
    
    get_text: function LxnBase_UI_Controls_ObjectSelector$get_text() {
        /// <value type="String"></value>
        return this._comboBox.getRawValue();
    },
    
    getValue: function LxnBase_UI_Controls_ObjectSelector$getValue() {
        /// <returns type="Array"></returns>
        return this._comboBox.getValue();
    },
    
    getObjectInfo: function LxnBase_UI_Controls_ObjectSelector$getObjectInfo() {
        /// <returns type="LxnBase.Data.ObjectInfo"></returns>
        return this._comboBox.getObjectInfo();
    },
    
    getObjectId: function LxnBase_UI_Controls_ObjectSelector$getObjectId() {
        /// <returns type="Object"></returns>
        var info = this._comboBox.getObjectInfo();
        return (info != null) ? info.Id : null;
    },
    
    setValue: function LxnBase_UI_Controls_ObjectSelector$setValue(value) {
        /// <param name="value" type="Object">
        /// </param>
        this._comboBox.setValue(value);
    },
    
    focus: function LxnBase_UI_Controls_ObjectSelector$focus() {
        this._comboBox.focus();
    },
    
    _render: function LxnBase_UI_Controls_ObjectSelector$_render() {
        var recordType = [];
        recordType.add('Id');
        recordType.add('Text');
        recordType.add('Type');
        if (this._config.get_valueProperties() != null) {
            var $enum1 = ss.IEnumerator.getEnumerator(this._config.get_valueProperties());
            while ($enum1.moveNext()) {
                var t = $enum1.current;
                recordType.add(t);
            }
        }
        if (this._config.get_store() == null) {
            var proxy = this._config.get_dataProxy() || LxnBase.Services.GenericService.suggestProxy(this._config.get_clazz());
            var metadata = this._config.get_readerMetadata() || { id: 0, root: 'List', totalProperty: 'TotalCount' };
            var store = new Ext.data.Store(new Ext.data.StoreConfig().proxy(proxy).reader(new LxnBase.Data.RangeReader(metadata, recordType)).toDictionary());
            store.on('beforeload', LxnBase.UI.Controls.ObjectSelector._onBeforeLoadData);
            store.on('load', ss.Delegate.create(this, this._onLoadData));
            this._config.store(store);
        }
        var parts = [];
        parts.add('<tpl for=".">');
        parts.add('<div class="{[values.' + 'Action' + ' ? "x-combo-list-item combo-list-action" : values.' + 'IsCaption' + ' ? "x-combo-list-item combo-list-caption" : "x-combo-list-item"]}">{' + 'Text' + '}</div>');
        parts.add('</tpl>');
        var template = new Ext.XTemplate(parts);
        this._config.tpl(template);
        this._config.triggerClass('selector-trigger');
        var dictionary = this._config.toDictionary();
        var css = dictionary['cls'] || '';
        dictionary['cls'] = 'selector ' + css;
        this._comboBox = new LxnBase.UI.Controls.ComboBox(dictionary);
        if (this._comboBox.minListWidth < 250) {
            this._comboBox.minListWidth = 250;
        }
        this._comboBox.on('beforeselect', ss.Delegate.create(this, this._onBeforeSelectItem));
        this._comboBox.on('render', ss.Delegate.create(this, this._onComboBoxRender));
        this._comboBox.on('keydown', ss.Delegate.create(this, this._onKeyDown));
        this._comboBox.on('beforequery', ss.Delegate.create(this, this._onBeforeQuery));
        this._comboBox.on('changeValue', ss.Delegate.create(this, this._onChangeValue));
    },
    
    _createEditButton: function LxnBase_UI_Controls_ObjectSelector$_createEditButton() {
        if (!this._config.get_allowEdit()) {
            return;
        }
        var wrap = this._comboBox.wrap;
        this._editButton = wrap.createChild({ tag: 'div', cls: 'selectorEdit' });
        this._editButton.on('click', ss.Delegate.create(this, this._onEditClick));
        if (this._comboBox.value == null) {
            this._editButton.addClass('selectorEditDisabled');
        }
    },
    
    updateTriggerStatus: function LxnBase_UI_Controls_ObjectSelector$updateTriggerStatus() {
        var trigger = this._comboBox.getTrigger();
        if (trigger == null) {
            return;
        }
        var disabled = false;
        if (String.isNullOrEmpty(this._comboBox.value)) {
            var actions = this._config.getActions(this._comboBox.store, null, null);
            if (actions == null || !actions.length) {
                disabled = true;
            }
        }
        if (disabled) {
            trigger.hide();
            var disableTrigger = $("<div class='tigger-disabled'></div>");
            disableTrigger.insertBefore($(trigger.dom));
        }
        else {
            $('.tigger-disabled', $(trigger.dom.parentNode)).remove();
            trigger.show();
        }
    },
    
    _onComboBoxRender: function LxnBase_UI_Controls_ObjectSelector$_onComboBoxRender() {
        this._createEditButton();
        this.updateTriggerStatus();
    },
    
    _onBeforeQuery: function LxnBase_UI_Controls_ObjectSelector$_onBeforeQuery(e) {
        /// <param name="e" type="Object">
        /// </param>
        var query = e.query;
        var actions = this._config.getActions(this._comboBox.store, null, query);
        if (String.isNullOrEmpty(query) && (actions == null || !actions.length)) {
            e.cancel = true;
            this._comboBox.collapse();
        }
    },
    
    _onLoadData: function LxnBase_UI_Controls_ObjectSelector$_onLoadData(store, records, options) {
        /// <param name="store" type="Ext.data.Store">
        /// </param>
        /// <param name="records" type="Array" elementType="Record">
        /// </param>
        /// <param name="options" type="Object">
        /// </param>
        if (this._comboBox.get_backgroundLoading()) {
            return;
        }
        var actions = this._config.getActions(store, records, store.baseParams.query);
        if (actions.length > 0) {
            for (var i = 0; i < actions.length; i++) {
                var action = actions[i];
                var data = {};
                data['Text'] = action.getText();
                data['Id'] = data['Text'];
                data['Action'] = action;
                var record = new Ext.data.Record();
                record.data = data;
                store.insert(records.length + i, [ record ]);
            }
        }
        if (records == null || !records.length || String.isNullOrEmpty(this._comboBox.getRawValue())) {
            var data = {};
            data['Text'] = LxnBase.BaseRes.noData_Text;
            data['Id'] = -1;
            data['IsCaption'] = true;
            var caption = new Ext.data.Record();
            caption.data = data;
            store.insert(0, [ caption ]);
        }
    },
    
    _onBeforeSelectItem: function LxnBase_UI_Controls_ObjectSelector$_onBeforeSelectItem(comboBox, record, index) {
        /// <param name="comboBox" type="LxnBase.UI.Controls.ComboBox">
        /// </param>
        /// <param name="record" type="Object">
        /// </param>
        /// <param name="index" type="Number">
        /// </param>
        /// <returns type="Boolean"></returns>
        var data = record;
        if (data['IsCaption']) {
            comboBox.collapse();
            return false;
        }
        var action = data['Action'];
        if (ss.isNullOrUndefined(action)) {
            return true;
        }
        comboBox.collapse();
        action.execute(this, this.get_text());
        return false;
    },
    
    _onChangeValue: function LxnBase_UI_Controls_ObjectSelector$_onChangeValue(comboBox, newvalue, oldvalue) {
        /// <param name="comboBox" type="Ext.form.Field">
        /// </param>
        /// <param name="newvalue" type="Object">
        /// </param>
        /// <param name="oldvalue" type="Object">
        /// </param>
        if (!this._comboBox.rendered) {
            return;
        }
        this.updateTriggerStatus();
        if (this._config.get_allowEdit()) {
            if (!this._config.get_allowCreate() && (newvalue == null || !newvalue)) {
                this._editButton.addClass('selectorEditDisabled');
            }
            else {
                this._editButton.removeClass('selectorEditDisabled');
            }
        }
    },
    
    _onKeyDown: function LxnBase_UI_Controls_ObjectSelector$_onKeyDown(field, e) {
        /// <param name="field" type="Object">
        /// </param>
        /// <param name="e" type="Ext.EventObject">
        /// </param>
        if (this._config.get_allowEdit() && e.keyCode === Ext.EventObject.F2 && !e.shiftKey && !e.ctrlKey && !e.altKey) {
            e.stopEvent();
            this._onEditClick();
        }
    },
    
    _onEditClick: function LxnBase_UI_Controls_ObjectSelector$_onEditClick() {
        var value = this.getValue();
        if (value == null && !this._config.get_allowCreate()) {
            return;
        }
        var id = (value == null) ? null : value[LxnBase.Data.ObjectInfo.IdPos];
        var clazz = ((value == null || value[LxnBase.Data.ObjectInfo.TypePos] == null) ? this._config.get_clazz() : value[LxnBase.Data.ObjectInfo.TypePos]);
        LxnBase.UI.FormsRegistry.editObject(clazz, id, null, ss.Delegate.create(this, function(result) {
            var response = result;
            var info = new LxnBase.Data.ObjectInfo();
            info.Id = response.Item.Id;
            info.Text = response.Item.__reference;
            info.Type = response.Item.__class;
            this.setValue(info);
            window.setTimeout(ss.Delegate.create(this, this.focus), 0);
        }), ss.Delegate.create(this, function() {
            this.focus();
        }));
    },
    
    _comboBox: null,
    _config: null,
    _editButton: null
}


////////////////////////////////////////////////////////////////////////////////
// LxnBase.UI.Controls.ObjectSelectorConfig

LxnBase.UI.Controls.ObjectSelectorConfig = function LxnBase_UI_Controls_ObjectSelectorConfig() {
    /// <field name="_class$8" type="String">
    /// </field>
    /// <field name="_valueProperties$8" type="Array" elementType="String">
    /// </field>
    /// <field name="_customActions$8" type="Array" elementType="Action">
    /// </field>
    /// <field name="_getCustomActions$8" type="Function">
    /// </field>
    /// <field name="_allowEdit$8" type="Boolean">
    /// </field>
    /// <field name="_allowCreate$8" type="Boolean">
    /// </field>
    /// <field name="_dataProxy$8" type="Ext.data.DataProxy">
    /// </field>
    /// <field name="_readerMetadata$8" type="Object">
    /// </field>
    LxnBase.UI.Controls.ObjectSelectorConfig.initializeBase(this);
    this.minChars(0);
    this.typeAhead(false);
    this.forceSelection(true);
    this.resizable(true);
    this.enableKeyEvents(true);
    this.queryDelay(50);
    this.selectOnFocus(true);
    this.valueField('Id');
    this.displayField('Text');
}
LxnBase.UI.Controls.ObjectSelectorConfig.prototype = {
    
    get_clazz: function LxnBase_UI_Controls_ObjectSelectorConfig$get_clazz() {
        /// <value type="String"></value>
        return this._class$8;
    },
    
    get_valueField: function LxnBase_UI_Controls_ObjectSelectorConfig$get_valueField() {
        /// <value type="String"></value>
        return this.o['valueField'];
    },
    
    get_displayField: function LxnBase_UI_Controls_ObjectSelectorConfig$get_displayField() {
        /// <value type="String"></value>
        return this.o['displayField'];
    },
    
    get_valueProperties: function LxnBase_UI_Controls_ObjectSelectorConfig$get_valueProperties() {
        /// <value type="Array" elementType="String"></value>
        return this._valueProperties$8;
    },
    
    getActions: function LxnBase_UI_Controls_ObjectSelectorConfig$getActions(store, records, query) {
        /// <param name="store" type="Ext.data.Store">
        /// </param>
        /// <param name="records" type="Array" elementType="Record">
        /// </param>
        /// <param name="query" type="String">
        /// </param>
        /// <returns type="Array" elementType="Action"></returns>
        var actions = [];
        if (this._getCustomActions$8 != null) {
            var list = this._getCustomActions$8(store, records, query);
            if (list != null && list.length > 0) {
                actions.addRange(list);
            }
        }
        if (this._customActions$8 != null && this._customActions$8.length > 0) {
            actions.addRange(this._customActions$8);
        }
        return actions;
    },
    
    get_dataProxy: function LxnBase_UI_Controls_ObjectSelectorConfig$get_dataProxy() {
        /// <value type="Ext.data.DataProxy"></value>
        return this._dataProxy$8;
    },
    
    get_readerMetadata: function LxnBase_UI_Controls_ObjectSelectorConfig$get_readerMetadata() {
        /// <value type="Object"></value>
        return this._readerMetadata$8;
    },
    
    get_allowCreate: function LxnBase_UI_Controls_ObjectSelectorConfig$get_allowCreate() {
        /// <value type="Boolean"></value>
        return this._allowCreate$8;
    },
    
    get_allowEdit: function LxnBase_UI_Controls_ObjectSelectorConfig$get_allowEdit() {
        /// <value type="Boolean"></value>
        return this._allowEdit$8;
    },
    
    get_store: function LxnBase_UI_Controls_ObjectSelectorConfig$get_store() {
        /// <value type="Object"></value>
        return this.o['store'];
    },
    
    setClass: function LxnBase_UI_Controls_ObjectSelectorConfig$setClass(clazz) {
        /// <param name="clazz" type="String">
        /// </param>
        /// <returns type="LxnBase.UI.Controls.ObjectSelectorConfig"></returns>
        this._class$8 = clazz;
        return this;
    },
    
    valueProperties: function LxnBase_UI_Controls_ObjectSelectorConfig$valueProperties(valueProperties) {
        /// <param name="valueProperties" type="Array" elementType="String">
        /// </param>
        /// <returns type="LxnBase.UI.Controls.ObjectSelectorConfig"></returns>
        this._valueProperties$8 = valueProperties;
        return this;
    },
    
    customActions: function LxnBase_UI_Controls_ObjectSelectorConfig$customActions(customActions) {
        /// <param name="customActions" type="Array" elementType="Action">
        /// </param>
        /// <returns type="LxnBase.UI.Controls.ObjectSelectorConfig"></returns>
        this._customActions$8 = customActions;
        return this;
    },
    
    customActionsDelegate: function LxnBase_UI_Controls_ObjectSelectorConfig$customActionsDelegate(getCustomActions) {
        /// <param name="getCustomActions" type="Function">
        /// </param>
        /// <returns type="LxnBase.UI.Controls.ObjectSelectorConfig"></returns>
        this._getCustomActions$8 = getCustomActions;
        return this;
    },
    
    setValue: function LxnBase_UI_Controls_ObjectSelectorConfig$setValue(val) {
        /// <param name="val" type="LxnBase.Data.ObjectInfo">
        /// </param>
        /// <returns type="LxnBase.UI.Controls.ObjectSelectorConfig"></returns>
        if (!ss.isNullOrUndefined(val)) {
            this.value([ val.Id, val.Text ]);
        }
        return this;
    },
    
    setDataProxy: function LxnBase_UI_Controls_ObjectSelectorConfig$setDataProxy(proxy) {
        /// <param name="proxy" type="Ext.data.DataProxy">
        /// </param>
        /// <returns type="LxnBase.UI.Controls.ObjectSelectorConfig"></returns>
        this._dataProxy$8 = proxy;
        return this;
    },
    
    setReaderMetadata: function LxnBase_UI_Controls_ObjectSelectorConfig$setReaderMetadata(readerMetadata) {
        /// <param name="readerMetadata" type="Object">
        /// </param>
        /// <returns type="LxnBase.UI.Controls.ObjectSelectorConfig"></returns>
        this._readerMetadata$8 = readerMetadata;
        return this;
    },
    
    allowCreate: function LxnBase_UI_Controls_ObjectSelectorConfig$allowCreate(allowCreate) {
        /// <param name="allowCreate" type="Boolean">
        /// </param>
        /// <returns type="LxnBase.UI.Controls.ObjectSelectorConfig"></returns>
        this._allowCreate$8 = allowCreate;
        return this;
    },
    
    allowEdit: function LxnBase_UI_Controls_ObjectSelectorConfig$allowEdit(allowEdit) {
        /// <param name="allowEdit" type="Boolean">
        /// </param>
        /// <returns type="LxnBase.UI.Controls.ObjectSelectorConfig"></returns>
        this._allowEdit$8 = allowEdit;
        return this;
    },
    
    _class$8: null,
    _valueProperties$8: null,
    _customActions$8: null,
    _getCustomActions$8: null,
    _allowEdit$8: false,
    _allowCreate$8: false,
    _dataProxy$8: null,
    _readerMetadata$8: null
}


LxnBase.EnumUtility.registerClass('LxnBase.EnumUtility');
LxnBase.MessageRegister.registerClass('LxnBase.MessageRegister');
LxnBase.MessageRegisterEventArgs.registerClass('LxnBase.MessageRegisterEventArgs', ss.EventArgs);
LxnBase.NumberUtility.registerClass('LxnBase.NumberUtility');
LxnBase.StringUtility.registerClass('LxnBase.StringUtility');
LxnBase.Data.ObjectInfo.registerClass('LxnBase.Data.ObjectInfo');
LxnBase.Data.DeleteOperationResponse.registerClass('LxnBase.Data.DeleteOperationResponse');
LxnBase.Data.DocumentExportArgs.registerClass('LxnBase.Data.DocumentExportArgs');
LxnBase.Data.ItemResponse.registerClass('LxnBase.Data.ItemResponse');
LxnBase.Data.OperationPermissions.registerClass('LxnBase.Data.OperationPermissions');
LxnBase.Data.OperationStatus.registerClass('LxnBase.Data.OperationStatus');
LxnBase.Data.PropertyFilter.registerClass('LxnBase.Data.PropertyFilter');
LxnBase.Data.PropertyFilterCondition.registerClass('LxnBase.Data.PropertyFilterCondition');
LxnBase.Data.RangeRequest.registerClass('LxnBase.Data.RangeRequest');
LxnBase.Data.RangeResponse.registerClass('LxnBase.Data.RangeResponse');
LxnBase.Data.ConfigManager.registerClass('LxnBase.Data.ConfigManager');
LxnBase.Data.ObjectPropertyNames.registerClass('LxnBase.Data.ObjectPropertyNames');
LxnBase.Services.ColumnConfig.registerClass('LxnBase.Services.ColumnConfig');
LxnBase.Services.ClassColumnConfig.registerClass('LxnBase.Services.ClassColumnConfig', LxnBase.Services.ColumnConfig);
LxnBase.Services.ClassDefinition.registerClass('LxnBase.Services.ClassDefinition');
LxnBase.Services.CustomTypeColumnConfig.registerClass('LxnBase.Services.CustomTypeColumnConfig', LxnBase.Services.ColumnConfig);
LxnBase.Services.DateTimeColumnConfig.registerClass('LxnBase.Services.DateTimeColumnConfig', LxnBase.Services.ColumnConfig);
LxnBase.Services.GenericService.registerClass('LxnBase.Services.GenericService');
LxnBase.Services.ItemConfig.registerClass('LxnBase.Services.ItemConfig');
LxnBase.Services.ListColumnConfig.registerClass('LxnBase.Services.ListColumnConfig', LxnBase.Services.ColumnConfig);
LxnBase.Services.ListConfig.registerClass('LxnBase.Services.ListConfig');
LxnBase.Services.NumberColumnConfig.registerClass('LxnBase.Services.NumberColumnConfig', LxnBase.Services.ColumnConfig);
LxnBase.Services.TextColumnConfig.registerClass('LxnBase.Services.TextColumnConfig', LxnBase.Services.ColumnConfig);
LxnBase.Knockout.EmailBindingHandler.registerClass('LxnBase.Knockout.EmailBindingHandler', Object);
LxnBase.Knockout.FixedBindingHandler.registerClass('LxnBase.Knockout.FixedBindingHandler', Object);
LxnBase.Knockout.LinkBindingHandler.registerClass('LxnBase.Knockout.LinkBindingHandler', Object);
LxnBase.Knockout.ResBindingHandler.registerClass('LxnBase.Knockout.ResBindingHandler', Object);
LxnBase.Knockout.StringTemplate.registerClass('LxnBase.Knockout.StringTemplate');
LxnBase.Knockout.ViewBindingHandler.registerClass('LxnBase.Knockout.ViewBindingHandler', Object);
LxnBase.Net.WebServiceError.registerClass('LxnBase.Net.WebServiceError');
LxnBase.Net.WebServiceFailureArgs.registerClass('LxnBase.Net.WebServiceFailureArgs');
LxnBase.Net.WebService.registerClass('LxnBase.Net.WebService');
LxnBase.UI.ActionFactory.registerClass('LxnBase.UI.ActionFactory');
LxnBase.UI.BaseEditForm.registerClass('LxnBase.UI.BaseEditForm', null, LxnBase.UI.IKeyHandler);
LxnBase.UI.BaseClassEditForm.registerClass('LxnBase.UI.BaseClassEditForm', LxnBase.UI.BaseEditForm);
LxnBase.UI.Tab.registerClass('LxnBase.UI.Tab', Ext.Panel, LxnBase.UI.IKeyHandler);
LxnBase.UI.BaseClassViewForm.registerClass('LxnBase.UI.BaseClassViewForm', LxnBase.UI.Tab);
LxnBase.UI.EditFormArgs.registerClass('LxnBase.UI.EditFormArgs');
LxnBase.UI.EventsManager.registerClass('LxnBase.UI.EventsManager');
LxnBase.UI.FieldActionManager.registerClass('LxnBase.UI.FieldActionManager');
LxnBase.UI.FieldActions.registerClass('LxnBase.UI.FieldActions');
LxnBase.UI.FieldNavigationManager.registerClass('LxnBase.UI.FieldNavigationManager');
LxnBase.UI.Form.registerClass('LxnBase.UI.Form', Ext.form.FormPanel);
LxnBase.UI.FormsRegistry.registerClass('LxnBase.UI.FormsRegistry');
LxnBase.UI.ViewAction.registerClass('LxnBase.UI.ViewAction');
LxnBase.UI.Infos.registerClass('LxnBase.UI.Infos');
LxnBase.UI.KeyEventHandler.registerClass('LxnBase.UI.KeyEventHandler');
LxnBase.UI.ListArgs.registerClass('LxnBase.UI.ListArgs');
LxnBase.UI.LxnMenu.registerClass('LxnBase.UI.LxnMenu', Ext.menu.Menu);
LxnBase.UI.MessageBoxWrap.registerClass('LxnBase.UI.MessageBoxWrap');
LxnBase.UI.Messages.registerClass('LxnBase.UI.Messages');
LxnBase.UI.PropertyFilterExtention.registerClass('LxnBase.UI.PropertyFilterExtention');
LxnBase.UI.ReplaceForm.registerClass('LxnBase.UI.ReplaceForm', LxnBase.UI.BaseEditForm);
LxnBase.UI.SelectArgs.registerClass('LxnBase.UI.SelectArgs');
LxnBase.UI.Tabs.registerClass('LxnBase.UI.Tabs');
LxnBase.UI.AutoControls.AutoGrid.registerClass('LxnBase.UI.AutoControls.AutoGrid', Ext.grid.EditorGridPanel);
LxnBase.UI.AutoControls.AutoGridArgs.registerClass('LxnBase.UI.AutoControls.AutoGridArgs');
LxnBase.UI.AutoControls.AutoGridView.registerClass('LxnBase.UI.AutoControls.AutoGridView', Ext.grid.GridView);
LxnBase.UI.AutoControls.ControlFactory.registerClass('LxnBase.UI.AutoControls.ControlFactory');
LxnBase.UI.AutoControls.GridFilterConfig.registerClass('LxnBase.UI.AutoControls.GridFilterConfig');
LxnBase.UI.AutoControls.GridFilterPlugin.registerClass('LxnBase.UI.AutoControls.GridFilterPlugin');
LxnBase.UI.AutoControls.RecordMeta.registerClass('LxnBase.UI.AutoControls.RecordMeta', ss.Record);
LxnBase.UI.AutoControls.ReportLoader.registerClass('LxnBase.UI.AutoControls.ReportLoader');
LxnBase.UI.AutoForms.AutoEditForm.registerClass('LxnBase.UI.AutoForms.AutoEditForm', LxnBase.UI.BaseClassEditForm);
LxnBase.UI.AutoForms.AutoFormCallbacks.registerClass('LxnBase.UI.AutoForms.AutoFormCallbacks');
LxnBase.UI.AutoForms.AutoListTab.registerClass('LxnBase.UI.AutoForms.AutoListTab', LxnBase.UI.Tab);
LxnBase.UI.AutoForms.AutoSelectForm.registerClass('LxnBase.UI.AutoForms.AutoSelectForm', null, LxnBase.UI.IKeyHandler);
LxnBase.UI.AutoForms.AutoViewForm.registerClass('LxnBase.UI.AutoForms.AutoViewForm', LxnBase.UI.BaseClassViewForm);
LxnBase.UI.Controls.ColumnFilters.BaseFilter.registerClass('LxnBase.UI.Controls.ColumnFilters.BaseFilter');
LxnBase.UI.Controls.ColumnFilters.BooleanFilter.registerClass('LxnBase.UI.Controls.ColumnFilters.BooleanFilter', LxnBase.UI.Controls.ColumnFilters.BaseFilter);
LxnBase.UI.Controls.ColumnFilters.DateFilter.registerClass('LxnBase.UI.Controls.ColumnFilters.DateFilter', LxnBase.UI.Controls.ColumnFilters.BaseFilter);
LxnBase.UI.Controls.ColumnFilters.EditorFilter.registerClass('LxnBase.UI.Controls.ColumnFilters.EditorFilter', LxnBase.UI.Controls.ColumnFilters.BaseFilter);
LxnBase.UI.Controls.ColumnFilters.ListFilter.registerClass('LxnBase.UI.Controls.ColumnFilters.ListFilter', LxnBase.UI.Controls.ColumnFilters.BaseFilter);
LxnBase.UI.Controls.ColumnFilters.NumberFilter.registerClass('LxnBase.UI.Controls.ColumnFilters.NumberFilter', LxnBase.UI.Controls.ColumnFilters.EditorFilter);
LxnBase.UI.Controls.ColumnFilters.StringFilter.registerClass('LxnBase.UI.Controls.ColumnFilters.StringFilter', LxnBase.UI.Controls.ColumnFilters.EditorFilter);
LxnBase.UI.Controls.ComboBox.registerClass('LxnBase.UI.Controls.ComboBox', Ext.form.ComboBox);
LxnBase.UI.Controls.DecimalField.registerClass('LxnBase.UI.Controls.DecimalField', Ext.form.NumberField);
LxnBase.UI.Controls.ObjectLink.registerClass('LxnBase.UI.Controls.ObjectLink');
LxnBase.UI.Controls.ObjectSelector.registerClass('LxnBase.UI.Controls.ObjectSelector');
LxnBase.UI.Controls.ObjectSelectorConfig.registerClass('LxnBase.UI.Controls.ObjectSelectorConfig', Ext.form.ComboBoxConfig);
LxnBase.MessageRegister.__newMessage = null;
LxnBase.Data.ObjectInfo.IdPos = 0;
LxnBase.Data.ObjectInfo.TextPos = 1;
LxnBase.Data.ObjectInfo.TypePos = 2;
LxnBase.Data.ConfigManager._listConfigs = {};
LxnBase.Data.ConfigManager._viewConfigs = {};
LxnBase.Data.ConfigManager._editConfigs = {};
LxnBase.Data.ObjectPropertyNames.idPropertyName = 'Id';
LxnBase.Data.ObjectPropertyNames.versionPropertyName = 'Version';
LxnBase.Data.ObjectPropertyNames.textPropertyName = 'Text';
LxnBase.Data.ObjectPropertyNames.typePropertyName = 'Type';
LxnBase.Data.ObjectPropertyNames.referencePropertyName = '__reference';
LxnBase.Data.ObjectPropertyNames.objectClassPropertyName = '__class';
LxnBase.Services.GenericService.service = null;
(function () {
    LxnBase.Services.GenericService.service = new LxnBase.Net.WebService('GenericService.asmx');
})();
LxnBase.Knockout.EmailBindingHandler._protocolCheck$1 = new RegExp('^.+:');
(function () {
    ko.bindingHandlers['email'] = new LxnBase.Knockout.EmailBindingHandler();
})();
(function () {
    ko.bindingHandlers['fixed'] = new LxnBase.Knockout.FixedBindingHandler();
})();
LxnBase.Knockout.LinkBindingHandler._protocolCheck$1 = new RegExp('^.+://');
(function () {
    ko.bindingHandlers['link'] = new LxnBase.Knockout.LinkBindingHandler();
})();
LxnBase.Knockout.ResBindingHandler._resources$1 = [];
(function () {
    ko.bindingHandlers['res'] = new LxnBase.Knockout.ResBindingHandler();
})();
LxnBase.Knockout.StringTemplate._templates = {};
(function () {
    var templateEngine = new ko.nativeTemplateEngine();
    templateEngine.makeTemplateSource = LxnBase.Knockout.StringTemplate.get;
    ko.setTemplateEngine(templateEngine);
})();
(function () {
    ko.bindingHandlers['view'] = new LxnBase.Knockout.ViewBindingHandler();
})();
LxnBase.Net.WebService.root = null;
LxnBase.Net.WebService.__failure = null;
LxnBase.UI.EventsManager._handlerList = null;
(function () {
    LxnBase.UI.EventsManager._handlerList = [];
    $(document).keydown(LxnBase.UI.EventsManager._onKeyDown);
})();
LxnBase.UI.FormsRegistry._lists = {};
LxnBase.UI.FormsRegistry._defaultList = null;
LxnBase.UI.FormsRegistry._views = {};
LxnBase.UI.FormsRegistry._defaultView = null;
LxnBase.UI.FormsRegistry._edits = {};
LxnBase.UI.FormsRegistry._defaultEdit = null;
LxnBase.UI.FormsRegistry._selects = {};
LxnBase.UI.FormsRegistry._defaultSelect = null;
LxnBase.UI.Infos._container = null;
LxnBase.UI.Tabs._panel = null;
LxnBase.UI.AutoControls.ControlFactory._customRenderers = {};
LxnBase.UI.AutoControls.ControlFactory._customEditors = {};
LxnBase.UI.AutoControls.GridFilterPlugin._customFilters = {};
LxnBase.UI.AutoControls.ReportLoader._instance = null;
LxnBase.UI.Controls.ColumnFilters.BaseFilter._filterMenuClass = 'filterMenu';
LxnBase.UI.Controls.ColumnFilters.EditorFilter._editorClassName = 'filterEditor';
LxnBase.UI.Controls.ColumnFilters.EditorFilter._defaultOperator = 1;
})(jQuery);

LxnBase.ObjectUtility = {
	merge: function (target, obj) {
		for (var key in obj) {
			if (obj.hasOwnProperty(key)) {
				var copy = obj[key];
				if (target.hasOwnProperty(key)) {
					var src = target[key];
					if (typeof(src) === 'object' && typeof(copy) === 'object') {
						LxnBase.ObjectUtility.merge(src, copy);
						continue;
					}
				}
				target[key] = copy;
			}
		}
		return target;
	}
};

//! This script was generated using Script# v0.7.4.0
