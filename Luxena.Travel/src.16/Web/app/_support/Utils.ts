
namespace Luxena.Utils
{
	export var reClassName = /\w+/g;
}


function classNameOf(obj: Object)
{
	return obj && obj.constructor.toString().match(Luxena.Utils.reClassName)[1];
}


interface Array<T>
{
	register(items: T[]);
	register(...items: T[]);
}

Array.prototype.register = function (...items: any[])
{
	if (!items) return;
	if (items.length === 1 && items[0] instanceof Array)
		items = items[0];

	for (let item of items)
	{
		if (item !== undefined && this.indexOf(item) < 0)
			this.push(item);
	}
};


function $as<T1, T2>(a: T1, action: (a: T1) => T2, catchAction?: (a: T1, ex) => T2): T2
{
	if (!catchAction)
	{
		return a && action && action(a);
	}
	else
	{
		try
		{
			return a && action && action(a);
		}
		catch (ex)
		{
			catchAction(a, ex);
		}
	}
}

function $do<T>(a: T, action: (a: T) => void, catchAction?: (a: T, ex) => void): T
{
	if (!catchAction)
	{
		a && action && action(a);
		return a;
	}
	else
	{
		try
		{
			a && action && action(a);
			return a;
		}
		catch (ex)
		{
			catchAction(a, ex);
			return a;
		}
	}
}

var $extend = $.extend;

function $append(target, ...objects: any[])
{
	if (objects && objects.length)
	{
		if (!target)
			target = {};
		for (const obj of objects)
		{
			if (!obj) continue;

			for (const prop of Object.getOwnPropertyNames(obj))
			{
				if (target[prop] === undefined)
					target[prop] = obj[prop];
			}
		}
	}

	return target;
}

function $position(obj)
{
	if (!obj.offsetParent) return null;

	let y = 0;
	let x = 0;
	do
	{
		x += obj.offsetLeft;
		y += obj.offsetTop;
	}
	while (obj = obj.offsetParent);

	return { x: x, y: y};
}


function $clip(obj)
{
	return typeof obj === "string" ? obj.trim() : obj;
}

function $clone<T>(obj: T, cfg?)
{
	return <T>$.extend({}, obj, cfg);
}

function $div(className?: string)
{
	return className ? $(`<div class="${className}">`) : $(`<div>`);
}



function isArray(obj): obj is any[]
{
	return obj instanceof Array;
}
function isFunction(obj): obj is Function
{
	return typeof obj === "function";
}
function isString(obj): obj is string
{
	return typeof obj === "string";
}