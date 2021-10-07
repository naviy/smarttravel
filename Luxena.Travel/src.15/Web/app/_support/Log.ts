var $log: (...args: Array<any>) => any;
var $log_: (...args: Array<any>) => any;
var $error: (...args: Array<any>) => any;
//var $logt: (...args: Array<any>) => any;

var $logb: (message?: string, action?: () => void) => any;

var $loge: () => void;

var $log_printStack = false;
//var $log_printStack = true;


var $trace = () =>
{
	console.groupCollapsed();
	console.trace();
	console.groupEnd();
};

var $alert = (message: any) =>
{
	$log(message);

	var msg = message;
	if (typeof msg === "object")
		msg = JSON.stringify(message);

	alert(msg);

	return message;
};


module Luxena
{

	var indent = "";
	var lineNo = 0;
	function incIndent() { indent += "    "; }
	function decIndent() { indent = indent.substring(0, indent.length - 4) || ""; }

	//$log = (...args: Array<any>) =>
	//{
	//	var console = window.console;
	//	if (!console || !console.log) return undefined;

	//	var line = (++lineNo).toString();
	//	while (line.length < 4) line = ' ' + line;

	//	var msg: any[] = [line + "    " + indent];
	//	msg.push.apply(msg, args);
	//	console.log.apply(console, msg);

	//	return args[0];
	//};


	var reStackClassMethod = /\<\/([\w\d_]+)\.prototype\.([\w\d_]+)\@/;

	$error = $log = $log_ = (...args: Array<any>) =>
	{
		var console = window.console;
		if (!console || !console.log) return undefined;

		var line = (++lineNo).toString();
		while (line.length < 4) line = ' ' + line;

		var indent2 = "        " + indent;

		var msg: any[] = [line + "    " + indent];
		msg.push.apply(msg, args);

		if ($log_printStack)
		{
			var stack: any[];
			try
			{
				throw new Error();
			}
			catch (ex)
			{
				stack = ex.stack.split("\n");
			}

			stack.splice(0, 1);
			for (var i = 0, len = stack.length; i < len; i++)
			{
				var caller = stack[i];
				var match = reStackClassMethod.exec(caller);
				if (match)
					caller = match[1] + "." + match[2] + "()";

				stack[i] = indent2 + caller;
			}


			console.groupCollapsed.apply(console, msg);
			console.log(indent2, $log.caller);
			console.log(stack.join("\n"));
			console.groupEnd();
		}
		else
		{
			console.log.apply(console, msg);
		}

		return args[0];
	};

	$logb = (message, action) =>
	{
		$log((message || "") + "{");
		incIndent();

		if (action)
		{
			var result = action();
			$loge();
			return result;
		}
	};

	$loge = () =>
	{
		decIndent();
		$log("}");
	};


} 