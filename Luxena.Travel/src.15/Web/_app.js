var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var tsUnit;
(function (tsUnit) {
    var Test = (function () {
        function Test() {
            var testModules = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                testModules[_i - 0] = arguments[_i];
            }
            this.privateMemberPrefix = '_';
            this.tests = [];
            this.reservedMethodNameContainer = new TestClass();
            this.createTestLimiter();
            for (var i = 0; i < testModules.length; i++) {
                var testModule = testModules[i];
                for (var testClass in testModule) {
                    this.addTestClass(new testModule[testClass](), testClass);
                }
            }
        }
        Test.prototype.addTestClass = function (testClass, name) {
            if (name === void 0) { name = 'Tests'; }
            this.tests.push(new TestDefintion(testClass, name));
        };
        Test.prototype.run = function (testRunLimiter) {
            if (testRunLimiter === void 0) { testRunLimiter = null; }
            var parameters = null;
            var testContext = new TestContext();
            var testResult = new TestResult();
            if (testRunLimiter == null) {
                testRunLimiter = this.testRunLimiter;
            }
            for (var i = 0; i < this.tests.length; ++i) {
                var testClass = this.tests[i].testClass;
                var dynamicTestClass = testClass;
                var testsGroupName = this.tests[i].name;
                if (testRunLimiter && !testRunLimiter.isTestsGroupActive(testsGroupName)) {
                    continue;
                }
                for (var unitTestName in testClass) {
                    if (this.isReservedFunctionName(unitTestName)
                        || (unitTestName.substring(0, this.privateMemberPrefix.length) === this.privateMemberPrefix)
                        || (typeof dynamicTestClass[unitTestName] !== 'function')
                        || (testRunLimiter && !testRunLimiter.isTestActive(unitTestName))) {
                        continue;
                    }
                    if (typeof dynamicTestClass[unitTestName].parameters !== 'undefined') {
                        parameters = dynamicTestClass[unitTestName].parameters;
                        for (var parameterIndex = 0; parameterIndex < parameters.length; parameterIndex++) {
                            if (testRunLimiter && !testRunLimiter.isParametersSetActive(parameterIndex)) {
                                continue;
                            }
                            this.runSingleTest(testResult, testClass, unitTestName, testsGroupName, parameters, parameterIndex);
                        }
                    }
                    else {
                        this.runSingleTest(testResult, testClass, unitTestName, testsGroupName);
                    }
                }
            }
            return testResult;
        };
        Test.prototype.showResults = function (target, result) {
            var template = '<article>' +
                '<h1>' + this.getTestResult(result) + '</h1>' +
                '<p>' + this.getTestSummary(result) + '</p>' +
                this.testRunLimiter.getLimitCleaner() +
                '<section id="tsFail">' +
                '<h2>Errors</h2>' +
                '<ul class="bad">' + this.getTestResultList(result.errors) + '</ul>' +
                '</section>' +
                '<section id="tsOkay">' +
                '<h2>Passing Tests</h2>' +
                '<ul class="good">' + this.getTestResultList(result.passes) + '</ul>' +
                '</section>' +
                '</article>' +
                this.testRunLimiter.getLimitCleaner();
            target.innerHTML = template;
        };
        Test.prototype.getTapResults = function (result) {
            var newLine = '\r\n';
            var template = '1..' + (result.passes.length + result.errors.length).toString() + newLine;
            for (var i = 0; i < result.errors.length; i++) {
                template += 'not ok ' + result.errors[i].message + ' ' + result.errors[i].testName + newLine;
            }
            for (var i = 0; i < result.passes.length; i++) {
                template += 'ok ' + result.passes[i].testName + newLine;
            }
            return template;
        };
        Test.prototype.createTestLimiter = function () {
            try {
                if (typeof window !== 'undefined') {
                    this.testRunLimiter = new TestRunLimiter();
                }
            }
            catch (ex) { }
        };
        Test.prototype.isReservedFunctionName = function (functionName) {
            for (var prop in this.reservedMethodNameContainer) {
                if (prop === functionName) {
                    return true;
                }
            }
            return false;
        };
        Test.prototype.runSingleTest = function (testResult, testClass, unitTestName, testsGroupName, parameters, parameterSetIndex) {
            if (parameters === void 0) { parameters = null; }
            if (parameterSetIndex === void 0) { parameterSetIndex = null; }
            if (typeof testClass['setUp'] === 'function') {
                testClass['setUp']();
            }
            try {
                var dynamicTestClass = testClass;
                var args = (parameterSetIndex !== null) ? parameters[parameterSetIndex] : null;
                dynamicTestClass[unitTestName].apply(testClass, args);
                testResult.passes.push(new TestDescription(testsGroupName, unitTestName, parameterSetIndex, 'OK'));
            }
            catch (err) {
                testResult.errors.push(new TestDescription(testsGroupName, unitTestName, parameterSetIndex, err.toString()));
            }
            if (typeof testClass['tearDown'] === 'function') {
                testClass['tearDown']();
            }
        };
        Test.prototype.getTestResult = function (result) {
            return result.errors.length === 0 ? 'Test Passed' : 'Test Failed';
        };
        Test.prototype.getTestSummary = function (result) {
            return 'Total tests: <span id="tsUnitTotalCout">' + (result.passes.length + result.errors.length).toString() + '</span>. ' +
                'Passed tests: <span id="tsUnitPassCount" class="good">' + result.passes.length + '</span>. ' +
                'Failed tests: <span id="tsUnitFailCount" class="bad">' + result.errors.length + '</span>.';
        };
        Test.prototype.getTestResultList = function (testResults) {
            var list = '';
            var group = '';
            var isFirst = true;
            for (var i = 0; i < testResults.length; ++i) {
                var result = testResults[i];
                if (result.testName !== group) {
                    group = result.testName;
                    if (isFirst) {
                        isFirst = false;
                    }
                    else {
                        list += '</li></ul>';
                    }
                    list += '<li>' + this.testRunLimiter.getLimiterForGroup(group) + result.testName + '<ul>';
                }
                var resultClass = (result.message === 'OK') ? 'success' : 'error';
                var functionLabal = result.funcName + ((result.parameterSetNumber === null)
                    ? '()'
                    : '(' + this.testRunLimiter.getLimiterForTest(group, result.funcName, result.parameterSetNumber) + ' paramater set: ' + result.parameterSetNumber + ')');
                list += '<li class="' + resultClass + '">' + this.testRunLimiter.getLimiterForTest(group, result.funcName) + functionLabal + ': ' + this.encodeHtmlEntities(result.message) + '</li>';
            }
            return list + '</ul>';
        };
        Test.prototype.encodeHtmlEntities = function (input) {
            return input.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
        };
        return Test;
    })();
    tsUnit.Test = Test;
    var TestRunLimiterRunAll = (function () {
        function TestRunLimiterRunAll() {
        }
        TestRunLimiterRunAll.prototype.isTestsGroupActive = function (groupName) {
            return true;
        };
        TestRunLimiterRunAll.prototype.isTestActive = function (testName) {
            return true;
        };
        TestRunLimiterRunAll.prototype.isParametersSetActive = function (paramatersSetNumber) {
            return true;
        };
        return TestRunLimiterRunAll;
    })();
    tsUnit.TestRunLimiterRunAll = TestRunLimiterRunAll;
    var TestRunLimiter = (function () {
        function TestRunLimiter() {
            this.groupName = null;
            this.testName = null;
            this.parameterSet = null;
            this.setRefreshOnLinksWithHash();
            this.translateStringIntoTestsLimit(window.location.hash);
        }
        TestRunLimiter.prototype.isTestsGroupActive = function (groupName) {
            if (this.groupName === null) {
                return true;
            }
            return this.groupName === groupName;
        };
        TestRunLimiter.prototype.isTestActive = function (testName) {
            if (this.testName === null) {
                return true;
            }
            return this.testName === testName;
        };
        TestRunLimiter.prototype.isParametersSetActive = function (paramatersSet) {
            if (this.parameterSet === null) {
                return true;
            }
            return this.parameterSet === paramatersSet;
        };
        TestRunLimiter.prototype.getLimiterForTest = function (groupName, testName, parameterSet) {
            if (parameterSet === void 0) { parameterSet = null; }
            if (parameterSet !== null) {
                testName += '(' + parameterSet + ')';
            }
            return '&nbsp;<a href="#' + groupName + '/' + testName + '\" class="ascii">&#9658;</a>&nbsp;';
        };
        TestRunLimiter.prototype.getLimiterForGroup = function (groupName) {
            return '&nbsp;<a href="#' + groupName + '" class="ascii">&#9658;</a>&nbsp;';
        };
        TestRunLimiter.prototype.getLimitCleaner = function () {
            return '<p><a href="#">Run all tests <span class="ascii">&#9658;</span></a></p>';
        };
        TestRunLimiter.prototype.setRefreshOnLinksWithHash = function () {
            var previousHandler = window.onhashchange;
            window.onhashchange = function (ev) {
                window.location.reload();
                if (typeof previousHandler === 'function') {
                    previousHandler(ev);
                }
            };
        };
        TestRunLimiter.prototype.translateStringIntoTestsLimit = function (value) {
            var regex = /^#([_a-zA-Z0-9]+)((\/([_a-zA-Z0-9]+))(\(([0-9]+)\))?)?$/;
            var result = regex.exec(value);
            if (result === null) {
                return;
            }
            if (result.length > 1 && !!result[1]) {
                this.groupName = result[1];
            }
            if (result.length > 4 && !!result[4]) {
                this.testName = result[4];
            }
            if (result.length > 6 && !!result[6]) {
                this.parameterSet = parseInt(result[6], 10);
            }
        };
        return TestRunLimiter;
    })();
    var TestContext = (function () {
        function TestContext() {
        }
        TestContext.prototype.setUp = function () {
        };
        TestContext.prototype.tearDown = function () {
        };
        TestContext.prototype.areIdentical = function (expected, actual, message) {
            if (message === void 0) { message = ''; }
            if (expected !== actual) {
                throw this.getError('areIdentical failed when given ' +
                    this.printVariable(expected) + ' and ' + this.printVariable(actual), message);
            }
        };
        TestContext.prototype.areNotIdentical = function (expected, actual, message) {
            if (message === void 0) { message = ''; }
            if (expected === actual) {
                throw this.getError('areNotIdentical failed when given ' +
                    this.printVariable(expected) + ' and ' + this.printVariable(actual), message);
            }
        };
        TestContext.prototype.areCollectionsIdentical = function (expected, actual, message) {
            var _this = this;
            if (message === void 0) { message = ''; }
            function resultToString(result) {
                var msg = '';
                while (result.length > 0) {
                    msg = '[' + result.pop() + ']' + msg;
                }
                return msg;
            }
            var compareArray = function (expected, actual, result) {
                var indexString = '';
                if (expected === null) {
                    if (actual !== null) {
                        indexString = resultToString(result);
                        throw _this.getError('areCollectionsIdentical failed when array a' +
                            indexString + ' is null and b' +
                            indexString + ' is not null', message);
                    }
                    return; // correct: both are nulls
                }
                else if (actual === null) {
                    indexString = resultToString(result);
                    throw _this.getError('areCollectionsIdentical failed when array a' +
                        indexString + ' is not null and b' +
                        indexString + ' is null', message);
                }
                if (expected.length !== actual.length) {
                    indexString = resultToString(result);
                    throw _this.getError('areCollectionsIdentical failed when length of array a' +
                        indexString + ' (length: ' + expected.length + ') is different of length of array b' +
                        indexString + ' (length: ' + actual.length + ')', message);
                }
                for (var i = 0; i < expected.length; i++) {
                    if ((expected[i] instanceof Array) && (actual[i] instanceof Array)) {
                        result.push(i);
                        compareArray(expected[i], actual[i], result);
                        result.pop();
                    }
                    else if (expected[i] !== actual[i]) {
                        result.push(i);
                        indexString = resultToString(result);
                        throw _this.getError('areCollectionsIdentical failed when element a' +
                            indexString + ' (' + _this.printVariable(expected[i]) + ') is different than element b' +
                            indexString + ' (' + _this.printVariable(actual[i]) + ')', message);
                    }
                }
                return;
            };
            compareArray(expected, actual, []);
        };
        TestContext.prototype.areCollectionsNotIdentical = function (expected, actual, message) {
            if (message === void 0) { message = ''; }
            try {
                this.areCollectionsIdentical(expected, actual);
            }
            catch (ex) {
                return;
            }
            throw this.getError('areCollectionsNotIdentical failed when both collections are identical', message);
        };
        TestContext.prototype.isTrue = function (actual, message) {
            if (message === void 0) { message = ''; }
            if (!actual) {
                throw this.getError('isTrue failed when given ' + this.printVariable(actual), message);
            }
        };
        TestContext.prototype.isFalse = function (actual, message) {
            if (message === void 0) { message = ''; }
            if (actual) {
                throw this.getError('isFalse failed when given ' + this.printVariable(actual), message);
            }
        };
        TestContext.prototype.isTruthy = function (actual, message) {
            if (message === void 0) { message = ''; }
            if (!actual) {
                throw this.getError('isTrue failed when given ' + this.printVariable(actual), message);
            }
        };
        TestContext.prototype.isFalsey = function (actual, message) {
            if (message === void 0) { message = ''; }
            if (actual) {
                throw this.getError('isFalse failed when given ' + this.printVariable(actual), message);
            }
        };
        TestContext.prototype.throws = function (a, message, errorString) {
            if (message === void 0) { message = ''; }
            if (errorString === void 0) { errorString = ''; }
            var actual;
            if (a.fn) {
                actual = a.fn;
                message = a.message;
                errorString = a.exceptionString;
            }
            var isThrown = false;
            try {
                actual();
            }
            catch (ex) {
                if (!errorString || ex.message === errorString) {
                    isThrown = true;
                }
                if (errorString && ex.message !== errorString) {
                    throw this.getError('different error string than supplied');
                }
            }
            if (!isThrown) {
                throw this.getError('did not throw an error', message || '');
            }
        };
        TestContext.prototype.executesWithin = function (actual, timeLimit, message) {
            if (message === void 0) { message = null; }
            function getTime() {
                return window.performance.now();
            }
            function timeToString(value) {
                return Math.round(value * 100) / 100;
            }
            var startOfExecution = getTime();
            try {
                actual();
            }
            catch (ex) {
                throw this.getError('isExecuteTimeLessThanLimit fails when given code throws an exception: "' + ex + '"', message);
            }
            var executingTime = getTime() - startOfExecution;
            if (executingTime > timeLimit) {
                throw this.getError('isExecuteTimeLessThanLimit fails when execution time of given code (' + timeToString(executingTime) + ' ms) ' +
                    'exceed the given limit(' + timeToString(timeLimit) + ' ms)', message);
            }
        };
        TestContext.prototype.fail = function (message) {
            if (message === void 0) { message = ''; }
            throw this.getError('fail', message);
        };
        TestContext.prototype.getError = function (resultMessage, message) {
            if (message === void 0) { message = ''; }
            if (message) {
                return new Error(resultMessage + '. ' + message);
            }
            return new Error(resultMessage);
        };
        TestContext.getNameOfClass = function (inputClass) {
            // see: https://www.stevefenton.co.uk/Content/Blog/Date/201304/Blog/Obtaining-A-Class-Name-At-Runtime-In-TypeScript/
            var funcNameRegex = /function (.{1,})\(/;
            var results = (funcNameRegex).exec(inputClass.constructor.toString());
            return (results && results.length > 1) ? results[1] : '';
        };
        TestContext.prototype.printVariable = function (variable) {
            if (variable === null) {
                return '"null"';
            }
            if (typeof variable === 'object') {
                return '{object: ' + TestContext.getNameOfClass(variable) + '}';
            }
            return '{' + (typeof variable) + '} "' + variable + '"';
        };
        return TestContext;
    })();
    tsUnit.TestContext = TestContext;
    var TestClass = (function (_super) {
        __extends(TestClass, _super);
        function TestClass() {
            _super.apply(this, arguments);
        }
        TestClass.prototype.parameterizeUnitTest = function (method, parametersArray) {
            method.parameters = parametersArray;
        };
        return TestClass;
    })(TestContext);
    tsUnit.TestClass = TestClass;
    var FakeFactory = (function () {
        function FakeFactory() {
        }
        FakeFactory.getFake = function (obj) {
            var implementations = [];
            for (var _i = 1; _i < arguments.length; _i++) {
                implementations[_i - 1] = arguments[_i];
            }
            var fakeType = function () { };
            this.populateFakeType(fakeType, obj);
            var fake = new fakeType();
            for (var member in fake) {
                if (typeof fake[member] === 'function') {
                    fake[member] = function () { console.log('Default fake called.'); };
                }
            }
            var memberNameIndex = 0;
            var memberValueIndex = 1;
            for (var i = 0; i < implementations.length; i++) {
                var impl = implementations[i];
                fake[impl[memberNameIndex]] = impl[memberValueIndex];
            }
            return fake;
        };
        FakeFactory.populateFakeType = function (fake, toCopy) {
            for (var property in toCopy) {
                if (toCopy.hasOwnProperty(property)) {
                    fake[property] = toCopy[property];
                }
            }
            var __ = function () {
                this.constructor = fake;
            };
            __.prototype = toCopy.prototype;
            fake.prototype = new __();
        };
        return FakeFactory;
    })();
    tsUnit.FakeFactory = FakeFactory;
    var TestDefintion = (function () {
        function TestDefintion(testClass, name) {
            this.testClass = testClass;
            this.name = name;
        }
        return TestDefintion;
    })();
    var TestDescription = (function () {
        function TestDescription(testName, funcName, parameterSetNumber, message) {
            this.testName = testName;
            this.funcName = funcName;
            this.parameterSetNumber = parameterSetNumber;
            this.message = message;
        }
        return TestDescription;
    })();
    tsUnit.TestDescription = TestDescription;
    var TestResult = (function () {
        function TestResult() {
            this.passes = [];
            this.errors = [];
        }
        return TestResult;
    })();
    tsUnit.TestResult = TestResult;
})(tsUnit || (tsUnit = {}));
ko.as = function (value, evaluator, defaults) {
    if (!value)
        return defaults !== undefined ? defaults : null;
    value = ko.unwrap(value);
    if (!value)
        return defaults !== undefined ? defaults : null;
    if (!evaluator)
        return value;
    return evaluator(value);
};
ko.format = function (value, format, precision) { return DevExpress["formatHelper"].format(value, format, precision); };
ko.unwrap2 = function (obj) {
    if (!obj)
        return obj;
    obj = ko.unwrap(obj);
    var r = {};
    var propCount = 0;
    for (var name in obj) {
        if (!obj.hasOwnProperty(name))
            continue;
        r[name] = ko.unwrap(obj[name]);
        propCount++;
    }
    return propCount > 0 ? r : obj;
};
ko.bindingHandlers.renderer =
    {
        update: function (element, valueAccessor, allBindings) {
            var value = ko.unwrap(valueAccessor());
            if (!value)
                return;
            element = $(element);
            element.html("");
            if ($.isFunction(value))
                value(element);
            else {
                for (var containerId in value) {
                    if (!value.hasOwnProperty(containerId))
                        continue;
                    var container = value[containerId];
                    if ($.isFunction(value))
                        container(element);
                    else if (container.renderer)
                        container.renderer(element);
                }
            }
        }
    };
ko.bindingHandlers.buttonsCol =
    {
        update: function (element, valueAccessor, allBindings) {
            var buttons = ko.unwrap(valueAccessor());
            if (!buttons)
                return;
            var jelement = $(element);
            jelement.html("");
            buttons.forEach(function (btn) {
                $("<div class=\"smart-action-button\">")
                    .dxButton(btn)
                    .appendTo(jelement);
            });
        }
    };
ko.bindingHandlers.pre =
    {
        update: function (element, valueAccessor, allBindings) {
            var value = ko.unwrap(valueAccessor());
            if (!value)
                return;
            $(element).append($("<pre>").text(value));
        }
    };
var $log;
var $log_;
var $error;
//var $logt: (...args: Array<any>) => any;
var $logb;
var $loge;
var $log_printStack = false;
//var $log_printStack = true;
var $trace = function () {
    console.groupCollapsed();
    console.trace();
    console.groupEnd();
};
var $alert = function (message) {
    $log(message);
    var msg = message;
    if (typeof msg === "object")
        msg = JSON.stringify(message);
    alert(msg);
    return message;
};
var Luxena;
(function (Luxena) {
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
    $error = $log = $log_ = function () {
        var args = [];
        for (var _i = 0; _i < arguments.length; _i++) {
            args[_i - 0] = arguments[_i];
        }
        var console = window.console;
        if (!console || !console.log)
            return undefined;
        var line = (++lineNo).toString();
        while (line.length < 4)
            line = ' ' + line;
        var indent2 = "        " + indent;
        var msg = [line + "    " + indent];
        msg.push.apply(msg, args);
        if ($log_printStack) {
            var stack;
            try {
                throw new Error();
            }
            catch (ex) {
                stack = ex.stack.split("\n");
            }
            stack.splice(0, 1);
            for (var i = 0, len = stack.length; i < len; i++) {
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
        else {
            console.log.apply(console, msg);
        }
        return args[0];
    };
    $logb = function (message, action) {
        $log((message || "") + "{");
        incIndent();
        if (action) {
            var result = action();
            $loge();
            return result;
        }
    };
    $loge = function () {
        decIndent();
        $log("}");
    };
})(Luxena || (Luxena = {}));
//module Luxena.layouts
//{
//
//	DX.framework.html.SimpleLayoutController = DX.framework.html.DefaultLayoutController.inherit({
//		ctor(options)
//		{
//			options = options || {};
//			options.name = options.name || "simple";
//			this.callBase(options);
//		}
//	});
//
//
//	var layoutSets = DX.framework.html.layoutSets;
//	layoutSets["simple"] = layoutSets["simple"] || [];
//	layoutSets["simple"].push({
//		controller: new DX.framework.html.SimpleLayoutController
//	});
//
//} 
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
var Luxena;
(function (Luxena) {
    var SemanticType = (function () {
        function SemanticType() {
            this.isComplex = false;
            this.allowFiltering = true;
            this.allowGrouping = true;
            this.dataType = "string";
            this.nullable = true;
        }
        SemanticType.prototype.getSelectFieldNames = function (sf) {
            return [sf._name];
        };
        SemanticType.prototype.getExpandFieldNames = function (sf) {
            return [];
        };
        SemanticType.prototype.getFilterExpr = function (sm, value, operation) {
            return [sm._name, operation || "=", value];
        };
        //#region Controls
        SemanticType.prototype.toGridColumns = function (sf) {
            return [this.toStdGridColumn(sf)];
        };
        SemanticType.prototype.toStdGridColumn = function (sf) {
            var _this = this;
            var sm = sf.member;
            var se = sf._entity;
            var cfg = sf._controller.config;
            return {
                allowFiltering: sm._allowFiltering && this.allowFiltering && !sm._isCalculated,
                allowGrouping: sm._allowGrouping && this.allowGrouping && !sm._isCalculated,
                allowSorting: !sm._isCalculated,
                caption: sm._title,
                dataField: sm._name,
                dataType: this.dataType,
                fixed: sm._columnFixed || (cfg.wide || se._isWide) && sm._kind === Luxena.SemanticMemberKind.Primary,
                format: sm._format || this.format,
                sortOrder: sf.sortOrder || sm._sortOrder,
                width: this.getColumnWidth(sf),
                visible: sm._columnVisible,
                calculateFilterExpression: function (value, operation) { return _this.getFilterExpr(sm, value, operation); },
            };
        };
        SemanticType.prototype.toGridTotalItems = function (sf) {
            return [];
        };
        SemanticType.prototype.getLength = function (sf) {
            var sm = sf.member;
            var length = sm._length || sm._maxLength;
            var minLength = sm._minLength;
            var maxLength = sm._maxLength;
            if (!length) {
                var refs = sm.getReference();
                if (refs) {
                    if (!refs._nameMember) {
                        console.log(sm);
                        throw Error("SemanticMember._nameMember is null");
                    }
                    length = refs._nameMember._length;
                    minLength = refs._nameMember._minLength;
                    maxLength = refs._nameMember._maxLength;
                }
            }
            return {
                length: length || sm._length || this.length,
                min: minLength || sm._minLength,
                max: maxLength || sm._maxLength,
            };
        };
        SemanticType.prototype.getColumnWidth = function (sf, length) {
            var sm = sf.member;
            if (!length)
                length = this.getLength(sf).length;
            if (length < 2)
                length = 2;
            var cfg = sf._controller.config;
            return sm._width || (14
                + Math.round((this.charWidth || SemanticType.charWidth) * length)
                + (cfg.useFilter && cfg.useFilterRow && this.allowFiltering && sm._allowFiltering ? 12 + (this.addColumnFilterWidth || 0) : 0));
        };
        //#endregion
        SemanticType.prototype.getFromData = function (sm, data) {
            return data[sm._name];
        };
        SemanticType.prototype.getModel = function (model, name) {
            return model[name];
        };
        SemanticType.prototype.setModel = function (model, sname, value) {
            var name, sf = null;
            if (typeof sname === "string") {
                name = sname;
            }
            else {
                sf = sname;
                name = sf._name;
            }
            var existsValue = model[name];
            if (sf && existsValue && value === undefined && sf._controller.modelIsExternal) {
                return;
            }
            if (value === undefined)
                value = null;
            if (!ko.isObservable(existsValue))
                model[name] = ko.observable(value);
            else
                existsValue(value);
        };
        SemanticType.prototype.loadFromData = function (sf, model, data) {
            var value = data[sf._name];
            this.setModel(model, sf, value);
        };
        SemanticType.prototype.saveToData = function (sf, model, data) {
            var name = sf._name;
            data[name] = ko.unwrap(model[name]);
        };
        SemanticType.prototype.removeFromData = function (sf, data) {
            delete data[sf["name"] || sf["_name"]];
        };
        SemanticType.prototype.getFieldLabel = function (sf) {
            return sf.member._title + ":";
        };
        SemanticType.prototype.render = function (sf, valueEl, rowEl) {
            if (sf._controller.editMode && !sf.member._isReadOnly)
                this.renderEditor(sf, valueEl, rowEl);
            else
                this.renderDisplay(sf, valueEl, rowEl);
        };
        SemanticType.prototype.renderDisplay = function (sf, valueEl, rowEl) {
            if (!sf._controller.editMode)
                rowEl.attr("data-bind", "visible: r." + sf._name);
            valueEl.attr("data-bind", "text: r." + sf._name);
        };
        SemanticType.prototype.renderEditor = function (sf, valueEl, rowEl) {
            this.renderDisplay(sf, valueEl, rowEl);
        };
        SemanticType.prototype.appendEditor = function (sf, valueEl, widgetClassName, options) {
            var ctrl = sf._controller;
            var sm = sf.member;
            var editorEl = $("<div>");
            editorEl[0]["sf"] = sf;
            options["hint"] = sm._title + (!sm._description ? "" : "\r\n" + sm._description);
            if (sm._isSubject)
                options.onValueChanged = function () { return ctrl.recalc(sf._name); };
            ctrl.widgets[sf._name] = options;
            var bindAttr = widgetClassName + ": $root.widgets." + sf._name;
            var rules = this.getValidationRules(sf);
            if (rules && rules.length) {
                ctrl.validators[sf._name] =
                    {
                        validationGroup: "edit-form",
                        validationRules: rules
                    };
                bindAttr += ", dxValidator: $root.validators." + sf._name;
            }
            editorEl
                .attr("data-bind", bindAttr)
                .appendTo(valueEl);
        };
        SemanticType.prototype.appendTextBoxEditor = function (sf, valueEl, widgetClassName, options) {
            var sm = sf.member;
            if (sf._controller.filterMode)
                options.mode = "search";
            if (sf._rowMode)
                options.placeholder = sm._shortTitle || sm._title;
            //options.maxLength = sm._maxLength || undefined;
            this.appendEditor(sf, valueEl, widgetClassName, options);
        };
        SemanticType.prototype.getValidationRules = function (sf) {
            var sm = sf.member;
            //var se = sf.entity;
            var rules = [];
            if (this.nullable && sm._required && !sf._controller.filterMode) {
                rules.push({
                    type: "required"
                });
            }
            if (sm._unique) {
                rules.push({
                    type: "custom",
                    message: "Уже существует",
                    validationCallback: Luxena.Validators.uniqueValidator,
                });
            }
            if ((sm._minLength || sm._maxLength) && !sm._enumType) {
                var msg = "Длина поля \"" + sm._title + "\" должна быть " +
                    (sm._minLength === sm._maxLength
                        ? "равна " + sm._minLength
                        : ko.as(sm._minLength, function (a) { return " от " + a; }, "") + ko.as(sm._maxLength, function (a) { return " до " + a; }, ""))
                    + ".";
                rules.push({
                    //type: "stringLength",
                    type: "custom",
                    min: sm._minLength || undefined,
                    max: sm._maxLength || undefined,
                    message: msg,
                    validationCallback: Luxena.Validators.stringLength,
                });
            }
            return rules;
        };
        SemanticType.charWidth = 10;
        SemanticType.digitWidth = 7.6;
        return SemanticType;
    })();
    Luxena.SemanticType = SemanticType;
    function getTextIconHtml(icon) {
        return !icon ? "" : "<i class=\"fa fa-" + icon + " fa-lg\" /> &nbsp;";
    }
    Luxena.getTextIconHtml = getTextIconHtml;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var BoolSemanticType = (function (_super) {
        __extends(BoolSemanticType, _super);
        function BoolSemanticType() {
            _super.apply(this, arguments);
            this.dataType = "boolean";
            this.nullable = false;
        }
        BoolSemanticType.prototype.loadFromData = function (sf, model, data) {
            this.setModel(model, sf, !!data[sf._name]);
        };
        BoolSemanticType.prototype.getFieldLabel = function (sf) {
            return "";
        };
        BoolSemanticType.prototype.renderDisplay = function (sf, valueEl, rowEl) {
            if (!sf._controller.editMode)
                rowEl.attr("data-bind", "visible: r." + sf._name);
            valueEl.append($("<div data-bind=\"dxCheckBox: { value: r." + sf._name + ", text: '" + sf.member._title + "', readOnly: true, }\"></div>"));
        };
        BoolSemanticType.prototype.renderEditor = function (sf, valueEl, rowEl) {
            this.appendEditor(sf, valueEl, "dxCheckBox", {
                value: sf.getModelValue(),
                text: sf.member._title,
            });
        };
        BoolSemanticType.Bool = new BoolSemanticType();
        return BoolSemanticType;
    })(Luxena.SemanticType);
    Luxena.BoolSemanticType = BoolSemanticType;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var DateSemanticType = (function (_super) {
        __extends(DateSemanticType, _super);
        function DateSemanticType(format, length) {
            _super.call(this);
            this.format = format;
            this.length = length;
            this.charWidth = Luxena.SemanticType.digitWidth;
            this.addColumnFilterWidth = length <= 10 ? 42 : 0;
            this.dataType = "date";
            this.chartDataType = "datetime";
        }
        DateSemanticType.prototype.loadFromData = function (sf, model, data) {
            var value = data[sf._name];
            if (typeof value === "string") {
                value = value.replace(/T.+/, "");
                value = new Date(value);
            }
            this.setModel(model, sf, value);
        };
        DateSemanticType.prototype.toGridColumns = function (sf) {
            var se = sf._entity;
            var sm = sf.member;
            var col = this.toStdGridColumn(sf);
            if (se._isBig && sm._isEntityDate)
                col.groupIndex = 0;
            return [col];
        };
        DateSemanticType.prototype.renderDisplay = function (sf, valueEl, rowEl) {
            if (!sf._controller.editMode)
                rowEl.attr("data-bind", "visible: r." + sf._name);
            var ref = sf.member.getReference();
            valueEl.attr("data-bind", "text: Globalize.format(r." + sf._name + "(), '" + this.format + "')");
        };
        DateSemanticType.prototype.renderEditor = function (sf, valueEl, rowEl) {
            valueEl = $("<div>").appendTo(valueEl);
            var options = {
                value: sf.getModelValue(),
                format: "date",
                formatString: this.format,
                showClearButton: !sf.member._required || sf._controller.filterMode,
            };
            if (this.format === "monthAndYear") {
                options.formatString = "MMMM yyyy";
                options.maxZoomLevel = "year";
                options.minZoomLevel = "year";
            }
            this.appendEditor(sf, valueEl, "dxDateBox", options);
        };
        DateSemanticType.Date = new DateSemanticType("dd.MM.yyyy", 10);
        DateSemanticType.MonthAndYear = new DateSemanticType("monthAndYear", 10);
        DateSemanticType.QuarterAndYear = new DateSemanticType("quarterAndYear", 10);
        DateSemanticType.Year = new DateSemanticType("dd.MM.yyyy", 10);
        DateSemanticType.DateTime = new DateSemanticType("dd.MM.yyyy H:mm", 15);
        DateSemanticType.DateTime2 = new DateSemanticType("dd.MM.yyyy H:mm:ss", 18);
        DateSemanticType.Time = new DateSemanticType("H:mm", 5);
        DateSemanticType.Time2 = new DateSemanticType("H:mm:ss", 8);
        return DateSemanticType;
    })(Luxena.SemanticType);
    Luxena.DateSemanticType = DateSemanticType;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var EnumSemanticType = (function (_super) {
        __extends(EnumSemanticType, _super);
        function EnumSemanticType() {
            _super.apply(this, arguments);
            this.allowGrouping = false;
            this.addColumnFilterWidth = -8;
        }
        EnumSemanticType.prototype.getFilterExpr = function (sm, value, operation) {
            if ($.isArray(value)) {
                var filter = [];
                value.forEach(function (a) {
                    filter.push([sm._name, operation || "=", sm._enumType._getEdm(a)]);
                    filter.push("or");
                });
                filter.pop();
                if (!filter.length)
                    filter = undefined;
                return filter;
            }
            else
                return [sm._name, operation || "=", sm._enumType._getEdm(value)];
        };
        EnumSemanticType.prototype.loadFromData = function (sf, model, data) {
            var sm = sf.member;
            if (!sf._controller.editMode || !sm._enumIsFlags) {
                var value = data[sf._name];
                if (sf._controller.editMode && sm._required && !value)
                    value = sm._enumType._array[0].Id;
                this.setModel(model, sf, value);
            }
            else {
                var values = data[sf._name];
                values = !values ? [] : values.split(",").map(function (a) { return a.trim(); });
                this.setModel(model, sf, values);
            }
        };
        EnumSemanticType.prototype.saveToData = function (sf, model, data) {
            if (!sf.member._enumIsFlags) {
                _super.prototype.saveToData.call(this, sf, model, data);
                return;
            }
            var values = ko.unwrap(model[sf._name]);
            data[sf._name] = values.join(", ");
        };
        EnumSemanticType.prototype.toGridColumns = function (sf) {
            var sm = sf.member;
            var col = this.toStdGridColumn(sf);
            if (sm._enumIsFlags) {
                col.allowFiltering = false;
                col.allowGrouping = false;
                col.allowSorting = false;
                col.calculateCellValue = function (data) { return getEnumNames(sm._enumType, data[sm._name]); };
            }
            else {
                col.lookup = {
                    dataSource: sm._enumList || sm._enumType._array,
                    valueExpr: "Id",
                    displayExpr: "Name",
                    allowClearing: sm._required,
                };
                col.cellTemplate = function (cell, cellInfo) {
                    return cell.html(getEnumNames(sm._enumType, cellInfo.value));
                };
                //col.calculateCellValue = data => getEnumNames(sm._enumType, data[sm._name]);
                col.calculateGroupValue = function (data) { return sm._enumType._getEdm(data[sm._name]); };
                col.groupCellTemplate = function (cell, cellInfo) {
                    return cell.html(sm._title + ": &nbsp; " + getEnumNames(sm._enumType, cellInfo.value));
                };
            }
            return [col];
        };
        EnumSemanticType.prototype.renderDisplay = function (sf, valueEl, rowEl) {
            if (!sf._controller.editMode)
                rowEl.attr("data-bind", "visible: r." + sf._name);
            valueEl.attr("data-bind", "html: Luxena.getEnumNames(Luxena." + sf.member._enumType._name + ", r." + sf._name + ")");
        };
        EnumSemanticType.prototype.renderEditor = function (sf, valueEl, rowEl) {
            var sm = sf.member;
            if (sm._enumIsFlags) {
                this.appendTextBoxEditor(sf, valueEl, "dxTagBox", {
                    values: sf.getModelValue(),
                    items: sm._enumType._array,
                    valueExpr: "Id",
                    displayExpr: "Name",
                    showClearButton: !sm._required,
                });
            }
            else {
                this.appendTextBoxEditor(sf, valueEl, "dxSelectBox", {
                    value: sf.getModelValue(),
                    dataSource: sm._enumType._array,
                    valueExpr: "Id",
                    displayExpr: "Name",
                    showClearButton: !sm._required,
                });
            }
        };
        EnumSemanticType.Enum = new EnumSemanticType();
        return EnumSemanticType;
    })(Luxena.SemanticType);
    Luxena.EnumSemanticType = EnumSemanticType;
    function getEnumNames(enumType, values) {
        if (!values)
            return "";
        if ($.isFunction(values))
            values = values();
        if (!values)
            return "";
        if (typeof values === "string")
            values = values.split(",");
        var names = [];
        for (var i = 0, value; value = values[i++];) {
            value = value.trim();
            var item = enumType._items[value];
            if (item)
                names.push(item.TextIconHtml + item.Name);
            else
                names.push(value);
        }
        return names.join(", ") || "";
    }
    Luxena.getEnumNames = getEnumNames;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var CurrencyCodeSemanticType = (function (_super) {
        __extends(CurrencyCodeSemanticType, _super);
        function CurrencyCodeSemanticType() {
            _super.apply(this, arguments);
        }
        CurrencyCodeSemanticType.prototype.renderEditor = function (sf, valueEl, rowEl) {
            this.appendEditor(sf, valueEl, "dxSelectBox", {
                value: sf.getModelValue(),
                dataSource: CurrencyCodeSemanticType.Codes,
            });
        };
        CurrencyCodeSemanticType.CurrencyCode = new CurrencyCodeSemanticType();
        CurrencyCodeSemanticType.Codes = ["UAH", "USD", "EUR", "RUB", "AED", "AFN", "ALL", "AMD", "ANG", "AOA", "ARS", "AUD", "AUH", "AWG", "AZN", "BAM", "BBD", "BDT", "BGN", "BHD", "BIF", "BMD", "BND", "BOB", "BOV", "BRL", "BSD", "BTN", "BWP", "BYR", "BZD", "CAD", "CDF", "CHE", "CHF", "CHW", "CLF", "CLP", "CNY", "COP", "COU", "CRC", "CUC", "CUP", "CVE", "CZK", "DJF", "DKK", "DOP", "DZD", "EEK", "EGP", "ERN", "ETB", "FJD", "FKP", "GBP", "GEL", "GHC", "GHS", "GIP", "GMD", "GNF", "GTQ", "GWP", "GYD", "HKD", "HNL", "HRK", "HTG", "HUF", "IDR", "ILS", "INR", "IQD", "IRR", "ISK", "IUA", "JMD", "JOD", "JPY", "KES", "KGS", "KHR", "KMF", "KPW", "KRW", "KWD", "KYD", "KZT", "LAK", "LBP", "LKR", "LRD", "LSL", "LTL", "LVL", "LYD", "MAD", "MDL", "MGA", "MKD", "MMK", "MNT", "MOP", "MRO", "MTL", "MUR", "MVR", "MWK", "MXN", "MXV", "MYR", "MZM", "MZN", "NAD", "NGN", "NIO", "NOK", "NPR", "NUC", "NZD", "OMR", "PAB", "PEN", "PGK", "PHP", "PKR", "PLN", "PYG", "QAR", "ROL", "RON", "RSD", "RWF", "SAR", "SBD", "SCR", "SDG", "SEK", "SGD", "SHP", "SIT", "SKK", "SLL", "SOS", "SRD", "STD", "SVC", "SYP", "SZL", "THB", "TJS", "TMT", "TND", "TOP", "TRY", "TTD", "TWD", "TZS", "UGX", "USN", "USS", "UYI", "UYU", "UZS", "VEF", "VND", "VUV", "WST", "XAF", "XAG", "XAU", "XBA", "XBB", "XBC", "XBD", "XCD", "XDR", "XOF", "XPD", "XPF", "XPT", "XTS", "XXX", "YER", "ZAR", "ZMK", "ZWD", "ZWL",];
        return CurrencyCodeSemanticType;
    })(Luxena.SemanticType);
    Luxena.CurrencyCodeSemanticType = CurrencyCodeSemanticType;
    var MoneySemanticType = (function (_super) {
        __extends(MoneySemanticType, _super);
        function MoneySemanticType() {
            _super.apply(this, arguments);
            this.isComplex = true;
            this.dataType = "number";
            this.length = 12;
            this.allowGrouping = false;
        }
        MoneySemanticType.prototype.getFilterExpr = function (sm, value, operation) {
            return [sm._name + ".Amount", operation || "=", value];
        };
        MoneySemanticType.prototype.loadFromData = function (sf, model, data) {
            var name = sf._name;
            var newValue = data[name];
            var value = model[name];
            if (value) {
                value.Amount(newValue && newValue.Amount);
                value.CurrencyId(newValue && newValue.CurrencyId);
            }
            else {
                model[name] = {
                    Amount: ko.observable(newValue && newValue.Amount),
                    CurrencyId: ko.observable(newValue && newValue.CurrencyId),
                };
            }
        };
        MoneySemanticType.prototype.saveToData = function (sf, model, data) {
            var value = model[sf._name];
            data[sf._name] = {
                Amount: ko.unwrap(value.Amount),
                CurrencyId: ko.unwrap(value.CurrencyId),
            };
        };
        MoneySemanticType.prototype.toGridColumns = function (sf) {
            var sm = sf.member;
            var col = this.toStdGridColumn(sf);
            col.dataField += ".Amount";
            col.calculateCellValue = function (data) { return moneyToString(data[sm._name]); };
            return [col];
        };
        MoneySemanticType.prototype.renderDisplay = function (sf, valueEl, rowEl) {
            if (!sf._controller.editMode)
                rowEl.attr("data-bind", "visible: r." + sf._name + " && r." + sf._name + ".Amount()");
            $("<div>")
                .addClass("money-display-row")
                .attr("data-bind", "text: Luxena.moneyToString(r." + sf._name + ")")
                .appendTo(valueEl);
        };
        MoneySemanticType.prototype.renderEditor = function (sf, valueEl, rowEl) {
            var sm = sf.member;
            if (sm._isCalculated || sm._isReadOnly) {
                if (valueEl.hasClass("dx-field-value")) {
                    valueEl.removeClass("dx-field-value");
                    valueEl.addClass("dx-field-value-static");
                }
                this.renderDisplay(sf, valueEl, rowEl);
                return;
            }
            var valueBox = $("<div>")
                .addClass("money-editor-row")
                .appendTo(valueEl);
            var amountDiv = $("<div>")
                .appendTo(valueBox);
            var currencyDiv = $("<div>")
                .appendTo(valueBox);
            var value = sf.getModelValue();
            var sf2 = $.extend({}, sf);
            sf2.name = sf._name + "_amount";
            this.appendEditor(sf2, amountDiv, "dxNumberBox", {
                value: value.Amount,
            });
            sf2.name = sf._name + "_currency";
            this.appendEditor(sf, currencyDiv, "dxSelectBox", {
                value: value.CurrencyId,
                dataSource: CurrencyCodeSemanticType.Codes,
            });
        };
        MoneySemanticType.Money = new MoneySemanticType();
        return MoneySemanticType;
    })(Luxena.SemanticType);
    Luxena.MoneySemanticType = MoneySemanticType;
    function moneyToString(v) {
        if (!v)
            return "";
        var amount = v.Amount && ko.unwrap(v.Amount);
        return !amount /*&& amount !== 0*/ ? "" : Globalize.format(amount, "n2") + " " + ko.unwrap(v.CurrencyId);
    }
    Luxena.moneyToString = moneyToString;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var NumericSemanticType = (function (_super) {
        __extends(NumericSemanticType, _super);
        function NumericSemanticType() {
            _super.apply(this, arguments);
            this.allowGrouping = false;
            this.length = 10;
            this.dataType = "number";
        }
        NumericSemanticType.prototype.toGridColumns = function (sf) {
            var sm = sf.member;
            var col = this.toStdGridColumn(sf);
            col.calculateCellValue = function (r) { return r[sm._name] || null; };
            if (sm._precision) {
                col.format = "n" + (sm._precision || "");
                col.precision = sm._precision;
            }
            return [col];
        };
        NumericSemanticType.prototype.toGridTotalItems = function (sf) {
            var sm = sf.member;
            if (!sm._useTotalSum)
                return [];
            return [{
                    column: sm._name,
                    summaryType: "sum",
                    displayFormat: "{0}",
                    valueFormat: "n" + (sm._precision || 0),
                }];
        };
        NumericSemanticType.prototype.renderDisplay = function (sf, valueEl, rowEl) {
            var sm = sf.member;
            if (!sf._controller.editMode)
                rowEl.attr("data-bind", "visible: r." + sf._name);
            $("<div style='max-width: 98px'></div>")
                .css("text-align", "right")
                .attr("data-bind", "text: Globalize.format(r." + sf._name + "(), 'n" + (sm._precision || "") + "')")
                .appendTo(valueEl);
        };
        NumericSemanticType.prototype.renderEditor = function (sf, valueEl, rowEl) {
            valueEl = $("<div style='max-width: 164px'></div>")
                .appendTo(valueEl);
            this.appendTextBoxEditor(sf, valueEl, "dxNumberBox", {
                value: sf.getModelValue(),
            });
        };
        NumericSemanticType.Float = new NumericSemanticType();
        NumericSemanticType.Int = new NumericSemanticType();
        NumericSemanticType.Percent = new NumericSemanticType();
        return NumericSemanticType;
    })(Luxena.SemanticType);
    Luxena.NumericSemanticType = NumericSemanticType;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var LookupSemanticType = (function (_super) {
        __extends(LookupSemanticType, _super);
        function LookupSemanticType() {
            _super.apply(this, arguments);
            this.isComplex = true;
        }
        LookupSemanticType.prototype.getSelectFieldNames = function (sf) {
            return sf._controller.editMode
                ? [sf._name + "Id"]
                : [];
        };
        LookupSemanticType.prototype.getExpandFieldNames = function (sf) {
            var sm = sf.member;
            var ref = sm.getReference();
            var refs = ref._referenceFields;
            if (sf._controller.editMode)
                return [];
            else
                return [(sm._name + "($select=" + refs.id + "," + refs.name + ")")];
        };
        LookupSemanticType.prototype.getFilterExpr = function (sm, value, operation) {
            return [sm._name + "Id", "=", value];
        };
        LookupSemanticType.prototype.loadFromData = function (sf, model, data) {
            var name = sf._name;
            var value = {};
            if (sf._controller.editMode) {
                var id = data[name + "Id"] || data[name];
                if (id && id.Id)
                    id = id.Id;
                value = id;
            }
            else {
                var newValue = data[name];
                for (var prop in newValue) {
                    if (!newValue.hasOwnProperty(prop))
                        continue;
                    value[prop] = ko.observable(newValue[prop]);
                }
            }
            this.setModel(model, sf, value);
        };
        LookupSemanticType.prototype.saveToData = function (sf, model, data) {
            var id = ko.unwrap(model[sf._name]);
            data[sf._name + "Id"] = id || null;
        };
        LookupSemanticType.prototype.removeFromData = function (sf, data) {
            delete data[sf._name];
            delete data[sf._name + "Id"];
        };
        LookupSemanticType.prototype.toGridColumns = function (sf) {
            var sm = sf.member;
            var col = this.toStdGridColumn(sf);
            var ref = sm.getReference();
            var refs = ref._referenceFields;
            col.dataField += "." + refs.name;
            col.calculateFilterExpression = function (filterValue, selectedFilterOperation) {
                return [sm._name + "." + refs.name, selectedFilterOperation || "contains", filterValue];
            };
            col.cellTemplate = function (cell, cellInfo) {
                if (cellInfo.column.groupIndex !== undefined)
                    return;
                renderReferenceDisplay(sf, cellInfo.data, cell, ref);
            };
            col.groupCellTemplate = function (cell, cellInfo) {
                if (cellInfo.data.items && cellInfo.data.items.length) {
                    var v = cellInfo.data.items[0][sf._name];
                    if (v && v[refs.id] && v[refs.name]) {
                        cell.html(sm._title + ": <a href='#" + ref._name + "/" + v[refs.id] + "'>" + v[refs.name] + "</a>");
                        return;
                    }
                }
                cell.html(sm._title + ": " + cellInfo.text);
            };
            return [col];
        };
        LookupSemanticType.prototype.renderDisplay = function (sf, valueEl, rowEl) {
            if (!sf._controller.editMode)
                rowEl.attr("data-bind", "visible: r." + sf._name + " && r." + sf._name + "().Id");
            var span = $("<span>")
                .attr("data-bind", "renderer: Luxena.referenceDisplayRendererByData($element.sf, $data.r)")
                .appendTo(valueEl);
            span[0]["sf"] = sf;
        };
        LookupSemanticType.prototype.renderEditor = function (sf, valueEl, rowEl) {
            var sm = sf.member;
            var ref = sm.getReference();
            var refs = ref._referenceFields;
            var defaultIconHtml = ref._textIconHtml;
            var itemTemplate;
            if (ref._lookupItemTemplate) {
                itemTemplate = function (data, index, container) {
                    var result = ref._lookupItemTemplate(data, index, container);
                    if (result) {
                        if (typeof result === "string" && result.indexOf("<") >= 0)
                            container.html(result);
                        else
                            return result;
                    }
                };
            }
            else {
                itemTemplate = function (r, index, itemContainer) { return itemContainer.html(r._iconHtml + r[refs.name]); };
            }
            var options = {
                value: sf.getModelValue(),
                dataSource: {
                    store: ref._lookupStore || ref._store,
                    sort: refs.name,
                    select: [refs.id, refs.name],
                    map: function (r) {
                        var ref2 = Luxena.sd.entityByOData(r, ref);
                        r._iconHtml = (ref2 ? ref2._textIconHtml : null) || defaultIconHtml;
                        return r;
                    }
                },
                valueExpr: refs.id,
                displayExpr: refs.name,
                showClearButton: !sm._required,
                itemTemplate: itemTemplate,
            };
            if (ref._isSmall) {
                this.appendTextBoxEditor(sf, valueEl, "dxSelectBox", options);
            }
            else {
                options.dataSource.paginate = true;
                options.title = sm._title;
                options.showPopupTitle = false;
                options.cleanSearchOnOpening = false;
                this.appendTextBoxEditor(sf, valueEl, "dxLookup", options);
            }
        };
        LookupSemanticType.Reference = new LookupSemanticType();
        return LookupSemanticType;
    })(Luxena.SemanticType);
    Luxena.LookupSemanticType = LookupSemanticType;
    function renderReferenceDisplay(sf, data, container, ref) {
        if (!data)
            return;
        var v = ko.unwrap(data[sf._name]);
        if (!v)
            return;
        ref = ref || sf.member.getReference();
        if (!ref)
            return;
        var ref2 = Luxena.sd.entityByOData(v, ref);
        var refs = ref2._referenceFields;
        var id = ko.unwrap(v[refs.id]);
        var name = ko.unwrap(v[refs.name]);
        if (!id && !name)
            return;
        var iconHtml = (ref2 ? ref2._textIconHtml : null) || ref._textIconHtml;
        if (!id) {
            container.html(iconHtml + name);
        }
        else if (sf._controller.smartMode) {
            container.html("<span>" + iconHtml + "<a href=\"#" + ref2._name + "/" + id + "\" class=\"dx-link\">" + name + "</a></span>");
        }
        else {
            var span = $("<span>" + iconHtml + "<a href=\"#" + ref2._name + "/" + id + "\" class=\"dx-link\">" + name + "</a></span>");
            span.click(function (e) {
                e.preventDefault();
                ref2.toggleSmart(span, { id: id });
            });
            span.appendTo(container);
        }
    }
    Luxena.renderReferenceDisplay = renderReferenceDisplay;
    function referenceDisplayRendererByData(sf, data) {
        return function (container) { return renderReferenceDisplay(sf, data, container); };
    }
    Luxena.referenceDisplayRendererByData = referenceDisplayRendererByData;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var TextAreaSemanticType = (function (_super) {
        __extends(TextAreaSemanticType, _super);
        function TextAreaSemanticType() {
            _super.apply(this, arguments);
        }
        TextAreaSemanticType.prototype.renderEditor = function (sf, valueEl, rowEl) {
            var sm = sf.member;
            this.appendTextBoxEditor(sf, valueEl, "dxTextArea", {
                value: sf.getModelValue(),
                height: sm._lineCount ? (sm._lineCount * 46 - 10) : undefined,
            });
        };
        return TextAreaSemanticType;
    })(Luxena.SemanticType);
    Luxena.TextAreaSemanticType = TextAreaSemanticType;
    var CodeTextAreaSemanticType = (function (_super) {
        __extends(CodeTextAreaSemanticType, _super);
        function CodeTextAreaSemanticType() {
            _super.apply(this, arguments);
        }
        CodeTextAreaSemanticType.prototype.renderDisplay = function (sf, valueEl, rowEl) {
            if (!sf._controller.editMode)
                rowEl.attr("data-bind", "visible: r." + sf._name);
            valueEl.attr("data-bind", "html: \"<pre>\" + r." + sf._name + "() + \"</pre>\"");
        };
        CodeTextAreaSemanticType.prototype.renderEditor = function (sf, valueEl, rowEl) {
            var sm = sf.member;
            this.appendTextBoxEditor(sf, valueEl, "dxTextArea", {
                value: sf.getModelValue(),
                height: sm._lineCount ? (sm._lineCount * 64 - 16 - 12) : undefined,
            });
        };
        return CodeTextAreaSemanticType;
    })(TextAreaSemanticType);
    Luxena.CodeTextAreaSemanticType = CodeTextAreaSemanticType;
    var PasswordSemanticType = (function (_super) {
        __extends(PasswordSemanticType, _super);
        function PasswordSemanticType() {
            _super.apply(this, arguments);
        }
        PasswordSemanticType.prototype.renderEditor = function (sf, valueEl, rowEl) {
            this.appendTextBoxEditor(sf, valueEl, "dxTextBox", {
                value: sf.getModelValue(),
                maxLength: 20,
                mode: "password",
            });
        };
        return PasswordSemanticType;
    })(Luxena.SemanticType);
    Luxena.PasswordSemanticType = PasswordSemanticType;
    var TextSemanticType = (function (_super) {
        __extends(TextSemanticType, _super);
        function TextSemanticType() {
            _super.apply(this, arguments);
        }
        TextSemanticType.prototype.getFilterExpr = function (sm, value, operation) {
            return [sm._name, operation || "contains", value];
        };
        TextSemanticType.prototype.toGridColumns = function (sf) {
            var sm = sf.member;
            var se = sm._entity;
            var col = this.toStdGridColumn(sf);
            if (sm._isEntityName) {
                //var cellIconHtml = getCellIconHtml(se._icon);
                col.cellTemplate = function (cell, cellInfo) {
                    if (cellInfo.column.groupIndex !== undefined)
                        return;
                    var data = cellInfo.data;
                    var v = data[sf._name];
                    if (!v)
                        return;
                    var id = data[se._referenceFields.id];
                    $("<a class=\"dx-link\" href=\"#" + data._viewEntity._name + "/" + id + "\"><b>" + v + "</b></a>").appendTo(cell);
                };
            }
            return [col];
        };
        TextSemanticType.prototype.renderDisplay = function (sf, valueEl, rowEl) {
            if (!sf._controller.editMode)
                rowEl.attr("data-bind", "visible: r." + sf._name);
            valueEl.addClass("pre");
            valueEl.attr("data-bind", "text: r." + sf._name);
        };
        TextSemanticType.prototype.renderEditor = function (sf, valueEl, rowEl) {
            this.appendTextBoxEditor(sf, valueEl, "dxTextBox", {
                value: sf.getModelValue(),
            });
        };
        TextSemanticType.String = new TextSemanticType();
        TextSemanticType.Text = new TextAreaSemanticType();
        TextSemanticType.CodeText = new CodeTextAreaSemanticType();
        TextSemanticType.Password = new PasswordSemanticType();
        return TextSemanticType;
    })(Luxena.SemanticType);
    Luxena.TextSemanticType = TextSemanticType;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    //SemanticEntity |
    //SemanticObject<any>[] |
    //((se: TEntity) => SemanticObject<any>[]) |
    //{ [containerId: string]: SemanticObject<any>[] | SemanticObject<any> } |
    //((se: TEntity) => { [containerId: string]: SemanticObject<any>[] | SemanticObject<any> })
    ;
    var SemanticObject = (function () {
        function SemanticObject() {
        }
        SemanticObject.prototype._localizeTitle = function (value) {
            this._title = value[Luxena.language] || value.ru || this._title;
            this._titles = value[Luxena.language + "s"] || value.rus || this._titles;
            this._title2 = value[Luxena.language + "2"] || value.ru2 || this._title2;
            this._title5 = value[Luxena.language + "5"] || value.ru5 || this._title5;
            this._description = value[Luxena.language + "Desc"] || value.ruDesc || this._description;
            this._shortTitle = value[Luxena.language + "Short"] || value.ruShort || this._shortTitle;
        };
        //#region Setters
        SemanticObject.prototype.icon = function (value) {
            this._icon = value;
            return this;
        };
        SemanticObject.prototype.localizeTitle = function (value) {
            this._localizeTitle(value);
            return this;
        };
        SemanticObject.prototype.title = function (title) {
            title = ko.unwrap(title);
            if ($.isFunction(title)) {
                title = title(this["_entity"] || this["entity"]);
            }
            if (title instanceof Luxena.SemanticMember) {
                title = title._title;
            }
            this._title = title;
            return this;
        };
        SemanticObject.prototype.description = function (value) {
            this._description = value;
            return this;
        };
        return SemanticObject;
    })();
    Luxena.SemanticObject = SemanticObject;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var SemanticComponent = (function (_super) {
        __extends(SemanticComponent, _super);
        function SemanticComponent() {
            _super.apply(this, arguments);
            this._visible = true;
        }
        SemanticComponent.prototype.clone = function () {
            return $.extend({}, this);
        };
        SemanticComponent.prototype.addItemsToController = function (ctrl, action) {
        };
        SemanticComponent.prototype.loadFromData = function (model, data) {
        };
        SemanticComponent.prototype.toGridColumns = function () {
            return [];
        };
        SemanticComponent.prototype.toGridTotalItems = function () {
            return [];
        };
        return SemanticComponent;
    })(Luxena.SemanticObject);
    Luxena.SemanticComponent = SemanticComponent;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    (function (SemanticMemberKind) {
        SemanticMemberKind[SemanticMemberKind["Primary"] = 1] = "Primary";
        SemanticMemberKind[SemanticMemberKind["Important"] = 2] = "Important";
        SemanticMemberKind[SemanticMemberKind["Secondary"] = 3] = "Secondary";
        SemanticMemberKind[SemanticMemberKind["Utility"] = 4] = "Utility";
    })(Luxena.SemanticMemberKind || (Luxena.SemanticMemberKind = {}));
    var SemanticMemberKind = Luxena.SemanticMemberKind;
    (function (SemanticMemberVisibility) {
        SemanticMemberVisibility[SemanticMemberVisibility["Hidden"] = 1] = "Hidden";
        SemanticMemberVisibility[SemanticMemberVisibility["Reserve"] = 2] = "Reserve";
    })(Luxena.SemanticMemberVisibility || (Luxena.SemanticMemberVisibility = {}));
    var SemanticMemberVisibility = Luxena.SemanticMemberVisibility;
    var SemanticMember = (function (_super) {
        __extends(SemanticMember, _super);
        function SemanticMember() {
            var _this = this;
            _super.apply(this, arguments);
            this.getReference = function () { return _this._reference || (_this._reference = _this._lookupGetter && _this._lookupGetter()); };
            this._allowFiltering = true;
            this._allowGrouping = true;
            this._columnVisible = true;
            this._optionChanged = true;
        }
        //#region SemanticTypes
        SemanticMember.prototype.bool = function () {
            this._type = Luxena.BoolSemanticType.Bool;
            return this;
        };
        //#region Date
        SemanticMember.prototype.date = function () {
            this._type = Luxena.DateSemanticType.Date;
            return this;
        };
        SemanticMember.prototype.monthAndYear = function () {
            this._type = Luxena.DateSemanticType.MonthAndYear;
            return this;
        };
        SemanticMember.prototype.quarterAndYear = function () {
            this._type = Luxena.DateSemanticType.QuarterAndYear;
            return this;
        };
        SemanticMember.prototype.year = function () {
            this._type = Luxena.DateSemanticType.Year;
            return this;
        };
        SemanticMember.prototype.dateTime = function () {
            this._type = Luxena.DateSemanticType.DateTime;
            return this;
        };
        SemanticMember.prototype.dateTime2 = function () {
            this._type = Luxena.DateSemanticType.DateTime2;
            return this;
        };
        SemanticMember.prototype.time = function () {
            this._type = Luxena.DateSemanticType.Time;
            return this;
        };
        SemanticMember.prototype.time2 = function () {
            this._type = Luxena.DateSemanticType.Time;
            return this;
        };
        //#endregion
        //#region Number
        SemanticMember.prototype.float = function (precision) {
            this._type = Luxena.NumericSemanticType.Float;
            this._precision = precision;
            return this;
        };
        SemanticMember.prototype.int = function () {
            this._type = Luxena.NumericSemanticType.Int;
            return this;
        };
        SemanticMember.prototype.percent = function () {
            this._type = Luxena.NumericSemanticType.Percent;
            return this;
        };
        //#endregion
        SemanticMember.prototype.enum = function (enumType) {
            var enumListIds = [];
            for (var _i = 1; _i < arguments.length; _i++) {
                enumListIds[_i - 1] = arguments[_i];
            }
            this._type = Luxena.EnumSemanticType.Enum;
            this._enumType = enumType;
            this._enumIsFlags = enumType._isFlags;
            this._maxLength = enumType["_maxLength"];
            if (enumListIds && enumListIds.length)
                this._enumList = enumListIds.map(function (a) { return enumType._items[a]; });
            return this;
        };
        SemanticMember.prototype.enumIsFlags = function (value) {
            this._enumIsFlags = value !== false;
            return this;
        };
        SemanticMember.prototype.currencyCode = function () {
            this._type = Luxena.CurrencyCodeSemanticType.CurrencyCode;
            return this;
        };
        SemanticMember.prototype.money = function () {
            this._type = Luxena.MoneySemanticType.Money;
            this._isMoney = true;
            return this;
        };
        SemanticMember.prototype.defaultMoney = function () {
            this._type = Luxena.MoneySemanticType.Money;
            this._isMoney = true;
            return this;
        };
        SemanticMember.prototype.lookup = function (lookupGetter) {
            this._type = Luxena.LookupSemanticType.Reference;
            this._lookupGetter = lookupGetter;
            return this;
        };
        SemanticMember.prototype.string = function (maxLength, length, minLength) {
            this._type = Luxena.TextSemanticType.String;
            if (maxLength)
                this._maxLength = maxLength;
            if (length)
                this._length = length;
            if (minLength)
                this._minLength = minLength;
            return this;
        };
        SemanticMember.prototype.text = function (lineCount) {
            this._type = Luxena.TextSemanticType.Text;
            if (lineCount)
                this._lineCount = lineCount;
            return this;
        };
        SemanticMember.prototype.codeText = function (lineCount) {
            this._type = Luxena.TextSemanticType.CodeText;
            if (lineCount)
                this._lineCount = lineCount;
            return this;
        };
        SemanticMember.prototype.password = function () {
            this._type = Luxena.TextSemanticType.Password;
            return this;
        };
        SemanticMember.prototype.confirmPassword = function (passwordField) {
            this._type = Luxena.TextSemanticType.Password;
            return this;
        };
        SemanticMember.prototype.lineCount = function (value) {
            this._lineCount = value;
            return this;
        };
        //#endregion
        //#region Tags
        SemanticMember.prototype.entityDate = function () {
            this._isEntityDate = true;
            this._kind = SemanticMemberKind.Primary;
            this._optionChanged = true;
            return this;
        };
        SemanticMember.prototype.entityName = function () {
            this._isEntityName = true;
            this._kind = SemanticMemberKind.Primary;
            this._entity._nameMember = this;
            this._optionChanged = true;
            return this;
        };
        SemanticMember.prototype.entityType = function () {
            this._isEntityType = true;
            this._kind = SemanticMemberKind.Primary;
            this._optionChanged = true;
            return this;
        };
        SemanticMember.prototype.entityPosition = function () {
            this._isEntityPosition = true;
            this._kind = SemanticMemberKind.Primary;
            this._optionChanged = true;
            return this;
        };
        SemanticMember.prototype.importent = function (value) {
            this._kind = SemanticMemberKind.Important;
            return this;
        };
        SemanticMember.prototype.secondary = function () {
            this._kind = SemanticMemberKind.Secondary;
            this._optionChanged = true;
            return this;
        };
        SemanticMember.prototype.utility = function () {
            this._kind = SemanticMemberKind.Utility;
            this._optionChanged = true;
            return this;
        };
        SemanticMember.prototype.hidden = function () {
            this._visibility = SemanticMemberVisibility.Hidden;
            this._optionChanged = true;
            return this;
        };
        SemanticMember.prototype.reserve = function () {
            this._visibility = SemanticMemberVisibility.Reserve;
            this._optionChanged = true;
            return this;
        };
        SemanticMember.prototype.prepare = function () {
            if (!this._optionChanged)
                return;
            if (this._isEntityDate) {
            }
            else if (this._isEntityName) {
            }
            else if (this._isEntityType) {
            }
            else if (this._isEntityPosition) {
                this._allowGrouping = false;
            }
            else if (this._kind === SemanticMemberKind.Utility) {
                this._columnVisible = false;
            }
            if (this._visibility === SemanticMemberVisibility.Hidden) {
                this._columnVisible = false;
                this._selectRequired = true;
            }
            else if (this._visibility === SemanticMemberVisibility.Reserve) {
                this._columnVisible = false;
            }
            this._optionChanged = false;
        };
        //#endregion
        //#region Setters
        //dependencies(value: SemanticObject[])
        //{
        //	if (value)
        //	{
        //		value.forEach(a =>
        //		{
        //			if (this._dependencies.indexOf(a) < 0)
        //				this._dependencies.push(a);
        //		});
        //	}
        //	return this;
        //}
        SemanticMember.prototype.allowFiltering = function (value) {
            this._allowFiltering = value;
            return this;
        };
        SemanticMember.prototype.fixed = function (value) {
            this._columnFixed = value !== false;
            return this;
        };
        SemanticMember.prototype.defaultValue = function (value) {
            this._defaultValue = value;
            return this;
        };
        SemanticMember.prototype.emptyText = function (value) {
            this._placeholder = value;
            return this;
        };
        SemanticMember.prototype.format = function (value) {
            this._format = value;
            return this;
        };
        SemanticMember.prototype.length = function (value, min, max) {
            this._length = value;
            if (min)
                this._minLength = min;
            if (max)
                this._maxLength = max;
            return this;
        };
        SemanticMember.prototype.maxLength = function (value) {
            this._maxLength = value;
            return this;
        };
        SemanticMember.prototype.minLength = function (value) {
            this._minLength = value;
            return this;
        };
        SemanticMember.prototype.precision = function (value) {
            this._precision = value;
            return this;
        };
        SemanticMember.prototype.required = function (value) {
            this._required = value !== false;
            return this;
        };
        SemanticMember.prototype.subject = function (value) {
            this._isSubject = value !== false;
            return this;
        };
        SemanticMember.prototype.title = function (value) {
            this._title = value;
            return this;
        };
        SemanticMember.prototype.titlePrefix = function (value) {
            this._title = (value || "") + (this._title || "");
            return this;
        };
        SemanticMember.prototype.titlePostfix = function (value) {
            this._title = (this._title || "") + (value || "");
            return this;
        };
        SemanticMember.prototype.localizeTitle = function (value) {
            this._localizeTitle(value);
            return this;
        };
        SemanticMember.prototype.ru = function (value) {
            this._localizeTitle({ ru: value });
            return this;
        };
        //en(value: string)
        //{
        //	this._localizeTitle({ en: value });
        //	return this;
        //}
        //ua(value: string)
        //{
        //	this._localizeTitle({ ua: value });
        //	return this;
        //}
        SemanticMember.prototype.width = function (value) {
            this._width = value;
            return this;
        };
        SemanticMember.prototype.unique = function (value) {
            this._unique = value !== false;
            return this;
        };
        SemanticMember.prototype.calculated = function (value) {
            this._isCalculated = value !== false;
            return this;
        };
        SemanticMember.prototype.nonsaved = function (value) {
            this._isNonsaved = value !== false;
            return this;
        };
        SemanticMember.prototype.readOnly = function (value) {
            this._isReadOnly = value !== false;
            return this;
        };
        SemanticMember.prototype.totalSum = function (value) {
            this._useTotalSum = value !== false;
            return this;
        };
        //#endregion
        SemanticMember.prototype.getFilterExpr = function (value, operation) {
            return this._type.getFilterExpr(this, value, operation);
        };
        SemanticMember.prototype.clone = function (cfg) {
            var clone = $.extend(new SemanticMember(), this, cfg);
            clone._original = this;
            return clone;
        };
        SemanticMember.prototype.data = function (data) {
            return this._type.getFromData(this, data);
        };
        SemanticMember.prototype.get = function (model) {
            return ko.unwrap(this._type.getModel(model, this._name));
        };
        SemanticMember.prototype.getModel = function (model) {
            return this._type.getModel(model, this._name);
        };
        SemanticMember.prototype.filter = function (model, operation) {
            var value = this.get(model);
            if (!value)
                return undefined;
            return this.getFilterExpr(value, operation);
        };
        return SemanticMember;
    })(Luxena.SemanticObject);
    Luxena.SemanticMember = SemanticMember;
    var SemanticCollectionMember = (function (_super) {
        __extends(SemanticCollectionMember, _super);
        function SemanticCollectionMember() {
            _super.apply(this, arguments);
        }
        SemanticCollectionMember.prototype.toGrid = function (members, cfg) {
            if (!this._collectionItemEntity || !this._collectionItemMasterMember)
                throw Error("Свойство " + this._entity._name + "." + this._name + " не является коллекцией");
            var se = this._collectionItemEntity();
            var masterMember = this._collectionItemMasterMember(se);
            var sf = new Luxena.SemanticGridField(se, function () { return masterMember; }, members, cfg);
            sf._name = this._name;
            if (this._title && this._title !== this._name)
                sf.title(this._title);
            return sf;
        };
        SemanticCollectionMember.prototype.toTab = function (ctrl, members, cfg) {
            var sf = this.toGrid(members, cfg);
            return sf.toTab(ctrl);
        };
        return SemanticCollectionMember;
    })(SemanticMember);
    Luxena.SemanticCollectionMember = SemanticCollectionMember;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var SemanticEntityAction = (function (_super) {
        __extends(SemanticEntityAction, _super);
        function SemanticEntityAction(se, getParams) {
            _super.call(this);
            this._entity = se;
            this._getParams = getParams;
            if (se) {
                this._names = this._name = se._name;
                this._icon = se._icon;
                this._title = se._shortTitle || se._title;
            }
        }
        //#region Setters
        SemanticEntityAction.prototype.onExecute = function (value, navigateOptions) {
            this._onExecute = value;
            if (navigateOptions)
                this._navigateOptions = navigateOptions;
            return this;
        };
        SemanticEntityAction.prototype.navigateOptions = function (value) {
            this._navigateOptions = value;
            return this;
        };
        SemanticEntityAction.prototype.normal = function () {
            this._buttonType = "normal";
            return this;
        };
        SemanticEntityAction.prototype.default = function () {
            this._buttonType = "default";
            return this;
        };
        SemanticEntityAction.prototype.success = function () {
            this._buttonType = "success";
            return this;
        };
        SemanticEntityAction.prototype.danger = function () {
            this._buttonType = "danger";
            return this;
        };
        //#endregion
        SemanticEntityAction.prototype.getExecuter = function (prms) {
            var _this = this;
            prms = prms || {};
            var onExecute = ko.unwrap(this._onExecute);
            if (this._getParams)
                prms = this._getParams(prms) || prms || {};
            if (!onExecute)
                onExecute = function () { return _this.exec(prms); };
            else if ($.isFunction(onExecute))
                onExecute = onExecute(prms);
            if ($.isFunction(onExecute))
                return onExecute;
            if (!prms.view) {
                if (typeof onExecute === "string")
                    prms.view = onExecute;
                else if (onExecute instanceof Luxena.SemanticEntity)
                    prms.view = onExecute.editView();
            }
            else if (prms.view instanceof Luxena.SemanticEntity)
                prms.view = prms.view.editView();
            var action = function () { return Luxena.app.navigate(prms, _this._navigateOptions); };
            return action;
        };
        SemanticEntityAction.prototype.exec = function (prms) {
            var store = this._entity._store;
            var url = store["_byKeyUrl"](prms.id || "") + "/Default." + this._name;
            var select = prms._select;
            if (select && select.length)
                url += "?$select=" + select.join(",").replace(",$usecalculated", "") + "&$expand=" + (prms._expand || []).join(",");
            var d = $.Deferred();
            if (typeof prms._delta !== "string")
                prms._delta = JSON.stringify(prms._delta);
            var onExecuteDone = prms._onExecuteDone;
            prms = $.extend({}, prms);
            delete prms.id;
            delete prms._select;
            delete prms._expand;
            delete prms._onExecuteDone;
            $.when(store["_sendRequest"](url, "POST", null, prms))
                .done(function (newData) { return d.resolve(prms.id, newData); })
                .done(onExecuteDone)
                .fail(d.reject, d)
                .fail(Luxena.showError);
            return d.promise();
        };
        SemanticEntityAction.prototype.toButton = function (prms) {
            var executer = this.getExecuter(prms);
            return {
                icon: this._icon,
                text: this._title,
                hint: this._description,
                type: this._buttonType || "normal",
                onClick: executer,
                onExecute: executer,
            };
        };
        return SemanticEntityAction;
    })(Luxena.SemanticObject);
    Luxena.SemanticEntityAction = SemanticEntityAction;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var SemanticEntity = (function (_super) {
        __extends(SemanticEntity, _super);
        function SemanticEntity() {
            var _this = this;
            _super.apply(this, arguments);
            this._referenceFields = { id: "Id", name: "Name" };
            this._members = [];
            this._textIconHtml = "";
            this.listView = function () { return _this._names; };
            this.viewView = function () { return _this._name; };
            this.editView = function () { return _this._name + "Edit"; };
        }
        SemanticEntity.prototype.titleMenuItems = function (titleMenuItems) {
            this._titleMenuItems = titleMenuItems;
            return this;
        };
        SemanticEntity.prototype.getTitleMenuItems = function () {
            var items = this._titleMenuItems;
            var baseEntity = this;
            while (!items) {
                if (!baseEntity._getBaseEntity)
                    break;
                baseEntity = baseEntity._getBaseEntity();
                if (!baseEntity)
                    break;
                items = baseEntity._titleMenuItems;
            }
            if (items) {
                var i = items.indexOf(this);
                if (i >= 0) {
                    items = items.slice(0);
                    items.splice(i, 1);
                }
            }
            return items;
        };
        SemanticEntity.prototype.member = function (original) {
            var m = original ? original.clone() : new Luxena.SemanticMember();
            m._entity = this;
            this._members.push(m);
            return m;
        };
        SemanticEntity.prototype.collection = function (collectionItemEntity, collectionItemMasterMember, setter) {
            var m = new Luxena.SemanticCollectionMember();
            m._entity = this;
            m._collectionItemEntity = collectionItemEntity;
            m._collectionItemMasterMember = collectionItemMasterMember;
            setter && setter(m);
            this._members.push(m);
            return m;
        };
        SemanticEntity.prototype.action = function (getParams) {
            return new Luxena.SemanticEntityAction(this, getParams);
        };
        SemanticEntity.prototype.toAction = function (getParams) {
            return new Luxena.SemanticEntityAction(this, getParams);
        };
        SemanticEntity.prototype.init = function () {
            this.initMembers();
        };
        SemanticEntity.prototype.initMembers = function () {
            var entityPositions = [];
            var entityNames = [];
            var entityDates = [];
            for (var name_1 in this) {
                if (!this.hasOwnProperty(name_1) || name_1.indexOf("_") === 0)
                    continue;
                var sm = this[name_1];
                if (sm instanceof Luxena.SemanticObject) {
                    sm._entity = this;
                    sm._name = name_1;
                    if (sm instanceof Luxena.SemanticMember) {
                        sm._title = sm._title || sm._name;
                        if (sm._isEntityPosition)
                            entityPositions.push(sm);
                        if (sm._isEntityName)
                            entityNames.push(sm);
                        else if (sm._isEntityDate)
                            entityDates.push(sm);
                    }
                }
            }
            this._nameMember = this._nameMember || this[this._referenceFields.name];
            if (entityPositions.length) {
                entityPositions.forEach(function (a) { return a._sortOrder = "asc"; });
            }
            else if (entityDates.length) {
                entityDates.forEach(function (a) { return a._sortOrder = "desc"; });
                entityNames.forEach(function (a) { return a._sortOrder = "desc"; });
            }
            else {
                entityNames.forEach(function (a) { return a._sortOrder = "asc"; });
            }
        };
        ///#region Setters
        SemanticEntity.prototype.icon = function (value) {
            this._icon = value;
            this._textIconHtml = Luxena.getTextIconHtml(value);
            return this;
        };
        //title(value: string): SemanticEntity
        //{
        //	this._title = value;
        //	return this;
        //}
        //localizeTitle(value: ILocalization): SemanticEntity
        //{
        //	this._localizeTitle(value);
        //	return this;
        //}
        SemanticEntity.prototype.big = function (value) {
            this._isBig = value !== false;
            return this;
        };
        SemanticEntity.prototype.small = function (value) {
            this._isSmall = value !== false;
            return this;
        };
        SemanticEntity.prototype.wide = function (value) {
            this._isWide = value !== false;
            return this;
        };
        ///#endregion
        SemanticEntity.prototype.getTitle = function (data) {
            if (!data)
                return undefined;
            var title = ko.unwrap(data[this._referenceFields.name]);
            return title ? this._title + " " + title : this._title;
        };
        SemanticEntity.prototype.resolveListAction = function (action) {
            if (action === null)
                return null;
            if (action instanceof SemanticEntity)
                return action.resolveListAction();
            if (action === undefined)
                action = this._listAction || { uri: { view: this.listView() } };
            return action;
        };
        SemanticEntity.prototype.resolveViewAction = function (action, formEntity) {
            if (action === null)
                return null;
            if (action instanceof SemanticEntity)
                return action.resolveViewAction();
            if (action === undefined) {
                formEntity = formEntity || this;
                action = formEntity._viewAction || { uri: { view: formEntity.viewView() } };
            }
            return action;
        };
        SemanticEntity.prototype.resolveEditAction = function (action, formEntity) {
            if (action === null)
                return null;
            if (action instanceof SemanticEntity)
                return action.resolveEditAction();
            if (action === undefined) {
                formEntity = formEntity || this;
                action = formEntity._editAction || { uri: { view: formEntity.editView() } };
            }
            return action;
        };
        SemanticEntity.prototype.navigateToList = function () {
            var action = this.resolveListAction();
            Luxena.app.navigate(action.uri);
        };
        SemanticEntity.prototype.toggleSmart = function (target, cfg) {
            if (!this.showSmart)
                return;
            if (Luxena.smartVisible() && Luxena.smartTarget() === target)
                Luxena.smartVisible(false);
            else {
                this.showSmart(target, cfg);
            }
        };
        SemanticEntity.prototype.save = function (id, data) {
            var _this = this;
            var store = this._saveStore || this._store;
            if (id)
                return store.update(id, data);
            else
                return store.insert(data).done(function (e, newId) {
                    _this._lastId = newId;
                });
        };
        SemanticEntity.prototype.loadDefaults = function (data, select) {
            data = $.extend({ Version: -1 }, data);
            var store = this._store;
            var url = store["_byKeyUrl"]("");
            if (select && select.length)
                url += "?$select=" + select.join(",").replace(",$usecalculated", "");
            return $.when(store["_sendRequest"](url, "PUT", null, data));
        };
        SemanticEntity.prototype.recalc = function (prms) {
            var store = this._saveStore || this._store;
            var select = prms.select;
            var data = $.extend({ Version: -1 }, prms.data);
            if (prms.propertyName)
                data.LastChangedPropertyName = prms.propertyName;
            var url = store["_byKeyUrl"](prms.id);
            if (select && select.length)
                url += "?$select=" + select.join(",").replace(",$usecalculated", "");
            var d = $.Deferred();
            $.when(store["_sendRequest"](url, "PATCH", null, data))
                .done(function (newData) { return d.resolve(prms.id, newData); })
                .fail(d.reject, d);
            return d.promise();
        };
        SemanticEntity.prototype.delete = function (id) {
            var store = this._saveStore || this._store;
            return store.remove(id);
        };
        SemanticEntity.prototype.toListMenuItem = function (onExecute) {
            var title = this._titles || this._title || this._names || this._name;
            return {
                icon: this._icon,
                text: title,
                title: title,
                description: this._description,
                url: this.resolveListAction().uri.view,
                onExecute: onExecute,
                isList: true,
            };
        };
        SemanticEntity.prototype.toViewMenuItem = function (id) {
            var title = this._title || this._name;
            return {
                icon: this._icon,
                text: title,
                title: title,
                description: this._description,
                url: this.resolveViewAction().uri.view + (id ? "/" + id : ''),
            };
        };
        SemanticEntity.prototype.toEditMenuItem = function (id) {
            var title = this._title || this._name;
            return {
                icon: this._icon,
                text: title,
                title: title,
                description: this._description,
                url: this.editView() + (id ? "/" + id : ''),
            };
        };
        SemanticEntity.prototype.toListButton = function () {
            var _this = this;
            var action = function () { return Luxena.openAction(_this.resolveListAction()); };
            return {
                icon: "list",
                text: "Перейти к списку",
                onClick: action,
                onExecute: action,
            };
        };
        SemanticEntity.prototype.toViewButton = function (id) {
            var _this = this;
            var action = function () { return Luxena.openAction(_this.resolveViewAction(), id); };
            return {
                icon: "fa fa-search",
                text: "Подробнее",
                type: "default",
                onClick: action,
                onExecute: action,
            };
        };
        SemanticEntity.prototype.toAddMenuItem = function (defaults) {
            var _this = this;
            var title = this._title;
            var action = function () {
                var a = _this.resolveEditAction();
                a.defaults = defaults;
                Luxena.openAction(a);
            };
            return {
                icon: this._icon,
                text: title,
                title: title,
                description: this._description,
                onClick: action,
                onExecute: action,
            };
        };
        SemanticEntity.prototype.toAddButton = function (defaults) {
            var addCmd = {
                icon: "plus",
                text: "Добавить",
            };
            if (!this._isAbstract) {
                addCmd.onExecute = this.toAddMenuItem(defaults).onExecute;
                addCmd.onClick = addCmd.onExecute;
            }
            else {
                var addItems = [];
                var deriveds = this._getDerivedEntities && this._getDerivedEntities();
                deriveds && deriveds.forEach(function (a) {
                    if (a._isAbstract)
                        return;
                    addItems.push(a.toAddMenuItem(defaults));
                });
                addCmd.items = addItems;
            }
            return addCmd;
        };
        SemanticEntity.prototype.toEditButton = function (id) {
            var _this = this;
            var action = function () { return Luxena.openAction(_this.resolveEditAction(), id); };
            return {
                icon: "edit",
                text: "Изменить",
                type: "success",
                onClick: action,
                onExecute: action,
            };
        };
        SemanticEntity.prototype.toDeleteButton = function (id, done) {
            var _this = this;
            var action = function () { return _this.delete(id).done(done); };
            return {
                icon: "trash",
                text: "Удалить",
                type: "danger",
                onClick: action,
                onExecute: action,
            };
        };
        SemanticEntity.prototype.toRefreshButton = function (resresh) {
            return {
                icon: "refresh",
                text: "Обновить",
                onClick: resresh,
                onExecute: resresh,
            };
        };
        SemanticEntity.prototype.toTab = function () {
            return {
                icon: "fa fa-" + this._icon,
                title: this._title || this._name,
                template: this._name,
            };
        };
        SemanticEntity.prototype.toTabs = function () {
            return {
                icon: "fa fa-" + this._icon,
                title: this._titles || this._title || this._names || this._name,
                template: this._names || this._name,
            };
        };
        SemanticEntity.prototype.applyToThisAndDerived = function (action) {
            if (!action)
                return;
            action(this);
            var derived = this._getDerivedEntities && this._getDerivedEntities();
            if (!derived)
                return;
            derived.forEach(function (se) { return action(se); });
        };
        return SemanticEntity;
    })(Luxena.SemanticObject);
    Luxena.SemanticEntity = SemanticEntity;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var SemanticButton = (function (_super) {
        __extends(SemanticButton, _super);
        function SemanticButton(_action) {
            _super.call(this);
            this._action = _action;
            this._name = _action._name;
        }
        SemanticButton.prototype.render = function (container) {
            var ctrl = this._controller;
            var prms = {
                id: ctrl.getId(),
                _resync: ctrl.editMode || ctrl.viewMode,
                _save: ctrl.viewMode,
                _select: ctrl.dataSourceConfig.select,
            };
            if (ctrl.viewMode)
                prms._expand = ctrl.dataSourceConfig.expand;
            if (prms._resync)
                prms._onExecuteDone = function (data) { return ctrl.loadFromData(data); };
            ctrl.widgets[this._name] = this._action.toButton(prms);
            container.append("<div data-bind=\"dxButton: $root.widgets." + this._name + "\">");
        };
        return SemanticButton;
    })(Luxena.SemanticComponent);
    Luxena.SemanticButton = SemanticButton;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Ui;
    (function (Ui) {
        Ui.emptyRow = function () { return new Luxena.SemanticEmptyFieldRow(); };
    })(Ui = Luxena.Ui || (Luxena.Ui = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var SemanticField = (function (_super) {
        __extends(SemanticField, _super);
        function SemanticField(member) {
            _super.call(this);
            this.member = member;
            this.readonly = false;
            this._entity = member._entity;
            this._name = member._name;
            member.prepare();
        }
        SemanticField.prototype.addItemsToController = function (ctrl, action) {
            ctrl.fields.push(this);
        };
        SemanticField.prototype.getSelectFieldNames = function () {
            var sm = this.member;
            var names = sm._type.getSelectFieldNames(this);
            //if (sm._dependencies)
            //	sm._dependencies.forEach(a =>
            //	{
            //		if (names.indexOf(a._name) < 0)
            //			names.push(a._name);
            //	});
            return names;
        };
        SemanticField.prototype.getExpandFieldNames = function () {
            return this.member._type.getExpandFieldNames(this);
        };
        SemanticField.prototype.loadFromData = function (model, data) {
            this.member._type.loadFromData(this, model, data);
        };
        SemanticField.prototype.saveToData = function (model, data) {
            this.member._type.saveToData(this, model, data);
        };
        SemanticField.prototype.removeFromData = function (data) {
            this.member._type.removeFromData(this, data);
        };
        SemanticField.prototype.getModelValue = function () {
            var model = this._controller.model;
            return !model ? undefined : model[this._name];
        };
        SemanticField.prototype.setModelValue = function (value) {
            var model = this._controller.model;
            if (model)
                this.member._type.setModel(model, this._name, value);
        };
        SemanticField.prototype.setModelValueDefault = function (value) {
            var model = this._controller.model;
            if (model && !ko.unwrap(model[this._name]))
                this.member._type.setModel(model, this._name, value);
        };
        SemanticField.prototype.getFieldLabel = function () {
            return this.member._type.getFieldLabel(this);
        };
        SemanticField.prototype.getLength = function () {
            return this.member._type.getLength(this);
        };
        SemanticField.prototype.getWidth = function (length) {
            return this.member._type.getColumnWidth(this, length);
        };
        SemanticField.prototype.toGridColumns = function () {
            return this.member._type.toGridColumns(this);
        };
        SemanticField.prototype.toGridTotalItems = function () {
            return this.member._type.toGridTotalItems(this);
        };
        SemanticField.prototype.render = function (container) {
            var sf = this;
            var sm = sf.member;
            var rowEl = $("<div>")
                .addClass("dx-field");
            $("<div>")
                .addClass("dx-field-label")
                .attr("title", sm._title + (sm._description ? ": " + sm._description : ""))
                .text(sf.getFieldLabel())
                .appendTo(rowEl);
            var valueEl = $("<div>")
                .addClass(sf._controller.editMode && !sm._isReadOnly ? "dx-field-value" : "dx-field-value-static");
            sm._type.render(sf, valueEl, rowEl);
            valueEl.appendTo(rowEl);
            rowEl.appendTo(container);
        };
        return SemanticField;
    })(Luxena.SemanticComponent);
    Luxena.SemanticField = SemanticField;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var SemanticFieldSet = (function (_super) {
        __extends(SemanticFieldSet, _super);
        function SemanticFieldSet(members1, members2, members3, members4) {
            _super.call(this);
            this.members1 = members1;
            this.members2 = members2;
            this.members3 = members3;
            this.members4 = members4;
        }
        SemanticFieldSet.prototype.addItemsToController = function (ctrl, action) {
            var cols = this.itemsByColumn = [];
            this.members1 && cols.push(ctrl.addComponents(this.members1, null));
            this.members2 && cols.push(ctrl.addComponents(this.members2, null));
            this.members3 && cols.push(ctrl.addComponents(this.members3, null));
            this.members4 && cols.push(ctrl.addComponents(this.members4, null));
        };
        SemanticFieldSet.prototype.render = function (container) {
            var colCount = this.itemsByColumn.length;
            if (colCount <= 0)
                return;
            if (colCount === 1) {
                this.itemsByColumn[0].forEach(function (sc) { return sc.render(container); });
                return;
            }
            var tbl = $("<div>")
                .css("display", "table")
                .css("width", "100%")
                .css("margin-bottom", "10px");
            this.itemsByColumn.forEach(function (items, i) {
                var colEl = $("<div>")
                    .css("display", "table-cell")
                    .appendTo(tbl);
                items.forEach(function (sc) { return sc.render(colEl); });
            });
            container.append(tbl);
        };
        SemanticFieldSet.prototype.toTab = function () {
            return {
                title: this._title,
                template: this._name,
            };
        };
        return SemanticFieldSet;
    })(Luxena.SemanticComponent);
    Luxena.SemanticFieldSet = SemanticFieldSet;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Ui;
    (function (Ui) {
        function fieldSet2(se, cfg) {
            if (cfg.name) {
                se.applyToThisAndDerived(function (dse) {
                    var sc = new Luxena.SemanticFieldSet(cfg.members, cfg.members2, cfg.members3, cfg.members4);
                    sc._entity = dse;
                    sc._name = cfg.name;
                    sc.title(cfg.title);
                    dse[cfg.name] = sc;
                });
                return null;
            }
            else {
                var sc = new Luxena.SemanticFieldSet(cfg.members, cfg.members2, cfg.members3, cfg.members4);
                sc._entity = se;
                sc.title(cfg.title);
                return sc;
            }
        }
        Ui.fieldSet2 = fieldSet2;
        function fieldSet(se, title, members, members2, members3, members4) {
            return fieldSet2(se, {
                title: title,
                members: members,
                members2: members2,
                members3: members3,
                members4: members4,
            });
        }
        Ui.fieldSet = fieldSet;
    })(Ui = Luxena.Ui || (Luxena.Ui = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var SemanticFieldRow = (function (_super) {
        __extends(SemanticFieldRow, _super);
        function SemanticFieldRow(members, config) {
            _super.call(this);
            this.members = members;
            this.config = config;
            this._rowMode = true;
            this.config = this.config || {};
        }
        SemanticFieldRow.prototype.title = function (title) {
            if (title === undefined)
                title = "/";
            return _super.prototype.title.call(this, title);
        };
        SemanticFieldRow.prototype.addItemsToController = function (ctrl, action) {
            if (this._controller.gridMode && this.config.cellTemplate)
                return;
            this.components = ctrl.addComponents(this.members, null);
            if (this._title === undefined || this._title === "/") {
                this._title = this.components.map(function (sc) {
                    if (sc instanceof Luxena.SemanticField)
                        return sc.member._title || sc._title || "";
                    else
                        return sc._title || "";
                }).join(" / ");
            }
        };
        SemanticFieldRow.prototype.loadFromData = function (model, data) {
            var cfg = this.config;
            cfg.loadFromData && cfg.loadFromData(this, model, data);
        };
        SemanticFieldRow.prototype.toGridColumns = function () {
            var _this = this;
            var cfg = this.config;
            if (!cfg.cellTemplate)
                return [];
            var width = cfg.width;
            if (!width && cfg.length)
                width = 14 + Math.round(Luxena.SemanticType.charWidth * cfg.length) + 12;
            var col = {
                allowFiltering: false,
                allowGrouping: false,
                allowSorting: false,
                caption: this._title,
                width: width,
                cellTemplate: function (cell, cellInfo) { return cfg.cellTemplate(_this, cell, cellInfo); },
            };
            return [col];
        };
        SemanticFieldRow.prototype.render = function (container) {
            var cfg = this.config;
            if (!this._controller.editMode && cfg.renderDisplay) {
                cfg.renderDisplay(this, container);
                return;
            }
            var rowEl = $("<div>")
                .addClass("dx-field");
            $("<div>")
                .addClass("dx-field-label")
                .attr("title", this._title)
                .text(this._title + (this._title && ":"))
                .appendTo(rowEl);
            if (this._controller.editMode) {
                var valueBox = $("<div>")
                    .addClass("field-row");
                $("<div>")
                    .addClass("dx-field-value")
                    .append(valueBox)
                    .appendTo(rowEl);
                this.components.forEach(function (sc) {
                    var valueEl = $("<div>")
                        .appendTo(valueBox);
                    sc._rowMode = true;
                    if (sc instanceof Luxena.SemanticField) {
                        var length_1 = sc.getLength();
                        valueEl.css("width", (length_1.length || 1));
                        if (length_1.min)
                            valueEl.css("min-width", sc.getWidth(length_1.min));
                        sc.member._type.render(sc, valueEl, valueEl);
                    }
                    else
                        sc.render(valueEl);
                });
            }
            else {
                var valueDiv = $("<div>")
                    .addClass("dx-field-value-static")
                    .appendTo(rowEl);
                this.components.forEach(function (sc, i) {
                    if (i > 0)
                        valueDiv.append("&nbsp; / &nbsp;");
                    var valueEl = $("<span>")
                        .appendTo(valueDiv);
                    sc._rowMode = true;
                    if (sc instanceof Luxena.SemanticField) {
                        var sm = sc.member;
                        valueEl.attr("title", sm._shortTitle || sm._title);
                        sm._type.render(sc, valueEl, valueEl);
                    }
                    else
                        sc.render(valueEl);
                });
            }
            container.append(rowEl);
        };
        return SemanticFieldRow;
    })(Luxena.SemanticComponent);
    Luxena.SemanticFieldRow = SemanticFieldRow;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Ui;
    (function (Ui) {
        function fieldRow2(se, cfg) {
            if (cfg.name) {
                se.applyToThisAndDerived(function (dse) {
                    var sc = new Luxena.SemanticFieldRow(cfg.members, cfg);
                    sc._entity = dse;
                    sc._name = cfg.name;
                    sc.title(cfg.title);
                    dse[cfg.name] = sc;
                });
                return null;
            }
            else {
                var sc = new Luxena.SemanticFieldRow(cfg.members, cfg);
                sc._entity = se;
                sc.title(cfg.title);
                return sc;
            }
        }
        Ui.fieldRow2 = fieldRow2;
        function fieldRow(se, title, members) {
            return fieldRow2(se, {
                title: title,
                members: members,
            });
        }
        Ui.fieldRow = fieldRow;
    })(Ui = Luxena.Ui || (Luxena.Ui = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var SemanticGridField = (function (_super) {
        __extends(SemanticGridField, _super);
        function SemanticGridField(gridEntity, getMasterMember, members, config) {
            _super.call(this);
            this.gridEntity = gridEntity;
            this.getMasterMember = getMasterMember;
            this.members = members;
            this.config = config;
        }
        SemanticGridField.prototype.gridController = function (master) {
            var cfg = this.config || { inline: true };
            master = master || this._controller;
            return new Luxena.GridController($.extend(cfg, {
                entity: this.gridEntity,
                master: master,
                defaults: [[this.getMasterMember(this.gridEntity), master.getId()]],
                members: this.members,
            }));
        };
        SemanticGridField.prototype.render = function (container) {
            var scope = this.gridController().getScope();
            this._controller.model[this._name] = scope.gridOptions;
            $("<div>")
                .attr("data-bind", "dxDataGrid: r." + this._name)
                .appendTo(container);
        };
        SemanticGridField.prototype.toTab = function (master) {
            return {
                entity: this.gridEntity,
                title: this._title,
                template: "grid",
                scope: this.gridController(master).getScope(),
            };
        };
        return SemanticGridField;
    })(Luxena.SemanticComponent);
    Luxena.SemanticGridField = SemanticGridField;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var SemanticDomain = (function () {
        function SemanticDomain() {
        }
        SemanticDomain.prototype.entity = function (se) {
            se.init();
            return se;
        };
        SemanticDomain.prototype.entityByOData = function (data, targetEntity) {
            if (targetEntity && !targetEntity._isAbstract)
                return targetEntity;
            var name = ko.unwrap(data["_Type"] || data["@odata.type"]);
            name = name && name.replace("#" + Luxena.config.serverNamespace + ".", "");
            if (name) {
                var entity = this[name];
                if (entity)
                    return entity;
            }
            if (!targetEntity)
                return null;
            return targetEntity._getBaseEntity && targetEntity._getBaseEntity() || targetEntity;
        };
        return SemanticDomain;
    })();
    Luxena.SemanticDomain = SemanticDomain;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var SemanticController = (function () {
        function SemanticController(cfg) {
            this.components = [];
            this.fields = [];
            this.members = [];
            this.containerIds = [];
            this.config = cfg;
            this.model = cfg.model || {};
            this.modelIsExternal = !!cfg.model;
            var se = cfg.entity;
            cfg.list = cfg.list === null ? null : cfg.list || se;
            cfg.form = cfg.form === null ? null : cfg.form || se;
            cfg.view = cfg.view === null ? null : cfg.view || cfg.form;
            cfg.smart = cfg.smart === null ? null : cfg.smart || (cfg.view === null ? null : cfg.view || cfg.form);
            cfg.edit = cfg.edit === null ? null : cfg.edit || cfg.form;
            //cfg.listAction = cfg.listAction !== undefined && cfg.entity.resolveListAction(cfg.viewAction) || cfg.list.resolveListAction();
            //cfg.viewAction = cfg.viewAction !== undefined && cfg.entity.resolveViewAction(cfg.viewAction) || cfg.view.resolveViewAction();
            //cfg.editAction = cfg.editAction !== undefined && cfg.entity.resolveEditAction(cfg.editAction) || cfg.edit.resolveEditAction();
        }
        SemanticController.prototype.addComponent = function (item, containerId, action) {
            if (!item) {
                var cfg = this.config;
                $error("entity: ", cfg.entity && cfg.entity._name || cfg.entity, ", members: ", cfg.members, ", config: ", cfg);
                throw Error("Попытка добавить несуществующий SemanticComponent в SemanticController (containerId: " + containerId + ")");
            }
            var sc = null;
            var sm = null;
            if (containerId && this.containerIds.indexOf(containerId) < 0)
                this.containerIds.push(containerId);
            //$log(item);
            //$log(item["name"] || item["_name"]);
            if (item instanceof Luxena.SemanticMember) {
                sm = item;
                if (sm && this.members.indexOf(sm) >= 0)
                    return null;
                sc = new Luxena.SemanticField(sm);
            }
            else if (item instanceof Luxena.SemanticEntityAction) {
                sc = new Luxena.SemanticButton(item);
            }
            else if (item instanceof Luxena.SemanticComponent) {
                sc = item;
                sm = sc instanceof Luxena.SemanticField && sc.member;
                if (sm && this.members.indexOf(sm) >= 0)
                    return null;
                sc = sc.clone();
            }
            if (!sc)
                return null;
            sc._controller = this;
            sc._containerId = containerId;
            this.components.push(sc);
            if (sm && this.members.indexOf(sm) < 0)
                this.members.push(sm);
            sc.addItemsToController(this, action);
            action && action(sm, sc);
            return sc;
        };
        SemanticController.prototype.addComponents = function (items, containerId, action) {
            var _this = this;
            if (!items)
                return [];
            var components = [];
            var item = items;
            if ($.isFunction(items))
                item = items(this.config.entity);
            if (items instanceof Luxena.SemanticEntity)
                item = items._members;
            if ($.isArray(item)) {
                var list2 = item;
                list2.forEach(function (a) { return a && components.push(_this.addComponent(a, containerId, action)); });
            }
            else if (item instanceof Luxena.SemanticObject) {
                components.push(this.addComponent(item, containerId, action));
            }
            else {
                var items2 = items;
                for (var containerId2 in items2) {
                    if (!items2.hasOwnProperty(containerId2))
                        continue;
                    //$log(containerId2, ": ", items2[containerId2]);
                    components.push.apply(components, this.addComponents(items2[containerId2], containerId2, action));
                }
            }
            return components;
        };
        SemanticController.prototype.getField = function (sm) {
            return this.fields.filter(function (a) { return a.member === sm; })[0];
        };
        SemanticController.prototype.modelValue = function (sm, value) {
            var field = this.getField(sm);
            if (!field)
                return undefined;
            if (value !== undefined) {
                field.setModelValue(value);
                return value;
            }
            else
                return field.getModelValue();
        };
        SemanticController.prototype.getDataSourceConfig = function () {
            var cfg = this.config;
            var select = ["Id"];
            var expand = [];
            var usecalculated = false;
            var nameMember = cfg.entity._nameMember;
            if (cfg.entityTitle instanceof Luxena.SemanticMember)
                nameMember = cfg.entityTitle;
            if (nameMember && select.indexOf(nameMember._name) < 0) {
                select.push(nameMember._name);
                if (nameMember._isCalculated)
                    usecalculated = true;
            }
            this.fields.forEach(function (sf) {
                var sm = sf.member;
                if (!sf._visible && !sm._selectRequired)
                    return;
                var fields = sf.getSelectFieldNames();
                fields.forEach(function (a) {
                    if (select.indexOf(a) < 0)
                        select.push(a);
                });
                var expandFields = sf.getExpandFieldNames();
                expandFields.forEach(function (a) {
                    if (expand.indexOf(a) < 0)
                        expand.push(a);
                });
                if (sm._isCalculated)
                    usecalculated = true;
            });
            if (usecalculated)
                select.push("$usecalculated");
            var ds = {
                store: cfg.entity._store,
                select: select,
                expand: expand,
            };
            return ds;
        };
        SemanticController.prototype.viewShown = function () { };
        SemanticController.prototype.viewHidden = function () { };
        SemanticController.prototype.getEntityTitle = function (data) {
            var cfg = this.config;
            var entityTitle = cfg.entityTitle;
            var se = cfg.entity;
            if (!entityTitle)
                return se.getTitle(data) || se._title;
            if ($.isFunction(entityTitle))
                return entityTitle(data) || se._title;
            if (entityTitle instanceof Luxena.SemanticMember) {
                var title = ko.unwrap(data[entityTitle._name]);
                return title ? se._title + " " + title : se._title;
            }
            return entityTitle + "";
        };
        return SemanticController;
    })();
    Luxena.SemanticController = SemanticController;
    function openAction(action, id, actionOptions) {
        if (!action || !action.uri)
            return;
        var uri = $.extend({ id: id }, action.uri);
        if (!id && action.defaults)
            uri = $.extend(uri, action.defaults);
        Luxena.smartVisible(false);
        Luxena.app.navigate(uri, actionOptions || action.options);
    }
    Luxena.openAction = openAction;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var GridController = (function (_super) {
        __extends(GridController, _super);
        function GridController(cfg) {
            this.gridMode = true;
            if (!cfg.members)
                cfg.members = cfg.entity._members;
            if (cfg.defaults && cfg.defaults.length) {
                var defaults = {};
                cfg.defaults.forEach(function (a) {
                    var sm = a[0];
                    var model = {};
                    model[sm._name] = a[1];
                    sm._type.saveToData(new Luxena.SemanticField(sm), model, defaults);
                });
                this.editDefaults = defaults;
            }
            // ReSharper disable RedundantComparisonWithBoolean
            cfg.inline = cfg.inline === true;
            cfg.fixed = cfg.fixed === true;
            cfg.useFilter = cfg.useFilter !== false;
            cfg.useFilterRow = cfg.useFilterRow !== false;
            cfg.useGrouping = cfg.useGrouping !== false;
            cfg.usePaging = cfg.usePaging !== false;
            cfg.useSearch = cfg.useSearch !== false;
            cfg.useSorting = cfg.useSorting !== false;
            cfg.columnsIsStatic = cfg.columnsIsStatic === true;
            // ReSharper restore RedundantComparisonWithBoolean
            if (cfg.inline) {
                cfg.useFilterRow = false;
                cfg.useGrouping = false;
                cfg.useSearch = false;
                cfg.columnsIsStatic = true;
            }
            if (cfg.entity._isSmall) {
                cfg.useFilterRow = false;
                cfg.useGrouping = false;
            }
            _super.call(this, cfg);
            this.selectedRowKeys = ko.observableArray([]);
            this.createMembers();
            if (cfg.master)
                cfg.master.addDetails(this);
        }
        GridController.prototype.getScope = function () {
            var _this = this;
            var ctrl = this;
            var cfg = this.config;
            var se = cfg.entity;
            return this.scope = {
                icon: se._icon,
                title: se._titles || se._title || se._names || se._name,
                titleMenuItems: Luxena.toMenuSubitems(se.getTitleMenuItems()),
                viewMenuItems: ctrl.getMenuItems(),
                gridOptions: ctrl.getGridOptions(),
                viewShown: function () { return _this.viewShown(); },
                viewHidden: function () { return _this.viewHidden(); },
            };
        };
        GridController.prototype.createMembers = function () {
            var cfg = this.config;
            var se = cfg.entity;
            var members = cfg.members;
            //$log(cfg.entity._name);
            //$log(members(cfg.entity));
            this.addComponents(members, "columns", function (sm, sc) { return sc._visible = sm._columnVisible; });
            if (se instanceof Luxena.EntitySemantic) {
                this.addComponent(se.Id);
                if (se instanceof Luxena.Entity2Semantic) {
                    this.addComponents([
                        se.CreatedOn,
                        se.CreatedBy,
                        se.ModifiedOn,
                        se.ModifiedBy,
                    ], "columns");
                }
            }
        };
        GridController.prototype.viewShown = function () {
            if (!this.grid)
                return;
            this.selectByLastId(true);
        };
        GridController.prototype.viewHidden = function () {
            Luxena.smartVisible(false);
        };
        GridController.prototype.getId = function (data) {
            if (data)
                return data[this.config.entity._referenceFields.id];
            var keys = this.selectedRowKeys();
            return keys[0];
        };
        GridController.prototype.getGridOptions = function () {
            var _this = this;
            var cfg = this.config;
            var options = $.extend({
                dataSource: this.getDataSource(),
                columnChooser: { enabled: !cfg.columnsIsStatic },
                allowColumnReordering: !cfg.columnsIsStatic,
                allowColumnResizing: true,
                hoverStateEnabled: true,
                groupPanel: {
                    visible: cfg.useGrouping,
                    emptyPanelText: "Для группировки по колонке перетащите сюда её заголовок."
                },
                filterRow: {
                    visible: cfg.useFilter && cfg.useFilterRow && !cfg.fixed
                },
                searchPanel: {
                    visible: cfg.useFilter && cfg.useSearch && !cfg.fixed,
                    width: 240,
                },
                paging: {
                    enabled: cfg.usePaging && !cfg.fixed,
                    pageSize: cfg.inline ? 10 : 30,
                },
                scrolling: {
                    mode: cfg.inline || !cfg.usePaging || cfg.fixed ? "standard" : "virtual",
                },
                selection: {
                    mode: "single",
                },
                sorting: { mode: cfg.useSorting ? "multiple" : "none" },
                "export": {
                    enabled: !cfg.inline,
                    fileName: cfg.entity._names,
                    //allowExportSelectedData: true,
                    excelFilterEnabled: true,
                    excelWrapTextEnabled: true,
                },
                onInitialized: function (e) {
                    _this.grid = e.component;
                },
                onSelectionChanged: function (e) {
                    _this.selectedRowKeys(e.selectedRowKeys);
                },
                onRowClick: function (e) {
                    if (e.rowType !== "data")
                        return;
                    var data = e.data;
                    data._smartEntity.toggleSmart(e.rowElement[0], {
                        id: _this.getId(data),
                        view: data._viewEntity,
                        edit: data._editEntity,
                        refreshMaster: function () { return _this.grid && _this.grid.refresh(); },
                    });
                },
                onContentReady: function (e) {
                    if (!_this._pagerIsRepainted) {
                        _this._pagerIsRepainted = true;
                        var accordionId = _this.scope.accordionId;
                        if (accordionId) {
                            var accordion = $("#" + accordionId).dxAccordion("instance");
                            var selectedItems = accordion.option("selectedItems");
                            accordion.option("selectedItems", []);
                            accordion.option("selectedItems", selectedItems);
                        }
                        if (_this.config.fullHeight) {
                            _this.grid.option("height", "100%");
                        }
                    }
                    _this.selectByLastId();
                },
            }, cfg.gridOptions);
            this.appendComponentToGridOptions(options);
            return options;
        };
        GridController.prototype.appendComponentToGridOptions = function (options) {
            //var cfg = this.config;
            var columns = [];
            var totalItems = [], groupItems = [];
            var entityPositions = [];
            var entityNames = [];
            var entityDates = [];
            this.components.forEach(function (sc) {
                if (sc instanceof Luxena.SemanticField) {
                    var sm = sc.member;
                    if (sm._isEntityPosition)
                        entityPositions.push(sc);
                    if (sm._isEntityName)
                        entityNames.push(sc);
                    else if (sm._isEntityDate)
                        entityDates.push(sc);
                }
            });
            if (entityPositions.length) {
                entityPositions.forEach(function (a) { return a.sortOrder = "asc"; });
            }
            else if (entityDates.length) {
                entityDates.forEach(function (a) { return a.sortOrder = "desc"; });
                entityNames.forEach(function (a) { return a.sortOrder = "desc"; });
            }
            else {
                entityNames.forEach(function (a) { return a.sortOrder = "asc"; });
            }
            this.components.forEach(function (sc) {
                var cols = sc.toGridColumns();
                cols.forEach(function (a) { return columns.push(a); });
                var items = sc.toGridTotalItems();
                items.forEach(function (a) {
                    totalItems.push(a);
                    groupItems.push($.extend({
                        showInGroupFooter: true,
                        alignByColumn: true,
                    }, a));
                });
            });
            options.columns = columns;
            if (totalItems.length)
                options.summary = {
                    groupItems: groupItems,
                    totalItems: totalItems,
                };
        };
        GridController.prototype.getDataSource = function () {
            var _this = this;
            var options = this.getDataSourceConfig();
            var cfg = this.config;
            var ofilter;
            if (cfg.filter)
                options.filter = cfg.filter;
            else if (cfg.defaults) {
                var filter = cfg.defaults.map(function (a) { return a[0].getFilterExpr(a[1]); });
                if (filter.length)
                    options.filter = filter;
            }
            else if (cfg.master instanceof Luxena.FilterFormController) {
                ofilter = cfg.master.filter;
                //				options.filter = ofilter;
                options.filter = ko.unwrap(ofilter);
            }
            if (cfg.entity._isQueryResult || cfg.fixed) {
                delete options.expand;
                delete options.select;
            }
            options.map = function (data) {
                var se = cfg.entity._isAbstract && Luxena.sd.entityByOData(data) || cfg.entity;
                data._viewEntity = cfg.view === cfg.entity ? se : cfg.view;
                data._smartEntity = cfg.smart === cfg.entity ? se : cfg.smart;
                data._editEntity = cfg.edit === cfg.entity ? se : cfg.edit;
                return data;
            };
            var ds = new DevExpress.data.DataSource(options);
            if (ofilter)
                ofilter.subscribe(function (newFilter) {
                    ds.filter(newFilter);
                    //					$log(newFilter);
                    _this.grid && _this.grid.refresh();
                });
            return ds;
        };
        GridController.prototype.selectByLastId = function (onViewShown) {
            var id = this.config.entity._lastId;
            if (!id || !this.grid)
                return;
            if (onViewShown) {
                this.grid.refresh();
            }
            else {
                this.config.entity._lastId = null;
                var keys = this.grid.getSelectedRowKeys();
                if (!keys.length || keys.length === 1 && keys[0] !== id) {
                    this.grid.selectRows([id], false);
                }
            }
        };
        GridController.prototype.getMenuItems = function () {
            var _this = this;
            var cfg = this.config;
            var items = [];
            cfg.edit && items.push(cfg.edit.toAddButton(this.editDefaults));
            items.push(cfg.entity.toRefreshButton(function () { return _this.grid && _this.grid.refresh(); }));
            return items;
        };
        return GridController;
    })(Luxena.SemanticController);
    Luxena.GridController = GridController;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    //export interface IFormController
    //{
    //	addMembers(container: JQuery|string, members: SemanticMember[]): void;
    //}
    var FormController = (function (_super) {
        __extends(FormController, _super);
        function FormController(cfg) {
            _super.call(this, cfg);
            this.modelIsLoading = ko.observable(false);
            this.loadingMessage = ko.observable(undefined);
            this.containers = {};
            this.widgets = {};
            this.title = ko.observable();
            this.details = [];
            this.defaultContainerId = "fields";
            if (!cfg.members)
                cfg.members = cfg.entity._members;
            this.params = cfg.args && cfg.args[0] || { id: cfg.id };
            this.id = this.params.id || null;
            //this.viewInfo = cfg.args[1];
            this.modelIsReady = $.Deferred();
        }
        FormController.prototype.getId = function () { return this.id; };
        FormController.prototype.addDetails = function (details) {
            this.details.push(details);
        };
        FormController.prototype.getScope = function () {
            var _this = this;
            var cfg = this.config;
            var se = cfg.entity;
            this.addComponents(cfg.members, this.defaultContainerId);
            this.createContainers();
            this.title(se._title || se._name);
            this.scope = {
                controller: this,
                r: this.model,
                icon: se._icon,
                title: this.title,
                containers: this.containers,
                widgets: this.widgets,
                deferRenderingOptions: {
                    renderWhen: this.modelIsReady.promise(),
                    showLoadIndicator: true,
                    animation: "stagger-3d-drop",
                    staggerItemSelector: ".dx-field, .dx-accordion-item",
                },
                loadingOptions: {
                    visible: this.modelIsLoading,
                    message: this.loadingMessage,
                },
                viewMenuItems: cfg.viewMenuItems || this.getMenuItems(),
                viewShown: function () { return _this.viewShown(); },
                viewHidden: function () { return _this.viewHidden(); },
            };
            return this.scope;
        };
        FormController.prototype.getRedirectUriToEntityType = function (entityTypeName) {
            return { view: entityTypeName, id: this.getId() };
        };
        FormController.prototype.viewShown = function () {
            _super.prototype.viewShown.call(this);
            var cfg = this.config;
            var se = cfg.entity;
            se._lastId = this.getId();
            this.loadData();
            this.details.forEach(function (a) { return a.viewShown(); });
        };
        FormController.prototype.viewHidden = function () {
            _super.prototype.viewHidden.call(this);
            this.details.forEach(function (a) { return a.viewHidden(); });
        };
        FormController.prototype.createContainers = function () {
            var _this = this;
            if (!this.modelIsExternal || !this.model["__isLoaded"]) {
                this.fields.forEach(function (sf) { return sf.loadFromData(_this.model, {}); });
            }
            this.containerIds.forEach(function (containerId) {
                _this.containers[containerId] =
                    {
                        renderer: function (containerEl) {
                            _this.components.forEach(function (sc) {
                                if (sc._containerId === containerId)
                                    sc.render(containerEl);
                            });
                        },
                    };
            });
        };
        FormController.prototype.loadData = function (onLoaded) {
            var _this = this;
            var cfg = this.config;
            //this.loadingMessage("Загрузка...");
            //this.modelIsLoading(true);
            var dsConfig = this.dataSourceConfig = this.getDataSourceConfig();
            dsConfig.filter = [cfg.entity._referenceFields.id, "=", this.getId()];
            var ds = new DevExpress.data.DataSource(dsConfig);
            ds.load()
                .done(function (data) { _this.loadFromData(data[0], true); onLoaded && onLoaded(); })
                .fail(function () { return _this.loadFromData({}, true); });
        };
        FormController.prototype.refresh = function () {
            this.loadData();
        };
        FormController.prototype.loadFromData = function (data, resolveModel) {
            //$log("loadFromData", ko.unwrap2(this.model));
            var _this = this;
            this.data = data;
            if (data !== undefined) {
                this.components.forEach(function (sc) { return sc.loadFromData(_this.model, data); });
            }
            this.applyModel();
            if (resolveModel) {
                //this.modelIsLoading(false);
                this.modelIsReady.resolve();
            }
        };
        FormController.prototype.applyModel = function () {
            var cfg = this.config;
            var title = this.getEntityTitle(this.data);
            this.title(title);
            if (cfg.onLoaded)
                cfg.onLoaded();
        };
        return FormController;
    })(Luxena.SemanticController);
    Luxena.FormController = FormController;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var ViewFormController = (function (_super) {
        __extends(ViewFormController, _super);
        function ViewFormController() {
            _super.apply(this, arguments);
            this.viewMode = true;
        }
        ViewFormController.prototype.getMenuItems = function () {
            var _this = this;
            var cfg = this.config;
            var menu = [];
            var id = this.getId();
            cfg.edit && menu.push(cfg.edit.toEditButton(id));
            var menu2 = {
                icon: "ellipsis-v",
                items: []
            };
            menu.push(menu2);
            cfg.edit && menu2.items.push(cfg.edit.toDeleteButton(id, function () { return Luxena.app.back(); }));
            cfg.list && menu2.items.push(cfg.list.toListButton());
            menu2.items.push(cfg.entity.toRefreshButton(function () { return _this.refresh(); }));
            return menu;
        };
        ViewFormController.prototype.toDetailListTab = function (se, getMasterMember, members, cfg) {
            return {
                entity: se,
                title: cfg && cfg.title,
                template: "grid",
                scope: new Luxena.GridController($.extend(cfg || { inline: true }, {
                    entity: se,
                    master: this,
                    defaults: [[getMasterMember(se), this.getId()]],
                    members: members,
                })).getScope()
            };
        };
        return ViewFormController;
    })(Luxena.FormController);
    Luxena.ViewFormController = ViewFormController;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    Luxena.smartTarget = ko.observable();
    Luxena.smartTitle = ko.observable();
    Luxena.smartViewScope = ko.observable();
    //export var smartButtons = ko.observable();
    Luxena.smartVisible = ko.observable(false);
    function getSmartPopoverOptions() {
        return {
            target: Luxena.smartTarget,
            title: Luxena.smartTitle,
            visible: Luxena.smartVisible,
            position: "bottom",
            width: 800,
            showTitle: true,
            showCloseButton: true,
            closeOnBackButton: true,
            //animation: {
            //	show: {
            //		type: "fade",
            //		from: 0,
            //		to: 1
            //	},
            //	hide: {},
            //},
            onShown: function (e) {
                //$log("repaint");
                //e.component.repaint();
                //var content = $("#smartcontent");
                //$log(content);
                //var scroll = content.dxScrollView("instance");
                //$log(scroll);
                //scroll["refresh"]();
            },
        };
    }
    Luxena.getSmartPopoverOptions = getSmartPopoverOptions;
    var SmartFormController = (function (_super) {
        __extends(SmartFormController, _super);
        function SmartFormController() {
            _super.apply(this, arguments);
            this.smartMode = true;
        }
        SmartFormController.prototype.getMenuItems = function () {
            return undefined;
        };
        SmartFormController.prototype.getScope = function () {
            var cfg = this.config;
            var se = cfg.entity;
            cfg.view = cfg.view || cfg.view !== null && (cfg.form || se);
            cfg.edit = cfg.edit || cfg.edit !== null && (cfg.form || se);
            var scope = _super.prototype.getScope.call(this);
            var id = this.id;
            var btns = scope.buttons = [];
            cfg.view && btns.push(cfg.view.toViewButton(id));
            cfg.edit && btns.push(cfg.edit.toEditButton(id));
            cfg.edit && btns.push(cfg.edit.toDeleteButton(id, function () {
                cfg.refreshMaster();
                Luxena.smartVisible(false);
            }));
            cfg.actions && cfg.actions.forEach(function (action) { return btns.push(action.toButton({ id: id })); });
            return scope;
        };
        SmartFormController.prototype.show = function (target) {
            Luxena.smartVisible(false);
            Luxena.smartTarget(target);
            var scope = this.getScope();
            this.loadData(function () {
                Luxena.smartTitle(scope.title);
                Luxena.smartViewScope(scope);
                Luxena.smartVisible(true);
            });
        };
        return SmartFormController;
    })(Luxena.FormController);
    Luxena.SmartFormController = SmartFormController;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var BaseEditFormController = (function (_super) {
        __extends(BaseEditFormController, _super);
        function BaseEditFormController() {
            _super.apply(this, arguments);
            this.editMode = true;
            this.validators = {};
        }
        BaseEditFormController.prototype.getScope = function () {
            var scope = _super.prototype.getScope.call(this);
            scope.validators = this.validators;
            return scope;
        };
        return BaseEditFormController;
    })(Luxena.FormController);
    Luxena.BaseEditFormController = BaseEditFormController;
    var EditFormController = (function (_super) {
        __extends(EditFormController, _super);
        function EditFormController() {
            _super.apply(this, arguments);
        }
        EditFormController.prototype.getRedirectUriToEntityType = function (entityTypeName) {
            return { view: entityTypeName + "Edit", id: this.getId() };
        };
        EditFormController.prototype.getMenuItems = function () {
            var _this = this;
            return [
                {
                    icon: "save",
                    text: "Сохранить",
                    onExecute: function () { return _this.save(); },
                }, {
                    icon: "ellipsis-v",
                    items: [
                        {
                            icon: "check-square-o",
                            text: "проверить",
                            onExecute: function () { return _this.validate(); },
                        }, {
                            icon: "refresh",
                            text: "Обновить",
                            onExecute: function () { return _this.refresh(); },
                        }, {
                            text: "$log(model)",
                            onExecute: function () { return $log_(ko.unwrap2(_this.model)); },
                        },
                    ],
                }
            ];
        };
        EditFormController.prototype.loadData = function (onLoaded) {
            var _this = this;
            var cfg = this.config;
            var se = cfg.entity;
            if (this.getId()) {
                _super.prototype.loadData.call(this, onLoaded);
            }
            else if (se._store) {
                var prms = this.params || {};
                delete prms.view;
                this.loadFromData(prms);
                prms = this.saveToData();
                for (var name_2 in prms) {
                    if (!prms.hasOwnProperty(name_2))
                        continue;
                    var prm = prms[name_2];
                    if (prm === null || prm == undefined)
                        delete prms[name_2];
                }
                var ds = this.dataSourceConfig = this.getDataSourceConfig();
                se.loadDefaults(prms, ds.select)
                    .done(function (defaults) {
                    _this.loadFromData(defaults, true);
                    onLoaded && onLoaded();
                })
                    .fail(function () { return _this.modelIsReady.resolve(); });
            }
            else {
                this.loadFromData({}, true);
                onLoaded && onLoaded();
            }
        };
        EditFormController.prototype.validate = function () {
            var validateGroup = DevExpress.validationEngine.getGroupConfig("edit-form");
            if (!validateGroup)
                return true;
            var validateResult = validateGroup.validate();
            if (!validateResult.isValid)
                console.log("validateResult: ", validateResult);
            return validateResult.isValid;
        };
        EditFormController.prototype.saveToData = function () {
            var _this = this;
            var data = this.data || {};
            this.config.entity._members.forEach(function (sm) {
                if (sm._isNonsaved)
                    sm._type.removeFromData(sm, data);
            });
            this.fields.forEach(function (sf) {
                if (!sf.member._isNonsaved)
                    sf.saveToData(_this.model, data);
            });
            return data;
        };
        EditFormController.prototype.save = function () {
            var _this = this;
            if (!this.validate())
                return;
            this.loadingMessage("Сохранение...");
            this.modelIsLoading(true);
            this.config.entity
                .save(this.getId(), this.saveToData())
                .done(function () {
                _this.modelIsLoading(false);
                Luxena.app.back();
            })
                .fail(function () { return _this.modelIsLoading(false); });
        };
        EditFormController.prototype.recalc = function (propertyName) {
            var _this = this;
            if (this.isRecalculating)
                return;
            this.isRecalculating = true;
            //$log("recalc");
            var data = this.saveToData();
            var se = this.config.entity;
            se.recalc({
                id: this.getId(),
                data: data,
                propertyName: propertyName,
                select: this.dataSourceConfig.select,
            })
                .done(function (id, data) {
                //s$log(data);
                try {
                    _this.fields.forEach(function (sf) { return sf.loadFromData(_this.model, data); });
                }
                finally {
                    _this.isRecalculating = false;
                }
            })
                .fail(function () { return _this.isRecalculating = false; });
        };
        return EditFormController;
    })(BaseEditFormController);
    Luxena.EditFormController = EditFormController;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var FilterFormController = (function (_super) {
        __extends(FilterFormController, _super);
        function FilterFormController() {
            _super.apply(this, arguments);
            this.defaultContainerId = "filterFields";
            this.filterMode = true;
            this.filter = ko.observable();
        }
        FilterFormController.prototype.getScope = function () {
            var _this = this;
            var scope = _super.prototype.getScope.call(this);
            this.apply();
            scope.applyFilter = function () { return _this.apply(); };
            scope.findButton = {
                text: "Начать поиск",
                icon: "search",
                onClick: function () { return _this.apply(); },
                type: "default"
            };
            var collapsed = false;
            var collapseIcon = ko.observable("fa fa-arrow-left");
            scope.filterPanelWidth = ko.observable(440);
            scope.collapseButton = {
                hint: "Свернуть панель фильтра",
                icon: collapseIcon,
                onClick: function () {
                    collapsed = !collapsed;
                    scope.filterPanelWidth(collapsed ? 55 : 440);
                    collapseIcon(collapsed ? "fa fa-arrow-right" : "fa fa-arrow-left");
                },
            };
            return scope;
        };
        FilterFormController.prototype.getScopeWithGrid = function (gridScope) {
            var scope = this.getScope();
            if (gridScope.getScope)
                gridScope = gridScope.getScope();
            scope.titleMenuItems = gridScope.titleMenuItems,
                scope.viewMenuItems = gridScope.viewMenuItems;
            scope.title = gridScope.title;
            scope.gridOptions = gridScope.gridOptions;
            return scope;
        };
        FilterFormController.prototype.getMenuItems = function () {
            var _this = this;
            return [{
                    icon: "refresh",
                    text: "Обновить",
                    onExecute: function () { return _this.apply(); },
                }];
        };
        FilterFormController.prototype.loadData = function () {
            if (!this.model["__isLoaded"]) {
                this.loadFromData({});
                this.model["__isLoaded"] = true;
            }
            else {
                this.applyModel();
            }
            this.apply();
        };
        FilterFormController.prototype.apply = function () {
            var cfg = this.config;
            var filter = cfg.filter && cfg.filter(this.model) || this.members;
            filter = prepareFilterExpression(this.model, filter);
            this.filter(filter);
        };
        return FilterFormController;
    })(Luxena.BaseEditFormController);
    Luxena.FilterFormController = FilterFormController;
    function prepareFilterExpression(model, list) {
        if (!list || !list.length)
            return undefined;
        //$logb("prepareFilterExpression");
        var result = [];
        list.forEach(function (a) {
            if ($.isArray(a))
                a = prepareFilterExpression(model, a);
            else if (a instanceof Luxena.SemanticMember)
                a = a.filter(model);
            //$log(" => ", a);
            if (a !== undefined)
                result.push(a);
        });
        //$log("result:", result);
        var result2 = [];
        var priorIsOperation = true;
        result.forEach(function (item, i) {
            var isOperation = item === "and" || item === "or";
            if (!isOperation || (!priorIsOperation && i < result.length - 1)) {
                result2.push(item);
                priorIsOperation = isOperation;
            }
        });
        if (priorIsOperation && result2.length)
            result2.pop();
        result2 = result2.length ? result2 : undefined;
        //$loge();
        return result2;
    }
    Luxena.prepareFilterExpression = prepareFilterExpression;
    ;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        function registerEntityControllers(entity, config) {
            if (!config || !entity)
                return;
            var entities = ($.isArray(entity) ? entity : [entity]);
            entities.forEach(function (se) {
                var cfg = config(se);
                if ($.isArray(cfg))
                    cfg = { members: cfg };
                var listEntity = cfg.list instanceof Luxena.SemanticEntity ? cfg.list : se;
                var viewEntity = cfg.view === null ? null :
                    (cfg.view || cfg.form) instanceof Luxena.SemanticEntity ? (cfg.view || cfg.form) : undefined;
                var editEntity = cfg.edit === null ? null :
                    (cfg.edit || cfg.form) instanceof Luxena.SemanticEntity ? (cfg.edit || cfg.form) : undefined;
                var listMembers = cfg.list || (cfg.list === null ? null : cfg.members);
                if (listMembers) {
                    se._listController = Views[se.listView()] = function () {
                        if (listMembers instanceof Luxena.SemanticEntity) {
                            var params = listMembers.resolveListAction().uri;
                            return {
                                viewShowing: function () { return Luxena.app.navigate(params, { target: 'current' }); }
                            };
                        }
                        var ctrl = new Luxena.GridController({
                            entity: se,
                            members: listMembers,
                            gridOptions: cfg.gridOptions,
                            view: viewEntity,
                            edit: editEntity,
                        });
                        return ctrl.getScope();
                    };
                }
                var viewMembers = cfg.view || (cfg.view === null ? null : cfg.form || cfg.members);
                if (viewMembers || se._isAbstract) {
                    se._viewController = Views[se.viewView()] = function () {
                        var args = [];
                        for (var _i = 0; _i < arguments.length; _i++) {
                            args[_i - 0] = arguments[_i];
                        }
                        if (viewMembers instanceof Luxena.SemanticEntity) {
                            var params = $.extend(args[0] || {}, viewMembers.resolveViewAction().uri);
                            return {
                                viewShowing: function () { return Luxena.app.navigate(params, { target: 'current' }); }
                            };
                        }
                        var ctrl = new Luxena.ViewFormController({
                            entity: se,
                            args: args,
                            entityTitle: cfg.viewTitle || cfg.formTitle || cfg.entityTitle,
                            members: viewMembers,
                            list: listEntity,
                            edit: editEntity,
                        });
                        var scope = ctrl.getScope();
                        var scopeExt = cfg.viewScope || cfg.formScope;
                        scopeExt = scopeExt && scopeExt(ctrl, scope);
                        //$log(scopeExt);
                        if (scopeExt)
                            scope = $.extend(scope, scopeExt);
                        return scope;
                    };
                }
                var smartMembers = cfg.smart || (cfg.smart === null ? null : cfg.view || cfg.form || cfg.members);
                if (smartMembers) {
                    se.showSmart = function (target, smartCfg) {
                        smartCfg.entity = se;
                        smartCfg.entityTitle = cfg.smartTitle || cfg.viewTitle || cfg.formTitle || cfg.entityTitle;
                        smartCfg.members = smartMembers;
                        smartCfg.view = smartCfg.view || viewEntity;
                        smartCfg.edit = smartCfg.edit || editEntity;
                        smartCfg.actions = cfg.actions;
                        var ctrl = new Luxena.SmartFormController(smartCfg);
                        ctrl.show(target);
                    };
                }
                var editMembers = cfg.edit || (cfg.edit === null ? null : cfg.form || cfg.members);
                if (editMembers || se._isAbstract) {
                    se._editController = Views[se.editView()] = function () {
                        var args = [];
                        for (var _i = 0; _i < arguments.length; _i++) {
                            args[_i - 0] = arguments[_i];
                        }
                        if (editMembers instanceof Luxena.SemanticEntity) {
                            var params = $.extend(args[0] || {}, viewMembers.resolveEditAction().uri);
                            return {
                                viewShowing: function () { return Luxena.app.navigate(params, { target: 'current' }); }
                            };
                        }
                        var ctrl = new Luxena.EditFormController({
                            entity: se,
                            args: args,
                            entityTitle: cfg.editTitle || cfg.formTitle || cfg.entityTitle,
                            members: editMembers,
                            list: cfg.list instanceof Luxena.SemanticEntity ? cfg.list : se,
                            view: cfg.view instanceof Luxena.SemanticEntity ? cfg.view : undefined,
                        });
                        var scope = ctrl.getScope();
                        var scopeExt = cfg.editScope || cfg.formScope;
                        scopeExt = scopeExt && scopeExt(ctrl, scope);
                        if (scopeExt)
                            scope = $.extend(scope, scopeExt);
                        return scope;
                    };
                }
            });
        }
        Views.registerEntityControllers = registerEntityControllers;
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Validators;
    (function (Validators) {
        function uniqueValidator(params) {
            var sf = params.validator.element()[0]["sf"];
            var se = sf.entity;
            var sm = sf.member;
            var id = sf.controller.getId();
            var filter = id
                ? [[sm._name, '=', params.value], [sm._entity._referenceFields.id, '<>', id]]
                : [sm._name, '=', params.value];
            se._store.createQuery({})
                .filter(filter)
                .select(sm._entity._referenceFields.id)
                .enumerate()
                .done(function (data) {
                params.rule.isValid = data && data.length === 0;
                params.validator.validate();
            });
            return true;
        }
        Validators.uniqueValidator = uniqueValidator;
        function stringLength(params) {
            var rule = params.rule;
            var value = params.value;
            if (!value) {
                rule.isValid = true;
                return true;
            }
            var min = rule["min"];
            var max = rule["max"];
            if (min !== undefined && value.length < min || max !== undefined && value.length > max)
                return false;
            rule.isValid = true;
            return true;
        }
        Validators.stringLength = stringLength;
    })(Validators = Luxena.Validators || (Luxena.Validators = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Ui;
    (function (Ui) {
        var moneyProgressCount = 0;
        function moneyProgress(se, valueMember, targetMember) {
            if (!valueMember)
                throw Error("Не указан valueMember");
            if (!valueMember._isMoney)
                throw Error("valueMember должен быть типа Money");
            return Ui.fieldRow2(se, (function () {
                var displayTarget = ko.observable();
                var displayValue = ko.observable();
                var displayMinValue = ko.observable();
                var displayMaxValue = ko.observable();
                var displayColor = ko.observable();
                var displayCurrency = ko.observable();
                var displaySubvalues = ko.observable();
                return {
                    title: valueMember,
                    members: function () { return [targetMember, valueMember.clone({ _selectRequired: true }),]; },
                    width: 150,
                    cellTemplate: function (sc, cell, cellInfo) {
                        var r = cellInfo.data;
                        var targetData = r[targetMember._name];
                        var valueData = r[valueMember._name];
                        var target = targetData && targetData.Amount;
                        var value = valueData && valueData.Amount;
                        var color = Math.abs(value - target) <= 0.01 ? "#82e583" : value >= target ? "#e55253" : undefined;
                        cell
                            .addClass("bullet")
                            .dxBullet({
                            startScaleValue: Math.min(0, target, value),
                            endScaleValue: Math.max(0, target, value),
                            value: value,
                            target: target,
                            color: color,
                            size: {
                                width: 140,
                                height: 25
                            },
                            tooltip: {
                                enabled: false,
                            }
                        });
                        //cell.text(totalAmount + " " + paidAmount);
                    },
                    loadFromData: function (sc, model, data) {
                        var targetData = data[targetMember._name];
                        var valueData = data[valueMember._name];
                        var target = targetData && targetData.Amount;
                        var value = valueData && valueData.Amount;
                        var color = Math.abs(value - target) <= 0.01 ? "#82e583" : value >= target ? "#e55253" : "#ebdd8f";
                        //$alert(target);
                        displayTarget(target);
                        displayValue(value);
                        displayMinValue(Math.min(0, target, value));
                        displayMaxValue(Math.max(0, target, value));
                        displayColor(color);
                        displayCurrency(targetData && targetData.CurrencyId || "");
                        displaySubvalues([target]);
                    },
                    renderDisplay: function (sc, container) {
                        var model = sc._controller.model;
                        var name = "moneyProgress" + (moneyProgressCount++);
                        model[name] = {
                            //geometry: { startAngle: 180, endAngle: 0 },
                            scale: {
                                startValue: displayMinValue,
                                endValue: displayMaxValue,
                                label: {
                                    format: "n0",
                                },
                            },
                            valueIndicator: {
                                //type: 'rangebar',
                                baseValue: 0
                            },
                            subvalueIndicator: {
                                type: "textcloud",
                                text: {
                                    format: "n0",
                                    customizeText: function (arg) { return (arg.valueText + " " + displayCurrency()); },
                                }
                            },
                            rangeContainer: {
                                backgroundColor: displayColor,
                            },
                            value: displayValue,
                            subvalues: displaySubvalues,
                        };
                        $("<div>")
                            .height(200)
                            .attr("data-bind", "dxCircularGauge: r." + name)
                            .appendTo(container);
                    },
                };
            })());
        }
        Ui.moneyProgress = moneyProgress;
    })(Ui = Luxena.Ui || (Luxena.Ui = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    Luxena.config = {
        serverNamespace: "Luxena.Travel.Domain",
        layoutSet: "agent",
        endpoints: {
            db: {
                local: "odata/",
                production: "odata/"
            }
        },
        services: {
            db: {}
        },
    };
    function toMenuSubitems(subitems, sudmenuTemplate) {
        if (!subitems)
            return null;
        var result = [];
        subitems.forEach(function (subitem, a, b) {
            if (!subitem)
                return;
            if (subitem instanceof Luxena.SemanticEntity)
                subitem = subitem.toListMenuItem();
            else
                subitem = $.extend({}, subitem);
            if (sudmenuTemplate)
                subitem["template"] = sudmenuTemplate;
            result.push(subitem);
        });
        return result;
    }
    Luxena.toMenuSubitems = toMenuSubitems;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    //#region Enums
    //#region CheckItemTaxMode 
    (function (CheckItemTaxMode) {
        CheckItemTaxMode[CheckItemTaxMode["Default"] = 0] = "Default";
        CheckItemTaxMode[CheckItemTaxMode["WithoutVAT"] = 1] = "WithoutVAT";
        CheckItemTaxMode[CheckItemTaxMode["WithoutVATAndServiceFeeVAT"] = 2] = "WithoutVATAndServiceFeeVAT";
    })(Luxena.CheckItemTaxMode || (Luxena.CheckItemTaxMode = {}));
    var CheckItemTaxMode = Luxena.CheckItemTaxMode;
    var CheckItemTaxMode;
    (function (CheckItemTaxMode) {
        CheckItemTaxMode._ns = "Luxena.Travel.Domain";
        CheckItemTaxMode._name = "CheckItemTaxMode";
        CheckItemTaxMode._fullName = "Luxena.Travel.Domain.CheckItemTaxMode";
        CheckItemTaxMode._getEdm = function (value) { return !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.CheckItemTaxMode'" + value + "'"); };
        CheckItemTaxMode._array = [
            { Id: "Default", Value: 0, Name: "По умолчанию", TextIconHtml: "", ru: "По умолчанию" },
            { Id: "WithoutVAT", Value: 1, Name: "Ставка Д и без позиции для сервисного сбора", TextIconHtml: "", ru: "Ставка Д и без позиции для сервисного сбора" },
            { Id: "WithoutVATAndServiceFeeVAT", Value: 2, Name: "Ставка Д и ставка А для сервисного сбора", TextIconHtml: "", ru: "Ставка Д и ставка А для сервисного сбора" },
        ];
        CheckItemTaxMode._maxLength = 32;
        CheckItemTaxMode._items = {
            "0": CheckItemTaxMode._array[0],
            "Default": CheckItemTaxMode._array[0],
            "1": CheckItemTaxMode._array[1],
            "WithoutVAT": CheckItemTaxMode._array[1],
            "2": CheckItemTaxMode._array[2],
            "WithoutVATAndServiceFeeVAT": CheckItemTaxMode._array[2],
        };
    })(CheckItemTaxMode = Luxena.CheckItemTaxMode || (Luxena.CheckItemTaxMode = {}));
    //#endregion
    //#region InvoiceNumberMode 
    (function (InvoiceNumberMode) {
        InvoiceNumberMode[InvoiceNumberMode["Default"] = 0] = "Default";
        InvoiceNumberMode[InvoiceNumberMode["ByOrderNumber"] = 1] = "ByOrderNumber";
    })(Luxena.InvoiceNumberMode || (Luxena.InvoiceNumberMode = {}));
    var InvoiceNumberMode = Luxena.InvoiceNumberMode;
    var InvoiceNumberMode;
    (function (InvoiceNumberMode) {
        InvoiceNumberMode._ns = "Luxena.Travel.Domain";
        InvoiceNumberMode._name = "InvoiceNumberMode";
        InvoiceNumberMode._fullName = "Luxena.Travel.Domain.InvoiceNumberMode";
        InvoiceNumberMode._getEdm = function (value) { return !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.InvoiceNumberMode'" + value + "'"); };
        InvoiceNumberMode._array = [
            { Id: "Default", Value: 0, Name: "По умолчанию", TextIconHtml: "", ru: "По умолчанию" },
            { Id: "ByOrderNumber", Value: 1, Name: "На основе номера заказа", TextIconHtml: "", ru: "На основе номера заказа" },
        ];
        InvoiceNumberMode._maxLength = 17;
        InvoiceNumberMode._items = {
            "0": InvoiceNumberMode._array[0],
            "Default": InvoiceNumberMode._array[0],
            "1": InvoiceNumberMode._array[1],
            "ByOrderNumber": InvoiceNumberMode._array[1],
        };
    })(InvoiceNumberMode = Luxena.InvoiceNumberMode || (Luxena.InvoiceNumberMode = {}));
    //#endregion
    //#region CheckType 
    (function (CheckType) {
        CheckType[CheckType["Unknown"] = 0] = "Unknown";
        CheckType[CheckType["Sale"] = 1] = "Sale";
        CheckType[CheckType["Return"] = 2] = "Return";
    })(Luxena.CheckType || (Luxena.CheckType = {}));
    var CheckType = Luxena.CheckType;
    var CheckType;
    (function (CheckType) {
        CheckType._ns = "Luxena.Travel.Domain";
        CheckType._name = "CheckType";
        CheckType._fullName = "Luxena.Travel.Domain.CheckType";
        CheckType._getEdm = function (value) { return !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.CheckType'" + value + "'"); };
        CheckType._array = [
            { Id: "Unknown", Value: 0, Name: "Неизвестно", TextIconHtml: "", ru: "Неизвестно" },
            { Id: "Sale", Value: 1, Name: "Чек продажи", TextIconHtml: "", ru: "Чек продажи" },
            { Id: "Return", Value: 2, Name: "Чек возврата", TextIconHtml: "", ru: "Чек возврата" },
        ];
        CheckType._maxLength = 9;
        CheckType._items = {
            "0": CheckType._array[0],
            "Unknown": CheckType._array[0],
            "1": CheckType._array[1],
            "Sale": CheckType._array[1],
            "2": CheckType._array[2],
            "Return": CheckType._array[2],
        };
    })(CheckType = Luxena.CheckType || (Luxena.CheckType = {}));
    //#endregion
    //#region CheckPaymentType 
    (function (CheckPaymentType) {
        CheckPaymentType[CheckPaymentType["Cash"] = 0] = "Cash";
        CheckPaymentType[CheckPaymentType["Credit"] = 1] = "Credit";
        CheckPaymentType[CheckPaymentType["Check"] = 2] = "Check";
        CheckPaymentType[CheckPaymentType["Card"] = 3] = "Card";
    })(Luxena.CheckPaymentType || (Luxena.CheckPaymentType = {}));
    var CheckPaymentType = Luxena.CheckPaymentType;
    var CheckPaymentType;
    (function (CheckPaymentType) {
        CheckPaymentType._ns = "Luxena.Travel.Domain";
        CheckPaymentType._name = "CheckPaymentType";
        CheckPaymentType._fullName = "Luxena.Travel.Domain.CheckPaymentType";
        CheckPaymentType._getEdm = function (value) { return !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.CheckPaymentType'" + value + "'"); };
        CheckPaymentType._array = [
            { Id: "Cash", Value: 0, Name: "Наличные", TextIconHtml: "", ru: "Наличные" },
            { Id: "Credit", Value: 1, Name: "Кредит", TextIconHtml: "", ru: "Кредит" },
            { Id: "Check", Value: 2, Name: "Чек", TextIconHtml: "", ru: "Чек" },
            { Id: "Card", Value: 3, Name: "Карточка", TextIconHtml: "", ru: "Карточка" },
        ];
        CheckPaymentType._maxLength = 6;
        CheckPaymentType._items = {
            "0": CheckPaymentType._array[0],
            "Cash": CheckPaymentType._array[0],
            "1": CheckPaymentType._array[1],
            "Credit": CheckPaymentType._array[1],
            "2": CheckPaymentType._array[2],
            "Check": CheckPaymentType._array[2],
            "3": CheckPaymentType._array[3],
            "Card": CheckPaymentType._array[3],
        };
    })(CheckPaymentType = Luxena.CheckPaymentType || (Luxena.CheckPaymentType = {}));
    //#endregion
    //#region InvoiceType 
    (function (InvoiceType) {
        InvoiceType[InvoiceType["Invoice"] = 0] = "Invoice";
        InvoiceType[InvoiceType["Receipt"] = 1] = "Receipt";
        InvoiceType[InvoiceType["CompletionCertificate"] = 2] = "CompletionCertificate";
    })(Luxena.InvoiceType || (Luxena.InvoiceType = {}));
    var InvoiceType = Luxena.InvoiceType;
    var InvoiceType;
    (function (InvoiceType) {
        InvoiceType._ns = "Luxena.Travel.Domain";
        InvoiceType._name = "InvoiceType";
        InvoiceType._fullName = "Luxena.Travel.Domain.InvoiceType";
        InvoiceType._getEdm = function (value) { return !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.InvoiceType'" + value + "'"); };
        InvoiceType._array = [
            { Id: "Invoice", Value: 0, Name: "Счет", TextIconHtml: "", ru: "Счет" },
            { Id: "Receipt", Value: 1, Name: "Квитанция", TextIconHtml: "", ru: "Квитанция" },
            { Id: "CompletionCertificate", Value: 2, Name: "Акт выполненных работ", TextIconHtml: "", ru: "Акт выполненных работ" },
        ];
        InvoiceType._maxLength = 16;
        InvoiceType._items = {
            "0": InvoiceType._array[0],
            "Invoice": InvoiceType._array[0],
            "1": InvoiceType._array[1],
            "Receipt": InvoiceType._array[1],
            "2": InvoiceType._array[2],
            "CompletionCertificate": InvoiceType._array[2],
        };
    })(InvoiceType = Luxena.InvoiceType || (Luxena.InvoiceType = {}));
    //#endregion
    //#region OrderItemLinkType 
    (function (OrderItemLinkType) {
        OrderItemLinkType[OrderItemLinkType["ProductData"] = 0] = "ProductData";
        OrderItemLinkType[OrderItemLinkType["ServiceFee"] = 1] = "ServiceFee";
        OrderItemLinkType[OrderItemLinkType["FullDocument"] = 2] = "FullDocument";
    })(Luxena.OrderItemLinkType || (Luxena.OrderItemLinkType = {}));
    var OrderItemLinkType = Luxena.OrderItemLinkType;
    var OrderItemLinkType;
    (function (OrderItemLinkType) {
        OrderItemLinkType._ns = "Luxena.Travel.Domain";
        OrderItemLinkType._name = "OrderItemLinkType";
        OrderItemLinkType._fullName = "Luxena.Travel.Domain.OrderItemLinkType";
        OrderItemLinkType._getEdm = function (value) { return !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.OrderItemLinkType'" + value + "'"); };
        OrderItemLinkType._array = [
            { Id: "ProductData", Value: 0, Name: "ProductData", TextIconHtml: "", },
            { Id: "ServiceFee", Value: 1, Name: "ServiceFee", TextIconHtml: "", },
            { Id: "FullDocument", Value: 2, Name: "FullDocument", TextIconHtml: "", },
        ];
        OrderItemLinkType._maxLength = 9;
        OrderItemLinkType._items = {
            "0": OrderItemLinkType._array[0],
            "ProductData": OrderItemLinkType._array[0],
            "1": OrderItemLinkType._array[1],
            "ServiceFee": OrderItemLinkType._array[1],
            "2": OrderItemLinkType._array[2],
            "FullDocument": OrderItemLinkType._array[2],
        };
    })(OrderItemLinkType = Luxena.OrderItemLinkType || (Luxena.OrderItemLinkType = {}));
    //#endregion
    //#region PaymentForm 
    (function (PaymentForm) {
        PaymentForm[PaymentForm["CashInOrder"] = 0] = "CashInOrder";
        PaymentForm[PaymentForm["WireTransfer"] = 1] = "WireTransfer";
        PaymentForm[PaymentForm["Check"] = 2] = "Check";
        PaymentForm[PaymentForm["Electronic"] = 3] = "Electronic";
        PaymentForm[PaymentForm["CashOutOrder"] = 4] = "CashOutOrder";
    })(Luxena.PaymentForm || (Luxena.PaymentForm = {}));
    var PaymentForm = Luxena.PaymentForm;
    var PaymentForm;
    (function (PaymentForm) {
        PaymentForm._ns = "Luxena.Travel.Domain";
        PaymentForm._name = "PaymentForm";
        PaymentForm._fullName = "Luxena.Travel.Domain.PaymentForm";
        PaymentForm._getEdm = function (value) { return !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.PaymentForm'" + value + "'"); };
        PaymentForm._array = [
            { Id: "CashInOrder", Value: 0, Name: "ПКО", TextIconHtml: "", ru: "ПКО" },
            { Id: "WireTransfer", Value: 1, Name: "Безналичный платеж", TextIconHtml: "", ru: "Безналичный платеж" },
            { Id: "Check", Value: 2, Name: "Кассовый чек", TextIconHtml: "", ru: "Кассовый чек" },
            { Id: "Electronic", Value: 3, Name: "Электронный платеж", TextIconHtml: "", ru: "Электронный платеж" },
            { Id: "CashOutOrder", Value: 4, Name: "РКО", TextIconHtml: "", ru: "РКО" },
        ];
        PaymentForm._maxLength = 14;
        PaymentForm._items = {
            "0": PaymentForm._array[0],
            "CashInOrder": PaymentForm._array[0],
            "1": PaymentForm._array[1],
            "WireTransfer": PaymentForm._array[1],
            "2": PaymentForm._array[2],
            "Check": PaymentForm._array[2],
            "3": PaymentForm._array[3],
            "Electronic": PaymentForm._array[3],
            "4": PaymentForm._array[4],
            "CashOutOrder": PaymentForm._array[4],
        };
    })(PaymentForm = Luxena.PaymentForm || (Luxena.PaymentForm = {}));
    //#endregion
    //#region ServiceFeeMode 
    (function (ServiceFeeMode) {
        ServiceFeeMode[ServiceFeeMode["Join"] = 0] = "Join";
        ServiceFeeMode[ServiceFeeMode["Separate"] = 1] = "Separate";
        ServiceFeeMode[ServiceFeeMode["AlwaysJoin"] = 2] = "AlwaysJoin";
        ServiceFeeMode[ServiceFeeMode["AlwaysSeparate"] = 3] = "AlwaysSeparate";
    })(Luxena.ServiceFeeMode || (Luxena.ServiceFeeMode = {}));
    var ServiceFeeMode = Luxena.ServiceFeeMode;
    var ServiceFeeMode;
    (function (ServiceFeeMode) {
        ServiceFeeMode._ns = "Luxena.Travel.Domain";
        ServiceFeeMode._name = "ServiceFeeMode";
        ServiceFeeMode._fullName = "Luxena.Travel.Domain.ServiceFeeMode";
        ServiceFeeMode._getEdm = function (value) { return !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.ServiceFeeMode'" + value + "'"); };
        ServiceFeeMode._array = [
            { Id: "Join", Value: 0, Name: "Join", TextIconHtml: "", },
            { Id: "Separate", Value: 1, Name: "Separate", TextIconHtml: "", },
            { Id: "AlwaysJoin", Value: 2, Name: "AlwaysJoin", TextIconHtml: "", },
            { Id: "AlwaysSeparate", Value: 3, Name: "AlwaysSeparate", TextIconHtml: "", },
        ];
        ServiceFeeMode._maxLength = 10;
        ServiceFeeMode._items = {
            "0": ServiceFeeMode._array[0],
            "Join": ServiceFeeMode._array[0],
            "1": ServiceFeeMode._array[1],
            "Separate": ServiceFeeMode._array[1],
            "2": ServiceFeeMode._array[2],
            "AlwaysJoin": ServiceFeeMode._array[2],
            "3": ServiceFeeMode._array[3],
            "AlwaysSeparate": ServiceFeeMode._array[3],
        };
    })(ServiceFeeMode = Luxena.ServiceFeeMode || (Luxena.ServiceFeeMode = {}));
    //#endregion
    //#region ProductStateFilter 
    (function (ProductStateFilter) {
        ProductStateFilter[ProductStateFilter["OnlyProcessed"] = 0] = "OnlyProcessed";
        ProductStateFilter[ProductStateFilter["All"] = 1] = "All";
        ProductStateFilter[ProductStateFilter["OnlyReservation"] = 2] = "OnlyReservation";
    })(Luxena.ProductStateFilter || (Luxena.ProductStateFilter = {}));
    var ProductStateFilter = Luxena.ProductStateFilter;
    var ProductStateFilter;
    (function (ProductStateFilter) {
        ProductStateFilter._ns = "Luxena.Travel.Domain";
        ProductStateFilter._name = "ProductStateFilter";
        ProductStateFilter._fullName = "Luxena.Travel.Domain.ProductStateFilter";
        ProductStateFilter._getEdm = function (value) { return !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.ProductStateFilter'" + value + "'"); };
        ProductStateFilter._array = [
            { Id: "OnlyProcessed", Value: 0, Name: "Только обработанные", TextIconHtml: "", ru: "Только обработанные" },
            { Id: "All", Value: 1, Name: "Все", TextIconHtml: "", ru: "Все" },
            { Id: "OnlyReservation", Value: 2, Name: "Только бронировки", TextIconHtml: "", ru: "Только бронировки" },
        ];
        ProductStateFilter._maxLength = 14;
        ProductStateFilter._items = {
            "0": ProductStateFilter._array[0],
            "OnlyProcessed": ProductStateFilter._array[0],
            "1": ProductStateFilter._array[1],
            "All": ProductStateFilter._array[1],
            "2": ProductStateFilter._array[2],
            "OnlyReservation": ProductStateFilter._array[2],
        };
    })(ProductStateFilter = Luxena.ProductStateFilter || (Luxena.ProductStateFilter = {}));
    //#endregion
    //#region GdsPassportStatus 
    (function (GdsPassportStatus) {
        GdsPassportStatus[GdsPassportStatus["Unknown"] = 0] = "Unknown";
        GdsPassportStatus[GdsPassportStatus["Exist"] = 1] = "Exist";
        GdsPassportStatus[GdsPassportStatus["NotExist"] = 2] = "NotExist";
        GdsPassportStatus[GdsPassportStatus["Incorrect"] = 3] = "Incorrect";
    })(Luxena.GdsPassportStatus || (Luxena.GdsPassportStatus = {}));
    var GdsPassportStatus = Luxena.GdsPassportStatus;
    var GdsPassportStatus;
    (function (GdsPassportStatus) {
        GdsPassportStatus._ns = "Luxena.Travel.Domain";
        GdsPassportStatus._name = "GdsPassportStatus";
        GdsPassportStatus._fullName = "Luxena.Travel.Domain.GdsPassportStatus";
        GdsPassportStatus._getEdm = function (value) { return !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.GdsPassportStatus'" + value + "'"); };
        GdsPassportStatus._array = [
            { Id: "Unknown", Value: 0, Name: "Неизвестно", TextIconHtml: "", ru: "Неизвестно" },
            { Id: "Exist", Value: 1, Name: "Есть", TextIconHtml: "", ru: "Есть" },
            { Id: "NotExist", Value: 2, Name: "Нет", TextIconHtml: "", ru: "Нет" },
            { Id: "Incorrect", Value: 3, Name: "Некорректен", TextIconHtml: "", ru: "Некорректен" },
        ];
        GdsPassportStatus._maxLength = 8;
        GdsPassportStatus._items = {
            "0": GdsPassportStatus._array[0],
            "Unknown": GdsPassportStatus._array[0],
            "1": GdsPassportStatus._array[1],
            "Exist": GdsPassportStatus._array[1],
            "2": GdsPassportStatus._array[2],
            "NotExist": GdsPassportStatus._array[2],
            "3": GdsPassportStatus._array[3],
            "Incorrect": GdsPassportStatus._array[3],
        };
    })(GdsPassportStatus = Luxena.GdsPassportStatus || (Luxena.GdsPassportStatus = {}));
    //#endregion
    //#region AirlinePassportRequirement 
    (function (AirlinePassportRequirement) {
        AirlinePassportRequirement[AirlinePassportRequirement["SystemDefault"] = 0] = "SystemDefault";
        AirlinePassportRequirement[AirlinePassportRequirement["Required"] = 1] = "Required";
        AirlinePassportRequirement[AirlinePassportRequirement["NotRequired"] = 2] = "NotRequired";
    })(Luxena.AirlinePassportRequirement || (Luxena.AirlinePassportRequirement = {}));
    var AirlinePassportRequirement = Luxena.AirlinePassportRequirement;
    var AirlinePassportRequirement;
    (function (AirlinePassportRequirement) {
        AirlinePassportRequirement._ns = "Luxena.Travel.Domain";
        AirlinePassportRequirement._name = "AirlinePassportRequirement";
        AirlinePassportRequirement._fullName = "Luxena.Travel.Domain.AirlinePassportRequirement";
        AirlinePassportRequirement._getEdm = function (value) { return !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.AirlinePassportRequirement'" + value + "'"); };
        AirlinePassportRequirement._array = [
            { Id: "SystemDefault", Value: 0, Name: "По умолчанию", TextIconHtml: "", ru: "По умолчанию" },
            { Id: "Required", Value: 1, Name: "Требуется", TextIconHtml: "", ru: "Требуется" },
            { Id: "NotRequired", Value: 2, Name: "Не требуется", TextIconHtml: "", ru: "Не требуется" },
        ];
        AirlinePassportRequirement._maxLength = 9;
        AirlinePassportRequirement._items = {
            "0": AirlinePassportRequirement._array[0],
            "SystemDefault": AirlinePassportRequirement._array[0],
            "1": AirlinePassportRequirement._array[1],
            "Required": AirlinePassportRequirement._array[1],
            "2": AirlinePassportRequirement._array[2],
            "NotRequired": AirlinePassportRequirement._array[2],
        };
    })(AirlinePassportRequirement = Luxena.AirlinePassportRequirement || (Luxena.AirlinePassportRequirement = {}));
    //#endregion
    //#region Gender 
    (function (Gender) {
        Gender[Gender["Male"] = 0] = "Male";
        Gender[Gender["Female"] = 1] = "Female";
    })(Luxena.Gender || (Luxena.Gender = {}));
    var Gender = Luxena.Gender;
    var Gender;
    (function (Gender) {
        Gender._ns = "Luxena.Travel.Domain";
        Gender._name = "Gender";
        Gender._fullName = "Luxena.Travel.Domain.Gender";
        Gender._getEdm = function (value) { return !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.Gender'" + value + "'"); };
        Gender._array = [
            { Id: "Male", Value: 0, Name: "Мужской", TextIconHtml: "", ru: "Мужской" },
            { Id: "Female", Value: 1, Name: "Женский", TextIconHtml: "", ru: "Женский" },
        ];
        Gender._maxLength = 5;
        Gender._items = {
            "0": Gender._array[0],
            "Male": Gender._array[0],
            "1": Gender._array[1],
            "Female": Gender._array[1],
        };
    })(Gender = Luxena.Gender || (Luxena.Gender = {}));
    //#endregion
    //#region PartyType 
    (function (PartyType) {
        PartyType[PartyType["Department"] = 0] = "Department";
        PartyType[PartyType["Organization"] = 1] = "Organization";
        PartyType[PartyType["Person"] = 2] = "Person";
    })(Luxena.PartyType || (Luxena.PartyType = {}));
    var PartyType = Luxena.PartyType;
    var PartyType;
    (function (PartyType) {
        PartyType._ns = "Luxena.Travel.Domain";
        PartyType._name = "PartyType";
        PartyType._fullName = "Luxena.Travel.Domain.PartyType";
        PartyType._getEdm = function (value) { return !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.PartyType'" + value + "'"); };
        PartyType._array = [
            { Id: "Department", Value: 0, Name: "Подразделение", TextIconHtml: "", ru: "Подразделение", rus: "Подразделения" },
            { Id: "Organization", Value: 1, Name: "Организация", Icon: "group", TextIconHtml: Luxena.getTextIconHtml("group"), ru: "Организация", rus: "Организации" },
            { Id: "Person", Value: 2, Name: "Персона", Icon: "user", TextIconHtml: Luxena.getTextIconHtml("user"), ru: "Персона", rus: "Персоны" },
        ];
        PartyType._maxLength = 10;
        PartyType._items = {
            "0": PartyType._array[0],
            "Department": PartyType._array[0],
            "1": PartyType._array[1],
            "Organization": PartyType._array[1],
            "2": PartyType._array[2],
            "Person": PartyType._array[2],
        };
    })(PartyType = Luxena.PartyType || (Luxena.PartyType = {}));
    //#endregion
    //#region AmadeusRizUsingMode 
    (function (AmadeusRizUsingMode) {
        AmadeusRizUsingMode[AmadeusRizUsingMode["None"] = 0] = "None";
        AmadeusRizUsingMode[AmadeusRizUsingMode["ServiceFeeOnly"] = 1] = "ServiceFeeOnly";
        AmadeusRizUsingMode[AmadeusRizUsingMode["All"] = 2] = "All";
    })(Luxena.AmadeusRizUsingMode || (Luxena.AmadeusRizUsingMode = {}));
    var AmadeusRizUsingMode = Luxena.AmadeusRizUsingMode;
    var AmadeusRizUsingMode;
    (function (AmadeusRizUsingMode) {
        AmadeusRizUsingMode._ns = "Luxena.Travel.Domain";
        AmadeusRizUsingMode._name = "AmadeusRizUsingMode";
        AmadeusRizUsingMode._fullName = "Luxena.Travel.Domain.AmadeusRizUsingMode";
        AmadeusRizUsingMode._getEdm = function (value) { return !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.AmadeusRizUsingMode'" + value + "'"); };
        AmadeusRizUsingMode._array = [
            { Id: "None", Value: 0, Name: "Не использовать", TextIconHtml: "", ru: "Не использовать" },
            { Id: "ServiceFeeOnly", Value: 1, Name: "Использовать сервисный сбор", TextIconHtml: "", ru: "Использовать сервисный сбор" },
            { Id: "All", Value: 2, Name: "Использовать полностью", TextIconHtml: "", ru: "Использовать полностью" },
        ];
        AmadeusRizUsingMode._maxLength = 20;
        AmadeusRizUsingMode._items = {
            "0": AmadeusRizUsingMode._array[0],
            "None": AmadeusRizUsingMode._array[0],
            "1": AmadeusRizUsingMode._array[1],
            "ServiceFeeOnly": AmadeusRizUsingMode._array[1],
            "2": AmadeusRizUsingMode._array[2],
            "All": AmadeusRizUsingMode._array[2],
        };
    })(AmadeusRizUsingMode = Luxena.AmadeusRizUsingMode || (Luxena.AmadeusRizUsingMode = {}));
    //#endregion
    //#region AviaDocumentVatOptions 
    (function (AviaDocumentVatOptions) {
        AviaDocumentVatOptions[AviaDocumentVatOptions["UseHFTax"] = 0] = "UseHFTax";
        AviaDocumentVatOptions[AviaDocumentVatOptions["TaxAirlineTotal"] = 1] = "TaxAirlineTotal";
    })(Luxena.AviaDocumentVatOptions || (Luxena.AviaDocumentVatOptions = {}));
    var AviaDocumentVatOptions = Luxena.AviaDocumentVatOptions;
    var AviaDocumentVatOptions;
    (function (AviaDocumentVatOptions) {
        AviaDocumentVatOptions._ns = "Luxena.Travel.Domain";
        AviaDocumentVatOptions._name = "AviaDocumentVatOptions";
        AviaDocumentVatOptions._fullName = "Luxena.Travel.Domain.AviaDocumentVatOptions";
        AviaDocumentVatOptions._getEdm = function (value) { return !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.AviaDocumentVatOptions'" + value + "'"); };
        AviaDocumentVatOptions._array = [
            { Id: "UseHFTax", Value: 0, Name: "Использовать HF таксу", TextIconHtml: "", ru: "Использовать HF таксу" },
            { Id: "TaxAirlineTotal", Value: 1, Name: "Рассчитывать от итога", TextIconHtml: "", ru: "Рассчитывать от итога" },
        ];
        AviaDocumentVatOptions._maxLength = 16;
        AviaDocumentVatOptions._items = {
            "0": AviaDocumentVatOptions._array[0],
            "UseHFTax": AviaDocumentVatOptions._array[0],
            "1": AviaDocumentVatOptions._array[1],
            "TaxAirlineTotal": AviaDocumentVatOptions._array[1],
        };
    })(AviaDocumentVatOptions = Luxena.AviaDocumentVatOptions || (Luxena.AviaDocumentVatOptions = {}));
    //#endregion
    //#region ProductOrderItemGenerationOption 
    (function (ProductOrderItemGenerationOption) {
        ProductOrderItemGenerationOption[ProductOrderItemGenerationOption["AlwaysOneOrderItem"] = 0] = "AlwaysOneOrderItem";
        ProductOrderItemGenerationOption[ProductOrderItemGenerationOption["SeparateServiceFee"] = 1] = "SeparateServiceFee";
        ProductOrderItemGenerationOption[ProductOrderItemGenerationOption["ManualSetting"] = 2] = "ManualSetting";
    })(Luxena.ProductOrderItemGenerationOption || (Luxena.ProductOrderItemGenerationOption = {}));
    var ProductOrderItemGenerationOption = Luxena.ProductOrderItemGenerationOption;
    var ProductOrderItemGenerationOption;
    (function (ProductOrderItemGenerationOption) {
        ProductOrderItemGenerationOption._ns = "Luxena.Travel.Domain";
        ProductOrderItemGenerationOption._name = "ProductOrderItemGenerationOption";
        ProductOrderItemGenerationOption._fullName = "Luxena.Travel.Domain.ProductOrderItemGenerationOption";
        ProductOrderItemGenerationOption._getEdm = function (value) { return !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.ProductOrderItemGenerationOption'" + value + "'"); };
        ProductOrderItemGenerationOption._array = [
            { Id: "AlwaysOneOrderItem", Value: 0, Name: "Всегда одна позиция", TextIconHtml: "", ru: "Всегда одна позиция" },
            { Id: "SeparateServiceFee", Value: 1, Name: "Cервисный сбор отдельной позицией", TextIconHtml: "", ru: "Cервисный сбор отдельной позицией" },
            { Id: "ManualSetting", Value: 2, Name: "Настраивать вручную", TextIconHtml: "", ru: "Настраивать вручную" },
        ];
        ProductOrderItemGenerationOption._maxLength = 25;
        ProductOrderItemGenerationOption._items = {
            "0": ProductOrderItemGenerationOption._array[0],
            "AlwaysOneOrderItem": ProductOrderItemGenerationOption._array[0],
            "1": ProductOrderItemGenerationOption._array[1],
            "SeparateServiceFee": ProductOrderItemGenerationOption._array[1],
            "2": ProductOrderItemGenerationOption._array[2],
            "ManualSetting": ProductOrderItemGenerationOption._array[2],
        };
    })(ProductOrderItemGenerationOption = Luxena.ProductOrderItemGenerationOption || (Luxena.ProductOrderItemGenerationOption = {}));
    //#endregion
    //#region GdsFileType 
    (function (GdsFileType) {
        GdsFileType[GdsFileType["AirFile"] = 0] = "AirFile";
        GdsFileType[GdsFileType["MirFile"] = 1] = "MirFile";
        GdsFileType[GdsFileType["TktFile"] = 2] = "TktFile";
        GdsFileType[GdsFileType["PrintFile"] = 3] = "PrintFile";
        GdsFileType[GdsFileType["SirenaFile"] = 4] = "SirenaFile";
        GdsFileType[GdsFileType["GalileoXmlFile"] = 5] = "GalileoXmlFile";
        GdsFileType[GdsFileType["AmadeusXmlFile"] = 6] = "AmadeusXmlFile";
    })(Luxena.GdsFileType || (Luxena.GdsFileType = {}));
    var GdsFileType = Luxena.GdsFileType;
    var GdsFileType;
    (function (GdsFileType) {
        GdsFileType._ns = "Luxena.Travel.Domain";
        GdsFileType._name = "GdsFileType";
        GdsFileType._fullName = "Luxena.Travel.Domain.GdsFileType";
        GdsFileType._getEdm = function (value) { return !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.GdsFileType'" + value + "'"); };
        GdsFileType._array = [
            { Id: "AirFile", Value: 0, Name: "AirFile", TextIconHtml: "", },
            { Id: "MirFile", Value: 1, Name: "MirFile", TextIconHtml: "", },
            { Id: "TktFile", Value: 2, Name: "TktFile", TextIconHtml: "", },
            { Id: "PrintFile", Value: 3, Name: "PrintFile", TextIconHtml: "", },
            { Id: "SirenaFile", Value: 4, Name: "SirenaFile", TextIconHtml: "", },
            { Id: "GalileoXmlFile", Value: 5, Name: "GalileoXmlFile", TextIconHtml: "", },
            { Id: "AmadeusXmlFile", Value: 6, Name: "AmadeusXmlFile", TextIconHtml: "", },
        ];
        GdsFileType._maxLength = 10;
        GdsFileType._items = {
            "0": GdsFileType._array[0],
            "AirFile": GdsFileType._array[0],
            "1": GdsFileType._array[1],
            "MirFile": GdsFileType._array[1],
            "2": GdsFileType._array[2],
            "TktFile": GdsFileType._array[2],
            "3": GdsFileType._array[3],
            "PrintFile": GdsFileType._array[3],
            "4": GdsFileType._array[4],
            "SirenaFile": GdsFileType._array[4],
            "5": GdsFileType._array[5],
            "GalileoXmlFile": GdsFileType._array[5],
            "6": GdsFileType._array[6],
            "AmadeusXmlFile": GdsFileType._array[6],
        };
    })(GdsFileType = Luxena.GdsFileType || (Luxena.GdsFileType = {}));
    //#endregion
    //#region GdsOriginator 
    (function (GdsOriginator) {
        GdsOriginator[GdsOriginator["Unknown"] = 0] = "Unknown";
        GdsOriginator[GdsOriginator["Amadeus"] = 1] = "Amadeus";
        GdsOriginator[GdsOriginator["Galileo"] = 2] = "Galileo";
        GdsOriginator[GdsOriginator["Sirena"] = 3] = "Sirena";
        GdsOriginator[GdsOriginator["Airline"] = 4] = "Airline";
        GdsOriginator[GdsOriginator["Gabriel"] = 5] = "Gabriel";
        GdsOriginator[GdsOriginator["WizzAir"] = 6] = "WizzAir";
        GdsOriginator[GdsOriginator["IATI"] = 7] = "IATI";
        GdsOriginator[GdsOriginator["ETravels"] = 8] = "ETravels";
        GdsOriginator[GdsOriginator["TicketConsolidator"] = 9] = "TicketConsolidator";
        GdsOriginator[GdsOriginator["DeltaTravel"] = 10] = "DeltaTravel";
        GdsOriginator[GdsOriginator["TicketsUA"] = 11] = "TicketsUA";
        GdsOriginator[GdsOriginator["FlyDubai"] = 12] = "FlyDubai";
        GdsOriginator[GdsOriginator["AirArabia"] = 13] = "AirArabia";
        GdsOriginator[GdsOriginator["Pegasus"] = 14] = "Pegasus";
    })(Luxena.GdsOriginator || (Luxena.GdsOriginator = {}));
    var GdsOriginator = Luxena.GdsOriginator;
    var GdsOriginator;
    (function (GdsOriginator) {
        GdsOriginator._ns = "Luxena.Travel.Domain";
        GdsOriginator._name = "GdsOriginator";
        GdsOriginator._fullName = "Luxena.Travel.Domain.GdsOriginator";
        GdsOriginator._getEdm = function (value) { return !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.GdsOriginator'" + value + "'"); };
        GdsOriginator._array = [
            { Id: "Unknown", Value: 0, Name: "Unknown", TextIconHtml: "", },
            { Id: "Amadeus", Value: 1, Name: "Amadeus", TextIconHtml: "", },
            { Id: "Galileo", Value: 2, Name: "Galileo", TextIconHtml: "", },
            { Id: "Sirena", Value: 3, Name: "Sirena", TextIconHtml: "", },
            { Id: "Airline", Value: 4, Name: "Airline", TextIconHtml: "", },
            { Id: "Gabriel", Value: 5, Name: "Gabriel", TextIconHtml: "", },
            { Id: "WizzAir", Value: 6, Name: "Wizz Air", TextIconHtml: "", ru: "Wizz Air" },
            { Id: "IATI", Value: 7, Name: "IATI", TextIconHtml: "", },
            { Id: "ETravels", Value: 8, Name: "E-Travels", TextIconHtml: "", ru: "E-Travels" },
            { Id: "TicketConsolidator", Value: 9, Name: "Ticket Consolidator", TextIconHtml: "", ru: "Ticket Consolidator" },
            { Id: "DeltaTravel", Value: 10, Name: "Delta TRAVEL", TextIconHtml: "", ru: "Delta TRAVEL" },
            { Id: "TicketsUA", Value: 11, Name: "Tickets.UA", TextIconHtml: "", ru: "Tickets.UA" },
            { Id: "FlyDubai", Value: 12, Name: "Fly Dubai", TextIconHtml: "", ru: "Fly Dubai" },
            { Id: "AirArabia", Value: 13, Name: "Air Arabia", TextIconHtml: "", ru: "Air Arabia" },
            { Id: "Pegasus", Value: 14, Name: "Pegasus", TextIconHtml: "", },
        ];
        GdsOriginator._maxLength = 14;
        GdsOriginator._items = {
            "0": GdsOriginator._array[0],
            "Unknown": GdsOriginator._array[0],
            "1": GdsOriginator._array[1],
            "Amadeus": GdsOriginator._array[1],
            "2": GdsOriginator._array[2],
            "Galileo": GdsOriginator._array[2],
            "3": GdsOriginator._array[3],
            "Sirena": GdsOriginator._array[3],
            "4": GdsOriginator._array[4],
            "Airline": GdsOriginator._array[4],
            "5": GdsOriginator._array[5],
            "Gabriel": GdsOriginator._array[5],
            "6": GdsOriginator._array[6],
            "WizzAir": GdsOriginator._array[6],
            "7": GdsOriginator._array[7],
            "IATI": GdsOriginator._array[7],
            "8": GdsOriginator._array[8],
            "ETravels": GdsOriginator._array[8],
            "9": GdsOriginator._array[9],
            "TicketConsolidator": GdsOriginator._array[9],
            "10": GdsOriginator._array[10],
            "DeltaTravel": GdsOriginator._array[10],
            "11": GdsOriginator._array[11],
            "TicketsUA": GdsOriginator._array[11],
            "12": GdsOriginator._array[12],
            "FlyDubai": GdsOriginator._array[12],
            "13": GdsOriginator._array[13],
            "AirArabia": GdsOriginator._array[13],
            "14": GdsOriginator._array[14],
            "Pegasus": GdsOriginator._array[14],
        };
    })(GdsOriginator = Luxena.GdsOriginator || (Luxena.GdsOriginator = {}));
    //#endregion
    //#region FlightSegmentType 
    (function (FlightSegmentType) {
        FlightSegmentType[FlightSegmentType["Ticketed"] = 0] = "Ticketed";
        FlightSegmentType[FlightSegmentType["Unticketed"] = 1] = "Unticketed";
        FlightSegmentType[FlightSegmentType["Voided"] = 2] = "Voided";
    })(Luxena.FlightSegmentType || (Luxena.FlightSegmentType = {}));
    var FlightSegmentType = Luxena.FlightSegmentType;
    var FlightSegmentType;
    (function (FlightSegmentType) {
        FlightSegmentType._ns = "Luxena.Travel.Domain";
        FlightSegmentType._name = "FlightSegmentType";
        FlightSegmentType._fullName = "Luxena.Travel.Domain.FlightSegmentType";
        FlightSegmentType._getEdm = function (value) { return !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.FlightSegmentType'" + value + "'"); };
        FlightSegmentType._array = [
            { Id: "Ticketed", Value: 0, Name: "Ticketed", TextIconHtml: "", },
            { Id: "Unticketed", Value: 1, Name: "Unticketed", TextIconHtml: "", },
            { Id: "Voided", Value: 2, Name: "Voided", TextIconHtml: "", },
        ];
        FlightSegmentType._maxLength = 8;
        FlightSegmentType._items = {
            "0": FlightSegmentType._array[0],
            "Ticketed": FlightSegmentType._array[0],
            "1": FlightSegmentType._array[1],
            "Unticketed": FlightSegmentType._array[1],
            "2": FlightSegmentType._array[2],
            "Voided": FlightSegmentType._array[2],
        };
    })(FlightSegmentType = Luxena.FlightSegmentType || (Luxena.FlightSegmentType = {}));
    //#endregion
    //#region ImportResult 
    (function (ImportResult) {
        ImportResult[ImportResult["None"] = 0] = "None";
        ImportResult[ImportResult["Success"] = 1] = "Success";
        ImportResult[ImportResult["Error"] = 2] = "Error";
        ImportResult[ImportResult["Warn"] = 3] = "Warn";
    })(Luxena.ImportResult || (Luxena.ImportResult = {}));
    var ImportResult = Luxena.ImportResult;
    var ImportResult;
    (function (ImportResult) {
        ImportResult._ns = "Luxena.Travel.Domain";
        ImportResult._name = "ImportResult";
        ImportResult._fullName = "Luxena.Travel.Domain.ImportResult";
        ImportResult._getEdm = function (value) { return !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.ImportResult'" + value + "'"); };
        ImportResult._array = [
            { Id: "None", Value: 0, Name: "None", TextIconHtml: "", },
            { Id: "Success", Value: 1, Name: "Success", TextIconHtml: "", },
            { Id: "Error", Value: 2, Name: "Error", TextIconHtml: "", },
            { Id: "Warn", Value: 3, Name: "Warn", TextIconHtml: "", },
        ];
        ImportResult._maxLength = 5;
        ImportResult._items = {
            "0": ImportResult._array[0],
            "None": ImportResult._array[0],
            "1": ImportResult._array[1],
            "Success": ImportResult._array[1],
            "2": ImportResult._array[2],
            "Error": ImportResult._array[2],
            "3": ImportResult._array[3],
            "Warn": ImportResult._array[3],
        };
    })(ImportResult = Luxena.ImportResult || (Luxena.ImportResult = {}));
    //#endregion
    //#region IsicCardType 
    (function (IsicCardType) {
        IsicCardType[IsicCardType["Unknown"] = 0] = "Unknown";
        IsicCardType[IsicCardType["Isic"] = 1] = "Isic";
        IsicCardType[IsicCardType["ITIC"] = 2] = "ITIC";
        IsicCardType[IsicCardType["IYTC"] = 3] = "IYTC";
    })(Luxena.IsicCardType || (Luxena.IsicCardType = {}));
    var IsicCardType = Luxena.IsicCardType;
    var IsicCardType;
    (function (IsicCardType) {
        IsicCardType._ns = "Luxena.Travel.Domain";
        IsicCardType._name = "IsicCardType";
        IsicCardType._fullName = "Luxena.Travel.Domain.IsicCardType";
        IsicCardType._getEdm = function (value) { return !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.IsicCardType'" + value + "'"); };
        IsicCardType._array = [
            { Id: "Unknown", Value: 0, Name: "Неизвестно", TextIconHtml: "", ru: "Неизвестно" },
            { Id: "Isic", Value: 1, Name: "Isic", TextIconHtml: "", },
            { Id: "ITIC", Value: 2, Name: "ITIC", TextIconHtml: "", },
            { Id: "IYTC", Value: 3, Name: "IYTC", TextIconHtml: "", },
        ];
        IsicCardType._maxLength = 8;
        IsicCardType._items = {
            "0": IsicCardType._array[0],
            "Unknown": IsicCardType._array[0],
            "1": IsicCardType._array[1],
            "Isic": IsicCardType._array[1],
            "2": IsicCardType._array[2],
            "ITIC": IsicCardType._array[2],
            "3": IsicCardType._array[3],
            "IYTC": IsicCardType._array[3],
        };
    })(IsicCardType = Luxena.IsicCardType || (Luxena.IsicCardType = {}));
    //#endregion
    //#region MealType 
    (function (MealType) {
        MealType[MealType["NoData"] = 0] = "NoData";
        MealType[MealType["Breakfast"] = 1] = "Breakfast";
        MealType[MealType["ContinentalBreakfast"] = 2] = "ContinentalBreakfast";
        MealType[MealType["Lunch"] = 4] = "Lunch";
        MealType[MealType["Dinner"] = 8] = "Dinner";
        MealType[MealType["Snack"] = 16] = "Snack";
        MealType[MealType["ColdMeal"] = 32] = "ColdMeal";
        MealType[MealType["HotMeal"] = 64] = "HotMeal";
        MealType[MealType["Meal"] = 128] = "Meal";
        MealType[MealType["Refreshment"] = 256] = "Refreshment";
        MealType[MealType["AlcoholicComplimentaryBeverages"] = 512] = "AlcoholicComplimentaryBeverages";
        MealType[MealType["Food"] = 1024] = "Food";
        MealType[MealType["AlcoholicBeveragesForPurchase"] = 2048] = "AlcoholicBeveragesForPurchase";
        MealType[MealType["DutyFree"] = 4096] = "DutyFree";
    })(Luxena.MealType || (Luxena.MealType = {}));
    var MealType = Luxena.MealType;
    var MealType;
    (function (MealType) {
        MealType._ns = "Luxena.Travel.Domain";
        MealType._name = "MealType";
        MealType._fullName = "Luxena.Travel.Domain.MealType";
        MealType._getEdm = function (value) { return !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.MealType'" + value + "'"); };
        MealType._isFlags = true;
        MealType._array = [
            { Id: "NoData", Value: 0, Name: "нет", TextIconHtml: "", ru: "нет" },
            { Id: "Breakfast", Value: 1, Name: "Завтрак", TextIconHtml: "", ru: "Завтрак" },
            { Id: "ContinentalBreakfast", Value: 2, Name: "Континентальный завтрак", TextIconHtml: "", ru: "Континентальный завтрак" },
            { Id: "Lunch", Value: 4, Name: "Ланч", TextIconHtml: "", ru: "Ланч" },
            { Id: "Dinner", Value: 8, Name: "Обед", TextIconHtml: "", ru: "Обед" },
            { Id: "Snack", Value: 16, Name: "Закуска", TextIconHtml: "", ru: "Закуска" },
            { Id: "ColdMeal", Value: 32, Name: "Холодная еда", TextIconHtml: "", ru: "Холодная еда" },
            { Id: "HotMeal", Value: 64, Name: "Горячая еда", TextIconHtml: "", ru: "Горячая еда" },
            { Id: "Meal", Value: 128, Name: "Еда", TextIconHtml: "", ru: "Еда" },
            { Id: "Refreshment", Value: 256, Name: "Напитки", TextIconHtml: "", ru: "Напитки" },
            { Id: "AlcoholicComplimentaryBeverages", Value: 512, Name: "Бесплатные алкогольные напитки", TextIconHtml: "", ru: "Бесплатные алкогольные напитки" },
            { Id: "Food", Value: 1024, Name: "Еда", TextIconHtml: "", ru: "Еда" },
            { Id: "AlcoholicBeveragesForPurchase", Value: 2048, Name: "Платные алкогольные напитки", TextIconHtml: "", ru: "Платные алкогольные напитки" },
            { Id: "DutyFree", Value: 4096, Name: "DutyFree", TextIconHtml: "", ru: "DutyFree" },
        ];
        MealType._maxLength = 22;
        MealType._items = {
            "0": MealType._array[0],
            "NoData": MealType._array[0],
            "1": MealType._array[1],
            "Breakfast": MealType._array[1],
            "2": MealType._array[2],
            "ContinentalBreakfast": MealType._array[2],
            "4": MealType._array[3],
            "Lunch": MealType._array[3],
            "8": MealType._array[4],
            "Dinner": MealType._array[4],
            "16": MealType._array[5],
            "Snack": MealType._array[5],
            "32": MealType._array[6],
            "ColdMeal": MealType._array[6],
            "64": MealType._array[7],
            "HotMeal": MealType._array[7],
            "128": MealType._array[8],
            "Meal": MealType._array[8],
            "256": MealType._array[9],
            "Refreshment": MealType._array[9],
            "512": MealType._array[10],
            "AlcoholicComplimentaryBeverages": MealType._array[10],
            "1024": MealType._array[11],
            "Food": MealType._array[11],
            "2048": MealType._array[12],
            "AlcoholicBeveragesForPurchase": MealType._array[12],
            "4096": MealType._array[13],
            "DutyFree": MealType._array[13],
        };
    })(MealType = Luxena.MealType || (Luxena.MealType = {}));
    //#endregion
    //#region PasteboardServiceClass 
    (function (PasteboardServiceClass) {
        PasteboardServiceClass[PasteboardServiceClass["FirstClass"] = 0] = "FirstClass";
        PasteboardServiceClass[PasteboardServiceClass["SecondClass"] = 1] = "SecondClass";
        PasteboardServiceClass[PasteboardServiceClass["LuxuryCoupe"] = 2] = "LuxuryCoupe";
        PasteboardServiceClass[PasteboardServiceClass["ReservedSeat"] = 3] = "ReservedSeat";
        PasteboardServiceClass[PasteboardServiceClass["Compartment"] = 4] = "Compartment";
        PasteboardServiceClass[PasteboardServiceClass["Unknown"] = 5] = "Unknown";
    })(Luxena.PasteboardServiceClass || (Luxena.PasteboardServiceClass = {}));
    var PasteboardServiceClass = Luxena.PasteboardServiceClass;
    var PasteboardServiceClass;
    (function (PasteboardServiceClass) {
        PasteboardServiceClass._ns = "Luxena.Travel.Domain";
        PasteboardServiceClass._name = "PasteboardServiceClass";
        PasteboardServiceClass._fullName = "Luxena.Travel.Domain.PasteboardServiceClass";
        PasteboardServiceClass._getEdm = function (value) { return !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.PasteboardServiceClass'" + value + "'"); };
        PasteboardServiceClass._array = [
            { Id: "FirstClass", Value: 0, Name: "1-й класс", TextIconHtml: "", ru: "1-й класс" },
            { Id: "SecondClass", Value: 1, Name: "2-й класс", TextIconHtml: "", ru: "2-й класс" },
            { Id: "LuxuryCoupe", Value: 2, Name: "люкс", TextIconHtml: "", ru: "люкс" },
            { Id: "ReservedSeat", Value: 3, Name: "плацкарт", TextIconHtml: "", ru: "плацкарт" },
            { Id: "Compartment", Value: 4, Name: "купе", TextIconHtml: "", ru: "купе" },
            { Id: "Unknown", Value: 5, Name: "Неизвестно", TextIconHtml: "", ru: "Неизвестно" },
        ];
        PasteboardServiceClass._maxLength = 8;
        PasteboardServiceClass._items = {
            "0": PasteboardServiceClass._array[0],
            "FirstClass": PasteboardServiceClass._array[0],
            "1": PasteboardServiceClass._array[1],
            "SecondClass": PasteboardServiceClass._array[1],
            "2": PasteboardServiceClass._array[2],
            "LuxuryCoupe": PasteboardServiceClass._array[2],
            "3": PasteboardServiceClass._array[3],
            "ReservedSeat": PasteboardServiceClass._array[3],
            "4": PasteboardServiceClass._array[4],
            "Compartment": PasteboardServiceClass._array[4],
            "5": PasteboardServiceClass._array[5],
            "Unknown": PasteboardServiceClass._array[5],
        };
    })(PasteboardServiceClass = Luxena.PasteboardServiceClass || (Luxena.PasteboardServiceClass = {}));
    //#endregion
    //#region PaymentType 
    (function (PaymentType) {
        PaymentType[PaymentType["Unknown"] = 0] = "Unknown";
        PaymentType[PaymentType["Cash"] = 1] = "Cash";
        PaymentType[PaymentType["Invoice"] = 2] = "Invoice";
        PaymentType[PaymentType["Check"] = 3] = "Check";
        PaymentType[PaymentType["CreditCard"] = 4] = "CreditCard";
        PaymentType[PaymentType["Exchange"] = 5] = "Exchange";
        PaymentType[PaymentType["WithoutPayment"] = 6] = "WithoutPayment";
    })(Luxena.PaymentType || (Luxena.PaymentType = {}));
    var PaymentType = Luxena.PaymentType;
    var PaymentType;
    (function (PaymentType) {
        PaymentType._ns = "Luxena.Travel.Domain";
        PaymentType._name = "PaymentType";
        PaymentType._fullName = "Luxena.Travel.Domain.PaymentType";
        PaymentType._getEdm = function (value) { return !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.PaymentType'" + value + "'"); };
        PaymentType._array = [
            { Id: "Unknown", Value: 0, Name: "Unknown", TextIconHtml: "", },
            { Id: "Cash", Value: 1, Name: "Cash", TextIconHtml: "", },
            { Id: "Invoice", Value: 2, Name: "Invoice", TextIconHtml: "", },
            { Id: "Check", Value: 3, Name: "Check", TextIconHtml: "", },
            { Id: "CreditCard", Value: 4, Name: "CreditCard", TextIconHtml: "", },
            { Id: "Exchange", Value: 5, Name: "Exchange", TextIconHtml: "", },
            { Id: "WithoutPayment", Value: 6, Name: "Без оплаты", TextIconHtml: "", ru: "Без оплаты" },
        ];
        PaymentType._maxLength = 8;
        PaymentType._items = {
            "0": PaymentType._array[0],
            "Unknown": PaymentType._array[0],
            "1": PaymentType._array[1],
            "Cash": PaymentType._array[1],
            "2": PaymentType._array[2],
            "Invoice": PaymentType._array[2],
            "3": PaymentType._array[3],
            "Check": PaymentType._array[3],
            "4": PaymentType._array[4],
            "CreditCard": PaymentType._array[4],
            "5": PaymentType._array[5],
            "Exchange": PaymentType._array[5],
            "6": PaymentType._array[6],
            "WithoutPayment": PaymentType._array[6],
        };
    })(PaymentType = Luxena.PaymentType || (Luxena.PaymentType = {}));
    //#endregion
    //#region ProductOrigin 
    (function (ProductOrigin) {
        ProductOrigin[ProductOrigin["AmadeusAir"] = 0] = "AmadeusAir";
        ProductOrigin[ProductOrigin["AmadeusPrint"] = 1] = "AmadeusPrint";
        ProductOrigin[ProductOrigin["GalileoMir"] = 2] = "GalileoMir";
        ProductOrigin[ProductOrigin["GalileoTkt"] = 3] = "GalileoTkt";
        ProductOrigin[ProductOrigin["BspLink"] = 4] = "BspLink";
        ProductOrigin[ProductOrigin["Manual"] = 5] = "Manual";
        ProductOrigin[ProductOrigin["SirenaXml"] = 6] = "SirenaXml";
        ProductOrigin[ProductOrigin["GalileoXml"] = 7] = "GalileoXml";
        ProductOrigin[ProductOrigin["AmadeusXml"] = 8] = "AmadeusXml";
    })(Luxena.ProductOrigin || (Luxena.ProductOrigin = {}));
    var ProductOrigin = Luxena.ProductOrigin;
    var ProductOrigin;
    (function (ProductOrigin) {
        ProductOrigin._ns = "Luxena.Travel.Domain";
        ProductOrigin._name = "ProductOrigin";
        ProductOrigin._fullName = "Luxena.Travel.Domain.ProductOrigin";
        ProductOrigin._getEdm = function (value) { return !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.ProductOrigin'" + value + "'"); };
        ProductOrigin._array = [
            { Id: "AmadeusAir", Value: 0, Name: "AmadeusAir", TextIconHtml: "", },
            { Id: "AmadeusPrint", Value: 1, Name: "AmadeusPrint", TextIconHtml: "", },
            { Id: "GalileoMir", Value: 2, Name: "GalileoMir", TextIconHtml: "", },
            { Id: "GalileoTkt", Value: 3, Name: "GalileoTkt", TextIconHtml: "", },
            { Id: "BspLink", Value: 4, Name: "BspLink", TextIconHtml: "", },
            { Id: "Manual", Value: 5, Name: "Manual", TextIconHtml: "", },
            { Id: "SirenaXml", Value: 6, Name: "SirenaXml", TextIconHtml: "", },
            { Id: "GalileoXml", Value: 7, Name: "GalileoXml", TextIconHtml: "", },
            { Id: "AmadeusXml", Value: 8, Name: "AmadeusXml", TextIconHtml: "", },
        ];
        ProductOrigin._maxLength = 9;
        ProductOrigin._items = {
            "0": ProductOrigin._array[0],
            "AmadeusAir": ProductOrigin._array[0],
            "1": ProductOrigin._array[1],
            "AmadeusPrint": ProductOrigin._array[1],
            "2": ProductOrigin._array[2],
            "GalileoMir": ProductOrigin._array[2],
            "3": ProductOrigin._array[3],
            "GalileoTkt": ProductOrigin._array[3],
            "4": ProductOrigin._array[4],
            "BspLink": ProductOrigin._array[4],
            "5": ProductOrigin._array[5],
            "Manual": ProductOrigin._array[5],
            "6": ProductOrigin._array[6],
            "SirenaXml": ProductOrigin._array[6],
            "7": ProductOrigin._array[7],
            "GalileoXml": ProductOrigin._array[7],
            "8": ProductOrigin._array[8],
            "AmadeusXml": ProductOrigin._array[8],
        };
    })(ProductOrigin = Luxena.ProductOrigin || (Luxena.ProductOrigin = {}));
    //#endregion
    //#region ProductType 
    (function (ProductType) {
        ProductType[ProductType["AviaTicket"] = 0] = "AviaTicket";
        ProductType[ProductType["AviaRefund"] = 1] = "AviaRefund";
        ProductType[ProductType["AviaMco"] = 2] = "AviaMco";
        ProductType[ProductType["Pasteboard"] = 3] = "Pasteboard";
        ProductType[ProductType["SimCard"] = 4] = "SimCard";
        ProductType[ProductType["Isic"] = 5] = "Isic";
        ProductType[ProductType["Excursion"] = 6] = "Excursion";
        ProductType[ProductType["Tour"] = 7] = "Tour";
        ProductType[ProductType["Accommodation"] = 8] = "Accommodation";
        ProductType[ProductType["Transfer"] = 9] = "Transfer";
        ProductType[ProductType["Insurance"] = 10] = "Insurance";
        ProductType[ProductType["CarRental"] = 11] = "CarRental";
        ProductType[ProductType["GenericProduct"] = 12] = "GenericProduct";
        ProductType[ProductType["BusTicket"] = 13] = "BusTicket";
        ProductType[ProductType["PasteboardRefund"] = 14] = "PasteboardRefund";
        ProductType[ProductType["InsuranceRefund"] = 15] = "InsuranceRefund";
        ProductType[ProductType["BusTicketRefund"] = 16] = "BusTicketRefund";
    })(Luxena.ProductType || (Luxena.ProductType = {}));
    var ProductType = Luxena.ProductType;
    var ProductType;
    (function (ProductType) {
        ProductType._ns = "Luxena.Travel.Domain";
        ProductType._name = "ProductType";
        ProductType._fullName = "Luxena.Travel.Domain.ProductType";
        ProductType._getEdm = function (value) { return !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.ProductType'" + value + "'"); };
        ProductType._array = [
            { Id: "AviaTicket", Value: 0, Name: "Авиабилет", Icon: "plane", TextIconHtml: Luxena.getTextIconHtml("plane"), ru: "Авиабилет", rus: "Авиабилеты", ua: "Авіаквиток" },
            { Id: "AviaRefund", Value: 1, Name: "Возврат авиабилета", Icon: "plane", TextIconHtml: Luxena.getTextIconHtml("plane"), ru: "Возврат авиабилета", rus: "Возвраты авиабилетов", ua: "Повернення авіаквитка" },
            { Id: "AviaMco", Value: 2, Name: "МСО", Icon: "plane", TextIconHtml: Luxena.getTextIconHtml("plane"), ru: "МСО", rus: "МСО", ua: "MCO" },
            { Id: "Pasteboard", Value: 3, Name: "Ж/д билет", Icon: "subway", TextIconHtml: Luxena.getTextIconHtml("subway"), ru: "Ж/д билет", rus: "Ж/д билеты", ua: "Залізничний квиток" },
            { Id: "SimCard", Value: 4, Name: "SIM-карта", Icon: "mobile", TextIconHtml: Luxena.getTextIconHtml("mobile"), ru: "SIM-карта", rus: "SIM-карты", ua: "SIM-картка" },
            { Id: "Isic", Value: 5, Name: "Студенческий билет", Icon: "graduation-cap", TextIconHtml: Luxena.getTextIconHtml("graduation-cap"), ru: "Студенческий билет", rus: "Студенческие билеты", ua: "Студентський квиток" },
            { Id: "Excursion", Value: 6, Name: "Экскурсия", Icon: "photo", TextIconHtml: Luxena.getTextIconHtml("photo"), ru: "Экскурсия", rus: "Экскурсии", ua: "Екскурсія" },
            { Id: "Tour", Value: 7, Name: "Турпакет", Icon: "suitcase", TextIconHtml: Luxena.getTextIconHtml("suitcase"), ru: "Турпакет", rus: "Турпакеты", ua: "Турпакет" },
            { Id: "Accommodation", Value: 8, Name: "Проживание", Icon: "bed", TextIconHtml: Luxena.getTextIconHtml("bed"), ru: "Проживание", rus: "Проживания", ua: "Готель" },
            { Id: "Transfer", Value: 9, Name: "Трансфер", Icon: "cab", TextIconHtml: Luxena.getTextIconHtml("cab"), ru: "Трансфер", rus: "Трансферы", ua: "Трансфер" },
            { Id: "Insurance", Value: 10, Name: "Страховка", Icon: "fire-extinguisher", TextIconHtml: Luxena.getTextIconHtml("fire-extinguisher"), ru: "Страховка", rus: "Страховки", ua: "Страховка" },
            { Id: "CarRental", Value: 11, Name: "Аренда автомобиля", Icon: "car", TextIconHtml: Luxena.getTextIconHtml("car"), ru: "Аренда автомобиля", rus: "Аренды автомобилей", ua: "Оренда автомобіля" },
            { Id: "GenericProduct", Value: 12, Name: "Дополнительная услуга", Icon: "suitcase", TextIconHtml: Luxena.getTextIconHtml("suitcase"), ru: "Дополнительная услуга", rus: "Дополнительные услуги", ua: "Додаткова послуга" },
            { Id: "BusTicket", Value: 13, Name: "Автобусный билет", Icon: "bus", TextIconHtml: Luxena.getTextIconHtml("bus"), ru: "Автобусный билет", rus: "Автобусные билеты", ua: "Автобусний квиток" },
            { Id: "PasteboardRefund", Value: 14, Name: "Возврат ж/д билета", Icon: "subway", TextIconHtml: Luxena.getTextIconHtml("subway"), ru: "Возврат ж/д билета", rus: "Возвраты ж/д билетов", ua: "Повернення залізничного квитка" },
            { Id: "InsuranceRefund", Value: 15, Name: "Возврат страховки", Icon: "fire-extinguisher", TextIconHtml: Luxena.getTextIconHtml("fire-extinguisher"), ru: "Возврат страховки", rus: "Возвраты страховок", ua: "Повернення страховки" },
            { Id: "BusTicketRefund", Value: 16, Name: "Возврат автобусного билета", Icon: "bus", TextIconHtml: Luxena.getTextIconHtml("bus"), ru: "Возврат автобусного билета", rus: "Возвраты автобусных билетов", ua: "Повернення автобусного квитка" },
        ];
        ProductType._maxLength = 20;
        ProductType._items = {
            "0": ProductType._array[0],
            "AviaTicket": ProductType._array[0],
            "1": ProductType._array[1],
            "AviaRefund": ProductType._array[1],
            "2": ProductType._array[2],
            "AviaMco": ProductType._array[2],
            "3": ProductType._array[3],
            "Pasteboard": ProductType._array[3],
            "4": ProductType._array[4],
            "SimCard": ProductType._array[4],
            "5": ProductType._array[5],
            "Isic": ProductType._array[5],
            "6": ProductType._array[6],
            "Excursion": ProductType._array[6],
            "7": ProductType._array[7],
            "Tour": ProductType._array[7],
            "8": ProductType._array[8],
            "Accommodation": ProductType._array[8],
            "9": ProductType._array[9],
            "Transfer": ProductType._array[9],
            "10": ProductType._array[10],
            "Insurance": ProductType._array[10],
            "11": ProductType._array[11],
            "CarRental": ProductType._array[11],
            "12": ProductType._array[12],
            "GenericProduct": ProductType._array[12],
            "13": ProductType._array[13],
            "BusTicket": ProductType._array[13],
            "14": ProductType._array[14],
            "PasteboardRefund": ProductType._array[14],
            "15": ProductType._array[15],
            "InsuranceRefund": ProductType._array[15],
            "16": ProductType._array[16],
            "BusTicketRefund": ProductType._array[16],
        };
    })(ProductType = Luxena.ProductType || (Luxena.ProductType = {}));
    //#endregion
    //#region ServiceClass 
    (function (ServiceClass) {
        ServiceClass[ServiceClass["Unknown"] = 0] = "Unknown";
        ServiceClass[ServiceClass["Economy"] = 1] = "Economy";
        ServiceClass[ServiceClass["PremiumEconomy"] = 2] = "PremiumEconomy";
        ServiceClass[ServiceClass["Business"] = 3] = "Business";
        ServiceClass[ServiceClass["First"] = 4] = "First";
    })(Luxena.ServiceClass || (Luxena.ServiceClass = {}));
    var ServiceClass = Luxena.ServiceClass;
    var ServiceClass;
    (function (ServiceClass) {
        ServiceClass._ns = "Luxena.Travel.Domain";
        ServiceClass._name = "ServiceClass";
        ServiceClass._fullName = "Luxena.Travel.Domain.ServiceClass";
        ServiceClass._getEdm = function (value) { return !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.ServiceClass'" + value + "'"); };
        ServiceClass._array = [
            { Id: "Unknown", Value: 0, Name: "Неизвестно", TextIconHtml: "", ru: "Неизвестно" },
            { Id: "Economy", Value: 1, Name: "Эконом", TextIconHtml: "", ru: "Эконом" },
            { Id: "PremiumEconomy", Value: 2, Name: "Эконом-премиум", TextIconHtml: "", ru: "Эконом-премиум" },
            { Id: "Business", Value: 3, Name: "Бизнес", TextIconHtml: "", ru: "Бизнес" },
            { Id: "First", Value: 4, Name: "Первый", TextIconHtml: "", ru: "Первый" },
        ];
        ServiceClass._maxLength = 10;
        ServiceClass._items = {
            "0": ServiceClass._array[0],
            "Unknown": ServiceClass._array[0],
            "1": ServiceClass._array[1],
            "Economy": ServiceClass._array[1],
            "2": ServiceClass._array[2],
            "PremiumEconomy": ServiceClass._array[2],
            "3": ServiceClass._array[3],
            "Business": ServiceClass._array[3],
            "4": ServiceClass._array[4],
            "First": ServiceClass._array[4],
        };
    })(ServiceClass = Luxena.ServiceClass || (Luxena.ServiceClass = {}));
    //#endregion
    //#region UserRole 
    (function (UserRole) {
        UserRole[UserRole["None"] = 0] = "None";
        UserRole[UserRole["Everyone"] = 1] = "Everyone";
        UserRole[UserRole["Administrator"] = 2] = "Administrator";
        UserRole[UserRole["Supervisor"] = 4] = "Supervisor";
        UserRole[UserRole["Agent"] = 8] = "Agent";
        UserRole[UserRole["Cashier"] = 16] = "Cashier";
        UserRole[UserRole["Analyst"] = 32] = "Analyst";
        UserRole[UserRole["SubAgent"] = 64] = "SubAgent";
    })(Luxena.UserRole || (Luxena.UserRole = {}));
    var UserRole = Luxena.UserRole;
    var UserRole;
    (function (UserRole) {
        UserRole._ns = "Luxena.Travel.Domain";
        UserRole._name = "UserRole";
        UserRole._fullName = "Luxena.Travel.Domain.UserRole";
        UserRole._getEdm = function (value) { return !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.UserRole'" + value + "'"); };
        UserRole._isFlags = true;
        UserRole._array = [
            { Id: "None", Value: 0, Name: "None", TextIconHtml: "", },
            { Id: "Everyone", Value: 1, Name: "Все", TextIconHtml: "", ru: "Все" },
            { Id: "Administrator", Value: 2, Name: "Администратор", TextIconHtml: "", ru: "Администратор" },
            { Id: "Supervisor", Value: 4, Name: "Супервизор", TextIconHtml: "", ru: "Супервизор" },
            { Id: "Agent", Value: 8, Name: "Агент", TextIconHtml: "", ru: "Агент" },
            { Id: "Cashier", Value: 16, Name: "Кассир", TextIconHtml: "", ru: "Кассир" },
            { Id: "Analyst", Value: 32, Name: "Аналитик", TextIconHtml: "", ru: "Аналитик" },
            { Id: "SubAgent", Value: 64, Name: "Субагент", TextIconHtml: "", ru: "Субагент" },
        ];
        UserRole._maxLength = 10;
        UserRole._items = {
            "0": UserRole._array[0],
            "None": UserRole._array[0],
            "1": UserRole._array[1],
            "Everyone": UserRole._array[1],
            "2": UserRole._array[2],
            "Administrator": UserRole._array[2],
            "4": UserRole._array[3],
            "Supervisor": UserRole._array[3],
            "8": UserRole._array[4],
            "Agent": UserRole._array[4],
            "16": UserRole._array[5],
            "Cashier": UserRole._array[5],
            "32": UserRole._array[6],
            "Analyst": UserRole._array[6],
            "64": UserRole._array[7],
            "SubAgent": UserRole._array[7],
        };
    })(UserRole = Luxena.UserRole || (Luxena.UserRole = {}));
    //#endregion
    //#endregion
    var Domain = (function (_super) {
        __extends(Domain, _super);
        function Domain() {
            _super.apply(this, arguments);
        }
        return Domain;
    })(DevExpress.data.ODataContext);
    Luxena.Domain = Domain;
    ;
    Luxena.config.services.db.entities =
        {
            Accommodations: { key: "Id", keyType: "String" },
            AccommodationLookup: { key: "Id", keyType: "String" },
            AccommodationProviders: { key: "Id", keyType: "String" },
            AccommodationProviderLookup: { key: "Id", keyType: "String" },
            AccommodationTypes: { key: "Id", keyType: "String" },
            AccommodationTypeLookup: { key: "Id", keyType: "String" },
            ActiveOwners: { key: "Id", keyType: "String" },
            ActiveOwnerLookup: { key: "Id", keyType: "String" },
            Agents: { key: "Id", keyType: "String" },
            AgentLookup: { key: "Id", keyType: "String" },
            Airlines: { key: "Id", keyType: "String" },
            AirlineLookup: { key: "Id", keyType: "String" },
            AirlineServiceClasses: { key: "Id", keyType: "String" },
            AirlineServiceClassLookup: { key: "Id", keyType: "String" },
            Airports: { key: "Id", keyType: "String" },
            AirportLookup: { key: "Id", keyType: "String" },
            AviaDocuments: { key: "Id", keyType: "String" },
            AviaDocumentLookup: { key: "Id", keyType: "String" },
            AviaMcos: { key: "Id", keyType: "String" },
            AviaMcoLookup: { key: "Id", keyType: "String" },
            AviaRefunds: { key: "Id", keyType: "String" },
            AviaRefundLookup: { key: "Id", keyType: "String" },
            AviaTickets: { key: "Id", keyType: "String" },
            AviaTicketLookup: { key: "Id", keyType: "String" },
            BankAccounts: { key: "Id", keyType: "String" },
            BankAccountLookup: { key: "Id", keyType: "String" },
            BusDocuments: { key: "Id", keyType: "String" },
            BusDocumentLookup: { key: "Id", keyType: "String" },
            BusTickets: { key: "Id", keyType: "String" },
            BusTicketLookup: { key: "Id", keyType: "String" },
            BusTicketProviders: { key: "Id", keyType: "String" },
            BusTicketProviderLookup: { key: "Id", keyType: "String" },
            BusTicketRefunds: { key: "Id", keyType: "String" },
            BusTicketRefundLookup: { key: "Id", keyType: "String" },
            CarRentals: { key: "Id", keyType: "String" },
            CarRentalLookup: { key: "Id", keyType: "String" },
            CarRentalProviders: { key: "Id", keyType: "String" },
            CarRentalProviderLookup: { key: "Id", keyType: "String" },
            CashInOrderPayments: { key: "Id", keyType: "String" },
            CashInOrderPaymentLookup: { key: "Id", keyType: "String" },
            CashOutOrderPayments: { key: "Id", keyType: "String" },
            CashOutOrderPaymentLookup: { key: "Id", keyType: "String" },
            CateringTypes: { key: "Id", keyType: "String" },
            CateringTypeLookup: { key: "Id", keyType: "String" },
            CheckPayments: { key: "Id", keyType: "String" },
            CheckPaymentLookup: { key: "Id", keyType: "String" },
            Consignments: { key: "Id", keyType: "String" },
            ConsignmentLookup: { key: "Id", keyType: "String" },
            Countries: { key: "Id", keyType: "String" },
            CountryLookup: { key: "Id", keyType: "String" },
            CurrencyDailyRates: { key: "Id", keyType: "String" },
            Customers: { key: "Id", keyType: "String" },
            CustomerLookup: { key: "Id", keyType: "String" },
            Departments: { key: "Id", keyType: "String" },
            DepartmentLookup: { key: "Id", keyType: "String" },
            DocumentAccesses: { key: "Id", keyType: "String" },
            DocumentOwners: { key: "Id", keyType: "String" },
            ElectronicPayments: { key: "Id", keyType: "String" },
            ElectronicPaymentLookup: { key: "Id", keyType: "String" },
            EverydayProfitReports: { key: "Id", keyType: "String" },
            Excursions: { key: "Id", keyType: "String" },
            ExcursionLookup: { key: "Id", keyType: "String" },
            Files: { key: "Id", keyType: "String" },
            FlightSegments: { key: "Id", keyType: "String" },
            FlightSegmentLookup: { key: "Id", keyType: "String" },
            FlownReports: { key: "Id", keyType: "String" },
            GdsAgents: { key: "Id", keyType: "String" },
            GdsAgentLookup: { key: "Id", keyType: "String" },
            GdsAgent_ApplyToUnassigned: { key: "Id", keyType: "String" },
            GdsFiles: { key: "Id", keyType: "String" },
            GdsFileLookup: { key: "Id", keyType: "String" },
            GenericProducts: { key: "Id", keyType: "String" },
            GenericProductLookup: { key: "Id", keyType: "String" },
            GenericProductProviders: { key: "Id", keyType: "String" },
            GenericProductProviderLookup: { key: "Id", keyType: "String" },
            GenericProductTypes: { key: "Id", keyType: "String" },
            GenericProductTypeLookup: { key: "Id", keyType: "String" },
            Identities: { key: "Id", keyType: "String" },
            IdentityLookup: { key: "Id", keyType: "String" },
            Insurances: { key: "Id", keyType: "String" },
            InsuranceLookup: { key: "Id", keyType: "String" },
            InsuranceCompanies: { key: "Id", keyType: "String" },
            InsuranceCompanyLookup: { key: "Id", keyType: "String" },
            InsuranceDocuments: { key: "Id", keyType: "String" },
            InsuranceDocumentLookup: { key: "Id", keyType: "String" },
            InsuranceRefunds: { key: "Id", keyType: "String" },
            InsuranceRefundLookup: { key: "Id", keyType: "String" },
            InternalIdentities: { key: "Id", keyType: "String" },
            InternalIdentityLookup: { key: "Id", keyType: "String" },
            InternalTransfers: { key: "Id", keyType: "String" },
            InternalTransferLookup: { key: "Id", keyType: "String" },
            Invoices: { key: "Id", keyType: "String" },
            InvoiceLookup: { key: "Id", keyType: "String" },
            Isics: { key: "Id", keyType: "String" },
            IsicLookup: { key: "Id", keyType: "String" },
            IssuedConsignments: { key: "Id", keyType: "String" },
            IssuedConsignmentLookup: { key: "Id", keyType: "String" },
            MilesCards: { key: "Id", keyType: "String" },
            MilesCardLookup: { key: "Id", keyType: "String" },
            Orders: { key: "Id", keyType: "String" },
            OrderLookup: { key: "Id", keyType: "String" },
            OrderChecks: { key: "Id", keyType: "String" },
            OrderCheckLookup: { key: "Id", keyType: "String" },
            OrderItems: { key: "Id", keyType: "String" },
            OrderItemLookup: { key: "Id", keyType: "String" },
            Organizations: { key: "Id", keyType: "String" },
            OrganizationLookup: { key: "Id", keyType: "String" },
            Parties: { key: "Id", keyType: "String" },
            PartyLookup: { key: "Id", keyType: "String" },
            Passports: { key: "Id", keyType: "String" },
            PassportLookup: { key: "Id", keyType: "String" },
            Pasteboards: { key: "Id", keyType: "String" },
            PasteboardLookup: { key: "Id", keyType: "String" },
            PasteboardProviders: { key: "Id", keyType: "String" },
            PasteboardProviderLookup: { key: "Id", keyType: "String" },
            PasteboardRefunds: { key: "Id", keyType: "String" },
            PasteboardRefundLookup: { key: "Id", keyType: "String" },
            Payments: { key: "Id", keyType: "String" },
            PaymentLookup: { key: "Id", keyType: "String" },
            PaymentSystems: { key: "Id", keyType: "String" },
            PaymentSystemLookup: { key: "Id", keyType: "String" },
            Persons: { key: "Id", keyType: "String" },
            PersonLookup: { key: "Id", keyType: "String" },
            Products: { key: "Id", keyType: "String" },
            ProductLookup: { key: "Id", keyType: "String" },
            ProductPassengers: { key: "Id", keyType: "String" },
            ProductSummaries: { key: "Id", keyType: "String" },
            ProductTotalByBookers: { key: "Id", keyType: "String" },
            ProductTotalByDays: { key: "Id", keyType: "String" },
            ProductTotalByMonths: { key: "Id", keyType: "String" },
            ProductTotalByOwners: { key: "Id", keyType: "String" },
            ProductTotalByProviders: { key: "Id", keyType: "String" },
            ProductTotalByQuarters: { key: "Id", keyType: "String" },
            ProductTotalBySellers: { key: "Id", keyType: "String" },
            ProductTotalByTypes: { key: "Id", keyType: "String" },
            ProductTotalByYears: { key: "Id", keyType: "String" },
            ProfitDistributionByCustomers: { key: "Id", keyType: "String" },
            ProfitDistributionByProviders: { key: "Id", keyType: "String" },
            RailwayDocuments: { key: "Id", keyType: "String" },
            RailwayDocumentLookup: { key: "Id", keyType: "String" },
            Receipts: { key: "Id", keyType: "String" },
            ReceiptLookup: { key: "Id", keyType: "String" },
            RoamingOperators: { key: "Id", keyType: "String" },
            RoamingOperatorLookup: { key: "Id", keyType: "String" },
            Sequences: { key: "Id", keyType: "String" },
            SequenceLookup: { key: "Id", keyType: "String" },
            SimCards: { key: "Id", keyType: "String" },
            SimCardLookup: { key: "Id", keyType: "String" },
            SystemConfigurations: { key: "Id", keyType: "String" },
            Tours: { key: "Id", keyType: "String" },
            TourLookup: { key: "Id", keyType: "String" },
            TourProviders: { key: "Id", keyType: "String" },
            TourProviderLookup: { key: "Id", keyType: "String" },
            Transfers: { key: "Id", keyType: "String" },
            TransferLookup: { key: "Id", keyType: "String" },
            TransferProviders: { key: "Id", keyType: "String" },
            TransferProviderLookup: { key: "Id", keyType: "String" },
            Users: { key: "Id", keyType: "String" },
            UserLookup: { key: "Id", keyType: "String" },
            WireTransfers: { key: "Id", keyType: "String" },
            WireTransferLookup: { key: "Id", keyType: "String" },
        };
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var endpointSelector = new DevExpress.EndpointSelector(Luxena.config.endpoints);
    var serviceConfig = $.extend(true, {}, Luxena.config.services, {
        db: {
            url: endpointSelector.urlFor("db"),
            //jsonp: true,
            version: 4,
            errorHandler: showError,
            beforeSend: function (request) {
                //if (request.method === "MERGE")
                //{
                //	request.headers['X-HTTP-Method'] = request.method;
                //	request.method = "PATCH";
                //}
                var prms = request.params;
                if (prms && prms.$select) {
                    if (prms.$select.indexOf(",$usecalculated") >= 0) {
                        prms.$select = prms.$select.replace(",$usecalculated", "");
                        prms.usecalculated = true;
                    }
                    if (prms.$select.indexOf(",$recalc") >= 0) {
                        prms.$select = prms.$select.replace(",$recalc", "");
                        prms.recalc = true;
                    }
                }
            }
        }
    });
    Luxena.db = new Luxena.Domain(serviceConfig.db);
    function showError(error) {
        var msg = "";
        if (typeof error === "string") {
            console.error("ERROR: " + error);
        }
        else {
            var err = error["errorDetails"] || error;
            if (err && err.message) {
                var priorMsg = "";
                while (err) {
                    if (err.message !== priorMsg)
                        msg = (msg ? "\r\n" : "") + "<li>" + err.message + "</li>" + msg;
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
            contentTemplate: "<ul style=\"display: table-cell; padding-left: 25px\">" + msg + "</ul>",
        });
    }
    Luxena.showError = showError;
})(Luxena || (Luxena = {}));
//00:00:00.0746334
var Luxena;
(function (Luxena) {
    var EntitySemantic = (function (_super) {
        __extends(EntitySemantic, _super);
        function EntitySemantic() {
            _super.apply(this, arguments);
            this.Id = this.member().string().utility();
            this.Version = this.member().int().utility();
        }
        return EntitySemantic;
    })(Luxena.SemanticEntity);
    Luxena.EntitySemantic = EntitySemantic;
    var DomainActionSemantic = (function (_super) {
        __extends(DomainActionSemantic, _super);
        function DomainActionSemantic() {
            _super.apply(this, arguments);
            this.Id = this.member().string().utility();
        }
        return DomainActionSemantic;
    })(Luxena.SemanticEntity);
    Luxena.DomainActionSemantic = DomainActionSemantic;
    //00:00:00.0782343
    var Entity2Semantic = (function (_super) {
        __extends(Entity2Semantic, _super);
        function Entity2Semantic() {
            _super.apply(this, arguments);
            /** Дата создания */
            this.CreatedOn = this.member()
                .localizeTitle({ ru: "Дата создания" })
                .dateTime2()
                .required()
                .utility();
            /** Создано пользователем */
            this.CreatedBy = this.member()
                .localizeTitle({ ru: "Создано пользователем" })
                .string()
                .utility();
            /** Дата изменения */
            this.ModifiedOn = this.member()
                .localizeTitle({ ru: "Дата изменения" })
                .dateTime2()
                .utility();
            /** Изменено пользователем */
            this.ModifiedBy = this.member()
                .localizeTitle({ ru: "Изменено пользователем" })
                .string()
                .utility();
        }
        Entity2Semantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return Entity2Semantic;
    })(EntitySemantic);
    Luxena.Entity2Semantic = Entity2Semantic;
    //00:00:00.0855055
    var Entity3Semantic = (function (_super) {
        __extends(Entity3Semantic, _super);
        function Entity3Semantic() {
            _super.apply(this, arguments);
            /** Название */
            this.Name = this.member()
                .localizeTitle({ ru: "Название" })
                .string()
                .entityName();
        }
        Entity3Semantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return Entity3Semantic;
    })(Entity2Semantic);
    Luxena.Entity3Semantic = Entity3Semantic;
    //00:00:00.0863927
    var Entity3DSemantic = (function (_super) {
        __extends(Entity3DSemantic, _super);
        function Entity3DSemantic() {
            _super.apply(this, arguments);
            /** Описание */
            this.Description = this.member()
                .localizeTitle({ ru: "Описание" })
                .string();
        }
        Entity3DSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return Entity3DSemantic;
    })(Entity3Semantic);
    Luxena.Entity3DSemantic = Entity3DSemantic;
    //00:00:00.0869704
    /** Владелец документов */
    var DocumentOwnerSemantic = (function (_super) {
        __extends(DocumentOwnerSemantic, _super);
        function DocumentOwnerSemantic() {
            _super.call(this);
            //00:00:00.0873091
            this._DocumentOwner = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Владелец документов", rus: "Владельцы документов" })
                .lookup(function () { return Luxena.sd.DocumentOwner; });
            /** Действующий */
            this.IsActive = this.member()
                .localizeTitle({ ru: "Действующий" })
                .bool()
                .required();
            /** Владелец */
            this.Owner = this.member()
                .localizeTitle({ ru: "Владелец" })
                .lookup(function () { return Luxena.sd.Party; })
                .entityName();
            this._isAbstract = false;
            this._name = "DocumentOwner";
            this._names = "DocumentOwners";
            this._isEntity = true;
            this._localizeTitle({ ru: "Владелец документов", rus: "Владельцы документов" });
            this._getDerivedEntities = null;
            this._className = "DocumentOwner";
            this._getRootEntity = function () { return Luxena.sd.DocumentOwner; };
            this._store = Luxena.db.DocumentOwners;
            this._saveStore = Luxena.db.DocumentOwners;
            this._referenceFields = { id: "Id", name: "" };
            this.small();
        }
        DocumentOwnerSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return DocumentOwnerSemantic;
    })(EntitySemantic);
    Luxena.DocumentOwnerSemantic = DocumentOwnerSemantic;
    //00:00:00.0882624
    var FileSemantic = (function (_super) {
        __extends(FileSemantic, _super);
        function FileSemantic() {
            _super.call(this);
            //00:00:00.0883418
            this._File = new Luxena.SemanticMember()
                .lookup(function () { return Luxena.sd.File; });
            this.FileName = this.member()
                .string();
            this.TimeStamp = this.member()
                .date()
                .required();
            this.Content = this.member();
            /** Контрагент */
            this.Party = this.member()
                .localizeTitle({ ru: "Контрагент", rus: "Контрагенты" })
                .lookup(function () { return Luxena.sd.Party; })
                .required();
            this.UploadedBy = this.member()
                .lookup(function () { return Luxena.sd.Person; });
            this._isAbstract = false;
            this._name = "File";
            this._names = "Files";
            this._isEntity = true;
            this._getDerivedEntities = null;
            this._className = "File";
            this._getRootEntity = function () { return Luxena.sd.File; };
            this._store = Luxena.db.Files;
            this._saveStore = Luxena.db.Files;
            this._referenceFields = { id: "Id", name: "" };
        }
        FileSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return FileSemantic;
    })(EntitySemantic);
    Luxena.FileSemantic = FileSemantic;
    //00:00:00.0929186
    /** Инвойс */
    var InvoiceSemantic = (function (_super) {
        __extends(InvoiceSemantic, _super);
        function InvoiceSemantic() {
            _super.call(this);
            //00:00:00.0934508
            this._Invoice = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Инвойс", rus: "Счета/квитанции" })
                .lookup(function () { return Luxena.sd.Invoice; });
            /** Тип */
            this.Type = this.member()
                .localizeTitle({ ru: "Тип" })
                .enum(Luxena.InvoiceType)
                .required()
                .entityType();
            /** Дата выпуска */
            this.IssueDate = this.member()
                .localizeTitle({ ru: "Дата выпуска" })
                .date()
                .required()
                .entityDate();
            /** Номер */
            this.Number = this.member()
                .localizeTitle({ ru: "Номер" })
                .string()
                .entityName();
            /** Договор */
            this.Agreement = this.member()
                .localizeTitle({ ru: "Договор" })
                .string();
            /** Дата создания */
            this.TimeStamp = this.member()
                .localizeTitle({ ru: "Дата создания" })
                .dateTime2()
                .required()
                .utility();
            this.Content = this.member();
            /** Итого */
            this.Total = this.member()
                .localizeTitle({ ru: "Итого" })
                .money();
            /** В т.ч. НДС */
            this.Vat = this.member()
                .localizeTitle({ ru: "В т.ч. НДС" })
                .money();
            /** Заказ */
            this.Order = this.member()
                .localizeTitle({ ru: "Заказ", rus: "Заказы" })
                .lookup(function () { return Luxena.sd.Order; });
            /** Выпустил */
            this.IssuedBy = this.member()
                .localizeTitle({ ru: "Выпустил", rus: "Агенты" })
                .lookup(function () { return Luxena.sd.Person; });
            this.Payments = this.collection(function () { return Luxena.sd.Payment; }, function (se) { return se.Invoice; });
            this._isAbstract = false;
            this._name = "Invoice";
            this._names = "Invoices";
            this._isEntity = true;
            this._localizeTitle({ ru: "Инвойс", rus: "Счета/квитанции" });
            this._getDerivedEntities = function () { return [
                Luxena.sd.Receipt
            ]; };
            this._className = "Invoice";
            this._getRootEntity = function () { return Luxena.sd.Invoice; };
            this._store = Luxena.db.Invoices;
            this._saveStore = Luxena.db.Invoices;
            this._lookupStore = Luxena.db.InvoiceLookup;
            this._referenceFields = { id: "Id", name: "Number" };
        }
        InvoiceSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return InvoiceSemantic;
    })(EntitySemantic);
    Luxena.InvoiceSemantic = InvoiceSemantic;
    //00:00:00.0953640
    /** Выпущенная накладная */
    var IssuedConsignmentSemantic = (function (_super) {
        __extends(IssuedConsignmentSemantic, _super);
        function IssuedConsignmentSemantic() {
            _super.call(this);
            //00:00:00.0954983
            this._IssuedConsignment = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Выпущенная накладная", rus: "Выпущенные накладные" })
                .lookup(function () { return Luxena.sd.IssuedConsignment; });
            /** Название */
            this.Number = this.member()
                .localizeTitle({ ru: "Название" })
                .string()
                .entityName();
            this.TimeStamp = this.member()
                .dateTime2()
                .required()
                .entityDate();
            this.Content = this.member();
            /** Накладная */
            this.Consignment = this.member()
                .localizeTitle({ ru: "Накладная", rus: "Накладные" })
                .lookup(function () { return Luxena.sd.Consignment; });
            /** Выпустил */
            this.IssuedBy = this.member()
                .localizeTitle({ ru: "Выпустил", rus: "Агенты" })
                .lookup(function () { return Luxena.sd.Person; });
            this._isAbstract = false;
            this._name = "IssuedConsignment";
            this._names = "IssuedConsignments";
            this._isEntity = true;
            this._localizeTitle({ ru: "Выпущенная накладная", rus: "Выпущенные накладные" });
            this._getDerivedEntities = null;
            this._className = "IssuedConsignment";
            this._getRootEntity = function () { return Luxena.sd.IssuedConsignment; };
            this._store = Luxena.db.IssuedConsignments;
            this._saveStore = Luxena.db.IssuedConsignments;
            this._lookupStore = Luxena.db.IssuedConsignmentLookup;
            this._referenceFields = { id: "Id", name: "Number" };
        }
        IssuedConsignmentSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return IssuedConsignmentSemantic;
    })(EntitySemantic);
    Luxena.IssuedConsignmentSemantic = IssuedConsignmentSemantic;
    //00:00:00.0963254
    var SequenceSemantic = (function (_super) {
        __extends(SequenceSemantic, _super);
        function SequenceSemantic() {
            _super.call(this);
            //00:00:00.0964039
            this._Sequence = new Luxena.SemanticMember()
                .lookup(function () { return Luxena.sd.Sequence; });
            this.Name = this.member()
                .string()
                .entityName();
            this.Discriminator = this.member()
                .string();
            this.Current = this.member()
                .int()
                .required();
            this.Format = this.member()
                .string();
            this.Timestamp = this.member()
                .date()
                .required();
            this._isAbstract = false;
            this._name = "Sequence";
            this._names = "Sequences";
            this._isEntity = true;
            this._getDerivedEntities = null;
            this._className = "Sequence";
            this._getRootEntity = function () { return Luxena.sd.Sequence; };
            this._store = Luxena.db.Sequences;
            this._saveStore = Luxena.db.Sequences;
            this._lookupStore = Luxena.db.SequenceLookup;
            this._referenceFields = { id: "Id", name: "Name" };
        }
        SequenceSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return SequenceSemantic;
    })(EntitySemantic);
    Luxena.SequenceSemantic = SequenceSemantic;
    //00:00:00.0991569
    /** Настройки системы */
    var SystemConfigurationSemantic = (function (_super) {
        __extends(SystemConfigurationSemantic, _super);
        function SystemConfigurationSemantic() {
            _super.call(this);
            //00:00:00.0992604
            this._SystemConfiguration = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Настройки системы" })
                .lookup(function () { return Luxena.sd.SystemConfiguration; });
            this.ModifiedOn = this.member()
                .date();
            this.ModifiedBy = this.member()
                .string();
            this.CompanyName = this.member()
                .string()
                .calculated()
                .nonsaved();
            /** Реквизиты организации */
            this.CompanyDetails = this.member()
                .localizeTitle({ ru: "Реквизиты организации" })
                .text(3);
            /** Валюта по умолчанию */
            this.DefaultCurrency = this.member()
                .localizeTitle({ ru: "Валюта по умолчанию" })
                .string();
            /** Использовать только валюту по умолчанию */
            this.UseDefaultCurrencyForInput = this.member()
                .localizeTitle({ ru: "Использовать только валюту по умолчанию" })
                .bool()
                .required();
            /** Ставка НДС,% */
            this.VatRate = this.member()
                .localizeTitle({ ru: "Ставка НДС,%" })
                .float()
                .required();
            /** Использование riz-данных в Amadeus Air */
            this.AmadeusRizUsingMode = this.member()
                .localizeTitle({ ru: "Использование riz-данных в Amadeus Air" })
                .enum(Luxena.AmadeusRizUsingMode)
                .required();
            /** Требование паспортных данных авиакомпаниями */
            this.IsPassengerPassportRequired = this.member()
                .localizeTitle({ ru: "Требование паспортных данных авиакомпаниями" })
                .bool()
                .required();
            /** Формирование номенклатур заказа по авиадокументу */
            this.AviaOrderItemGenerationOption = this.member()
                .localizeTitle({ ru: "Формирование номенклатур заказа по авиадокументу" })
                .enum(Luxena.ProductOrderItemGenerationOption)
                .required();
            /** Разрешить редактирование НДС в заказе */
            this.AllowAgentSetOrderVat = this.member()
                .localizeTitle({ ru: "Разрешить редактирование НДС в заказе" })
                .bool()
                .required();
            /** Использовать НДС авиадокумента в заказе */
            this.UseAviaDocumentVatInOrder = this.member()
                .localizeTitle({ ru: "Использовать НДС авиадокумента в заказе" })
                .bool()
                .required();
            /** Расчет НДС в авиадокументе */
            this.AviaDocumentVatOptions = this.member()
                .localizeTitle({ ru: "Расчет НДС в авиадокументе" })
                .enum(Luxena.AviaDocumentVatOptions)
                .required();
            /** Текст поля "Главный бухгалтер" */
            this.AccountantDisplayString = this.member()
                .localizeTitle({ ru: "Текст поля \"Главный бухгалтер\"" })
                .text(3);
            /** Корреспондентский счет для ПКО */
            this.IncomingCashOrderCorrespondentAccount = this.member()
                .localizeTitle({ ru: "Корреспондентский счет для ПКО" })
                .string();
            /** Разделять доступ к документам */
            this.SeparateDocumentAccess = this.member()
                .localizeTitle({ ru: "Разделять доступ к документам" })
                .bool()
                .required();
            /** Обязательное ЕДРПОУ для организаций */
            this.IsOrganizationCodeRequired = this.member()
                .localizeTitle({ ru: "Обязательное ЕДРПОУ для организаций" })
                .bool()
                .required();
            /** Использовать доп. доход от АК при обработке авиадокументов */
            this.UseAviaHandling = this.member()
                .localizeTitle({ ru: "Использовать доп. доход от АК при обработке авиадокументов" })
                .bool()
                .required();
            /** Дней до отправления */
            this.DaysBeforeDeparture = this.member()
                .localizeTitle({ ru: "Дней до отправления" })
                .int()
                .required();
            /** Заказ обязателен для обработки документов */
            this.IsOrderRequiredForProcessedDocument = this.member()
                .localizeTitle({ ru: "Заказ обязателен для обработки документов" })
                .bool()
                .required();
            /** Показывать статистику на главной страницы с */
            this.MetricsFromDate = this.member()
                .localizeTitle({ ru: "Показывать статистику на главной страницы с" })
                .date();
            /** Бронировки в документах по офису */
            this.ReservationsInOfficeMetrics = this.member()
                .localizeTitle({ ru: "Бронировки в документах по офису" })
                .bool()
                .required();
            /** Описание обязательно для MCO */
            this.McoRequiresDescription = this.member()
                .localizeTitle({ ru: "Описание обязательно для MCO" })
                .bool()
                .required();
            /** Neutral Airline Code */
            this.NeutralAirlineCode = this.member()
                .localizeTitle({ ru: "Neutral Airline Code" })
                .string();
            /** Заказы: НДС только от сервисного сбора */
            this.Order_UseServiceFeeOnlyInVat = this.member()
                .localizeTitle({ ru: "Заказы: НДС только от сервисного сбора" })
                .bool()
                .required();
            /** Инвойсы: новый номер */
            this.Invoice_NumberMode = this.member()
                .localizeTitle({ ru: "Инвойсы: новый номер" })
                .enum(Luxena.InvoiceNumberMode)
                .required();
            /** Инвойсы: важное примечание */
            this.InvoicePrinter_FooterDetails = this.member()
                .localizeTitle({ ru: "Инвойсы: важное примечание" })
                .text(6);
            this.GalileoWebService_LoadedOn = this.member()
                .date();
            /** Организация */
            this.Company = this.member()
                .localizeTitle({ ru: "Организация" })
                .lookup(function () { return Luxena.sd.Organization; });
            /** Страна */
            this.Country = this.member()
                .localizeTitle({ ru: "Страна", rus: "Страны" })
                .lookup(function () { return Luxena.sd.Country; });
            /** Ответственный за поздравление именинников */
            this.BirthdayTaskResponsible = this.member()
                .localizeTitle({ ru: "Ответственный за поздравление именинников" })
                .lookup(function () { return Luxena.sd.Person; });
            this._isAbstract = false;
            this._name = "SystemConfiguration";
            this._names = "SystemConfigurations";
            this._isEntity = true;
            this._localizeTitle({ ru: "Настройки системы" });
            this._getDerivedEntities = null;
            this._className = "SystemConfiguration";
            this._getRootEntity = function () { return Luxena.sd.SystemConfiguration; };
            this._store = Luxena.db.SystemConfigurations;
            this._saveStore = Luxena.db.SystemConfigurations;
            this._referenceFields = { id: "Id", name: "" };
        }
        SystemConfigurationSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return SystemConfigurationSemantic;
    })(EntitySemantic);
    Luxena.SystemConfigurationSemantic = SystemConfigurationSemantic;
    //00:00:00.1023360
    /** Сервис-класс авиакомпании */
    var AirlineServiceClassSemantic = (function (_super) {
        __extends(AirlineServiceClassSemantic, _super);
        function AirlineServiceClassSemantic() {
            _super.call(this);
            //00:00:00.1024444
            this._AirlineServiceClass = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Сервис-класс авиакомпании", rus: "Сервис-классы авиакомпаний" })
                .lookup(function () { return Luxena.sd.AirlineServiceClass; });
            /** Код */
            this.Code = this.member()
                .localizeTitle({ ru: "Код" })
                .string()
                .required()
                .entityName();
            /** Сервис-класс */
            this.ServiceClass = this.member()
                .localizeTitle({ ru: "Сервис-класс" })
                .enum(Luxena.ServiceClass)
                .required();
            /** Авиакомпания */
            this.Airline = this.member()
                .localizeTitle({ ru: "Авиакомпания", rus: "Авиакомпании" })
                .lookup(function () { return Luxena.sd.Organization; })
                .required();
            this._isAbstract = false;
            this._name = "AirlineServiceClass";
            this._names = "AirlineServiceClasses";
            this._isEntity = true;
            this._localizeTitle({ ru: "Сервис-класс авиакомпании", rus: "Сервис-классы авиакомпаний" });
            this._getDerivedEntities = null;
            this._className = "AirlineServiceClass";
            this._getRootEntity = function () { return Luxena.sd.AirlineServiceClass; };
            this._store = Luxena.db.AirlineServiceClasses;
            this._saveStore = Luxena.db.AirlineServiceClasses;
            this._lookupStore = Luxena.db.AirlineServiceClassLookup;
            this._referenceFields = { id: "Id", name: "Code" };
        }
        AirlineServiceClassSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return AirlineServiceClassSemantic;
    })(Entity2Semantic);
    Luxena.AirlineServiceClassSemantic = AirlineServiceClassSemantic;
    //00:00:00.1041158
    /** Накладная */
    var ConsignmentSemantic = (function (_super) {
        __extends(ConsignmentSemantic, _super);
        function ConsignmentSemantic() {
            _super.call(this);
            //00:00:00.1042181
            this._Consignment = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Накладная", rus: "Накладные" })
                .lookup(function () { return Luxena.sd.Consignment; });
            /** Номер */
            this.Number = this.member()
                .localizeTitle({ ru: "Номер" })
                .string()
                .length(12, 0, 0)
                .entityName();
            /** Дата выпуска */
            this.IssueDate = this.member()
                .localizeTitle({ ru: "Дата выпуска" })
                .date()
                .required()
                .entityDate();
            /** Заказ */
            this.Order = this.member()
                .localizeTitle({ ru: "Заказ", rus: "Заказы" })
                .lookup(function () { return Luxena.sd.Order; })
                .calculated()
                .nonsaved();
            /** Сумма с НДС */
            this.GrandTotal = this.member()
                .localizeTitle({ ru: "Сумма с НДС" })
                .money();
            /** В т.ч. НДС */
            this.Vat = this.member()
                .localizeTitle({ ru: "В т.ч. НДС" })
                .money();
            /** Сумма без НДС */
            this.Total = this.member()
                .localizeTitle({ ru: "Сумма без НДС" })
                .money()
                .calculated()
                .nonsaved();
            /** Скидка */
            this.Discount = this.member()
                .localizeTitle({ ru: "Скидка" })
                .money();
            /** Всего отпущено */
            this.TotalSupplied = this.member()
                .localizeTitle({ ru: "Всего отпущено" })
                .string();
            /** Отпущено */
            this.Supplier = this.member()
                .localizeTitle({ ru: "Отпущено" })
                .lookup(function () { return Luxena.sd.Party; });
            /** Получено */
            this.Acquirer = this.member()
                .localizeTitle({ ru: "Получено" })
                .lookup(function () { return Luxena.sd.Party; });
            this.OrderItems = this.collection(function () { return Luxena.sd.OrderItem; }, function (se) { return se.Consignment; });
            this.IssuedConsignments = this.collection(function () { return Luxena.sd.IssuedConsignment; }, function (se) { return se.Consignment; });
            this._isAbstract = false;
            this._name = "Consignment";
            this._names = "Consignments";
            this._isEntity = true;
            this._localizeTitle({ ru: "Накладная", rus: "Накладные" });
            this._getDerivedEntities = null;
            this._className = "Consignment";
            this._getRootEntity = function () { return Luxena.sd.Consignment; };
            this._store = Luxena.db.Consignments;
            this._saveStore = Luxena.db.Consignments;
            this._lookupStore = Luxena.db.ConsignmentLookup;
            this._referenceFields = { id: "Id", name: "Number" };
        }
        ConsignmentSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ConsignmentSemantic;
    })(Entity2Semantic);
    Luxena.ConsignmentSemantic = ConsignmentSemantic;
    //00:00:00.1065135
    /** Курс валюты */
    var CurrencyDailyRateSemantic = (function (_super) {
        __extends(CurrencyDailyRateSemantic, _super);
        function CurrencyDailyRateSemantic() {
            _super.call(this);
            //00:00:00.1066137
            this._CurrencyDailyRate = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Курс валюты", rus: "Курсы валют" })
                .lookup(function () { return Luxena.sd.CurrencyDailyRate; });
            /** Дата */
            this.Date = this.member()
                .localizeTitle({ en: "Date", ru: "Дата" })
                .date()
                .required()
                .entityDate()
                .entityName()
                .unique();
            /** UAH/EUR */
            this.UAH_EUR = this.member()
                .localizeTitle({ ru: "UAH/EUR" })
                .float();
            /** UAH/RUB */
            this.UAH_RUB = this.member()
                .localizeTitle({ ru: "UAH/RUB" })
                .float();
            /** UAH/USD */
            this.UAH_USD = this.member()
                .localizeTitle({ ru: "UAH/USD" })
                .float();
            /** RUB/EUR */
            this.RUB_EUR = this.member()
                .localizeTitle({ ru: "RUB/EUR" })
                .float();
            /** RUB/USD */
            this.RUB_USD = this.member()
                .localizeTitle({ ru: "RUB/USD" })
                .float();
            /** EUR/USD */
            this.EUR_USD = this.member()
                .localizeTitle({ ru: "EUR/USD" })
                .float();
            this._isAbstract = false;
            this._name = "CurrencyDailyRate";
            this._names = "CurrencyDailyRates";
            this._isEntity = true;
            this._localizeTitle({ ru: "Курс валюты", rus: "Курсы валют" });
            this._getDerivedEntities = null;
            this._className = "CurrencyDailyRate";
            this._getRootEntity = function () { return Luxena.sd.CurrencyDailyRate; };
            this._store = Luxena.db.CurrencyDailyRates;
            this._saveStore = Luxena.db.CurrencyDailyRates;
            this._referenceFields = { id: "Id", name: "" };
        }
        CurrencyDailyRateSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return CurrencyDailyRateSemantic;
    })(Entity2Semantic);
    Luxena.CurrencyDailyRateSemantic = CurrencyDailyRateSemantic;
    //00:00:00.1080998
    /** Доступ к документам */
    var DocumentAccessSemantic = (function (_super) {
        __extends(DocumentAccessSemantic, _super);
        function DocumentAccessSemantic() {
            _super.call(this);
            //00:00:00.1081973
            this._DocumentAccess = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Доступ к документам" })
                .lookup(function () { return Luxena.sd.DocumentAccess; });
            /** Полный доступ */
            this.FullDocumentControl = this.member()
                .localizeTitle({ ru: "Полный доступ" })
                .bool()
                .required();
            /** Персона */
            this.Person = this.member()
                .localizeTitle({ ru: "Персона", rus: "Персоны" })
                .lookup(function () { return Luxena.sd.Person; })
                .entityName();
            /** Владелец */
            this.Owner = this.member()
                .localizeTitle({ ru: "Владелец" })
                .lookup(function () { return Luxena.sd.Party; });
            this._isAbstract = false;
            this._name = "DocumentAccess";
            this._names = "DocumentAccesses";
            this._isEntity = true;
            this._localizeTitle({ ru: "Доступ к документам" });
            this._getDerivedEntities = null;
            this._className = "DocumentAccess";
            this._getRootEntity = function () { return Luxena.sd.DocumentAccess; };
            this._store = Luxena.db.DocumentAccesses;
            this._saveStore = Luxena.db.DocumentAccesses;
            this._referenceFields = { id: "Id", name: "" };
        }
        DocumentAccessSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return DocumentAccessSemantic;
    })(Entity2Semantic);
    Luxena.DocumentAccessSemantic = DocumentAccessSemantic;
    //00:00:00.1114405
    /** Полетный сегмент */
    var FlightSegmentSemantic = (function (_super) {
        __extends(FlightSegmentSemantic, _super);
        function FlightSegmentSemantic() {
            _super.call(this);
            //00:00:00.1115392
            this._FlightSegment = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Полетный сегмент", rus: "Полетные сегменты" })
                .lookup(function () { return Luxena.sd.FlightSegment; });
            this.Name = this.member()
                .string()
                .calculated()
                .nonsaved()
                .entityName();
            /** № позиции */
            this.Position = this.member()
                .localizeTitle({ ru: "№ позиции" })
                .int()
                .required()
                .entityPosition();
            this.Type = this.member()
                .enum(Luxena.FlightSegmentType)
                .required();
            /** Из аэропорта (код) */
            this.FromAirportCode = this.member()
                .localizeTitle({ ru: "Из аэропорта (код)", ruShort: "код" })
                .emptyText("код")
                .string();
            /** Из аэропорта (название) */
            this.FromAirportName = this.member()
                .localizeTitle({ ru: "Из аэропорта (название)", ruShort: "название" })
                .emptyText("название")
                .string();
            /** В аэропорт (код) */
            this.ToAirportCode = this.member()
                .localizeTitle({ ru: "В аэропорт (код)", ruShort: "код" })
                .emptyText("код")
                .string();
            /** В аэропорт (название) */
            this.ToAirportName = this.member()
                .localizeTitle({ ru: "В аэропорт (название)", ruShort: "название" })
                .emptyText("название")
                .string();
            this.CarrierIataCode = this.member()
                .string();
            this.CarrierPrefixCode = this.member()
                .string();
            this.CarrierName = this.member()
                .string();
            /** Рейс */
            this.FlightNumber = this.member()
                .localizeTitle({ ru: "Рейс" })
                .string()
                .length(4, 0, 0);
            /** Код сервис-класса */
            this.ServiceClassCode = this.member()
                .localizeTitle({ ru: "Код сервис-класса" })
                .string();
            this.ServiceClass = this.member()
                .localizeTitle({ ru: "Сервис-класс" })
                .enum(Luxena.ServiceClass);
            /** Дата/время отправления */
            this.DepartureTime = this.member()
                .localizeTitle({ ru: "Дата/время отправления" })
                .dateTime();
            /** Дата/время прибытия */
            this.ArrivalTime = this.member()
                .localizeTitle({ ru: "Дата/время прибытия" })
                .dateTime();
            /** Коды питания */
            this.MealCodes = this.member()
                .localizeTitle({ ru: "Коды питания" })
                .string();
            /** Питание */
            this.MealTypes = this.member()
                .localizeTitle({ ru: "Питание" })
                .enum(Luxena.MealType);
            /** Остановки */
            this.NumberOfStops = this.member()
                .localizeTitle({ ru: "Остановки" })
                .int();
            /** Багаж */
            this.Luggage = this.member()
                .localizeTitle({ ru: "Багаж" })
                .string()
                .length(3, 0, 0);
            /** Терминал отправления */
            this.CheckInTerminal = this.member()
                .localizeTitle({ ru: "Терминал отправления", ruShort: "терминал" })
                .emptyText("терминал")
                .string();
            /** Регистрация */
            this.CheckInTime = this.member()
                .localizeTitle({ ru: "Регистрация" })
                .string();
            /** Перелет */
            this.Duration = this.member()
                .localizeTitle({ ru: "Перелет" })
                .string()
                .length(4, 0, 0);
            /** Терминал прибытия */
            this.ArrivalTerminal = this.member()
                .localizeTitle({ ru: "Терминал прибытия", ruShort: "терминал" })
                .emptyText("терминал")
                .string();
            /** Место */
            this.Seat = this.member()
                .localizeTitle({ ru: "Место" })
                .string()
                .length(3, 0, 0);
            /** База тарифа */
            this.FareBasis = this.member()
                .localizeTitle({ ru: "База тарифа" })
                .string()
                .length(10, 0, 0);
            /** Это конечный пункт */
            this.Stopover = this.member()
                .localizeTitle({ ru: "Это конечный пункт" })
                .bool()
                .required();
            this.Surcharges = this.member()
                .float();
            this.IsInclusive = this.member()
                .bool()
                .required();
            this.Fare = this.member()
                .float();
            this.StopoverOrTransferCharge = this.member()
                .float();
            this.IsSideTrip = this.member()
                .bool()
                .required();
            /** Расстояние, км */
            this.Distance = this.member()
                .localizeTitle({ ru: "Расстояние, км" })
                .float()
                .required();
            this.Amount = this.member()
                .money();
            this.CouponAmount = this.member()
                .money();
            /** Авиабилет */
            this.Ticket = this.member()
                .localizeTitle({ ru: "Авиабилет", rus: "Авиабилеты", ua: "Авіаквиток" })
                .lookup(function () { return Luxena.sd.AviaTicket; });
            /** Из аэропорта */
            this.FromAirport = this.member()
                .localizeTitle({ ru: "Из аэропорта" })
                .lookup(function () { return Luxena.sd.Airport; });
            /** В аэропорт */
            this.ToAirport = this.member()
                .localizeTitle({ ru: "В аэропорт" })
                .lookup(function () { return Luxena.sd.Airport; });
            /** Перевозчик */
            this.Carrier = this.member()
                .localizeTitle({ ru: "Перевозчик" })
                .lookup(function () { return Luxena.sd.Organization; });
            this._isAbstract = false;
            this._name = "FlightSegment";
            this._names = "FlightSegments";
            this._isEntity = true;
            this._localizeTitle({ ru: "Полетный сегмент", rus: "Полетные сегменты" });
            this._getDerivedEntities = null;
            this._className = "FlightSegment";
            this._getRootEntity = function () { return Luxena.sd.FlightSegment; };
            this._store = Luxena.db.FlightSegments;
            this._saveStore = Luxena.db.FlightSegments;
            this._lookupStore = Luxena.db.FlightSegmentLookup;
            this._referenceFields = { id: "Id", name: "Name" };
        }
        FlightSegmentSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return FlightSegmentSemantic;
    })(Entity2Semantic);
    Luxena.FlightSegmentSemantic = FlightSegmentSemantic;
    //00:00:00.1154697
    /** Gds-агент */
    var GdsAgentSemantic = (function (_super) {
        __extends(GdsAgentSemantic, _super);
        function GdsAgentSemantic() {
            _super.call(this);
            //00:00:00.1155672
            this._GdsAgent = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Gds-агент", rus: "Gds-агенты" })
                .lookup(function () { return Luxena.sd.GdsAgent; });
            /** Название */
            this.Name = this.member()
                .localizeTitle({ ru: "Название" })
                .string()
                .calculated()
                .nonsaved()
                .entityName();
            this.Codes = this.member()
                .string()
                .calculated()
                .nonsaved();
            /** Источник документов */
            this.Origin = this.member()
                .localizeTitle({ ru: "Источник документов" })
                .enum(Luxena.ProductOrigin)
                .required();
            /** Код агента */
            this.Code = this.member()
                .localizeTitle({ ru: "Код агента" })
                .string();
            /** Код офиса */
            this.OfficeCode = this.member()
                .localizeTitle({ ru: "Код офиса" })
                .string();
            /** Персона */
            this.Person = this.member()
                .localizeTitle({ ru: "Персона", rus: "Персоны" })
                .lookup(function () { return Luxena.sd.Person; });
            /** Владелец */
            this.Office = this.member()
                .localizeTitle({ ru: "Владелец" })
                .lookup(function () { return Luxena.sd.Party; });
            this._isAbstract = false;
            this._name = "GdsAgent";
            this._names = "GdsAgents";
            this._isEntity = true;
            this._localizeTitle({ ru: "Gds-агент", rus: "Gds-агенты" });
            this._getDerivedEntities = null;
            this._className = "GdsAgent";
            this._getRootEntity = function () { return Luxena.sd.GdsAgent; };
            this._store = Luxena.db.GdsAgents;
            this._saveStore = Luxena.db.GdsAgents;
            this._lookupStore = Luxena.db.GdsAgentLookup;
            this._referenceFields = { id: "Id", name: "Name" };
        }
        GdsAgentSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return GdsAgentSemantic;
    })(Entity2Semantic);
    Luxena.GdsAgentSemantic = GdsAgentSemantic;
    //00:00:00.1202786
    /** Внутренний перевод */
    var InternalTransferSemantic = (function (_super) {
        __extends(InternalTransferSemantic, _super);
        function InternalTransferSemantic() {
            _super.call(this);
            //00:00:00.1204133
            this._InternalTransfer = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Внутренний перевод", rus: "Внутренние переводы" })
                .lookup(function () { return Luxena.sd.InternalTransfer; });
            /** Номер */
            this.Number = this.member()
                .localizeTitle({ ru: "Номер" })
                .string()
                .entityName();
            /** Дата */
            this.Date = this.member()
                .localizeTitle({ en: "Date", ru: "Дата" })
                .date()
                .required()
                .entityDate();
            /** Сумма */
            this.Amount = this.member()
                .localizeTitle({ ru: "Сумма" })
                .float()
                .required();
            /** Из заказа */
            this.FromOrder = this.member()
                .localizeTitle({ ru: "Из заказа" })
                .lookup(function () { return Luxena.sd.Order; });
            /** От контрагента */
            this.FromParty = this.member()
                .localizeTitle({ ru: "От контрагента", rus: "Заказчики" })
                .lookup(function () { return Luxena.sd.Party; })
                .required();
            /** В заказ */
            this.ToOrder = this.member()
                .localizeTitle({ ru: "В заказ" })
                .lookup(function () { return Luxena.sd.Order; });
            /** К контрагенту */
            this.ToParty = this.member()
                .localizeTitle({ ru: "К контрагенту", rus: "Заказчики" })
                .lookup(function () { return Luxena.sd.Party; })
                .required();
            this._isAbstract = false;
            this._name = "InternalTransfer";
            this._names = "InternalTransfers";
            this._isEntity = true;
            this._localizeTitle({ ru: "Внутренний перевод", rus: "Внутренние переводы" });
            this._getDerivedEntities = null;
            this._className = "InternalTransfer";
            this._getRootEntity = function () { return Luxena.sd.InternalTransfer; };
            this._store = Luxena.db.InternalTransfers;
            this._saveStore = Luxena.db.InternalTransfers;
            this._lookupStore = Luxena.db.InternalTransferLookup;
            this._referenceFields = { id: "Id", name: "Number" };
        }
        InternalTransferSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return InternalTransferSemantic;
    })(Entity2Semantic);
    Luxena.InternalTransferSemantic = InternalTransferSemantic;
    //00:00:00.1218779
    /** Мильная карта */
    var MilesCardSemantic = (function (_super) {
        __extends(MilesCardSemantic, _super);
        function MilesCardSemantic() {
            _super.call(this);
            //00:00:00.1221004
            this._MilesCard = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Мильная карта", rus: "Мильные карты" })
                .lookup(function () { return Luxena.sd.MilesCard; });
            /** Номер */
            this.Number = this.member()
                .localizeTitle({ ru: "Номер" })
                .string()
                .entityName();
            /** Владелец */
            this.Owner = this.member()
                .localizeTitle({ ru: "Владелец" })
                .lookup(function () { return Luxena.sd.Person; })
                .required();
            /** Организация */
            this.Organization = this.member()
                .localizeTitle({ ru: "Организация", rus: "Организации" })
                .lookup(function () { return Luxena.sd.Organization; });
            this._isAbstract = false;
            this._name = "MilesCard";
            this._names = "MilesCards";
            this.icon("desktop");
            this._isEntity = true;
            this._localizeTitle({ ru: "Мильная карта", rus: "Мильные карты" });
            this._getDerivedEntities = null;
            this._className = "MilesCard";
            this._getRootEntity = function () { return Luxena.sd.MilesCard; };
            this._store = Luxena.db.MilesCards;
            this._saveStore = Luxena.db.MilesCards;
            this._lookupStore = Luxena.db.MilesCardLookup;
            this._referenceFields = { id: "Id", name: "Number" };
        }
        MilesCardSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return MilesCardSemantic;
    })(Entity2Semantic);
    Luxena.MilesCardSemantic = MilesCardSemantic;
    //00:00:00.1249445
    /** Заказ */
    var OrderSemantic = (function (_super) {
        __extends(OrderSemantic, _super);
        function OrderSemantic() {
            _super.call(this);
            //00:00:00.1250803
            this._Order = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Заказ", rus: "Заказы" })
                .lookup(function () { return Luxena.sd.Order; });
            /** Номер */
            this.Number = this.member()
                .localizeTitle({ ru: "Номер" })
                .string()
                .length(10, 0, 0)
                .entityName()
                .unique();
            /** Дата выпуска */
            this.IssueDate = this.member()
                .localizeTitle({ ru: "Дата выпуска" })
                .date()
                .required()
                .entityDate();
            /** Аннулирован */
            this.IsVoid = this.member()
                .localizeTitle({ ru: "Аннулирован" })
                .bool()
                .required();
            /** Плательщик */
            this.BillToName = this.member()
                .localizeTitle({ ru: "Плательщик" })
                .string();
            /** Общий доступ */
            this.IsPublic = this.member()
                .localizeTitle({ ru: "Общий доступ" })
                .bool()
                .required();
            /** Отображать в контроле оплат */
            this.IsSubjectOfPaymentsControl = this.member()
                .localizeTitle({ ru: "Отображать в контроле оплат" })
                .bool()
                .required();
            /** Выделять сервисный сбор */
            this.SeparateServiceFee = this.member()
                .localizeTitle({ ru: "Выделять сервисный сбор" })
                .bool();
            /** НДС только от сервисного сбора */
            this.UseServiceFeeOnlyInVat = this.member()
                .localizeTitle({ ru: "НДС только от сервисного сбора" })
                .bool()
                .required();
            /** Скидка */
            this.Discount = this.member()
                .localizeTitle({ ru: "Скидка" })
                .money()
                .readOnly()
                .nonsaved();
            /** Итого */
            this.Total = this.member()
                .localizeTitle({ ru: "Итого" })
                .money()
                .readOnly()
                .nonsaved();
            /** В т.ч. НДС */
            this.Vat = this.member()
                .localizeTitle({ ru: "В т.ч. НДС" })
                .money()
                .readOnly()
                .nonsaved();
            /** Оплачено */
            this.Paid = this.member()
                .localizeTitle({ ru: "Оплачено" })
                .money()
                .readOnly()
                .nonsaved();
            /** К оплате */
            this.TotalDue = this.member()
                .localizeTitle({ ru: "К оплате" })
                .money()
                .readOnly()
                .nonsaved();
            /** Оплачен */
            this.IsPaid = this.member()
                .localizeTitle({ ru: "Оплачен" })
                .bool()
                .readOnly()
                .nonsaved()
                .required();
            /** НДС к оплате */
            this.VatDue = this.member()
                .localizeTitle({ ru: "НДС к оплате" })
                .money()
                .readOnly()
                .nonsaved();
            /** Баланс взаиморасчетов */
            this.DeliveryBalance = this.member()
                .localizeTitle({ ru: "Баланс взаиморасчетов" })
                .float()
                .readOnly()
                .nonsaved()
                .required();
            /** Сервисный сбор */
            this.ServiceFee = this.member()
                .localizeTitle({ ru: "Сервисный сбор" })
                .money()
                .calculated()
                .nonsaved();
            /** Примечание */
            this.Note = this.member()
                .localizeTitle({ ru: "Примечание" })
                .string();
            this.InvoiceLastIndex = this.member()
                .int();
            /** Заказчик */
            this.Customer = this.member()
                .localizeTitle({ ru: "Заказчик", rus: "Заказчики" })
                .lookup(function () { return Luxena.sd.Party; });
            /** Плательщик */
            this.BillTo = this.member()
                .localizeTitle({ ru: "Плательщик", rus: "Заказчики" })
                .lookup(function () { return Luxena.sd.Party; });
            /** Получатель */
            this.ShipTo = this.member()
                .localizeTitle({ ru: "Получатель", rus: "Заказчики" })
                .lookup(function () { return Luxena.sd.Party; });
            /** Ответственный */
            this.AssignedTo = this.member()
                .localizeTitle({ ru: "Ответственный", rus: "Агенты" })
                .lookup(function () { return Luxena.sd.Person; });
            /** Владелец */
            this.Owner = this.member()
                .localizeTitle({ ru: "Владелец" })
                .lookup(function () { return Luxena.sd.Party; });
            /** Банковский счёт */
            this.BankAccount = this.member()
                .localizeTitle({ ru: "Банковский счёт", rus: "Банковские счёта" })
                .lookup(function () { return Luxena.sd.BankAccount; });
            this.Items = this.collection(function () { return Luxena.sd.OrderItem; }, function (se) { return se.Order; });
            this.Products = this.collection(function () { return Luxena.sd.Product; }, function (se) { return se.Order; });
            this.Invoices = this.collection(function () { return Luxena.sd.Invoice; }, function (se) { return se.Order; });
            this.Payments = this.collection(function () { return Luxena.sd.Payment; }, function (se) { return se.Order; });
            /** Входящие внутренние переводы */
            this.IncomingTransfers = this.collection(function () { return Luxena.sd.InternalTransfer; }, function (se) { return se.ToOrder; }, function (m) { return m
                .localizeTitle({ ru: "Входящие внутренние переводы" }); });
            /** Исходящие внутренние переводы */
            this.OutgoingTransfers = this.collection(function () { return Luxena.sd.InternalTransfer; }, function (se) { return se.FromOrder; }, function (m) { return m
                .localizeTitle({ ru: "Исходящие внутренние переводы" }); });
            this._isAbstract = false;
            this._name = "Order";
            this._names = "Orders";
            this.icon("briefcase");
            this._isEntity = true;
            this._localizeTitle({ ru: "Заказ", rus: "Заказы" });
            this._getDerivedEntities = null;
            this._className = "Order";
            this._getRootEntity = function () { return Luxena.sd.Order; };
            this._store = Luxena.db.Orders;
            this._saveStore = Luxena.db.Orders;
            this._lookupStore = Luxena.db.OrderLookup;
            this._referenceFields = { id: "Id", name: "Number" };
            this.big();
        }
        OrderSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return OrderSemantic;
    })(Entity2Semantic);
    Luxena.OrderSemantic = OrderSemantic;
    //00:00:00.1283060
    /** Чек */
    var OrderCheckSemantic = (function (_super) {
        __extends(OrderCheckSemantic, _super);
        function OrderCheckSemantic() {
            _super.call(this);
            //00:00:00.1284002
            this._OrderCheck = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Чек", rus: "Чеки" })
                .lookup(function () { return Luxena.sd.OrderCheck; });
            /** Дата */
            this.Date = this.member()
                .localizeTitle({ en: "Date", ru: "Дата" })
                .dateTime2()
                .required()
                .entityDate();
            /** Тип чека */
            this.CheckType = this.member()
                .localizeTitle({ ru: "Тип чека" })
                .enum(Luxena.CheckType)
                .required();
            /** Номер чека */
            this.CheckNumber = this.member()
                .localizeTitle({ ru: "Номер чека" })
                .string()
                .length(10, 0, 0)
                .entityName();
            /** Валюта */
            this.Currency = this.member()
                .localizeTitle({ ru: "Валюта" })
                .string();
            /** Сумма чека */
            this.CheckAmount = this.member()
                .localizeTitle({ ru: "Сумма чека" })
                .float(2);
            /** В т.ч. НДС */
            this.CheckVat = this.member()
                .localizeTitle({ ru: "В т.ч. НДС" })
                .float(2);
            /** Сумма оплаты */
            this.PayAmount = this.member()
                .localizeTitle({ ru: "Сумма оплаты", ruDesc: "Деньги, которые клиент передал кассиру (из которых последний возвращает сдачу)" })
                .float(2);
            /** Тип оплаты */
            this.PaymentType = this.member()
                .localizeTitle({ ru: "Тип оплаты" })
                .enum(Luxena.CheckPaymentType);
            /** Описание */
            this.Description = this.member()
                .localizeTitle({ ru: "Описание" })
                .string();
            /** Заказ */
            this.Order = this.member()
                .localizeTitle({ ru: "Заказ", rus: "Заказы" })
                .lookup(function () { return Luxena.sd.Order; });
            /** Печатал чек */
            this.Person = this.member()
                .localizeTitle({ ru: "Печатал чек" })
                .lookup(function () { return Luxena.sd.Person; });
            this._isAbstract = false;
            this._name = "OrderCheck";
            this._names = "OrderChecks";
            this._isEntity = true;
            this._localizeTitle({ ru: "Чек", rus: "Чеки" });
            this._getDerivedEntities = null;
            this._className = "OrderCheck";
            this._getRootEntity = function () { return Luxena.sd.OrderCheck; };
            this._store = Luxena.db.OrderChecks;
            this._saveStore = Luxena.db.OrderChecks;
            this._lookupStore = Luxena.db.OrderCheckLookup;
            this._referenceFields = { id: "Id", name: "CheckNumber" };
        }
        OrderCheckSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return OrderCheckSemantic;
    })(Entity2Semantic);
    Luxena.OrderCheckSemantic = OrderCheckSemantic;
    //00:00:00.1316908
    /** Позиция заказа */
    var OrderItemSemantic = (function (_super) {
        __extends(OrderItemSemantic, _super);
        function OrderItemSemantic() {
            _super.call(this);
            //00:00:00.1318046
            this._OrderItem = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Позиция заказа", rus: "Позиции заказа" })
                .lookup(function () { return Luxena.sd.OrderItem; });
            /** Номер */
            this.Position = this.member()
                .localizeTitle({ ru: "Номер" })
                .int()
                .required()
                .entityPosition();
            /** Название */
            this.Text = this.member()
                .localizeTitle({ ru: "Название" })
                .text(3)
                .entityName();
            /** Тип */
            this.LinkType = this.member()
                .localizeTitle({ ru: "Тип" })
                .enum(Luxena.OrderItemLinkType);
            /** Цена */
            this.Price = this.member()
                .localizeTitle({ ru: "Цена" })
                .money();
            /** Количество */
            this.Quantity = this.member()
                .localizeTitle({ ru: "Количество" })
                .int()
                .required();
            /** Итого */
            this.Total = this.member()
                .localizeTitle({ ru: "Итого" })
                .money()
                .calculated()
                .nonsaved();
            /** Скидка */
            this.Discount = this.member()
                .localizeTitle({ ru: "Скидка" })
                .money();
            /** К оплате */
            this.GrandTotal = this.member()
                .localizeTitle({ ru: "К оплате" })
                .money()
                .readOnly()
                .nonsaved();
            this.GivenVat = this.member()
                .money()
                .readOnly()
                .nonsaved();
            this.TaxedTotal = this.member()
                .money()
                .readOnly()
                .nonsaved();
            this.HasVat = this.member()
                .bool()
                .required();
            this.ServiceFee = this.member()
                .money()
                .calculated()
                .nonsaved();
            this.IsDelivered = this.member()
                .bool()
                .calculated()
                .nonsaved()
                .required();
            this.CheckNameUA = this.member()
                .string()
                .calculated()
                .nonsaved();
            /** Заказ */
            this.Order = this.member()
                .localizeTitle({ ru: "Заказ", rus: "Заказы" })
                .lookup(function () { return Luxena.sd.Order; });
            /** Услуга */
            this.Product = this.member()
                .localizeTitle({ ru: "Услуга", rus: "Все услуги" })
                .lookup(function () { return Luxena.sd.Product; });
            /** Накладная */
            this.Consignment = this.member()
                .localizeTitle({ ru: "Накладная", rus: "Накладные" })
                .lookup(function () { return Luxena.sd.Consignment; });
            this._isAbstract = false;
            this._name = "OrderItem";
            this._names = "OrderItems";
            this._isEntity = true;
            this._localizeTitle({ ru: "Позиция заказа", rus: "Позиции заказа" });
            this._getDerivedEntities = null;
            this._className = "OrderItem";
            this._getRootEntity = function () { return Luxena.sd.OrderItem; };
            this._store = Luxena.db.OrderItems;
            this._saveStore = Luxena.db.OrderItems;
            this._lookupStore = Luxena.db.OrderItemLookup;
            this._referenceFields = { id: "Id", name: "Text" };
        }
        OrderItemSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return OrderItemSemantic;
    })(Entity2Semantic);
    Luxena.OrderItemSemantic = OrderItemSemantic;
    //00:00:00.1344199
    /** Паспорт */
    var PassportSemantic = (function (_super) {
        __extends(PassportSemantic, _super);
        function PassportSemantic() {
            _super.call(this);
            //00:00:00.1345234
            this._Passport = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Паспорт", rus: "Паспорта" })
                .lookup(function () { return Luxena.sd.Passport; });
            /** Номер */
            this.Number = this.member()
                .localizeTitle({ ru: "Номер" })
                .string()
                .entityName();
            /** Имя */
            this.FirstName = this.member()
                .localizeTitle({ ru: "Имя" })
                .string();
            /** Отчество */
            this.MiddleName = this.member()
                .localizeTitle({ ru: "Отчество" })
                .string();
            /** Фамилия */
            this.LastName = this.member()
                .localizeTitle({ ru: "Фамилия" })
                .string();
            /** Ф.И.О. */
            this.Name = this.member()
                .localizeTitle({ ru: "Ф.И.О." })
                .string()
                .calculated()
                .nonsaved();
            /** Дата рождения */
            this.Birthday = this.member()
                .localizeTitle({ ru: "Дата рождения" })
                .date();
            /** Пол */
            this.Gender = this.member()
                .localizeTitle({ ru: "Пол" })
                .enum(Luxena.Gender);
            /** Действителен до */
            this.ExpiredOn = this.member()
                .localizeTitle({ ru: "Действителен до" })
                .date();
            /** Примечание */
            this.Note = this.member()
                .localizeTitle({ ru: "Примечание" })
                .string();
            /** Данные для Amadeus */
            this.AmadeusString = this.member()
                .localizeTitle({ ru: "Данные для Amadeus" })
                .string()
                .calculated()
                .nonsaved();
            /** Данные для Galileo */
            this.GalileoString = this.member()
                .localizeTitle({ ru: "Данные для Galileo" })
                .string()
                .calculated()
                .nonsaved();
            /** Владелец */
            this.Owner = this.member()
                .localizeTitle({ ru: "Владелец" })
                .lookup(function () { return Luxena.sd.Person; })
                .required();
            /** Гражданство */
            this.Citizenship = this.member()
                .localizeTitle({ ru: "Гражданство" })
                .lookup(function () { return Luxena.sd.Country; });
            /** Выдавшая страна */
            this.IssuedBy = this.member()
                .localizeTitle({ ru: "Выдавшая страна" })
                .lookup(function () { return Luxena.sd.Country; });
            this._isAbstract = false;
            this._name = "Passport";
            this._names = "Passports";
            this.icon("certificate");
            this._isEntity = true;
            this._localizeTitle({ ru: "Паспорт", rus: "Паспорта" });
            this._getDerivedEntities = null;
            this._className = "Passport";
            this._getRootEntity = function () { return Luxena.sd.Passport; };
            this._store = Luxena.db.Passports;
            this._saveStore = Luxena.db.Passports;
            this._lookupStore = Luxena.db.PassportLookup;
            this._referenceFields = { id: "Id", name: "Number" };
        }
        PassportSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return PassportSemantic;
    })(Entity2Semantic);
    Luxena.PassportSemantic = PassportSemantic;
    //00:00:00.1389451
    /** Платёж */
    var PaymentSemantic = (function (_super) {
        __extends(PaymentSemantic, _super);
        function PaymentSemantic() {
            _super.call(this);
            //00:00:00.1391226
            this._Payment = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Платёж", rus: "Платежи" })
                .lookup(function () { return Luxena.sd.Payment; });
            /** Форма оплаты */
            this.PaymentForm = this.member()
                .localizeTitle({ ru: "Форма оплаты" })
                .enum(Luxena.PaymentForm)
                .required()
                .entityType();
            /** Номер */
            this.Number = this.member()
                .localizeTitle({ ru: "Номер" })
                .string()
                .entityName();
            /** Дата */
            this.Date = this.member()
                .localizeTitle({ en: "Date", ru: "Дата" })
                .date()
                .required()
                .entityDate();
            /** Номер документа */
            this.DocumentNumber = this.member()
                .localizeTitle({ ru: "Номер документа" })
                .string()
                .length(10, 0, 0);
            this.DocumentUniqueCode = this.member()
                .string()
                .calculated()
                .nonsaved();
            /** Дата счета/квитанции */
            this.InvoiceDate = this.member()
                .localizeTitle({ ru: "Дата счета/квитанции" })
                .date()
                .calculated()
                .nonsaved();
            /** Сумма */
            this.Amount = this.member()
                .localizeTitle({ ru: "Сумма" })
                .money()
                .subject();
            /** В т.ч. НДС */
            this.Vat = this.member()
                .localizeTitle({ ru: "В т.ч. НДС" })
                .money()
                .subject();
            /** Получен от */
            this.ReceivedFrom = this.member()
                .localizeTitle({ ru: "Получен от" })
                .string();
            /** Дата проводки */
            this.PostedOn = this.member()
                .localizeTitle({ ru: "Дата проводки" })
                .date()
                .subject();
            /** Сохранить проведенным */
            this.SavePosted = this.member()
                .localizeTitle({ ru: "Сохранить проведенным" })
                .bool()
                .calculated()
                .subject();
            /** Примечание */
            this.Note = this.member()
                .localizeTitle({ ru: "Примечание" })
                .string();
            /** Аннулирован */
            this.IsVoid = this.member()
                .localizeTitle({ ru: "Аннулирован" })
                .bool()
                .required();
            this.IsPosted = this.member()
                .bool()
                .calculated()
                .nonsaved()
                .required();
            this.PrintedDocument = this.member();
            /** Плательщик */
            this.Payer = this.member()
                .localizeTitle({ ru: "Плательщик" })
                .lookup(function () { return Luxena.sd.Party; })
                .required();
            /** Заказ */
            this.Order = this.member()
                .localizeTitle({ ru: "Заказ", rus: "Заказы" })
                .lookup(function () { return Luxena.sd.Order; })
                .subject();
            /** Квитанция */
            this.Invoice = this.member()
                .localizeTitle({ ru: "Квитанция", rus: "Квитанции" })
                .lookup(function () { return Luxena.sd.Invoice; });
            /** Ответственный */
            this.AssignedTo = this.member()
                .localizeTitle({ ru: "Ответственный", rus: "Агенты" })
                .lookup(function () { return Luxena.sd.Person; });
            /** Зарегистрирован */
            this.RegisteredBy = this.member()
                .localizeTitle({ ru: "Зарегистрирован", rus: "Агенты" })
                .lookup(function () { return Luxena.sd.Person; });
            /** Владелец */
            this.Owner = this.member()
                .localizeTitle({ ru: "Владелец" })
                .lookup(function () { return Luxena.sd.Party; });
            /** Платёжная система */
            this.PaymentSystem = this.member()
                .localizeTitle({ ru: "Платёжная система", rus: "Платёжные системы" })
                .lookup(function () { return Luxena.sd.PaymentSystem; });
            /** Анулировать */
            this.Void = this.action()
                .localizeTitle({ ru: "Анулировать" });
            /** Восстановить */
            this.Unvoid = this.action()
                .localizeTitle({ ru: "Восстановить" });
            this.GetNote = this.action();
            this._isAbstract = true;
            this._name = "Payment";
            this._names = "Payments";
            this._isEntity = true;
            this._localizeTitle({ ru: "Платёж", rus: "Платежи" });
            this._getDerivedEntities = function () { return [
                Luxena.sd.WireTransfer, Luxena.sd.CheckPayment, Luxena.sd.CashInOrderPayment, Luxena.sd.CashOutOrderPayment, Luxena.sd.ElectronicPayment
            ]; };
            this._className = "Payment";
            this._getRootEntity = function () { return Luxena.sd.Payment; };
            this._store = Luxena.db.Payments;
            this._saveStore = Luxena.db.Payments;
            this._lookupStore = Luxena.db.PaymentLookup;
            this._referenceFields = { id: "Id", name: "Number" };
        }
        PaymentSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return PaymentSemantic;
    })(Entity2Semantic);
    Luxena.PaymentSemantic = PaymentSemantic;
    //00:00:00.1464680
    /** Услуга */
    var ProductSemantic = (function (_super) {
        __extends(ProductSemantic, _super);
        function ProductSemantic() {
            _super.call(this);
            //00:00:00.1466332
            this._Product = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Услуга", rus: "Все услуги" })
                .lookup(function () { return Luxena.sd.Product; });
            this.Type = this.member()
                .localizeTitle({ ru: "Вид услуги" })
                .enum(Luxena.ProductType)
                .required()
                .length(12, 0, 0)
                .entityType();
            /** Название */
            this.Name = this.member()
                .localizeTitle({ ru: "Название" })
                .string()
                .length(16, 0, 0)
                .entityName();
            /** Дата выпуска */
            this.IssueDate = this.member()
                .localizeTitle({ ru: "Дата выпуска" })
                .date()
                .required()
                .entityDate();
            /** Это возврат */
            this.IsRefund = this.member()
                .localizeTitle({ ru: "Это возврат" })
                .bool()
                .required();
            this.IsReservation = this.member()
                .bool()
                .required();
            /** Обработан */
            this.IsProcessed = this.member()
                .localizeTitle({ ru: "Обработан" })
                .bool()
                .required();
            /** Аннулирован */
            this.IsVoid = this.member()
                .localizeTitle({ ru: "Аннулирован" })
                .bool()
                .required();
            /** К обработке */
            this.RequiresProcessing = this.member()
                .localizeTitle({ ru: "К обработке" })
                .bool()
                .required();
            this.IsDelivered = this.member()
                .bool()
                .calculated()
                .nonsaved()
                .required();
            /** Оплачен */
            this.IsPaid = this.member()
                .localizeTitle({ ru: "Оплачен" })
                .bool()
                .calculated()
                .nonsaved()
                .required();
            /** Маршрут */
            this.Itinerary = this.member()
                .localizeTitle({ ru: "Маршрут" })
                .string()
                .length(16, 0, 0);
            /** Дата начала */
            this.StartDate = this.member()
                .localizeTitle({ ru: "Дата начала" })
                .date();
            /** Дата окончания */
            this.FinishDate = this.member()
                .localizeTitle({ ru: "Дата окончания" })
                .date();
            /** Бронировка */
            this.PnrCode = this.member()
                .localizeTitle({ ru: "Бронировка" })
                .string();
            /** Туркод */
            this.TourCode = this.member()
                .localizeTitle({ ru: "Туркод" })
                .string();
            /** Бронировщик: код офиса GDS-агента */
            this.BookerOffice = this.member()
                .localizeTitle({ ru: "Бронировщик: код офиса GDS-агента", ruShort: "код офиса" })
                .emptyText("код офиса")
                .string()
                .length(8, 0, 0);
            /** Бронировщик: код GDS-агента */
            this.BookerCode = this.member()
                .localizeTitle({ ru: "Бронировщик: код GDS-агента", ruShort: "код агента" })
                .emptyText("код агента")
                .string()
                .length(8, 0, 0);
            /** Тикетер: код офиса GDS-агента */
            this.TicketerOffice = this.member()
                .localizeTitle({ ru: "Тикетер: код офиса GDS-агента", ruShort: "код офиса" })
                .emptyText("код офиса")
                .string()
                .length(8, 0, 0);
            /** Тикетер: код GDS-агента */
            this.TicketerCode = this.member()
                .localizeTitle({ ru: "Тикетер: код GDS-агента", ruShort: "код агента" })
                .emptyText("код агента")
                .string()
                .length(8, 0, 0);
            /** IATA офис */
            this.TicketingIataOffice = this.member()
                .localizeTitle({ ru: "IATA офис" })
                .string(10);
            this.IsTicketerRobot = this.member()
                .bool()
                .required();
            /** Тариф */
            this.Fare = this.member()
                .localizeTitle({ ru: "Тариф" })
                .money();
            /** Экв. тариф */
            this.EqualFare = this.member()
                .localizeTitle({ ru: "Экв. тариф" })
                .defaultMoney()
                .subject();
            /** Таксы */
            this.FeesTotal = this.member()
                .localizeTitle({ ru: "Таксы" })
                .defaultMoney()
                .subject();
            /** Штраф за отмену */
            this.CancelFee = this.member()
                .localizeTitle({ ru: "Штраф за отмену" })
                .defaultMoney()
                .subject();
            /** К перечислению провайдеру */
            this.Total = this.member()
                .localizeTitle({ ru: "К перечислению провайдеру" })
                .defaultMoney()
                .readOnly()
                .nonsaved();
            /** В т.ч. НДС */
            this.Vat = this.member()
                .localizeTitle({ ru: "В т.ч. НДС" })
                .defaultMoney();
            /** Сервисный сбор */
            this.ServiceFee = this.member()
                .localizeTitle({ ru: "Сервисный сбор" })
                .defaultMoney()
                .subject();
            /** Штраф сервисного сбора */
            this.ServiceFeePenalty = this.member()
                .localizeTitle({ ru: "Штраф сервисного сбора" })
                .defaultMoney()
                .subject();
            /** Доп. доход */
            this.Handling = this.member()
                .localizeTitle({ ru: "Доп. доход" })
                .defaultMoney()
                .subject();
            /** Комиссия */
            this.Commission = this.member()
                .localizeTitle({ ru: "Комиссия" })
                .defaultMoney();
            /** Скидка от комиссии */
            this.CommissionDiscount = this.member()
                .localizeTitle({ ru: "Скидка от комиссии" })
                .defaultMoney()
                .subject();
            /** Скидка */
            this.Discount = this.member()
                .localizeTitle({ ru: "Скидка" })
                .defaultMoney()
                .subject();
            /** Бонусная скидка */
            this.BonusDiscount = this.member()
                .localizeTitle({ ru: "Бонусная скидка" })
                .defaultMoney()
                .subject();
            /** Бонусное накопление */
            this.BonusAccumulation = this.member()
                .localizeTitle({ ru: "Бонусное накопление" })
                .defaultMoney();
            /** Cбор за возврат */
            this.RefundServiceFee = this.member()
                .localizeTitle({ ru: "Cбор за возврат" })
                .defaultMoney()
                .subject();
            this.ServiceTotal = this.member()
                .money()
                .calculated()
                .nonsaved();
            /** К оплате */
            this.GrandTotal = this.member()
                .localizeTitle({ ru: "К оплате" })
                .defaultMoney();
            this.CancelCommissionPercent = this.member()
                .float();
            /** Комисия за возврат */
            this.CancelCommission = this.member()
                .localizeTitle({ ru: "Комисия за возврат" })
                .money();
            /** % комиссии */
            this.CommissionPercent = this.member()
                .localizeTitle({ ru: "% комиссии" })
                .float();
            this.TotalToTransfer = this.member()
                .money()
                .calculated()
                .nonsaved();
            this.Profit = this.member()
                .money()
                .calculated()
                .nonsaved();
            this.ExtraCharge = this.member()
                .money()
                .calculated()
                .nonsaved();
            /** Тип оплаты */
            this.PaymentType = this.member()
                .localizeTitle({ ru: "Тип оплаты" })
                .enum(Luxena.PaymentType)
                .required();
            /** Налоговая группа */
            this.TaxMode = this.member()
                .localizeTitle({ ru: "Налоговая группа" })
                .enum(Luxena.CheckItemTaxMode)
                .required();
            /** Примечание */
            this.Note = this.member()
                .localizeTitle({ ru: "Примечание" })
                .string();
            /** Оригинатор */
            this.Originator = this.member()
                .localizeTitle({ ru: "Оригинатор" })
                .enum(Luxena.GdsOriginator)
                .required();
            /** Источник */
            this.Origin = this.member()
                .localizeTitle({ ru: "Источник" })
                .enum(Luxena.ProductOrigin)
                .required();
            /** Пассажир */
            this.PassengerName = this.member()
                .localizeTitle({ ru: "Пассажир" })
                .string();
            /** Пассажир из GDS */
            this.GdsPassengerName = this.member()
                .localizeTitle({ ru: "Пассажир из GDS" })
                .string()
                .calculated()
                .length(20, 0, 0);
            /** Пассажир */
            this.Passenger = this.member()
                .localizeTitle({ ru: "Пассажир" })
                .lookup(function () { return Luxena.sd.Person; })
                .calculated()
                .nonsaved();
            /** Продюсер */
            this.Producer = this.member()
                .localizeTitle({ ru: "Продюсер" })
                .lookup(function () { return Luxena.sd.Organization; });
            /** Провайдер */
            this.Provider = this.member()
                .localizeTitle({ ru: "Провайдер" })
                .lookup(function () { return Luxena.sd.Organization; });
            /** Перевыпуск для */
            this.ReissueFor = this.member()
                .localizeTitle({ ru: "Перевыпуск для" })
                .lookup(function () { return Luxena.sd.Product; });
            /** Исходный документ */
            this.RefundedProduct = this.member()
                .localizeTitle({ ru: "Исходный документ" })
                .lookup(function () { return Luxena.sd.Product; });
            /** Заказчик */
            this.Customer = this.member()
                .localizeTitle({ ru: "Заказчик", rus: "Заказчики" })
                .lookup(function () { return Luxena.sd.Party; });
            /** Заказ */
            this.Order = this.member()
                .localizeTitle({ ru: "Заказ", rus: "Заказы" })
                .lookup(function () { return Luxena.sd.Order; })
                .subject();
            /** Посредник */
            this.Intermediary = this.member()
                .localizeTitle({ ru: "Посредник" })
                .lookup(function () { return Luxena.sd.Party; });
            /** Страна */
            this.Country = this.member()
                .localizeTitle({ ru: "Страна", rus: "Страны" })
                .lookup(function () { return Luxena.sd.Country; });
            /** Бронировщик */
            this.Booker = this.member()
                .localizeTitle({ ru: "Бронировщик", rus: "Агенты" })
                .lookup(function () { return Luxena.sd.Person; });
            /** Тикетер */
            this.Ticketer = this.member()
                .localizeTitle({ ru: "Тикетер", rus: "Агенты" })
                .lookup(function () { return Luxena.sd.Person; });
            /** Продавец */
            this.Seller = this.member()
                .localizeTitle({ ru: "Продавец", rus: "Агенты" })
                .lookup(function () { return Luxena.sd.Person; });
            /** Владелец */
            this.Owner = this.member()
                .localizeTitle({ ru: "Владелец" })
                .lookup(function () { return Luxena.sd.Party; });
            /** Оригинальный документ */
            this.OriginalDocument = this.member()
                .localizeTitle({ ru: "Оригинальный документ" })
                .lookup(function () { return Luxena.sd.GdsFile; });
            this.Passengers = this.collection(function () { return Luxena.sd.ProductPassenger; }, function (se) { return se.Product; });
            this.Products_ReissueFor = this.collection(function () { return Luxena.sd.Product; }, function (se) { return se.ReissueFor; });
            this.Products_RefundedProduct = this.collection(function () { return Luxena.sd.Product; }, function (se) { return se.RefundedProduct; });
            this._isAbstract = true;
            this._name = "Product";
            this._names = "Products";
            this.icon("suitcase");
            this._isEntity = true;
            this._localizeTitle({ ru: "Услуга", rus: "Все услуги" });
            this._getDerivedEntities = function () { return [
                Luxena.sd.SimCard, Luxena.sd.AviaTicket, Luxena.sd.AviaDocument, Luxena.sd.BusTicket, Luxena.sd.BusDocument, Luxena.sd.CarRental, Luxena.sd.AviaRefund, Luxena.sd.BusTicketRefund, Luxena.sd.PasteboardRefund, Luxena.sd.InsuranceRefund, Luxena.sd.GenericProduct, Luxena.sd.Pasteboard, Luxena.sd.RailwayDocument, Luxena.sd.AviaMco, Luxena.sd.Accommodation, Luxena.sd.Insurance, Luxena.sd.InsuranceDocument, Luxena.sd.Isic, Luxena.sd.Transfer, Luxena.sd.Tour, Luxena.sd.Excursion
            ]; };
            this._className = "Product";
            this._getRootEntity = function () { return Luxena.sd.Product; };
            this._store = Luxena.db.Products;
            this._saveStore = Luxena.db.Products;
            this._lookupStore = Luxena.db.ProductLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.big();
        }
        ProductSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ProductSemantic;
    })(Entity2Semantic);
    Luxena.ProductSemantic = ProductSemantic;
    //00:00:00.1519604
    /** Пассажир */
    var ProductPassengerSemantic = (function (_super) {
        __extends(ProductPassengerSemantic, _super);
        function ProductPassengerSemantic() {
            _super.call(this);
            //00:00:00.1520591
            this._ProductPassenger = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Пассажир", rus: "Пассажиры" })
                .lookup(function () { return Luxena.sd.ProductPassenger; });
            /** Имя пассажира */
            this.PassengerName = this.member()
                .localizeTitle({ ru: "Имя пассажира", ruShort: "имя" })
                .emptyText("имя")
                .string();
            /** Услуга */
            this.Product = this.member()
                .localizeTitle({ ru: "Услуга", rus: "Все услуги" })
                .lookup(function () { return Luxena.sd.Product; });
            /** Пассажир */
            this.Passenger = this.member()
                .localizeTitle({ ru: "Пассажир", ruShort: "персона" })
                .emptyText("персона")
                .lookup(function () { return Luxena.sd.Person; });
            this._isAbstract = false;
            this._name = "ProductPassenger";
            this._names = "ProductPassengers";
            this._isEntity = true;
            this._localizeTitle({ ru: "Пассажир", rus: "Пассажиры" });
            this._getDerivedEntities = null;
            this._className = "ProductPassenger";
            this._getRootEntity = function () { return Luxena.sd.ProductPassenger; };
            this._store = Luxena.db.ProductPassengers;
            this._saveStore = Luxena.db.ProductPassengers;
            this._referenceFields = { id: "Id", name: "" };
        }
        ProductPassengerSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ProductPassengerSemantic;
    })(Entity2Semantic);
    Luxena.ProductPassengerSemantic = ProductPassengerSemantic;
    //00:00:00.1631319
    /** Проживание */
    var AccommodationSemantic = (function (_super) {
        __extends(AccommodationSemantic, _super);
        function AccommodationSemantic() {
            _super.call(this);
            //00:00:00.1656842
            this._Accommodation = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Проживание", rus: "Проживания", ua: "Готель" })
                .lookup(function () { return Luxena.sd.Accommodation; });
            /** Гостиница */
            this.HotelName = this.member()
                .localizeTitle({ ru: "Гостиница" })
                .string();
            /** Офис гостиницы */
            this.HotelOffice = this.member()
                .localizeTitle({ ru: "Офис гостиницы", ruShort: "офис" })
                .emptyText("офис")
                .string();
            /** Код гостиницы */
            this.HotelCode = this.member()
                .localizeTitle({ ru: "Код гостиницы", ruShort: "код" })
                .emptyText("код")
                .string();
            /** Расположение */
            this.PlacementName = this.member()
                .localizeTitle({ ru: "Расположение" })
                .string();
            this.PlacementOffice = this.member()
                .emptyText("офис")
                .string();
            this.PlacementCode = this.member()
                .emptyText("код")
                .string();
            /** Тип проживания */
            this.AccommodationType = this.member()
                .localizeTitle({ ru: "Тип проживания", rus: "Типы проживания" })
                .lookup(function () { return Luxena.sd.AccommodationType; });
            /** Тип питания */
            this.CateringType = this.member()
                .localizeTitle({ ru: "Тип питания", rus: "Типы питания" })
                .lookup(function () { return Luxena.sd.CateringType; });
            this._isAbstract = false;
            this._name = "Accommodation";
            this._names = "Accommodations";
            this.icon("bed");
            this._isEntity = true;
            this._localizeTitle({ ru: "Проживание", rus: "Проживания", ua: "Готель" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Product; };
            this._className = "Product";
            this._getRootEntity = function () { return Luxena.sd.Product; };
            this._store = Luxena.db.Accommodations;
            this._saveStore = Luxena.db.Accommodations;
            this._lookupStore = Luxena.db.AccommodationLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.big();
            this.ReissueFor
                .localizeTitle({ ru: "Перевыпуск для" })
                .lookup(function () { return Luxena.sd.Accommodation; });
            this.Provider
                .lookup(function () { return Luxena.sd.AccommodationProvider; });
        }
        AccommodationSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return AccommodationSemantic;
    })(ProductSemantic);
    Luxena.AccommodationSemantic = AccommodationSemantic;
    //00:00:00.1678754
    /** Аэропорт */
    var AirportSemantic = (function (_super) {
        __extends(AirportSemantic, _super);
        function AirportSemantic() {
            _super.call(this);
            //00:00:00.1680489
            this._Airport = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Аэропорт", rus: "Аэропорты" })
                .lookup(function () { return Luxena.sd.Airport; });
            /** Код */
            this.Code = this.member()
                .localizeTitle({ ru: "Код" })
                .string(3, 3, 3)
                .required()
                .unique();
            /** Населенный пункт (англ.) */
            this.Settlement = this.member()
                .localizeTitle({ ru: "Населенный пункт (англ.)" })
                .string();
            /** Населенный пункт */
            this.LocalizedSettlement = this.member()
                .localizeTitle({ ru: "Населенный пункт" })
                .string();
            /** Широта */
            this.Latitude = this.member()
                .localizeTitle({ ru: "Широта" })
                .float();
            /** Долгота */
            this.Longitude = this.member()
                .localizeTitle({ ru: "Долгота" })
                .float();
            /** Страна */
            this.Country = this.member()
                .localizeTitle({ ru: "Страна", rus: "Страны" })
                .lookup(function () { return Luxena.sd.Country; })
                .required();
            this._isAbstract = false;
            this._name = "Airport";
            this._names = "Airports";
            this.icon("road");
            this._isEntity = true;
            this._localizeTitle({ ru: "Аэропорт", rus: "Аэропорты" });
            this._getDerivedEntities = null;
            this._className = "Airport";
            this._getRootEntity = function () { return Luxena.sd.Airport; };
            this._store = Luxena.db.Airports;
            this._saveStore = Luxena.db.Airports;
            this._lookupStore = Luxena.db.AirportLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.Name
                .required()
                .length(12, 0, 0);
        }
        AirportSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return AirportSemantic;
    })(Entity3Semantic);
    Luxena.AirportSemantic = AirportSemantic;
    //00:00:00.1798716
    /** Авиадокумент */
    var AviaDocumentSemantic = (function (_super) {
        __extends(AviaDocumentSemantic, _super);
        function AviaDocumentSemantic() {
            _super.call(this);
            //00:00:00.1805882
            this._AviaDocument = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Авиадокумент", rus: "Авиадокументы" })
                .lookup(function () { return Luxena.sd.AviaDocument; });
            this.AirlineIataCode = this.member()
                .string();
            /** Код АК */
            this.AirlinePrefixCode = this.member()
                .localizeTitle({ ru: "Код АК" })
                .string()
                .length(3, 3, 3);
            this.AirlineName = this.member()
                .string();
            /** Номер */
            this.Number = this.member()
                .localizeTitle({ ru: "Номер" })
                .string()
                .length(10, 10, 10);
            /** Номер */
            this.FullNumber = this.member()
                .localizeTitle({ ru: "Номер" })
                .string()
                .calculated()
                .nonsaved();
            this.ConjunctionNumbers = this.member()
                .string();
            /** Паспорт в GDS */
            this.GdsPassportStatus = this.member()
                .localizeTitle({ ru: "Паспорт в GDS" })
                .enum(Luxena.GdsPassportStatus)
                .required();
            this.GdsPassport = this.member()
                .string();
            this.PaymentForm = this.member()
                .string();
            this.PaymentDetails = this.member()
                .string();
            this.AirlinePnrCode = this.member()
                .string();
            this.Remarks = this.member()
                .string();
            this.AviaMcos_InConnectionWith = this.collection(function () { return Luxena.sd.AviaMco; }, function (se) { return se.InConnectionWith; });
            this._isAbstract = true;
            this._name = "AviaDocument";
            this._names = "AviaDocuments";
            this.icon("plane");
            this._isEntity = true;
            this._localizeTitle({ ru: "Авиадокумент", rus: "Авиадокументы" });
            this._getDerivedEntities = function () { return [
                Luxena.sd.AviaTicket, Luxena.sd.AviaRefund, Luxena.sd.AviaMco
            ]; };
            this._getBaseEntity = function () { return Luxena.sd.Product; };
            this._className = "Product";
            this._getRootEntity = function () { return Luxena.sd.Product; };
            this._store = Luxena.db.AviaDocuments;
            this._saveStore = Luxena.db.AviaDocuments;
            this._lookupStore = Luxena.db.AviaDocumentLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.big();
            this.Type
                .localizeTitle({ ru: "Вид услуги" })
                .enum(Luxena.ProductType, "AviaTicket", "AviaRefund", "AviaMco")
                .required()
                .length(12, 0, 0);
            this.Name
                .localizeTitle({ ru: "Номер" })
                .length(10, 0, 0);
            this.ReissueFor
                .lookup(function () { return Luxena.sd.AviaDocument; });
            this.Producer
                .localizeTitle({ ru: "Авиакомпания", rus: "Авиакомпании" })
                .lookup(function () { return Luxena.sd.Airline; });
        }
        AviaDocumentSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return AviaDocumentSemantic;
    })(ProductSemantic);
    Luxena.AviaDocumentSemantic = AviaDocumentSemantic;
    //00:00:00.1888133
    /** Автобусный билет или возврат */
    var BusDocumentSemantic = (function (_super) {
        __extends(BusDocumentSemantic, _super);
        function BusDocumentSemantic() {
            _super.call(this);
            //00:00:00.1892021
            this._BusDocument = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Автобусный билет или возврат", rus: "Автобусные билеты и возвраты" })
                .lookup(function () { return Luxena.sd.BusDocument; });
            /** Номер */
            this.Number = this.member()
                .localizeTitle({ ru: "Номер" })
                .string();
            /** Начальная станция */
            this.DeparturePlace = this.member()
                .localizeTitle({ ru: "Начальная станция", ruShort: "место" })
                .emptyText("место")
                .string();
            /** Дата отправления */
            this.DepartureDate = this.member()
                .localizeTitle({ ru: "Дата отправления", ruShort: "дата" })
                .emptyText("дата")
                .date();
            /** Время отправления */
            this.DepartureTime = this.member()
                .localizeTitle({ ru: "Время отправления", ruShort: "время" })
                .emptyText("время")
                .string();
            /** Конечная станция */
            this.ArrivalPlace = this.member()
                .localizeTitle({ ru: "Конечная станция", ruShort: "место" })
                .emptyText("место")
                .string();
            /** Дата прибытия */
            this.ArrivalDate = this.member()
                .localizeTitle({ ru: "Дата прибытия", ruShort: "дата" })
                .emptyText("дата")
                .date();
            /** Время прибытия */
            this.ArrivalTime = this.member()
                .localizeTitle({ ru: "Время прибытия", ruShort: "время" })
                .emptyText("время")
                .string();
            /** Номер места */
            this.SeatNumber = this.member()
                .localizeTitle({ ru: "Номер места" })
                .string();
            this._isAbstract = true;
            this._name = "BusDocument";
            this._names = "BusDocuments";
            this.icon("bus");
            this._isEntity = true;
            this._localizeTitle({ ru: "Автобусный билет или возврат", rus: "Автобусные билеты и возвраты" });
            this._getDerivedEntities = function () { return [
                Luxena.sd.BusTicket, Luxena.sd.BusTicketRefund
            ]; };
            this._getBaseEntity = function () { return Luxena.sd.Product; };
            this._className = "Product";
            this._getRootEntity = function () { return Luxena.sd.Product; };
            this._store = Luxena.db.BusDocuments;
            this._saveStore = Luxena.db.BusDocuments;
            this._lookupStore = Luxena.db.BusDocumentLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.big();
            this.Type
                .localizeTitle({ ru: "Вид услуги" })
                .enum(Luxena.ProductType, "BusTicket", "BusTicketRefund")
                .required()
                .length(12, 0, 0);
            this.RefundedProduct
                .lookup(function () { return Luxena.sd.BusTicket; });
            this.ReissueFor
                .lookup(function () { return Luxena.sd.BusTicket; });
            this.Provider
                .lookup(function () { return Luxena.sd.BusTicketProvider; });
        }
        BusDocumentSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return BusDocumentSemantic;
    })(ProductSemantic);
    Luxena.BusDocumentSemantic = BusDocumentSemantic;
    //00:00:00.1962569
    /** Аренда автомобиля */
    var CarRentalSemantic = (function (_super) {
        __extends(CarRentalSemantic, _super);
        function CarRentalSemantic() {
            _super.call(this);
            //00:00:00.1964703
            this._CarRental = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Аренда автомобиля", rus: "Аренды автомобилей", ua: "Оренда автомобіля" })
                .lookup(function () { return Luxena.sd.CarRental; });
            /** Марка авто */
            this.CarBrand = this.member()
                .localizeTitle({ ru: "Марка авто" })
                .string();
            this._isAbstract = false;
            this._name = "CarRental";
            this._names = "CarRentals";
            this.icon("car");
            this._isEntity = true;
            this._localizeTitle({ ru: "Аренда автомобиля", rus: "Аренды автомобилей", ua: "Оренда автомобіля" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Product; };
            this._className = "Product";
            this._getRootEntity = function () { return Luxena.sd.Product; };
            this._store = Luxena.db.CarRentals;
            this._saveStore = Luxena.db.CarRentals;
            this._lookupStore = Luxena.db.CarRentalLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.big();
            this.ReissueFor
                .lookup(function () { return Luxena.sd.CarRental; });
            this.Provider
                .lookup(function () { return Luxena.sd.CarRentalProvider; });
        }
        CarRentalSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return CarRentalSemantic;
    })(ProductSemantic);
    Luxena.CarRentalSemantic = CarRentalSemantic;
    //00:00:00.1995810
    /** ПКО */
    var CashInOrderPaymentSemantic = (function (_super) {
        __extends(CashInOrderPaymentSemantic, _super);
        function CashInOrderPaymentSemantic() {
            _super.call(this);
            //00:00:00.1997228
            this._CashInOrderPayment = new Luxena.SemanticMember()
                .localizeTitle({ ru: "ПКО" })
                .lookup(function () { return Luxena.sd.CashInOrderPayment; });
            this._isAbstract = false;
            this._name = "CashInOrderPayment";
            this._names = "CashInOrderPayments";
            this._isEntity = true;
            this._localizeTitle({ ru: "ПКО" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Payment; };
            this._className = "Payment";
            this._getRootEntity = function () { return Luxena.sd.Payment; };
            this._store = Luxena.db.CashInOrderPayments;
            this._saveStore = Luxena.db.CashInOrderPayments;
            this._lookupStore = Luxena.db.CashInOrderPaymentLookup;
            this._referenceFields = { id: "Id", name: "Number" };
            this.DocumentNumber
                .localizeTitle({ ru: "№ ПКО" });
        }
        CashInOrderPaymentSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return CashInOrderPaymentSemantic;
    })(PaymentSemantic);
    Luxena.CashInOrderPaymentSemantic = CashInOrderPaymentSemantic;
    //00:00:00.2025220
    /** РКО */
    var CashOutOrderPaymentSemantic = (function (_super) {
        __extends(CashOutOrderPaymentSemantic, _super);
        function CashOutOrderPaymentSemantic() {
            _super.call(this);
            //00:00:00.2026639
            this._CashOutOrderPayment = new Luxena.SemanticMember()
                .localizeTitle({ ru: "РКО" })
                .lookup(function () { return Luxena.sd.CashOutOrderPayment; });
            this._isAbstract = false;
            this._name = "CashOutOrderPayment";
            this._names = "CashOutOrderPayments";
            this._isEntity = true;
            this._localizeTitle({ ru: "РКО" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Payment; };
            this._className = "Payment";
            this._getRootEntity = function () { return Luxena.sd.Payment; };
            this._store = Luxena.db.CashOutOrderPayments;
            this._saveStore = Luxena.db.CashOutOrderPayments;
            this._lookupStore = Luxena.db.CashOutOrderPaymentLookup;
            this._referenceFields = { id: "Id", name: "Number" };
            this.DocumentNumber
                .localizeTitle({ ru: "№ РКО" });
        }
        CashOutOrderPaymentSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return CashOutOrderPaymentSemantic;
    })(PaymentSemantic);
    Luxena.CashOutOrderPaymentSemantic = CashOutOrderPaymentSemantic;
    //00:00:00.2055868
    /** Кассовый чек */
    var CheckPaymentSemantic = (function (_super) {
        __extends(CheckPaymentSemantic, _super);
        function CheckPaymentSemantic() {
            _super.call(this);
            //00:00:00.2057785
            this._CheckPayment = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Кассовый чек" })
                .lookup(function () { return Luxena.sd.CheckPayment; });
            this._isAbstract = false;
            this._name = "CheckPayment";
            this._names = "CheckPayments";
            this._isEntity = true;
            this._localizeTitle({ ru: "Кассовый чек" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Payment; };
            this._className = "Payment";
            this._getRootEntity = function () { return Luxena.sd.Payment; };
            this._store = Luxena.db.CheckPayments;
            this._saveStore = Luxena.db.CheckPayments;
            this._lookupStore = Luxena.db.CheckPaymentLookup;
            this._referenceFields = { id: "Id", name: "Number" };
            this.DocumentNumber
                .localizeTitle({ ru: "№ чека", ruShort: "автоматически" })
                .emptyText("автоматически");
        }
        CheckPaymentSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return CheckPaymentSemantic;
    })(PaymentSemantic);
    Luxena.CheckPaymentSemantic = CheckPaymentSemantic;
    //00:00:00.2069222
    /** Страна */
    var CountrySemantic = (function (_super) {
        __extends(CountrySemantic, _super);
        function CountrySemantic() {
            _super.call(this);
            //00:00:00.2070861
            this._Country = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Страна", rus: "Страны" })
                .lookup(function () { return Luxena.sd.Country; });
            /** Код (2-х сим.) */
            this.TwoCharCode = this.member()
                .localizeTitle({ ru: "Код (2-х сим.)" })
                .string(2, undefined, undefined);
            /** Код (3-х сим.) */
            this.ThreeCharCode = this.member()
                .localizeTitle({ ru: "Код (3-х сим.)" })
                .string(3, undefined, undefined);
            this.Airports = this.collection(function () { return Luxena.sd.Airport; }, function (se) { return se.Country; });
            this._isAbstract = false;
            this._name = "Country";
            this._names = "Countries";
            this.icon("globe");
            this._isEntity = true;
            this._localizeTitle({ ru: "Страна", rus: "Страны" });
            this._getDerivedEntities = null;
            this._className = "Country";
            this._getRootEntity = function () { return Luxena.sd.Country; };
            this._store = Luxena.db.Countries;
            this._saveStore = Luxena.db.Countries;
            this._lookupStore = Luxena.db.CountryLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.Name
                .length(16, 0, 0);
        }
        CountrySemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return CountrySemantic;
    })(Entity3Semantic);
    Luxena.CountrySemantic = CountrySemantic;
    //00:00:00.2134557
    /** Электронный платеж */
    var ElectronicPaymentSemantic = (function (_super) {
        __extends(ElectronicPaymentSemantic, _super);
        function ElectronicPaymentSemantic() {
            _super.call(this);
            //00:00:00.2136594
            this._ElectronicPayment = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Электронный платеж" })
                .lookup(function () { return Luxena.sd.ElectronicPayment; });
            /** Код авторизации */
            this.AuthorizationCode = this.member()
                .localizeTitle({ ru: "Код авторизации" })
                .string();
            this._isAbstract = false;
            this._name = "ElectronicPayment";
            this._names = "ElectronicPayments";
            this._isEntity = true;
            this._localizeTitle({ ru: "Электронный платеж" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Payment; };
            this._className = "Payment";
            this._getRootEntity = function () { return Luxena.sd.Payment; };
            this._store = Luxena.db.ElectronicPayments;
            this._saveStore = Luxena.db.ElectronicPayments;
            this._lookupStore = Luxena.db.ElectronicPaymentLookup;
            this._referenceFields = { id: "Id", name: "Number" };
            this.DocumentNumber
                .localizeTitle({ ru: "№ транзакции" });
        }
        ElectronicPaymentSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ElectronicPaymentSemantic;
    })(PaymentSemantic);
    Luxena.ElectronicPaymentSemantic = ElectronicPaymentSemantic;
    //00:00:00.2198874
    /** Экскурсия */
    var ExcursionSemantic = (function (_super) {
        __extends(ExcursionSemantic, _super);
        function ExcursionSemantic() {
            _super.call(this);
            //00:00:00.2200263
            this._Excursion = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Экскурсия", rus: "Экскурсии", ua: "Екскурсія" })
                .lookup(function () { return Luxena.sd.Excursion; });
            /** Название тура */
            this.TourName = this.member()
                .localizeTitle({ ru: "Название тура" })
                .string()
                .required();
            this._isAbstract = false;
            this._name = "Excursion";
            this._names = "Excursions";
            this.icon("photo");
            this._isEntity = true;
            this._localizeTitle({ ru: "Экскурсия", rus: "Экскурсии", ua: "Екскурсія" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Product; };
            this._className = "Product";
            this._getRootEntity = function () { return Luxena.sd.Product; };
            this._store = Luxena.db.Excursions;
            this._saveStore = Luxena.db.Excursions;
            this._lookupStore = Luxena.db.ExcursionLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.big();
            this.ReissueFor
                .lookup(function () { return Luxena.sd.Excursion; });
        }
        ExcursionSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ExcursionSemantic;
    })(ProductSemantic);
    Luxena.ExcursionSemantic = ExcursionSemantic;
    //00:00:00.2210967
    /** Gds-файл */
    var GdsFileSemantic = (function (_super) {
        __extends(GdsFileSemantic, _super);
        function GdsFileSemantic() {
            _super.call(this);
            //00:00:00.2211945
            this._GdsFile = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Gds-файл", rus: "Gds-файлы" })
                .lookup(function () { return Luxena.sd.GdsFile; });
            /** Тип */
            this.FileType = this.member()
                .localizeTitle({ ru: "Тип" })
                .enum(Luxena.GdsFileType)
                .required();
            /** Дата импорта */
            this.TimeStamp = this.member()
                .localizeTitle({ ru: "Дата импорта" })
                .dateTime2()
                .required()
                .entityDate();
            /** Содержимое */
            this.Content = this.member()
                .localizeTitle({ ru: "Содержимое" })
                .codeText(8);
            /** Результат импорта */
            this.ImportResult = this.member()
                .localizeTitle({ ru: "Результат импорта" })
                .enum(Luxena.ImportResult)
                .required();
            /** Журнал */
            this.ImportOutput = this.member()
                .localizeTitle({ ru: "Журнал" })
                .string();
            this.Products = this.collection(function () { return Luxena.sd.Product; }, function (se) { return se.OriginalDocument; });
            this._isAbstract = true;
            this._name = "GdsFile";
            this._names = "GdsFiles";
            this._isEntity = true;
            this._localizeTitle({ ru: "Gds-файл", rus: "Gds-файлы" });
            this._getDerivedEntities = null;
            this._className = "GdsFile";
            this._getRootEntity = function () { return Luxena.sd.GdsFile; };
            this._store = Luxena.db.GdsFiles;
            this._saveStore = Luxena.db.GdsFiles;
            this._lookupStore = Luxena.db.GdsFileLookup;
            this._referenceFields = { id: "Id", name: "Name" };
        }
        GdsFileSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return GdsFileSemantic;
    })(Entity3Semantic);
    Luxena.GdsFileSemantic = GdsFileSemantic;
    //00:00:00.2288551
    /** Дополнительная услуга */
    var GenericProductSemantic = (function (_super) {
        __extends(GenericProductSemantic, _super);
        function GenericProductSemantic() {
            _super.call(this);
            //00:00:00.2290667
            this._GenericProduct = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Дополнительная услуга", rus: "Дополнительные услуги", ua: "Додаткова послуга" })
                .lookup(function () { return Luxena.sd.GenericProduct; });
            /** Номер */
            this.Number = this.member()
                .localizeTitle({ ru: "Номер" })
                .string();
            /** Вид услуги */
            this.GenericType = this.member()
                .localizeTitle({ ru: "Вид услуги" })
                .lookup(function () { return Luxena.sd.GenericProductType; })
                .required();
            this._isAbstract = false;
            this._name = "GenericProduct";
            this._names = "GenericProducts";
            this.icon("suitcase");
            this._isEntity = true;
            this._localizeTitle({ ru: "Дополнительная услуга", rus: "Дополнительные услуги", ua: "Додаткова послуга" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Product; };
            this._className = "Product";
            this._getRootEntity = function () { return Luxena.sd.Product; };
            this._store = Luxena.db.GenericProducts;
            this._saveStore = Luxena.db.GenericProducts;
            this._lookupStore = Luxena.db.GenericProductLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.big();
            this.ReissueFor
                .lookup(function () { return Luxena.sd.GenericProduct; });
            this.Provider
                .lookup(function () { return Luxena.sd.GenericProductProvider; });
        }
        GenericProductSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return GenericProductSemantic;
    })(ProductSemantic);
    Luxena.GenericProductSemantic = GenericProductSemantic;
    //00:00:00.2298192
    /** Вид дополнительной услуги */
    var GenericProductTypeSemantic = (function (_super) {
        __extends(GenericProductTypeSemantic, _super);
        function GenericProductTypeSemantic() {
            _super.call(this);
            //00:00:00.2299246
            this._GenericProductType = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Вид дополнительной услуги", rus: "Виды дополнительных услуг" })
                .lookup(function () { return Luxena.sd.GenericProductType; });
            this._isAbstract = false;
            this._name = "GenericProductType";
            this._names = "GenericProductTypes";
            this._isEntity = true;
            this._localizeTitle({ ru: "Вид дополнительной услуги", rus: "Виды дополнительных услуг" });
            this._getDerivedEntities = null;
            this._className = "GenericProductType";
            this._getRootEntity = function () { return Luxena.sd.GenericProductType; };
            this._store = Luxena.db.GenericProductTypes;
            this._saveStore = Luxena.db.GenericProductTypes;
            this._lookupStore = Luxena.db.GenericProductTypeLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.small();
        }
        GenericProductTypeSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return GenericProductTypeSemantic;
    })(Entity3Semantic);
    Luxena.GenericProductTypeSemantic = GenericProductTypeSemantic;
    //00:00:00.2366156
    /** Страховка или возврат */
    var InsuranceDocumentSemantic = (function (_super) {
        __extends(InsuranceDocumentSemantic, _super);
        function InsuranceDocumentSemantic() {
            _super.call(this);
            //00:00:00.2369781
            this._InsuranceDocument = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Страховка или возврат", rus: "Страховки и возвраты" })
                .lookup(function () { return Luxena.sd.InsuranceDocument; });
            /** Номер */
            this.Number = this.member()
                .localizeTitle({ ru: "Номер" })
                .string()
                .required();
            this._isAbstract = true;
            this._name = "InsuranceDocument";
            this._names = "InsuranceDocuments";
            this.icon("fire-extinguisher");
            this._isEntity = true;
            this._localizeTitle({ ru: "Страховка или возврат", rus: "Страховки и возвраты" });
            this._getDerivedEntities = function () { return [
                Luxena.sd.InsuranceRefund, Luxena.sd.Insurance
            ]; };
            this._getBaseEntity = function () { return Luxena.sd.Product; };
            this._className = "Product";
            this._getRootEntity = function () { return Luxena.sd.Product; };
            this._store = Luxena.db.InsuranceDocuments;
            this._saveStore = Luxena.db.InsuranceDocuments;
            this._lookupStore = Luxena.db.InsuranceDocumentLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.big();
            this.Type
                .localizeTitle({ ru: "Вид услуги" })
                .enum(Luxena.ProductType, "Insurance", "InsuranceRefund")
                .required()
                .length(12, 0, 0);
            this.ReissueFor
                .lookup(function () { return Luxena.sd.Insurance; });
            this.Producer
                .localizeTitle({ ru: "Страховая компания" })
                .lookup(function () { return Luxena.sd.InsuranceCompany; })
                .required();
        }
        InsuranceDocumentSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return InsuranceDocumentSemantic;
    })(ProductSemantic);
    Luxena.InsuranceDocumentSemantic = InsuranceDocumentSemantic;
    //00:00:00.2436324
    /** Студенческий билет */
    var IsicSemantic = (function (_super) {
        __extends(IsicSemantic, _super);
        function IsicSemantic() {
            _super.call(this);
            //00:00:00.2437760
            this._Isic = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Студенческий билет", rus: "Студенческие билеты", ua: "Студентський квиток" })
                .lookup(function () { return Luxena.sd.Isic; });
            /** Тип карты */
            this.CardType = this.member()
                .localizeTitle({ ru: "Тип карты" })
                .enum(Luxena.IsicCardType)
                .required()
                .defaultValue(1);
            /** Номер */
            this.Number1 = this.member()
                .localizeTitle({ ru: "Номер" })
                .string(12)
                .required();
            /** Номер */
            this.Number2 = this.member()
                .localizeTitle({ ru: "Номер" })
                .string(1)
                .required();
            this._isAbstract = false;
            this._name = "Isic";
            this._names = "Isics";
            this.icon("graduation-cap");
            this._isEntity = true;
            this._localizeTitle({ ru: "Студенческий билет", rus: "Студенческие билеты", ua: "Студентський квиток" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Product; };
            this._className = "Product";
            this._getRootEntity = function () { return Luxena.sd.Product; };
            this._store = Luxena.db.Isics;
            this._saveStore = Luxena.db.Isics;
            this._lookupStore = Luxena.db.IsicLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.big();
            this.ReissueFor
                .lookup(function () { return Luxena.sd.Isic; });
        }
        IsicSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return IsicSemantic;
    })(ProductSemantic);
    Luxena.IsicSemantic = IsicSemantic;
    //00:00:00.2465181
    /** Контрагент */
    var PartySemantic = (function (_super) {
        __extends(PartySemantic, _super);
        function PartySemantic() {
            _super.call(this);
            //00:00:00.2467476
            this._Party = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Контрагент", rus: "Контрагенты" })
                .lookup(function () { return Luxena.sd.Party; });
            this.Type = this.member()
                .enum(Luxena.PartyType)
                .required()
                .entityType();
            /** Официальное название */
            this.LegalName = this.member()
                .localizeTitle({ ru: "Официальное название" })
                .string();
            /** Код */
            this.Code = this.member()
                .localizeTitle({ ru: "Код" })
                .string();
            this.NameForDocuments = this.member()
                .string()
                .calculated()
                .nonsaved();
            /** Телефон 1 */
            this.Phone1 = this.member()
                .localizeTitle({ ru: "Телефон 1" })
                .string();
            /** Телефон 2 */
            this.Phone2 = this.member()
                .localizeTitle({ ru: "Телефон 2" })
                .string();
            /** Факс */
            this.Fax = this.member()
                .localizeTitle({ ru: "Факс" })
                .string();
            /** E-mail 1 */
            this.Email1 = this.member()
                .localizeTitle({ ru: "E-mail 1" })
                .string();
            /** E-mail 2 */
            this.Email2 = this.member()
                .localizeTitle({ ru: "E-mail 2" })
                .string();
            /** Веб адрес */
            this.WebAddress = this.member()
                .localizeTitle({ ru: "Веб адрес" })
                .string();
            /** Заказчик */
            this.IsCustomer = this.member()
                .localizeTitle({ ru: "Заказчик" })
                .bool()
                .required();
            /** Поставщик */
            this.IsSupplier = this.member()
                .localizeTitle({ ru: "Поставщик" })
                .bool()
                .required();
            /** Дополнительная информация */
            this.Details = this.member()
                .localizeTitle({ ru: "Дополнительная информация" })
                .text(3);
            /** Юридический адрес */
            this.LegalAddress = this.member()
                .localizeTitle({ ru: "Юридический адрес" })
                .text(3);
            /** Фактический адрес */
            this.ActualAddress = this.member()
                .localizeTitle({ ru: "Фактический адрес" })
                .text(3);
            /** Примечание */
            this.Note = this.member()
                .localizeTitle({ ru: "Примечание" })
                .string();
            /** Подчиняется */
            this.ReportsTo = this.member()
                .localizeTitle({ ru: "Подчиняется" })
                .lookup(function () { return Luxena.sd.Party; });
            /** На банковский счёт по умолчанию */
            this.DefaultBankAccount = this.member()
                .localizeTitle({ ru: "На банковский счёт по умолчанию", ruDesc: "По умолчаничанию оплачивать через выбранный банковский счёт агенства" })
                .lookup(function () { return Luxena.sd.BankAccount; });
            this.Files = this.collection(function () { return Luxena.sd.File; }, function (se) { return se.Party; });
            this.DocumentOwners = this.collection(function () { return Luxena.sd.DocumentOwner; }, function (se) { return se.Owner; });
            this._isAbstract = true;
            this._name = "Party";
            this._names = "Parties";
            this._isEntity = true;
            this._localizeTitle({ ru: "Контрагент", rus: "Контрагенты" });
            this._getDerivedEntities = function () { return [
                Luxena.sd.Airline, Luxena.sd.Agent, Luxena.sd.ActiveOwner, Luxena.sd.Customer, Luxena.sd.RoamingOperator, Luxena.sd.Organization, Luxena.sd.Person, Luxena.sd.Department, Luxena.sd.BusTicketProvider, Luxena.sd.CarRentalProvider, Luxena.sd.GenericProductProvider, Luxena.sd.PasteboardProvider, Luxena.sd.AccommodationProvider, Luxena.sd.TransferProvider, Luxena.sd.TourProvider, Luxena.sd.InsuranceCompany
            ]; };
            this._className = "Party";
            this._getRootEntity = function () { return Luxena.sd.Party; };
            this._store = Luxena.db.Parties;
            this._saveStore = Luxena.db.Parties;
            this._lookupStore = Luxena.db.PartyLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.Name
                .length(20, 0, 0);
        }
        PartySemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return PartySemantic;
    })(Entity3Semantic);
    Luxena.PartySemantic = PartySemantic;
    //00:00:00.2485466
    /** Платёжная система */
    var PaymentSystemSemantic = (function (_super) {
        __extends(PaymentSystemSemantic, _super);
        function PaymentSystemSemantic() {
            _super.call(this);
            //00:00:00.2486496
            this._PaymentSystem = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Платёжная система", rus: "Платёжные системы" })
                .lookup(function () { return Luxena.sd.PaymentSystem; });
            this._isAbstract = false;
            this._name = "PaymentSystem";
            this._names = "PaymentSystems";
            this._isEntity = true;
            this._localizeTitle({ ru: "Платёжная система", rus: "Платёжные системы" });
            this._getDerivedEntities = null;
            this._className = "PaymentSystem";
            this._getRootEntity = function () { return Luxena.sd.PaymentSystem; };
            this._store = Luxena.db.PaymentSystems;
            this._saveStore = Luxena.db.PaymentSystems;
            this._lookupStore = Luxena.db.PaymentSystemLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.small();
        }
        PaymentSystemSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return PaymentSystemSemantic;
    })(Entity3Semantic);
    Luxena.PaymentSystemSemantic = PaymentSystemSemantic;
    //00:00:00.2563144
    /** Ж/д билет или возврат */
    var RailwayDocumentSemantic = (function (_super) {
        __extends(RailwayDocumentSemantic, _super);
        function RailwayDocumentSemantic() {
            _super.call(this);
            //00:00:00.2598552
            this._RailwayDocument = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Ж/д билет или возврат", rus: "Ж/д билеты и возвраты" })
                .lookup(function () { return Luxena.sd.RailwayDocument; });
            /** Номер */
            this.Number = this.member()
                .localizeTitle({ ru: "Номер" })
                .string();
            /** Начальная станция */
            this.DeparturePlace = this.member()
                .localizeTitle({ ru: "Начальная станция", ruShort: "место" })
                .emptyText("место")
                .string()
                .length(20, 0, 0);
            /** Дата отправления */
            this.DepartureDate = this.member()
                .localizeTitle({ ru: "Дата отправления", ruShort: "дата" })
                .emptyText("дата")
                .date();
            /** Время отправления */
            this.DepartureTime = this.member()
                .localizeTitle({ ru: "Время отправления", ruShort: "время" })
                .emptyText("время")
                .string();
            /** Конечная станция */
            this.ArrivalPlace = this.member()
                .localizeTitle({ ru: "Конечная станция", ruShort: "место" })
                .emptyText("место")
                .string()
                .length(20, 0, 0);
            /** Дата прибытия */
            this.ArrivalDate = this.member()
                .localizeTitle({ ru: "Дата прибытия", ruShort: "дата" })
                .emptyText("дата")
                .date();
            /** Время прибытия */
            this.ArrivalTime = this.member()
                .localizeTitle({ ru: "Время прибытия", ruShort: "время" })
                .emptyText("время")
                .string();
            /** Номер поезда */
            this.TrainNumber = this.member()
                .localizeTitle({ ru: "Номер поезда" })
                .string();
            /** Номер вагона */
            this.CarNumber = this.member()
                .localizeTitle({ ru: "Номер вагона" })
                .string();
            /** Номер места */
            this.SeatNumber = this.member()
                .localizeTitle({ ru: "Номер места" })
                .string();
            /** Сервис-класс */
            this.ServiceClass = this.member()
                .localizeTitle({ ru: "Сервис-класс" })
                .enum(Luxena.PasteboardServiceClass)
                .required()
                .defaultValue(0);
            this._isAbstract = true;
            this._name = "RailwayDocument";
            this._names = "RailwayDocuments";
            this.icon("subway");
            this._isEntity = true;
            this._localizeTitle({ ru: "Ж/д билет или возврат", rus: "Ж/д билеты и возвраты" });
            this._getDerivedEntities = function () { return [
                Luxena.sd.PasteboardRefund, Luxena.sd.Pasteboard
            ]; };
            this._getBaseEntity = function () { return Luxena.sd.Product; };
            this._className = "Product";
            this._getRootEntity = function () { return Luxena.sd.Product; };
            this._store = Luxena.db.RailwayDocuments;
            this._saveStore = Luxena.db.RailwayDocuments;
            this._lookupStore = Luxena.db.RailwayDocumentLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.big();
            this.Type
                .localizeTitle({ ru: "Вид услуги" })
                .enum(Luxena.ProductType, "Pasteboard", "PasteboardRefund")
                .required()
                .length(12, 0, 0);
            this.Number
                .length(24, 0, 0);
            this.Provider
                .lookup(function () { return Luxena.sd.PasteboardProvider; });
            this.RefundedProduct
                .lookup(function () { return Luxena.sd.Pasteboard; });
            this.ReissueFor
                .lookup(function () { return Luxena.sd.Pasteboard; });
        }
        RailwayDocumentSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return RailwayDocumentSemantic;
    })(ProductSemantic);
    Luxena.RailwayDocumentSemantic = RailwayDocumentSemantic;
    //00:00:00.2670477
    /** SIM-карта */
    var SimCardSemantic = (function (_super) {
        __extends(SimCardSemantic, _super);
        function SimCardSemantic() {
            _super.call(this);
            //00:00:00.2673006
            this._SimCard = new Luxena.SemanticMember()
                .localizeTitle({ ru: "SIM-карта", rus: "SIM-карты", ua: "SIM-картка" })
                .lookup(function () { return Luxena.sd.SimCard; });
            /** Номер */
            this.Number = this.member()
                .localizeTitle({ ru: "Номер" })
                .string(16)
                .required();
            /** Продажа SIM-карты */
            this.IsSale = this.member()
                .localizeTitle({ ru: "Продажа SIM-карты" })
                .bool()
                .required();
            this._isAbstract = false;
            this._name = "SimCard";
            this._names = "SimCards";
            this.icon("mobile");
            this._isEntity = true;
            this._localizeTitle({ ru: "SIM-карта", rus: "SIM-карты", ua: "SIM-картка" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Product; };
            this._className = "Product";
            this._getRootEntity = function () { return Luxena.sd.Product; };
            this._store = Luxena.db.SimCards;
            this._saveStore = Luxena.db.SimCards;
            this._lookupStore = Luxena.db.SimCardLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.big();
            this.ReissueFor
                .lookup(function () { return Luxena.sd.SimCard; });
            this.Producer
                .localizeTitle({ ru: "Оператор" })
                .lookup(function () { return Luxena.sd.RoamingOperator; })
                .required();
        }
        SimCardSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return SimCardSemantic;
    })(ProductSemantic);
    Luxena.SimCardSemantic = SimCardSemantic;
    //00:00:00.2745637
    /** Турпакет */
    var TourSemantic = (function (_super) {
        __extends(TourSemantic, _super);
        function TourSemantic() {
            _super.call(this);
            //00:00:00.2747647
            this._Tour = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Турпакет", rus: "Турпакеты", ua: "Турпакет" })
                .lookup(function () { return Luxena.sd.Tour; });
            /** Гостиница */
            this.HotelName = this.member()
                .localizeTitle({ ru: "Гостиница" })
                .string();
            /** Офис гостиницы */
            this.HotelOffice = this.member()
                .localizeTitle({ ru: "Офис гостиницы", ruShort: "офис" })
                .emptyText("офис")
                .string();
            /** Код гостиницы */
            this.HotelCode = this.member()
                .localizeTitle({ ru: "Код гостиницы", ruShort: "код" })
                .emptyText("код")
                .string();
            /** Расположение */
            this.PlacementName = this.member()
                .localizeTitle({ ru: "Расположение" })
                .string();
            this.PlacementOffice = this.member()
                .emptyText("офис")
                .string();
            this.PlacementCode = this.member()
                .emptyText("код")
                .string();
            /** Авиа (описание) */
            this.AviaDescription = this.member()
                .localizeTitle({ ru: "Авиа (описание)" })
                .string();
            /** Трансфер (описание) */
            this.TransferDescription = this.member()
                .localizeTitle({ ru: "Трансфер (описание)" })
                .string();
            /** Тип проживания */
            this.AccommodationType = this.member()
                .localizeTitle({ ru: "Тип проживания", rus: "Типы проживания" })
                .lookup(function () { return Luxena.sd.AccommodationType; });
            /** Тип питания */
            this.CateringType = this.member()
                .localizeTitle({ ru: "Тип питания", rus: "Типы питания" })
                .lookup(function () { return Luxena.sd.CateringType; });
            this._isAbstract = false;
            this._name = "Tour";
            this._names = "Tours";
            this.icon("suitcase");
            this._isEntity = true;
            this._localizeTitle({ ru: "Турпакет", rus: "Турпакеты", ua: "Турпакет" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Product; };
            this._className = "Product";
            this._getRootEntity = function () { return Luxena.sd.Product; };
            this._store = Luxena.db.Tours;
            this._saveStore = Luxena.db.Tours;
            this._lookupStore = Luxena.db.TourLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.big();
            this.ReissueFor
                .lookup(function () { return Luxena.sd.Tour; });
            this.Provider
                .lookup(function () { return Luxena.sd.TourProvider; });
        }
        TourSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return TourSemantic;
    })(ProductSemantic);
    Luxena.TourSemantic = TourSemantic;
    //00:00:00.2815508
    /** Трансфер */
    var TransferSemantic = (function (_super) {
        __extends(TransferSemantic, _super);
        function TransferSemantic() {
            _super.call(this);
            //00:00:00.2817431
            this._Transfer = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Трансфер", rus: "Трансферы", ua: "Трансфер" })
                .lookup(function () { return Luxena.sd.Transfer; });
            this._isAbstract = false;
            this._name = "Transfer";
            this._names = "Transfers";
            this.icon("cab");
            this._isEntity = true;
            this._localizeTitle({ ru: "Трансфер", rus: "Трансферы", ua: "Трансфер" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Product; };
            this._className = "Product";
            this._getRootEntity = function () { return Luxena.sd.Product; };
            this._store = Luxena.db.Transfers;
            this._saveStore = Luxena.db.Transfers;
            this._lookupStore = Luxena.db.TransferLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.big();
            this.ReissueFor
                .lookup(function () { return Luxena.sd.Transfer; });
            this.Provider
                .lookup(function () { return Luxena.sd.TransferProvider; });
        }
        TransferSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return TransferSemantic;
    })(ProductSemantic);
    Luxena.TransferSemantic = TransferSemantic;
    //00:00:00.2846796
    /** Безналичный платеж */
    var WireTransferSemantic = (function (_super) {
        __extends(WireTransferSemantic, _super);
        function WireTransferSemantic() {
            _super.call(this);
            //00:00:00.2848921
            this._WireTransfer = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Безналичный платеж" })
                .lookup(function () { return Luxena.sd.WireTransfer; });
            this._isAbstract = false;
            this._name = "WireTransfer";
            this._names = "WireTransfers";
            this._isEntity = true;
            this._localizeTitle({ ru: "Безналичный платеж" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Payment; };
            this._className = "Payment";
            this._getRootEntity = function () { return Luxena.sd.Payment; };
            this._store = Luxena.db.WireTransfers;
            this._saveStore = Luxena.db.WireTransfers;
            this._lookupStore = Luxena.db.WireTransferLookup;
            this._referenceFields = { id: "Id", name: "Number" };
            this.DocumentNumber
                .localizeTitle({ ru: "№ платежного поручения" });
            this.Invoice
                .localizeTitle({ ru: "Счёт" })
                .lookup(function () { return Luxena.sd.Invoice; });
        }
        WireTransferSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return WireTransferSemantic;
    })(PaymentSemantic);
    Luxena.WireTransferSemantic = WireTransferSemantic;
    //00:00:00.2855632
    /** Тип проживания */
    var AccommodationTypeSemantic = (function (_super) {
        __extends(AccommodationTypeSemantic, _super);
        function AccommodationTypeSemantic() {
            _super.call(this);
            //00:00:00.2856679
            this._AccommodationType = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Тип проживания", rus: "Типы проживания" })
                .lookup(function () { return Luxena.sd.AccommodationType; });
            this._isAbstract = false;
            this._name = "AccommodationType";
            this._names = "AccommodationTypes";
            this._isEntity = true;
            this._localizeTitle({ ru: "Тип проживания", rus: "Типы проживания" });
            this._getDerivedEntities = null;
            this._className = "AccommodationType";
            this._getRootEntity = function () { return Luxena.sd.AccommodationType; };
            this._store = Luxena.db.AccommodationTypes;
            this._saveStore = Luxena.db.AccommodationTypes;
            this._lookupStore = Luxena.db.AccommodationTypeLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.small();
        }
        AccommodationTypeSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return AccommodationTypeSemantic;
    })(Entity3DSemantic);
    Luxena.AccommodationTypeSemantic = AccommodationTypeSemantic;
    //00:00:00.2928691
    /** МСО */
    var AviaMcoSemantic = (function (_super) {
        __extends(AviaMcoSemantic, _super);
        function AviaMcoSemantic() {
            _super.call(this);
            //00:00:00.2929597
            this._AviaMco = new Luxena.SemanticMember()
                .localizeTitle({ ru: "МСО", rus: "МСО", ua: "MCO" })
                .lookup(function () { return Luxena.sd.AviaMco; });
            /** Описание */
            this.Description = this.member()
                .localizeTitle({ ru: "Описание" })
                .text(3);
            /** Связан с */
            this.InConnectionWith = this.member()
                .localizeTitle({ ru: "Связан с" })
                .lookup(function () { return Luxena.sd.AviaDocument; });
            this._isAbstract = false;
            this._name = "AviaMco";
            this._names = "AviaMcos";
            this.icon("plane");
            this._isEntity = true;
            this._localizeTitle({ ru: "МСО", rus: "МСО", ua: "MCO" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.AviaDocument; };
            this._className = "Product";
            this._getRootEntity = function () { return Luxena.sd.Product; };
            this._store = Luxena.db.AviaMcos;
            this._saveStore = Luxena.db.AviaMcos;
            this._lookupStore = Luxena.db.AviaMcoLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.big();
        }
        AviaMcoSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return AviaMcoSemantic;
    })(AviaDocumentSemantic);
    Luxena.AviaMcoSemantic = AviaMcoSemantic;
    //00:00:00.2997201
    /** Возврат авиабилета */
    var AviaRefundSemantic = (function (_super) {
        __extends(AviaRefundSemantic, _super);
        function AviaRefundSemantic() {
            _super.call(this);
            //00:00:00.2998046
            this._AviaRefund = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Возврат авиабилета", rus: "Возвраты авиабилетов", ua: "Повернення авіаквитка" })
                .lookup(function () { return Luxena.sd.AviaRefund; });
            /** Исходный документ */
            this.RefundedDocument = this.member()
                .localizeTitle({ ru: "Исходный документ" })
                .lookup(function () { return Luxena.sd.AviaDocument; })
                .calculated()
                .nonsaved();
            this._isAbstract = false;
            this._name = "AviaRefund";
            this._names = "AviaRefunds";
            this.icon("plane");
            this._isEntity = true;
            this._localizeTitle({ ru: "Возврат авиабилета", rus: "Возвраты авиабилетов", ua: "Повернення авіаквитка" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.AviaDocument; };
            this._className = "Product";
            this._getRootEntity = function () { return Luxena.sd.Product; };
            this._store = Luxena.db.AviaRefunds;
            this._saveStore = Luxena.db.AviaRefunds;
            this._lookupStore = Luxena.db.AviaRefundLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.big();
        }
        AviaRefundSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return AviaRefundSemantic;
    })(AviaDocumentSemantic);
    Luxena.AviaRefundSemantic = AviaRefundSemantic;
    //00:00:00.3099792
    /** Авиабилет */
    var AviaTicketSemantic = (function (_super) {
        __extends(AviaTicketSemantic, _super);
        function AviaTicketSemantic() {
            _super.call(this);
            //00:00:00.3101086
            this._AviaTicket = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Авиабилет", rus: "Авиабилеты", ua: "Авіаквиток" })
                .lookup(function () { return Luxena.sd.AviaTicket; });
            /** Дата отправления */
            this.Departure = this.member()
                .localizeTitle({ ru: "Дата отправления", ruShort: "дата" })
                .emptyText("дата")
                .date();
            this.Domestic = this.member()
                .bool()
                .required();
            this.Interline = this.member()
                .bool()
                .required();
            /** Классы сегментов */
            this.SegmentClasses = this.member()
                .localizeTitle({ ru: "Классы сегментов" })
                .string();
            this.Endorsement = this.member()
                .string();
            this.FareTotal = this.member()
                .money();
            this.Segments = this.collection(function () { return Luxena.sd.FlightSegment; }, function (se) { return se.Ticket; });
            this._isAbstract = false;
            this._name = "AviaTicket";
            this._names = "AviaTickets";
            this.icon("plane");
            this._isEntity = true;
            this._localizeTitle({ ru: "Авиабилет", rus: "Авиабилеты", ua: "Авіаквиток" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.AviaDocument; };
            this._className = "Product";
            this._getRootEntity = function () { return Luxena.sd.Product; };
            this._store = Luxena.db.AviaTickets;
            this._saveStore = Luxena.db.AviaTickets;
            this._lookupStore = Luxena.db.AviaTicketLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.big();
        }
        AviaTicketSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return AviaTicketSemantic;
    })(AviaDocumentSemantic);
    Luxena.AviaTicketSemantic = AviaTicketSemantic;
    //00:00:00.3112669
    /** Банковский счёт */
    var BankAccountSemantic = (function (_super) {
        __extends(BankAccountSemantic, _super);
        function BankAccountSemantic() {
            _super.call(this);
            //00:00:00.3113716
            this._BankAccount = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Банковский счёт", rus: "Банковские счёта" })
                .lookup(function () { return Luxena.sd.BankAccount; });
            /** Использовать по умолчанию */
            this.IsDefault = this.member()
                .localizeTitle({ ru: "Использовать по умолчанию" })
                .bool()
                .required();
            /** Примечание */
            this.Note = this.member()
                .localizeTitle({ ru: "Примечание" })
                .string();
            this._isAbstract = false;
            this._name = "BankAccount";
            this._names = "BankAccounts";
            this._isEntity = true;
            this._localizeTitle({ ru: "Банковский счёт", rus: "Банковские счёта" });
            this._getDerivedEntities = null;
            this._className = "BankAccount";
            this._getRootEntity = function () { return Luxena.sd.BankAccount; };
            this._store = Luxena.db.BankAccounts;
            this._saveStore = Luxena.db.BankAccounts;
            this._lookupStore = Luxena.db.BankAccountLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.small();
        }
        BankAccountSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return BankAccountSemantic;
    })(Entity3DSemantic);
    Luxena.BankAccountSemantic = BankAccountSemantic;
    //00:00:00.3179712
    /** Автобусный билет */
    var BusTicketSemantic = (function (_super) {
        __extends(BusTicketSemantic, _super);
        function BusTicketSemantic() {
            _super.call(this);
            //00:00:00.3180587
            this._BusTicket = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Автобусный билет", rus: "Автобусные билеты", ua: "Автобусний квиток" })
                .lookup(function () { return Luxena.sd.BusTicket; });
            this._isAbstract = false;
            this._name = "BusTicket";
            this._names = "BusTickets";
            this.icon("bus");
            this._isEntity = true;
            this._localizeTitle({ ru: "Автобусный билет", rus: "Автобусные билеты", ua: "Автобусний квиток" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.BusDocument; };
            this._className = "Product";
            this._getRootEntity = function () { return Luxena.sd.Product; };
            this._store = Luxena.db.BusTickets;
            this._saveStore = Luxena.db.BusTickets;
            this._lookupStore = Luxena.db.BusTicketLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.big();
        }
        BusTicketSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return BusTicketSemantic;
    })(BusDocumentSemantic);
    Luxena.BusTicketSemantic = BusTicketSemantic;
    //00:00:00.3245973
    /** Возврат автобусного билета */
    var BusTicketRefundSemantic = (function (_super) {
        __extends(BusTicketRefundSemantic, _super);
        function BusTicketRefundSemantic() {
            _super.call(this);
            //00:00:00.3246870
            this._BusTicketRefund = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Возврат автобусного билета", rus: "Возвраты автобусных билетов", ua: "Повернення автобусного квитка" })
                .lookup(function () { return Luxena.sd.BusTicketRefund; });
            this._isAbstract = false;
            this._name = "BusTicketRefund";
            this._names = "BusTicketRefunds";
            this.icon("bus");
            this._isEntity = true;
            this._localizeTitle({ ru: "Возврат автобусного билета", rus: "Возвраты автобусных билетов", ua: "Повернення автобусного квитка" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.BusDocument; };
            this._className = "Product";
            this._getRootEntity = function () { return Luxena.sd.Product; };
            this._store = Luxena.db.BusTicketRefunds;
            this._saveStore = Luxena.db.BusTicketRefunds;
            this._lookupStore = Luxena.db.BusTicketRefundLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.big();
        }
        BusTicketRefundSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return BusTicketRefundSemantic;
    })(BusDocumentSemantic);
    Luxena.BusTicketRefundSemantic = BusTicketRefundSemantic;
    //00:00:00.3253867
    /** Тип питания */
    var CateringTypeSemantic = (function (_super) {
        __extends(CateringTypeSemantic, _super);
        function CateringTypeSemantic() {
            _super.call(this);
            //00:00:00.3254924
            this._CateringType = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Тип питания", rus: "Типы питания" })
                .lookup(function () { return Luxena.sd.CateringType; });
            this._isAbstract = false;
            this._name = "CateringType";
            this._names = "CateringTypes";
            this._isEntity = true;
            this._localizeTitle({ ru: "Тип питания", rus: "Типы питания" });
            this._getDerivedEntities = null;
            this._className = "CateringType";
            this._getRootEntity = function () { return Luxena.sd.CateringType; };
            this._store = Luxena.db.CateringTypes;
            this._saveStore = Luxena.db.CateringTypes;
            this._lookupStore = Luxena.db.CateringTypeLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.small();
        }
        CateringTypeSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return CateringTypeSemantic;
    })(Entity3DSemantic);
    Luxena.CateringTypeSemantic = CateringTypeSemantic;
    //00:00:00.3275613
    /** Подразделение */
    var DepartmentSemantic = (function (_super) {
        __extends(DepartmentSemantic, _super);
        function DepartmentSemantic() {
            _super.call(this);
            //00:00:00.3276401
            this._Department = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Подразделение", rus: "Подразделения" })
                .lookup(function () { return Luxena.sd.Department; });
            /** Организация */
            this.Organization = this.member()
                .localizeTitle({ ru: "Организация", rus: "Организации" })
                .lookup(function () { return Luxena.sd.Organization; });
            this._isAbstract = false;
            this._name = "Department";
            this._names = "Departments";
            this._isEntity = true;
            this._localizeTitle({ ru: "Подразделение", rus: "Подразделения" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Party; };
            this._className = "Party";
            this._getRootEntity = function () { return Luxena.sd.Party; };
            this._store = Luxena.db.Departments;
            this._saveStore = Luxena.db.Departments;
            this._lookupStore = Luxena.db.DepartmentLookup;
            this._referenceFields = { id: "Id", name: "Name" };
        }
        DepartmentSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return DepartmentSemantic;
    })(PartySemantic);
    Luxena.DepartmentSemantic = DepartmentSemantic;
    //00:00:00.3284008
    var IdentitySemantic = (function (_super) {
        __extends(IdentitySemantic, _super);
        function IdentitySemantic() {
            _super.call(this);
            //00:00:00.3284944
            this._Identity = new Luxena.SemanticMember()
                .lookup(function () { return Luxena.sd.Identity; });
            this._isAbstract = true;
            this._name = "Identity";
            this._names = "Identities";
            this._isEntity = true;
            this._getDerivedEntities = function () { return [
                Luxena.sd.InternalIdentity, Luxena.sd.User
            ]; };
            this._className = "Identity";
            this._getRootEntity = function () { return Luxena.sd.Identity; };
            this._store = Luxena.db.Identities;
            this._saveStore = Luxena.db.Identities;
            this._lookupStore = Luxena.db.IdentityLookup;
            this._referenceFields = { id: "Id", name: "Name" };
        }
        IdentitySemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return IdentitySemantic;
    })(Entity3DSemantic);
    Luxena.IdentitySemantic = IdentitySemantic;
    //00:00:00.3345995
    /** Страховка */
    var InsuranceSemantic = (function (_super) {
        __extends(InsuranceSemantic, _super);
        function InsuranceSemantic() {
            _super.call(this);
            //00:00:00.3346876
            this._Insurance = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Страховка", rus: "Страховки", ua: "Страховка" })
                .lookup(function () { return Luxena.sd.Insurance; });
            this._isAbstract = false;
            this._name = "Insurance";
            this._names = "Insurances";
            this.icon("fire-extinguisher");
            this._isEntity = true;
            this._localizeTitle({ ru: "Страховка", rus: "Страховки", ua: "Страховка" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.InsuranceDocument; };
            this._className = "Product";
            this._getRootEntity = function () { return Luxena.sd.Product; };
            this._store = Luxena.db.Insurances;
            this._saveStore = Luxena.db.Insurances;
            this._lookupStore = Luxena.db.InsuranceLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.big();
        }
        InsuranceSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return InsuranceSemantic;
    })(InsuranceDocumentSemantic);
    Luxena.InsuranceSemantic = InsuranceSemantic;
    //00:00:00.3407423
    /** Возврат страховки */
    var InsuranceRefundSemantic = (function (_super) {
        __extends(InsuranceRefundSemantic, _super);
        function InsuranceRefundSemantic() {
            _super.call(this);
            //00:00:00.3408317
            this._InsuranceRefund = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Возврат страховки", rus: "Возвраты страховок", ua: "Повернення страховки" })
                .lookup(function () { return Luxena.sd.InsuranceRefund; });
            this._isAbstract = false;
            this._name = "InsuranceRefund";
            this._names = "InsuranceRefunds";
            this.icon("fire-extinguisher");
            this._isEntity = true;
            this._localizeTitle({ ru: "Возврат страховки", rus: "Возвраты страховок", ua: "Повернення страховки" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.InsuranceDocument; };
            this._className = "Product";
            this._getRootEntity = function () { return Luxena.sd.Product; };
            this._store = Luxena.db.InsuranceRefunds;
            this._saveStore = Luxena.db.InsuranceRefunds;
            this._lookupStore = Luxena.db.InsuranceRefundLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.big();
        }
        InsuranceRefundSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return InsuranceRefundSemantic;
    })(InsuranceDocumentSemantic);
    Luxena.InsuranceRefundSemantic = InsuranceRefundSemantic;
    //00:00:00.3443429
    /** Организация */
    var OrganizationSemantic = (function (_super) {
        __extends(OrganizationSemantic, _super);
        function OrganizationSemantic() {
            _super.call(this);
            //00:00:00.3445307
            this._Organization = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Организация", rus: "Организации" })
                .lookup(function () { return Luxena.sd.Organization; });
            /** Данная организация является Авиакомпанией */
            this.IsAirline = this.member()
                .localizeTitle({ ru: "Данная организация является Авиакомпанией" })
                .bool()
                .required();
            /** IATA код */
            this.AirlineIataCode = this.member()
                .localizeTitle({ ru: "IATA код" })
                .string(2);
            /** Prefix код */
            this.AirlinePrefixCode = this.member()
                .localizeTitle({ ru: "Prefix код" })
                .string(3);
            /** Требование паспортных данных */
            this.AirlinePassportRequirement = this.member()
                .localizeTitle({ ru: "Требование паспортных данных" })
                .enum(Luxena.AirlinePassportRequirement)
                .required();
            /** Для проживания */
            this.IsAccommodationProvider = this.member()
                .localizeTitle({ ru: "Для проживания" })
                .bool()
                .required();
            /** Для автобусных билетов */
            this.IsBusTicketProvider = this.member()
                .localizeTitle({ ru: "Для автобусных билетов" })
                .bool()
                .required();
            /** Для аренды авто */
            this.IsCarRentalProvider = this.member()
                .localizeTitle({ ru: "Для аренды авто" })
                .bool()
                .required();
            /** Для ж/д билетов */
            this.IsPasteboardProvider = this.member()
                .localizeTitle({ ru: "Для ж/д билетов" })
                .bool()
                .required();
            /** Для туров (готовых) */
            this.IsTourProvider = this.member()
                .localizeTitle({ ru: "Для туров (готовых)" })
                .bool()
                .required();
            /** Для трансферов */
            this.IsTransferProvider = this.member()
                .localizeTitle({ ru: "Для трансферов" })
                .bool()
                .required();
            /** Для дополнительных услуг */
            this.IsGenericProductProvider = this.member()
                .localizeTitle({ ru: "Для дополнительных услуг" })
                .bool()
                .required();
            /** Данная организация является Провайдером услуг */
            this.IsProvider = this.member()
                .localizeTitle({ ru: "Данная организация является Провайдером услуг" })
                .bool()
                .required();
            /** Данная организация является Страховой компанией */
            this.IsInsuranceCompany = this.member()
                .localizeTitle({ ru: "Данная организация является Страховой компанией" })
                .bool()
                .required();
            /** Данная организация является Роуминг-оператором */
            this.IsRoamingOperator = this.member()
                .localizeTitle({ ru: "Данная организация является Роуминг-оператором" })
                .bool()
                .required();
            this.MilesCards = this.collection(function () { return Luxena.sd.MilesCard; }, function (se) { return se.Organization; });
            this.Departments = this.collection(function () { return Luxena.sd.Department; }, function (se) { return se.Organization; });
            this.Persons = this.collection(function () { return Luxena.sd.Person; }, function (se) { return se.Organization; });
            this.AirlineServiceClasses = this.collection(function () { return Luxena.sd.AirlineServiceClass; }, function (se) { return se.Airline; });
            this._isAbstract = false;
            this._name = "Organization";
            this._names = "Organizations";
            this.icon("group");
            this._isEntity = true;
            this._localizeTitle({ ru: "Организация", rus: "Организации" });
            this._getDerivedEntities = function () { return [
                Luxena.sd.Airline, Luxena.sd.RoamingOperator, Luxena.sd.BusTicketProvider, Luxena.sd.CarRentalProvider, Luxena.sd.GenericProductProvider, Luxena.sd.PasteboardProvider, Luxena.sd.AccommodationProvider, Luxena.sd.TransferProvider, Luxena.sd.TourProvider, Luxena.sd.InsuranceCompany
            ]; };
            this._getBaseEntity = function () { return Luxena.sd.Party; };
            this._className = "Party";
            this._getRootEntity = function () { return Luxena.sd.Party; };
            this._store = Luxena.db.Organizations;
            this._saveStore = Luxena.db.Organizations;
            this._lookupStore = Luxena.db.OrganizationLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.Code
                .localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
        }
        OrganizationSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return OrganizationSemantic;
    })(PartySemantic);
    Luxena.OrganizationSemantic = OrganizationSemantic;
    //00:00:00.3579964
    /** Ж/д билет */
    var PasteboardSemantic = (function (_super) {
        __extends(PasteboardSemantic, _super);
        function PasteboardSemantic() {
            _super.call(this);
            //00:00:00.3581512
            this._Pasteboard = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Ж/д билет", rus: "Ж/д билеты", ua: "Залізничний квиток" })
                .lookup(function () { return Luxena.sd.Pasteboard; });
            this._isAbstract = false;
            this._name = "Pasteboard";
            this._names = "Pasteboards";
            this.icon("subway");
            this._isEntity = true;
            this._localizeTitle({ ru: "Ж/д билет", rus: "Ж/д билеты", ua: "Залізничний квиток" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.RailwayDocument; };
            this._className = "Product";
            this._getRootEntity = function () { return Luxena.sd.Product; };
            this._store = Luxena.db.Pasteboards;
            this._saveStore = Luxena.db.Pasteboards;
            this._lookupStore = Luxena.db.PasteboardLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.big();
        }
        PasteboardSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return PasteboardSemantic;
    })(RailwayDocumentSemantic);
    Luxena.PasteboardSemantic = PasteboardSemantic;
    //00:00:00.3649072
    /** Возврат ж/д билета */
    var PasteboardRefundSemantic = (function (_super) {
        __extends(PasteboardRefundSemantic, _super);
        function PasteboardRefundSemantic() {
            _super.call(this);
            //00:00:00.3650077
            this._PasteboardRefund = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Возврат ж/д билета", rus: "Возвраты ж/д билетов", ua: "Повернення залізничного квитка" })
                .lookup(function () { return Luxena.sd.PasteboardRefund; });
            this._isAbstract = false;
            this._name = "PasteboardRefund";
            this._names = "PasteboardRefunds";
            this.icon("subway");
            this._isEntity = true;
            this._localizeTitle({ ru: "Возврат ж/д билета", rus: "Возвраты ж/д билетов", ua: "Повернення залізничного квитка" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.RailwayDocument; };
            this._className = "Product";
            this._getRootEntity = function () { return Luxena.sd.Product; };
            this._store = Luxena.db.PasteboardRefunds;
            this._saveStore = Luxena.db.PasteboardRefunds;
            this._lookupStore = Luxena.db.PasteboardRefundLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.big();
        }
        PasteboardRefundSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return PasteboardRefundSemantic;
    })(RailwayDocumentSemantic);
    Luxena.PasteboardRefundSemantic = PasteboardRefundSemantic;
    //00:00:00.3680809
    /** Персона */
    var PersonSemantic = (function (_super) {
        __extends(PersonSemantic, _super);
        function PersonSemantic() {
            _super.call(this);
            //00:00:00.3682500
            this._Person = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Персона", rus: "Персоны" })
                .lookup(function () { return Luxena.sd.Person; });
            /** Номера мильных карт */
            this.MilesCardsString = this.member()
                .localizeTitle({ ru: "Номера мильных карт" })
                .string();
            /** Дата рождения */
            this.Birthday = this.member()
                .localizeTitle({ ru: "Дата рождения" })
                .date();
            /** Должность */
            this.Title = this.member()
                .localizeTitle({ ru: "Должность" })
                .string();
            /** № бонусной карты */
            this.BonusCardNumber = this.member()
                .localizeTitle({ ru: "№ бонусной карты" })
                .string();
            /** Организация */
            this.Organization = this.member()
                .localizeTitle({ ru: "Организация", rus: "Организации" })
                .lookup(function () { return Luxena.sd.Organization; });
            this.MilesCards = this.collection(function () { return Luxena.sd.MilesCard; }, function (se) { return se.Owner; });
            this.Passports = this.collection(function () { return Luxena.sd.Passport; }, function (se) { return se.Owner; });
            this.DocumentAccesses = this.collection(function () { return Luxena.sd.DocumentAccess; }, function (se) { return se.Person; });
            this.GdsAgents = this.collection(function () { return Luxena.sd.GdsAgent; }, function (se) { return se.Person; });
            this._isAbstract = false;
            this._name = "Person";
            this._names = "Persons";
            this.icon("user");
            this._isEntity = true;
            this._localizeTitle({ ru: "Персона", rus: "Персоны" });
            this._getDerivedEntities = function () { return [
                Luxena.sd.Agent
            ]; };
            this._getBaseEntity = function () { return Luxena.sd.Party; };
            this._className = "Party";
            this._getRootEntity = function () { return Luxena.sd.Party; };
            this._store = Luxena.db.Persons;
            this._saveStore = Luxena.db.Persons;
            this._lookupStore = Luxena.db.PersonLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.Name
                .localizeTitle({ ru: "Ф.И.О." })
                .length(20, 0, 0);
        }
        PersonSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return PersonSemantic;
    })(PartySemantic);
    Luxena.PersonSemantic = PersonSemantic;
    //00:00:00.3694810
    var InternalIdentitySemantic = (function (_super) {
        __extends(InternalIdentitySemantic, _super);
        function InternalIdentitySemantic() {
            _super.call(this);
            //00:00:00.3695694
            this._InternalIdentity = new Luxena.SemanticMember()
                .lookup(function () { return Luxena.sd.InternalIdentity; });
            this._isAbstract = false;
            this._name = "InternalIdentity";
            this._names = "InternalIdentities";
            this._isEntity = true;
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Identity; };
            this._className = "Identity";
            this._getRootEntity = function () { return Luxena.sd.Identity; };
            this._store = Luxena.db.InternalIdentities;
            this._saveStore = Luxena.db.InternalIdentities;
            this._lookupStore = Luxena.db.InternalIdentityLookup;
            this._referenceFields = { id: "Id", name: "Name" };
        }
        InternalIdentitySemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return InternalIdentitySemantic;
    })(IdentitySemantic);
    Luxena.InternalIdentitySemantic = InternalIdentitySemantic;
    //00:00:00.3716758
    /** Пользователь */
    var UserSemantic = (function (_super) {
        __extends(UserSemantic, _super);
        function UserSemantic() {
            _super.call(this);
            //00:00:00.3718620
            this._User = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Пользователь", rus: "Пользователи" })
                .lookup(function () { return Luxena.sd.User; });
            /** Пароль */
            this.Password = this.member()
                .localizeTitle({ ru: "Пароль" })
                .string();
            /** Новый пароль */
            this.NewPassword = this.member()
                .localizeTitle({ ru: "Новый пароль" })
                .string()
                .calculated();
            /** Подтверждение пароля */
            this.ConfirmPassword = this.member()
                .localizeTitle({ ru: "Подтверждение пароля" })
                .string()
                .calculated();
            /** Активный */
            this.Active = this.member()
                .localizeTitle({ ru: "Активный" })
                .bool()
                .required()
                .defaultValue(true);
            /** Администратор */
            this.IsAdministrator = this.member()
                .localizeTitle({ ru: "Администратор" })
                .bool()
                .required()
                .secondary();
            /** Супервизор */
            this.IsSupervisor = this.member()
                .localizeTitle({ ru: "Супервизор" })
                .bool()
                .required()
                .secondary();
            /** Агент */
            this.IsAgent = this.member()
                .localizeTitle({ ru: "Агент" })
                .bool()
                .required()
                .secondary();
            /** Кассир */
            this.IsCashier = this.member()
                .localizeTitle({ ru: "Кассир" })
                .bool()
                .required()
                .secondary();
            /** Аналитик */
            this.IsAnalyst = this.member()
                .localizeTitle({ ru: "Аналитик" })
                .bool()
                .required()
                .secondary();
            /** Субагент */
            this.IsSubAgent = this.member()
                .localizeTitle({ ru: "Субагент" })
                .bool()
                .required()
                .secondary();
            /** Роли */
            this.Roles = this.member()
                .localizeTitle({ ru: "Роли" })
                .enum(Luxena.UserRole)
                .calculated()
                .required()
                .length(30, 0, 0);
            /** Персона */
            this.Person = this.member()
                .localizeTitle({ ru: "Персона", rus: "Персоны" })
                .lookup(function () { return Luxena.sd.Person; });
            this._isAbstract = false;
            this._name = "User";
            this._names = "Users";
            this._isEntity = true;
            this._localizeTitle({ ru: "Пользователь", rus: "Пользователи" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Identity; };
            this._className = "Identity";
            this._getRootEntity = function () { return Luxena.sd.Identity; };
            this._store = Luxena.db.Users;
            this._saveStore = Luxena.db.Users;
            this._lookupStore = Luxena.db.UserLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.Name
                .localizeTitle({ ru: "Логин" })
                .length(16, 0, 0);
        }
        UserSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return UserSemantic;
    })(IdentitySemantic);
    Luxena.UserSemantic = UserSemantic;
    //00:00:00.3757603
    /** Провайдер проживания */
    var AccommodationProviderSemantic = (function (_super) {
        __extends(AccommodationProviderSemantic, _super);
        function AccommodationProviderSemantic() {
            _super.call(this);
            //00:00:00.3759269
            this._AccommodationProvider = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Провайдер проживания", rus: "Провайдеры проживания" })
                .lookup(function () { return Luxena.sd.AccommodationProvider; });
            this.MilesCards = this.collection(function () { return Luxena.sd.MilesCard; }, function (se) { return se.Organization; });
            this.Departments = this.collection(function () { return Luxena.sd.Department; }, function (se) { return se.Organization; });
            this.Persons = this.collection(function () { return Luxena.sd.Person; }, function (se) { return se.Organization; });
            this.AirlineServiceClasses = this.collection(function () { return Luxena.sd.AirlineServiceClass; }, function (se) { return se.Airline; });
            this._isAbstract = false;
            this._name = "AccommodationProvider";
            this._names = "AccommodationProviders";
            this.icon("");
            this._isEntityQuery = true;
            this._localizeTitle({ ru: "Провайдер проживания", rus: "Провайдеры проживания" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Organization; };
            this._className = "Party";
            this._getRootEntity = function () { return Luxena.sd.Party; };
            this._store = Luxena.db.AccommodationProviders;
            this._saveStore = Luxena.db.Organizations;
            this._lookupStore = Luxena.db.AccommodationProviderLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.Code
                .localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
        }
        AccommodationProviderSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return AccommodationProviderSemantic;
    })(OrganizationSemantic);
    Luxena.AccommodationProviderSemantic = AccommodationProviderSemantic;
    //00:00:00.3779424
    /** Владелец документов (активный) */
    var ActiveOwnerSemantic = (function (_super) {
        __extends(ActiveOwnerSemantic, _super);
        function ActiveOwnerSemantic() {
            _super.call(this);
            //00:00:00.3780807
            this._ActiveOwner = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Владелец документов (активный)", rus: "Владельцы документов (активные)" })
                .lookup(function () { return Luxena.sd.ActiveOwner; });
            this.Files = this.collection(function () { return Luxena.sd.File; }, function (se) { return se.Party; });
            this.DocumentOwners = this.collection(function () { return Luxena.sd.DocumentOwner; }, function (se) { return se.Owner; });
            this._isAbstract = true;
            this._name = "ActiveOwner";
            this._names = "ActiveOwners";
            this._isEntityQuery = true;
            this._localizeTitle({ ru: "Владелец документов (активный)", rus: "Владельцы документов (активные)" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Party; };
            this._className = "Party";
            this._getRootEntity = function () { return Luxena.sd.Party; };
            this._store = Luxena.db.ActiveOwners;
            this._saveStore = Luxena.db.Parties;
            this._lookupStore = Luxena.db.ActiveOwnerLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.small();
            this.Name
                .length(20, 0, 0);
        }
        ActiveOwnerSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ActiveOwnerSemantic;
    })(PartySemantic);
    Luxena.ActiveOwnerSemantic = ActiveOwnerSemantic;
    //00:00:00.3806214
    /** Агент */
    var AgentSemantic = (function (_super) {
        __extends(AgentSemantic, _super);
        function AgentSemantic() {
            _super.call(this);
            //00:00:00.3807856
            this._Agent = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Агент", rus: "Агенты" })
                .lookup(function () { return Luxena.sd.Agent; });
            this.MilesCards = this.collection(function () { return Luxena.sd.MilesCard; }, function (se) { return se.Owner; });
            this.Passports = this.collection(function () { return Luxena.sd.Passport; }, function (se) { return se.Owner; });
            this.DocumentAccesses = this.collection(function () { return Luxena.sd.DocumentAccess; }, function (se) { return se.Person; });
            this.GdsAgents = this.collection(function () { return Luxena.sd.GdsAgent; }, function (se) { return se.Person; });
            this._isAbstract = false;
            this._name = "Agent";
            this._names = "Agents";
            this.icon("user-secret");
            this._isEntityQuery = true;
            this._localizeTitle({ ru: "Агент", rus: "Агенты" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Person; };
            this._className = "Party";
            this._getRootEntity = function () { return Luxena.sd.Party; };
            this._store = Luxena.db.Agents;
            this._saveStore = Luxena.db.Persons;
            this._lookupStore = Luxena.db.AgentLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.small();
            this.Name
                .localizeTitle({ ru: "Ф.И.О." })
                .length(20, 0, 0);
        }
        AgentSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return AgentSemantic;
    })(PersonSemantic);
    Luxena.AgentSemantic = AgentSemantic;
    //00:00:00.3837427
    /** Авиакомпания */
    var AirlineSemantic = (function (_super) {
        __extends(AirlineSemantic, _super);
        function AirlineSemantic() {
            _super.call(this);
            //00:00:00.3839081
            this._Airline = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Авиакомпания", rus: "Авиакомпании" })
                .lookup(function () { return Luxena.sd.Airline; });
            this.MilesCards = this.collection(function () { return Luxena.sd.MilesCard; }, function (se) { return se.Organization; });
            this.Departments = this.collection(function () { return Luxena.sd.Department; }, function (se) { return se.Organization; });
            this.Persons = this.collection(function () { return Luxena.sd.Person; }, function (se) { return se.Organization; });
            this.AirlineServiceClasses = this.collection(function () { return Luxena.sd.AirlineServiceClass; }, function (se) { return se.Airline; });
            this._isAbstract = false;
            this._name = "Airline";
            this._names = "Airlines";
            this.icon("plane");
            this._isEntityQuery = true;
            this._localizeTitle({ ru: "Авиакомпания", rus: "Авиакомпании" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Organization; };
            this._className = "Party";
            this._getRootEntity = function () { return Luxena.sd.Party; };
            this._store = Luxena.db.Airlines;
            this._saveStore = Luxena.db.Organizations;
            this._lookupStore = Luxena.db.AirlineLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.Code
                .localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
        }
        AirlineSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return AirlineSemantic;
    })(OrganizationSemantic);
    Luxena.AirlineSemantic = AirlineSemantic;
    //00:00:00.3867878
    /** Провайдер автобусных билетов */
    var BusTicketProviderSemantic = (function (_super) {
        __extends(BusTicketProviderSemantic, _super);
        function BusTicketProviderSemantic() {
            _super.call(this);
            //00:00:00.3869430
            this._BusTicketProvider = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Провайдер автобусных билетов", rus: "Провайдеры автобусных билетов" })
                .lookup(function () { return Luxena.sd.BusTicketProvider; });
            this.MilesCards = this.collection(function () { return Luxena.sd.MilesCard; }, function (se) { return se.Organization; });
            this.Departments = this.collection(function () { return Luxena.sd.Department; }, function (se) { return se.Organization; });
            this.Persons = this.collection(function () { return Luxena.sd.Person; }, function (se) { return se.Organization; });
            this.AirlineServiceClasses = this.collection(function () { return Luxena.sd.AirlineServiceClass; }, function (se) { return se.Airline; });
            this._isAbstract = false;
            this._name = "BusTicketProvider";
            this._names = "BusTicketProviders";
            this.icon("");
            this._isEntityQuery = true;
            this._localizeTitle({ ru: "Провайдер автобусных билетов", rus: "Провайдеры автобусных билетов" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Organization; };
            this._className = "Party";
            this._getRootEntity = function () { return Luxena.sd.Party; };
            this._store = Luxena.db.BusTicketProviders;
            this._saveStore = Luxena.db.Organizations;
            this._lookupStore = Luxena.db.BusTicketProviderLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.Code
                .localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
        }
        BusTicketProviderSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return BusTicketProviderSemantic;
    })(OrganizationSemantic);
    Luxena.BusTicketProviderSemantic = BusTicketProviderSemantic;
    //00:00:00.3899420
    /** Провайдер аренды авто */
    var CarRentalProviderSemantic = (function (_super) {
        __extends(CarRentalProviderSemantic, _super);
        function CarRentalProviderSemantic() {
            _super.call(this);
            //00:00:00.3901007
            this._CarRentalProvider = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Провайдер аренды авто", rus: "Провайдеры аренды авто" })
                .lookup(function () { return Luxena.sd.CarRentalProvider; });
            this.MilesCards = this.collection(function () { return Luxena.sd.MilesCard; }, function (se) { return se.Organization; });
            this.Departments = this.collection(function () { return Luxena.sd.Department; }, function (se) { return se.Organization; });
            this.Persons = this.collection(function () { return Luxena.sd.Person; }, function (se) { return se.Organization; });
            this.AirlineServiceClasses = this.collection(function () { return Luxena.sd.AirlineServiceClass; }, function (se) { return se.Airline; });
            this._isAbstract = false;
            this._name = "CarRentalProvider";
            this._names = "CarRentalProviders";
            this.icon("");
            this._isEntityQuery = true;
            this._localizeTitle({ ru: "Провайдер аренды авто", rus: "Провайдеры аренды авто" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Organization; };
            this._className = "Party";
            this._getRootEntity = function () { return Luxena.sd.Party; };
            this._store = Luxena.db.CarRentalProviders;
            this._saveStore = Luxena.db.Organizations;
            this._lookupStore = Luxena.db.CarRentalProviderLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.Code
                .localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
        }
        CarRentalProviderSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return CarRentalProviderSemantic;
    })(OrganizationSemantic);
    Luxena.CarRentalProviderSemantic = CarRentalProviderSemantic;
    //00:00:00.3921274
    /** Заказчик */
    var CustomerSemantic = (function (_super) {
        __extends(CustomerSemantic, _super);
        function CustomerSemantic() {
            _super.call(this);
            //00:00:00.3922584
            this._Customer = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Заказчик", rus: "Заказчики" })
                .lookup(function () { return Luxena.sd.Customer; });
            this.Files = this.collection(function () { return Luxena.sd.File; }, function (se) { return se.Party; });
            this.DocumentOwners = this.collection(function () { return Luxena.sd.DocumentOwner; }, function (se) { return se.Owner; });
            this._isAbstract = true;
            this._name = "Customer";
            this._names = "Customers";
            this.icon("");
            this._isEntityQuery = true;
            this._localizeTitle({ ru: "Заказчик", rus: "Заказчики" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Party; };
            this._className = "Party";
            this._getRootEntity = function () { return Luxena.sd.Party; };
            this._store = Luxena.db.Customers;
            this._saveStore = Luxena.db.Parties;
            this._lookupStore = Luxena.db.CustomerLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.Name
                .length(20, 0, 0);
        }
        CustomerSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return CustomerSemantic;
    })(PartySemantic);
    Luxena.CustomerSemantic = CustomerSemantic;
    //00:00:00.3978383
    /** Провайдер дополнительных услуг */
    var GenericProductProviderSemantic = (function (_super) {
        __extends(GenericProductProviderSemantic, _super);
        function GenericProductProviderSemantic() {
            _super.call(this);
            //00:00:00.3980231
            this._GenericProductProvider = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Провайдер дополнительных услуг", rus: "Провайдеры дополнительных услуг" })
                .lookup(function () { return Luxena.sd.GenericProductProvider; });
            this.MilesCards = this.collection(function () { return Luxena.sd.MilesCard; }, function (se) { return se.Organization; });
            this.Departments = this.collection(function () { return Luxena.sd.Department; }, function (se) { return se.Organization; });
            this.Persons = this.collection(function () { return Luxena.sd.Person; }, function (se) { return se.Organization; });
            this.AirlineServiceClasses = this.collection(function () { return Luxena.sd.AirlineServiceClass; }, function (se) { return se.Airline; });
            this._isAbstract = false;
            this._name = "GenericProductProvider";
            this._names = "GenericProductProviders";
            this.icon("");
            this._isEntityQuery = true;
            this._localizeTitle({ ru: "Провайдер дополнительных услуг", rus: "Провайдеры дополнительных услуг" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Organization; };
            this._className = "Party";
            this._getRootEntity = function () { return Luxena.sd.Party; };
            this._store = Luxena.db.GenericProductProviders;
            this._saveStore = Luxena.db.Organizations;
            this._lookupStore = Luxena.db.GenericProductProviderLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.Code
                .localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
        }
        GenericProductProviderSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return GenericProductProviderSemantic;
    })(OrganizationSemantic);
    Luxena.GenericProductProviderSemantic = GenericProductProviderSemantic;
    //00:00:00.4009584
    /** Страховая компания */
    var InsuranceCompanySemantic = (function (_super) {
        __extends(InsuranceCompanySemantic, _super);
        function InsuranceCompanySemantic() {
            _super.call(this);
            //00:00:00.4011153
            this._InsuranceCompany = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Страховая компания", rus: "Страховые компании" })
                .lookup(function () { return Luxena.sd.InsuranceCompany; });
            this.MilesCards = this.collection(function () { return Luxena.sd.MilesCard; }, function (se) { return se.Organization; });
            this.Departments = this.collection(function () { return Luxena.sd.Department; }, function (se) { return se.Organization; });
            this.Persons = this.collection(function () { return Luxena.sd.Person; }, function (se) { return se.Organization; });
            this.AirlineServiceClasses = this.collection(function () { return Luxena.sd.AirlineServiceClass; }, function (se) { return se.Airline; });
            this._isAbstract = false;
            this._name = "InsuranceCompany";
            this._names = "InsuranceCompanies";
            this.icon("");
            this._isEntityQuery = true;
            this._localizeTitle({ ru: "Страховая компания", rus: "Страховые компании" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Organization; };
            this._className = "Party";
            this._getRootEntity = function () { return Luxena.sd.Party; };
            this._store = Luxena.db.InsuranceCompanies;
            this._saveStore = Luxena.db.Organizations;
            this._lookupStore = Luxena.db.InsuranceCompanyLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.Code
                .localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
        }
        InsuranceCompanySemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return InsuranceCompanySemantic;
    })(OrganizationSemantic);
    Luxena.InsuranceCompanySemantic = InsuranceCompanySemantic;
    //00:00:00.4039981
    /** Провайдер ж/д билетов */
    var PasteboardProviderSemantic = (function (_super) {
        __extends(PasteboardProviderSemantic, _super);
        function PasteboardProviderSemantic() {
            _super.call(this);
            //00:00:00.4041532
            this._PasteboardProvider = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Провайдер ж/д билетов", rus: "Провайдеры ж/д билетов" })
                .lookup(function () { return Luxena.sd.PasteboardProvider; });
            this.MilesCards = this.collection(function () { return Luxena.sd.MilesCard; }, function (se) { return se.Organization; });
            this.Departments = this.collection(function () { return Luxena.sd.Department; }, function (se) { return se.Organization; });
            this.Persons = this.collection(function () { return Luxena.sd.Person; }, function (se) { return se.Organization; });
            this.AirlineServiceClasses = this.collection(function () { return Luxena.sd.AirlineServiceClass; }, function (se) { return se.Airline; });
            this._isAbstract = false;
            this._name = "PasteboardProvider";
            this._names = "PasteboardProviders";
            this.icon("");
            this._isEntityQuery = true;
            this._localizeTitle({ ru: "Провайдер ж/д билетов", rus: "Провайдеры ж/д билетов" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Organization; };
            this._className = "Party";
            this._getRootEntity = function () { return Luxena.sd.Party; };
            this._store = Luxena.db.PasteboardProviders;
            this._saveStore = Luxena.db.Organizations;
            this._lookupStore = Luxena.db.PasteboardProviderLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.Code
                .localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
        }
        PasteboardProviderSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return PasteboardProviderSemantic;
    })(OrganizationSemantic);
    Luxena.PasteboardProviderSemantic = PasteboardProviderSemantic;
    //00:00:00.4054869
    /** Квитанция */
    var ReceiptSemantic = (function (_super) {
        __extends(ReceiptSemantic, _super);
        function ReceiptSemantic() {
            _super.call(this);
            //00:00:00.4055611
            this._Receipt = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Квитанция", rus: "Квитанции" })
                .lookup(function () { return Luxena.sd.Receipt; });
            this.Payments = this.collection(function () { return Luxena.sd.Payment; }, function (se) { return se.Invoice; });
            this._isAbstract = false;
            this._name = "Receipt";
            this._names = "Receipts";
            this._isEntityQuery = true;
            this._localizeTitle({ ru: "Квитанция", rus: "Квитанции" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Invoice; };
            this._className = "Invoice";
            this._getRootEntity = function () { return Luxena.sd.Invoice; };
            this._store = Luxena.db.Receipts;
            this._saveStore = Luxena.db.Invoices;
            this._lookupStore = Luxena.db.ReceiptLookup;
            this._referenceFields = { id: "Id", name: "Number" };
        }
        ReceiptSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ReceiptSemantic;
    })(InvoiceSemantic);
    Luxena.ReceiptSemantic = ReceiptSemantic;
    //00:00:00.4083503
    /** Мобильный оператор */
    var RoamingOperatorSemantic = (function (_super) {
        __extends(RoamingOperatorSemantic, _super);
        function RoamingOperatorSemantic() {
            _super.call(this);
            //00:00:00.4085139
            this._RoamingOperator = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Мобильный оператор", rus: "Мобильные операторы" })
                .lookup(function () { return Luxena.sd.RoamingOperator; });
            this.MilesCards = this.collection(function () { return Luxena.sd.MilesCard; }, function (se) { return se.Organization; });
            this.Departments = this.collection(function () { return Luxena.sd.Department; }, function (se) { return se.Organization; });
            this.Persons = this.collection(function () { return Luxena.sd.Person; }, function (se) { return se.Organization; });
            this.AirlineServiceClasses = this.collection(function () { return Luxena.sd.AirlineServiceClass; }, function (se) { return se.Airline; });
            this._isAbstract = false;
            this._name = "RoamingOperator";
            this._names = "RoamingOperators";
            this.icon("");
            this._isEntityQuery = true;
            this._localizeTitle({ ru: "Мобильный оператор", rus: "Мобильные операторы" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Organization; };
            this._className = "Party";
            this._getRootEntity = function () { return Luxena.sd.Party; };
            this._store = Luxena.db.RoamingOperators;
            this._saveStore = Luxena.db.Organizations;
            this._lookupStore = Luxena.db.RoamingOperatorLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.small();
            this.Code
                .localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
        }
        RoamingOperatorSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return RoamingOperatorSemantic;
    })(OrganizationSemantic);
    Luxena.RoamingOperatorSemantic = RoamingOperatorSemantic;
    //00:00:00.4118513
    /** Провайдер туров (готовых) */
    var TourProviderSemantic = (function (_super) {
        __extends(TourProviderSemantic, _super);
        function TourProviderSemantic() {
            _super.call(this);
            //00:00:00.4120113
            this._TourProvider = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Провайдер туров (готовых)", rus: "Провайдеры туров (готовых)" })
                .lookup(function () { return Luxena.sd.TourProvider; });
            this.MilesCards = this.collection(function () { return Luxena.sd.MilesCard; }, function (se) { return se.Organization; });
            this.Departments = this.collection(function () { return Luxena.sd.Department; }, function (se) { return se.Organization; });
            this.Persons = this.collection(function () { return Luxena.sd.Person; }, function (se) { return se.Organization; });
            this.AirlineServiceClasses = this.collection(function () { return Luxena.sd.AirlineServiceClass; }, function (se) { return se.Airline; });
            this._isAbstract = false;
            this._name = "TourProvider";
            this._names = "TourProviders";
            this.icon("");
            this._isEntityQuery = true;
            this._localizeTitle({ ru: "Провайдер туров (готовых)", rus: "Провайдеры туров (готовых)" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Organization; };
            this._className = "Party";
            this._getRootEntity = function () { return Luxena.sd.Party; };
            this._store = Luxena.db.TourProviders;
            this._saveStore = Luxena.db.Organizations;
            this._lookupStore = Luxena.db.TourProviderLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.Code
                .localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
        }
        TourProviderSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return TourProviderSemantic;
    })(OrganizationSemantic);
    Luxena.TourProviderSemantic = TourProviderSemantic;
    //00:00:00.4150462
    /** Провайдер трансферов */
    var TransferProviderSemantic = (function (_super) {
        __extends(TransferProviderSemantic, _super);
        function TransferProviderSemantic() {
            _super.call(this);
            //00:00:00.4152092
            this._TransferProvider = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Провайдер трансферов", rus: "Провайдеры трансферов" })
                .lookup(function () { return Luxena.sd.TransferProvider; });
            this.MilesCards = this.collection(function () { return Luxena.sd.MilesCard; }, function (se) { return se.Organization; });
            this.Departments = this.collection(function () { return Luxena.sd.Department; }, function (se) { return se.Organization; });
            this.Persons = this.collection(function () { return Luxena.sd.Person; }, function (se) { return se.Organization; });
            this.AirlineServiceClasses = this.collection(function () { return Luxena.sd.AirlineServiceClass; }, function (se) { return se.Airline; });
            this._isAbstract = false;
            this._name = "TransferProvider";
            this._names = "TransferProviders";
            this.icon("");
            this._isEntityQuery = true;
            this._localizeTitle({ ru: "Провайдер трансферов", rus: "Провайдеры трансферов" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Organization; };
            this._className = "Party";
            this._getRootEntity = function () { return Luxena.sd.Party; };
            this._store = Luxena.db.TransferProviders;
            this._saveStore = Luxena.db.Organizations;
            this._lookupStore = Luxena.db.TransferProviderLookup;
            this._referenceFields = { id: "Id", name: "Name" };
            this.Code
                .localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
        }
        TransferProviderSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return TransferProviderSemantic;
    })(OrganizationSemantic);
    Luxena.TransferProviderSemantic = TransferProviderSemantic;
    //00:00:00.4200429
    var ProductTotalSemantic = (function (_super) {
        __extends(ProductTotalSemantic, _super);
        function ProductTotalSemantic() {
            _super.call(this);
            //00:00:00.4204456
            this.Total = this.member()
                .float(2);
            this.ServiceFee = this.member()
                .float(2);
            this.GrandTotal = this.member()
                .float(2);
            /** Примечание */
            this.Note = this.member()
                .localizeTitle({ ru: "Примечание" })
                .string();
            this._isAbstract = false;
            this._name = "ProductTotal";
            this._names = "ProductTotal";
            this._getDerivedEntities = function () { return [
                Luxena.sd.ProductTotalByBooker, Luxena.sd.ProductTotalByType, Luxena.sd.ProductTotalByOwner, Luxena.sd.ProductTotalByYear, Luxena.sd.ProductTotalByProvider, Luxena.sd.ProductTotalBySeller, Luxena.sd.ProductTotalByQuarter, Luxena.sd.ProductTotalByMonth, Luxena.sd.ProductTotalByDay
            ]; };
            this.Total
                .localizeTitle({ ru: "К перечислению провайдеру" });
            this.ServiceFee
                .localizeTitle({ ru: "Сервисный сбор" })
                .subject();
            this.GrandTotal
                .localizeTitle({ ru: "К оплате" });
        }
        ProductTotalSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ProductTotalSemantic;
    })(Luxena.SemanticEntity);
    Luxena.ProductTotalSemantic = ProductTotalSemantic;
    //00:00:00.4227237
    var ProfitDistributionTotalSemantic = (function (_super) {
        __extends(ProfitDistributionTotalSemantic, _super);
        function ProfitDistributionTotalSemantic() {
            _super.call(this);
            //00:00:00.4230998
            /** № */
            this.Rank = this.member()
                .localizeTitle({ ru: "№" })
                .int()
                .required()
                .entityPosition();
            /** Кол-во продаж */
            this.SellCount = this.member()
                .localizeTitle({ ru: "Кол-во продаж" })
                .int()
                .required();
            /** Кол-во возвратов */
            this.RefundCount = this.member()
                .localizeTitle({ ru: "Кол-во возвратов" })
                .int()
                .required();
            /** Кол-во ануляций */
            this.VoidCount = this.member()
                .localizeTitle({ ru: "Кол-во ануляций" })
                .int()
                .required();
            /** Валюта */
            this.Currency = this.member()
                .localizeTitle({ ru: "Валюта" })
                .string();
            /** Продано */
            this.SellGrandTotal = this.member()
                .localizeTitle({ ru: "Продано" })
                .float(2);
            /** Возврат */
            this.RefundGrandTotal = this.member()
                .localizeTitle({ ru: "Возврат" })
                .float(2);
            this.GrandTotal = this.member()
                .float(2);
            this.Total = this.member()
                .float(2);
            this.ServiceFee = this.member()
                .float(2);
            this.Commission = this.member()
                .float(2);
            /** Итого по агенту */
            this.AgentTotal = this.member()
                .localizeTitle({ ru: "Итого по агенту" })
                .float(2);
            this.Vat = this.member()
                .float(2);
            this._isAbstract = false;
            this._name = "ProfitDistributionTotal";
            this._names = "ProfitDistributionTotal";
            this._getDerivedEntities = function () { return [
                Luxena.sd.ProfitDistributionByCustomer, Luxena.sd.ProfitDistributionByProvider
            ]; };
            this.GrandTotal
                .localizeTitle({ ru: "К оплате" });
            this.Total
                .localizeTitle({ ru: "К перечислению провайдеру" });
            this.ServiceFee
                .localizeTitle({ ru: "Сервисный сбор" })
                .subject();
            this.Commission
                .localizeTitle({ ru: "Комиссия" });
            this.Vat
                .localizeTitle({ ru: "В т.ч. НДС" });
        }
        ProfitDistributionTotalSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ProfitDistributionTotalSemantic;
    })(Luxena.SemanticEntity);
    Luxena.ProfitDistributionTotalSemantic = ProfitDistributionTotalSemantic;
    //00:00:00.4284956
    /** Ежедневный отчет по выручке */
    var EverydayProfitReportSemantic = (function (_super) {
        __extends(EverydayProfitReportSemantic, _super);
        function EverydayProfitReportSemantic() {
            _super.call(this);
            //00:00:00.4299717
            this._EverydayProfitReport = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Ежедневный отчет по выручке" })
                .lookup(function () { return Luxena.sd.EverydayProfitReport; });
            this.Provider = this.member()
                .lookup(function () { return Luxena.sd.Organization; });
            /** Вид услуги */
            this.ProductType = this.member()
                .localizeTitle({ ru: "Вид услуги" })
                .enum(Luxena.ProductType)
                .required()
                .length(12, 0, 0);
            /** Услуга */
            this.Product = this.member()
                .localizeTitle({ ru: "Услуга", rus: "Все услуги" })
                .lookup(function () { return Luxena.sd.Product; })
                .entityName();
            this.IssueDate = this.member()
                .date()
                .required();
            this.Seller = this.member()
                .lookup(function () { return Luxena.sd.Person; });
            /** Пассажиры / Покупатели */
            this.PassengerName = this.member()
                .localizeTitle({ ru: "Пассажиры / Покупатели" })
                .string()
                .length(16, 0, 0);
            this.Itinerary = this.member()
                .string();
            this.StartDate = this.member()
                .date();
            this.FinishDate = this.member()
                .date();
            /** Страна */
            this.Country = this.member()
                .localizeTitle({ ru: "Страна", rus: "Страны" })
                .lookup(function () { return Luxena.sd.Country; });
            this.Fare = this.member()
                .money();
            /** Валюта */
            this.Currency = this.member()
                .localizeTitle({ ru: "Валюта" })
                .string();
            /** Курс валюты */
            this.CurrencyRate = this.member()
                .localizeTitle({ ru: "Курс валюты" })
                .float();
            this.EqualFare = this.member()
                .float(2);
            this.FeesTotal = this.member()
                .float(2);
            this.CancelFee = this.member()
                .float(2);
            this.Total = this.member()
                .float(2);
            this.Commission = this.member()
                .float(2);
            this.ServiceFee = this.member()
                .float(2);
            this.Vat = this.member()
                .float(2);
            this.GrandTotal = this.member()
                .float(2);
            /** Заказ */
            this.Order = this.member()
                .localizeTitle({ ru: "Заказ", rus: "Заказы" })
                .lookup(function () { return Luxena.sd.Order; });
            this.Payer = this.member()
                .lookup(function () { return Luxena.sd.Party; });
            /** Счёт */
            this.Invoice = this.member()
                .localizeTitle({ ru: "Счёт" })
                .lookup(function () { return Luxena.sd.Invoice; });
            /** Дата счёта */
            this.InvoiceDate = this.member()
                .localizeTitle({ ru: "Дата счёта" })
                .date();
            /** Акт */
            this.CompletionCertificate = this.member()
                .localizeTitle({ ru: "Акт" })
                .lookup(function () { return Luxena.sd.Invoice; });
            /** Дата акта */
            this.CompletionCertificateDate = this.member()
                .localizeTitle({ ru: "Дата акта" })
                .date();
            /** Оплата */
            this.Payment = this.member()
                .localizeTitle({ ru: "Оплата" })
                .lookup(function () { return Luxena.sd.Payment; });
            /** Дата оплаты */
            this.PaymentDate = this.member()
                .localizeTitle({ ru: "Дата оплаты" })
                .date();
            this._isAbstract = false;
            this._name = "EverydayProfitReport";
            this._names = "EverydayProfitReports";
            this._isQueryResult = true;
            this._localizeTitle({ ru: "Ежедневный отчет по выручке" });
            this._getDerivedEntities = null;
            this._className = "EverydayProfitReport";
            this._getRootEntity = function () { return Luxena.sd.EverydayProfitReport; };
            this._store = Luxena.db.EverydayProfitReports;
            this._saveStore = Luxena.db.EverydayProfitReports;
            this._referenceFields = { id: "", name: "" };
            this.Provider
                .localizeTitle({ ru: "Провайдер" })
                .lookup(function () { return Luxena.sd.Organization; });
            this.ProductType
                .localizeTitle({ ru: "Вид услуги" })
                .required()
                .length(12, 0, 0)
                .entityType();
            this.IssueDate
                .localizeTitle({ ru: "Дата выпуска" })
                .required()
                .entityDate();
            this.Seller
                .localizeTitle({ ru: "Продавец", rus: "Агенты" })
                .lookup(function () { return Luxena.sd.Person; });
            this.Itinerary
                .localizeTitle({ ru: "Маршрут" })
                .length(16, 0, 0);
            this.StartDate
                .localizeTitle({ ru: "Дата начала" });
            this.FinishDate
                .localizeTitle({ ru: "Дата окончания" });
            this.Fare
                .localizeTitle({ ru: "Тариф" });
            this.EqualFare
                .localizeTitle({ ru: "Экв. тариф" })
                .subject();
            this.FeesTotal
                .localizeTitle({ ru: "Таксы" })
                .subject();
            this.CancelFee
                .localizeTitle({ ru: "Штраф за отмену" })
                .subject();
            this.Total
                .localizeTitle({ ru: "К перечислению провайдеру" });
            this.Commission
                .localizeTitle({ ru: "Комиссия" });
            this.ServiceFee
                .localizeTitle({ ru: "Сервисный сбор" })
                .subject();
            this.Vat
                .localizeTitle({ ru: "В т.ч. НДС" });
            this.GrandTotal
                .localizeTitle({ ru: "К оплате" });
            this.Order
                .localizeTitle({ ru: "Заказ", rus: "Заказы" })
                .lookup(function () { return Luxena.sd.Order; })
                .subject();
            this.Payer
                .localizeTitle({ ru: "Плательщик", rus: "Заказчики" })
                .lookup(function () { return Luxena.sd.Party; });
        }
        EverydayProfitReportSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return EverydayProfitReportSemantic;
    })(Luxena.SemanticEntity);
    Luxena.EverydayProfitReportSemantic = EverydayProfitReportSemantic;
    //00:00:00.4338693
    /** Flown-отчет */
    var FlownReportSemantic = (function (_super) {
        __extends(FlownReportSemantic, _super);
        function FlownReportSemantic() {
            _super.call(this);
            //00:00:00.4340051
            this._FlownReport = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Flown-отчет" })
                .lookup(function () { return Luxena.sd.FlownReport; });
            this.Date = this.member()
                .date()
                .required()
                .entityDate();
            this.Op = this.member()
                .string()
                .length(2, 0, 0);
            this.AC = this.member()
                .string()
                .length(2, 0, 0);
            this.TicketNumber = this.member()
                .lookup(function () { return Luxena.sd.AviaDocument; });
            this.Client = this.member()
                .lookup(function () { return Luxena.sd.Party; })
                .length(20, 0, 0);
            this.Passenger = this.member()
                .string()
                .length(20, 0, 0);
            this.Route = this.member()
                .string()
                .length(16, 0, 0);
            this.Curr = this.member()
                .string()
                .length(3, 0, 0);
            this.Fare = this.member()
                .float(2);
            this.Tax = this.member()
                .float(2);
            this.Flown1 = this.member()
                .float(2);
            this.Flown2 = this.member()
                .float(2);
            this.Flown3 = this.member()
                .float(2);
            this.Flown4 = this.member()
                .float(2);
            this.Flown5 = this.member()
                .float(2);
            this.Flown6 = this.member()
                .float(2);
            this.Flown7 = this.member()
                .float(2);
            this.Flown8 = this.member()
                .float(2);
            this.Flown9 = this.member()
                .float(2);
            this.Flown10 = this.member()
                .float(2);
            this.Flown11 = this.member()
                .float(2);
            this.Flown12 = this.member()
                .float(2);
            this.TourCode = this.member()
                .string()
                .length(10, 0, 0);
            /** Добор с билета */
            this.CheapTicket = this.member()
                .localizeTitle({ ru: "Добор с билета" })
                .lookup(function () { return Luxena.sd.AviaDocument; })
                .length(14, 0, 0);
            this._isAbstract = false;
            this._name = "FlownReport";
            this._names = "FlownReports";
            this._isQueryResult = true;
            this._localizeTitle({ ru: "Flown-отчет" });
            this._getDerivedEntities = null;
            this._className = "FlownReport";
            this._getRootEntity = function () { return Luxena.sd.FlownReport; };
            this._store = Luxena.db.FlownReports;
            this._saveStore = Luxena.db.FlownReports;
            this._referenceFields = { id: "", name: "" };
            this.TourCode
                .localizeTitle({ ru: "Туркод" });
        }
        FlownReportSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return FlownReportSemantic;
    })(Luxena.SemanticEntity);
    Luxena.FlownReportSemantic = FlownReportSemantic;
    //00:00:00.4369455
    /** Сводка по услугам */
    var ProductSummarySemantic = (function (_super) {
        __extends(ProductSummarySemantic, _super);
        function ProductSummarySemantic() {
            _super.call(this);
            //00:00:00.4377428
            this._ProductSummary = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Сводка по услугам" })
                .lookup(function () { return Luxena.sd.ProductSummary; });
            this.IssueDate = this.member()
                .date();
            this.Type = this.member()
                .localizeTitle({ ru: "Вид услуги" })
                .enum(Luxena.ProductType)
                .length(12, 0, 0);
            this.Name = this.member()
                .string();
            this.Itinerary = this.member()
                .string();
            this.IsRefund = this.member()
                .bool()
                .required();
            this.Total = this.member()
                .money();
            this.ServiceFee = this.member()
                .money();
            this.GrandTotal = this.member()
                .money();
            /** Заказ */
            this.Order = this.member()
                .localizeTitle({ ru: "Заказ", rus: "Заказы" })
                .lookup(function () { return Luxena.sd.Order; });
            this._isAbstract = false;
            this._name = "ProductSummary";
            this._names = "ProductSummaries";
            this._isQueryResult = true;
            this._localizeTitle({ ru: "Сводка по услугам" });
            this._getDerivedEntities = null;
            this._className = "ProductSummary";
            this._getRootEntity = function () { return Luxena.sd.ProductSummary; };
            this._store = Luxena.db.ProductSummaries;
            this._saveStore = Luxena.db.ProductSummaries;
            this._referenceFields = { id: "", name: "" };
            this.IssueDate
                .localizeTitle({ ru: "Дата выпуска" })
                .entityDate();
            this.Type
                .localizeTitle({ ru: "Вид услуги" })
                .length(12, 0, 0)
                .entityType();
            this.Name
                .localizeTitle({ ru: "Название" })
                .length(16, 0, 0)
                .entityName();
            this.Itinerary
                .localizeTitle({ ru: "Маршрут" })
                .length(16, 0, 0);
            this.IsRefund
                .localizeTitle({ ru: "Это возврат" })
                .required();
            this.Total
                .localizeTitle({ ru: "К перечислению провайдеру" });
            this.ServiceFee
                .localizeTitle({ ru: "Сервисный сбор" })
                .subject();
            this.GrandTotal
                .localizeTitle({ ru: "К оплате" });
            this.Order
                .localizeTitle({ ru: "Заказ", rus: "Заказы" })
                .lookup(function () { return Luxena.sd.Order; })
                .subject();
        }
        ProductSummarySemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ProductSummarySemantic;
    })(Luxena.SemanticEntity);
    Luxena.ProductSummarySemantic = ProductSummarySemantic;
    //00:00:00.4395554
    /** Услуги итого по бронировщику */
    var ProductTotalByBookerSemantic = (function (_super) {
        __extends(ProductTotalByBookerSemantic, _super);
        function ProductTotalByBookerSemantic() {
            _super.call(this);
            //00:00:00.4425994
            this._ProductTotalByBooker = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Услуги итого по бронировщику", ruShort: "По бронировщику" })
                .lookup(function () { return Luxena.sd.ProductTotalByBooker; });
            /** № */
            this.Rank = this.member()
                .localizeTitle({ ru: "№" })
                .int()
                .required();
            this.BookerName = this.member()
                .string();
            this.Booker = this.member()
                .lookup(function () { return Luxena.sd.Person; });
            this._isAbstract = false;
            this._name = "ProductTotalByBooker";
            this._names = "ProductTotalByBookers";
            this._isQueryResult = true;
            this._localizeTitle({ ru: "Услуги итого по бронировщику", ruShort: "По бронировщику" });
            this._getDerivedEntities = null;
            this._className = "ProductTotalByBooker";
            this._getRootEntity = function () { return Luxena.sd.ProductTotalByBooker; };
            this._store = Luxena.db.ProductTotalByBookers;
            this._saveStore = Luxena.db.ProductTotalByBookers;
            this._referenceFields = { id: "", name: "" };
            this.Booker
                .localizeTitle({ ru: "Бронировщик" })
                .lookup(function () { return Luxena.sd.Person; });
        }
        ProductTotalByBookerSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ProductTotalByBookerSemantic;
    })(ProductTotalSemantic);
    Luxena.ProductTotalByBookerSemantic = ProductTotalByBookerSemantic;
    //00:00:00.4432306
    /** Услуги итого посуточно */
    var ProductTotalByDaySemantic = (function (_super) {
        __extends(ProductTotalByDaySemantic, _super);
        function ProductTotalByDaySemantic() {
            _super.call(this);
            //00:00:00.4433130
            this._ProductTotalByDay = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Услуги итого посуточно", ruShort: "Посуточно" })
                .lookup(function () { return Luxena.sd.ProductTotalByDay; });
            /** Дата */
            this.IssueDate = this.member()
                .localizeTitle({ en: "Date", ru: "Дата" })
                .date()
                .required();
            this._isAbstract = false;
            this._name = "ProductTotalByDay";
            this._names = "ProductTotalByDays";
            this._isQueryResult = true;
            this._localizeTitle({ ru: "Услуги итого посуточно", ruShort: "Посуточно" });
            this._getDerivedEntities = null;
            this._className = "ProductTotalByDay";
            this._getRootEntity = function () { return Luxena.sd.ProductTotalByDay; };
            this._store = Luxena.db.ProductTotalByDays;
            this._saveStore = Luxena.db.ProductTotalByDays;
            this._referenceFields = { id: "IssueDate", name: "" };
        }
        ProductTotalByDaySemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ProductTotalByDaySemantic;
    })(ProductTotalSemantic);
    Luxena.ProductTotalByDaySemantic = ProductTotalByDaySemantic;
    //00:00:00.4438089
    /** Услуги итого помесячно */
    var ProductTotalByMonthSemantic = (function (_super) {
        __extends(ProductTotalByMonthSemantic, _super);
        function ProductTotalByMonthSemantic() {
            _super.call(this);
            //00:00:00.4438871
            this._ProductTotalByMonth = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Услуги итого помесячно", ruShort: "Помесячно" })
                .lookup(function () { return Luxena.sd.ProductTotalByMonth; });
            /** Месяц */
            this.IssueDate = this.member()
                .localizeTitle({ ru: "Месяц" })
                .monthAndYear()
                .required();
            this._isAbstract = false;
            this._name = "ProductTotalByMonth";
            this._names = "ProductTotalByMonths";
            this._isQueryResult = true;
            this._localizeTitle({ ru: "Услуги итого помесячно", ruShort: "Помесячно" });
            this._getDerivedEntities = null;
            this._className = "ProductTotalByMonth";
            this._getRootEntity = function () { return Luxena.sd.ProductTotalByMonth; };
            this._store = Luxena.db.ProductTotalByMonths;
            this._saveStore = Luxena.db.ProductTotalByMonths;
            this._referenceFields = { id: "", name: "" };
        }
        ProductTotalByMonthSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ProductTotalByMonthSemantic;
    })(ProductTotalSemantic);
    Luxena.ProductTotalByMonthSemantic = ProductTotalByMonthSemantic;
    //00:00:00.4451229
    /** Услуги итого по владельцу */
    var ProductTotalByOwnerSemantic = (function (_super) {
        __extends(ProductTotalByOwnerSemantic, _super);
        function ProductTotalByOwnerSemantic() {
            _super.call(this);
            //00:00:00.4452902
            this._ProductTotalByOwner = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Услуги итого по владельцу", ruShort: "По владельцу" })
                .lookup(function () { return Luxena.sd.ProductTotalByOwner; });
            /** № */
            this.Rank = this.member()
                .localizeTitle({ ru: "№" })
                .int()
                .required();
            this.OwnerName = this.member()
                .string();
            this.Owner = this.member()
                .lookup(function () { return Luxena.sd.Party; });
            this._isAbstract = false;
            this._name = "ProductTotalByOwner";
            this._names = "ProductTotalByOwners";
            this._isQueryResult = true;
            this._localizeTitle({ ru: "Услуги итого по владельцу", ruShort: "По владельцу" });
            this._getDerivedEntities = null;
            this._className = "ProductTotalByOwner";
            this._getRootEntity = function () { return Luxena.sd.ProductTotalByOwner; };
            this._store = Luxena.db.ProductTotalByOwners;
            this._saveStore = Luxena.db.ProductTotalByOwners;
            this._referenceFields = { id: "", name: "" };
            this.Owner
                .localizeTitle({ ru: "Владелец" })
                .lookup(function () { return Luxena.sd.Party; });
        }
        ProductTotalByOwnerSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ProductTotalByOwnerSemantic;
    })(ProductTotalSemantic);
    Luxena.ProductTotalByOwnerSemantic = ProductTotalByOwnerSemantic;
    //00:00:00.4464116
    /** Услуги итого по провайдеру */
    var ProductTotalByProviderSemantic = (function (_super) {
        __extends(ProductTotalByProviderSemantic, _super);
        function ProductTotalByProviderSemantic() {
            _super.call(this);
            //00:00:00.4465526
            this._ProductTotalByProvider = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Услуги итого по провайдеру", ruShort: "По провайдеру" })
                .lookup(function () { return Luxena.sd.ProductTotalByProvider; });
            /** № */
            this.Rank = this.member()
                .localizeTitle({ ru: "№" })
                .int()
                .required();
            this.ProviderName = this.member()
                .string();
            this.Provider = this.member()
                .lookup(function () { return Luxena.sd.Organization; });
            this._isAbstract = false;
            this._name = "ProductTotalByProvider";
            this._names = "ProductTotalByProviders";
            this._isQueryResult = true;
            this._localizeTitle({ ru: "Услуги итого по провайдеру", ruShort: "По провайдеру" });
            this._getDerivedEntities = null;
            this._className = "ProductTotalByProvider";
            this._getRootEntity = function () { return Luxena.sd.ProductTotalByProvider; };
            this._store = Luxena.db.ProductTotalByProviders;
            this._saveStore = Luxena.db.ProductTotalByProviders;
            this._referenceFields = { id: "", name: "" };
            this.Provider
                .localizeTitle({ ru: "Провайдер" })
                .lookup(function () { return Luxena.sd.Organization; });
        }
        ProductTotalByProviderSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ProductTotalByProviderSemantic;
    })(ProductTotalSemantic);
    Luxena.ProductTotalByProviderSemantic = ProductTotalByProviderSemantic;
    //00:00:00.4470992
    /** Услуги итого поквартально */
    var ProductTotalByQuarterSemantic = (function (_super) {
        __extends(ProductTotalByQuarterSemantic, _super);
        function ProductTotalByQuarterSemantic() {
            _super.call(this);
            //00:00:00.4471747
            this._ProductTotalByQuarter = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Услуги итого поквартально", ruShort: "Поквартально" })
                .lookup(function () { return Luxena.sd.ProductTotalByQuarter; });
            /** Квартал */
            this.IssueDate = this.member()
                .localizeTitle({ ru: "Квартал" })
                .quarterAndYear()
                .required();
            this._isAbstract = false;
            this._name = "ProductTotalByQuarter";
            this._names = "ProductTotalByQuarters";
            this._isQueryResult = true;
            this._localizeTitle({ ru: "Услуги итого поквартально", ruShort: "Поквартально" });
            this._getDerivedEntities = null;
            this._className = "ProductTotalByQuarter";
            this._getRootEntity = function () { return Luxena.sd.ProductTotalByQuarter; };
            this._store = Luxena.db.ProductTotalByQuarters;
            this._saveStore = Luxena.db.ProductTotalByQuarters;
            this._referenceFields = { id: "", name: "" };
        }
        ProductTotalByQuarterSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ProductTotalByQuarterSemantic;
    })(ProductTotalSemantic);
    Luxena.ProductTotalByQuarterSemantic = ProductTotalByQuarterSemantic;
    //00:00:00.4482342
    /** Услуги итого по продавцу */
    var ProductTotalBySellerSemantic = (function (_super) {
        __extends(ProductTotalBySellerSemantic, _super);
        function ProductTotalBySellerSemantic() {
            _super.call(this);
            //00:00:00.4483993
            this._ProductTotalBySeller = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Услуги итого по продавцу", ruShort: "По продавцу" })
                .lookup(function () { return Luxena.sd.ProductTotalBySeller; });
            /** № */
            this.Rank = this.member()
                .localizeTitle({ ru: "№" })
                .int()
                .required();
            this.SellerName = this.member()
                .string();
            this.Seller = this.member()
                .lookup(function () { return Luxena.sd.Person; });
            this._isAbstract = false;
            this._name = "ProductTotalBySeller";
            this._names = "ProductTotalBySellers";
            this._isQueryResult = true;
            this._localizeTitle({ ru: "Услуги итого по продавцу", ruShort: "По продавцу" });
            this._getDerivedEntities = null;
            this._className = "ProductTotalBySeller";
            this._getRootEntity = function () { return Luxena.sd.ProductTotalBySeller; };
            this._store = Luxena.db.ProductTotalBySellers;
            this._saveStore = Luxena.db.ProductTotalBySellers;
            this._referenceFields = { id: "", name: "" };
            this.Seller
                .localizeTitle({ ru: "Продавец", rus: "Агенты" })
                .lookup(function () { return Luxena.sd.Person; });
        }
        ProductTotalBySellerSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ProductTotalBySellerSemantic;
    })(ProductTotalSemantic);
    Luxena.ProductTotalBySellerSemantic = ProductTotalBySellerSemantic;
    //00:00:00.4505226
    /** Услуги итого по видам услуг */
    var ProductTotalByTypeSemantic = (function (_super) {
        __extends(ProductTotalByTypeSemantic, _super);
        function ProductTotalByTypeSemantic() {
            _super.call(this);
            //00:00:00.4507282
            this._ProductTotalByType = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Услуги итого по видам услуг", ruShort: "По видам услуг" })
                .lookup(function () { return Luxena.sd.ProductTotalByType; });
            /** № */
            this.Rank = this.member()
                .localizeTitle({ ru: "№" })
                .int()
                .required();
            this.Type = this.member()
                .localizeTitle({ ru: "Вид услуги" })
                .enum(Luxena.ProductType)
                .required()
                .length(12, 0, 0);
            this.TypeName = this.member()
                .string();
            this._isAbstract = false;
            this._name = "ProductTotalByType";
            this._names = "ProductTotalByTypes";
            this._isQueryResult = true;
            this._localizeTitle({ ru: "Услуги итого по видам услуг", ruShort: "По видам услуг" });
            this._getDerivedEntities = null;
            this._className = "ProductTotalByType";
            this._getRootEntity = function () { return Luxena.sd.ProductTotalByType; };
            this._store = Luxena.db.ProductTotalByTypes;
            this._saveStore = Luxena.db.ProductTotalByTypes;
            this._referenceFields = { id: "", name: "" };
            this.Type
                .localizeTitle({ ru: "Вид услуги" })
                .required()
                .length(20, 0, 0)
                .entityType();
        }
        ProductTotalByTypeSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ProductTotalByTypeSemantic;
    })(ProductTotalSemantic);
    Luxena.ProductTotalByTypeSemantic = ProductTotalByTypeSemantic;
    //00:00:00.4513195
    /** Услуги итого по годам */
    var ProductTotalByYearSemantic = (function (_super) {
        __extends(ProductTotalByYearSemantic, _super);
        function ProductTotalByYearSemantic() {
            _super.call(this);
            //00:00:00.4513950
            this._ProductTotalByYear = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Услуги итого по годам", ruShort: "По годам" })
                .lookup(function () { return Luxena.sd.ProductTotalByYear; });
            /** Год */
            this.Year = this.member()
                .localizeTitle({ ru: "Год" })
                .int()
                .required();
            this._isAbstract = false;
            this._name = "ProductTotalByYear";
            this._names = "ProductTotalByYears";
            this._isQueryResult = true;
            this._localizeTitle({ ru: "Услуги итого по годам", ruShort: "По годам" });
            this._getDerivedEntities = null;
            this._className = "ProductTotalByYear";
            this._getRootEntity = function () { return Luxena.sd.ProductTotalByYear; };
            this._store = Luxena.db.ProductTotalByYears;
            this._saveStore = Luxena.db.ProductTotalByYears;
            this._referenceFields = { id: "Year", name: "" };
        }
        ProductTotalByYearSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ProductTotalByYearSemantic;
    })(ProductTotalSemantic);
    Luxena.ProductTotalByYearSemantic = ProductTotalByYearSemantic;
    //00:00:00.4528020
    /** Распределение выручки по заказчикам */
    var ProfitDistributionByCustomerSemantic = (function (_super) {
        __extends(ProfitDistributionByCustomerSemantic, _super);
        function ProfitDistributionByCustomerSemantic() {
            _super.call(this);
            //00:00:00.4529620
            this._ProfitDistributionByCustomer = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Распределение выручки по заказчикам" })
                .lookup(function () { return Luxena.sd.ProfitDistributionByCustomer; });
            this.Customer = this.member()
                .lookup(function () { return Luxena.sd.Party; })
                .entityName();
            this._isAbstract = false;
            this._name = "ProfitDistributionByCustomer";
            this._names = "ProfitDistributionByCustomers";
            this._isQueryResult = true;
            this._localizeTitle({ ru: "Распределение выручки по заказчикам" });
            this._getDerivedEntities = null;
            this._className = "ProfitDistributionByCustomer";
            this._getRootEntity = function () { return Luxena.sd.ProfitDistributionByCustomer; };
            this._store = Luxena.db.ProfitDistributionByCustomers;
            this._saveStore = Luxena.db.ProfitDistributionByCustomers;
            this._referenceFields = { id: "", name: "" };
            this.Customer
                .localizeTitle({ ru: "Заказчик", rus: "Заказчики" })
                .lookup(function () { return Luxena.sd.Party; });
        }
        ProfitDistributionByCustomerSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ProfitDistributionByCustomerSemantic;
    })(ProfitDistributionTotalSemantic);
    Luxena.ProfitDistributionByCustomerSemantic = ProfitDistributionByCustomerSemantic;
    //00:00:00.4543451
    /** Распределение выручки по провайдерам */
    var ProfitDistributionByProviderSemantic = (function (_super) {
        __extends(ProfitDistributionByProviderSemantic, _super);
        function ProfitDistributionByProviderSemantic() {
            _super.call(this);
            //00:00:00.4544857
            this._ProfitDistributionByProvider = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Распределение выручки по провайдерам" })
                .lookup(function () { return Luxena.sd.ProfitDistributionByProvider; });
            this.Provider = this.member()
                .lookup(function () { return Luxena.sd.Organization; });
            this._isAbstract = false;
            this._name = "ProfitDistributionByProvider";
            this._names = "ProfitDistributionByProviders";
            this._isQueryResult = true;
            this._localizeTitle({ ru: "Распределение выручки по провайдерам" });
            this._getDerivedEntities = null;
            this._className = "ProfitDistributionByProvider";
            this._getRootEntity = function () { return Luxena.sd.ProfitDistributionByProvider; };
            this._store = Luxena.db.ProfitDistributionByProviders;
            this._saveStore = Luxena.db.ProfitDistributionByProviders;
            this._referenceFields = { id: "", name: "" };
            this.Provider
                .localizeTitle({ ru: "Провайдер" })
                .lookup(function () { return Luxena.sd.Organization; });
        }
        ProfitDistributionByProviderSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ProfitDistributionByProviderSemantic;
    })(ProfitDistributionTotalSemantic);
    Luxena.ProfitDistributionByProviderSemantic = ProfitDistributionByProviderSemantic;
    //00:00:00.4574379
    var ProductFilterSemantic = (function (_super) {
        __extends(ProductFilterSemantic, _super);
        function ProductFilterSemantic() {
            _super.call(this);
            //00:00:00.4587776
            this.Provider = this.member()
                .lookup(function () { return Luxena.sd.Organization; });
            this.IssueDate = this.member()
                .date()
                .entityName();
            this.Seller = this.member()
                .lookup(function () { return Luxena.sd.Person; });
            this.IssueMonth = this.member()
                .monthAndYear();
            this.MinIssueDate = this.member()
                .date();
            this.MaxIssueDate = this.member()
                .date();
            this.Type = this.member()
                .localizeTitle({ ru: "Вид услуги" })
                .enum(Luxena.ProductType).enumIsFlags()
                .length(12, 0, 0);
            /** Название услуги */
            this.Name = this.member()
                .localizeTitle({ ru: "Название услуги" })
                .string();
            this.State = this.member()
                .localizeTitle({ ru: "Статус услуги" })
                .enum(Luxena.ProductStateFilter)
                .required();
            /** Валюта услуги */
            this.ProductCurrency = this.member()
                .localizeTitle({ ru: "Валюта услуги" })
                .currencyCode();
            this.Customer = this.member()
                .lookup(function () { return Luxena.sd.Party; });
            this.Booker = this.member()
                .lookup(function () { return Luxena.sd.Person; });
            this.Ticketer = this.member()
                .lookup(function () { return Luxena.sd.Person; });
            this.Owner = this.member()
                .lookup(function () { return Luxena.sd.Party; });
            this.AllowVoided = this.member()
                .bool()
                .required();
            this._isAbstract = false;
            this._name = "ProductFilter";
            this._names = "ProductFilter";
            this._isQueryParams = true;
            this._getDerivedEntities = function () { return [
                Luxena.sd.EverydayProfitReportParams, Luxena.sd.FlownReportParams, Luxena.sd.ProductSummaryParams, Luxena.sd.ProductTotalByBookerParams, Luxena.sd.ProductTotalByDayParams, Luxena.sd.ProductTotalByMonthParams, Luxena.sd.ProductTotalByOwnerParams, Luxena.sd.ProductTotalByProviderParams, Luxena.sd.ProductTotalByQuarterParams, Luxena.sd.ProductTotalBySellerParams, Luxena.sd.ProductTotalByTypeParams, Luxena.sd.ProductTotalByYearParams, Luxena.sd.ProfitDistributionByCustomerParams, Luxena.sd.ProfitDistributionByProviderParams
            ]; };
            this.IssueDate
                .localizeTitle({ ru: "Дата выпуска" })
                .entityDate();
            this.IssueMonth
                .localizeTitle({ ru: "Дата выпуска" })
                .entityDate();
            this.MinIssueDate
                .localizeTitle({ ru: "Дата выпуска" })
                .entityDate();
            this.MaxIssueDate
                .localizeTitle({ ru: "Дата выпуска" })
                .entityDate();
            this.Type
                .localizeTitle({ ru: "Вид услуги" })
                .length(12, 0, 0)
                .entityType();
            this.Provider
                .localizeTitle({ ru: "Провайдер" })
                .lookup(function () { return Luxena.sd.Organization; });
            this.Customer
                .localizeTitle({ ru: "Заказчик", rus: "Заказчики" })
                .lookup(function () { return Luxena.sd.Party; });
            this.Booker
                .localizeTitle({ ru: "Бронировщик" })
                .lookup(function () { return Luxena.sd.Person; });
            this.Ticketer
                .localizeTitle({ ru: "Тикетер" })
                .lookup(function () { return Luxena.sd.Person; });
            this.Seller
                .localizeTitle({ ru: "Продавец", rus: "Агенты" })
                .lookup(function () { return Luxena.sd.Person; });
            this.Owner
                .localizeTitle({ ru: "Владелец" })
                .lookup(function () { return Luxena.sd.Party; });
        }
        ProductFilterSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ProductFilterSemantic;
    })(Luxena.SemanticEntity);
    Luxena.ProductFilterSemantic = ProductFilterSemantic;
    //00:00:00.4634127
    var EverydayProfitReportParamsSemantic = (function (_super) {
        __extends(EverydayProfitReportParamsSemantic, _super);
        function EverydayProfitReportParamsSemantic() {
            _super.call(this);
            //00:00:00.4634504
            /** Вид услуги */
            this.ProductType = this.member()
                .localizeTitle({ ru: "Вид услуги" })
                .enum(Luxena.ProductType)
                .required()
                .length(12, 0, 0);
            /** Услуга */
            this.Product = this.member()
                .localizeTitle({ ru: "Услуга", rus: "Все услуги" })
                .lookup(function () { return Luxena.sd.Product; });
            this.PassengerName = this.member()
                .string();
            this.Itinerary = this.member()
                .string();
            this.StartDate = this.member()
                .date();
            this.FinishDate = this.member()
                .date();
            /** Страна */
            this.Country = this.member()
                .localizeTitle({ ru: "Страна", rus: "Страны" })
                .lookup(function () { return Luxena.sd.Country; });
            this.Fare = this.member()
                .money();
            this.Currency = this.member()
                .string();
            this.CurrencyRate = this.member()
                .float();
            this.EqualFare = this.member()
                .float();
            this.FeesTotal = this.member()
                .float();
            this.CancelFee = this.member()
                .float();
            this.Total = this.member()
                .float();
            this.Commission = this.member()
                .float();
            this.ServiceFee = this.member()
                .float();
            this.Vat = this.member()
                .float();
            this.GrandTotal = this.member()
                .float();
            /** Заказ */
            this.Order = this.member()
                .localizeTitle({ ru: "Заказ", rus: "Заказы" })
                .lookup(function () { return Luxena.sd.Order; });
            this.Payer = this.member()
                .lookup(function () { return Luxena.sd.Party; });
            /** Инвойс */
            this.Invoice = this.member()
                .localizeTitle({ ru: "Инвойс", rus: "Счета/квитанции" })
                .lookup(function () { return Luxena.sd.Invoice; });
            this.InvoiceDate = this.member()
                .date();
            this.CompletionCertificate = this.member()
                .lookup(function () { return Luxena.sd.Invoice; });
            this.CompletionCertificateDate = this.member()
                .date();
            /** Платёж */
            this.Payment = this.member()
                .localizeTitle({ ru: "Платёж", rus: "Платежи" })
                .lookup(function () { return Luxena.sd.Payment; });
            this.PaymentDate = this.member()
                .date();
            this._isAbstract = false;
            this._name = "EverydayProfitReportParams";
            this._names = "EverydayProfitReportParams";
            this._isQueryParams = true;
            this._getDerivedEntities = null;
        }
        EverydayProfitReportParamsSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return EverydayProfitReportParamsSemantic;
    })(ProductFilterSemantic);
    Luxena.EverydayProfitReportParamsSemantic = EverydayProfitReportParamsSemantic;
    //00:00:00.4675421
    var FlownReportParamsSemantic = (function (_super) {
        __extends(FlownReportParamsSemantic, _super);
        function FlownReportParamsSemantic() {
            _super.call(this);
            //00:00:00.4675771
            this.Date = this.member()
                .date()
                .required();
            this.Op = this.member()
                .string();
            this.AC = this.member()
                .string();
            this.TicketNumber = this.member()
                .lookup(function () { return Luxena.sd.AviaDocument; });
            this.Client = this.member()
                .lookup(function () { return Luxena.sd.Party; });
            this.Passenger = this.member()
                .string();
            this.Route = this.member()
                .string();
            this.Curr = this.member()
                .string();
            this.Fare = this.member()
                .float();
            this.Tax = this.member()
                .float();
            this.Flown1 = this.member()
                .float();
            this.Flown2 = this.member()
                .float();
            this.Flown3 = this.member()
                .float();
            this.Flown4 = this.member()
                .float();
            this.Flown5 = this.member()
                .float();
            this.Flown6 = this.member()
                .float();
            this.Flown7 = this.member()
                .float();
            this.Flown8 = this.member()
                .float();
            this.Flown9 = this.member()
                .float();
            this.Flown10 = this.member()
                .float();
            this.Flown11 = this.member()
                .float();
            this.Flown12 = this.member()
                .float();
            this.TourCode = this.member()
                .string();
            this.CheapTicket = this.member()
                .lookup(function () { return Luxena.sd.AviaDocument; });
            this._isAbstract = false;
            this._name = "FlownReportParams";
            this._names = "FlownReportParams";
            this._isQueryParams = true;
            this._getDerivedEntities = null;
        }
        FlownReportParamsSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return FlownReportParamsSemantic;
    })(ProductFilterSemantic);
    Luxena.FlownReportParamsSemantic = FlownReportParamsSemantic;
    //00:00:00.4701173
    var ProductSummaryParamsSemantic = (function (_super) {
        __extends(ProductSummaryParamsSemantic, _super);
        function ProductSummaryParamsSemantic() {
            _super.call(this);
            //00:00:00.4701520
            this.Itinerary = this.member()
                .string();
            this.IsRefund = this.member()
                .bool()
                .required();
            this.Total = this.member()
                .money();
            this.ServiceFee = this.member()
                .money();
            this.GrandTotal = this.member()
                .money();
            /** Заказ */
            this.Order = this.member()
                .localizeTitle({ ru: "Заказ", rus: "Заказы" })
                .lookup(function () { return Luxena.sd.Order; });
            this._isAbstract = false;
            this._name = "ProductSummaryParams";
            this._names = "ProductSummaryParams";
            this._isQueryParams = true;
            this._getDerivedEntities = null;
        }
        ProductSummaryParamsSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ProductSummaryParamsSemantic;
    })(ProductFilterSemantic);
    Luxena.ProductSummaryParamsSemantic = ProductSummaryParamsSemantic;
    //00:00:00.4720927
    var ProductTotalByBookerParamsSemantic = (function (_super) {
        __extends(ProductTotalByBookerParamsSemantic, _super);
        function ProductTotalByBookerParamsSemantic() {
            _super.call(this);
            //00:00:00.4721289
            this.Rank = this.member()
                .int()
                .required();
            this.BookerName = this.member()
                .string();
            this.Total = this.member()
                .float();
            this.ServiceFee = this.member()
                .float();
            this.GrandTotal = this.member()
                .float();
            this.Note = this.member()
                .string();
            this._isAbstract = false;
            this._name = "ProductTotalByBookerParams";
            this._names = "ProductTotalByBookerParams";
            this._isQueryParams = true;
            this._getDerivedEntities = null;
        }
        ProductTotalByBookerParamsSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ProductTotalByBookerParamsSemantic;
    })(ProductFilterSemantic);
    Luxena.ProductTotalByBookerParamsSemantic = ProductTotalByBookerParamsSemantic;
    //00:00:00.4742111
    var ProductTotalByDayParamsSemantic = (function (_super) {
        __extends(ProductTotalByDayParamsSemantic, _super);
        function ProductTotalByDayParamsSemantic() {
            _super.call(this);
            //00:00:00.4742491
            this.Total = this.member()
                .float();
            this.ServiceFee = this.member()
                .float();
            this.GrandTotal = this.member()
                .float();
            this.Note = this.member()
                .string();
            this._isAbstract = false;
            this._name = "ProductTotalByDayParams";
            this._names = "ProductTotalByDayParams";
            this._isQueryParams = true;
            this._getDerivedEntities = null;
        }
        ProductTotalByDayParamsSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ProductTotalByDayParamsSemantic;
    })(ProductFilterSemantic);
    Luxena.ProductTotalByDayParamsSemantic = ProductTotalByDayParamsSemantic;
    //00:00:00.4758457
    var ProductTotalByMonthParamsSemantic = (function (_super) {
        __extends(ProductTotalByMonthParamsSemantic, _super);
        function ProductTotalByMonthParamsSemantic() {
            _super.call(this);
            //00:00:00.4758828
            this.Total = this.member()
                .float();
            this.ServiceFee = this.member()
                .float();
            this.GrandTotal = this.member()
                .float();
            this.Note = this.member()
                .string();
            this._isAbstract = false;
            this._name = "ProductTotalByMonthParams";
            this._names = "ProductTotalByMonthParams";
            this._isQueryParams = true;
            this._getDerivedEntities = null;
        }
        ProductTotalByMonthParamsSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ProductTotalByMonthParamsSemantic;
    })(ProductFilterSemantic);
    Luxena.ProductTotalByMonthParamsSemantic = ProductTotalByMonthParamsSemantic;
    //00:00:00.4775832
    var ProductTotalByOwnerParamsSemantic = (function (_super) {
        __extends(ProductTotalByOwnerParamsSemantic, _super);
        function ProductTotalByOwnerParamsSemantic() {
            _super.call(this);
            //00:00:00.4776206
            this.Rank = this.member()
                .int()
                .required();
            this.OwnerName = this.member()
                .string();
            this.Total = this.member()
                .float();
            this.ServiceFee = this.member()
                .float();
            this.GrandTotal = this.member()
                .float();
            this.Note = this.member()
                .string();
            this._isAbstract = false;
            this._name = "ProductTotalByOwnerParams";
            this._names = "ProductTotalByOwnerParams";
            this._isQueryParams = true;
            this._getDerivedEntities = null;
        }
        ProductTotalByOwnerParamsSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ProductTotalByOwnerParamsSemantic;
    })(ProductFilterSemantic);
    Luxena.ProductTotalByOwnerParamsSemantic = ProductTotalByOwnerParamsSemantic;
    //00:00:00.4793358
    var ProductTotalByProviderParamsSemantic = (function (_super) {
        __extends(ProductTotalByProviderParamsSemantic, _super);
        function ProductTotalByProviderParamsSemantic() {
            _super.call(this);
            //00:00:00.4793708
            this.Rank = this.member()
                .int()
                .required();
            this.ProviderName = this.member()
                .string();
            this.Total = this.member()
                .float();
            this.ServiceFee = this.member()
                .float();
            this.GrandTotal = this.member()
                .float();
            this.Note = this.member()
                .string();
            this._isAbstract = false;
            this._name = "ProductTotalByProviderParams";
            this._names = "ProductTotalByProviderParams";
            this._isQueryParams = true;
            this._getDerivedEntities = null;
        }
        ProductTotalByProviderParamsSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ProductTotalByProviderParamsSemantic;
    })(ProductFilterSemantic);
    Luxena.ProductTotalByProviderParamsSemantic = ProductTotalByProviderParamsSemantic;
    //00:00:00.4809601
    var ProductTotalByQuarterParamsSemantic = (function (_super) {
        __extends(ProductTotalByQuarterParamsSemantic, _super);
        function ProductTotalByQuarterParamsSemantic() {
            _super.call(this);
            //00:00:00.4809945
            this.Total = this.member()
                .float();
            this.ServiceFee = this.member()
                .float();
            this.GrandTotal = this.member()
                .float();
            this.Note = this.member()
                .string();
            this._isAbstract = false;
            this._name = "ProductTotalByQuarterParams";
            this._names = "ProductTotalByQuarterParams";
            this._isQueryParams = true;
            this._getDerivedEntities = null;
        }
        ProductTotalByQuarterParamsSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ProductTotalByQuarterParamsSemantic;
    })(ProductFilterSemantic);
    Luxena.ProductTotalByQuarterParamsSemantic = ProductTotalByQuarterParamsSemantic;
    //00:00:00.4853630
    var ProductTotalBySellerParamsSemantic = (function (_super) {
        __extends(ProductTotalBySellerParamsSemantic, _super);
        function ProductTotalBySellerParamsSemantic() {
            _super.call(this);
            //00:00:00.4854255
            this.Rank = this.member()
                .int()
                .required();
            this.SellerName = this.member()
                .string();
            this.Total = this.member()
                .float();
            this.ServiceFee = this.member()
                .float();
            this.GrandTotal = this.member()
                .float();
            this.Note = this.member()
                .string();
            this._isAbstract = false;
            this._name = "ProductTotalBySellerParams";
            this._names = "ProductTotalBySellerParams";
            this._isQueryParams = true;
            this._getDerivedEntities = null;
        }
        ProductTotalBySellerParamsSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ProductTotalBySellerParamsSemantic;
    })(ProductFilterSemantic);
    Luxena.ProductTotalBySellerParamsSemantic = ProductTotalBySellerParamsSemantic;
    //00:00:00.4871516
    var ProductTotalByTypeParamsSemantic = (function (_super) {
        __extends(ProductTotalByTypeParamsSemantic, _super);
        function ProductTotalByTypeParamsSemantic() {
            _super.call(this);
            //00:00:00.4871875
            this.Rank = this.member()
                .int()
                .required();
            this.TypeName = this.member()
                .string();
            this.Total = this.member()
                .float();
            this.ServiceFee = this.member()
                .float();
            this.GrandTotal = this.member()
                .float();
            this.Note = this.member()
                .string();
            this._isAbstract = false;
            this._name = "ProductTotalByTypeParams";
            this._names = "ProductTotalByTypeParams";
            this._isQueryParams = true;
            this._getDerivedEntities = null;
        }
        ProductTotalByTypeParamsSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ProductTotalByTypeParamsSemantic;
    })(ProductFilterSemantic);
    Luxena.ProductTotalByTypeParamsSemantic = ProductTotalByTypeParamsSemantic;
    //00:00:00.4891149
    var ProductTotalByYearParamsSemantic = (function (_super) {
        __extends(ProductTotalByYearParamsSemantic, _super);
        function ProductTotalByYearParamsSemantic() {
            _super.call(this);
            //00:00:00.4891882
            this.Year = this.member()
                .int()
                .required();
            this.Total = this.member()
                .float();
            this.ServiceFee = this.member()
                .float();
            this.GrandTotal = this.member()
                .float();
            this.Note = this.member()
                .string();
            this._isAbstract = false;
            this._name = "ProductTotalByYearParams";
            this._names = "ProductTotalByYearParams";
            this._isQueryParams = true;
            this._getDerivedEntities = null;
        }
        ProductTotalByYearParamsSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ProductTotalByYearParamsSemantic;
    })(ProductFilterSemantic);
    Luxena.ProductTotalByYearParamsSemantic = ProductTotalByYearParamsSemantic;
    //00:00:00.4912587
    var ProfitDistributionByCustomerParamsSemantic = (function (_super) {
        __extends(ProfitDistributionByCustomerParamsSemantic, _super);
        function ProfitDistributionByCustomerParamsSemantic() {
            _super.call(this);
            //00:00:00.4912979
            this.Rank = this.member()
                .int()
                .required();
            this.SellCount = this.member()
                .int()
                .required();
            this.RefundCount = this.member()
                .int()
                .required();
            this.VoidCount = this.member()
                .int()
                .required();
            this.Currency = this.member()
                .string();
            this.SellGrandTotal = this.member()
                .float();
            this.RefundGrandTotal = this.member()
                .float();
            this.GrandTotal = this.member()
                .float();
            this.Total = this.member()
                .float();
            this.ServiceFee = this.member()
                .float();
            this.Commission = this.member()
                .float();
            this.AgentTotal = this.member()
                .float();
            this.Vat = this.member()
                .float();
            this._isAbstract = false;
            this._name = "ProfitDistributionByCustomerParams";
            this._names = "ProfitDistributionByCustomerParams";
            this._isQueryParams = true;
            this._getDerivedEntities = null;
        }
        ProfitDistributionByCustomerParamsSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ProfitDistributionByCustomerParamsSemantic;
    })(ProductFilterSemantic);
    Luxena.ProfitDistributionByCustomerParamsSemantic = ProfitDistributionByCustomerParamsSemantic;
    //00:00:00.4936660
    var ProfitDistributionByProviderParamsSemantic = (function (_super) {
        __extends(ProfitDistributionByProviderParamsSemantic, _super);
        function ProfitDistributionByProviderParamsSemantic() {
            _super.call(this);
            //00:00:00.4937040
            this.Rank = this.member()
                .int()
                .required();
            this.SellCount = this.member()
                .int()
                .required();
            this.RefundCount = this.member()
                .int()
                .required();
            this.VoidCount = this.member()
                .int()
                .required();
            this.Currency = this.member()
                .string();
            this.SellGrandTotal = this.member()
                .float();
            this.RefundGrandTotal = this.member()
                .float();
            this.GrandTotal = this.member()
                .float();
            this.Total = this.member()
                .float();
            this.ServiceFee = this.member()
                .float();
            this.Commission = this.member()
                .float();
            this.AgentTotal = this.member()
                .float();
            this.Vat = this.member()
                .float();
            this._isAbstract = false;
            this._name = "ProfitDistributionByProviderParams";
            this._names = "ProfitDistributionByProviderParams";
            this._isQueryParams = true;
            this._getDerivedEntities = null;
        }
        ProfitDistributionByProviderParamsSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return ProfitDistributionByProviderParamsSemantic;
    })(ProductFilterSemantic);
    Luxena.ProfitDistributionByProviderParamsSemantic = ProfitDistributionByProviderParamsSemantic;
    //00:00:00.4947455
    /** Применить к документам */
    var GdsAgent_ApplyToUnassignedSemantic = (function (_super) {
        __extends(GdsAgent_ApplyToUnassignedSemantic, _super);
        function GdsAgent_ApplyToUnassignedSemantic() {
            _super.call(this);
            //00:00:00.4948191
            this._GdsAgent_ApplyToUnassigned = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Применить к документам" })
                .lookup(function () { return Luxena.sd.GdsAgent_ApplyToUnassigned; });
            /** С даты */
            this.DateFrom = this.member()
                .localizeTitle({ ru: "С даты" })
                .date()
                .subject();
            /** По дату */
            this.DateTo = this.member()
                .localizeTitle({ ru: "По дату" })
                .date()
                .subject();
            /** Кол-во несвязанных услуг */
            this.ProductCount = this.member()
                .localizeTitle({ ru: "Кол-во несвязанных услуг" })
                .int()
                .readOnly()
                .nonsaved();
            /** Gds-агент */
            this.GdsAgent = this.member()
                .localizeTitle({ ru: "Gds-агент", rus: "Gds-агенты" })
                .lookup(function () { return Luxena.sd.GdsAgent; })
                .required()
                .subject();
            this._isAbstract = false;
            this._name = "GdsAgent_ApplyToUnassigned";
            this._names = "GdsAgent_ApplyToUnassigned";
            this._isDomainAction = true;
            this._localizeTitle({ ru: "Применить к документам" });
            this._getDerivedEntities = null;
            this._className = "GdsAgent_ApplyToUnassigned";
            this._getRootEntity = function () { return Luxena.sd.GdsAgent_ApplyToUnassigned; };
            this._store = Luxena.db.GdsAgent_ApplyToUnassigned;
            this._saveStore = Luxena.db.GdsAgent_ApplyToUnassigned;
            this._referenceFields = { id: "Id", name: "" };
        }
        GdsAgent_ApplyToUnassignedSemantic.prototype.clone = function (cfg) {
            return $.extend({}, this, cfg);
        };
        return GdsAgent_ApplyToUnassignedSemantic;
    })(DomainActionSemantic);
    Luxena.GdsAgent_ApplyToUnassignedSemantic = GdsAgent_ApplyToUnassignedSemantic;
    //00:00:00.4951192
    var DomainSemantic = (function (_super) {
        __extends(DomainSemantic, _super);
        function DomainSemantic() {
            _super.apply(this, arguments);
            /** Проживание */
            this.Accommodation = this.entity(new AccommodationSemantic());
            /** Провайдер проживания */
            this.AccommodationProvider = this.entity(new AccommodationProviderSemantic());
            /** Тип проживания */
            this.AccommodationType = this.entity(new AccommodationTypeSemantic());
            /** Владелец документов (активный) */
            this.ActiveOwner = this.entity(new ActiveOwnerSemantic());
            /** Агент */
            this.Agent = this.entity(new AgentSemantic());
            /** Авиакомпания */
            this.Airline = this.entity(new AirlineSemantic());
            /** Сервис-класс авиакомпании */
            this.AirlineServiceClass = this.entity(new AirlineServiceClassSemantic());
            /** Аэропорт */
            this.Airport = this.entity(new AirportSemantic());
            /** Авиадокумент */
            this.AviaDocument = this.entity(new AviaDocumentSemantic());
            /** МСО */
            this.AviaMco = this.entity(new AviaMcoSemantic());
            /** Возврат авиабилета */
            this.AviaRefund = this.entity(new AviaRefundSemantic());
            /** Авиабилет */
            this.AviaTicket = this.entity(new AviaTicketSemantic());
            /** Банковский счёт */
            this.BankAccount = this.entity(new BankAccountSemantic());
            /** Автобусный билет или возврат */
            this.BusDocument = this.entity(new BusDocumentSemantic());
            /** Автобусный билет */
            this.BusTicket = this.entity(new BusTicketSemantic());
            /** Провайдер автобусных билетов */
            this.BusTicketProvider = this.entity(new BusTicketProviderSemantic());
            /** Возврат автобусного билета */
            this.BusTicketRefund = this.entity(new BusTicketRefundSemantic());
            /** Аренда автомобиля */
            this.CarRental = this.entity(new CarRentalSemantic());
            /** Провайдер аренды авто */
            this.CarRentalProvider = this.entity(new CarRentalProviderSemantic());
            /** ПКО */
            this.CashInOrderPayment = this.entity(new CashInOrderPaymentSemantic());
            /** РКО */
            this.CashOutOrderPayment = this.entity(new CashOutOrderPaymentSemantic());
            /** Тип питания */
            this.CateringType = this.entity(new CateringTypeSemantic());
            /** Кассовый чек */
            this.CheckPayment = this.entity(new CheckPaymentSemantic());
            /** Накладная */
            this.Consignment = this.entity(new ConsignmentSemantic());
            /** Страна */
            this.Country = this.entity(new CountrySemantic());
            /** Курс валюты */
            this.CurrencyDailyRate = this.entity(new CurrencyDailyRateSemantic());
            /** Заказчик */
            this.Customer = this.entity(new CustomerSemantic());
            /** Подразделение */
            this.Department = this.entity(new DepartmentSemantic());
            /** Доступ к документам */
            this.DocumentAccess = this.entity(new DocumentAccessSemantic());
            /** Владелец документов */
            this.DocumentOwner = this.entity(new DocumentOwnerSemantic());
            /** Электронный платеж */
            this.ElectronicPayment = this.entity(new ElectronicPaymentSemantic());
            /** Ежедневный отчет по выручке */
            this.EverydayProfitReport = this.entity(new EverydayProfitReportSemantic());
            this.EverydayProfitReportParams = this.entity(new EverydayProfitReportParamsSemantic());
            /** Экскурсия */
            this.Excursion = this.entity(new ExcursionSemantic());
            this.File = this.entity(new FileSemantic());
            /** Полетный сегмент */
            this.FlightSegment = this.entity(new FlightSegmentSemantic());
            /** Flown-отчет */
            this.FlownReport = this.entity(new FlownReportSemantic());
            this.FlownReportParams = this.entity(new FlownReportParamsSemantic());
            /** Gds-агент */
            this.GdsAgent = this.entity(new GdsAgentSemantic());
            /** Применить к документам */
            this.GdsAgent_ApplyToUnassigned = this.entity(new GdsAgent_ApplyToUnassignedSemantic());
            /** Gds-файл */
            this.GdsFile = this.entity(new GdsFileSemantic());
            /** Дополнительная услуга */
            this.GenericProduct = this.entity(new GenericProductSemantic());
            /** Провайдер дополнительных услуг */
            this.GenericProductProvider = this.entity(new GenericProductProviderSemantic());
            /** Вид дополнительной услуги */
            this.GenericProductType = this.entity(new GenericProductTypeSemantic());
            this.Identity = this.entity(new IdentitySemantic());
            /** Страховка */
            this.Insurance = this.entity(new InsuranceSemantic());
            /** Страховая компания */
            this.InsuranceCompany = this.entity(new InsuranceCompanySemantic());
            /** Страховка или возврат */
            this.InsuranceDocument = this.entity(new InsuranceDocumentSemantic());
            /** Возврат страховки */
            this.InsuranceRefund = this.entity(new InsuranceRefundSemantic());
            this.InternalIdentity = this.entity(new InternalIdentitySemantic());
            /** Внутренний перевод */
            this.InternalTransfer = this.entity(new InternalTransferSemantic());
            /** Инвойс */
            this.Invoice = this.entity(new InvoiceSemantic());
            /** Студенческий билет */
            this.Isic = this.entity(new IsicSemantic());
            /** Выпущенная накладная */
            this.IssuedConsignment = this.entity(new IssuedConsignmentSemantic());
            /** Мильная карта */
            this.MilesCard = this.entity(new MilesCardSemantic());
            /** Заказ */
            this.Order = this.entity(new OrderSemantic());
            /** Чек */
            this.OrderCheck = this.entity(new OrderCheckSemantic());
            /** Позиция заказа */
            this.OrderItem = this.entity(new OrderItemSemantic());
            /** Организация */
            this.Organization = this.entity(new OrganizationSemantic());
            /** Контрагент */
            this.Party = this.entity(new PartySemantic());
            /** Паспорт */
            this.Passport = this.entity(new PassportSemantic());
            /** Ж/д билет */
            this.Pasteboard = this.entity(new PasteboardSemantic());
            /** Провайдер ж/д билетов */
            this.PasteboardProvider = this.entity(new PasteboardProviderSemantic());
            /** Возврат ж/д билета */
            this.PasteboardRefund = this.entity(new PasteboardRefundSemantic());
            /** Платёж */
            this.Payment = this.entity(new PaymentSemantic());
            /** Платёжная система */
            this.PaymentSystem = this.entity(new PaymentSystemSemantic());
            /** Персона */
            this.Person = this.entity(new PersonSemantic());
            /** Услуга */
            this.Product = this.entity(new ProductSemantic());
            this.ProductFilter = this.entity(new ProductFilterSemantic());
            /** Пассажир */
            this.ProductPassenger = this.entity(new ProductPassengerSemantic());
            /** Сводка по услугам */
            this.ProductSummary = this.entity(new ProductSummarySemantic());
            this.ProductSummaryParams = this.entity(new ProductSummaryParamsSemantic());
            this.ProductTotal = this.entity(new ProductTotalSemantic());
            /** Услуги итого по бронировщику */
            this.ProductTotalByBooker = this.entity(new ProductTotalByBookerSemantic());
            this.ProductTotalByBookerParams = this.entity(new ProductTotalByBookerParamsSemantic());
            /** Услуги итого посуточно */
            this.ProductTotalByDay = this.entity(new ProductTotalByDaySemantic());
            this.ProductTotalByDayParams = this.entity(new ProductTotalByDayParamsSemantic());
            /** Услуги итого помесячно */
            this.ProductTotalByMonth = this.entity(new ProductTotalByMonthSemantic());
            this.ProductTotalByMonthParams = this.entity(new ProductTotalByMonthParamsSemantic());
            /** Услуги итого по владельцу */
            this.ProductTotalByOwner = this.entity(new ProductTotalByOwnerSemantic());
            this.ProductTotalByOwnerParams = this.entity(new ProductTotalByOwnerParamsSemantic());
            /** Услуги итого по провайдеру */
            this.ProductTotalByProvider = this.entity(new ProductTotalByProviderSemantic());
            this.ProductTotalByProviderParams = this.entity(new ProductTotalByProviderParamsSemantic());
            /** Услуги итого поквартально */
            this.ProductTotalByQuarter = this.entity(new ProductTotalByQuarterSemantic());
            this.ProductTotalByQuarterParams = this.entity(new ProductTotalByQuarterParamsSemantic());
            /** Услуги итого по продавцу */
            this.ProductTotalBySeller = this.entity(new ProductTotalBySellerSemantic());
            this.ProductTotalBySellerParams = this.entity(new ProductTotalBySellerParamsSemantic());
            /** Услуги итого по видам услуг */
            this.ProductTotalByType = this.entity(new ProductTotalByTypeSemantic());
            this.ProductTotalByTypeParams = this.entity(new ProductTotalByTypeParamsSemantic());
            /** Услуги итого по годам */
            this.ProductTotalByYear = this.entity(new ProductTotalByYearSemantic());
            this.ProductTotalByYearParams = this.entity(new ProductTotalByYearParamsSemantic());
            /** Распределение выручки по заказчикам */
            this.ProfitDistributionByCustomer = this.entity(new ProfitDistributionByCustomerSemantic());
            this.ProfitDistributionByCustomerParams = this.entity(new ProfitDistributionByCustomerParamsSemantic());
            /** Распределение выручки по провайдерам */
            this.ProfitDistributionByProvider = this.entity(new ProfitDistributionByProviderSemantic());
            this.ProfitDistributionByProviderParams = this.entity(new ProfitDistributionByProviderParamsSemantic());
            this.ProfitDistributionTotal = this.entity(new ProfitDistributionTotalSemantic());
            /** Ж/д билет или возврат */
            this.RailwayDocument = this.entity(new RailwayDocumentSemantic());
            /** Квитанция */
            this.Receipt = this.entity(new ReceiptSemantic());
            /** Мобильный оператор */
            this.RoamingOperator = this.entity(new RoamingOperatorSemantic());
            this.Sequence = this.entity(new SequenceSemantic());
            /** SIM-карта */
            this.SimCard = this.entity(new SimCardSemantic());
            /** Настройки системы */
            this.SystemConfiguration = this.entity(new SystemConfigurationSemantic());
            /** Турпакет */
            this.Tour = this.entity(new TourSemantic());
            /** Провайдер туров (готовых) */
            this.TourProvider = this.entity(new TourProviderSemantic());
            /** Трансфер */
            this.Transfer = this.entity(new TransferSemantic());
            /** Провайдер трансферов */
            this.TransferProvider = this.entity(new TransferProviderSemantic());
            /** Пользователь */
            this.User = this.entity(new UserSemantic());
            /** Безналичный платеж */
            this.WireTransfer = this.entity(new WireTransferSemantic());
        }
        return DomainSemantic;
    })(Luxena.SemanticDomain);
    Luxena.DomainSemantic = DomainSemantic;
    ;
    Luxena.sd = new DomainSemantic();
})(Luxena || (Luxena = {}));
//00:00:00.4962753 
var Luxena;
(function (Luxena) {
    function sameTitleMenuItems(entities) {
        entities.forEach(function (se) { return se.titleMenuItems(entities); });
        return entities;
    }
    Luxena.config.menu = [
        //		{
        //			icon: "navicon",
        //			title: "Домашняя страница",
        //			url: "home",
        //		},
        {
            icon: "shopping-cart",
            title: "Продажи",
            description: "Информация о заказах и платежах",
            items: [
                Luxena.sd.Order,
                Luxena.sd.Consignment,
                Luxena.sd.IssuedConsignment,
                Luxena.sd.Invoice,
                Luxena.sd.Payment,
                Luxena.sd.InternalTransfer,
                Luxena.sd.OrderCheck,
                Luxena.sd.CurrencyDailyRate,
            ],
        },
        {
            icon: "suitcase",
            title: "Услуги",
            items: [
                Luxena.sd.Product.titleMenuItems([
                    Luxena.sd.Product,
                    Luxena.sd.AviaDocument,
                    Luxena.sd.BusDocument,
                    Luxena.sd.CarRental,
                    Luxena.sd.RailwayDocument,
                    Luxena.sd.Accommodation,
                    Luxena.sd.InsuranceDocument,
                    Luxena.sd.Isic,
                    Luxena.sd.SimCard,
                    Luxena.sd.Transfer,
                    Luxena.sd.Tour,
                    Luxena.sd.Excursion,
                    Luxena.sd.GenericProduct,
                ]),
                Luxena.sd.ProductSummary,
                Luxena.sd.FlightSegment,
            ],
        },
        {
            icon: "line-chart",
            title: "Аналитика",
            items: [
                Luxena.sd.EverydayProfitReport.titleMenuItems(sameTitleMenuItems([
                    Luxena.sd.EverydayProfitReport,
                    Luxena.sd.ProfitDistributionByProvider,
                    Luxena.sd.ProfitDistributionByCustomer,
                    Luxena.sd.FlownReport,
                    Luxena.sd.ProductSummary,
                ])),
                Luxena.sd.ProfitDistributionByProvider,
                Luxena.sd.ProfitDistributionByCustomer,
                Luxena.sd.FlownReport,
                Luxena.sd.ProductSummary.titleMenuItems(sameTitleMenuItems([
                    Luxena.sd.ProductSummary,
                    Luxena.sd.ProductTotalByYear,
                    Luxena.sd.ProductTotalByQuarter,
                    Luxena.sd.ProductTotalByMonth,
                    Luxena.sd.ProductTotalByDay,
                    Luxena.sd.ProductTotalByType,
                    Luxena.sd.ProductTotalByProvider,
                    Luxena.sd.ProductTotalBySeller,
                    Luxena.sd.ProductTotalByBooker,
                    Luxena.sd.ProductTotalByOwner,
                ])),
            ],
        },
        {
            icon: "group",
            title: "Заказчики и поставщики",
            items: [
                Luxena.sd.Party.titleMenuItems([
                    Luxena.sd.Organization,
                    Luxena.sd.Person,
                    Luxena.sd.Department,
                    Luxena.sd.Party,
                    Luxena.sd.Customer,
                ]),
                Luxena.sd.Organization.titleMenuItems([
                    Luxena.sd.Organization,
                    Luxena.sd.Airline,
                    Luxena.sd.InsuranceCompany,
                    Luxena.sd.RoamingOperator,
                    Luxena.sd.AccommodationProvider,
                    Luxena.sd.BusTicketProvider,
                    Luxena.sd.CarRentalProvider,
                    Luxena.sd.GenericProductProvider,
                    Luxena.sd.PasteboardProvider,
                    Luxena.sd.TourProvider,
                    Luxena.sd.TransferProvider,
                    Luxena.sd.Person,
                    Luxena.sd.Party,
                ]),
                Luxena.sd.Person.titleMenuItems([
                    Luxena.sd.Person,
                    Luxena.sd.Party,
                    Luxena.sd.Organization,
                    Luxena.sd.Agent,
                    Luxena.sd.Passport,
                    Luxena.sd.MilesCard,
                ]),
                Luxena.sd.MilesCard.titleMenuItems([
                    Luxena.sd.Person,
                    Luxena.sd.Passport,
                ]),
                Luxena.sd.Passport.titleMenuItems([
                    Luxena.sd.Person,
                    Luxena.sd.MilesCard,
                ]),
                Luxena.sd.AirlineServiceClass,
            ],
        },
        {
            icon: "book",
            title: "Справочники",
            items: [
                Luxena.sd.Airport,
                Luxena.sd.Country,
                Luxena.sd.AccommodationType,
                Luxena.sd.CateringType,
                Luxena.sd.GenericProductType,
                Luxena.sd.PaymentSystem,
            ],
        },
        {
            icon: "gears",
            title: "Настройки",
            items: [
                Luxena.sd.BankAccount,
                Luxena.sd.DocumentOwner,
                Luxena.sd.DocumentAccess,
                Luxena.sd.GdsAgent,
                Luxena.sd.GdsFile,
                Luxena.sd.User.titleMenuItems(sameTitleMenuItems([
                    Luxena.sd.User,
                    Luxena.sd.Identity,
                    Luxena.sd.InternalIdentity,
                ])),
                Luxena.sd.Sequence,
                Luxena.sd.SystemConfiguration.toViewMenuItem("single"),
            ],
        },
        {
            icon: "support",
            title: "Отзывы и предложения",
            onExecute: "support",
        },
    ];
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Layouts;
    (function (Layouts) {
        var layoutName = "agent";
        var layoutSets = DevExpress.framework.html.layoutSets;
        layoutSets[layoutName] = layoutSets[layoutName] || [];
        layoutSets[layoutName].push({
            platform: "generic",
            controller: new DevExpress.framework.html["DefaultLayoutController"]({
                name: layoutName,
            })
        });
        function renderLeftToolMenuItem(data, index, containerEl) {
            containerEl.append($("\n<div class=\"main-menu-item single brd-right\" title=\"" + (data.title || data.text || "") + "\">\n\t<i class=\"fa fa-" + data.icon + " fa-4x\"></i>\n</div>"));
        }
        function renderMainMenuItem(data, index, containerEl) {
            containerEl.append($("\n<div class=\"main-menu-item\"" + (data.action ? " data-bind=\"dxAction: '#" + data.action + "'\"" : "") + ">\n\t<i class=\"fa fa-" + data.icon + " fa-3x\"></i>\n\t<h4" + (data.description ? "" : " class=\"title-only\"") + ">" + (data.title || data.text) + "</h4>\n\t<span>" + (data.description || "") + "</span>\n</div>"));
        }
        function renderMainMenuSubItem(data, index, containerEl) {
            containerEl.append($("\n<div class=\"main-menu-subitem single\">\n\t<i class=\"fa fa-" + data.icon + " fa-2x\"></i>\n\t<h4>" + (data.title || data.text) + "</h4>\n</div>"));
        }
        function renderToolMenuItem(data, index, containerEl) {
            containerEl.append($("\n<div class=\"main-menu-item " + (data.items ? "" : " single") + " brd-left\" title=\"" + (data.title || data.text || "") + "\">\n\t<i class=\"fa fa-" + data.icon + " fa-3x\"></i>\n</div>"));
        }
        function getToolMenuOptions(items, itemTemplate, subItemTemplate) {
            if (items) {
                items.forEach(function (a) {
                    if (!a.items)
                        return;
                    a.items.forEach(function (b) { return b.template = b.template || subItemTemplate || itemTemplate || renderMainMenuItem; });
                });
            }
            return {
                dataSource: items,
                cssClass: "main-menu",
                showFirstSubmenuMode: "onHover",
                showSubmenuMode: "onHover",
                hideSubmenuOnMouseLeave: true,
                itemTemplate: itemTemplate || renderToolMenuItem,
                onItemClick: function (e) {
                    if (e.itemData.url)
                        Luxena.app.navigate(e.itemData.url);
                    else if (e.itemData.onExecute)
                        if (typeof e.itemData.onExecute === "string")
                            Luxena.app.navigate(e.itemData.onExecute);
                        else
                            e.itemData.onExecute(e);
                },
            };
        }
        Layouts.getToolMenuOptions = getToolMenuOptions;
        function getMainMenuOptions() {
            var menus = $.map(Luxena.config.menu, function (menu) {
                return $.extend({}, menu, {
                    template: renderMainMenuItem,
                    items: Luxena.toMenuSubitems(menu.items, renderMainMenuSubItem),
                });
            });
            var titleMenu = {
                icon: "navicon",
                items: menus,
                template: renderLeftToolMenuItem,
            };
            return getToolMenuOptions([titleMenu]);
        }
        Layouts.getMainMenuOptions = getMainMenuOptions;
        function getBackMenuOptions() {
            var backMenu = {
                icon: "remove",
                title: "Закрыть текущую страницу и вернуться на предыдущую",
                //			template: renderLeftToolMenuItem,
                onExecute: function () { return Luxena.app.back(); },
            };
            return getToolMenuOptions([backMenu]);
        }
        Layouts.getBackMenuOptions = getBackMenuOptions;
        function getTitleMenuOptions(viewMenuItems) {
            viewMenuItems = [
                {
                    template: "title",
                    items: viewMenuItems,
                }
            ];
            return getToolMenuOptions(viewMenuItems, renderMainMenuSubItem);
        }
        Layouts.getTitleMenuOptions = getTitleMenuOptions;
        function getViewMenuOptions(viewMenuItems) {
            return getToolMenuOptions(viewMenuItems, null, renderMainMenuSubItem);
        }
        Layouts.getViewMenuOptions = getViewMenuOptions;
    })(Layouts = Luxena.Layouts || (Luxena.Layouts = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    Luxena.language = "ru"; // navigator.language || navigator.browserLanguage || "ru";
    //	if (language == "ru-RU" || language == "ua" || language == "uk" || language == "uk-UA")
    //		language = "ru";
    Globalize.culture(Luxena.language);
    $(function () {
        DevExpress.devices.current({ platform: "generic" });
        Luxena.app = new DevExpress.framework.html.HtmlApplication({
            namespace: Luxena.Views,
            layoutSet: DevExpress.framework.html.layoutSets[Luxena.config.layoutSet || "desktop"],
            mode: "webSite",
            navigation: Luxena.config.menu,
            commandMapping: Luxena.config.commandMapping,
        });
        $(window).unload(function () { return Luxena.app.saveState(); });
        Luxena.app.router.register(":view/:id", { view: Luxena.config.startupView || "home", id: undefined });
        Luxena.app.navigate();
    });
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.AirlineServiceClass, function (se) { return [se.Airline, se.Code, se.ServiceClass,]; });
        Views.registerEntityControllers(Luxena.sd.AccommodationType, function (se) { return [se.Name, se.Description,]; });
        Views.registerEntityControllers(Luxena.sd.BankAccount, function (se) { return [se.Name, se.IsDefault, se.Description, se.Note]; });
        Views.registerEntityControllers(Luxena.sd.CateringType, function (se) { return [se.Name, se.Description,]; });
        Views.registerEntityControllers(Luxena.sd.CurrencyDailyRate, function (se) { return [se.Date, se.UAH_EUR, se.UAH_RUB, se.UAH_USD, se.RUB_EUR, se.RUB_USD, se.EUR_USD,]; });
        Views.registerEntityControllers(Luxena.sd.DocumentAccess, function (se) { return [se.Person, se.Owner, se.FullDocumentControl,]; });
        Views.registerEntityControllers(Luxena.sd.DocumentOwner, function (se) { return [se.Owner, se.IsActive,]; });
        Views.registerEntityControllers(Luxena.sd.GenericProductType, function (se) { return [se.Name,]; });
        Views.registerEntityControllers(Luxena.sd.PaymentSystem, function (se) { return [se.Name,]; });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    Luxena.Ui.fieldSet2(Luxena.sd.Product, {
        name: "IssueDateAndReissueFor",
        members: function (se) { return [se.IssueDate]; },
        members2: function (se) { return [se.ReissueFor]; },
    });
    Luxena.Ui.fieldRow2(Luxena.sd.Product, {
        name: "PassengerRow",
        title: function (se) { return se.Passenger; },
        members: function (se) { return [se.GdsPassengerName, se.Passenger,]; },
    });
    Luxena.Ui.fieldRow2(Luxena.sd.Product, {
        name: "CustomerAndOrder",
        members: function (se) { return [se.Customer, se.Order,]; },
    });
    Luxena.Ui.fieldSet2(Luxena.sd.Product, {
        name: "StartAndFinishDate",
        members: function (se) { return [se.StartDate, se.FinishDate]; },
    });
    Luxena.Ui.fieldRow2(Luxena.sd.Product, {
        name: "PnrAndTourCode",
        members: function (se) { return [se.PnrCode, se.TourCode,]; },
    });
    Luxena.Ui.fieldRow2(Luxena.sd.Product, {
        name: "BookerRow",
        title: function (se) { return se.Booker; },
        members: function (se) { return [se.Booker, se.BookerOffice, se.BookerCode,]; },
    });
    Luxena.Ui.fieldRow2(Luxena.sd.Product, {
        name: "TicketerRow",
        title: function (se) { return se.Ticketer; },
        members: function (se) { return [se.Ticketer, se.TicketerOffice, se.TicketerCode,]; },
    });
    Luxena.Ui.fieldRow2(Luxena.sd.Product, {
        name: "BookerAndTicketer",
        members: function (se) { return [se.Booker, se.Ticketer,]; },
    });
    Luxena.Ui.fieldRow2(Luxena.sd.Product, {
        name: "SellerAndOwner",
        members: function (se) { return [se.Seller, se.Owner]; }
    });
    Luxena.Ui.fieldSet2(Luxena.sd.Product, {
        name: "Finance",
        members: function (se) { return [
            se.Fare,
            se.EqualFare,
            se.FeesTotal,
            se.Total,
            se.Vat,
            se.Commission,
            se.CommissionDiscount,
            se.ServiceFee,
            se.Handling,
            se.Discount,
            //a.BonusDiscount,
            //a.BonusAccumulation,
            se.GrandTotal,
            se.PaymentType,
            se.TaxMode,
        ]; }
    });
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.Product, function (se) { return ({
            list: [
                se.IssueDate,
                se.Type,
                se.Name,
                se.PassengerName,
                se.Order,
                //se.Customer,
                se.Seller,
                //se.Producer,
                //se.Provider,
                //se.Country.ToColumn(true),
                //se.PnrCode.ToColumn(true),
                //se.TourCode.ToColumn(true),
                //se.TicketingIataOffice,
                se.Total,
                se.ServiceFee,
                se.GrandTotal,
            ],
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
/// <reference path="../Scripts/typings/globalize/globalize.d.ts" />
/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="../scripts/typings/devextreme/dx.devextreme.d.ts" />
/// <reference path="../scripts/tsUnit/tsUnit.ts" />
/// <reference path="_support/Extensions.ts" />
/// <reference path="_support/Log.ts" />
/// <reference path="_support/layouts/Simple/SimpleLayout.ts" />
/// <reference path="_support/layouts/Popup/PopupLayout.ts" />
/// <reference path="_support/Semantic/SemanticType..ts" />
/// <reference path="_support/Semantic/SemanticType.Bool.ts" />
/// <reference path="_support/Semantic/SemanticType.Date.ts" />
/// <reference path="_support/Semantic/SemanticType.Enum.ts" />
/// <reference path="_support/Semantic/SemanticType.Money.ts" />
/// <reference path="_support/Semantic/SemanticType.Numeric.ts" />
/// <reference path="_support/Semantic/SemanticType.Lookup.ts" />
/// <reference path="_support/Semantic/SemanticType.Text.ts" />
/// <reference path="_support/Semantic/SemanticObject.ts" />
/// <reference path="_support/Semantic/SemanticComponent..ts" />
/// <reference path="_support/Semantic/SemanticMember.ts" />
/// <reference path="_support/Semantic/SemanticEntityAction.ts" />
/// <reference path="_support/Semantic/SemanticEntity.ts" />
/// <reference path="_support/Semantic/SemanticComponent.Button.ts" />
/// <reference path="_support/Semantic/SemanticComponent.Field.ts" />
/// <reference path="_support/Semantic/SemanticComponent.FieldSet.ts" />
/// <reference path="_support/Semantic/SemanticComponent.FieldRow.ts" />
/// <reference path="_support/Semantic/SemanticComponent.GridField.ts" />
/// <reference path="_support/Semantic/SemanticDomain.ts" />
/// <reference path="_support/Semantic/SemanticController..ts" />
/// <reference path="_support/Semantic/SemanticController.Grid.ts" />
/// <reference path="_support/Semantic/SemanticController.Form..ts" />
/// <reference path="_support/Semantic/SemanticController.Form.View.ts" />
/// <reference path="_support/Semantic/SemanticController.Form.Smart.ts" />
/// <reference path="_support/Semantic/SemanticController.Form.Edit.ts" />
/// <reference path="_support/Semantic/SemanticController.Form.Filter.ts" />
/// <reference path="_support/Semantic/SemanticController.Registers.ts" />
/// <reference path="_support/Validators.ts" />
/// <reference path="_support/Ui.moneyProgress.ts" />
/// <reference path="Config.ts" />
/// <reference path="Domain.Entities.ts" />
/// <reference path="Domain.ts" />
/// <reference path="Semantics.ts" />
/// <reference path="Config.Menu.ts" />
/// <reference path="_support/layouts/Agent/AgentLayout.ts" />
/// <reference path="App.ts" />
/// <reference path="References/SimpleReferences.ts" />
/// <reference path="Products/Product.ts" />
/// dx.webappjs.debug.js
/// в _loadTemplatesFromMarkupCore строку
/// DX.utils.createComponents($markup, ["dxContent", "dxContentPlaceholder", "dxTransition"]);
/// вынести из цикла вверх под 
/// $markup.appendTo(that.$root);
var Luxena;
(function (Luxena) {
    var SemanticEmptyFieldRow = (function (_super) {
        __extends(SemanticEmptyFieldRow, _super);
        function SemanticEmptyFieldRow() {
            _super.apply(this, arguments);
        }
        SemanticEmptyFieldRow.prototype.render = function (container) {
            container.append("<div class=\"dx-field\"><div class=\"dx-field-value-static\">&nbsp;</div></div>");
        };
        return SemanticEmptyFieldRow;
    })(Luxena.SemanticComponent);
    Luxena.SemanticEmptyFieldRow = SemanticEmptyFieldRow;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Ui;
    (function (Ui) {
        Ui.emptyRow = function () { return new Luxena.SemanticEmptyFieldRow(); };
    })(Ui = Luxena.Ui || (Luxena.Ui = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Tests;
    (function (Tests) {
        //var se = sd.Product;
        var FilterFormControllerTests = (function (_super) {
            __extends(FilterFormControllerTests, _super);
            function FilterFormControllerTests() {
                _super.apply(this, arguments);
            }
            FilterFormControllerTests.prototype.prepareFilterExpression01 = function () {
                var filter = Luxena.prepareFilterExpression({}, null);
                this.areIdentical(undefined, filter);
            };
            FilterFormControllerTests.prototype.prepareFilterExpression02 = function () {
                var filter = Luxena.prepareFilterExpression({}, []);
                this.areIdentical(undefined, filter);
            };
            FilterFormControllerTests.prototype.prepareFilterExpression03 = function () {
                var filter = Luxena.prepareFilterExpression({}, [undefined, undefined]);
                this.areIdentical(undefined, filter);
            };
            FilterFormControllerTests.prototype.prepareFilterExpression04 = function () {
                var filter = Luxena.prepareFilterExpression({}, ["or"]);
                this.areIdentical(undefined, filter);
            };
            FilterFormControllerTests.prototype.prepareFilterExpression05 = function () {
                var filter = Luxena.prepareFilterExpression({}, ["or", undefined]);
                this.areIdentical(undefined, filter);
            };
            FilterFormControllerTests.prototype.prepareFilterExpression06 = function () {
                var filter = Luxena.prepareFilterExpression({}, [undefined, "or", undefined]);
                this.areIdentical(undefined, filter);
            };
            FilterFormControllerTests.prototype.prepareFilterExpression07 = function () {
                var filter = Luxena.prepareFilterExpression({}, ["or", undefined, "or", undefined]);
                this.areIdentical(undefined, filter);
            };
            FilterFormControllerTests.prototype.prepareFilterExpression08 = function () {
                var filter = Luxena.prepareFilterExpression({}, [undefined, "or", "or", undefined]);
                this.areIdentical(undefined, filter);
            };
            FilterFormControllerTests.prototype.prepareFilterExpression09 = function () {
                var filter = Luxena.prepareFilterExpression({}, ["or", "or"]);
                this.areIdentical(undefined, filter);
            };
            FilterFormControllerTests.prototype.prepareFilterExpression10 = function () {
                var filter = Luxena.prepareFilterExpression({}, ["and", undefined, "and", "and", undefined]);
                this.areIdentical(undefined, filter);
            };
            FilterFormControllerTests.prototype.prepareFilterExpression11 = function () {
                var filter = Luxena.prepareFilterExpression({}, [1, "and", "and", 2]);
                this.areCollectionsIdentical([1, "and", 2], filter);
            };
            FilterFormControllerTests.prototype.prepareFilterExpression12 = function () {
                var filter = Luxena.prepareFilterExpression({}, [1, "or"]);
                this.areCollectionsIdentical([1], filter);
            };
            FilterFormControllerTests.prototype.prepareFilterExpression13 = function () {
                var filter = Luxena.prepareFilterExpression({}, [1, "or", undefined]);
                this.areCollectionsIdentical([1], filter);
            };
            FilterFormControllerTests.prototype.prepareFilterExpression14 = function () {
                var filter = Luxena.prepareFilterExpression({}, [1, "or", null]);
                this.areCollectionsIdentical([1, "or", null], filter);
            };
            FilterFormControllerTests.prototype.prepareFilterExpression15 = function () {
                var filter = Luxena.prepareFilterExpression({}, [1, "or", [undefined]]);
                this.areCollectionsIdentical([1], filter);
            };
            FilterFormControllerTests.prototype.prepareFilterExpression16 = function () {
                var filter = Luxena.prepareFilterExpression({}, [1, "or", [undefined, "or", undefined]]);
                this.areCollectionsIdentical([1], filter);
            };
            FilterFormControllerTests.prototype.prepareFilterExpression17 = function () {
                var filter = Luxena.prepareFilterExpression({}, [1, "or", [2, "or", undefined]]);
                this.areCollectionsIdentical([1, "or", [2]], filter);
            };
            FilterFormControllerTests.prototype.prepareFilterExpression18 = function () {
                var filter = Luxena.prepareFilterExpression({}, [undefined, [2, "or"]]);
                this.areCollectionsIdentical([[2]], filter);
            };
            FilterFormControllerTests.prototype.prepareFilterExpression19 = function () {
                var filter = Luxena.prepareFilterExpression({}, [undefined, undefined, undefined, [2, "or", undefined, undefined,]]);
                this.areCollectionsIdentical([[2]], filter);
            };
            FilterFormControllerTests.prototype.prepareFilterExpression20 = function () {
                var filter = Luxena.prepareFilterExpression({}, [undefined, undefined, undefined, [undefined, "or", 2, "or", undefined, undefined,]]);
                this.areCollectionsIdentical([[2]], filter);
            };
            FilterFormControllerTests.prototype.prepareFilterExpression21 = function () {
                var filter = Luxena.prepareFilterExpression({}, [undefined, undefined, undefined, [undefined, "or", ["2", "=", "2"], "or", undefined, undefined,]]);
                this.areCollectionsIdentical([[["2", "=", "2"]]], filter);
            };
            FilterFormControllerTests.prototype.prepareFilterExpression22 = function () {
                var filter = Luxena.prepareFilterExpression({}, [["Booker.Id", "=", "a87fdce4760c4d7ea7e686182c583cb1"], "or", "or"]);
                this.areCollectionsIdentical([["Booker.Id", "=", "a87fdce4760c4d7ea7e686182c583cb1"]], filter);
            };
            return FilterFormControllerTests;
        })(tsUnit.TestClass);
        Tests.FilterFormControllerTests = FilterFormControllerTests;
    })(Tests = Luxena.Tests || (Luxena.Tests = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.Tests = function () { return ({
            viewShown: function () {
                var test = new tsUnit.Test(Luxena.Tests);
                var result = test.run(new tsUnit.TestRunLimiterRunAll());
                test.showResults($("#results").get(0), result);
            },
        }); };
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.Consignment, function (se) { return ({
            list: [
                se.IssueDate,
                se.Number,
                se.Supplier,
                se.Acquirer,
                se.TotalSupplied,
                se.GrandTotal,
            ],
            view: {
                "fields1": [
                    se.IssueDate,
                    se.Number,
                    se.Supplier,
                    se.Acquirer,
                    se.TotalSupplied,
                ],
                "fields2": [
                    se.Discount,
                    se.Total,
                    se.Vat,
                    se.GrandTotal,
                ],
            },
            viewScope: function (ctrl) { return ({
                tabs: [
                    se.OrderItems.toTab(ctrl, function (a) { return [a.Order, a.Position, a.Product, a.Text, a.GrandTotal,]; }),
                    se.IssuedConsignments.toTab(ctrl, function (a) { return [a.TimeStamp, a.Number, a.IssuedBy,]; }),
                ]
            }); },
            edit: {
                "fields": [
                    se.Number,
                    se.IssueDate,
                    se.Supplier,
                    se.Acquirer,
                    se.TotalSupplied,
                ],
            },
        }); });
        Views.registerEntityControllers(Luxena.sd.IssuedConsignment, function (se) { return [
            se.Consignment, se.TimeStamp, se.Number, se.IssuedBy,
        ]; });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.InternalTransfer, function (se) { return ({
            list: [
                se.Number,
                se.Date,
                se.FromOrder,
                se.FromParty,
                se.ToOrder,
                se.ToParty,
                se.Amount,
            ],
            form: [
                se.Number,
                se.Date,
                Luxena.Ui.fieldRow(se, "/", function () { return [se.FromOrder, se.FromParty,]; }),
                Luxena.Ui.fieldRow(se, "/", function () { return [se.ToOrder, se.ToParty,]; }),
                se.Amount,
            ],
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers([Luxena.sd.Invoice, Luxena.sd.Receipt], function (se) { return ({
            list: [
                se.IssueDate,
                se.Type,
                se.Number,
                se.Order,
                se.IssuedBy,
                se.Total,
                se.Vat,
                se.TimeStamp,
            ],
            view: {
                "fields1": [
                    se.IssueDate,
                    se.Number,
                    se.Type,
                    se.Order,
                    se.IssuedBy,
                ],
                "fields2": [
                    se.TimeStamp,
                    se.Total,
                    se.Vat,
                ],
            },
            viewScope: function (ctrl) { return ({
                tabs: [
                    se.Payments.toTab(ctrl, function (a) { return [a.Date, a.PaymentForm, a.Number, a.Payer, a.Amount, a.Vat]; }),
                ]
            }); },
            edit: null,
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.Order, function (se) { return ({
            list: [
                se.IssueDate,
                se.Number,
                se.Customer,
                se.Total,
                ////se.Vat,
                se.Paid.hidden(),
                se.TotalDue,
                Luxena.Ui.moneyProgress(se, se.Paid, se.Total),
                //se.DeliveryBalance,
                se.AssignedTo,
            ],
            view: {
                "fields1": [
                    se.Number,
                    se.IssueDate,
                    se.Customer,
                    se.BillTo,
                    se.ShipTo,
                    se.AssignedTo,
                    se.Owner,
                    se.BankAccount,
                    se.IsPublic,
                    se.IsSubjectOfPaymentsControl,
                    se.Note,
                ],
                "fields2": [
                    Luxena.Ui.moneyProgress(se, se.Paid, se.Total),
                    se.Total,
                    se.ServiceFee,
                    se.Discount,
                    se.Vat,
                    se.Paid,
                    se.TotalDue,
                    se.VatDue,
                ],
            },
            smart: [
                se.IssueDate,
                se.Customer,
                se.BillTo,
                se.ShipTo,
                se.AssignedTo,
                se.Owner,
                se.Total,
                se.Paid,
                se.TotalDue,
                se.Note,
            ],
            viewScope: function (ctrl) { return ({
                tabs: [
                    se.Items.toTab(ctrl, function (a) { return [a.Position, a.Product, a.Text, a.GrandTotal, a.Consignment,]; }),
                    se.Payments.toTab(ctrl, function (a) { return [a.Date, a.Number, a.DocumentNumber, a.PostedOn, a.Payer, a.RegisteredBy, a.Amount, a.Note,]; }),
                    se.IncomingTransfers.toTab(ctrl, function (a) { return [a.Date, a.Number, a.FromOrder, a.FromParty, a.Amount]; }),
                    se.OutgoingTransfers.toTab(ctrl, function (a) { return [a.Date, a.Number, a.ToOrder, a.ToParty, a.Amount]; }),
                ]
            }); },
            edit: {
                "fields": [
                    se.IssueDate,
                    se.Number,
                    se.Customer,
                    se.BillTo,
                    se.ShipTo,
                    Luxena.Ui.fieldRow(se, "/", function () { return [se.AssignedTo, se.Owner,]; }),
                    se.BankAccount,
                    se.IsPublic,
                    se.IsSubjectOfPaymentsControl,
                    se.SeparateServiceFee,
                    se.Note,
                ],
            },
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.OrderCheck, function (se) { return ({
            members: [
                se.Date,
                se.Order,
                se.Person,
                se.CheckType,
                se.CheckNumber,
                se.Currency,
                se.CheckAmount,
                se.PayAmount,
                se.PaymentType,
                se.Description,
                se.CreatedOn,
            ],
            edit: null,
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.OrderItem, function (se) { return ({
            list: [
                se.Order,
                se.Position,
                se.Text,
                se.GrandTotal,
            ],
            view: [
                se.Order,
                se.Product,
                se.LinkType,
                se.Text,
                se.Quantity,
                se.Price,
                se.Discount,
                se.GrandTotal,
            ],
            edit: [
                se.Order,
                se.Product,
                se.Text,
                se.Quantity,
                se.Price,
                se.Discount,
                se.GrandTotal,
            ],
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    Luxena.Ui.fieldSet2(Luxena.sd.Payment, {
        name: "DateAndDocumentNumber",
        members: function (se) { return [se.Date,]; },
        members2: function (se) { return [se.DocumentNumber,]; },
    });
    Luxena.Ui.fieldRow2(Luxena.sd.Payment, {
        name: "OrderAndPayer",
        members: function (se) { return [se.Order, se.Payer,]; },
    });
    Luxena.Ui.fieldSet2(Luxena.sd.Payment, {
        name: "AmountAndVat",
        members: function (se) { return [se.Amount, se.Vat,]; },
    });
    Luxena.Ui.fieldRow2(Luxena.sd.Payment, {
        name: "AssignedToAndOwner",
        members: function (se) { return [se.AssignedTo, se.Owner,]; },
    });
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.Payment, function (se) { return ({
            list: [
                se.DateAndDocumentNumber,
                se.Date,
                se.PaymentForm,
                se.Number,
                se.DocumentNumber,
                se.Invoice,
                se.Order,
                //se.Payer,
                se.Amount,
                //se.Vat,
                se.PostedOn,
                se.AssignedTo,
            ],
        }); });
        Views.registerEntityControllers(Luxena.sd.CashInOrderPayment, function (se) { return ({
            list: Luxena.sd.Payment,
            view: {
                "fields1": [
                    se.Date,
                    se.DocumentNumber,
                    se.Invoice,
                    se.Order,
                    se.Payer,
                    se.RegisteredBy,
                    se.ReceivedFrom,
                    se.AssignedTo,
                    se.Owner,
                ],
                "fields2": [
                    se.Amount,
                    se.Vat,
                    se.SavePosted,
                    se.PostedOn,
                    Luxena.Ui.fieldRow(se, null, [se.Void, se.Unvoid,]),
                    se.IsVoid,
                ],
                "fields3": se.Note,
            },
            edit: {
                "fields1": [
                    se.Date,
                    se.DocumentNumber,
                    se.Invoice,
                    se.OrderAndPayer,
                    se.ReceivedFrom,
                    se.AssignedToAndOwner,
                ],
                "fields2": [
                    se.Amount,
                    se.Vat,
                    se.SavePosted,
                    se.PostedOn,
                    Luxena.Ui.fieldRow(se, null, [se.Void, se.Unvoid,]),
                ],
                "fields3": se.Note,
            }
        }); });
        Views.registerEntityControllers(Luxena.sd.CashOutOrderPayment, function (se) { return ({
            list: Luxena.sd.Payment,
            view: [
                se.Date,
                se.DocumentNumber,
                se.Invoice,
                se.Order,
                se.Payer,
                se.RegisteredBy,
                se.Amount,
                se.Vat,
                se.ReceivedFrom,
                se.AssignedTo,
                se.Owner,
                se.Note,
                se.SavePosted,
                se.PostedOn,
            ],
            edit: [
                se.DateAndDocumentNumber,
                se.Invoice,
                se.OrderAndPayer,
                se.RegisteredBy,
                se.AmountAndVat,
                se.ReceivedFrom,
                se.AssignedToAndOwner,
                se.Note,
                se.SavePosted,
                se.PostedOn,
            ],
        }); });
        Views.registerEntityControllers(Luxena.sd.CheckPayment, function (se) { return ({
            list: Luxena.sd.Payment,
            form: [
                se.DateAndDocumentNumber,
                se.Invoice,
                se.OrderAndPayer,
                se.RegisteredBy,
                se.AmountAndVat,
                se.AssignedToAndOwner,
                se.Note,
            ],
        }); });
        Views.registerEntityControllers(Luxena.sd.ElectronicPayment, function (se) { return ({
            list: Luxena.sd.Payment,
            form: [
                se.DateAndDocumentNumber,
                se.AuthorizationCode,
                se.PaymentSystem,
                se.Invoice,
                se.OrderAndPayer,
                se.RegisteredBy,
                se.AmountAndVat,
                se.AssignedToAndOwner,
                se.Note,
            ],
        }); });
        Views.registerEntityControllers(Luxena.sd.WireTransfer, function (se) { return ({
            list: Luxena.sd.Payment,
            form: [
                se.DateAndDocumentNumber,
                se.Invoice,
                se.OrderAndPayer,
                se.RegisteredBy,
                se.AmountAndVat,
                se.ReceivedFrom,
                se.AssignedToAndOwner,
                se.Note,
            ],
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.EverydayProfitReports = function () {
            var args = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                args[_i - 0] = arguments[_i];
            }
            var filterForm = Views.NewProductFilterController(args, { oneDayOnly: true });
            var se = Luxena.sd.EverydayProfitReport;
            var grid = new Luxena.GridController({
                entity: se,
                master: filterForm,
                form: Luxena.sd.Product,
                smart: null,
                members: [
                    se.IssueDate,
                    se.ProductType,
                    se.Product,
                    se.Provider,
                    se.Seller.reserve(),
                    se.PassengerName.reserve(),
                    se.Itinerary,
                    se.StartDate.reserve(),
                    se.FinishDate.reserve(),
                    se.Country.reserve(),
                    se.Fare.reserve(),
                    se.Currency,
                    se.CurrencyRate,
                    se.EqualFare.totalSum(),
                    se.FeesTotal.totalSum(),
                    se.CancelFee.totalSum(),
                    se.Total.totalSum(),
                    se.Commission.totalSum(),
                    se.ServiceFee.totalSum(),
                    se.Vat.totalSum(),
                    se.GrandTotal.totalSum(),
                    se.Payer,
                    se.InvoiceDate,
                    se.CompletionCertificateDate,
                    se.PaymentDate,
                    se.Order.reserve(),
                    se.Invoice.reserve(),
                    se.CompletionCertificate.reserve(),
                    se.Payment.reserve(),
                ],
                fixed: true,
                wide: true,
            });
            return filterForm.getScopeWithGrid(grid);
        };
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.FlownReports = function () {
            var args = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                args[_i - 0] = arguments[_i];
            }
            var filterForm = Views.NewProductFilterController(args, { oneMonth: true });
            var se = Luxena.sd.FlownReport;
            var grid = new Luxena.GridController({
                entity: se,
                master: filterForm,
                form: Luxena.sd.Product,
                smart: null,
                members: [
                    se.Date.fixed(),
                    se.Op.fixed(),
                    se.AC.fixed(),
                    se.TicketNumber.fixed(),
                    se.Client,
                    se.Passenger,
                    se.Route,
                    se.Curr,
                    se.Fare,
                    se.Tax,
                    se.Flown1,
                    se.Flown2,
                    se.Flown3,
                    se.Flown4,
                    se.Flown5,
                    se.Flown6,
                    se.Flown7,
                    se.Flown8,
                    se.Flown9,
                    se.Flown10,
                    se.Flown11,
                    se.Flown12,
                    se.TourCode,
                    se.CheapTicket,
                ],
                //fixed: true,
                useFilter: false,
                useGrouping: false,
                wide: true,
            });
            return filterForm.getScopeWithGrid(grid);
        };
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        var fse = Luxena.sd.ProductFilter;
        fse.MinIssueDate.titlePostfix(" с");
        fse.MaxIssueDate.ru("по");
        var date = new Date(), y = date.getFullYear(), m = date.getMonth();
        var filterModel = {
            IssueDate: ko.observable(date),
            IssueMonth: ko.observable(new Date(y, m, 1)),
            MinIssueDate: ko.observable(new Date(y, m, 1)),
            MaxIssueDate: ko.observable(new Date(y, m + 1, 0)),
        };
        function NewProductFilterController(args, cfg) {
            if (!cfg)
                cfg = {};
            var members = [];
            if (cfg.oneMonth)
                members.push(fse.IssueMonth);
            else {
                members.push(fse.MinIssueDate, fse.MaxIssueDate);
                var minDate = filterModel.MinIssueDate;
                var maxDate = filterModel.MaxIssueDate;
                if (cfg.oneDayOnly && (!minDate() || minDate() !== maxDate())) {
                    var date_1 = new Date();
                    minDate(date_1);
                    maxDate(date_1);
                }
            }
            members.push(fse.Type, fse.Name, fse.State, fse.ProductCurrency, fse.Provider, fse.Customer, fse.Booker, fse.Ticketer, fse.Seller, fse.Owner);
            return new Luxena.FilterFormController({
                entity: fse,
                args: args,
                model: filterModel,
                members: members,
            });
        }
        Views.NewProductFilterController = NewProductFilterController;
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        function registerProductTotalController(se, cfg) {
            Views[se._names] = function () {
                var args = [];
                for (var _i = 0; _i < arguments.length; _i++) {
                    args[_i - 0] = arguments[_i];
                }
                var filterCtrl = Views.NewProductFilterController(args);
                var p = Luxena.sd.Product;
                var scope = filterCtrl.getScope();
                scope.viewMenuItems = [
                    {
                        icon: "refresh",
                        text: "Обновить",
                        onExecute: function () { return filterCtrl.apply(); },
                    }
                ];
                scope.title = se._titles || se._title;
                var argumentMember = cfg.argumentField(se);
                var tagMember = cfg.tagField && cfg.tagField(se);
                var argumentFormat = argumentMember._format || argumentMember._type.format;
                var g = new Luxena.GridController({
                    entity: se,
                    members: cfg.members,
                    filter: filterCtrl.filter,
                    fixed: true,
                }).getScope().gridOptions;
                scope = $.extend(scope, {
                    title: scope.title,
                    template: "chart",
                    titleMenuItems: Luxena.toMenuSubitems(se.getTitleMenuItems()),
                    gridOptions: g,
                    chartOptions: {
                        dataSource: {
                            store: se._store,
                            filter: filterCtrl.filter,
                        },
                        commonSeriesSettings: {
                            argumentField: argumentMember._name,
                            type: "stackedBar",
                            label: {
                                format: "fixedPoint",
                                precision: 2,
                            },
                            point: {
                                hoverMode: "allArgumentPoints",
                            },
                            tagField: tagMember && tagMember._name,
                        },
                        series: [
                            { valueField: "Total", name: p.Total._title, stack: "1", },
                            { valueField: "ServiceFee", name: p.ServiceFee._title, stack: "1", },
                            {
                                valueField: "GrandTotal",
                                name: p.GrandTotal._title,
                                stack: "2",
                                label: {
                                    visible: true,
                                    connector: { visible: true, },
                                    position: "outside",
                                },
                            },
                        ],
                        argumentAxis: {
                            argumentType: argumentMember._type.chartDataType,
                            tickInterval: 1,
                            label: {
                                format: argumentFormat,
                            },
                            inverted: cfg.rotated,
                        },
                        legend: {
                            verticalAlignment: "bottom",
                            horizontalAlignment: "center",
                            itemTextPosition: "right",
                        },
                        loadingIndicator: {
                            show: true,
                        },
                        //palette: "Harmony Light",
                        palette: "Violet",
                        resolveLabelOverlapping: "hide",
                        rotated: cfg.rotated,
                        valueAxis: [
                            {
                                title: {
                                    text: "Суммы",
                                },
                                label: {
                                    format: "fixedPoint",
                                    precision: 2,
                                },
                            }
                        ],
                        scrollBar: {
                            visible: true
                        },
                        scrollingMode: "all",
                        zoomingMode: "all",
                        title: scope.title,
                        tooltip: {
                            enabled: true,
                            format: "fixedPoint",
                            precision: 2,
                            shared: true,
                            //location: "edge",
                            customizeTooltip: function (point) {
                                //$log(point);
                                var argumentText = ko.format(point.originalArgument, argumentFormat);
                                return {
                                    text: "<b>" + argumentText + "</b>:<br>" + point.valueText,
                                };
                            },
                        },
                        onPointClick: function (e) {
                            if (!cfg.onPointClick)
                                return;
                            //$log(e);
                            cfg.onPointClick(filterCtrl, e.target.originalArgument, e.target.tag);
                        },
                    },
                });
                cfg.chartOptions && cfg.chartOptions(scope.chartOptions, scope);
                return scope;
            };
        }
        Views.ProductSummaries = function () {
            var args = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                args[_i - 0] = arguments[_i];
            }
            var filterForm = Views.NewProductFilterController(args);
            var se = Luxena.sd.ProductSummary;
            var grid = new Luxena.GridController({
                entity: se,
                form: Luxena.sd.Product,
                master: filterForm,
                members: [
                    se.IssueDate,
                    se.Type,
                    se.Name,
                    se.Itinerary,
                    se.Total,
                    se.ServiceFee,
                    se.GrandTotal,
                ],
                fixed: true,
            });
            return filterForm.getScopeWithGrid(grid);
        };
        var fse = Luxena.sd.ProductFilter;
        registerProductTotalController(Luxena.sd.ProductTotalByYear, {
            argumentField: function (se) { return se.Year; },
            members: function (se) { return [se.Year, se.Total, se.ServiceFee, se.GrandTotal, se.Note,]; },
            onPointClick: function (filterCtrl, arg) {
                filterCtrl.modelValue(fse.MinIssueDate, new Date(arg, 0));
                filterCtrl.modelValue(fse.MaxIssueDate, new Date(arg, 11, 31));
                Luxena.sd.ProductTotalByMonth.navigateToList();
            }
        });
        registerProductTotalController(Luxena.sd.ProductTotalByQuarter, {
            argumentField: function (se) { return se.IssueDate; },
            members: function (se) { return [se.IssueDate, se.Total, se.ServiceFee, se.GrandTotal, se.Note,]; },
            onPointClick: function (filterCtrl, arg) {
                var quarter = arg.getMonth() / 3 >> 0;
                var min = new Date(arg.getFullYear(), quarter * 3, 1);
                var max = new Date(arg.getFullYear(), quarter * 3 + 3, 0);
                filterCtrl.modelValue(fse.MinIssueDate, min);
                filterCtrl.modelValue(fse.MaxIssueDate, max);
                Luxena.sd.ProductTotalByMonth.navigateToList();
            }
        });
        registerProductTotalController(Luxena.sd.ProductTotalByMonth, {
            argumentField: function (se) { return se.IssueDate; },
            members: function (se) { return [se.IssueDate, se.Total, se.ServiceFee, se.GrandTotal, se.Note,]; },
            onPointClick: function (filterCtrl, arg) {
                var min = new Date(arg.getFullYear(), arg.getMonth(), 1);
                var max = new Date(arg.getFullYear(), arg.getMonth() + 1, 0);
                filterCtrl.modelValue(fse.MinIssueDate, min);
                filterCtrl.modelValue(fse.MaxIssueDate, max);
                Luxena.sd.ProductTotalBySeller.navigateToList();
            }
        });
        registerProductTotalController(Luxena.sd.ProductTotalByDay, {
            argumentField: function (se) { return se.IssueDate; },
            members: function (se) { return [se.IssueDate, se.Total, se.ServiceFee, se.GrandTotal, se.Note,]; },
            onPointClick: function (filterCtrl, arg) {
                filterCtrl.modelValue(fse.MinIssueDate, arg);
                filterCtrl.modelValue(fse.MaxIssueDate, arg);
                Luxena.sd.ProductTotalBySeller.navigateToList();
            },
            chartOptions: function (chart) {
                chart.commonSeriesSettings.type = "bar";
                //chart.commonSeriesSettings.line = { point: { visible: false } };
                chart.series = [chart.series[2]];
                chart.series[0].label.visible = false;
                chart.tooltip.enabled = false;
            },
        });
        registerProductTotalController(Luxena.sd.ProductTotalByType, {
            argumentField: function (se) { return se.TypeName; },
            tagField: function (se) { return se.Type; },
            rotated: true,
            members: function (se) { return [se.Rank, se.Type, se.Total, se.ServiceFee, se.GrandTotal, se.Note,]; },
            onPointClick: function (filterCtrl, arg, tag) {
                filterCtrl.modelValue(fse.Type, tag),
                    Luxena.sd.ProductTotalByMonth.navigateToList();
            }
        });
        registerProductTotalController(Luxena.sd.ProductTotalByProvider, {
            argumentField: function (se) { return se.ProviderName; },
            tagField: function (se) { return se.Provider; },
            rotated: true,
            members: function (se) { return [se.Rank, se.Provider, se.Total, se.ServiceFee, se.GrandTotal, se.Note,]; },
            onPointClick: function (filterCtrl, arg, tag) {
                filterCtrl.modelValue(fse.Provider, tag.Id),
                    Luxena.sd.ProductTotalByMonth.navigateToList();
            }
        });
        registerProductTotalController(Luxena.sd.ProductTotalBySeller, {
            argumentField: function (se) { return se.SellerName; },
            tagField: function (se) { return se.Seller; },
            rotated: true,
            members: function (se) { return [se.Rank, se.Seller, se.Total, se.ServiceFee, se.GrandTotal, se.Note,]; },
            onPointClick: function (filterCtrl, arg, tag) {
                filterCtrl.modelValue(fse.Seller, tag.Id),
                    Luxena.sd.ProductTotalByMonth.navigateToList();
            }
        });
        registerProductTotalController(Luxena.sd.ProductTotalByBooker, {
            argumentField: function (se) { return se.BookerName; },
            tagField: function (se) { return se.Booker; },
            rotated: true,
            members: function (se) { return [se.Rank, se.Booker, se.Total, se.ServiceFee, se.GrandTotal, se.Note,]; },
            onPointClick: function (filterCtrl, arg, tag) {
                filterCtrl.modelValue(fse.Booker, tag.Id),
                    Luxena.sd.ProductTotalByMonth.navigateToList();
            }
        });
        registerProductTotalController(Luxena.sd.ProductTotalByOwner, {
            argumentField: function (se) { return se.OwnerName; },
            tagField: function (se) { return se.Owner; },
            rotated: true,
            members: function (se) { return [se.Rank, se.Owner, se.Total, se.ServiceFee, se.GrandTotal, se.Note,]; },
            onPointClick: function (filterCtrl, arg, tag) {
                filterCtrl.modelValue(fse.Owner, tag.Id),
                    Luxena.sd.ProductTotalByMonth.navigateToList();
            }
        });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
//module Luxena.Views
//{
//	import DxChartOptions = DevExpress.viz.charts.dxChartOptions;
//	
//
//	export var ProductSummaries = (...args) =>
//	{
//		var filterCtrl = NewProductFilterController(args);
//
//		var scope = filterCtrl.getScope();
//		var se = sd.ProductSummary;
//		scope.title = se._title;
//		scope.tabs = [];
//		scope.titleMenuItems = toMenuSubitems(se.getTitleMenuItems()),
//		//scope.tabIndex = ko.observable(0);
//
//		scope.tabs.push($.extend(
//			new GridController({
//				entity: se,
//				form: sd.Product,
//				master: filterCtrl,
//				members: [
//					se.IssueDate,
//					se.Type,
//					se.Name,
//					se.Itinerary,
//					se.Total,
//					se.ServiceFee,
//					se.GrandTotal,
//					se.Order,
//				],
//
//				useFilterRow: false,
//				useGrouping: false,
//				useSearch: false,
//				columnsIsStatic: true,
//
//				fullHeight: true,
//			}).getScope(),
//			{
//				title: sd.Product._titles,
//				template: "grid",
//			}
//		));
//
//
//		var fse = sd.ProductFilter;
//
//		addProductTotalTabs(scope.tabs, filterCtrl, sd.ProductTotalByYear, {
//			argumentField: se => se.Year,
//
//			members: se => [se.Year, se.Total, se.ServiceFee, se.GrandTotal, se.Note, ],
//
//			onPointClick: (filterCtrl, arg) =>
//			{
//				filterCtrl.modelValue(fse.MinIssueDate, new Date(arg, 0));
//				filterCtrl.modelValue(fse.MaxIssueDate, new Date(arg, 11, 31));
//
//				filterCtrl.apply();
//				//sd.ProductTotalByMonth.navigateToList();
//			}
//		});
//
//
//		addProductTotalTabs(scope.tabs, filterCtrl, sd.ProductTotalByQuarter, {
//			argumentField: se => se.IssueDate,
//
//			members: se => [se.IssueDate, se.Total, se.ServiceFee, se.GrandTotal, se.Note, ],
//
//			onPointClick: (filterCtrl, arg) =>
//			{
//				var quarter = arg.getMonth() / 3 >> 0;
//				var min = new Date(arg.getFullYear(), quarter * 3, 1);
//				var max = new Date(arg.getFullYear(), quarter * 3 + 3, 0);
//
//				filterCtrl.modelValue(fse.MinIssueDate, min);
//				filterCtrl.modelValue(fse.MaxIssueDate, max);
//
//				filterCtrl.apply();
//				//sd.ProductTotalByMonth.navigateToList();
//			}
//		});
//
//
//		addProductTotalTabs(scope.tabs, filterCtrl, sd.ProductTotalByMonth, {
//			argumentField: se => se.IssueDate,
//
//			members: se => [se.IssueDate, se.Total, se.ServiceFee, se.GrandTotal, se.Note, ],
//
//			onPointClick: (filterCtrl, arg) =>
//			{
//				var min = new Date(arg.getFullYear(), arg.getMonth(), 1);
//				var max = new Date(arg.getFullYear(), arg.getMonth() + 1, 0);
//
//				filterCtrl.modelValue(fse.MinIssueDate, min);
//				filterCtrl.modelValue(fse.MaxIssueDate, max);
//
//				filterCtrl.apply();
//				//sd.ProductTotalBySeller.navigateToList();
//			}
//		});
//
//
//		addProductTotalTabs(scope.tabs, filterCtrl, sd.ProductTotalByDay, {
//			argumentField: se => se.IssueDate,
//
//			members: se => [se.IssueDate, se.Total, se.ServiceFee, se.GrandTotal, se.Note, ],
//
//			onPointClick: (filterCtrl, arg) =>
//			{
//				filterCtrl.modelValue(fse.MinIssueDate, arg);
//				filterCtrl.modelValue(fse.MaxIssueDate, arg);
//				filterCtrl.apply();
//				//sd.ProductTotalBySeller.navigateToList();
//			},
//
//			chartOptions: chart =>
//			{
//				chart.commonSeriesSettings.type = "bar";
//
//				chart.series = [chart.series[2]];
//				chart.series[0].label.visible = false;
//
//				chart.tooltip.enabled = false;
//			},
//		});
//
//		addProductTotalTabs(scope.tabs, filterCtrl, sd.ProductTotalByType, {
//			argumentField: se => se.TypeName,
//			tagField: se => se.Type,
//			rotated: true,
//
//			members: se => [se.Rank, se.Type, se.Total, se.ServiceFee, se.GrandTotal, se.Note, ],
//
//			onPointClick: (filterCtrl, arg, tag) =>
//			{
//				filterCtrl.modelValue(fse.Type, tag);
//				filterCtrl.apply();
//				//sd.ProductTotalByMonth.navigateToList();
//			}
//		});
//
//		addProductTotalTabs(scope.tabs, filterCtrl, sd.ProductTotalByProvider, {
//			argumentField: se => se.ProviderName,
//			tagField: se => se.Provider,
//			rotated: true,
//
//			members: se => [se.Rank, se.Provider, se.Total, se.ServiceFee, se.GrandTotal, se.Note, ],
//
//			onPointClick: (filterCtrl, arg, tag) =>
//			{
//				filterCtrl.modelValue(fse.Provider, tag.Id);
//				filterCtrl.apply();
//				//sd.ProductTotalByMonth.navigateToList();
//			}
//		});
//
//		addProductTotalTabs(scope.tabs, filterCtrl, sd.ProductTotalBySeller, {
//			argumentField: se => se.SellerName,
//			tagField: se => se.Seller,
//			rotated: true,
//
//			members: se => [se.Rank, se.Seller, se.Total, se.ServiceFee, se.GrandTotal, se.Note, ],
//
//			onPointClick: (filterCtrl, arg, tag) =>
//			{
//				filterCtrl.modelValue(fse.Seller, tag.Id);
//				filterCtrl.apply();
//				//sd.ProductTotalByMonth.navigateToList();
//			}
//		});
//
//		addProductTotalTabs(scope.tabs, filterCtrl, sd.ProductTotalByBooker, {
//			argumentField: se => se.BookerName,
//			tagField: se => se.Booker,
//			rotated: true,
//
//			members: se => [se.Rank, se.Booker, se.Total, se.ServiceFee, se.GrandTotal, se.Note, ],
//
//			onPointClick: (filterCtrl, arg, tag) =>
//			{
//				filterCtrl.modelValue(fse.Booker, tag.Id);
//				filterCtrl.apply();
//				//sd.ProductTotalByMonth.navigateToList();
//			}
//		});
//
//		addProductTotalTabs(scope.tabs, filterCtrl, sd.ProductTotalByOwner, {
//			argumentField: se => se.OwnerName,
//			tagField: se => se.Owner,
//			rotated: true,
//
//			members: se => [se.Rank, se.Owner, se.Total, se.ServiceFee, se.GrandTotal, se.Note, ],
//
//			onPointClick: (filterCtrl, arg, tag) =>
//			{
//				filterCtrl.modelValue(fse.Owner, tag.Id);
//				filterCtrl.apply();
//				//sd.ProductTotalByMonth.navigateToList();
//			}
//		});
//
//		return scope;
//	};
//
//
//	function addProductTotalTabs<TEntity extends SemanticEntity>(
//		tabs: any[],
//		filterCtrl: FilterFormController,
//		se: TEntity,
//		cfg: {
//			argumentField: (se: TEntity) => SemanticMember;
//			tagField?: (se: TEntity) => SemanticMember;
//			rotated?: boolean;
//			members?: (se: TEntity) => SemanticMember[];
//			onPointClick?: (filterCtrl, arg, tag) => void;
//
//			chartOptions?: (options: DxChartOptions, scope) => void;
//		})
//	{
//		var p = sd.Product;
//
//		var argumentMember = cfg.argumentField(se);
//		var tagMember = cfg.tagField && cfg.tagField(se);
//		var argumentFormat = argumentMember._format || argumentMember._type.format;
//
//		var chartOptions = <DxChartOptions>{
//			dataSource: {
//				store: se._store,
//				filter: filterCtrl.filter,
//			},
//
//			commonSeriesSettings: {
//				argumentField: argumentMember._name,
//				type: "stackedBar",
//				label: {
//					format: "fixedPoint",
//					precision: 2,
//				},
//				point: {
//					hoverMode: "allArgumentPoints",
//				},
//				tagField: tagMember && tagMember._name,
//			},
//
//			series: [
//				{ valueField: "Total", name: p.Total._title, stack: "1", },
//				{ valueField: "ServiceFee", name: p.ServiceFee._title, stack: "1", },
//				{
//					valueField: "GrandTotal",
//					name: p.GrandTotal._title,
//					stack: "2",
//					label: {
//						visible: true,
//						connector: { visible: true, },
//						position: "outside",
//					},
//				},
//			],
//
//			argumentAxis: {
//				argumentType: argumentMember._type.chartDataType,
//				tickInterval: 1,
//				label: {
//					format: argumentFormat,
//				},
//				inverted: cfg.rotated,
//			},
//
//			legend: {
//				verticalAlignment: "bottom",
//				horizontalAlignment: "center",
//				itemTextPosition: "right",
//			},
//
//			loadingIndicator: {
//				show: true,
//			},
//
//			//palette: "Harmony Light",
//			palette: "Violet",
//
//			resolveLabelOverlapping: "hide",
//
//			rotated: cfg.rotated,
//
//			valueAxis: [
//				{
//					title: {
//						text: "Суммы",
//					},
//
//					label: {
//						format: "fixedPoint",
//						precision: 2,
//					},
//				}
//			],
//
//			scrollBar: {
//				visible: true
//			},
//
//			scrollingMode: "all",
//			zoomingMode: "all",
//
//			title: se._titles || se._title,
//
//			tooltip: {
//				enabled: true,
//				format: "fixedPoint",
//				precision: 2,
//				shared: true,
//				//location: "edge",
//				customizeTooltip: (point: any) =>
//				{
//					//$log(point);
//					var argumentText = ko.format(point.originalArgument, argumentFormat);
//
//					return {
//						text: "<b>" + argumentText + "</b>:<br>" + point.valueText,
//					};
//				},
//			},
//
//
//			onPointClick: e =>
//			{
//				if (!cfg.onPointClick) return;
//
//				//$log(e);
//				cfg.onPointClick(filterCtrl, e.target.originalArgument, e.target.tag);
//			},
//
//		};
//
//		var chartScope = {
//			//icon: "fa fa-bar-chart",
//			title: se._shortTitle || se._titles || se._title,
//			template: "chart",
//			//gridOptions: g,
//			chartOptions: chartOptions,
//		};
//
//		cfg.chartOptions && cfg.chartOptions(chartScope.chartOptions, chartScope);
//		tabs.push(chartScope);
//
//		var gridScope = new GridController({
//			entity: se,
//			members: cfg.members,
//			master: filterCtrl,
//			fixed: true,
//			fullHeight: true,
//		}).getScope();
//
//		gridScope.icon = "fa fa-table";
//		gridScope.template = "grid";
//		delete gridScope.title;
//
//		tabs.push(gridScope);
//	}
//
//
//	//var fse = sd.ProductFilter;
//
//	//registerProductTotalController(sd.ProductTotalByYear, {
//	//	argumentField: se => se.Year,
//
//	//	members: se => [se.Year, se.Total, se.ServiceFee, se.GrandTotal, se.Note, ],
//
//	//	onPointClick: (filterCtrl, arg) =>
//	//	{
//	//		filterCtrl.modelValue(fse.MinIssueDate, new Date(arg, 0));
//	//		filterCtrl.modelValue(fse.MaxIssueDate, new Date(arg, 11, 31));
//
//	//		sd.ProductTotalByMonth.navigateToList();
//	//	}
//	//});
//
//
//	//registerProductTotalController(sd.ProductTotalByQuarter, {
//	//	argumentField: se => se.IssueDate,
//
//	//	members: se => [se.IssueDate, se.Total, se.ServiceFee, se.GrandTotal, se.Note, ],
//
//	//	onPointClick: (filterCtrl, arg) =>
//	//	{
//	//		var quarter = arg.getMonth() / 3 >> 0;
//	//		var min = new Date(arg.getFullYear(), quarter * 3, 1);
//	//		var max = new Date(arg.getFullYear(), quarter * 3 + 3, 0);
//
//	//		filterCtrl.modelValue(fse.MinIssueDate, min);
//	//		filterCtrl.modelValue(fse.MaxIssueDate, max);
//
//	//		sd.ProductTotalByMonth.navigateToList();
//	//	}
//	//});
//
//
//	//registerProductTotalController(sd.ProductTotalByMonth, {
//	//	argumentField: se => se.IssueDate,
//
//	//	members: se => [se.IssueDate, se.Total, se.ServiceFee, se.GrandTotal, se.Note, ],
//
//	//	onPointClick: (filterCtrl, arg) =>
//	//	{
//	//		var min = new Date(arg.getFullYear(), arg.getMonth(), 1);
//	//		var max = new Date(arg.getFullYear(), arg.getMonth() + 1, 0);
//
//	//		filterCtrl.modelValue(fse.MinIssueDate, min);
//	//		filterCtrl.modelValue(fse.MaxIssueDate, max);
//
//	//		sd.ProductTotalBySeller.navigateToList();
//	//	}
//	//});
//
//
//	//registerProductTotalController(sd.ProductTotalByDay, {
//	//	argumentField: se => se.IssueDate,
//
//	//	members: se => [se.IssueDate, se.Total, se.ServiceFee, se.GrandTotal, se.Note, ],
//
//	//	onPointClick: (filterCtrl, arg) =>
//	//	{
//	//		filterCtrl.modelValue(fse.MinIssueDate, arg);
//	//		filterCtrl.modelValue(fse.MaxIssueDate, arg);
//
//	//		sd.ProductTotalBySeller.navigateToList();
//	//	},
//
//	//	chartOptions: chart =>
//	//	{
//	//		chart.commonSeriesSettings.type = "bar";
//	//		//chart.commonSeriesSettings.line = { point: { visible: false } };
//
//	//		chart.series = [chart.series[2]];
//	//		chart.series[0].label.visible = false;
//
//	//		chart.tooltip.enabled = false;
//	//	},
//	//});
//
//
//	//registerProductTotalController(sd.ProductTotalByType, {
//	//	argumentField: se => se.TypeName,
//	//	tagField: se => se.Type,
//	//	rotated: true,
//
//	//	members: se => [se.Rank, se.Type, se.Total, se.ServiceFee, se.GrandTotal, se.Note, ],
//
//	//	onPointClick: (filterCtrl, arg, tag) =>
//	//	{
//	//		filterCtrl.modelValue(fse.Type, tag),
//
//	//		sd.ProductTotalByMonth.navigateToList();
//	//	}
//	//});
//
//
//	//registerProductTotalController(sd.ProductTotalByProvider, {
//	//	argumentField: se => se.ProviderName,
//	//	tagField: se => se.Provider,
//	//	rotated: true,
//
//	//	members: se => [se.Rank, se.Provider, se.Total, se.ServiceFee, se.GrandTotal, se.Note, ],
//
//	//	onPointClick: (filterCtrl, arg, tag) =>
//	//	{
//	//		filterCtrl.modelValue(fse.Provider, tag.Id),
//	//		sd.ProductTotalByMonth.navigateToList();
//	//	}
//	//});
//
//
//	//registerProductTotalController(sd.ProductTotalBySeller, {
//	//	argumentField: se => se.SellerName,
//	//	tagField: se => se.Seller,
//	//	rotated: true,
//
//	//	members: se => [se.Rank, se.Seller, se.Total, se.ServiceFee, se.GrandTotal, se.Note, ],
//
//	//	onPointClick: (filterCtrl, arg, tag) =>
//	//	{
//	//		filterCtrl.modelValue(fse.Seller, tag.Id),
//	//		sd.ProductTotalByMonth.navigateToList();
//	//	}
//	//});
//
//
//	//registerProductTotalController(sd.ProductTotalByBooker, {
//	//	argumentField: se => se.BookerName,
//	//	tagField: se => se.Booker,
//	//	rotated: true,
//
//	//	members: se => [se.Rank, se.Booker, se.Total, se.ServiceFee, se.GrandTotal, se.Note, ],
//
//	//	onPointClick: (filterCtrl, arg, tag) =>
//	//	{
//	//		filterCtrl.modelValue(fse.Booker, tag.Id),
//	//		sd.ProductTotalByMonth.navigateToList();
//	//	}
//	//});
//
//
//	//registerProductTotalController(sd.ProductTotalByOwner, {
//	//	argumentField: se => se.OwnerName,
//	//	tagField: se => se.Owner,
//	//	rotated: true,
//
//	//	members: se => [se.Rank, se.Owner, se.Total, se.ServiceFee, se.GrandTotal, se.Note, ],
//
//	//	onPointClick: (filterCtrl, arg, tag) =>
//	//	{
//	//		filterCtrl.modelValue(fse.Owner, tag.Id),
//	//		sd.ProductTotalByMonth.navigateToList();
//	//	}
//	//});
//
//
//	//function registerProductTotalController<TEntity extends SemanticEntity>(se: TEntity, cfg: {
//	//	argumentField: (se: TEntity) => SemanticMember;
//	//	//argumentType?: string;
//	//	//argumentFormat?: string;
//	//	tagField?: (se: TEntity) => SemanticMember;
//	//	rotated?: boolean;
//	//	members?: (se: TEntity) => SemanticMember[];
//	//	onPointClick?: (filterCtrl, arg, tag) => void;
//
//	//	chartOptions?: (options: DxChartOptions, scope) => void;
//	//})
//	//{
//	//	Views[se._names] = () =>
//	//	{
//	//		var filterCtrl = NewProductFilterController(arguments);
//
//	//		var p = sd.Product;
//
//	//		var scope = filterCtrl.getScope();
//
//	//		scope.viewMenuItems = [
//	//			{
//	//				icon: "refresh",
//	//				text: "Обновить",
//	//				onExecute: () => filterCtrl.apply(),
//	//			}
//	//		];
//
//	//		scope.title = se._titles || se._title;
//
//	//		var argumentMember = cfg.argumentField(se);
//	//		var tagMember = cfg.tagField && cfg.tagField(se);
//	//		var argumentFormat = argumentMember._format || argumentMember._type.format;
//
//	//		var g = new GridController({
//	//			entity: se,
//	//			members: cfg.members,
//	//			filter: filterCtrl.filter,
//	//			fixed: true,
//	//		}).getScope().gridOptions;
//
//	//		var chartOptions = <DxChartOptions>{
//	//			dataSource: {
//	//				store: se._store,
//	//				filter: filterCtrl.filter,
//	//			},
//
//	//			commonSeriesSettings: {
//	//				argumentField: argumentMember._name,
//	//				type: "stackedBar",
//	//				label: {
//	//					format: "fixedPoint",
//	//					precision: 2,
//	//				},
//	//				point: {
//	//					hoverMode: "allArgumentPoints",
//	//				},
//	//				tagField: tagMember && tagMember._name,
//	//			},
//
//	//			series: [
//	//				{ valueField: "Total", name: p.Total._title, stack: "1", },
//	//				{ valueField: "ServiceFee", name: p.ServiceFee._title, stack: "1", },
//	//				{
//	//					valueField: "GrandTotal",
//	//					name: p.GrandTotal._title,
//	//					stack: "2",
//	//					label: {
//	//						visible: true,
//	//						connector: { visible: true, },
//	//						position: "outside",
//	//					},
//	//				},
//	//			],
//
//	//			argumentAxis: {
//	//				argumentType: argumentMember._type.chartDataType,
//	//				tickInterval: 1,
//	//				label: {
//	//					format: argumentFormat,
//	//				},
//	//				inverted: cfg.rotated,
//	//			},
//
//	//			legend: {
//	//				verticalAlignment: "bottom",
//	//				horizontalAlignment: "center",
//	//				itemTextPosition: "right",
//	//			},
//
//	//			loadingIndicator: {
//	//				show: true,
//	//			},
//
//	//			//palette: "Harmony Light",
//	//			palette: "Violet",
//
//	//			resolveLabelOverlapping: "hide",
//
//	//			rotated: cfg.rotated,
//
//	//			valueAxis: [
//	//				{
//	//					title: {
//	//						text: "Суммы",
//	//					},
//
//	//					label: {
//	//						format: "fixedPoint",
//	//						precision: 2,
//	//					},
//	//				}
//	//			],
//
//	//			scrollBar: {
//	//				visible: true
//	//			},
//
//	//			scrollingMode: "all",
//	//			zoomingMode: "all",
//
//	//			title: scope.title,
//
//	//			tooltip: {
//	//				enabled: true,
//	//				format: "fixedPoint",
//	//				precision: 2,
//	//				shared: true,
//	//				//location: "edge",
//	//				customizeTooltip: (point: any) =>
//	//				{
//	//					//$log(point);
//	//					var argumentText = ko.format(point.originalArgument, argumentFormat);
//
//	//					return {
//	//						text: "<b>" + argumentText + "</b>:<br>" + point.valueText,
//	//					};
//	//				},
//	//			},
//
//
//	//			onPointClick: e =>
//	//			{
//	//				if (!cfg.onPointClick) return;
//
//	//				//$log(e);
//	//				cfg.onPointClick(filterCtrl, e.target.originalArgument, e.target.tag);
//	//			},
//
//	//		};
//
//	//		scope = $.extend(scope, {
//
//	//			title: scope.title,
//	//			template: "chart",
//	//			titleMenuItems: toMenuSubitems(se.getTitleMenuItems()),
//	//			gridOptions: g,
//	//			chartOptions: chartOptions,
//
//	//		});
//
//	//		cfg.chartOptions && cfg.chartOptions(scope.chartOptions, scope);
//
//	//		return scope;
//	//	};
//	//}
//
//
//
//} 
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.ProfitDistributionByCustomers = function () {
            var args = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                args[_i - 0] = arguments[_i];
            }
            var filterForm = Views.NewProductFilterController(args);
            var se = Luxena.sd.ProfitDistributionByCustomer;
            var grid = new Luxena.GridController({
                entity: se,
                master: filterForm,
                members: [
                    se.Rank,
                    se.Customer,
                    se.SellCount.totalSum(),
                    se.RefundCount.totalSum(),
                    se.VoidCount.totalSum(),
                    se.Currency,
                    se.SellGrandTotal.totalSum(),
                    se.RefundGrandTotal.totalSum(),
                    se.GrandTotal.totalSum(),
                    se.Total.totalSum(),
                    se.ServiceFee.totalSum(),
                    se.Commission.totalSum(),
                    se.AgentTotal.totalSum(),
                    se.Vat.totalSum(),
                ],
                fixed: true,
                useGrouping: false,
                wide: true,
            });
            return filterForm.getScopeWithGrid(grid);
        };
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.ProfitDistributionByProviders = function () {
            var args = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                args[_i - 0] = arguments[_i];
            }
            var filterForm = Views.NewProductFilterController(args);
            var se = Luxena.sd.ProfitDistributionByProvider;
            var grid = new Luxena.GridController({
                entity: se,
                master: filterForm,
                members: [
                    se.Rank,
                    se.Provider,
                    se.SellCount.totalSum(),
                    se.RefundCount.totalSum(),
                    se.VoidCount.totalSum(),
                    se.Currency,
                    se.SellGrandTotal.totalSum(),
                    se.RefundGrandTotal.totalSum(),
                    se.GrandTotal.totalSum(),
                    se.Total.totalSum(),
                    se.ServiceFee.totalSum(),
                    se.Commission.totalSum(),
                    se.AgentTotal.totalSum(),
                    se.Vat.totalSum(),
                ],
                fixed: true,
                useGrouping: false,
                wide: true,
            });
            return filterForm.getScopeWithGrid(grid);
        };
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.Identity, function (se) { return ({
            list: [
                se.Name,
                se.Description,
            ],
        }); });
        Views.registerEntityControllers(Luxena.sd.InternalIdentity, function (se) { return ({
            members: [
                se.Name,
                se.Description,
            ],
        }); });
        Views.registerEntityControllers(Luxena.sd.User, function (se) { return ({
            list: [
                se.Person,
                se.Name,
                se.Roles,
                se.Description,
            ],
            view: [
                se.Person,
                se.Name,
                se.Roles,
                se.Description,
                se.Password,
            ],
            edit: [
                se.Person,
                se.Name,
                se.NewPassword,
                se.ConfirmPassword,
                se.Roles,
                se.Description,
            ],
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.Sequence, function (se) { return [
            se.Name,
            se.Format,
            se.Discriminator,
            se.Current,
            se.Timestamp,
        ]; });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.SystemConfiguration, function (se) { return ({
            form: {
                Company: [
                    se.Company,
                    se.CompanyDetails,
                    se.AccountantDisplayString,
                    se.Country,
                    se.DefaultCurrency,
                    se.UseDefaultCurrencyForInput,
                    se.VatRate,
                ],
                Products: [
                    se.UseAviaHandling,
                    se.IsPassengerPassportRequired,
                    se.AviaDocumentVatOptions,
                    se.NeutralAirlineCode,
                ],
                Orders: [
                    se.AviaOrderItemGenerationOption,
                    se.AmadeusRizUsingMode,
                    se.IncomingCashOrderCorrespondentAccount,
                    se.DaysBeforeDeparture,
                    se.MetricsFromDate,
                    se.UseAviaDocumentVatInOrder,
                    se.AllowAgentSetOrderVat,
                    se.SeparateDocumentAccess,
                    se.IsOrderRequiredForProcessedDocument,
                    se.ReservationsInOfficeMetrics,
                    se.McoRequiresDescription,
                    se.Order_UseServiceFeeOnlyInVat,
                ],
                Invoices: [
                    se.Invoice_NumberMode,
                    se.InvoicePrinter_FooterDetails,
                ],
                Rest: [
                    se.BirthdayTaskResponsible,
                    se.IsOrganizationCodeRequired,
                ],
            },
            formScope: function (ctrl) { return ({
                tabs: [
                    { template: "Company", title: "Турагенство", },
                    Luxena.sd.Product.toTabs(),
                    Luxena.sd.Order.toTabs(),
                    Luxena.sd.Invoice.toTabs(),
                    { template: "Rest", title: "Прочее", },
                ]
            }); },
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Luxena.sd.GdsAgent._lookupItemTemplate = function (r) { return ("<b class=\"span-150\">" + r.Person + "</b> " + r.Codes); };
        Views.registerEntityControllers(Luxena.sd.GdsAgent, function (se) { return ({
            members: [
                se.Person,
                se.Origin,
                se.OfficeCode,
                se.Code,
                se.Office,
            ],
            actions: [
                Luxena.sd.GdsAgent_ApplyToUnassigned.toAction(function (prms) { return ({
                    GdsAgentId: prms.id
                }); }),
            ],
        }); });
        Views.registerEntityControllers(Luxena.sd.GdsAgent_ApplyToUnassigned, function (se) { return ({
            edit: [
                se.GdsAgent,
                se.DateFrom,
                se.DateTo,
                se.ProductCount,
            ],
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.GdsFile, function (se) { return ({
            members: [
                se.TimeStamp,
                se.Name,
                se.FileType,
                se.ImportResult,
                se.ImportOutput,
                se.CreatedOn,
            ],
            form: [
                se.TimeStamp,
                se.Name,
                se.FileType,
                se.ImportResult,
                se.ImportOutput,
                se.Content,
            ],
            smart: [
                se.TimeStamp,
                se.Name,
                se.FileType,
                se.ImportResult,
                se.ImportOutput,
            ],
            viewScope: function (ctrl) { return ({
                tabs: [
                    se.Products.toTab(ctrl, function (a) { return [a.CreatedBy, a.Type, a.Name,]; }),
                ]
            }); },
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.Agent, function (se) { return ({
            list: [
                se.Name,
            ],
            view: {
                "fields1": [
                    se.Name,
                    se.LegalName,
                    se.Title,
                    se.Organization,
                    se.ReportsTo,
                ],
                "fields2": [
                    se.Note,
                ],
                "Contacts1": se.Contacts,
                "Contacts2": se.Addresses,
            },
            viewScope: function (ctrl) { return ({
                tabs: [
                    se.Contacts.toTab(),
                    se.GdsAgents.toTab(ctrl),
                ]
            }); },
            edit: Luxena.sd.Person,
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    Luxena.sd.Airline._lookupItemTemplate = function (r) { return ("<b class=\"span-40\">" + r.IataCode + "</b>" + r.Name); };
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.Airline, function (se) { return ({
            list: [
                se.AirlineIataCode,
                se.AirlinePrefixCode,
                se.Name,
            ],
            form: function () { return ({
                "fields1": [
                    se.Name,
                    se.LegalName,
                    se.Code,
                ],
                "fields2": [
                    se.AirlineIataCode,
                    se.AirlinePrefixCode,
                    se.AirlinePassportRequirement,
                ],
                "fields3": [
                    se.Note,
                ],
                "Contacts1": se.Contacts,
                "Contacts2": se.Addresses,
            }); },
            viewScope: function (ctrl) { return ({
                tabs: [
                    se.Contacts.toTab(),
                    se.AirlineServiceClasses.toTab(ctrl, function (a) { return [a.Code, a.ServiceClass,]; }),
                    se.MilesCards.toTab(ctrl, function (a) { return [a.Owner, a.Number,]; }),
                ]
            }); },
            editScope: function (ctrl) { return ({
                tabs: [
                    se.Contacts.toTab(),
                ]
            }); },
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.Customer, function (se) { return ({
            list: [
                se.Type,
                se.Name,
            ],
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers([
            Luxena.sd.Organization,
            Luxena.sd.InsuranceCompany,
            Luxena.sd.RoamingOperator,
            Luxena.sd.AccommodationProvider,
            Luxena.sd.BusTicketProvider,
            Luxena.sd.CarRentalProvider,
            Luxena.sd.GenericProductProvider,
            Luxena.sd.PasteboardProvider,
            Luxena.sd.TourProvider,
            Luxena.sd.TransferProvider,
        ], function (se) { return ({
            list: [
                se.Name,
                se.IsCustomer,
                se.IsSupplier,
            ],
            form: function () { return ({
                "fields1": [
                    se.Name,
                    se.LegalName,
                    se.Code,
                    se.IsCustomer,
                    se.IsSupplier,
                ],
                "fields2": [
                    //se.ReportsTo,
                    se.Note.lineCount(4),
                    se.DefaultBankAccount,
                ],
                "Contacts1": se.Contacts,
                "Contacts2": se.Addresses,
                "Providers1": [
                    se.IsBusTicketProvider,
                    se.IsCarRentalProvider,
                    se.IsPasteboardProvider,
                    se.IsAccommodationProvider,
                    se.IsTourProvider,
                    se.IsTransferProvider,
                    se.IsGenericProductProvider,
                ],
                "Providers2": [
                    se.IsInsuranceCompany,
                    se.IsRoamingOperator,
                    se.IsAirline,
                    se.AirlineIataCode,
                    se.AirlinePrefixCode,
                    se.AirlinePassportRequirement,
                ],
            }); },
            viewScope: function (ctrl) { return ({
                tabs: [
                    se.Contacts.toTab(),
                    {
                        title: "Провайдер услуг",
                        template: "Providers",
                    },
                    se.Persons.toTab(ctrl, function (a) { return [a.Name,]; }),
                    se.Departments.toTab(ctrl, function (a) { return [a.Name,]; }),
                ]
            }); },
            editScope: function (ctrl) { return ({
                tabs: [
                    se.Contacts.toTab(),
                    {
                        title: "Провайдер услуг",
                        template: "Providers",
                    },
                ]
            }); },
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    Luxena.Ui.fieldSet2(Luxena.sd.Party, {
        name: "Contacts",
        title: "Контакты",
        members: function (se) { return [
            se.Phone1,
            se.Phone2,
            se.Email1,
            se.Email2,
            se.Fax,
            se.WebAddress,
        ]; }
    });
    Luxena.Ui.fieldSet2(Luxena.sd.Party, {
        name: "Addresses",
        title: "Адреса",
        members: function (se) { return [
            se.ActualAddress,
            se.LegalAddress,
        ]; }
    });
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.Party, function (se) { return ({
            list: [
                se.Type,
                se.Name,
                se.IsCustomer,
                se.IsSupplier,
            ],
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.Person, function (se) { return ({
            list: [
                se.Name,
                se.IsCustomer,
                se.IsSupplier,
            ],
            form: function () { return ({
                "fields1": [
                    se.Name,
                    se.LegalName,
                    se.Title,
                    se.IsCustomer,
                    se.IsSupplier,
                    se.Organization,
                    se.ReportsTo,
                ],
                "fields2": [
                    se.BonusCardNumber,
                    se.Note.lineCount(5),
                    se.DefaultBankAccount,
                ],
                "Contacts1": se.Contacts,
                "Contacts2": se.Addresses,
            }); },
            viewScope: function (ctrl) { return ({
                tabs: [
                    se.Contacts.toTab(),
                    se.MilesCards.toTab(ctrl, function (a) { return [a.Number, a.Organization]; }),
                    se.Passports.toTab(ctrl, function (a) { return [a.Number, a.Name, a.Citizenship]; }),
                ]
            }); },
            editScope: function (ctrl) { return ({
                tabs: [
                    se.Contacts.toTab(),
                ]
            }); },
        }); });
        Views.registerEntityControllers(Luxena.sd.MilesCard, function (se) { return [se.Owner, se.Number, se.Organization,]; });
        Views.registerEntityControllers(Luxena.sd.Passport, function (se) { return ({
            list: [
                se.Number,
                se.Owner,
                se.LastName,
                se.FirstName,
                se.MiddleName,
                se.Citizenship,
            ],
            view: [
                se.Owner,
                se.Number,
                se.Name,
                se.Citizenship,
                se.Birthday,
                se.Gender,
                se.IssuedBy,
                se.ExpiredOn,
                se.Note,
                se.AmadeusString,
                se.GalileoString,
            ],
            edit: [
                se.Owner,
                se.Number,
                se.FirstName,
                se.MiddleName,
                se.LastName,
                se.Citizenship,
                se.Birthday,
                se.Gender,
                se.IssuedBy,
                se.ExpiredOn,
                se.Note,
            ],
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.Accommodation, function (se) { return ({
            list: [
                se.IssueDate,
                se.Name,
                se.PassengerName,
                se.Provider,
                se.Customer,
                se.Order,
                se.Total,
                se.ServiceFee,
                se.GrandTotal,
            ],
            view: {
                "fields1": [
                    se.IssueDate,
                    se.ReissueFor,
                    se.PassengerRow,
                    se.CustomerAndOrder,
                    se.Intermediary,
                    se.Provider,
                    se.StartDate,
                    se.FinishDate,
                    se.Country,
                    se.PnrCode,
                    se.TourCode,
                    se.HotelName, se.HotelOffice, se.HotelCode,
                    se.PlacementName, se.PlacementOffice, se.PlacementCode,
                    se.AccommodationType,
                    se.CateringType,
                    se.SellerAndOwner,
                ],
                "fields2": se.Finance,
                "fields3": se.Note,
            },
            edit: {
                "fields1": [
                    se.IssueDateAndReissueFor,
                    se.PassengerRow,
                    se.CustomerAndOrder,
                    se.Intermediary,
                    se.Provider,
                    se.StartAndFinishDate,
                    se.Country,
                    se.PnrAndTourCode,
                    Luxena.Ui.fieldRow(se, se.HotelName, [se.HotelName, se.HotelOffice, se.HotelCode,]),
                    Luxena.Ui.fieldRow(se, se.PlacementName, [se.PlacementName, se.PlacementOffice, se.PlacementCode,]),
                    Luxena.Ui.fieldSet(se, null, [se.AccommodationType], [se.CateringType]),
                    se.SellerAndOwner,
                ],
                "fields2": se.Finance,
                "fields3": se.Note,
            }
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    Luxena.Ui.fieldRow2(Luxena.sd.AviaDocument, {
        name: "NumberRow",
        title: "Номер / Авиакомпания",
        members: function (se) { return [se.AirlinePrefixCode, se.Number, se.Producer,]; },
    });
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.AviaDocument, function (se) { return ({
            list: [
                se.IssueDate,
                se.Type,
                se.Name,
                se.PassengerName,
                se.Itinerary,
                se.Customer,
                se.Order,
                se.Total,
                se.ServiceFee,
                se.GrandTotal,
            ],
        }); });
        Views.registerEntityControllers([Luxena.sd.AviaRefund, Luxena.sd.AviaMco], function (se) { return ({
            list: Luxena.sd.AviaDocument,
            view: {
                "fields1": [
                    se.IssueDate,
                    se.ReissueFor,
                    Luxena.Ui.fieldRow(se, "/", function () { return [se.FullNumber, se.Producer,]; }),
                    se.PassengerRow,
                    se.Itinerary,
                    se.CustomerAndOrder,
                    se.Intermediary,
                    se.GdsPassportStatus,
                    se.PnrAndTourCode,
                    se.BookerAndTicketer,
                    se.SellerAndOwner,
                    se.OriginalDocument,
                ],
                "fields2": se.Finance,
                "fields3": se.Note,
            },
            edit: {
                "fields1": [
                    se.IssueDateAndReissueFor,
                    se.NumberRow,
                    se.PassengerRow,
                    se.CustomerAndOrder,
                    se.Intermediary,
                    Luxena.Ui.fieldSet(se, null, [se.PnrCode, se.TourCode,], [se.GdsPassportStatus, se.Originator,]),
                    se.BookerRow,
                    se.TicketerRow,
                    se.SellerAndOwner,
                ],
                "fields2": se.Finance,
                "fields3": se.Note,
            },
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.AviaDocument, function (se) { return ({
            list: [
                se.IssueDate,
                se.Type,
                se.Name,
                se.PassengerName,
                se.Itinerary,
                se.Customer,
                se.Order,
                se.Total,
                se.ServiceFee,
                se.GrandTotal,
            ],
        }); });
        Views.registerEntityControllers(Luxena.sd.AviaTicket, function (se) { return ({
            list: Luxena.sd.AviaDocument,
            formTitle: se.FullNumber,
            view: function () { return ({
                "fields1": [
                    se.IssueDate,
                    Luxena.Ui.fieldRow(se, "/", function () { return [se.FullNumber, se.Producer,]; }),
                    se.ReissueFor,
                    se.PassengerRow,
                    se.Itinerary,
                    se.CustomerAndOrder,
                    se.Intermediary,
                    se.GdsPassportStatus,
                    se.PnrCode,
                    se.TourCode,
                    se.Booker,
                    se.Ticketer,
                    se.SellerAndOwner,
                    se.OriginalDocument,
                ],
                "fields2": se.Finance,
                "fields3": se.Note,
            }); },
            edit: function () { return ({
                "fields1": [
                    se.IssueDateAndReissueFor,
                    se.NumberRow,
                    se.PassengerRow,
                    se.CustomerAndOrder,
                    se.Intermediary,
                    Luxena.Ui.fieldSet(se, null, [se.PnrCode, se.TourCode,], [se.GdsPassportStatus, se.Originator,]),
                    se.BookerRow,
                    se.TicketerRow,
                    se.SellerAndOwner,
                ],
                "fields2": se.Finance,
                "fields3": se.Note,
            }); },
            viewScope: function (ctrl) { return ({
                tabs: [
                    se.Segments.toTab(ctrl, function (a) { return [
                        a.Position,
                        a.FromAirport,
                        a.ToAirport,
                        a.Carrier,
                        a.FlightNumber,
                        a.Seat,
                        a.ServiceClass,
                        a.DepartureTime,
                        a.ArrivalTime,
                        a.Duration,
                        a.FareBasis,
                        a.Luggage,
                        a.MealTypes,
                        a.CouponAmount,
                    ]; })
                ],
            }); },
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.BusDocument, function (se) { return ({
            list: [
                se.IssueDate,
                se.Type,
                se.Name,
                se.PassengerName,
                se.Provider,
                se.Customer,
                se.Order,
                se.Total,
                se.ServiceFee,
                se.GrandTotal,
            ],
        }); });
        Views.registerEntityControllers([Luxena.sd.BusTicket, Luxena.sd.BusTicketRefund], function (se) { return ({
            list: Luxena.sd.BusDocument,
            view: {
                "fields1": [
                    se.IssueDate,
                    se.ReissueFor,
                    se.PassengerRow,
                    se.CustomerAndOrder,
                    se.Intermediary,
                    se.Provider,
                    Luxena.Ui.fieldRow(se, "Отправление", [se.DeparturePlace, se.DepartureDate, se.DepartureTime]),
                    Luxena.Ui.fieldRow(se, "Прибытие", [se.ArrivalPlace, se.ArrivalDate, se.ArrivalTime]),
                    se.SeatNumber,
                    se.SellerAndOwner,
                ],
                "fields2": se.Finance,
                "fields3": se.Note,
            },
            edit: {
                "fields1": [
                    se.IssueDateAndReissueFor,
                    se.PassengerRow,
                    se.CustomerAndOrder,
                    se.Intermediary,
                    se.Provider,
                    Luxena.Ui.fieldRow(se, "Отправление", [se.DeparturePlace, se.DepartureDate, se.DepartureTime]),
                    Luxena.Ui.fieldRow(se, "Прибытие", [se.ArrivalPlace, se.ArrivalDate, se.ArrivalTime]),
                    se.SeatNumber,
                    se.SellerAndOwner,
                ],
                "fields2": se.Finance,
                "fields3": se.Note,
            },
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.CarRental, function (se) { return ({
            list: [
                se.IssueDate,
                se.Name,
                se.PassengerName,
                se.Producer,
                se.Customer,
                se.Order,
                se.Total,
                se.ServiceFee,
                se.GrandTotal,
            ],
            view: {
                "fields1": [
                    se.IssueDate,
                    se.ReissueFor,
                    se.PassengerRow,
                    se.CustomerAndOrder,
                    se.Intermediary,
                    se.Producer,
                    se.StartDate,
                    se.FinishDate,
                    se.CarBrand,
                    se.Country,
                    se.PnrCode,
                    se.TourCode,
                    se.SellerAndOwner,
                ],
                "fields2": se.Finance,
                "fields3": se.Note,
            },
            edit: {
                "fields1": [
                    se.IssueDateAndReissueFor,
                    se.PassengerRow,
                    se.CustomerAndOrder,
                    se.Intermediary,
                    se.Producer,
                    se.StartAndFinishDate,
                    se.CarBrand,
                    se.Country,
                    se.PnrAndTourCode,
                    se.SellerAndOwner,
                ],
                "fields2": se.Finance,
                "fields3": se.Note,
            },
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.Excursion, function (se) { return ({
            list: [
                se.IssueDate,
                se.Name,
                se.PassengerName,
                se.Customer,
                se.Order,
                se.Total,
                se.ServiceFee,
                se.GrandTotal,
            ],
            view: {
                "fields1": [
                    se.IssueDate,
                    se.ReissueFor,
                    se.PassengerRow,
                    se.CustomerAndOrder,
                    se.Intermediary,
                    se.StartDate,
                    se.FinishDate,
                    se.TourName,
                    se.Country,
                    se.PnrCode,
                    se.TourCode,
                    se.SellerAndOwner,
                ],
                "fields2": se.Finance,
                "fields3": se.Note,
            },
            edit: {
                "fields1": [
                    se.IssueDateAndReissueFor,
                    se.PassengerRow,
                    se.CustomerAndOrder,
                    se.Intermediary,
                    se.StartAndFinishDate,
                    se.TourName,
                    se.Country,
                    se.PnrAndTourCode,
                    se.SellerAndOwner,
                ],
                "fields2": se.Finance,
                "fields3": se.Note,
            },
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.FlightSegment, function (se) { return ({
            list: [
                se.CreatedOn.clone().entityDate(),
                se.Ticket,
                se.FromAirport,
                se.ToAirport,
                se.Carrier,
                se.FlightNumber,
                //se.Seat,
                se.ServiceClass,
                se.DepartureTime,
                se.ArrivalTime,
                se.Duration,
                se.FareBasis,
                se.Luggage,
                //se.MealTypes,
                se.CouponAmount,
            ],
            form: {
                "fields1": [
                    se.Ticket,
                    Luxena.Ui.fieldRow(se, se.FromAirport, function (a) { return [a.FromAirportCode, a.FromAirport]; }),
                    Luxena.Ui.fieldRow(se, se.ToAirport, function (a) { return [a.ToAirportCode, a.ToAirport]; }),
                    se.Carrier,
                    Luxena.Ui.fieldRow(se, "/", function (a) { return [a.FlightNumber, a.Seat]; }),
                    Luxena.Ui.fieldRow(se, se.ServiceClass, function (a) { return [a.ServiceClassCode, a.ServiceClass]; }),
                ],
                "fields2": [
                    se.DepartureTime,
                    se.CheckInTime,
                    se.CheckInTerminal,
                    se.ArrivalTime,
                    se.ArrivalTerminal,
                    se.MealTypes,
                    se.CouponAmount,
                ],
            },
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.GenericProduct, function (se) { return ({
            list: [
                se.IssueDate,
                se.Name,
                se.PassengerName,
                se.Provider,
                se.Customer,
                se.Order,
                se.Total,
                se.ServiceFee,
                se.GrandTotal,
            ],
            view: {
                "fields1": [
                    se.IssueDate,
                    se.ReissueFor,
                    se.PassengerRow,
                    se.CustomerAndOrder,
                    se.Intermediary,
                    se.Provider,
                    se.GenericType,
                    se.Number,
                    se.StartDate,
                    se.FinishDate,
                    se.Country,
                    se.PnrCode,
                    se.TourCode,
                    se.SellerAndOwner,
                ],
                "fields2": se.Finance,
                "fields3": se.Note,
            },
            edit: {
                "fields1": [
                    se.IssueDateAndReissueFor,
                    se.PassengerRow,
                    se.CustomerAndOrder,
                    se.Intermediary,
                    se.Provider,
                    se.GenericType,
                    se.Number,
                    se.StartAndFinishDate,
                    se.Country,
                    se.PnrAndTourCode,
                    se.SellerAndOwner,
                ],
                "fields2": se.Finance,
                "fields3": se.Note,
            },
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.InsuranceDocument, function (se) { return ({
            list: [
                se.IssueDate,
                se.Type,
                se.Name,
                se.PassengerName,
                se.Provider,
                se.Customer,
                se.Order,
                se.Total,
                se.ServiceFee,
                se.GrandTotal,
            ],
        }); });
        Views.registerEntityControllers([Luxena.sd.Insurance, Luxena.sd.InsuranceRefund], function (se) { return ({
            list: Luxena.sd.InsuranceDocument,
            view: {
                "fields1": [
                    se.IssueDate,
                    se.ReissueFor,
                    se.PassengerRow,
                    se.CustomerAndOrder,
                    se.Intermediary,
                    se.Producer,
                    se.StartDate,
                    se.FinishDate,
                    se.Country,
                    se.PnrCode,
                    se.TourCode,
                    se.SellerAndOwner,
                ],
                "fields2": se.Finance,
                "fields3": se.Note,
            },
            edit: {
                "fields1": [
                    se.IssueDateAndReissueFor,
                    se.PassengerRow,
                    se.CustomerAndOrder,
                    se.Intermediary,
                    se.Producer,
                    se.StartAndFinishDate,
                    se.Country,
                    se.PnrAndTourCode,
                    se.SellerAndOwner,
                ],
                "fields2": se.Finance,
                "fields3": se.Note,
            },
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.Isic, function (se) { return ({
            members: [
                se.IssueDate,
                se.Name,
                se.PassengerName,
                se.Customer,
                se.Order,
                se.Total,
                se.ServiceFee,
                se.GrandTotal,
            ],
            view: {
                "fields1": [
                    se.IssueDate,
                    se.ReissueFor,
                    se.PassengerRow,
                    se.CustomerAndOrder,
                    se.Intermediary,
                    se.CardType,
                    se.Number1,
                    se.Number2,
                    se.SellerAndOwner,
                ],
                "fields2": se.Finance,
                "fields3": se.Note,
            },
            edit: {
                "fields1": [
                    se.IssueDateAndReissueFor,
                    se.PassengerRow,
                    se.CustomerAndOrder,
                    se.Intermediary,
                    se.CardType,
                    se.Number1,
                    se.Number2,
                    se.SellerAndOwner,
                ],
                "fields2": se.Finance,
                "fields3": se.Note,
            },
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.RailwayDocument, function (se) { return ({
            list: [
                se.IssueDate,
                se.Type,
                se.Name,
                se.PassengerName,
                se.Provider,
                se.Customer,
                se.Order,
                se.Total,
                se.ServiceFee,
                se.GrandTotal,
            ],
        }); });
        Views.registerEntityControllers([Luxena.sd.Pasteboard, Luxena.sd.PasteboardRefund], function (se) { return ({
            list: Luxena.sd.RailwayDocument,
            view: {
                "fields1": [
                    se.IssueDate,
                    se.ReissueFor,
                    Luxena.Ui.fieldRow(se, "/", [se.Number, se.Provider,]),
                    se.PassengerRow,
                    se.CustomerAndOrder,
                    se.Intermediary,
                    Luxena.Ui.fieldRow(se, se.DeparturePlace, [se.DeparturePlace, se.DepartureDate, se.DepartureTime,]),
                    Luxena.Ui.fieldRow(se, se.ArrivalPlace, [se.ArrivalPlace, se.ArrivalDate, se.ArrivalTime,]),
                    Luxena.Ui.fieldSet(se, null, [se.ServiceClass, se.TrainNumber], [se.CarNumber, se.SeatNumber]),
                    se.BookerRow,
                    se.TicketerRow,
                    se.SellerAndOwner,
                ],
                "fields2": se.Finance,
                "fields3": se.Note,
            },
            edit: {
                "fields1": [
                    se.IssueDateAndReissueFor,
                    Luxena.Ui.fieldRow(se, "/", [se.Number, se.Provider,]),
                    se.PassengerRow,
                    se.CustomerAndOrder,
                    se.Intermediary,
                    Luxena.Ui.fieldRow(se, se.DeparturePlace, [se.DeparturePlace, se.DepartureDate, se.DepartureTime,]),
                    Luxena.Ui.fieldRow(se, se.ArrivalPlace, [se.ArrivalPlace, se.ArrivalDate, se.ArrivalTime,]),
                    Luxena.Ui.fieldSet(se, null, [se.ServiceClass, se.TrainNumber], [se.CarNumber, se.SeatNumber]),
                    se.BookerRow,
                    se.TicketerRow,
                    se.SellerAndOwner,
                ],
                "fields2": se.Finance,
                "fields3": se.Note,
            },
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.SimCard, function (se) { return ({
            list: [
                se.IssueDate,
                se.Name,
                se.PassengerName,
                se.Producer,
                se.Customer,
                se.Order,
                se.Total,
                se.ServiceFee,
                se.GrandTotal,
            ],
            view: {
                "fields1": [
                    se.IssueDate,
                    se.ReissueFor,
                    se.PassengerRow,
                    se.CustomerAndOrder,
                    se.Intermediary,
                    se.Number,
                    se.Producer,
                    se.IsSale,
                    se.SellerAndOwner,
                ],
                "fields2": se.Finance,
                "fields3": se.Note,
            },
            edit: {
                "fields1": [
                    se.IssueDateAndReissueFor,
                    se.PassengerRow,
                    se.CustomerAndOrder,
                    se.Intermediary,
                    se.Number,
                    se.Producer,
                    se.IsSale,
                    se.SellerAndOwner,
                ],
                "fields2": se.Finance,
                "fields3": se.Note,
            },
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.Tour, function (se) { return ({
            list: [
                se.IssueDate,
                se.Name,
                se.PassengerName,
                se.Producer,
                se.Customer,
                se.Order,
                se.Total,
                se.ServiceFee,
                se.GrandTotal,
            ],
            view: {
                "fields1": [
                    se.IssueDate,
                    se.ReissueFor,
                    se.PassengerRow,
                    se.CustomerAndOrder,
                    se.Intermediary,
                    se.Producer,
                    se.StartDate,
                    se.FinishDate,
                    se.Country,
                    se.PnrCode,
                    se.TourCode,
                    se.HotelName,
                    se.HotelOffice,
                    se.HotelCode,
                    se.PlacementName,
                    se.PlacementOffice,
                    se.PlacementCode,
                    se.AccommodationType,
                    se.CateringType,
                    se.AviaDescription,
                    se.TransferDescription,
                    se.SellerAndOwner,
                ],
                "fields2": se.Finance,
                "fields3": se.Note,
            },
            edit: {
                "fields1": [
                    se.IssueDateAndReissueFor,
                    se.PassengerRow,
                    se.CustomerAndOrder,
                    se.Intermediary,
                    se.Producer,
                    se.StartAndFinishDate,
                    se.Country,
                    se.PnrAndTourCode,
                    Luxena.Ui.fieldRow(se, se.HotelName, [se.HotelName, se.HotelOffice, se.HotelCode,]),
                    Luxena.Ui.fieldRow(se, se.PlacementName, [se.PlacementName, se.PlacementOffice, se.PlacementCode,]),
                    Luxena.Ui.fieldSet(se, null, [se.AccommodationType], [se.CateringType]),
                    se.AviaDescription,
                    se.TransferDescription,
                    se.SellerAndOwner,
                ],
                "fields2": se.Finance,
                "fields3": se.Note,
            },
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.Transfer, function (se) { return ({
            list: [
                se.IssueDate,
                se.Name,
                se.PassengerName,
                se.Customer,
                se.Order,
                se.Total,
                se.ServiceFee,
                se.GrandTotal,
            ],
            view: {
                "fields1": [
                    se.IssueDate,
                    se.ReissueFor,
                    se.PassengerRow,
                    se.CustomerAndOrder,
                    se.Intermediary,
                    se.Provider,
                    se.StartDate,
                    se.Country,
                    se.PnrCode,
                    se.TourCode,
                    se.SellerAndOwner,
                ],
                "fields2": se.Finance,
                "fields3": se.Note,
            },
            edit: {
                "fields1": [
                    se.IssueDateAndReissueFor,
                    se.PassengerRow,
                    se.CustomerAndOrder,
                    se.Intermediary,
                    se.Provider,
                    se.StartDate,
                    se.Country,
                    se.PnrAndTourCode,
                    se.SellerAndOwner,
                ],
                "fields2": se.Finance,
                "fields3": se.Note,
            },
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.Airport, function (se) { return ({
            list: [
                se.Code,
                se.Name,
                se.Country,
                se.Settlement,
            ],
            view: function () {
                return ({
                    "fields1": [
                        se.Code,
                        se.Name,
                        se.Country,
                        se.Settlement,
                        se.LocalizedSettlement,
                    ],
                    "fields2": [
                        se.Id,
                        se.CreatedOn,
                        se.CreatedBy,
                        se.ModifiedOn,
                        se.ModifiedBy,
                    ]
                });
            },
            edit: [
                se.Code,
                se.Name,
                se.Country,
                se.Settlement,
                se.LocalizedSettlement,
            ],
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.Country, function (se) { return ({
            members: [
                se.TwoCharCode,
                se.ThreeCharCode,
                se.Name,
            ],
            viewScope: function (ctrl) { return ({
                tabs: [
                    se.Airports.toTab(ctrl, function (a) { return [a.Code, a.Name, a.Settlement,]; }),
                ]
            }); },
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
//# sourceMappingURL=/app/_app.js.map