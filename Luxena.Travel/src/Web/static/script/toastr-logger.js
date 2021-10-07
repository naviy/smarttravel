(function (toastr)
{
	toastr.options.timeOut = 2000; // 2 second toast timeout
	toastr.options.positionClass = 'toast-bottom-right';

	var indent = ' ';
	var lineNo = 0;
	function incIndent() { indent += '    '; }
	function decIndent() { indent = indent.substring(0, indent.length - 4) || ' '; }

	var $log = window.$log = function ()
	{
		var console = window.console;
		var line = (++lineNo).toString();
		while (line.length < 4) line = ' ' + line;

		var msg = [line + '    ' + indent];
		msg.push.apply(msg, arguments);
		!!console && console.log && console.log.apply(console, msg);
		return arguments[0];
	};

	window.$logb = function(message, action)
	{
		$log(indent, message, '{');
		incIndent();
		
		if (action) {
			var result = action();
			$loge();
			return result;
		}
	};

	window.$loge = function ()
	{
		decIndent();
		$log('}');
	};

	function showLog(type1, type2, message, title)
	{
		if (message.Message && !title)
			title = title || message.Message;
		

		toastr[type1](message.ExceptionMessage || message, title || message.Message);

		if (console && console[type2]) {
			var msg;
			if (!title)
				msg = message;
			else {
				msg = {};
				msg[title] = message;
			}
			console[type2](indent, msg);
		}
		return message;
	}

	$log.error = function (message, title)
	{
		return showLog('error', 'error', message, title);
	};
	$log.info = function (message, title)
	{
		return showLog('info', 'info', message, title);
	};
	$log.success = function (message, title)
	{
		return showLog('success', 'info', message, title);
	};
	$log.warning = function (message, title)
	{
		return showLog('warning', 'warn', message, title);
	};
	$log.debug = function (message)
	{
		console.debug(message);
		return message;
	};

	$log.trace = function()
	{
		console.groupCollapsed('Call Stack');
		console.trace();
		console.groupEnd();
	};
})(toastr);