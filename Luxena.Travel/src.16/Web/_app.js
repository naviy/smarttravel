/* tsUnit (c) Copyright 2012-2015 Steve Fenton, licensed under Apache 2.0 https://github.com/Steve-Fenton/tsUnit */
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
            this.passes = [];
            this.errors = [];
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
                            this.runSingleTest(testClass, unitTestName, testsGroupName, parameters, parameterIndex);
                        }
                    }
                    else {
                        this.runSingleTest(testClass, unitTestName, testsGroupName);
                    }
                }
            }
            return this;
        };
        Test.prototype.showResults = function (target) {
            var elem;
            if (typeof target === 'string') {
                var id = target;
                elem = document.getElementById(id);
            }
            else {
                elem = target;
            }
            var template = '<article>' +
                '<h1>' + this.getTestResult() + '</h1>' +
                '<p>' + this.getTestSummary() + '</p>' +
                this.testRunLimiter.getLimitCleaner() +
                '<section id="tsFail">' +
                '<h2>Errors</h2>' +
                '<ul class="bad">' + this.getTestResultList(this.errors) + '</ul>' +
                '</section>' +
                '<section id="tsOkay">' +
                '<h2>Passing Tests</h2>' +
                '<ul class="good">' + this.getTestResultList(this.passes) + '</ul>' +
                '</section>' +
                '</article>' +
                this.testRunLimiter.getLimitCleaner();
            elem.innerHTML = template;
            return this;
        };
        Test.prototype.getTapResults = function () {
            var newLine = '\r\n';
            var template = '1..' + (this.passes.length + this.errors.length).toString() + newLine;
            for (var i = 0; i < this.errors.length; i++) {
                template += 'not ok ' + this.errors[i].message + ' ' + this.errors[i].testName + newLine;
            }
            for (var i = 0; i < this.passes.length; i++) {
                template += 'ok ' + this.passes[i].testName + newLine;
            }
            return template;
        };
        Test.prototype.createTestLimiter = function () {
            try {
                if (typeof window !== 'undefined') {
                    this.testRunLimiter = new TestRunLimiter();
                }
            }
            catch (ex) {
            }
        };
        Test.prototype.isReservedFunctionName = function (functionName) {
            for (var prop in this.reservedMethodNameContainer) {
                if (prop === functionName) {
                    return true;
                }
            }
            return false;
        };
        Test.prototype.runSingleTest = function (testClass, unitTestName, testsGroupName, parameters, parameterSetIndex) {
            if (parameters === void 0) { parameters = null; }
            if (parameterSetIndex === void 0) { parameterSetIndex = null; }
            if (typeof testClass['setUp'] === 'function') {
                testClass['setUp']();
            }
            try {
                var dynamicTestClass = testClass;
                var args = (parameterSetIndex !== null) ? parameters[parameterSetIndex] : null;
                dynamicTestClass[unitTestName].apply(testClass, args);
                this.passes.push(new TestDescription(testsGroupName, unitTestName, parameterSetIndex, 'OK'));
            }
            catch (err) {
                this.errors.push(new TestDescription(testsGroupName, unitTestName, parameterSetIndex, err.toString()));
            }
            if (typeof testClass['tearDown'] === 'function') {
                testClass['tearDown']();
            }
        };
        Test.prototype.getTestResult = function () {
            return this.errors.length === 0 ? 'Test Passed' : 'Test Failed';
        };
        Test.prototype.getTestSummary = function () {
            return 'Total tests: <span id="tsUnitTotalCout">' + (this.passes.length + this.errors.length).toString() + '</span>. ' +
                'Passed tests: <span id="tsUnitPassCount" class="good">' + this.passes.length + '</span>. ' +
                'Failed tests: <span id="tsUnitFailCount" class="bad">' + this.errors.length + '</span>.';
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
            if (typeof a === 'function') {
                actual = a;
            }
            else if (a.fn) {
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
        TestContext.prototype.doesNotThrow = function (actual, message) {
            try {
                actual();
            }
            catch (ex) {
                throw this.getError('threw an error ' + ex, message || '');
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
})(tsUnit || (tsUnit = {}));
var Luxena;
(function (Luxena) {
    var Utils;
    (function (Utils) {
        Utils.reClassName = /\w+/g;
    })(Utils = Luxena.Utils || (Luxena.Utils = {}));
})(Luxena || (Luxena = {}));
function classNameOf(obj) {
    return obj && obj.constructor.toString().match(Luxena.Utils.reClassName)[1];
}
Array.prototype.register = function () {
    var items = [];
    for (var _i = 0; _i < arguments.length; _i++) {
        items[_i - 0] = arguments[_i];
    }
    if (!items)
        return;
    if (items.length === 1 && items[0] instanceof Array)
        items = items[0];
    for (var _a = 0; _a < items.length; _a++) {
        var item = items[_a];
        if (item !== undefined && this.indexOf(item) < 0)
            this.push(item);
    }
};
function $as(a, action, catchAction) {
    if (!catchAction) {
        return a && action && action(a);
    }
    else {
        try {
            return a && action && action(a);
        }
        catch (ex) {
            catchAction(a, ex);
        }
    }
}
function $do(a, action, catchAction) {
    if (!catchAction) {
        a && action && action(a);
        return a;
    }
    else {
        try {
            a && action && action(a);
            return a;
        }
        catch (ex) {
            catchAction(a, ex);
            return a;
        }
    }
}
var $extend = $.extend;
function $append(target) {
    var objects = [];
    for (var _i = 1; _i < arguments.length; _i++) {
        objects[_i - 1] = arguments[_i];
    }
    if (objects && objects.length) {
        if (!target)
            target = {};
        for (var _a = 0; _a < objects.length; _a++) {
            var obj = objects[_a];
            if (!obj)
                continue;
            for (var _b = 0, _c = Object.getOwnPropertyNames(obj); _b < _c.length; _b++) {
                var prop = _c[_b];
                if (target[prop] === undefined)
                    target[prop] = obj[prop];
            }
        }
    }
    return target;
}
function $position(obj) {
    if (!obj.offsetParent)
        return null;
    var y = 0;
    var x = 0;
    do {
        x += obj.offsetLeft;
        y += obj.offsetTop;
    } while (obj = obj.offsetParent);
    return { x: x, y: y };
}
function $clip(obj) {
    return typeof obj === "string" ? obj.trim() : obj;
}
function $clone(obj, cfg) {
    return $.extend({}, obj, cfg);
}
function $div(className) {
    return className ? $("<div class=\"" + className + "\">") : $("<div>");
}
function isArray(obj) {
    return obj instanceof Array;
}
function isFunction(obj) {
    return typeof obj === "function";
}
function isString(obj) {
    return typeof obj === "string";
}
//declare var DX;
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
ko.unwrap3 = function (obj) {
    if (!obj)
        return obj;
    obj = ko.unwrap(obj);
    var r = {};
    var propCount = 0;
    for (var name in obj) {
        if (!obj.hasOwnProperty(name))
            continue;
        r[name] = ko.unwrap2(obj[name]);
        propCount++;
    }
    return propCount > 0 ? r : obj;
};
ko.unwrap4 = function (obj) {
    if (!obj)
        return obj;
    obj = ko.unwrap(obj);
    var r = {};
    var propCount = 0;
    for (var name in obj) {
        if (!obj.hasOwnProperty(name))
            continue;
        r[name] = ko.unwrap3(obj[name]);
        propCount++;
    }
    return propCount > 0 ? r : obj;
};
ko.bindingHandlers.renderer = {
    update: function (element, valueAccessor, allBindings) {
        var value = ko.unwrap(valueAccessor());
        if (!value)
            return;
        //element = $(element);
        //element.html("");
        value(element);
    }
};
//ko.bindingHandlers.renderer =
//{
//	update(element: JQuery, valueAccessor, allBindings)
//	{
//		let value = ko.unwrap(valueAccessor());
//		if (!value) return;
//		if (value.renderer)
//			value = value.renderer;
//		element = $(element);
//		element.html("");
//		if ($.isFunction(value))
//			value(element);
//		else
//		{
//			for (let containerId in value)
//			{
//				if (!value.hasOwnProperty(containerId)) continue;
//				const container = value[containerId];
//				if ($.isFunction(value))
//					container(element);
//				else if (container.renderer)
//					container.renderer(element);
//			}
//		}
//	}
//};
//ko.bindingHandlers.buttonsCol =
//{
//	update(element, valueAccessor, allBindings)
//	{
//		var buttons: DevExpress.ui.dxButtonOptions[] = ko.unwrap(valueAccessor());
//		if (!buttons) return;
//		var jelement = $(element);
//		jelement.html("");
//		buttons.forEach(btn =>
//		{
//			$(`<div class="smart-action-button">`)
//				.dxButton(btn)
//				.appendTo(jelement);
//		});
//	}
//};
//ko.bindingHandlers.pre =
//{
//	update(element, valueAccessor, allBindings)
//	{
//		const value = ko.unwrap(valueAccessor());
//		if (!value) return;
//		$(element).append($("<pre>").text(value));
//	}
//}; 
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
    $log_(message);
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
            line = " " + line;
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
            console.log(indent2, $log_.caller);
            console.log(stack.join("\n"));
            console.groupEnd();
        }
        else {
            console.log.apply(console, msg);
        }
        return args[0];
    };
    $logb = function (message, action) {
        $log_((message || "") + "{");
        incIndent();
        if (action) {
            var result = action();
            $loge();
            return result;
        }
    };
    $loge = function () {
        decIndent();
        $log_("}");
    };
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
        SemanticObject.prototype.name = function (value) {
            this._name = value;
            return this;
        };
        SemanticObject.prototype.uname = function () {
            return this._uname || (this._uname = this._name + "__" + SemanticObject._unameIndex++);
        };
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
            if (value instanceof SemanticObject)
                this._icon = value._icon || "sticky-note-o";
            else if (value === "props")
                this._icon = "sticky-note-o"; //"info"
            else
                this._icon = value;
            return this;
        };
        SemanticObject.prototype.iconAndTitle = function (icon, title) {
            this._icon = icon;
            this._title = title;
            return this;
        };
        SemanticObject.prototype.localizeTitle = function (value) {
            this._localizeTitle(value);
            return this;
        };
        SemanticObject.prototype.ru = function (value) {
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
        SemanticObject.prototype.title = function (title) {
            title = ko.unwrap(title);
            if (!this._icon && title instanceof SemanticObject && title._icon)
                this._icon = title._icon;
            this._title = semanticTitleToString(title);
            return this;
        };
        SemanticObject.prototype.titleForList = function (se) {
            this._icon = se._icon;
            this._title = se._titles;
            return this;
        };
        SemanticObject.prototype.titlePrefix = function (value) {
            this._title = (value || "") + (this._title || "");
            return this;
        };
        SemanticObject.prototype.titlePostfix = function (value) {
            this._title = (this._title || "") + (value || "");
            return this;
        };
        SemanticObject.prototype.description = function (value) {
            this._description = value;
            return this;
        };
        //#endregion
        SemanticObject.prototype.getIconHtml = function (icon, withTitle) {
            icon = icon || this._icon;
            return icon ? "<i class=\"fa fa-" + icon + " text-icon\"" + (withTitle ? " title=\"" + this._title + "\"" : "") + "></i>" : "";
        };
        SemanticObject._unameIndex = 0;
        return SemanticObject;
    })();
    Luxena.SemanticObject = SemanticObject;
    function semanticTitleToString(title) {
        title = ko.unwrap(title);
        if (title instanceof SemanticObject) {
            var so = title;
            title = so._title;
            if ((!title || title === so._name) && so instanceof Luxena.SemanticMember) {
                //$do(so._lookupGetter && so._lookupGetter(), a => title = a._title);
                //// ReSharper disable once ConditionIsAlwaysConst
                //if (!title)
                $do(so._collectionItemEntity && so._collectionItemEntity(), function (a) { return title = a._titles || a._title; });
            }
        }
        return title;
    }
    Luxena.semanticTitleToString = semanticTitleToString;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var SemanticComponent = (function (_super) {
        __extends(SemanticComponent, _super);
        function SemanticComponent() {
            _super.apply(this, arguments);
            this._visible = true;
            this._columnVisible = true;
            this._selectRequired = true;
        }
        SemanticComponent.prototype.clone = function () {
            var clone = Object.create(this.constructor.prototype);
            for (var attr in this) {
                if (this.hasOwnProperty(attr))
                    clone[attr] = this[attr];
            }
            return clone;
        };
        SemanticComponent.prototype.addItemsToController = function (action) {
        };
        SemanticComponent.prototype.loadFromData = function (model, data) {
        };
        SemanticComponent.prototype.isComposite = function () {
            return this._isComposite;
        };
        SemanticComponent.prototype.toGridColumns = function () {
            return [];
        };
        SemanticComponent.prototype.toGridTotalItems = function () {
            return [];
        };
        SemanticComponent.prototype.renderDisplayStatic = function (container, data) {
            this.render(container);
        };
        SemanticComponent.prototype.getLength = function () {
            return { length: this._length, min: undefined, max: undefined };
        };
        SemanticComponent.prototype.otitle = function (getter) {
            var _this = this;
            if (getter) {
                this._otitleGetter = getter;
                return this;
            }
            if (!this._otitleGetter)
                return null;
            if (!this._otitle)
                this._otitle = ko.computed(function () { return ko.unwrap(_this._otitleGetter(_this._controller.model)); });
            return this._otitle;
        };
        SemanticComponent.prototype.badge = function (getter) {
            var _this = this;
            if (getter) {
                this._badgeGetter = getter;
                return this;
            }
            if (!this._badgeGetter)
                return null;
            if (!this._badge)
                this._badge = ko.computed(function () { return ko.unwrap(_this._badgeGetter(_this._controller.model)); });
            return this._badge;
        };
        //#region Setters
        SemanticComponent.prototype.columnVisible = function (value) {
            this._columnVisible = value !== false;
            return this;
        };
        SemanticComponent.prototype.hideLabel = function (value) {
            this._hideLabel = value !== false;
            return this;
        };
        SemanticComponent.prototype.hideLabelItems = function (value) {
            this._hideLabelItems = value !== false;
            return this;
        };
        SemanticComponent.prototype.indentLabel = function (value) {
            this._indentLabel = value !== false;
            return this;
        };
        SemanticComponent.prototype.indentLabelItems = function (value) {
            this._indentLabelItems = value !== false;
            return this;
        };
        SemanticComponent.prototype.labelAsHeader = function (value) {
            this._labelAsHeader = value !== false;
            return this;
        };
        SemanticComponent.prototype.labelAsHeaderItems = function (value) {
            this._labelAsHeaderItems = value !== false;
            return this;
        };
        SemanticComponent.prototype.length = function (value) {
            this._length = value;
            return this;
        };
        SemanticComponent.prototype.unlabel = function (value) {
            this._unlabel = value !== false;
            return this;
        };
        SemanticComponent.prototype.unlabelItems = function (value) {
            this._unlabelItems = value !== false;
            return this;
        };
        return SemanticComponent;
    })(Luxena.SemanticObject);
    Luxena.SemanticComponent = SemanticComponent;
    function newFieldComponent(se, cfg, creater) {
        if (cfg.name) {
            se.applyToThisAndDerived(function (dse) {
                var sc = creater();
                sc._entity = dse;
                sc._name = cfg.name;
                sc.title(cfg.title);
                dse[cfg.name] = sc;
            });
            return null;
        }
        else {
            var sc = creater();
            sc._entity = se;
            sc.title(cfg.title);
            return sc;
        }
    }
    Luxena.newFieldComponent = newFieldComponent;
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
    var SemanticMember = (function (_super) {
        __extends(SemanticMember, _super);
        function SemanticMember() {
            var _this = this;
            _super.apply(this, arguments);
            this.getLookupEntity = function () { return _this._lookupEntity || (_this._lookupEntity = _this._lookupGetter && _this._lookupGetter()); };
            this._allowFiltering = true;
            this._allowGrouping = true;
            this._columnVisible = true;
            this._optionChanged = true;
        }
        //#region SemanticTypes
        SemanticMember.prototype.bool = function () {
            this._type = Luxena.FieldTypes.Bool.Bool;
            return this.initType();
        };
        SemanticMember.prototype.boolSet = function (members) {
            this._type = Luxena.FieldTypes.BoolSet.BoolSet;
            this._members = members;
            return this.initType();
        };
        //#region Date
        SemanticMember.prototype.date = function () {
            this._type = Luxena.FieldTypes.DateTime.Date;
            return this.initType();
        };
        SemanticMember.prototype.monthAndYear = function () {
            this._type = Luxena.FieldTypes.DateTime.MonthAndYear;
            return this.initType();
        };
        SemanticMember.prototype.quarterAndYear = function () {
            this._type = Luxena.FieldTypes.DateTime.QuarterAndYear;
            return this.initType();
        };
        SemanticMember.prototype.year = function () {
            this._type = Luxena.FieldTypes.DateTime.Year;
            return this.initType();
        };
        SemanticMember.prototype.dateTime = function () {
            this._type = Luxena.FieldTypes.DateTime.DateTime;
            return this.initType();
        };
        SemanticMember.prototype.dateTime2 = function () {
            this._type = Luxena.FieldTypes.DateTime.DateTime2;
            return this.initType();
        };
        SemanticMember.prototype.time = function () {
            this._type = Luxena.FieldTypes.DateTime.Time;
            return this;
        };
        SemanticMember.prototype.time2 = function () {
            this._type = Luxena.FieldTypes.DateTime.Time;
            return this.initType();
        };
        //#endregion
        //#region Number
        SemanticMember.prototype.float = function (precision) {
            this._type = Luxena.FieldTypes.Numeric.Float;
            this._precision = precision;
            return this.initType();
        };
        SemanticMember.prototype.int = function () {
            this._type = Luxena.FieldTypes.Numeric.Int;
            return this.initType();
        };
        SemanticMember.prototype.percent = function () {
            this._type = Luxena.FieldTypes.Numeric.Percent;
            return this.initType();
        };
        //#endregion
        SemanticMember.prototype.enum = function (enumType) {
            var enumListIds = [];
            for (var _i = 1; _i < arguments.length; _i++) {
                enumListIds[_i - 1] = arguments[_i];
            }
            this._type = Luxena.FieldTypes.Enum.Enum;
            this._enumType = enumType;
            this._enumIsFlags = enumType._isFlags;
            this._maxLength = enumType["_maxLength"];
            if (enumListIds && enumListIds.length)
                this._enumList = enumListIds.map(function (a) { return enumType._items[a]; });
            return this.initType();
        };
        SemanticMember.prototype.enumIsFlags = function (value) {
            this._enumIsFlags = value !== false;
            return this;
        };
        SemanticMember.prototype.currencyCode = function () {
            this._type = Luxena.FieldTypes.CurrencyCode.CurrencyCode;
            return this.initType();
        };
        SemanticMember.prototype.money = function () {
            this._type = Luxena.FieldTypes.Money.Money;
            this._isMoney = true;
            return this.initType();
        };
        SemanticMember.prototype.defaultMoney = function () {
            this._type = Luxena.FieldTypes.Money.Money;
            this._isMoney = true;
            return this.initType();
        };
        SemanticMember.prototype.lookup = function (lookupGetter) {
            this._type = Luxena.FieldTypes.Lookup.Reference;
            this._lookupGetter = lookupGetter;
            return this.initType();
        };
        SemanticMember.prototype.col = function (members) {
            this._type = Luxena.FieldTypes.FieldColumn.FieldColumn;
            if (members)
                this._members = members;
            return this.initType();
        };
        SemanticMember.prototype.row = function (members) {
            this._type = Luxena.FieldTypes.FieldRow.FieldRow;
            if (members)
                this._members = members;
            return this.initType();
        };
        //#region Text
        SemanticMember.prototype.string = function (maxLength, length, minLength) {
            this._type = Luxena.FieldTypes.Text.String;
            if (maxLength)
                this._maxLength = maxLength;
            if (length)
                this._length = length;
            if (minLength)
                this._minLength = minLength;
            return this.initType();
        };
        SemanticMember.prototype.text = function (lineCount) {
            this._type = Luxena.FieldTypes.Text.Text;
            if (lineCount)
                this._lineCount = lineCount;
            return this.initType();
        };
        SemanticMember.prototype.codeText = function (lineCount) {
            this._type = Luxena.FieldTypes.Text.CodeText;
            if (lineCount)
                this._lineCount = lineCount;
            return this.initType();
        };
        SemanticMember.prototype.password = function () {
            this._type = Luxena.FieldTypes.Text.Password;
            return this.initType();
        };
        SemanticMember.prototype.confirmPassword = function (passwordField) {
            this._type = Luxena.FieldTypes.Text.Password;
            return this.initType();
        };
        SemanticMember.prototype.lineCount = function (value) {
            this._lineCount = value;
            return this.initType();
        };
        SemanticMember.prototype.email = function () {
            this._type = Luxena.FieldTypes.Text.Email;
            if (!this._length)
                this._length = 18;
            return this.initType();
        };
        SemanticMember.prototype.phone = function () {
            this._type = Luxena.FieldTypes.Text.Phone;
            if (!this._length)
                this._length = 14;
            return this.initType();
        };
        SemanticMember.prototype.fax = function () {
            this._type = Luxena.FieldTypes.Text.Fax;
            if (!this._length)
                this._length = 14;
            return this.initType();
        };
        SemanticMember.prototype.address = function (lineCount) {
            this._type = Luxena.FieldTypes.Text.Address;
            if (!this._length)
                this._length = 24;
            if (lineCount)
                this._lineCount = lineCount;
            return this.initType();
        };
        SemanticMember.prototype.hyperlink = function () {
            this._type = Luxena.FieldTypes.Text.Hyperlink;
            if (!this._length)
                this._length = 18;
            return this.initType();
        };
        //#endregion
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
            return this.field().hidden();
        };
        SemanticMember.prototype.reserved = function () {
            return this.field().reserved();
        };
        SemanticMember.prototype.header1 = function () {
            return this.field().header1();
        };
        SemanticMember.prototype.header2 = function () {
            return this.field().header2();
        };
        SemanticMember.prototype.header3 = function () {
            return this.field().header3();
        };
        SemanticMember.prototype.header4 = function () {
            return this.field().header4();
        };
        SemanticMember.prototype.header5 = function () {
            return this.field().header5();
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
        //card(value?: boolean)
        //{
        //	this._isCard = value !== false;
        //	return this;
        //}
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
        SemanticMember.prototype.fit = function () {
            this._length = 0;
            return this;
        };
        SemanticMember.prototype.format = function (value) {
            this._format = value;
            return this;
        };
        SemanticMember.prototype.groupIndex = function (value) {
            this._groupIndex = value;
            return this;
        };
        SemanticMember.prototype.ungroup = function () {
            this._groupIndex = -1;
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
        SemanticMember.prototype.items = function () {
            var members = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                members[_i - 0] = arguments[_i];
            }
            this._members = members;
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
        SemanticMember.prototype.sortOrder = function (value) {
            this._sortOrder = value || "asc";
            return this;
        };
        SemanticMember.prototype.totalSum = function (value) {
            this._useTotalSum = value !== false;
            return this;
        };
        //#endregion
        SemanticMember.prototype.initType = function () {
            this._type.initMember(this);
            return this;
        };
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
        SemanticMember.prototype.setModel = function (model, value) {
            return this._type.setModel(model, this._name, value);
        };
        SemanticMember.prototype.filter = function (model, operation) {
            var value = this.get(model);
            if (!value)
                return undefined;
            return this.getFilterExpr(value, operation);
        };
        SemanticMember.prototype.field = function () {
            return new Luxena.Field(this);
        };
        SemanticMember.prototype.toTab = function () {
            return {
                title: this._title,
                template: this._name,
            };
        };
        SemanticMember.prototype.getIconHtml = function (icon, withTitle) {
            return _super.prototype.getIconHtml.call(this, icon || this._icon || this._type._icon, withTitle);
        };
        SemanticMember.prototype.getLength = function () {
            var sm = this;
            var minLength = sm._minLength;
            var maxLength = sm._maxLength;
            var length = sm._length;
            if (length === undefined)
                length = sm._maxLength;
            if (length === undefined) {
                var ref = sm.getLookupEntity();
                if (ref) {
                    var name_1 = ref._nameMember;
                    if (!name_1) {
                        $log_(sm);
                        throw Error("SemanticMember._nameMember is null");
                    }
                    length = name_1._length;
                    minLength = name_1._minLength;
                    maxLength = name_1._maxLength;
                }
            }
            if (length === undefined)
                length = sm._type && sm._type.length;
            return {
                length: length,
                min: minLength || sm._minLength,
                max: maxLength || sm._maxLength,
            };
        };
        return SemanticMember;
    })(Luxena.SemanticObject);
    Luxena.SemanticMember = SemanticMember;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var SemanticFieldType = (function () {
        function SemanticFieldType() {
            this.allowFiltering = true;
            this.allowGrouping = true;
            this.allowSorting = true;
            this.dataType = "string";
            this.nullable = true;
        }
        SemanticFieldType.prototype.initMember = function (sm) {
        };
        //#region Data & Model
        SemanticFieldType.prototype.addItemsToController = function (sf, ctrl, action) {
        };
        SemanticFieldType.prototype.getSelectFieldNames = function (sf) {
            return [sf._name];
        };
        SemanticFieldType.prototype.getExpandFieldNames = function (sf) {
            return [];
        };
        SemanticFieldType.prototype.getFilterExpr = function (sm, value, operation) {
            return [sm._name, operation || "=", value];
        };
        SemanticFieldType.prototype.getFromData = function (sm, data) {
            return data[sm._name];
        };
        SemanticFieldType.prototype.getModel = function (model, name) {
            return model[name];
        };
        SemanticFieldType.prototype.setModel = function (model, sname, value) {
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
        SemanticFieldType.prototype.loadFromData = function (sf, model, data) {
            var value = data[sf._name];
            this.setModel(model, sf, value);
        };
        SemanticFieldType.prototype.saveToData = function (sf, model, data) {
            var name = sf._name;
            data[name] = $clip(ko.unwrap(model[name]));
        };
        SemanticFieldType.prototype.removeFromData = function (sf, data) {
            delete data[sf._name];
        };
        //#endregion
        //#region Renders
        SemanticFieldType.prototype.toGridColumns = function (sf) {
            return [this.toStdGridColumn(sf)];
        };
        SemanticFieldType.prototype.toStdGridColumn = function (sf) {
            var _this = this;
            var sm = sf._member;
            var se = sf._entity;
            var cfg = sf._controller.config;
            var col = {
                allowFiltering: sm._allowFiltering && this.allowFiltering && !sm._isCalculated,
                allowGrouping: sm._allowGrouping && this.allowGrouping && !sm._isCalculated,
                allowSorting: !sm._isCalculated && this.allowSorting,
                caption: sm._title,
                dataField: sm._name,
                dataType: this.dataType,
                fixed: sm._columnFixed || (cfg.wide || se._isWide) && sm._kind === Luxena.SemanticMemberKind.Primary,
                format: sm._format || this.format,
                groupIndex: sf._groupIndex !== undefined ? sf._groupIndex : sm._groupIndex,
                sortOrder: sf.sortOrder || sm._sortOrder,
                width: sf.getWidth(),
                visible: sf._columnVisible && sm._columnVisible,
                calculateFilterExpression: function (value, operation) { return _this.getFilterExpr(sm, value, operation); },
            };
            col.cellTemplate = function (cell, cellInfo) {
                _this.renderDisplayStatic(sf, cell, cellInfo.data);
            };
            return col;
        };
        SemanticFieldType.prototype.toGridTotalItems = function (sf) {
            return [];
        };
        SemanticFieldType.prototype.prerender = function (sf) { };
        SemanticFieldType.prototype.render = function (sf, valueEl, rowEl) {
            if (sf._controller.editMode && !sf._member._isReadOnly)
                this.renderEditor(sf, valueEl, rowEl);
            else
                this.renderDisplay(sf, valueEl, rowEl);
        };
        SemanticFieldType.prototype.renderDisplayStatic = function (sf, container, data) {
            var value = $clip(data[sf._name]);
            if (!value)
                return;
            if (sf._member._kind === Luxena.SemanticMemberKind.Important)
                value = "<b>" + value + "</b>";
            if (this._singleLine)
                container.append("<span class=\"nowrap\">" + sf._member.getIconHtml() + value + "</span>");
            else
                container.append(sf.getIconHtml() + value);
        };
        SemanticFieldType.prototype.renderDisplay = function (sf, valueEl, rowEl) {
            this.renderDisplayBind(sf, valueEl, rowEl);
            this.renderDisplayVisible(sf, valueEl, rowEl);
        };
        SemanticFieldType.prototype.getDisplayValueVisible = function (sf, model) {
            var name = sf._name;
            if (!name)
                return function () { return true; };
            return function () {
                var value = model[name];
                return !value || $clip(value());
            };
        };
        SemanticFieldType.prototype.renderDisplayVisible = function (sf, valueEl, rowEl) {
            var ctrl = sf._controller;
            ctrl.widgets[sf.uname()] = {
                valueVisible: ko.computed(this.getDisplayValueVisible(sf, ctrl.model)),
            };
            rowEl.attr("data-bind", "visible: widgets." + sf.uname() + ".valueVisible");
        };
        SemanticFieldType.prototype.renderDisplayBind = function (sf, valueEl, rowEl) {
            valueEl.append(sf.getIconHtml() +
                ("<span data-bind=\"text: $clip(r." + sf._name + "())\">"));
        };
        SemanticFieldType.prototype.renderEditor = function (sf, valueEl, rowEl) {
            this.renderDisplay(sf, valueEl, rowEl);
        };
        SemanticFieldType.prototype.pureRender = function (sf, container) {
            var valueEl = $("<div>");
            sf._type.render(sf, valueEl, valueEl);
            valueEl.appendTo(container);
        };
        SemanticFieldType.prototype.appendEditor = function (sf, valueEl, widgetClassName, options) {
            var ctrl = sf._controller;
            var sm = sf._member;
            options["hint"] = sm._title + (!sm._description ? "" : "\r\n" + sm._description);
            if (sm._isSubject)
                options.onValueChanged = function () { return ctrl.recalc(sf._name); };
            var widgetOption = sf._widgetOptions["dxTextBox"];
            if (widgetOption)
                $.extend(options, widgetOption);
            ctrl.widgets[sf.uname()] = options;
            var editorEl = $("<div>");
            //editorEl[0]["sf"] = sf;
            var bindAttr = widgetClassName + ": widgets." + sf.uname();
            var rules = this.getValidationRules(sf);
            if (rules && rules.length) {
                ctrl.validators[sf.uname()] =
                    {
                        validationGroup: "edit-form",
                        validationRules: rules
                    };
                bindAttr += ", dxValidator: validators." + sf.uname();
            }
            editorEl
                .attr("data-bind", bindAttr)
                .appendTo(valueEl);
        };
        SemanticFieldType.prototype.appendTextBoxEditor = function (sf, valueEl, widgetClassName, options) {
            var sm = sf._member;
            if (sf._controller.filterMode)
                options.mode = "search";
            if (sf._rowMode || sf._unlabel || sf._hideLabel)
                options.placeholder = sm._shortTitle || sf._title;
            var widgetOption = sf._widgetOptions["dxTextBox"];
            if (widgetOption)
                $.extend(options, widgetOption);
            //options.maxLength = sm._maxLength || undefined;
            this.appendEditor(sf, valueEl, widgetClassName, options);
        };
        //#endregion
        SemanticFieldType.prototype.getValidationRules = function (sf) {
            var sm = sf._member;
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
        SemanticFieldType.charWidth = 10;
        SemanticFieldType.digitWidth = 7.6;
        return SemanticFieldType;
    })();
    Luxena.SemanticFieldType = SemanticFieldType;
    function getTextIconHtml(icon) {
        return !icon ? "" : "<i class=\"fa fa-" + icon + " fa-lg\" /> &nbsp;";
    }
    Luxena.getTextIconHtml = getTextIconHtml;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var SemanticEntityAction = (function (_super) {
        __extends(SemanticEntityAction, _super);
        function SemanticEntityAction(se) {
            _super.call(this);
            this._entity = se;
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
        SemanticEntityAction.prototype.button = function () {
            return new Luxena.Components.Button(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
            this._members = [];
            this._textIconHtml = "";
            //#region Actions
            this.listAction = this.action()
                .onExecute(function (btn) { return btn._entity.openList(); });
            this.backToListAction = this.action()
                .title("Вернуться к списку")
                .icon("list")
                .onExecute(function (btn) { return btn._entity.openList(); });
            this.viewAction = this.action()
                .title("Подробнее")
                .icon("fa fa-search")
                .default()
                .onExecute(function (btn) { return btn._entity.openView(btn.getId()); });
            this.viewSingletonAction = this.action()
                .default()
                .onExecute(function (btn) { return btn._entity.openView("single"); });
            this.newAction = this.action()
                .title("Добавить")
                .icon("fa fa-plus")
                .success()
                .onExecute(function (btn) { return btn._entity.openNew(btn._controller.defaults); });
            this.editAction = this.action()
                .title("Изменить")
                .icon("fa fa-pencil")
                .success()
                .onExecute(function (btn) { return btn._entity.openEdit(btn.getId()); });
            this.editSingletonAction = this.action()
                .success()
                .onExecute(function (btn) { return btn._entity.openEdit("single"); });
            this.deleteAction = this.action()
                .title("Удалить")
                .icon("fa fa-trash")
                .danger()
                .onExecute(function (btn) { return btn._entity.delete(btn.getId()); });
            this.refreshAction = this.action()
                .title("Обновить")
                .icon("refresh")
                .onExecute(function (btn) { return btn._controller.refresh(); });
            //#region Navigations
            this.listView = function () { return _this._names; };
            this.viewView = function () { return _this._name; };
            this.editView = function () { return _this._name + "Edit"; };
        }
        //#endregion
        SemanticEntity.prototype.init = function () {
            this.initMembers();
            this.listAction.iconAndTitle(this._icon, this._titles || this._title || this._names || this._name);
            this.viewSingletonAction.iconAndTitle(this._icon, this._title || this._name);
            this.editSingletonAction.iconAndTitle(this._icon, this._title || this._name);
        };
        SemanticEntity.prototype.initMembers = function () {
            var entityPositions = [];
            var entityNames = [];
            var entityDates = [];
            for (var name_2 in this) {
                if (!this.hasOwnProperty(name_2) || name_2.indexOf("_") === 0)
                    continue;
                var sm = this[name_2];
                if (sm instanceof Luxena.SemanticObject) {
                    sm._entity = this;
                    sm._name = name_2;
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
            this._nameMember = this._nameMember || this[this._lookupFields.name];
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
        SemanticEntity.prototype.member0 = function () {
            var m = new Luxena.SemanticMember();
            m._entity = this;
            return m;
        };
        SemanticEntity.prototype.member = function (original) {
            var m = original ? original.clone() : new Luxena.SemanticMember();
            m._entity = this;
            this._members.push(m);
            return m;
        };
        SemanticEntity.prototype.collection = function (collectionItemEntity, collectionItemMasterMember, setter) {
            var m = this.member0();
            m._type = Luxena.FieldTypes.Grid.Grid;
            m._collectionItemEntity = collectionItemEntity;
            if (collectionItemMasterMember instanceof Luxena.SemanticMember) {
                var collectionItemMasterMember_ = collectionItemMasterMember;
                collectionItemMasterMember = (function () { return collectionItemMasterMember_; });
            }
            m._collectionItemMasterMember = collectionItemMasterMember;
            setter && setter(m);
            return m;
        };
        SemanticEntity.prototype.action = function () {
            return new Luxena.SemanticEntityAction(this);
        };
        //#region Setters
        SemanticEntity.prototype.icon = function (value) {
            this._icon = value;
            this._textIconHtml = Luxena.getTextIconHtml(value);
            return this;
        };
        SemanticEntity.prototype.entityStatus = function (value) {
            this._entityStatusGetter = value;
            return this;
        };
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
            var title = ko.unwrap(data[this._lookupFields.name]);
            return title ? this._title + " " + title : this._title;
        };
        SemanticEntity.prototype.resolveListUri = function (action) {
            if (action === null)
                return null;
            if (action instanceof SemanticEntity)
                return action.resolveListUri();
            if (action === undefined)
                action = this._listUri || { uri: { view: this.listView() } };
            return action;
        };
        SemanticEntity.prototype.resolveViewUri = function (action, formEntity) {
            if (action === null)
                return null;
            if (action instanceof SemanticEntity)
                return action.resolveViewUri();
            if (action === undefined) {
                formEntity = formEntity || this;
                action = formEntity._viewUri || { uri: { view: formEntity.viewView() } };
            }
            return action;
        };
        SemanticEntity.prototype.resolveEditUri = function (action, formEntity) {
            if (action === null)
                return null;
            if (action instanceof SemanticEntity)
                return action.resolveEditUri();
            if (action === undefined) {
                formEntity = formEntity || this;
                action = formEntity._editUri || { uri: { view: formEntity.editView() } };
            }
            return action;
        };
        SemanticEntity.prototype.openList = function () {
            Luxena.openEntityUri(this.resolveListUri());
        };
        SemanticEntity.prototype.openView = function (id) {
            Luxena.openEntityUri(this.resolveViewUri(), id);
        };
        SemanticEntity.prototype.openNew = function (defaults) {
            var action = this.resolveEditUri();
            action.defaults = defaults;
            Luxena.openEntityUri(action);
        };
        SemanticEntity.prototype.openEdit = function (id) {
            Luxena.openEntityUri(this.resolveEditUri(), id);
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
        //#endregion
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
        //toTab()
        //{
        //	return {
        //		icon: "fa fa-" + this._icon,
        //		title: this._title || this._name,
        //		template: this._name,
        //	}
        //}
        //toTabs()
        //{
        //	return {
        //		icon: "fa fa-" + this._icon,
        //		title: this._titles || this._title || this._names || this._name,
        //		template: this._names || this._name,
        //	}
        //}
        SemanticEntity.prototype.applyToThisAndDerived = function (action) {
            if (!action)
                return;
            action(this);
            var deriveds = this._getDerivedEntities && this._getDerivedEntities();
            if (!deriveds)
                return;
            deriveds.forEach(function (se) { return se.applyToThisAndDerived(action); });
        };
        //#region Components
        SemanticEntity.prototype.col = function () {
            var members = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                members[_i - 0] = arguments[_i];
            }
            return this.member0().col(members);
        };
        SemanticEntity.prototype.row = function () {
            var members = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                members[_i - 0] = arguments[_i];
            }
            return this.member0().row(members);
        };
        SemanticEntity.prototype.grid = function (masterMember, setter) {
            var _this = this;
            return this.collection(function () { return _this; }, masterMember, setter).field();
        };
        SemanticEntity.prototype.chart = function (masterMember, setter) {
            var _this = this;
            var sf = this.collection(function () { return _this; }, masterMember, setter).field();
            sf._type = Luxena.FieldTypes.Chart.Chart;
            return sf;
        };
        return SemanticEntity;
    })(Luxena.SemanticObject);
    Luxena.SemanticEntity = SemanticEntity;
    function $doForDerived(se, action) {
        se.applyToThisAndDerived(action);
    }
    Luxena.$doForDerived = $doForDerived;
    function isEntity(obj) {
        return obj instanceof SemanticEntity;
    }
    Luxena.isEntity = isEntity;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var FieldTypes;
    (function (FieldTypes) {
        var CollectionFieldType = (function (_super) {
            __extends(CollectionFieldType, _super);
            function CollectionFieldType() {
                _super.apply(this, arguments);
            }
            CollectionFieldType.prototype.getSelectFieldNames = function (sf) { return []; };
            CollectionFieldType.prototype.loadFromData = function (sf, model, data) { };
            CollectionFieldType.prototype.saveToData = function (sf, model, data) { };
            CollectionFieldType.prototype.removeFromData = function (sf, data) { };
            CollectionFieldType.prototype.addItemsToController = function (sf, ctrl, action) {
                var sm = sf._member;
                if (!sm._collectionItemEntity)
                    throw Error("\u0421\u0432\u043E\u0439\u0441\u0442\u0432\u043E " + sm._entity._name + "." + sm._name + " \u043D\u0435 \u044F\u0432\u043B\u044F\u0435\u0442\u0441\u044F \u043A\u043E\u043B\u043B\u0435\u043A\u0446\u0438\u0435\u0439");
                var gse = sm._collectionItemEntity();
                if (!sf._title || sf._title === gse._names)
                    sf._title = gse._titles || gse._title || gse._names || gse._name;
                if (!sf._icon)
                    sf._icon = sm._icon || gse._icon;
            };
            CollectionFieldType.prototype.getControllerConfig = function (sf, controllerName, defaultConfig) {
                var sm = sf._member;
                var ctrl = sf._controller;
                var gse = sm._collectionItemEntity();
                var masterMember = sm._collectionItemMasterMember && sm._collectionItemMasterMember(gse);
                var gcfg = sf._widgetOptions[controllerName] || defaultConfig;
                gcfg = $.extend({}, gcfg, {
                    entity: gse,
                    master: ctrl,
                    defaults: masterMember ? [[masterMember, ctrl.getId()]] : undefined,
                    members: sf._members || sf._member._members,
                });
                return gcfg;
            };
            return CollectionFieldType;
        })(Luxena.SemanticFieldType);
        FieldTypes.CollectionFieldType = CollectionFieldType;
    })(FieldTypes = Luxena.FieldTypes || (Luxena.FieldTypes = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var CompositeFieldType = (function (_super) {
        __extends(CompositeFieldType, _super);
        function CompositeFieldType() {
            _super.apply(this, arguments);
            this._memberColumnVisible = false;
            this._isComposite = true;
        }
        CompositeFieldType.prototype.addItemsToController = function (sf, ctrl, action) {
            var _this = this;
            sf._components = ctrl.addComponents(sf._members || sf._member._members, sf, null, function (sm2, sc2) {
                if (!_this._memberColumnVisible)
                    sc2.columnVisible(false);
                if (Luxena.isField(sc2))
                    sc2._type.addItemsToController(sc2, ctrl, action);
                action && action(sm2, sc2);
            });
        };
        CompositeFieldType.prototype.getSelectFieldNames = function (sf) {
            return [];
        };
        CompositeFieldType.prototype.loadFromData = function (sf, model, data) {
        };
        CompositeFieldType.prototype.saveToData = function (sf, model, data) {
        };
        CompositeFieldType.prototype.getDisplayValueVisible = function (sf, model) {
            var widgets = sf._controller.widgets;
            return function () {
                var visible = false;
                sf._components.forEach(function (sc2) {
                    if (sc2.isComposite())
                        visible = true;
                    else {
                        var widget = widgets[sc2.uname()];
                        visible = visible || widget && (!widget.valueVisible || widget.valueVisible());
                    }
                });
                return visible;
            };
        };
        return CompositeFieldType;
    })(Luxena.SemanticFieldType);
    Luxena.CompositeFieldType = CompositeFieldType;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var FieldTypes;
    (function (FieldTypes) {
        var Bool = (function (_super) {
            __extends(Bool, _super);
            function Bool() {
                _super.apply(this, arguments);
                this.dataType = "boolean";
                this.nullable = false;
            }
            Bool.prototype.loadFromData = function (sf, model, data) {
                this.setModel(model, sf, !!data[sf._name]);
            };
            Bool.prototype.toGridColumns = function (sf) {
                var col = this.toStdGridColumn(sf);
                delete col.cellTemplate;
                return [col];
            };
            Bool.prototype.prerender = function (sf) {
                if (sf._hideLabel === undefined)
                    sf._hideLabel = true;
            };
            Bool.prototype.renderDisplayBind = function (sf, valueEl, rowEl) {
                valueEl.append("<div data-bind=\"dxCheckBox: { value: r." + sf._name + ", text: '" + sf._member._title + "', readOnly: true, }\"></div>");
            };
            Bool.prototype.renderEditor = function (sf, valueEl, rowEl) {
                this.appendEditor(sf, valueEl, "dxCheckBox", {
                    value: sf.getModelValue(),
                    text: /*sf._member.getIconHtml() +*/ sf._member._title,
                });
            };
            Bool.Bool = new Bool();
            return Bool;
        })(Luxena.SemanticFieldType);
        FieldTypes.Bool = Bool;
    })(FieldTypes = Luxena.FieldTypes || (Luxena.FieldTypes = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var FieldTypes;
    (function (FieldTypes) {
        var BoolSet = (function (_super) {
            __extends(BoolSet, _super);
            function BoolSet() {
                _super.apply(this, arguments);
                this._isComposite = true;
                this.allowFiltering = false;
                this.allowGrouping = false;
                this.allowSorting = false;
                this.addColumnFilterWidth = -8;
            }
            BoolSet.prototype.renderDisplayStatic = function (sf, container, data) {
                var html = [];
                if (sf._isCompact) {
                    container
                        .addClass("cell-boolset")
                        .css("text-align", "right");
                    sf._components.forEach(function (sc2) {
                        if (!Luxena.isField(sc2))
                            return;
                        var value = data[sc2._name];
                        if (!value)
                            return;
                        html.push(sc2.getIconHtml(null, true));
                    });
                }
                else {
                    html.push("<div class=\"chips\">");
                    sf._components.forEach(function (sc2) {
                        if (!Luxena.isField(sc2))
                            return;
                        var value = data[sc2._name];
                        if (!value)
                            return;
                        html.push("<div class=\"chip\">" + sc2.getIconHtml() + sc2._title + "</div>");
                    });
                    html.push("</div>");
                }
                container.append(html.join(""));
            };
            BoolSet.prototype.getDisplayValueVisible = function (sf, model) {
                return function () {
                    var visible = false;
                    sf._components.forEach(function (sc2) {
                        if (Luxena.isField(sc2))
                            visible = visible || model[sc2._name] && model[sc2._name]();
                    });
                    return visible;
                };
            };
            BoolSet.prototype.renderDisplayBind = function (sf, valueEl, rowEl) {
                valueEl.addClass("chips");
                sf._components.forEach(function (sc2) {
                    if (!Luxena.isField(sc2))
                        return;
                    $("<div class=\"chip\" data-bind=\"visible: r." + sc2._name + "\">" + sc2.getIconHtml() + sc2._title + "</div>")
                        .appendTo(valueEl);
                });
            };
            BoolSet.prototype.renderEditor = function (sf, valueEl, rowEl) {
                var sm = sf._member;
                var model = sf._controller.model;
                var values = ko.observableArray();
                sf._components.forEach(function (sc2) {
                    if (!Luxena.isField(sc2))
                        return;
                    var sm2 = sc2._member;
                    var mvalue = sm2.getModel(model);
                    if (mvalue) {
                        mvalue.subscribe(function (newValue) {
                            if (newValue) {
                                var i = values.indexOf(sm2);
                                if (i < 0)
                                    values.push(sm2);
                            }
                            else {
                                //if (i >= 0)
                                values.remove(sm2);
                            }
                        });
                    }
                    if (mvalue.peek())
                        values.push(sm2);
                });
                values.subscribe(function (changes) { return changes.forEach(function (change) {
                    //$log(change.value._name, ko.unwrap(model[change.value._name]), change.status === "added");
                    change.value.setModel(model, change.status === "added");
                }); }, null, "arrayChange");
                this.appendTextBoxEditor(sf, valueEl, "dxTagBox", {
                    values: values,
                    items: sm._members,
                    //displayExpr: "_title",
                    showClearButton: true,
                    itemTemplate: function (item, index, container) {
                        return item.getIconHtml() + item._title;
                    },
                    tagTemplate: function (item, index, container) {
                        return item.getIconHtml() + item._title;
                    },
                });
            };
            BoolSet.BoolSet = new BoolSet();
            return BoolSet;
        })(Luxena.CompositeFieldType);
        FieldTypes.BoolSet = BoolSet;
    })(FieldTypes = Luxena.FieldTypes || (Luxena.FieldTypes = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var FieldTypes;
    (function (FieldTypes) {
        var DateTime = (function (_super) {
            __extends(DateTime, _super);
            function DateTime(format, length) {
                _super.call(this);
                this.format = format;
                this.length = length;
                this.charWidth = Luxena.SemanticFieldType.digitWidth;
                this.addColumnFilterWidth = length <= 10 ? 42 : 0;
                this.dataType = "date";
                this.chartDataType = "datetime";
            }
            DateTime.prototype.loadFromData = function (sf, model, data) {
                var value = data[sf._name];
                if (typeof value === "string") {
                    value = value.replace(/T.+/, "");
                    value = new Date(value);
                }
                this.setModel(model, sf, value);
            };
            DateTime.prototype.toGridColumns = function (sf) {
                var se = sf._entity;
                var sm = sf._member;
                var col = this.toStdGridColumn(sf);
                delete col.cellTemplate;
                if (col.groupIndex === undefined && se._isBig && sm._isEntityDate)
                    col.groupIndex = 0;
                return [col];
            };
            DateTime.prototype.renderDisplayBind = function (sf, valueEl, rowEl) {
                valueEl.attr("data-bind", "text: Globalize.format(r." + sf._name + "(), '" + this.format + "')");
            };
            DateTime.prototype.renderEditor = function (sf, valueEl, rowEl) {
                valueEl = $("<div>").appendTo(valueEl);
                var options = {
                    value: sf.getModelValue(),
                    format: "date",
                    formatString: this.format,
                    showClearButton: !sf._member._required || sf._controller.filterMode,
                };
                if (this.format === "monthAndYear") {
                    options.formatString = "MMMM yyyy";
                    options.maxZoomLevel = "year";
                    options.minZoomLevel = "year";
                }
                this.appendEditor(sf, valueEl, "dxDateBox", options);
            };
            DateTime.Date = new DateTime("dd.MM.yyyy", 10);
            DateTime.MonthAndYear = new DateTime("monthAndYear", 10);
            DateTime.QuarterAndYear = new DateTime("quarterAndYear", 10);
            DateTime.Year = new DateTime("dd.MM.yyyy", 10);
            DateTime.DateTime = new DateTime("dd.MM.yyyy H:mm", 15);
            DateTime.DateTime2 = new DateTime("dd.MM.yyyy H:mm:ss", 18);
            DateTime.Time = new DateTime("H:mm", 5);
            DateTime.Time2 = new DateTime("H:mm:ss", 8);
            return DateTime;
        })(Luxena.SemanticFieldType);
        FieldTypes.DateTime = DateTime;
    })(FieldTypes = Luxena.FieldTypes || (Luxena.FieldTypes = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var FieldTypes;
    (function (FieldTypes) {
        var Enum = (function (_super) {
            __extends(Enum, _super);
            function Enum() {
                _super.apply(this, arguments);
                this.allowGrouping = false;
                this.addColumnFilterWidth = -8;
            }
            Enum.prototype.getFilterExpr = function (sm, value, operation) {
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
            Enum.prototype.loadFromData = function (sf, model, data) {
                var sm = sf._member;
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
            Enum.prototype.saveToData = function (sf, model, data) {
                if (!sf._member._enumIsFlags) {
                    _super.prototype.saveToData.call(this, sf, model, data);
                    return;
                }
                var values = ko.unwrap(model[sf._name]);
                data[sf._name] = values.join(", ");
            };
            Enum.prototype.toGridColumns = function (sf) {
                var sm = sf._member;
                var col = this.toStdGridColumn(sf);
                if (sf._isCompact)
                    col.alignment = "right";
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
                    //col.cellTemplate = (cell, cellInfo) =>
                    //	cell.html(getEnumNames(sm._enumType, cellInfo.value));
                    //col.calculateCellValue = data => getEnumNames(sm._enumType, data[sm._name]);
                    col.calculateGroupValue = function (data) { return sm._enumType._getEdm(data[sm._name]); };
                    col.groupCellTemplate = function (cell, cellInfo) {
                        return cell.html(sm._title + ": &nbsp; " + getEnumNames(sm._enumType, cellInfo.value));
                    };
                }
                return [col];
            };
            Enum.prototype.renderDisplayStatic = function (sf, container, data) {
                var value = data[sf._name];
                if (!value)
                    return;
                value = getEnumNames(sf._member._enumType, value, sf._isCompact);
                if (sf._member._kind === Luxena.SemanticMemberKind.Important)
                    value = "<b>" + value + "</b>";
                container.addClass("nowrap");
                container.append(value);
            };
            Enum.prototype.getDisplayValueVisible = function (sf, model) {
                var defValue = sf._member._enumType._array[0].Id;
                return function () {
                    var value = model[sf._name]();
                    return value && value !== defValue;
                };
            };
            Enum.prototype.renderDisplayBind = function (sf, valueEl, rowEl) {
                valueEl.attr("data-bind", "html: Luxena.FieldTypes.getEnumNames(Luxena." + sf._member._enumType._name + ", r." + sf._name + ")");
            };
            Enum.prototype.renderEditor = function (sf, valueEl, rowEl) {
                var sm = sf._member;
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
            Enum.Enum = new Enum();
            return Enum;
        })(Luxena.SemanticFieldType);
        FieldTypes.Enum = Enum;
        function getEnumNames(enumType, values, compact) {
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
                if (!item)
                    names.push(value);
                else if (compact)
                    names.push(item.TextIconHtml || item.Name);
                else
                    names.push(item.TextIconHtml + item.Name);
            }
            return names.join(", ") || "";
        }
        FieldTypes.getEnumNames = getEnumNames;
    })(FieldTypes = Luxena.FieldTypes || (Luxena.FieldTypes = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var FieldTypes;
    (function (FieldTypes) {
        var FieldColumn = (function (_super) {
            __extends(FieldColumn, _super);
            function FieldColumn() {
                _super.apply(this, arguments);
            }
            FieldColumn.prototype.toGridColumns = function (sf) {
                var sm = sf._member;
                var maxWidth = 0;
                sf._components.forEach(function (sc2) {
                    if (Luxena.isField(sc2)) {
                        var width = sc2.getWidth();
                        if (width > maxWidth)
                            maxWidth = width;
                    }
                });
                var col = {
                    allowFiltering: true,
                    allowGrouping: false,
                    allowSearch: true,
                    allowSorting: false,
                    caption: sf._title || sm._title || "",
                    width: maxWidth,
                    dataField: "Id",
                };
                col.cellTemplate = function (cell, cellInfo) {
                    var data = cellInfo.data;
                    sf._components.forEach(function (sc2) {
                        var div = $("<div title=\"" + sc2._title + "\">").appendTo(cell);
                        sc2.renderDisplayStatic(div, data);
                    });
                };
                col.calculateFilterExpression = function (value, operation) {
                    var filter = [];
                    sf._components.forEach(function (sc2) {
                        if (Luxena.isField(sc2) && !sc2._type._isComposite) {
                            var f = sc2._member.getFilterExpr(value, operation);
                            if (f && f.length) {
                                if (filter.length)
                                    filter.push("or");
                                filter.push(f);
                            }
                        }
                    });
                    return filter;
                };
                return [col];
            };
            FieldColumn.prototype.prerender = function (sf) {
                if (sf._unlabel === undefined)
                    sf._unlabel = true;
            };
            //pureRender(sf: Field, container: JQuery)
            //{
            //	const valueEl = sf._height
            //		? $(`<div data-bind="dxScrollView: { showScrollbar: 'always', height: ${sf._height} }">`)
            //		: $(`<div>`);
            //	sf._type.render(sf, valueEl, valueEl);
            //	valueEl.appendTo(container);
            //}
            FieldColumn.prototype.render = function (sf, valueEl, rowEl) {
                var unlabelItems = sf._unlabelItems || !sf._unlabel && !sf._labelAsHeader;
                var mustPureRender = sf._controller.viewMode && unlabelItems;
                if (mustPureRender) {
                    rowEl.addClass("field-label-none");
                }
                if (sf._height)
                    valueEl = $("<div data-bind=\"dxScrollView: { showScrollbar: 'always', height: " + sf._height + " }\">")
                        .appendTo(valueEl);
                sf._components.forEach(function (sc2) {
                    if (sf._indentLabelItems)
                        sc2.indentLabel();
                    if (sf._labelAsHeaderItems)
                        sc2.labelAsHeader();
                    else if (unlabelItems) {
                        sc2.unlabel();
                        if (mustPureRender)
                            sc2._mustPureRender = true;
                    }
                    else if (sf._hideLabelItems)
                        sc2.hideLabel();
                    sc2.render(valueEl);
                });
                if (!sf._controller.editMode)
                    this.renderDisplayVisible(sf, valueEl, rowEl);
            };
            FieldColumn.FieldColumn = new FieldColumn();
            return FieldColumn;
        })(Luxena.CompositeFieldType);
        FieldTypes.FieldColumn = FieldColumn;
    })(FieldTypes = Luxena.FieldTypes || (Luxena.FieldTypes = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var FieldTypes;
    (function (FieldTypes) {
        var FieldRow = (function (_super) {
            __extends(FieldRow, _super);
            function FieldRow() {
                _super.apply(this, arguments);
                this._memberColumnVisible = true;
            }
            FieldRow.prototype.toGridColumns = function (sf) {
                return [];
            };
            FieldRow.prototype.renderDisplayStatic = function (sf, container, data) {
                container.addClass("field-row");
                sf._components.forEach(function (sc2) {
                    sc2._rowMode = true;
                    var span = $("<span>").appendTo(container);
                    sc2.renderDisplayStatic(span, data);
                });
            };
            FieldRow.prototype.renderDisplay = function (sf, valueEl, rowEl) {
                var hasComposite = false;
                sf._components.forEach(function (sc2) { return hasComposite = hasComposite ||
                    sc2._isComposite || Luxena.isField(sc2) && (sc2._type._isComposite); });
                // ReSharper disable once ConditionIsAlwaysConst
                if (hasComposite)
                    this.renderEditor(sf, valueEl, rowEl);
                else {
                    this.renderDisplayBind(sf, valueEl, rowEl);
                    this.renderDisplayVisible(sf, valueEl, rowEl);
                }
            };
            FieldRow.prototype.renderDisplayBind = function (sf, valueEl, rowEl) {
                var multirows = sf._unlabel && sf._controller.viewMode;
                var tag = multirows ? "<div>" : "<span>";
                if (!multirows)
                    valueEl.addClass("field-row");
                sf._components.forEach(function (sc2) {
                    sc2._rowMode = true;
                    var span = $(tag).appendTo(valueEl);
                    if (Luxena.isField(sc2))
                        sc2._type.render(sc2, span, span);
                    else
                        sc2.render(span);
                });
            };
            FieldRow.prototype.renderEditor = function (sf, valueEl, rowEl) {
                var getMinColumnWidth = function (col) { return Luxena.isField(col.sc) ? 2 : 1; };
                var cols = [];
                var totalLength = 0;
                sf._components.forEach(function (sc2) {
                    //if (sc2._parent !== sf) return;
                    var length = sc2.getLength().length || 10;
                    totalLength += length;
                    cols.push({ sc: sc2, length: length });
                });
                if (!cols.length)
                    return;
                var totalWidth = 0;
                cols.forEach(function (col) {
                    totalWidth += col.width = Math.max(getMinColumnWidth(col), Math.round(12 * col.length / totalLength));
                });
                var totalWidth2 = 0;
                cols.forEach(function (col) {
                    totalWidth2 += col.width = Math.max(getMinColumnWidth(col), Math.round(12 * col.width / totalWidth));
                });
                for (var i = 0; i < totalWidth2 - 12; i++) {
                    var maxCol = cols[0];
                    cols.forEach(function (col) {
                        // ReSharper disable ClosureOnModifiedVariable
                        if (maxCol.width < col.width)
                            maxCol = col;
                        // ReSharper restore ClosureOnModifiedVariable
                    });
                    maxCol.width--;
                }
                var row = $("<div class=\"row\">");
                cols.forEach(function (col) {
                    var cell = $("<div class=\"col s" + col.width + "\">").appendTo(row);
                    var sc2 = col.sc;
                    sc2._rowMode = true;
                    if (Luxena.isField(sc2)) {
                        sc2._type.prerender(sc2);
                        sc2._type.render(sc2, cell, rowEl);
                    }
                    else
                        sc2.render(cell);
                });
                valueEl.append(row);
            };
            FieldRow.FieldRow = new FieldRow();
            return FieldRow;
        })(Luxena.CompositeFieldType);
        FieldTypes.FieldRow = FieldRow;
    })(FieldTypes = Luxena.FieldTypes || (Luxena.FieldTypes = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var FieldTypes;
    (function (FieldTypes) {
        var Grid = (function (_super) {
            __extends(Grid, _super);
            function Grid() {
                _super.apply(this, arguments);
                this._gridMode = true;
                this._isComposite = true;
            }
            Grid.prototype.render = function (sf, valueEl, rowEl) {
                var cfg = this.getControllerConfig(sf, "GridController", { inline: true });
                //var totalCount = ko.observable(0);
                //cfg.onTotalCountChange = (ctrl, newCount) => totalCount(newCount);
                //sf.otitle(() => (sf._titles || sf._title || sf._names || sf._name) + " (" + totalCount() + ")");
                var scope = new Luxena.GridController(cfg).getScope();
                sf._controller.widgets[sf.uname()] = scope;
                valueEl.append("<div data-bind=\"dxDataGrid: widgets." + sf.uname() + ".gridOptions\"></div>");
            };
            Grid.Grid = new Grid();
            return Grid;
        })(FieldTypes.CollectionFieldType);
        FieldTypes.Grid = Grid;
    })(FieldTypes = Luxena.FieldTypes || (Luxena.FieldTypes = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var FieldTypes;
    (function (FieldTypes) {
        var CurrencyCode = (function (_super) {
            __extends(CurrencyCode, _super);
            function CurrencyCode() {
                _super.apply(this, arguments);
            }
            CurrencyCode.prototype.renderEditor = function (sf, valueEl, rowEl) {
                this.appendEditor(sf, valueEl, "dxSelectBox", {
                    value: sf.getModelValue(),
                    dataSource: CurrencyCode.Codes,
                });
            };
            CurrencyCode.CurrencyCode = new CurrencyCode();
            CurrencyCode.Codes = ["UAH", "USD", "EUR", "RUB", "AED", "AFN", "ALL", "AMD", "ANG", "AOA", "ARS", "AUD", "AUH", "AWG", "AZN", "BAM", "BBD", "BDT", "BGN", "BHD", "BIF", "BMD", "BND", "BOB", "BOV", "BRL", "BSD", "BTN", "BWP", "BYR", "BZD", "CAD", "CDF", "CHE", "CHF", "CHW", "CLF", "CLP", "CNY", "COP", "COU", "CRC", "CUC", "CUP", "CVE", "CZK", "DJF", "DKK", "DOP", "DZD", "EEK", "EGP", "ERN", "ETB", "FJD", "FKP", "GBP", "GEL", "GHC", "GHS", "GIP", "GMD", "GNF", "GTQ", "GWP", "GYD", "HKD", "HNL", "HRK", "HTG", "HUF", "IDR", "ILS", "INR", "IQD", "IRR", "ISK", "IUA", "JMD", "JOD", "JPY", "KES", "KGS", "KHR", "KMF", "KPW", "KRW", "KWD", "KYD", "KZT", "LAK", "LBP", "LKR", "LRD", "LSL", "LTL", "LVL", "LYD", "MAD", "MDL", "MGA", "MKD", "MMK", "MNT", "MOP", "MRO", "MTL", "MUR", "MVR", "MWK", "MXN", "MXV", "MYR", "MZM", "MZN", "NAD", "NGN", "NIO", "NOK", "NPR", "NUC", "NZD", "OMR", "PAB", "PEN", "PGK", "PHP", "PKR", "PLN", "PYG", "QAR", "ROL", "RON", "RSD", "RWF", "SAR", "SBD", "SCR", "SDG", "SEK", "SGD", "SHP", "SIT", "SKK", "SLL", "SOS", "SRD", "STD", "SVC", "SYP", "SZL", "THB", "TJS", "TMT", "TND", "TOP", "TRY", "TTD", "TWD", "TZS", "UGX", "USN", "USS", "UYI", "UYU", "UZS", "VEF", "VND", "VUV", "WST", "XAF", "XAG", "XAU", "XBA", "XBB", "XBC", "XBD", "XCD", "XDR", "XOF", "XPD", "XPF", "XPT", "XTS", "XXX", "YER", "ZAR", "ZMK", "ZWD", "ZWL",];
            return CurrencyCode;
        })(Luxena.SemanticFieldType);
        FieldTypes.CurrencyCode = CurrencyCode;
        var Money = (function (_super) {
            __extends(Money, _super);
            function Money() {
                _super.apply(this, arguments);
                this.dataType = "number";
                this.length = 12;
                this.allowGrouping = false;
            }
            Money.prototype.getFilterExpr = function (sm, value, operation) {
                return [sm._name + ".Amount", operation || "=", value];
            };
            Money.prototype.loadFromData = function (sf, model, data) {
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
            Money.prototype.saveToData = function (sf, model, data) {
                var value = model[sf._name];
                data[sf._name] = {
                    Amount: ko.unwrap(value.Amount),
                    CurrencyId: ko.unwrap(value.CurrencyId),
                };
            };
            Money.prototype.toGridColumns = function (sf) {
                //var sm = sf.member;
                var col = this.toStdGridColumn(sf);
                col.dataField += ".Amount";
                //col.calculateCellValue = data => moneyToString(data[sm._name]);
                return [col];
            };
            Money.prototype.renderDisplayStatic = function (sf, container, data) {
                var value = data[sf._name];
                if (!value)
                    return;
                value = Luxena.moneyToString(value);
                if (sf._member._kind === Luxena.SemanticMemberKind.Important)
                    value = "<b>" + value + "</b>";
                container.append(value);
            };
            Money.prototype.getDisplayValueVisible = function (sf, model) {
                return function () {
                    var value = model[sf._name];
                    return value && value.Amount();
                };
            };
            Money.prototype.renderDisplayBind = function (sf, valueEl, rowEl) {
                $("<div>")
                    .addClass("money-display-row")
                    .attr("data-bind", "text: Luxena.moneyToString(r." + sf._name + ")")
                    .appendTo(valueEl);
            };
            Money.prototype.renderEditor = function (sf, valueEl, rowEl) {
                var sm = sf._member;
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
                sf2._name += "_amount";
                this.appendEditor(sf2, amountDiv, "dxNumberBox", {
                    value: value.Amount,
                });
                sf2 = $.extend({}, sf);
                sf2._name += "_currency";
                this.appendEditor(sf2, currencyDiv, "dxSelectBox", {
                    value: value.CurrencyId,
                    dataSource: CurrencyCode.Codes,
                });
            };
            Money.Money = new Money();
            return Money;
        })(Luxena.SemanticFieldType);
        FieldTypes.Money = Money;
    })(FieldTypes = Luxena.FieldTypes || (Luxena.FieldTypes = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    function moneyToString(v) {
        if (!v)
            return "";
        var amount = v.Amount && ko.unwrap(v.Amount);
        return !amount /*&& amount !== 0*/ ? "" : Globalize.format(amount, "n2") + " " + (ko.unwrap(v.CurrencyId) || "");
    }
    Luxena.moneyToString = moneyToString;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var FieldTypes;
    (function (FieldTypes) {
        var Numeric = (function (_super) {
            __extends(Numeric, _super);
            function Numeric() {
                _super.apply(this, arguments);
                this.allowGrouping = false;
                this.length = 10;
                this.dataType = "number";
                this.chartDataType = "numeric";
            }
            Numeric.prototype.initMember = function (sm) {
                if (!sm._format)
                    sm._format = "n";
            };
            Numeric.prototype.toGridColumns = function (sf) {
                var sm = sf._member;
                var col = this.toStdGridColumn(sf);
                col.calculateCellValue = function (r) { return r[sm._name] || null; };
                if (sm._precision) {
                    col.format = "n" + (sm._precision || "");
                    col.precision = sm._precision;
                }
                return [col];
            };
            Numeric.prototype.toGridTotalItems = function (sf) {
                var sm = sf._member;
                if (!sm._useTotalSum)
                    return [];
                return [{
                        column: sm._name,
                        summaryType: "sum",
                        displayFormat: "{0}",
                        valueFormat: "n" + (sm._precision || 0),
                    }];
            };
            Numeric.prototype.renderDisplayStatic = function (sf, container, data) {
                var value = data[sf._name];
                if (!value)
                    return;
                value = Globalize.format(ko.unwrap(value), "n" + (sf._member._precision || ""));
                if (sf._member._kind === Luxena.SemanticMemberKind.Important)
                    value = "<b>" + value + "</b>";
                container.append(
                //`<div style="max-width: 98px; text-align: right">${sf._member.getIconHtml()}${value}</div>`
                "" + sf._member.getIconHtml() + value);
            };
            Numeric.prototype.renderDisplayBind = function (sf, valueEl, rowEl) {
                var iconHtml = sf._member.getIconHtml();
                if (iconHtml)
                    valueEl.append("<div style=\"max-width: 98px; text-align: right\" data-bind=\"html: '" + iconHtml + "' + Globalize.format(r." + sf._name + "(), 'n" + (sf._member._precision || "") + "')\"> </div>");
                else
                    valueEl.append("<div style=\"max-width: 98px; text-align: right\" data-bind=\"text: Globalize.format(r." + sf._name + "(), 'n" + (sf._member._precision || "") + "')\"></div>");
            };
            Numeric.prototype.renderEditor = function (sf, valueEl, rowEl) {
                valueEl = $("<div style='max-width: 164px'></div>")
                    .appendTo(valueEl);
                this.appendTextBoxEditor(sf, valueEl, "dxNumberBox", {
                    value: sf.getModelValue(),
                });
            };
            Numeric.Float = new Numeric();
            Numeric.Int = new Numeric();
            Numeric.Percent = new Numeric();
            return Numeric;
        })(Luxena.SemanticFieldType);
        FieldTypes.Numeric = Numeric;
    })(FieldTypes = Luxena.FieldTypes || (Luxena.FieldTypes = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var FieldTypes;
    (function (FieldTypes) {
        var Lookup = (function (_super) {
            __extends(Lookup, _super);
            function Lookup() {
                _super.apply(this, arguments);
            }
            Lookup.prototype.getSelectFieldNames = function (sf) {
                return sf._controller.editMode
                    ? [sf._name + "Id"]
                    : [];
            };
            Lookup.prototype.getExpandFieldNames = function (sf) {
                var sm = sf._member;
                var ref = sm.getLookupEntity();
                var refs = ref._lookupFields;
                if (sf._controller.editMode)
                    return [];
                else
                    return [(sm._name + "($select=" + refs.id + "," + refs.name + ")")];
            };
            Lookup.prototype.getFilterExpr = function (sm, value, operation) {
                return [sm._name + "Id", "=", value];
            };
            Lookup.prototype.loadFromData = function (sf, model, data) {
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
            Lookup.prototype.saveToData = function (sf, model, data) {
                var id = ko.unwrap(model[sf._name]);
                data[sf._name + "Id"] = id || null;
            };
            Lookup.prototype.removeFromData = function (sf, data) {
                delete data[sf._name];
                delete data[sf._name + "Id"];
            };
            Lookup.prototype.toGridColumns = function (sf) {
                var _this = this;
                var sm = sf._member;
                var col = this.toStdGridColumn(sf);
                var ref = sm.getLookupEntity();
                var refs = ref._lookupFields;
                col.dataField += "." + refs.name;
                col.calculateFilterExpression = function (filterValue, selectedFilterOperation) {
                    return [sm._name + "." + refs.name, selectedFilterOperation || "contains", filterValue];
                };
                col.cellTemplate = function (cell, cellInfo) {
                    if (cellInfo.column.groupIndex !== undefined)
                        return;
                    _this.renderDisplayStatic(sf, cell, cellInfo.data, ref);
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
            Lookup.prototype.renderDisplayStatic = function (sf, container, data, ref) {
                var value = data[sf._name];
                if (!value)
                    return;
                renderReferenceDisplay(sf, container, value, ref);
            };
            Lookup.prototype.getDisplayValueVisible = function (sf, model) {
                return function () {
                    var value = model[sf._name];
                    return value && value().Id;
                };
            };
            Lookup.prototype.renderDisplayVisible = function (sf, valueEl, rowEl) {
                rowEl.attr("data-bind", "visible: r." + sf._name + " && r." + sf._name + "().Id");
            };
            Lookup.prototype.renderDisplayBind = function (sf, valueEl, rowEl) {
                var ctrl = sf._controller;
                ctrl.widgets[sf.uname()] = {
                    renderer: referenceDisplayRendererByData(sf, ctrl.model),
                };
                $("<span>")
                    .attr("data-bind", "renderer: widgets." + sf.uname() + ".renderer")
                    .appendTo(valueEl);
            };
            Lookup.prototype.renderEditor = function (sf, valueEl, rowEl) {
                var sm = sf._member;
                var ref = sm.getLookupEntity();
                var refs = ref._lookupFields;
                var defaultIconHtml = ref._textIconHtml;
                var itemTemplate;
                if (ref._lookupItemTemplate) {
                    itemTemplate = function (data, index, container) {
                        var result = ref._lookupItemTemplate(data, index, container);
                        if (typeof result === "string" && result.indexOf("<") >= 0)
                            container.html(result);
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
                    options.title = sm._title;
                    options.showPopupTitle = false;
                    options.cleanSearchOnOpening = false;
                    options.pageLoadMode = "scrollBottom";
                    this.appendTextBoxEditor(sf, valueEl, "dxLookup", options);
                }
            };
            Lookup.Reference = new Lookup();
            return Lookup;
        })(Luxena.SemanticFieldType);
        FieldTypes.Lookup = Lookup;
        function referenceDisplayRendererByData(sf, data) {
            return function (container) {
                if (!data)
                    return;
                var value = ko.unwrap(data[sf._name]);
                renderReferenceDisplay(sf, container, value);
            };
        }
        FieldTypes.referenceDisplayRendererByData = referenceDisplayRendererByData;
        function renderReferenceDisplay(sf, container, v, ref) {
            if (!v)
                return;
            ref = ref || sf._member.getLookupEntity();
            if (!ref)
                return;
            var ref2 = Luxena.sd.entityByOData(v, ref);
            var refs = ref2._lookupFields;
            var id = ko.unwrap(v[refs.id]);
            var name = ko.unwrap(v[refs.name]);
            if (!id && !name)
                return;
            var iconHtml = (ref2 ? ref2._textIconHtml : null) || ref._textIconHtml;
            if (sf._member._kind === Luxena.SemanticMemberKind.Important)
                name = "<b>" + name + "</b>";
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
        FieldTypes.renderReferenceDisplay = renderReferenceDisplay;
    })(FieldTypes = Luxena.FieldTypes || (Luxena.FieldTypes = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var FieldTypes;
    (function (FieldTypes) {
        var BaseTextFieldType = (function (_super) {
            __extends(BaseTextFieldType, _super);
            function BaseTextFieldType() {
                _super.apply(this, arguments);
            }
            BaseTextFieldType.prototype.getFilterExpr = function (sm, value, operation) {
                return [sm._name, operation || "contains", value];
            };
            BaseTextFieldType.prototype.renderDisplayBind = function (sf, valueEl, rowEl) {
                valueEl.addClass("pre");
                _super.prototype.renderDisplayBind.call(this, sf, valueEl, rowEl);
            };
            BaseTextFieldType.prototype.renderEditor = function (sf, valueEl, rowEl) {
                this.appendTextBoxEditor(sf, valueEl, "dxTextBox", {
                    value: sf.getModelValue(),
                    mask: this.mask,
                    mode: this.mode,
                    maxLength: this.maxLength,
                });
            };
            return BaseTextFieldType;
        })(Luxena.SemanticFieldType);
        FieldTypes.BaseTextFieldType = BaseTextFieldType;
        var TextArea = (function (_super) {
            __extends(TextArea, _super);
            function TextArea() {
                _super.apply(this, arguments);
            }
            TextArea.prototype.renderEditor = function (sf, valueEl, rowEl) {
                var sm = sf._member;
                this.appendTextBoxEditor(sf, valueEl, "dxTextArea", {
                    value: sf.getModelValue(),
                    height: sm._lineCount ? (sm._lineCount * 46 - 10) : undefined,
                });
            };
            return TextArea;
        })(BaseTextFieldType);
        FieldTypes.TextArea = TextArea;
        var CodeTextArea = (function (_super) {
            __extends(CodeTextArea, _super);
            function CodeTextArea() {
                _super.apply(this, arguments);
            }
            CodeTextArea.prototype.renderEditor = function (sf, valueEl, rowEl) {
                var sm = sf._member;
                this.appendTextBoxEditor(sf, valueEl, "dxTextArea", {
                    value: sf.getModelValue(),
                    height: sm._lineCount ? (sm._lineCount * 64 - 16 - 12) : undefined,
                });
            };
            return CodeTextArea;
        })(BaseTextFieldType);
        FieldTypes.CodeTextArea = CodeTextArea;
        var Password = (function (_super) {
            __extends(Password, _super);
            function Password() {
                _super.apply(this, arguments);
                this.maxLength = 20;
                this.mode = "password";
            }
            return Password;
        })(BaseTextFieldType);
        FieldTypes.Password = Password;
        var Address = (function (_super) {
            __extends(Address, _super);
            function Address() {
                _super.apply(this, arguments);
                this._icon = "map-marker";
            }
            Address.prototype.renderDisplayStatic = function (sf, container, data) {
                var value = $clip(data[sf._name]);
                if (!value)
                    return;
                var evalue = encodeURI(value);
                container.addClass("pre").append(sf._member.getIconHtml() +
                    ("<span>" + value + "</span>&nbsp; ") +
                    "<span class=\"nowrap\">" +
                    ("<a title=\"Yandex Maps\" target=\"_blank\" href=\"http://maps.yandex.ru/?text=" + evalue + "\" class=\"icon-yandex-map\"></a> ") +
                    ("<a title=\"Google Maps\" target=\"_blank\" href=\"http://maps.google.com/maps?q=" + evalue + "\" class=\"icon-google-map\"></a>") +
                    "</span>");
            };
            Address.prototype.renderDisplayBind = function (sf, valueEl, rowEl) {
                valueEl.addClass("pre").append(sf._member.getIconHtml() +
                    ("<span data-bind=\"text: r." + sf._name + "\"></span> &nbsp;") +
                    "<span class=\"nowrap\">" +
                    ("<a title=\"Yandex Maps\" target=\"_blank\" data-bind=\"attr: { href: 'http://maps.yandex.ru/?text=' + encodeURI(r." + sf._name + "()) }\" class=\"icon-yandex-map\"></a> ") +
                    ("<a title=\"Google Maps\" target=\"_blank\" data-bind=\"attr: { href: 'http://maps.google.com/maps?q=' + encodeURI(r." + sf._name + "()) }\" class=\"icon-google-map\"></a>") +
                    "</span>");
            };
            return Address;
        })(TextArea);
        FieldTypes.Address = Address;
        var Email = (function (_super) {
            __extends(Email, _super);
            function Email() {
                _super.apply(this, arguments);
                this.mode = "email";
                this._icon = "envelope";
                this._singleLine = true;
            }
            Email.prototype.renderDisplayStatic = function (sf, container, data) {
                var value = $clip(data[sf._name]);
                if (!value)
                    return;
                container.addClass("nowrap");
                container.append("<span class=\"nowrap\">" + sf._member.getIconHtml() + "<a class=\"dx-link\" href=\"mailto:" + value + "\" target=\"_blank\">" + value + "</a></span>");
            };
            Email.prototype.renderDisplayBind = function (sf, valueEl, rowEl) {
                valueEl.addClass("nowrap");
                valueEl.append(sf._member.getIconHtml() +
                    ("<a data-bind=\"text: r." + sf._name + ", attr: { href: 'mailto:' + r." + sf._name + "() }\"></a>"));
            };
            return Email;
        })(BaseTextFieldType);
        FieldTypes.Email = Email;
        var Phone = (function (_super) {
            __extends(Phone, _super);
            function Phone() {
                _super.apply(this, arguments);
                this.mode = "tel";
                this._icon = "phone";
                this._singleLine = true;
            }
            return Phone;
        })(BaseTextFieldType);
        FieldTypes.Phone = Phone;
        var Fax = (function (_super) {
            __extends(Fax, _super);
            function Fax() {
                _super.apply(this, arguments);
                this.mode = "tel";
                this._icon = "fax";
                this._singleLine = true;
            }
            return Fax;
        })(BaseTextFieldType);
        FieldTypes.Fax = Fax;
        var Hyperlink = (function (_super) {
            __extends(Hyperlink, _super);
            function Hyperlink() {
                _super.apply(this, arguments);
                this.mode = "url";
                this._icon = "globe";
            }
            Hyperlink.prototype.renderDisplayStatic = function (sf, container, data) {
                var value = Luxena.toExternalUrl(data[sf._name]);
                if (!value)
                    return;
                container.addClass("nowrap");
                container.append(sf._member.getIconHtml() +
                    ("<a class=\"dx-link\" href=\"" + value + "\" target=\"_blank\">" + value + "</a>"));
            };
            Hyperlink.prototype.renderDisplayBind = function (sf, valueEl, rowEl) {
                valueEl.addClass("nowrap");
                valueEl.append(sf._member.getIconHtml() +
                    ("<a data-bind=\"text: r." + sf._name + ", attr: { href: Luxena.toExternalUrl(r." + sf._name + "()) }\" target=\"_blank\"></a>"));
            };
            return Hyperlink;
        })(BaseTextFieldType);
        FieldTypes.Hyperlink = Hyperlink;
        var Text = (function (_super) {
            __extends(Text, _super);
            function Text() {
                _super.apply(this, arguments);
            }
            Text.prototype.toGridColumns = function (sf) {
                var sm = sf._member;
                var se = sm._entity;
                var col = this.toStdGridColumn(sf);
                if (sm._isEntityName) {
                    col.width = Math.round(col.width * 1.29);
                    col.cellTemplate = function (cell, cellInfo) {
                        if (cellInfo.column.groupIndex !== undefined)
                            return;
                        var data = cellInfo.data;
                        var v = data[sf._name];
                        if (!v)
                            return;
                        var id = data[se._lookupFields.id];
                        cell.append("<a class=\"dx-link entity-name-cell\" href=\"#" + data._viewEntity._name + "/" + id + "\">" + v + "</a>");
                    };
                }
                return [col];
            };
            Text.String = new Text();
            Text.Text = new TextArea();
            Text.CodeText = new CodeTextArea();
            Text.Password = new Password();
            Text.Address = new Address();
            Text.Email = new Email();
            Text.Phone = new Phone();
            Text.Fax = new Fax();
            Text.Hyperlink = new Hyperlink();
            return Text;
        })(BaseTextFieldType);
        FieldTypes.Text = Text;
    })(FieldTypes = Luxena.FieldTypes || (Luxena.FieldTypes = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    function toExternalUrl(url) {
        url = $clip(url);
        if (!url)
            return "";
        if (url.indexOf("://") < 0)
            url = "http://" + url;
        url = url.replace("\"", "");
        return url;
    }
    Luxena.toExternalUrl = toExternalUrl;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Components;
    (function (Components) {
        var Container = (function (_super) {
            __extends(Container, _super);
            function Container() {
                _super.apply(this, arguments);
            }
            Container.prototype.items = function () {
                var value = [];
                for (var _i = 0; _i < arguments.length; _i++) {
                    value[_i - 0] = arguments[_i];
                }
                this._items = value;
                return this;
            };
            Container.prototype.addItemsToController = function (action) {
                this._components = this._controller.addComponents(this._items, this, null, action);
            };
            return Container;
        })(Luxena.SemanticComponent);
        Components.Container = Container;
    })(Components = Luxena.Components || (Luxena.Components = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Components;
    (function (Components) {
        var Card = (function (_super) {
            __extends(Card, _super);
            function Card() {
                _super.apply(this, arguments);
                this._isComposite = true;
            }
            Card.prototype.render = function (container) {
                var _this = this;
                container.addClass("card card-fieldset");
                this._components.forEach(function (sc2) {
                    if (_this._indentLabelItems)
                        sc2.indentLabel();
                    if (_this._labelAsHeaderItems)
                        sc2.labelAsHeader();
                    else if (_this._unlabelItems)
                        sc2.unlabel();
                    else if (_this._hideLabelItems)
                        sc2.hideLabel();
                    sc2.render(container);
                });
            };
            return Card;
        })(Components.Container);
        Components.Card = Card;
    })(Components = Luxena.Components || (Luxena.Components = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Components;
    (function (Components) {
        var TabControl = (function (_super) {
            __extends(TabControl, _super);
            function TabControl() {
                _super.apply(this, arguments);
            }
            TabControl.prototype.card = function (value) {
                this._isCard = value !== false;
                return this;
            };
            TabControl.prototype.renderTabs = function (container, widgetClassName, options) {
                var ctrl = this._controller;
                var widgets = ctrl.widgets;
                var accEl = $("<div data-bind=\"" + widgetClassName + ": widgets." + this.uname() + "\"></div>");
                this._components.forEach(function (sc2, i) {
                    var hdiv2 = Luxena.isField(sc2) && sc2._type._gridMode ? "style=\"padding: 10px\""
                        : sc2 instanceof TabControl ? "style=\"padding-top: 20px\""
                            : "class=\"card-fieldset\"";
                    var itemEl = $("<div data-bind=\"with: $parent\" " + hdiv2 + ">").appendTo($("<div data-options=\"dxTemplate: { name: 'item" + i + "' }\"></div>")
                        .appendTo(accEl));
                    sc2.unlabel();
                    sc2._mustPureRender = true;
                    sc2.render(itemEl);
                });
                options.items = this._components.map(function (tab, i) {
                    var widget = widgets[tab.uname()];
                    return {
                        title: tab.otitle() || tab._title,
                        badge: tab.badge(),
                        icon: tab._icon ? "fa fa-" + tab._icon : "",
                        template: "item" + i,
                        visible: !widget || widget.valueVisible,
                    };
                });
                widgets[this.uname()] = options;
                if (this._isCard)
                    accEl = $("<div class=\"card-accordion\">").append(accEl);
                container.append(accEl);
            };
            return TabControl;
        })(Components.Container);
        Components.TabControl = TabControl;
    })(Components = Luxena.Components || (Luxena.Components = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Components;
    (function (Components) {
        var Accordion = (function (_super) {
            __extends(Accordion, _super);
            function Accordion() {
                _super.apply(this, arguments);
            }
            //static Accordion = new Accordion();
            Accordion.prototype.render = function (container) {
                this.renderTabs(container, "dxAccordion", {
                    collapsible: true,
                });
            };
            return Accordion;
        })(Components.TabControl);
        Components.Accordion = Accordion;
    })(Components = Luxena.Components || (Luxena.Components = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Components;
    (function (Components) {
        var Button = (function (_super) {
            __extends(Button, _super);
            function Button(_action) {
                _super.call(this);
                this._action = _action;
                if (_action) {
                    this._entity = _action._entity;
                    this._name = _action._name + "Button";
                    this._icon = _action._icon;
                    this._title = _action._title;
                    this._description = _action._description;
                    this._buttonType = _action._buttonType;
                    this._onExecute = _action._onExecute;
                    this._navigateOptions = _action._navigateOptions;
                }
            }
            Button.prototype.getId = function () { return this._controller.getId(); };
            //#region Setters
            Button.prototype.onExecute = function (value, navigateOptions) {
                this._onExecute = value;
                if (navigateOptions)
                    this._navigateOptions = navigateOptions;
                return this;
            };
            Button.prototype.onExecuteDone = function (value) {
                this._onExecuteDone = value;
                return this;
            };
            Button.prototype.navigateOptions = function (value) {
                this._navigateOptions = value;
                return this;
            };
            Button.prototype.items = function () {
                var value = [];
                for (var _i = 0; _i < arguments.length; _i++) {
                    value[_i - 0] = arguments[_i];
                }
                this._items = value;
                return this;
            };
            Button.prototype.normal = function () {
                this._buttonType = "normal";
                return this;
            };
            Button.prototype.default = function () {
                this._buttonType = "default";
                return this;
            };
            Button.prototype.success = function () {
                this._buttonType = "success";
                return this;
            };
            Button.prototype.danger = function () {
                this._buttonType = "danger";
                return this;
            };
            Button.prototype.back = function () {
                this._buttonType = "back";
                this._title = "Назад";
                this._onExecute = function () { return Luxena.app.back(); };
                return this;
            };
            Button.prototype.small = function (value) {
                this._isSmall = value !== false;
                return this;
            };
            Button.prototype.location = function (value) {
                if (value === "left")
                    value = "before";
                else if (value === "right")
                    value = "after";
                this._location = value;
                return this;
            };
            Button.prototype.left = function () {
                this._location = "before";
                return this;
            };
            Button.prototype.right = function () {
                this._location = "after";
                return this;
            };
            Button.prototype.template = function (value) {
                this._template = value;
                return this;
            };
            //#endregion
            Button.prototype.addItemsToController = function () {
                this._buttons = this._controller.addComponents(this._items, this, null);
            };
            Button.prototype.getExecuter = function (prms) {
                var _this = this;
                prms = prms || {};
                if (this._getParams)
                    $.extend(prms, this._getParams(prms));
                var onExecute = this._onExecute;
                if (!onExecute)
                    return function () { return _this.execODataAction(prms); };
                else if ($.isFunction(onExecute))
                    return function () {
                        var result = onExecute(_this, prms);
                        if (_this._onExecuteDone)
                            if (result && $.isFunction(result.done))
                                result.done(function () { return _this._onExecuteDone(_this, prms); });
                            else
                                _this._onExecuteDone(_this, prms);
                    };
                if (!prms.view) {
                    if (typeof onExecute === "string")
                        prms.view = onExecute;
                    else if (Luxena.isEntity(onExecute))
                        prms.view = onExecute.editView();
                }
                else if (Luxena.isEntity(prms.view))
                    prms.view = prms.view.editView();
                var action = function () { return Luxena.app.navigate(prms, _this._navigateOptions); };
                return action;
            };
            Button.prototype.execODataAction = function (prms) {
                var ctrl = this._controller;
                var fctrl = ctrl instanceof Luxena.FormController ? ctrl : null;
                var ectrl = ctrl instanceof Luxena.EditFormController ? ctrl : null;
                if (ectrl)
                    ectrl.isRecalculating = true;
                if (fctrl) {
                    fctrl.loadingMessage(this._title + "...");
                    fctrl.modelIsLoading(true);
                }
                var store = this._entity._store;
                var id = this.getId();
                var url = store["_byKeyUrl"](id || "") + "/Default." + this._name;
                var select = fctrl.dataSourceConfig.select;
                var expand = ctrl.viewMode ? fctrl.dataSourceConfig.expand : undefined;
                if (select && select.length)
                    url += "?$select=" + select.join(",").replace(",$usecalculated", "") + "&$expand=" + (expand || []).join(",");
                var d = $.Deferred();
                var qprms = $.extend({}, prms);
                if ($.isFunction(qprms._delta))
                    qprms._delta = qprms._delta();
                if (ectrl)
                    qprms._delta = JSON.stringify(ectrl.saveToData());
                else if (ctrl.viewMode)
                    qprms._save = true;
                $.when(store["_sendRequest"](url, "POST", null, qprms))
                    .done(function (newData) { return d.resolve(qprms.id, newData); })
                    .done(function (data) {
                    if (fctrl && (ctrl.editMode || ctrl.viewMode))
                        fctrl.loadFromData(data);
                })
                    .fail(d.reject, d)
                    .fail(Luxena.showError)
                    .always(function () {
                    fctrl && fctrl.modelIsLoading(false);
                    if (ectrl)
                        ectrl.isRecalculating = false;
                });
                return d.promise();
            };
            Button.prototype.render = function (container) {
                var ctrl = this._controller;
                ctrl.widgets[this.uname()] = this.buttonOptions();
                container.append("<div data-bind=\"dxButton: widgets." + this.uname() + "\">");
            };
            Button.prototype.buttonOptions = function (prms) {
                return {
                    icon: this._icon,
                    text: this._isSmall ? undefined : this._title,
                    hint: this._title + ($as(this._description, function (a) { return ": " + a; }) || ""),
                    type: this._buttonType || "normal",
                    onClick: this.getExecuter(prms),
                };
            };
            Button.prototype.dropDownMenuItemOptions = function (prms) {
                return {
                    icon: this._icon,
                    text: this._title,
                    hint: this._title + ($as(this._description, function (a) { return ": " + a; }) || ""),
                    onClick: this.getExecuter(prms),
                };
            };
            Button.prototype.toolbarItemOptions = function (prms) {
                var options = {
                    location: this._location,
                };
                if (this._template) {
                    options.template = this._template;
                }
                else if (this._buttons && this._buttons.length) {
                    options.widget = "dropDownMenu";
                    options.options = { items: this._buttons.map(function (a) { return a.dropDownMenuItemOptions(prms); }), };
                }
                else {
                    options.widget = "button";
                    options.options = this.buttonOptions(prms);
                }
                ;
                return options;
            };
            return Button;
        })(Luxena.SemanticComponent);
        Components.Button = Button;
    })(Components = Luxena.Components || (Luxena.Components = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Field = (function (_super) {
        __extends(Field, _super);
        function Field(_member) {
            var _this = this;
            _super.call(this);
            this._member = _member;
            this._dependencies = [];
            this._widgetOptions = {};
            this.readonly = false;
            this._rowClasses = [];
            this.chartController = function (options) {
                return _this.widgetOptions("ChartController", options);
            };
            this.gridController = function (options) {
                return _this.widgetOptions("GridController", options);
            };
            this.textBox = function (options) {
                return _this.widgetOptions("dxTextBox", options);
            };
            this.numberBox = function (options) {
                return _this.widgetOptions("dxNumberBox", options);
            };
            this._entity = _member._entity;
            this._name = _member._name;
            this._icon = _member._icon;
            this._title = _member._title;
            this._type = _member._type;
            _member.prepare();
        }
        //#region Setters
        Field.prototype.items = function () {
            var members = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                members[_i - 0] = arguments[_i];
            }
            this._members = members;
            return this;
        };
        Field.prototype.dependencies = function () {
            var _this = this;
            var value = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                value[_i - 0] = arguments[_i];
            }
            if (value) {
                value.forEach(function (a) {
                    if (_this._dependencies.indexOf(a) < 0)
                        _this._dependencies.push(a);
                });
            }
            return this;
        };
        Field.prototype.addRowClass = function () {
            var classes = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                classes[_i - 0] = arguments[_i];
            }
            var rowClasses = this._rowClasses;
            classes.forEach(function (cls) {
                if (rowClasses.indexOf(cls) < 0)
                    rowClasses.push(cls);
            });
            return this;
        };
        Field.prototype.hidden = function () {
            this._isHidden = true;
            this._columnVisible = false;
            this._selectRequired = true;
            return this;
        };
        Field.prototype.reserved = function (value) {
            this._isReserved = true;
            this._columnVisible = false;
            return this;
        };
        Field.prototype.compact = function (value) {
            this._isCompact = value !== false;
            return this;
        };
        //card(value?: boolean)
        //{
        //	this._isCard = value !== false;
        //	return this;
        //}
        Field.prototype.header1 = function (value) {
            this._isHeader1 = value !== false;
            this._unlabel = true;
            return this;
        };
        Field.prototype.header2 = function (value) {
            this._isHeader2 = value !== false;
            this._unlabel = true;
            return this;
        };
        Field.prototype.header3 = function (value) {
            this._isHeader3 = value !== false;
            //this._useBorderBottom = true;
            this._unlabel = true;
            return this;
        };
        Field.prototype.header4 = function (value) {
            this._isHeader4 = value !== false;
            this._unlabel = true;
            return this;
        };
        Field.prototype.header5 = function (value) {
            this._isHeader5 = value !== false;
            this._unlabel = true;
            return this;
        };
        Field.prototype.width = function (value) {
            this._width = value;
            return this;
        };
        Field.prototype.height = function (value) {
            this._height = value;
            return this;
        };
        Field.prototype.impotent = function (value) {
            this._isImpotent = value !== false;
            return this;
        };
        Field.prototype.borderLeft = function (value) {
            this._useBorderLeft = value !== false;
            return this;
        };
        Field.prototype.borderRight = function (value) {
            this._useBorderRight = value !== false;
            return this;
        };
        Field.prototype.borderTop = function (value) {
            this._useBorderTop = value !== false;
            return this;
        };
        Field.prototype.borderBottom = function (value) {
            this._useBorderBottom = value !== false;
            return this;
        };
        Field.prototype.groupIndex = function (value) {
            this._groupIndex = value;
            return this;
        };
        Field.prototype.ungroup = function () {
            this._groupIndex = -1;
            return this;
        };
        //#endregion
        //#region Widget Options
        Field.prototype.widgetOptions = function (typeName, options) {
            var exists = this._widgetOptions[typeName];
            this._widgetOptions[typeName] = exists ? $.extend(exists, options) : options;
            return this;
        };
        //#endregion
        //#region Data & Model
        Field.prototype.getSelectFieldNames = function () {
            var names = this._type.getSelectFieldNames(this);
            return names;
        };
        Field.prototype.getExpandFieldNames = function () {
            return this._type.getExpandFieldNames(this);
        };
        Field.prototype.loadFromData = function (model, data) {
            this._type.loadFromData(this, model, data);
        };
        Field.prototype.saveToData = function (model, data) {
            this._type.saveToData(this, model, data);
        };
        Field.prototype.removeFromData = function (data) {
            this._type.removeFromData(this, data);
        };
        Field.prototype.getModelValue = function () {
            var model = this._controller.model;
            return !model ? undefined : model[this._name];
        };
        Field.prototype.setModelValue = function (value) {
            var model = this._controller.model;
            if (model)
                this._type.setModel(model, this._name, value);
        };
        Field.prototype.setModelValueDefault = function (value) {
            var model = this._controller.model;
            if (model && !ko.unwrap(model[this._name]))
                this._type.setModel(model, this._name, value);
        };
        //#endregion
        Field.prototype.isComposite = function () {
            return this._type._isComposite;
        };
        Field.prototype.addItemsToController = function (action) {
            var _this = this;
            var ctrl = this._controller;
            ctrl.fields.push(this);
            var comps = ctrl.components;
            this._dependencies.forEach(function (d) {
                if (!d || comps.filter(function (a) { return a._name === d._name; }).length)
                    return;
                if (isField(d))
                    ctrl.addComponent(d, _this, null);
                else if (d instanceof Luxena.SemanticMember)
                    ctrl.addComponent(d.hidden(), _this, null);
            });
        };
        Field.prototype.toGridColumns = function () {
            return this._type.toGridColumns(this);
        };
        Field.prototype.toGridTotalItems = function () {
            return this._type.toGridTotalItems(this);
        };
        Field.prototype.renderDisplayStatic = function (container, data) {
            this._type.renderDisplayStatic(this, container, data);
        };
        Field.prototype.render = function (container) {
            var sf = this;
            var sm = sf._member;
            if (sf._isHidden)
                return;
            //if (sm._required)
            //	sf.impotent();
            sf.renderField(container, {
                title: (sm._title || "") + (sm._description ? ": " + sm._description : ""),
                fieldLabel: sf._title,
                isReadOnly: sm._isReadOnly,
            });
        };
        Field.prototype.renderField = function (container, cfg) {
            var sf = this;
            //const sm = sf._member;
            var type = sf._type;
            if (sf._mustPureRender) {
                container.addClass(this._rowClasses.join(" "));
                sf._type.pureRender(sf, container);
                return;
            }
            var title = this._title = cfg.title || this._title || "";
            var fieldLabel = cfg.fieldLabel || title || "";
            type.prerender(sf);
            var rowEl = $("<div>").addClass(this._rowClasses.join(" "));
            if (title)
                rowEl.attr("title", title);
            var useLabel = fieldLabel && !this._unlabel && !sf._labelAsHeader;
            if (!useLabel)
                rowEl.addClass("field-label-none");
            rowEl.addClass("dx-field");
            if (fieldLabel) {
                if (this._isImpotent)
                    fieldLabel = "<b>" + fieldLabel + "</b>";
                if (sf._labelAsHeader) {
                    $("<div>")
                        .addClass("dx-fieldset-header")
                        .html(fieldLabel)
                        .appendTo(rowEl);
                }
                else if (useLabel) {
                    if (sf._hideLabel)
                        rowEl.addClass("field-label-hide");
                    else if (!sf._unlabel)
                        $("<div>")
                            .addClass("dx-field-label")
                            .addClass(sf._indentLabel ? "field-label-indent" : "")
                            .html(fieldLabel) // + ":")
                            .appendTo(rowEl);
                }
            }
            var valueEl = $("<div>")
                .addClass(sf._controller.editMode && !cfg.isReadOnly ? "dx-field-value" : "dx-field-value-static")
                .appendTo(rowEl);
            if (this._isHeader1)
                valueEl = $("<h1>").appendTo(valueEl);
            else if (this._isHeader2)
                valueEl = $("<h2>").appendTo(valueEl);
            else if (this._isHeader3)
                valueEl = $("<h3>").appendTo(valueEl);
            else if (this._isHeader4)
                valueEl = $("<h4>").appendTo(valueEl);
            else if (this._isHeader5)
                valueEl = $("<h5>").appendTo(valueEl);
            if (this._useBorderLeft)
                rowEl.addClass("field-border-left");
            if (this._useBorderRight)
                rowEl.addClass("field-border-right");
            if (this._useBorderTop)
                rowEl.addClass("field-border-top");
            if (this._useBorderBottom)
                rowEl.addClass("field-border-bottom");
            sf._type.render(sf, valueEl, rowEl);
            //if (sf._isCard || sm._isCard)
            //	rowEl = $(`<div class="card card-fieldset">`).append($(`<div class="dx-fieldset">`).append(rowEl));
            rowEl.appendTo(container);
        };
        Field.prototype.getIconHtml = function (icon, withTitle) {
            return this._member.getIconHtml(this._icon, withTitle);
        };
        Field.prototype.getLength = function () {
            var l = this._member.getLength();
            if (this._length)
                l.length = this._length;
            return l;
        };
        Field.prototype.getWidth = function (length) {
            var sf = this;
            var sm = sf._member;
            if (sf._width)
                return sf._width;
            if (sm._width)
                return sm._width;
            if (!length)
                length = sm.getLength().length;
            if (!length)
                return undefined;
            if (length < 2)
                length = 2;
            var cfg = sf._controller.config;
            var type = sf._type;
            return (14
                + Math.round((type && type.charWidth || Luxena.SemanticFieldType.charWidth) * length)
                + (cfg.useFilter && cfg.useFilterRow && type && type.allowFiltering && sm._allowFiltering ? 12 + (type.addColumnFilterWidth || 0) : 0));
        };
        return Field;
    })(Luxena.SemanticComponent);
    Luxena.Field = Field;
    function isField(o) {
        return o instanceof Field;
    }
    Luxena.isField = isField;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Components;
    (function (Components) {
        var TabPanel = (function (_super) {
            __extends(TabPanel, _super);
            function TabPanel() {
                _super.apply(this, arguments);
            }
            //static TabPanel = new TabPanel();
            TabPanel.prototype.render = function (container) {
                this.renderTabs(container, "dxTabPanel", {
                    collapsible: true,
                });
            };
            return TabPanel;
        })(Components.TabControl);
        Components.TabPanel = TabPanel;
    })(Components = Luxena.Components || (Luxena.Components = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Components;
    (function (Components) {
        var Toolbar = (function (_super) {
            __extends(Toolbar, _super);
            function Toolbar() {
                _super.apply(this, arguments);
            }
            Toolbar.prototype.addItemsToController = function () {
                this._buttons = this._controller.addComponents(this._items, this, null);
            };
            Toolbar.prototype.items = function () {
                var value = [];
                for (var _i = 0; _i < arguments.length; _i++) {
                    value[_i - 0] = arguments[_i];
                }
                this._items = value;
                return this;
            };
            Toolbar.prototype.render = function (container) {
                this._controller.widgets[this.uname()] = {
                    items: this._buttons.map(function (btn) {
                        return btn.toolbarItemOptions ? btn.toolbarItemOptions() : btn;
                    }),
                };
                container.append("<div data-bind=\"dxToolbar: widgets." + this.uname() + "\"></div>");
            };
            return Toolbar;
        })(Luxena.SemanticComponent);
        Components.Toolbar = Toolbar;
    })(Components = Luxena.Components || (Luxena.Components = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var SemanticDomain = (function () {
        function SemanticDomain() {
            this.er = function () {
                return "<div class=\"dx-field\"><div class=\"dx-field-value-static\">&nbsp;</div></div>";
            };
            this.hr = function () { return "<hr/>"; };
            this.hr2 = function () { return "<div class=\"form-divider\"><div></div><div></div></div>"; };
            this.hr3 = function () { return "<hr class=\"gap\"/>"; };
            this.hr4 = function () { return "<hr class=\"gap4\"/>"; };
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
        //#region Components
        SemanticDomain.prototype.accordion = function () {
            var items = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                items[_i - 0] = arguments[_i];
            }
            return (_a = new Luxena.Components.Accordion()).items.apply(_a, items);
            var _a;
        };
        SemanticDomain.prototype.tabPanel = function () {
            var items = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                items[_i - 0] = arguments[_i];
            }
            return (_a = new Luxena.Components.TabPanel()).items.apply(_a, items);
            var _a;
        };
        SemanticDomain.prototype.tabCard = function () {
            var items = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                items[_i - 0] = arguments[_i];
            }
            return (_a = new Luxena.Components.TabPanel()).items.apply(_a, items).card();
            var _a;
        };
        SemanticDomain.prototype.card = function () {
            var items = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                items[_i - 0] = arguments[_i];
            }
            return new Luxena.Components.Card().items(items);
        };
        SemanticDomain.prototype.col = function () {
            var items = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                items[_i - 0] = arguments[_i];
            }
            return new Luxena.SemanticMember().col(items).field();
        };
        SemanticDomain.prototype.row = function () {
            var items = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                items[_i - 0] = arguments[_i];
            }
            return new Luxena.SemanticMember().row(items).field();
        };
        SemanticDomain.prototype.header = function (title) {
            title = Luxena.semanticTitleToString(title);
            return "<div class=\"dx-fieldset-header\" style=\"text-align: center\">" + title + "</div>";
        };
        SemanticDomain.prototype.gheader = function (title) {
            title = Luxena.semanticTitleToString(title);
            return "<div class=\"dx-fieldset-header\" style=\"position: absolute; z-index: 999; padding-top: 12px\">" + title + "</div>";
        };
        SemanticDomain.prototype.button = function () {
            var items = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                items[_i - 0] = arguments[_i];
            }
            return (_a = new Luxena.Components.Button()).items.apply(_a, items);
            var _a;
        };
        SemanticDomain.prototype.toolbar = function () {
            var items = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                items[_i - 0] = arguments[_i];
            }
            return (_a = new Luxena.Components.Toolbar()).items.apply(_a, items);
            var _a;
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
            this.iconHtml = ko.observable();
            this.title = ko.observable();
            this.containers = {};
            this.details = [];
            this.widgets = {};
            this.config = cfg;
            var se = this._entity = cfg.entity;
            this.model = cfg.model || {};
            this.modelIsExternal = !!cfg.model;
            cfg.list = cfg.list === null ? null : cfg.list || se;
            cfg.form = cfg.form === null ? null : cfg.form || se;
            cfg.view = cfg.view === null ? null : cfg.view || cfg.form;
            cfg.smart = cfg.smart === null ? null : cfg.smart || (cfg.view === null ? null : cfg.view || cfg.form);
            cfg.edit = cfg.edit === null ? null : cfg.edit || cfg.form;
            //cfg.listAction = cfg.listAction !== undefined && cfg.entity.resolveListAction(cfg.viewAction) || cfg.list.resolveListAction();
            //cfg.viewAction = cfg.viewAction !== undefined && cfg.entity.resolveViewAction(cfg.viewAction) || cfg.view.resolveViewAction();
            //cfg.editAction = cfg.editAction !== undefined && cfg.entity.resolveEditAction(cfg.editAction) || cfg.edit.resolveEditAction();
        }
        SemanticController.prototype.addComponent = function (item, parent, containerId, action, addedComponents) {
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
                //if (sm && this.members.indexOf(sm) >= 0) return null;
                sc = sm.field();
            }
            else if (typeof item === "string") {
                sc = new Luxena.Components.Html(item);
            }
            else if (item instanceof Luxena.SemanticEntityAction) {
                sc = new Luxena.Components.Button(item);
            }
            else if (item instanceof Luxena.SemanticComponent) {
                sc = item;
                sm = sc instanceof Luxena.Field && sc._member;
            }
            if (!sc)
                return null;
            sc._controller = this;
            sc._entity = this._entity;
            sc._containerId = containerId;
            sc._parent = parent;
            this.components.push(sc);
            addedComponents && addedComponents.push(sc);
            //if (sm && this.members.indexOf(sm) < 0 && isField(sc))
            if (sm && Luxena.isField(sc)) {
                sc._type.addItemsToController(sc, this, action);
                this.members.push(sm);
            }
            sc.addItemsToController(action);
            action && action(sm, sc);
            return sc;
        };
        SemanticController.prototype.addComponents = function (items, parent, containerId, action, addedComponents) {
            var _this = this;
            if (!items)
                return [];
            var components = [];
            var item = items;
            if ($.isArray(item) && item.length === 1)
                item = item[0];
            if ($.isFunction(item))
                item = item(this.config.entity);
            if (item instanceof Luxena.SemanticEntity)
                item = item._members;
            if ($.isArray(item)) {
                var list2 = item;
                list2.forEach(function (a) { return a && _this.addComponent(a, parent, containerId, action, components); });
            }
            else if (item instanceof Luxena.SemanticObject) {
                this.addComponent(item, parent, containerId, action, components);
            }
            else {
                var items2 = item;
                for (var containerId2 in items2) {
                    if (!items2.hasOwnProperty(containerId2))
                        continue;
                    this.addComponents(items2[containerId2], parent, containerId2, action, components);
                }
            }
            return components;
        };
        SemanticController.prototype.addDetails = function (details) {
            this.details.push(details);
        };
        SemanticController.prototype.getFieldByMember = function (sm) {
            return this.fields.filter(function (a) { return a._member === sm; })[0];
        };
        SemanticController.prototype.modelValue = function (sm, value) {
            var field = this.getFieldByMember(sm);
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
                var sm = sf._member;
                if (!sf._visible && !sf._selectRequired && !sm._selectRequired)
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
            return {
                store: cfg.entity._store,
                select: select,
                expand: expand,
            };
        };
        //getEntityActionParams()
        //{
        //	return <EntityActionParams>{
        //		id: this.getId(),
        //		_controller: this,
        //		_entity: this.config.entity,
        //	};
        //	//const ectrl = ctrl instanceof EditFormController ? ctrl : null;
        //	//const prms: EntityActionParams = {
        //	//	id: ctrl.getId(),
        //	//	_controller: ctrl,
        //	//	_resync: ctrl.editMode || ctrl.viewMode,
        //	//	_save: ctrl.viewMode,
        //	//	_select: <any>ctrl.dataSourceConfig.select,
        //	//	_delta: !ectrl ? undefined : () => ectrl.saveToData(),
        //	//};
        //	//if (ctrl.viewMode)
        //	//	prms._expand = <any>ctrl.dataSourceConfig.expand;
        //	//if (prms._resync)
        //	//	prms._onExecuteDone = data => ctrl.loadFromData(data);
        //}
        SemanticController.prototype.getScope = function () {
            var _this = this;
            var se = this._entity;
            this.iconHtml(se._textIconHtml);
            this.addMembers();
            this.createContainers();
            return this.scope = {
                controller: this,
                containers: this.containers,
                widgets: this.widgets,
                viewShown: function () { return _this.viewShown(); },
                viewHidden: function () { return _this.viewHidden(); },
            };
        };
        SemanticController.prototype.addMembers = function () {
            this.addComponents(this.getToolbarItems(), null, "toolbarItems");
            var toolbarItems = this.components.filter(function (sc) { return sc._containerId === "toolbarItems"; });
            this.addComponent(Luxena.sd.toolbar(toolbarItems), null, "toolbar");
        };
        SemanticController.prototype.createContainers = function () {
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
        SemanticController.prototype.getToolbarItems = function () {
            var _this = this;
            return [
                new Luxena.Components.Button().back().left(),
                new Luxena.Components.Button().template(function (data, index, container) {
                    var h = $("<h1>" + ko.unwrap(_this.iconHtml) + "<b data-bind=\"text: title\"></b></h1>").appendTo(container);
                    ko.applyBindings({ title: _this.title }, h[0]);
                })
            ];
        };
        SemanticController.prototype.viewShown = function () { };
        SemanticController.prototype.viewHidden = function () { };
        SemanticController.prototype.refresh = function () { };
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
    function openEntityUri(action, id, actionOptions) {
        if (!action || !action.uri)
            return;
        var uri = $.extend({ id: id }, action.uri);
        if (!id && action.defaults)
            uri = $.extend(uri, action.defaults);
        Luxena.smartVisible(false);
        Luxena.app.navigate(uri, actionOptions || action.options);
    }
    Luxena.openEntityUri = openEntityUri;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var CollectionController = (function (_super) {
        __extends(CollectionController, _super);
        function CollectionController(cfg) {
            this.chartMode = true;
            if (!cfg.members)
                cfg.members = cfg.entity._members;
            if (cfg.defaults && cfg.defaults.length) {
                var defaults = {};
                cfg.defaults.forEach(function (a) {
                    var sm = a[0];
                    var model = {};
                    model[sm._name] = a[1];
                    sm.field().saveToData(model, defaults);
                });
                this.defaults = defaults;
            }
            _super.call(this, cfg);
            if (cfg.master)
                cfg.master.addDetails(this);
        }
        CollectionController.prototype.getScope = function () {
            var se = this._entity;
            this.title(se._titles || se._title || se._names || se._name);
            return _super.prototype.getScope.call(this);
        };
        CollectionController.prototype.masterId = function () {
            var master = this.config.master;
            return master && master.getId();
        };
        CollectionController.prototype.prepareFilter = function (filter) {
            if (!filter)
                return filter;
            for (var i = 0, len = filter.length; i < len; i++) {
                var f = filter[i];
                if (f instanceof Luxena.SemanticMember)
                    filter[i] = f.getFilterExpr(null)[0];
                else if ($.isArray(f))
                    this.prepareFilter(f);
            }
            return filter;
        };
        CollectionController.prototype.getDataSourceConfig = function () {
            var _this = this;
            var options = _super.prototype.getDataSourceConfig.call(this);
            var cfg = this.config;
            var se = this._entity;
            if (cfg.filter) {
                var filter = cfg.filter;
                if (!ko.isObservable(filter)) {
                    if ($.isFunction(filter))
                        filter = filter(this);
                    filter = this.prepareFilter(filter);
                }
                options.filter = filter;
            }
            else if (cfg.defaults) {
                if (se._isDomainFunction) {
                    var params = options["customQueryParams"] = {};
                    cfg.defaults.forEach(function (def) {
                        return params[def[0].getFilterExpr(null)[0]] = def[1];
                    });
                }
                else {
                    var filter = cfg.defaults.map(function (a) { return a[0].getFilterExpr(a[1]); });
                    if (filter.length)
                        options.filter = filter;
                }
            }
            else if (cfg.master instanceof Luxena.FilterFormController) {
                var ofilter = cfg.master.filter;
                options.filter = ko.unwrap(ofilter);
            }
            options.map = function (data) { return _this.dataMap(data); };
            if (cfg.entity._isQueryResult || cfg.fixed) {
                delete options.expand;
                delete options.select;
            }
            return options;
        };
        CollectionController.prototype.dataMap = function (data) {
            var cfg = this.config;
            var se = cfg.entity._isAbstract && Luxena.sd.entityByOData(data) || cfg.entity;
            data._viewEntity = cfg.view === cfg.entity ? se : cfg.view;
            data._smartEntity = cfg.smart === cfg.entity ? se : cfg.smart;
            data._editEntity = cfg.edit === cfg.entity ? se : cfg.edit;
            return data;
        };
        return CollectionController;
    })(Luxena.SemanticController);
    Luxena.CollectionController = CollectionController;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    (function (ChartColorMode) {
        ChartColorMode[ChartColorMode["Default"] = 0] = "Default";
        ChartColorMode[ChartColorMode["NegativePositive"] = 1] = "NegativePositive";
    })(Luxena.ChartColorMode || (Luxena.ChartColorMode = {}));
    var ChartColorMode = Luxena.ChartColorMode;
    var ChartController = (function (_super) {
        __extends(ChartController, _super);
        function ChartController(cfg) {
            this.chartMode = true;
            if (!cfg.members) {
                cfg.members = [];
                if (cfg.argument)
                    cfg.members.push(cfg.argument);
                if (cfg.value)
                    cfg.members.push(cfg.value);
            }
            _super.call(this, cfg);
            this.addComponents(cfg.members, null, "columns");
        }
        ChartController.prototype.getId = function () { Error("Not Implement"); };
        ChartController.prototype.getScope = function () {
            var _this = this;
            return $do(_super.prototype.getScope.call(this), function (a) {
                return a.chartOptions = _this.getChartOptions();
            });
        };
        ChartController.prototype.dataMap = function (data) {
            _super.prototype.dataMap.call(this, data);
            data["$data"] = data;
            this._argumentDataMap && this._argumentDataMap(data);
            this._valueDataMap && this._valueDataMap(data);
            return data;
        };
        ChartController.prototype.getChartOptions = function () {
            var _this = this;
            var cfg = this.config;
            var arg = cfg.argument;
            var values = (cfg.value instanceof Luxena.SemanticMember ? [cfg.value] : cfg.value);
            if (!values.length)
                values = null;
            if (!this.fields.length) {
                if (arg)
                    this.fields.push(arg.field());
                if (values && values.length)
                    (_a = this.fields).push.apply(_a, values.map(function (a) { return a.field(); }));
            }
            var options = $.extend({
                dataSource: this.getDataSourceConfig(),
                loadingIndicator: { show: true },
            }, cfg.chartOptions);
            if (cfg.height) {
                if (!options.size)
                    options.size = {};
                options.size.height = cfg.height;
            }
            if (cfg.zoom) {
                options.zoomingMode = "all";
                options.scrollingMode = "all";
                options.scrollBar = $.extend(options.scrollBar, {
                    visible: true,
                    width: 3,
                    position: "bottom",
                    opacity: 0.5,
                });
            }
            var commonSeries = options.commonSeriesSettings;
            if (!commonSeries)
                commonSeries = options.commonSeriesSettings = {};
            if (!commonSeries.type && cfg.type)
                commonSeries.type = cfg.type;
            //#region argument
            if (arg) {
                var argName = arg._name;
                var ref = arg.getLookupEntity();
                var argAxis = options.argumentAxis;
                if (!argAxis)
                    options.argumentAxis = argAxis = {};
                if (!commonSeries.argumentField) {
                    commonSeries.argumentField = argName;
                    if (ref)
                        commonSeries.argumentField += "_" + ref._lookupFields.name;
                    if (arg._type.chartDataType) {
                        if (!options.argumentAxis)
                            options.argumentAxis = {};
                        options.argumentAxis.argumentType = arg._type.chartDataType;
                    }
                }
                //if (!ref)
                //	argAxis.grid = $.extend(argAxis.grid, { visible: true });
                if (ref) {
                    var nameName = ref._lookupFields.name;
                    var idName = ref._lookupFields.id;
                    this._argumentDataMap = function (data) {
                        var argData = data[argName];
                        if (!argData)
                            return;
                        data[argName + "_" + idName] = argData[idName];
                        data[argName + "_" + nameName] = argData[nameName];
                    };
                    if (options.rotated === undefined) {
                        options.rotated = true;
                        argAxis.inverted = true;
                    }
                    if (!commonSeries.type)
                        commonSeries.type = "bar";
                    if (!commonSeries.tagField)
                        commonSeries.tagField = "$data"; //argName + "_" + idName;
                    if (!options.onPointClick)
                        options.onPointClick = function (e) {
                            var data = e.target.tag;
                            var argData = data[argName];
                            var id = argData[idName];
                            if (!id)
                                return;
                            e.target.hideTooltip();
                            var ref2 = ref._isAbstract && Luxena.sd.entityByOData(argData, ref) || ref;
                            ref2.toggleSmart(e.target.graphic.element, {
                                id: id,
                                view: ref2,
                                edit: ref2,
                            });
                        };
                }
            }
            //#endregion
            //#region value
            if (!options.series)
                options.series = [];
            var panes = {};
            if ($.isArray(options.panes)) {
                options.panes.forEach(function (a) {
                    return panes[a.name] = a;
                });
            }
            values && values.forEach(function (val, valueIndex) {
                var varName = val._name;
                var format = val._format; // + (val._precision || "");
                var series = options.series[valueIndex];
                if (!series)
                    series = options.series[valueIndex] = {};
                if (!series.name)
                    series.name = val._title || val._name;
                if (!series.valueField) {
                    series.valueField = varName;
                    if (val._type instanceof Luxena.FieldTypes.Money) {
                        series.valueField += "_Amount";
                        _this._valueDataMap = function (r) { return r[varName + "_Amount"] = r[varName].Amount; };
                        format = "n";
                    }
                }
                if (!options.legend)
                    options.legend = { visible: false };
                var pane = panes[series.pane];
                if (series.pane && !pane) {
                    if (!options.panes)
                        options.panes = [];
                    options.panes.push(panes[series.pane] = pane = { name: series.pane });
                }
                if (!options.valueAxis)
                    options.valueAxis = [];
                var valueAxis;
                if (pane) {
                    valueAxis = options.valueAxis.filter(function (a) { return a.pane === pane.name; })[0];
                    if (!valueAxis)
                        options.valueAxis.push(valueAxis = { pane: pane.name });
                }
                else {
                    valueAxis = options.valueAxis[0];
                    if (!valueAxis)
                        options.valueAxis.push(valueAxis = {});
                }
                if (!valueAxis.title)
                    valueAxis.title = { text: val._title };
                if (val._type.chartDataType)
                    valueAxis.valueType = val._type.chartDataType;
                if (cfg.colorMode === ChartColorMode.NegativePositive && valueAxis.valueType === "numeric")
                    options.customizePoint = function () {
                        // ReSharper disable once SuspiciousThisUsage
                        var value = this.value;
                        if (value > 0)
                            return { color: '#859666', hoverStyle: { color: '#859666' } };
                        else
                            return { color: '#BA4D51', hoverStyle: { color: '#BA4D51' } };
                    };
                if (format) {
                    var label = valueAxis.label;
                    if (!label)
                        valueAxis.label = label = {};
                    if (format === "n" && !series.point)
                        series.point = { visible: false };
                    if (!label.format)
                        label.format = format === "n" ? "n0" : format;
                }
            });
            //#endregion
            if (arg && cfg.tooltip !== false) {
                if (!options.tooltip)
                    options.tooltip = { enabled: true };
                if (values && values.length) {
                    var val = values[0];
                    options.tooltip.customizeTooltip = function (args) {
                        var argument = args.argument;
                        var argFormat = arg._format || arg._type.format;
                        if (argFormat)
                            argument = Globalize.format(argument, argFormat);
                        var value = args.point.value;
                        if (val._format)
                            value = Globalize.format(value, val._format);
                        var html = [
                            ("<table><tr><td>" + arg._title + ": &nbsp;</td><td><b>" + argument + "</b></td></tr>"),
                            ("<tr><td>" + args.seriesName + ": &nbsp;</td><td><b>" + value + "</b></td></tr>"),
                            "</table>",
                        ];
                        return { html: html.join(""), };
                    };
                }
            }
            return options;
            var _a;
        };
        return ChartController;
    })(Luxena.CollectionController);
    Luxena.ChartController = ChartController;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var GridController = (function (_super) {
        __extends(GridController, _super);
        function GridController(cfg) {
            this.gridMode = true;
            if (cfg.inline) {
                //cfg.useExport = cfg.useExport === true;
                cfg.useFilterRow = cfg.useFilterRow === true;
                cfg.useGrouping = cfg.useGrouping === true;
                cfg.useSearch = cfg.useSearch === true;
                cfg.columnsIsStatic = cfg.columnsIsStatic !== false;
                cfg.usePager = cfg.usePager !== false;
            }
            if (cfg.listMode) {
                cfg.useExport = cfg.useExport === true;
                cfg.useHeader = cfg.useHeader === true;
                cfg.useFilterRow = cfg.useFilterRow === true;
                cfg.useGrouping = cfg.useGrouping === true;
                cfg.useSearch = cfg.useSearch === true;
                cfg.columnsIsStatic = cfg.columnsIsStatic !== false;
                if (!cfg.usePager && cfg.height === undefined)
                    cfg.height = 300;
            }
            if (cfg.fixed) {
                cfg.useFilter = cfg.useFilter === true;
                cfg.useSearch = cfg.useSearch === true;
                cfg.usePager = cfg.usePager !== false;
            }
            if (cfg.small || cfg.entity._isSmall) {
                cfg.useFilterRow = cfg.useFilterRow === true;
                cfg.useGrouping = cfg.useGrouping === true;
            }
            // ReSharper disable RedundantComparisonWithBoolean
            cfg.inline = cfg.inline === true;
            cfg.fixed = cfg.fixed === true;
            cfg.useExport = cfg.useExport !== false;
            cfg.useHeader = cfg.useHeader !== false;
            cfg.useFilter = cfg.useFilter !== false;
            cfg.useFilterRow = cfg.useFilterRow !== false;
            cfg.useGrouping = cfg.useGrouping !== false;
            cfg.usePager = cfg.usePager === true;
            cfg.usePaging = cfg.usePaging !== false || cfg.usePager;
            cfg.useSearch = cfg.useSearch !== false;
            cfg.useSorting = cfg.useSorting !== false;
            cfg.columnsIsStatic = cfg.columnsIsStatic === true;
            // ReSharper restore RedundantComparisonWithBoolean
            _super.call(this, cfg);
            this.selectedRowKeys = ko.observableArray([]);
            this.createMembers();
        }
        GridController.prototype.getScope = function () {
            var _this = this;
            return $do(_super.prototype.getScope.call(this), function (a) {
                return a.gridOptions = _this.getGridOptions();
            });
        };
        GridController.prototype.createMembers = function () {
            var cfg = this.config;
            var se = cfg.entity;
            var members = cfg.members;
            this.addComponents(members, null, "columns", function (sm, sc) { return sc._visible = sc._columnVisible && sm._columnVisible; });
            if (se instanceof Luxena.EntitySemantic) {
                this.addComponent(se.Id, null);
                if (se instanceof Luxena.Entity2Semantic) {
                    this.addComponents([
                        se.CreatedOn,
                        se.CreatedBy,
                        se.ModifiedOn,
                        se.ModifiedBy,
                    ], null, "columns");
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
                return data[this.config.entity._lookupFields.id];
            var keys = this.selectedRowKeys();
            return keys[0];
        };
        GridController.prototype.getGridOptions = function () {
            var _this = this;
            var cfg = this.config;
            var se = this._entity;
            var options = $.extend({
                dataSource: this.getDataSource(),
                allowColumnReordering: !cfg.columnsIsStatic,
                allowColumnResizing: true,
                allowGrouping: cfg.useGrouping,
                columnAutoWidth: true,
                columnChooser: { enabled: !cfg.columnsIsStatic },
                hoverStateEnabled: true,
                //rowAlternationEnabled: true,
                showRowLines: true,
                showColumnLines: false,
                wordWrapEnabled: true,
                height: cfg.height,
                groupPanel: {
                    visible: cfg.useGrouping,
                },
                filterRow: {
                    visible: cfg.useFilter && cfg.useFilterRow,
                },
                searchPanel: {
                    visible: cfg.useFilter && cfg.useSearch,
                    width: 240,
                },
                paging: {
                    enabled: cfg.usePaging,
                    pageSize: cfg.usePager ? 10 : 30,
                },
                scrolling: {
                    mode: cfg.usePager || cfg.inline || cfg.fixed ? "standard" : "virtual",
                },
                selection: {
                    mode: "single",
                },
                showColumnHeaders: cfg.useHeader,
                sorting: { mode: cfg.useSorting ? "multiple" : "none" },
                "export": {
                    enabled: cfg.useExport,
                    fileName: cfg.entity._names,
                    //allowExportSelectedData: true,
                    excelFilterEnabled: true,
                    excelWrapTextEnabled: true,
                },
                onInitialized: function (e) {
                    _this.grid = e.component;
                    //$log(e.element[0].outerHTML);
                    //this.appendButtons(e.element.find(".dx-datagrid-header-panel"));
                },
                onSelectionChanged: function (e) {
                    _this.selectedRowKeys(e.selectedRowKeys);
                },
                onRowClick: function (e) {
                    if (e.rowType !== "data")
                        return;
                    if (_this.config.master && _this.config.master.smartMode)
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
                    //if (!this._pagerIsRepainted)
                    //{
                    //	this._pagerIsRepainted = true;
                    //	const accordionId = this.scope.accordionId;
                    //	if (accordionId)
                    //	{
                    //		const accordion = $("#" + accordionId).dxAccordion("instance");
                    //		const selectedItems = <any>accordion.option("selectedItems");
                    //		accordion.option("selectedItems", []);
                    //		accordion.option("selectedItems", selectedItems);
                    //	}
                    //	if (this.config.fullHeight)
                    //	{
                    //		this.grid.option("height", "100%");
                    //	}
                    //}
                    if (cfg.onTotalCountChange)
                        cfg.onTotalCountChange(_this, e.component.totalCount());
                    _this.appendButtons(e.element);
                    _this.selectByLastId();
                },
            }, cfg.gridOptions);
            var entityStatus = cfg.entityStatus || se._entityStatusGetter;
            if (entityStatus)
                options.onCellPrepared = function (e) {
                    if (e.rowType !== "data")
                        return;
                    var rowClass = entityStatus(e.data);
                    if (!rowClass)
                        return;
                    if (rowClass === "error")
                        rowClass = "cell-state-error";
                    else if (rowClass === "success")
                        rowClass = "cell-state-success";
                    else if (rowClass === "warning")
                        rowClass = "cell-state-warning";
                    else if (rowClass === "disabled")
                        rowClass = "cell-state-disabled";
                    e.cellElement.addClass(rowClass);
                };
            this.appendComponentsToGridOptions(options);
            return options;
        };
        GridController.prototype.appendComponentsToGridOptions = function (options) {
            var cfg = this.config;
            var columns = [];
            var totalItems = [], groupItems = [];
            var entityPositions = [];
            var entityNames = [];
            var entityDates = [];
            this.components.forEach(function (sc) {
                if (Luxena.isField(sc)) {
                    var sm = sc._member;
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
            this.components.forEach(function (sc2) {
                var cols = sc2.toGridColumns();
                if (sc2._parent)
                    cols.forEach(function (c) { return c.visible = false; });
                if (cols.length && Luxena.isField(sc2)) {
                    if (sc2._member._isEntityName)
                        cols[0].width = 0;
                    if (cfg.useGrouping === false)
                        delete cols[0].groupIndex;
                }
                columns.push.apply(columns, cols);
                var items = sc2.toGridTotalItems();
                items.forEach(function (a) {
                    totalItems.push(a);
                    groupItems.push($.extend({
                        showInGroupFooter: true,
                        alignByColumn: true,
                    }, a));
                });
            });
            //$log(columns);
            options.columns = columns;
            if (totalItems.length)
                options.summary = {
                    groupItems: groupItems,
                    totalItems: totalItems,
                };
        };
        GridController.prototype.refresh = function () {
            this.grid && this.grid.refresh();
        };
        GridController.prototype.getDataSource = function () {
            var _this = this;
            var options = this.getDataSourceConfig();
            var cfg = this.config;
            var ds = new DevExpress.data.DataSource(options);
            if (cfg.master instanceof Luxena.FilterFormController) {
                var ofilter = cfg.master.filter;
                ofilter.subscribe(function (newFilter) {
                    ds.filter(newFilter);
                    _this.grid && _this.grid.refresh();
                });
            }
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
        GridController.prototype.appendButtons = function (container) {
            var cfg = this.config;
            if (!cfg.master)
                return;
            var header = container.find(".dx-datagrid-header-panel");
            if (container.find("#app-buttons").length)
                return;
            var btns = $("<div id=\"app-buttons\" class=\"pull-right\">");
            $("<div>").appendTo(btns).dxButton(this.addComponent(this._entity.refreshAction.button().small(), null, "buttons").buttonOptions());
            if (cfg.edit) {
                var newBtn = cfg.edit.newAction.button();
                if (this.config.master && this.config.master.smartMode)
                    newBtn.small();
                $("<div>").appendTo(btns).dxButton(this.addComponent(newBtn, null, "buttons").buttonOptions());
            }
            header.append(btns);
        };
        GridController.prototype.getToolbarItems = function () {
            var cfg = this.config;
            var items = _super.prototype.getToolbarItems.call(this);
            if (!cfg.master) {
                if (cfg.edit)
                    items.push(cfg.edit.newAction.button().right());
                items.push(this._entity.refreshAction.button().small().right());
            }
            return items;
        };
        return GridController;
    })(Luxena.CollectionController);
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
            if (!cfg.members)
                cfg.members = cfg.entity._members;
            this.params = cfg.args && cfg.args[0] || { id: cfg.id };
            this.id = this.params.id || null;
            //this.viewInfo = cfg.args[1];
            this.modelIsReady = $.Deferred();
        }
        FormController.prototype.getId = function () { return this.id; };
        FormController.prototype.getScope = function () {
            var se = this._entity;
            this.title(se._title || se._name);
            this.scope = $.extend(_super.prototype.getScope.call(this), {
                r: this.model,
                deferRenderingOptions: {
                    renderWhen: this.modelIsReady.promise(),
                    showLoadIndicator: true,
                    animation: "stagger-3d-drop",
                    staggerItemSelector: ".dx-field",
                },
                loadingOptions: {
                    delay: 500,
                    message: this.loadingMessage,
                    visible: this.modelIsLoading,
                },
            });
            return this.scope;
        };
        //protected addMembers()
        //{
        //	this.addComponents(this.config.members, null, "fields");
        //}
        FormController.prototype.addMembers = function () {
            this.addComponents(this.config.members, null, "fields");
            _super.prototype.addMembers.call(this);
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
        FormController.prototype.loadData = function (onLoaded) {
            var _this = this;
            var cfg = this.config;
            //this.loadingMessage("Загрузка...");
            //this.modelIsLoading(true);
            var dsConfig = this.dataSourceConfig = this.getDataSourceConfig();
            dsConfig.filter = [cfg.entity._lookupFields.id, "=", this.getId()];
            //$log(dsConfig);
            var ds = new DevExpress.data.DataSource(dsConfig);
            ds.load()
                .done(function (data) { _this.loadFromData(data[0], true); onLoaded && onLoaded(); })
                .fail(function () { return _this.loadFromData({}, true); });
        };
        FormController.prototype.refresh = function () {
            this.loadData();
            this.details.forEach(function (a) { return a.refresh(); });
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
        ViewFormController.prototype.getToolbarItems = function () {
            var cfg = this.config;
            var se = this._entity;
            var items = _super.prototype.getToolbarItems.call(this);
            if (cfg.edit)
                items.push(cfg.edit.editAction.button().right());
            items.push(se.refreshAction.button().small().right());
            var items2 = [];
            cfg.edit && items2.push(cfg.edit.deleteAction.button().right().onExecuteDone(function () { return Luxena.app.back(); }));
            cfg.list && items2.push(cfg.list.backToListAction);
            items.push(Luxena.sd.button.apply(Luxena.sd, items2).right());
            return items;
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
    Luxena.smartVisible = ko.observable(false);
    Luxena.smartWidth = ko.observable();
    //export var smartButtons = ko.observableArray();
    function getSmartPopoverOptions() {
        return {
            target: Luxena.smartTarget,
            title: Luxena.smartTitle,
            visible: Luxena.smartVisible,
            //buttons: <any>smartButtons,
            position: "bottom",
            width: Luxena.smartWidth,
            //showTitle: true,
            showCloseButton: true,
            closeOnBackButton: true,
        };
    }
    Luxena.getSmartPopoverOptions = getSmartPopoverOptions;
    var SmartFormController = (function (_super) {
        __extends(SmartFormController, _super);
        function SmartFormController() {
            _super.apply(this, arguments);
            this.smartMode = true;
        }
        SmartFormController.prototype.getToolbarItems = function () {
            var cfg = this.config;
            var se = this._entity;
            return [
                cfg.view && cfg.view.viewAction.button(),
                cfg.edit && cfg.edit.editAction.button().small().right(),
                Luxena.sd.button(cfg.edit && cfg.edit.deleteAction, se.refreshAction).right(),
            ];
        };
        //getBottomToolbarItems()
        //{
        //	const cfg = this.config;
        //	const se = this._entity;
        //	const buttons = [
        //		cfg.view && cfg.view.viewAction.button().right(),
        //		cfg.edit && cfg.edit.editAction.button().small().right(),
        //		sd.button(
        //			cfg.edit && cfg.edit.deleteAction.button().small(),
        //			se.refreshAction.button().small()
        //		).right(),
        //	].map(btn => $.extend(btn.toolbarItemOptions(), { toolbar: "bottom" }));
        //	$log(buttons);
        //	return buttons;
        //}
        SmartFormController.prototype.show = function (target) {
            var _this = this;
            var cfg = this.config;
            Luxena.smartVisible(false);
            Luxena.smartTarget(target);
            var scope = this.getScope();
            this.loadData(function () {
                Luxena.smartWidth(cfg.contentWidth ? cfg.contentWidth + 40 : 500);
                Luxena.smartTitle(/*this.iconHtml() +*/ _this.title());
                //smartButtons(this.getBottomToolbarItems());
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
        EditFormController.prototype.getToolbarItems = function () {
            var _this = this;
            var se = this._entity;
            var items = _super.prototype.getToolbarItems.call(this);
            items.push(Luxena.sd.button().icon("save").title("Сохранить").success().right()
                .onExecute(function () { return _this.save(); }));
            items.push(Luxena.sd.button().right().items(Luxena.sd.button().icon("check-square-o").title("проверить").onExecute(function () { _this.validate(); }), se.refreshAction, Luxena.sd.button().title("$log(model)").onExecute(function () { return $log_(ko.unwrap3(_this.model)); }), Luxena.sd.button().title("$log(scope)").onExecute(function () { return $log_(ko.unwrap4(_this.scope)); })));
            return items;
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
                for (var name_3 in prms) {
                    if (!prms.hasOwnProperty(name_3))
                        continue;
                    var prm = prms[name_3];
                    if (prm === null || prm == undefined)
                        delete prms[name_3];
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
                if (!sf._member._isNonsaved)
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
                .done(function () { return Luxena.app.back(); })
                .always(function () { return _this.modelIsLoading(false); });
        };
        EditFormController.prototype.recalc = function (propertyName) {
            var _this = this;
            if (this.isRecalculating)
                return;
            this.isRecalculating = true;
            this.loadingMessage("Идёт пересчёт...");
            this.modelIsLoading(true);
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
                .always(function () {
                _this.isRecalculating = false;
                _this.modelIsLoading(false);
            });
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
                scope.viewToolbarItems = gridScope.viewToolbarItems;
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
                    Views[se.listView()] = function () {
                        if (listMembers instanceof Luxena.SemanticEntity) {
                            var params = listMembers.resolveListAction().uri;
                            return {
                                viewShowing: function () { return Luxena.app.navigate(params, { target: "current" }); }
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
                if ($.isArray(viewMembers)) {
                    // ReSharper disable once TypeGuardDoesntAffectAnything
                    if (se instanceof Luxena.Entity2Semantic)
                        viewMembers = Luxena.sd.tabPanel().card().items((_a = Luxena.sd.col().icon(se)).items.apply(_a, viewMembers), se.HistoryTab);
                    else
                        viewMembers = (_b = Luxena.sd.card()).items.apply(_b, viewMembers);
                }
                if (viewMembers || se._isAbstract) {
                    Views[se.viewView()] = function () {
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
                    if ($.isArray(smartMembers)) {
                        // ReSharper disable once TypeGuardDoesntAffectAnything
                        if (se instanceof Luxena.Entity2Semantic)
                            smartMembers = Luxena.sd.tabPanel((_c = Luxena.sd.col().icon(se)).items.apply(_c, smartMembers), se.HistoryTab);
                    }
                    se.showSmart = function (target, smartCfg) {
                        smartCfg.entity = se;
                        smartCfg.entityTitle = cfg.smartTitle || cfg.viewTitle || cfg.formTitle || cfg.entityTitle;
                        smartCfg.members = smartMembers;
                        smartCfg.view = smartCfg.view || viewEntity;
                        smartCfg.edit = smartCfg.edit || editEntity;
                        //smartCfg.actions = cfg.actions;
                        $.extend(smartCfg, cfg.smartConfig);
                        var ctrl = new Luxena.SmartFormController(smartCfg);
                        ctrl.show(target);
                    };
                }
                var editMembers = cfg.edit || (cfg.edit === null ? null : cfg.form || cfg.members);
                if ($.isArray(editMembers))
                    editMembers = Luxena.sd.card.apply(Luxena.sd, editMembers);
                if (editMembers || se._isAbstract) {
                    Views[se.editView()] = function () {
                        var args = [];
                        for (var _i = 0; _i < arguments.length; _i++) {
                            args[_i - 0] = arguments[_i];
                        }
                        if (editMembers instanceof Luxena.SemanticEntity) {
                            var params = $.extend(args[0] || {}, viewMembers.resolveEditUri().uri);
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
                var _a, _b, _c;
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
            var sm = sf._member;
            var id = sf.controller.getId();
            var filter = id
                ? [[sm._name, '=', params.value], [sm._entity._lookupFields.id, '<>', id]]
                : [sm._name, '=', params.value];
            se._store.createQuery({})
                .filter(filter)
                .select(sm._entity._lookupFields.id)
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
            //return fieldRow2(se, 
            //	(() =>
            //	{
            //		var displayTarget = ko.observable();
            //		var displayValue = ko.observable();
            //		var displayMinValue = ko.observable();
            //		var displayMaxValue = ko.observable();
            //		var displayColor = ko.observable();
            //		var displayCurrency = ko.observable();
            //		var displaySubvalues = ko.observable();
            //		return <ISemanticFieldComponentConfig2<SemanticEntity>>{
            //			title: valueMember,
            //			members: () => [targetMember, valueMember.clone(<any>{ _selectRequired: true }), ],
            //			width: 150,
            //			cellTemplate: (sc, cell, cellInfo) =>
            //			{
            //				var r = cellInfo.data;
            //				var targetData = r[targetMember._name];
            //				var valueData = r[valueMember._name];
            //				var target: number = targetData && targetData.Amount;
            //				var value: number = valueData && valueData.Amount;
            //				var color = Math.abs(value - target) <= 0.01 ? "#82e583" : value >= target ? "#e55253" : undefined;
            //				cell
            //					.addClass("bullet")
            //					.dxBullet(<DevExpress.viz.sparklines.dxBulletOptions><any>
            //					{
            //						startScaleValue: Math.min(0, target, value),
            //						endScaleValue: Math.max(0, target, value),
            //						value: value,
            //						target: target,
            //						color: color,
            //						size: {
            //							width: 140,
            //							height: 25
            //						},
            //						tooltip:
            //						{
            //							enabled: false,
            //						}
            //					});
            //			},
            //			loadFromData: (sc, model, data) =>
            //			{
            //				var targetData = data[targetMember._name];
            //				var valueData = data[valueMember._name];
            //				var target: number = targetData && targetData.Amount;
            //				var value: number = valueData && valueData.Amount;
            //				var color = Math.abs(value - target) <= 0.01 ? "#82e583" : value >= target ? "#e55253" : "#ebdd8f";
            //				//$alert(target);
            //				displayTarget(target);
            //				displayValue(value);
            //				displayMinValue(Math.min(0, target, value));
            //				displayMaxValue(Math.max(0, target, value));
            //				displayColor(color);
            //				displayCurrency(targetData && targetData.CurrencyId || "");
            //				displaySubvalues([target]);
            //			},
            //			renderDisplay: (sc, container) =>
            //			{
            //				var model = sc._controller.model;
            //				var name = "moneyProgress" + (moneyProgressCount++);
            //				model[name] = <DevExpress.viz.gauges.dxCircularGaugeOptions><any>
            //				{
            //					//geometry: { startAngle: 180, endAngle: 0 },
            //					scale: {
            //						startValue: displayMinValue,
            //						endValue: displayMaxValue,
            //						label: {
            //							format: "n0",
            //							//customizeText: arg => (arg.valueText + " " + displayCurrency())
            //						},
            //					},
            //					valueIndicator: {
            //						//type: 'rangebar',
            //						baseValue: 0
            //					},
            //					subvalueIndicator: {
            //						type: "textcloud",
            //						text: {
            //							format: "n0",
            //							customizeText: arg => (arg.valueText + " " + displayCurrency()),
            //						}
            //					},
            //					rangeContainer: {
            //						backgroundColor: displayColor,
            //					},
            //					value: displayValue,
            //					subvalues: displaySubvalues,
            //				};
            //				$("<div>")
            //					.height(200)
            //					.attr("data-bind", "dxCircularGauge: r." + name)
            //					.appendTo(container);
            //			},
            //			//renderDisplay1: (sc, container) =>
            //			//{
            //			//	var model = sc.controller.model;
            //			//	var name = "moneyProgress" + (moneyProgressCount++);
            //			//	model[name] = <DevExpress.viz.gauges.dxLinearGaugeOptions><any>
            //			//	{
            //			//		geometry: { orientation: 'vertical' },
            //			//		scale: {
            //			//			startValue: displayMinValue,
            //			//			endValue: displayMaxValue,
            //			//			label: {
            //			//				format: "n0",
            //			//				customizeText: arg => (arg.valueText + " " + displayCurrency())
            //			//			}
            //			//		},
            //			//		valueIndicator: {
            //			//			baseValue: 0,
            //			//			color: displayColor,
            //			//		},
            //			//		rangeContainer: {
            //			//			palette: 'pastel',
            //			//			//ranges: [
            //			//			//	{ startValue: 50, endValue: 90 },
            //			//			//	{ startValue: 90, endValue: 130 },
            //			//			//	{ startValue: 130, endValue: 150 },
            //			//			//]
            //			//		},
            //			//		title: {
            //			//			text: valueMember._title,
            //			//			//font: { size: 16 }
            //			//		},
            //			//		value: displayValue
            //			//	};
            //			//	$("<div>")
            //			//		//.height(200)
            //			//		.attr("data-bind", "dxLinearGauge: r." + name)
            //			//		.appendTo(container);
            //			//},
            //		}
            //	})()
            //);
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
    function toMenuItems(subitems, submenuTemplate) {
        if (!subitems)
            return null;
        var buttons = [];
        subitems.forEach(function (subitem) {
            if (!subitem)
                return;
            var btn;
            if (subitem instanceof Luxena.Components.Button)
                btn = subitem.buttonOptions();
            else if (subitem instanceof Luxena.SemanticEntityAction)
                btn = subitem.button().buttonOptions();
            else if (subitem instanceof Luxena.SemanticEntity)
                btn = subitem.listAction.button().buttonOptions();
            else
                btn = $.extend({}, subitem);
            if (submenuTemplate)
                btn.template = submenuTemplate;
            //btn.template = submenuTemplate || "item";
            buttons.push(btn);
        });
        return buttons;
    }
    Luxena.toMenuItems = toMenuItems;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    //#region Enums
    //#region TaxRate 
    (function (TaxRate) {
        TaxRate[TaxRate["Default"] = 0] = "Default";
        TaxRate[TaxRate["A"] = 1] = "A";
        TaxRate[TaxRate["B"] = 2] = "B";
        TaxRate[TaxRate["D"] = 5] = "D";
        TaxRate[TaxRate["None"] = -1] = "None";
    })(Luxena.TaxRate || (Luxena.TaxRate = {}));
    var TaxRate = Luxena.TaxRate;
    var TaxRate;
    (function (TaxRate) {
        TaxRate._ns = "Luxena.Travel.Domain";
        TaxRate._name = "TaxRate";
        TaxRate._fullName = "Luxena.Travel.Domain.TaxRate";
        TaxRate._getEdm = function (value) { return !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.TaxRate'" + value + "'"); };
        TaxRate._array = [
            { Id: "Default", Value: 0, Name: "По умолчанию", TextIconHtml: "", ru: "По умолчанию" },
            { Id: "A", Value: 1, Name: "А (с НДС)", TextIconHtml: "", ru: "А (с НДС)" },
            { Id: "B", Value: 2, Name: "Б (без НДС)", TextIconHtml: "", ru: "Б (без НДС)" },
            { Id: "D", Value: 5, Name: "Д (без НДС)", TextIconHtml: "", ru: "Д (без НДС)" },
            { Id: "None", Value: -1, Name: "не печатать", TextIconHtml: "", ru: "не печатать" },
        ];
        TaxRate._maxLength = 9;
        TaxRate._items = {
            "0": TaxRate._array[0],
            "Default": TaxRate._array[0],
            "1": TaxRate._array[1],
            "A": TaxRate._array[1],
            "2": TaxRate._array[2],
            "B": TaxRate._array[2],
            "5": TaxRate._array[3],
            "D": TaxRate._array[3],
            "-1": TaxRate._array[4],
            "None": TaxRate._array[4],
        };
    })(TaxRate = Luxena.TaxRate || (Luxena.TaxRate = {}));
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
            { Id: "Male", Value: 0, Name: "Мужской", Icon: "male", TextIconHtml: Luxena.getTextIconHtml("male"), ru: "Мужской" },
            { Id: "Female", Value: 1, Name: "Женский", Icon: "female", TextIconHtml: Luxena.getTextIconHtml("female"), ru: "Женский" },
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
            Employees: { key: "Id", keyType: "String" },
            EmployeeLookup: { key: "Id", keyType: "String" },
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
            OpeningBalances: { key: "Id", keyType: "String" },
            OpeningBalanceLookup: { key: "Id", keyType: "String" },
            Orders: { key: "Id", keyType: "String" },
            OrderLookup: { key: "Id", keyType: "String" },
            OrderBalances: { key: "Id", keyType: "String" },
            OrderByAssignedTo_TotalByIssueDate: { key: "Id", keyType: "String" },
            OrderByCustomer_TotalByIssueDate: { key: "Id", keyType: "String" },
            OrderByOwner_TotalByIssueDate: { key: "Id", keyType: "String" },
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
            ProductByBooker_TotalByIssueDate: { key: "Id", keyType: "String" },
            ProductByProvider_TotalByIssueDate: { key: "Id", keyType: "String" },
            ProductBySeller_TotalByIssueDate: { key: "Id", keyType: "String" },
            ProductByTicketer_TotalByIssueDate: { key: "Id", keyType: "String" },
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
    var Entity2Semantic = (function (_super) {
        __extends(Entity2Semantic, _super);
        function Entity2Semantic() {
            _super.call(this);
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
            var se = this;
            se.HistoryTab = se.member0().col().field().icon("history").items(se.CreatedBy, se.CreatedOn, se.ModifiedBy, se.ModifiedOn);
        }
        return Entity2Semantic;
    })(EntitySemantic);
    Luxena.Entity2Semantic = Entity2Semantic;
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
        return Entity3Semantic;
    })(Entity2Semantic);
    Luxena.Entity3Semantic = Entity3Semantic;
    var Entity3DSemantic = (function (_super) {
        __extends(Entity3DSemantic, _super);
        function Entity3DSemantic() {
            _super.apply(this, arguments);
            /** Описание */
            this.Description = this.member()
                .localizeTitle({ ru: "Описание" })
                .text(4);
        }
        return Entity3DSemantic;
    })(Entity3Semantic);
    Luxena.Entity3DSemantic = Entity3DSemantic;
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    /** Владелец документов */
    var DocumentOwnerSemantic = (function (_super) {
        __extends(DocumentOwnerSemantic, _super);
        function DocumentOwnerSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "" };
            this.small();
        }
        return DocumentOwnerSemantic;
    })(Luxena.EntitySemantic);
    Luxena.DocumentOwnerSemantic = DocumentOwnerSemantic;
    var FileSemantic = (function (_super) {
        __extends(FileSemantic, _super);
        function FileSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "" };
        }
        return FileSemantic;
    })(Luxena.EntitySemantic);
    Luxena.FileSemantic = FileSemantic;
    /** Инвойс */
    var InvoiceSemantic = (function (_super) {
        __extends(InvoiceSemantic, _super);
        function InvoiceSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Number" };
        }
        return InvoiceSemantic;
    })(Luxena.EntitySemantic);
    Luxena.InvoiceSemantic = InvoiceSemantic;
    /** Выпущенная накладная */
    var IssuedConsignmentSemantic = (function (_super) {
        __extends(IssuedConsignmentSemantic, _super);
        function IssuedConsignmentSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Number" };
        }
        return IssuedConsignmentSemantic;
    })(Luxena.EntitySemantic);
    Luxena.IssuedConsignmentSemantic = IssuedConsignmentSemantic;
    var SequenceSemantic = (function (_super) {
        __extends(SequenceSemantic, _super);
        function SequenceSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
        }
        return SequenceSemantic;
    })(Luxena.EntitySemantic);
    Luxena.SequenceSemantic = SequenceSemantic;
    /** Настройки системы */
    var SystemConfigurationSemantic = (function (_super) {
        __extends(SystemConfigurationSemantic, _super);
        function SystemConfigurationSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "" };
        }
        return SystemConfigurationSemantic;
    })(Luxena.EntitySemantic);
    Luxena.SystemConfigurationSemantic = SystemConfigurationSemantic;
    /** Сервис-класс авиакомпании */
    var AirlineServiceClassSemantic = (function (_super) {
        __extends(AirlineServiceClassSemantic, _super);
        function AirlineServiceClassSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Code" };
        }
        return AirlineServiceClassSemantic;
    })(Luxena.Entity2Semantic);
    Luxena.AirlineServiceClassSemantic = AirlineServiceClassSemantic;
    /** Накладная */
    var ConsignmentSemantic = (function (_super) {
        __extends(ConsignmentSemantic, _super);
        function ConsignmentSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Number" };
        }
        return ConsignmentSemantic;
    })(Luxena.Entity2Semantic);
    Luxena.ConsignmentSemantic = ConsignmentSemantic;
    /** Курс валюты */
    var CurrencyDailyRateSemantic = (function (_super) {
        __extends(CurrencyDailyRateSemantic, _super);
        function CurrencyDailyRateSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "" };
        }
        return CurrencyDailyRateSemantic;
    })(Luxena.Entity2Semantic);
    Luxena.CurrencyDailyRateSemantic = CurrencyDailyRateSemantic;
    /** Доступ к документам */
    var DocumentAccessSemantic = (function (_super) {
        __extends(DocumentAccessSemantic, _super);
        function DocumentAccessSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "" };
        }
        return DocumentAccessSemantic;
    })(Luxena.Entity2Semantic);
    Luxena.DocumentAccessSemantic = DocumentAccessSemantic;
    /** Полетный сегмент */
    var FlightSegmentSemantic = (function (_super) {
        __extends(FlightSegmentSemantic, _super);
        function FlightSegmentSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
        }
        return FlightSegmentSemantic;
    })(Luxena.Entity2Semantic);
    Luxena.FlightSegmentSemantic = FlightSegmentSemantic;
    /** Gds-агент */
    var GdsAgentSemantic = (function (_super) {
        __extends(GdsAgentSemantic, _super);
        function GdsAgentSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
        }
        return GdsAgentSemantic;
    })(Luxena.Entity2Semantic);
    Luxena.GdsAgentSemantic = GdsAgentSemantic;
    /** Внутренний перевод */
    var InternalTransferSemantic = (function (_super) {
        __extends(InternalTransferSemantic, _super);
        function InternalTransferSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Number" };
        }
        return InternalTransferSemantic;
    })(Luxena.Entity2Semantic);
    Luxena.InternalTransferSemantic = InternalTransferSemantic;
    /** Мильная карта */
    var MilesCardSemantic = (function (_super) {
        __extends(MilesCardSemantic, _super);
        function MilesCardSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Number" };
        }
        return MilesCardSemantic;
    })(Luxena.Entity2Semantic);
    Luxena.MilesCardSemantic = MilesCardSemantic;
    /** Начальный остаток */
    var OpeningBalanceSemantic = (function (_super) {
        __extends(OpeningBalanceSemantic, _super);
        function OpeningBalanceSemantic() {
            _super.call(this);
            this._OpeningBalance = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Начальный остаток", rus: "Начальные остатки" })
                .lookup(function () { return Luxena.sd.OpeningBalance; });
            /** Номер */
            this.Number = this.member()
                .localizeTitle({ ru: "Номер" })
                .string()
                .entityName();
            /** Дата */
            this.Date = this.member()
                .localizeTitle({ en: "Date", ru: "Дата" })
                .date()
                .required();
            /** Остаток */
            this.Balance = this.member()
                .localizeTitle({ ru: "Остаток" })
                .float()
                .required();
            /** Контрагент */
            this.Party = this.member()
                .localizeTitle({ ru: "Контрагент" })
                .lookup(function () { return Luxena.sd.Party; });
            this._isAbstract = false;
            this._name = "OpeningBalance";
            this._names = "OpeningBalances";
            this._isEntity = true;
            this._localizeTitle({ ru: "Начальный остаток", rus: "Начальные остатки" });
            this._getDerivedEntities = null;
            this._className = "OpeningBalance";
            this._getRootEntity = function () { return Luxena.sd.OpeningBalance; };
            this._store = Luxena.db.OpeningBalances;
            this._saveStore = Luxena.db.OpeningBalances;
            this._lookupStore = Luxena.db.OpeningBalanceLookup;
            this._lookupFields = { id: "Id", name: "Number" };
        }
        return OpeningBalanceSemantic;
    })(Luxena.Entity2Semantic);
    Luxena.OpeningBalanceSemantic = OpeningBalanceSemantic;
    /** Заказ */
    var OrderSemantic = (function (_super) {
        __extends(OrderSemantic, _super);
        function OrderSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Number" };
            this.big();
        }
        return OrderSemantic;
    })(Luxena.Entity2Semantic);
    Luxena.OrderSemantic = OrderSemantic;
    /** Чек */
    var OrderCheckSemantic = (function (_super) {
        __extends(OrderCheckSemantic, _super);
        function OrderCheckSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "CheckNumber" };
        }
        return OrderCheckSemantic;
    })(Luxena.Entity2Semantic);
    Luxena.OrderCheckSemantic = OrderCheckSemantic;
    /** Позиция заказа */
    var OrderItemSemantic = (function (_super) {
        __extends(OrderItemSemantic, _super);
        function OrderItemSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Text" };
        }
        return OrderItemSemantic;
    })(Luxena.Entity2Semantic);
    Luxena.OrderItemSemantic = OrderItemSemantic;
    /** Паспорт */
    var PassportSemantic = (function (_super) {
        __extends(PassportSemantic, _super);
        function PassportSemantic() {
            _super.call(this);
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
                .text(3);
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
            this._lookupFields = { id: "Id", name: "Number" };
        }
        return PassportSemantic;
    })(Luxena.Entity2Semantic);
    Luxena.PassportSemantic = PassportSemantic;
    /** Платёж */
    var PaymentSemantic = (function (_super) {
        __extends(PaymentSemantic, _super);
        function PaymentSemantic() {
            _super.call(this);
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
                .money();
            /** В т.ч. НДС */
            this.Vat = this.member()
                .localizeTitle({ ru: "В т.ч. НДС" })
                .money();
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
            this._lookupFields = { id: "Id", name: "Number" };
        }
        return PaymentSemantic;
    })(Luxena.Entity2Semantic);
    Luxena.PaymentSemantic = PaymentSemantic;
    /** Услуга */
    var ProductSemantic = (function (_super) {
        __extends(ProductSemantic, _super);
        function ProductSemantic() {
            _super.call(this);
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
                .string();
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
            /** Ставка НДС для услуги */
            this.TaxRateOfProduct = this.member()
                .localizeTitle({ ru: "Ставка НДС для услуги" })
                .enum(Luxena.TaxRate)
                .required();
            /** Ставка НДС для сбора */
            this.TaxRateOfServiceFee = this.member()
                .localizeTitle({ ru: "Ставка НДС для сбора" })
                .enum(Luxena.TaxRate)
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.big();
        }
        return ProductSemantic;
    })(Luxena.Entity2Semantic);
    Luxena.ProductSemantic = ProductSemantic;
    /** Пассажир */
    var ProductPassengerSemantic = (function (_super) {
        __extends(ProductPassengerSemantic, _super);
        function ProductPassengerSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "" };
        }
        return ProductPassengerSemantic;
    })(Luxena.Entity2Semantic);
    Luxena.ProductPassengerSemantic = ProductPassengerSemantic;
    /** Проживание */
    var AccommodationSemantic = (function (_super) {
        __extends(AccommodationSemantic, _super);
        function AccommodationSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.big();
            this.ReissueFor
                .localizeTitle({ ru: "Перевыпуск для" })
                .lookup(function () { return Luxena.sd.Accommodation; });
            this.Provider
                .lookup(function () { return Luxena.sd.AccommodationProvider; });
        }
        return AccommodationSemantic;
    })(ProductSemantic);
    Luxena.AccommodationSemantic = AccommodationSemantic;
    /** Аэропорт */
    var AirportSemantic = (function (_super) {
        __extends(AirportSemantic, _super);
        function AirportSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.Name
                .required()
                .length(12, 0, 0);
        }
        return AirportSemantic;
    })(Luxena.Entity3Semantic);
    Luxena.AirportSemantic = AirportSemantic;
    /** Авиадокумент */
    var AviaDocumentSemantic = (function (_super) {
        __extends(AviaDocumentSemantic, _super);
        function AviaDocumentSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
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
        return AviaDocumentSemantic;
    })(ProductSemantic);
    Luxena.AviaDocumentSemantic = AviaDocumentSemantic;
    /** Автобусный билет или возврат */
    var BusDocumentSemantic = (function (_super) {
        __extends(BusDocumentSemantic, _super);
        function BusDocumentSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
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
        return BusDocumentSemantic;
    })(ProductSemantic);
    Luxena.BusDocumentSemantic = BusDocumentSemantic;
    /** Аренда автомобиля */
    var CarRentalSemantic = (function (_super) {
        __extends(CarRentalSemantic, _super);
        function CarRentalSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.big();
            this.ReissueFor
                .lookup(function () { return Luxena.sd.CarRental; });
            this.Provider
                .lookup(function () { return Luxena.sd.CarRentalProvider; });
        }
        return CarRentalSemantic;
    })(ProductSemantic);
    Luxena.CarRentalSemantic = CarRentalSemantic;
    /** ПКО */
    var CashInOrderPaymentSemantic = (function (_super) {
        __extends(CashInOrderPaymentSemantic, _super);
        function CashInOrderPaymentSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Number" };
            this.DocumentNumber
                .localizeTitle({ ru: "№ ПКО" });
        }
        return CashInOrderPaymentSemantic;
    })(PaymentSemantic);
    Luxena.CashInOrderPaymentSemantic = CashInOrderPaymentSemantic;
    /** РКО */
    var CashOutOrderPaymentSemantic = (function (_super) {
        __extends(CashOutOrderPaymentSemantic, _super);
        function CashOutOrderPaymentSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Number" };
            this.DocumentNumber
                .localizeTitle({ ru: "№ РКО" });
        }
        return CashOutOrderPaymentSemantic;
    })(PaymentSemantic);
    Luxena.CashOutOrderPaymentSemantic = CashOutOrderPaymentSemantic;
    /** Кассовый чек */
    var CheckPaymentSemantic = (function (_super) {
        __extends(CheckPaymentSemantic, _super);
        function CheckPaymentSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Number" };
            this.DocumentNumber
                .localizeTitle({ ru: "№ чека", ruShort: "автоматически" })
                .emptyText("автоматически");
        }
        return CheckPaymentSemantic;
    })(PaymentSemantic);
    Luxena.CheckPaymentSemantic = CheckPaymentSemantic;
    /** Страна */
    var CountrySemantic = (function (_super) {
        __extends(CountrySemantic, _super);
        function CountrySemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.Name
                .length(16, 0, 0);
        }
        return CountrySemantic;
    })(Luxena.Entity3Semantic);
    Luxena.CountrySemantic = CountrySemantic;
    /** Электронный платеж */
    var ElectronicPaymentSemantic = (function (_super) {
        __extends(ElectronicPaymentSemantic, _super);
        function ElectronicPaymentSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Number" };
            this.DocumentNumber
                .localizeTitle({ ru: "№ транзакции" });
        }
        return ElectronicPaymentSemantic;
    })(PaymentSemantic);
    Luxena.ElectronicPaymentSemantic = ElectronicPaymentSemantic;
    /** Экскурсия */
    var ExcursionSemantic = (function (_super) {
        __extends(ExcursionSemantic, _super);
        function ExcursionSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.big();
            this.ReissueFor
                .lookup(function () { return Luxena.sd.Excursion; });
        }
        return ExcursionSemantic;
    })(ProductSemantic);
    Luxena.ExcursionSemantic = ExcursionSemantic;
    /** Gds-файл */
    var GdsFileSemantic = (function (_super) {
        __extends(GdsFileSemantic, _super);
        function GdsFileSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
        }
        return GdsFileSemantic;
    })(Luxena.Entity3Semantic);
    Luxena.GdsFileSemantic = GdsFileSemantic;
    /** Дополнительная услуга */
    var GenericProductSemantic = (function (_super) {
        __extends(GenericProductSemantic, _super);
        function GenericProductSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.big();
            this.ReissueFor
                .lookup(function () { return Luxena.sd.GenericProduct; });
            this.Provider
                .lookup(function () { return Luxena.sd.GenericProductProvider; });
        }
        return GenericProductSemantic;
    })(ProductSemantic);
    Luxena.GenericProductSemantic = GenericProductSemantic;
    /** Вид дополнительной услуги */
    var GenericProductTypeSemantic = (function (_super) {
        __extends(GenericProductTypeSemantic, _super);
        function GenericProductTypeSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.small();
        }
        return GenericProductTypeSemantic;
    })(Luxena.Entity3Semantic);
    Luxena.GenericProductTypeSemantic = GenericProductTypeSemantic;
    /** Страховка или возврат */
    var InsuranceDocumentSemantic = (function (_super) {
        __extends(InsuranceDocumentSemantic, _super);
        function InsuranceDocumentSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
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
        return InsuranceDocumentSemantic;
    })(ProductSemantic);
    Luxena.InsuranceDocumentSemantic = InsuranceDocumentSemantic;
    /** Студенческий билет */
    var IsicSemantic = (function (_super) {
        __extends(IsicSemantic, _super);
        function IsicSemantic() {
            _super.call(this);
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
                .string()
                .required();
            /** Номер */
            this.Number2 = this.member()
                .localizeTitle({ ru: "Номер" })
                .string()
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.big();
            this.ReissueFor
                .lookup(function () { return Luxena.sd.Isic; });
        }
        return IsicSemantic;
    })(ProductSemantic);
    Luxena.IsicSemantic = IsicSemantic;
    /** Контрагент */
    var PartySemantic = (function (_super) {
        __extends(PartySemantic, _super);
        function PartySemantic() {
            _super.call(this);
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
                .string()
                .icon("asterisk");
            this.NameForDocuments = this.member()
                .string()
                .calculated()
                .nonsaved();
            /** Телефон 1 */
            this.Phone1 = this.member()
                .localizeTitle({ ru: "Телефон 1" })
                .phone();
            /** Телефон 2 */
            this.Phone2 = this.member()
                .localizeTitle({ ru: "Телефон 2" })
                .phone();
            /** Факс */
            this.Fax = this.member()
                .localizeTitle({ ru: "Факс" })
                .fax();
            /** E-mail 1 */
            this.Email1 = this.member()
                .localizeTitle({ ru: "E-mail 1" })
                .email();
            /** E-mail 2 */
            this.Email2 = this.member()
                .localizeTitle({ ru: "E-mail 2" })
                .email();
            /** Веб адрес */
            this.WebAddress = this.member()
                .localizeTitle({ ru: "Веб адрес" })
                .hyperlink();
            /** Заказчик */
            this.IsCustomer = this.member()
                .localizeTitle({ ru: "Заказчик" })
                .bool()
                .required()
                .icon("street-view");
            /** Поставщик */
            this.IsSupplier = this.member()
                .localizeTitle({ ru: "Поставщик" })
                .bool()
                .required()
                .icon("industry");
            /** Дополнительная информация */
            this.Details = this.member()
                .localizeTitle({ ru: "Дополнительная информация" })
                .text(3);
            /** Юридический адрес */
            this.LegalAddress = this.member()
                .localizeTitle({ ru: "Юридический адрес" })
                .address(3);
            /** Фактический адрес */
            this.ActualAddress = this.member()
                .localizeTitle({ ru: "Фактический адрес" })
                .address(3);
            /** Примечание */
            this.Note = this.member()
                .localizeTitle({ ru: "Примечание" })
                .text(3);
            this.FileCount = this.member()
                .int()
                .calculated()
                .nonsaved();
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
                Luxena.sd.Airline, Luxena.sd.Agent, Luxena.sd.ActiveOwner, Luxena.sd.Customer, Luxena.sd.RoamingOperator, Luxena.sd.Organization, Luxena.sd.Person, Luxena.sd.Department, Luxena.sd.BusTicketProvider, Luxena.sd.CarRentalProvider, Luxena.sd.GenericProductProvider, Luxena.sd.PasteboardProvider, Luxena.sd.AccommodationProvider, Luxena.sd.TransferProvider, Luxena.sd.TourProvider, Luxena.sd.Employee, Luxena.sd.InsuranceCompany
            ]; };
            this._className = "Party";
            this._getRootEntity = function () { return Luxena.sd.Party; };
            this._store = Luxena.db.Parties;
            this._saveStore = Luxena.db.Parties;
            this._lookupStore = Luxena.db.PartyLookup;
            this._lookupFields = { id: "Id", name: "Name" };
            this.Name
                .length(20, 0, 0);
        }
        return PartySemantic;
    })(Luxena.Entity3Semantic);
    Luxena.PartySemantic = PartySemantic;
    /** Платёжная система */
    var PaymentSystemSemantic = (function (_super) {
        __extends(PaymentSystemSemantic, _super);
        function PaymentSystemSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.small();
        }
        return PaymentSystemSemantic;
    })(Luxena.Entity3Semantic);
    Luxena.PaymentSystemSemantic = PaymentSystemSemantic;
    /** Ж/д билет или возврат */
    var RailwayDocumentSemantic = (function (_super) {
        __extends(RailwayDocumentSemantic, _super);
        function RailwayDocumentSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
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
        return RailwayDocumentSemantic;
    })(ProductSemantic);
    Luxena.RailwayDocumentSemantic = RailwayDocumentSemantic;
    /** SIM-карта */
    var SimCardSemantic = (function (_super) {
        __extends(SimCardSemantic, _super);
        function SimCardSemantic() {
            _super.call(this);
            this._SimCard = new Luxena.SemanticMember()
                .localizeTitle({ ru: "SIM-карта", rus: "SIM-карты", ua: "SIM-картка" })
                .lookup(function () { return Luxena.sd.SimCard; });
            /** Номер */
            this.Number = this.member()
                .localizeTitle({ ru: "Номер" })
                .string()
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.big();
            this.ReissueFor
                .lookup(function () { return Luxena.sd.SimCard; });
            this.Producer
                .localizeTitle({ ru: "Оператор" })
                .lookup(function () { return Luxena.sd.RoamingOperator; })
                .required();
        }
        return SimCardSemantic;
    })(ProductSemantic);
    Luxena.SimCardSemantic = SimCardSemantic;
    /** Турпакет */
    var TourSemantic = (function (_super) {
        __extends(TourSemantic, _super);
        function TourSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.big();
            this.ReissueFor
                .lookup(function () { return Luxena.sd.Tour; });
            this.Provider
                .lookup(function () { return Luxena.sd.TourProvider; });
        }
        return TourSemantic;
    })(ProductSemantic);
    Luxena.TourSemantic = TourSemantic;
    /** Трансфер */
    var TransferSemantic = (function (_super) {
        __extends(TransferSemantic, _super);
        function TransferSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.big();
            this.ReissueFor
                .lookup(function () { return Luxena.sd.Transfer; });
            this.Provider
                .lookup(function () { return Luxena.sd.TransferProvider; });
        }
        return TransferSemantic;
    })(ProductSemantic);
    Luxena.TransferSemantic = TransferSemantic;
    /** Безналичный платеж */
    var WireTransferSemantic = (function (_super) {
        __extends(WireTransferSemantic, _super);
        function WireTransferSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Number" };
            this.DocumentNumber
                .localizeTitle({ ru: "№ платежного поручения" });
            this.Invoice
                .localizeTitle({ ru: "Счёт" })
                .lookup(function () { return Luxena.sd.Invoice; });
        }
        return WireTransferSemantic;
    })(PaymentSemantic);
    Luxena.WireTransferSemantic = WireTransferSemantic;
    /** Тип проживания */
    var AccommodationTypeSemantic = (function (_super) {
        __extends(AccommodationTypeSemantic, _super);
        function AccommodationTypeSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.small();
        }
        return AccommodationTypeSemantic;
    })(Luxena.Entity3DSemantic);
    Luxena.AccommodationTypeSemantic = AccommodationTypeSemantic;
    /** МСО */
    var AviaMcoSemantic = (function (_super) {
        __extends(AviaMcoSemantic, _super);
        function AviaMcoSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.big();
        }
        return AviaMcoSemantic;
    })(AviaDocumentSemantic);
    Luxena.AviaMcoSemantic = AviaMcoSemantic;
    /** Возврат авиабилета */
    var AviaRefundSemantic = (function (_super) {
        __extends(AviaRefundSemantic, _super);
        function AviaRefundSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.big();
        }
        return AviaRefundSemantic;
    })(AviaDocumentSemantic);
    Luxena.AviaRefundSemantic = AviaRefundSemantic;
    /** Авиабилет */
    var AviaTicketSemantic = (function (_super) {
        __extends(AviaTicketSemantic, _super);
        function AviaTicketSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.big();
        }
        return AviaTicketSemantic;
    })(AviaDocumentSemantic);
    Luxena.AviaTicketSemantic = AviaTicketSemantic;
    /** Банковский счёт */
    var BankAccountSemantic = (function (_super) {
        __extends(BankAccountSemantic, _super);
        function BankAccountSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.small();
        }
        return BankAccountSemantic;
    })(Luxena.Entity3DSemantic);
    Luxena.BankAccountSemantic = BankAccountSemantic;
    /** Автобусный билет */
    var BusTicketSemantic = (function (_super) {
        __extends(BusTicketSemantic, _super);
        function BusTicketSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.big();
        }
        return BusTicketSemantic;
    })(BusDocumentSemantic);
    Luxena.BusTicketSemantic = BusTicketSemantic;
    /** Возврат автобусного билета */
    var BusTicketRefundSemantic = (function (_super) {
        __extends(BusTicketRefundSemantic, _super);
        function BusTicketRefundSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.big();
        }
        return BusTicketRefundSemantic;
    })(BusDocumentSemantic);
    Luxena.BusTicketRefundSemantic = BusTicketRefundSemantic;
    /** Тип питания */
    var CateringTypeSemantic = (function (_super) {
        __extends(CateringTypeSemantic, _super);
        function CateringTypeSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.small();
        }
        return CateringTypeSemantic;
    })(Luxena.Entity3DSemantic);
    Luxena.CateringTypeSemantic = CateringTypeSemantic;
    /** Подразделение */
    var DepartmentSemantic = (function (_super) {
        __extends(DepartmentSemantic, _super);
        function DepartmentSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
        }
        return DepartmentSemantic;
    })(PartySemantic);
    Luxena.DepartmentSemantic = DepartmentSemantic;
    var IdentitySemantic = (function (_super) {
        __extends(IdentitySemantic, _super);
        function IdentitySemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
        }
        return IdentitySemantic;
    })(Luxena.Entity3DSemantic);
    Luxena.IdentitySemantic = IdentitySemantic;
    /** Страховка */
    var InsuranceSemantic = (function (_super) {
        __extends(InsuranceSemantic, _super);
        function InsuranceSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.big();
        }
        return InsuranceSemantic;
    })(InsuranceDocumentSemantic);
    Luxena.InsuranceSemantic = InsuranceSemantic;
    /** Возврат страховки */
    var InsuranceRefundSemantic = (function (_super) {
        __extends(InsuranceRefundSemantic, _super);
        function InsuranceRefundSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.big();
        }
        return InsuranceRefundSemantic;
    })(InsuranceDocumentSemantic);
    Luxena.InsuranceRefundSemantic = InsuranceRefundSemantic;
    /** Организация */
    var OrganizationSemantic = (function (_super) {
        __extends(OrganizationSemantic, _super);
        function OrganizationSemantic() {
            _super.call(this);
            this._Organization = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Организация", rus: "Организации" })
                .lookup(function () { return Luxena.sd.Organization; });
            /** Авиакомпания */
            this.IsAirline = this.member()
                .localizeTitle({ ru: "Авиакомпания" })
                .bool()
                .required()
                .icon("plane");
            /** IATA код */
            this.AirlineIataCode = this.member()
                .localizeTitle({ ru: "IATA код" })
                .string();
            /** Prefix код */
            this.AirlinePrefixCode = this.member()
                .localizeTitle({ ru: "Prefix код" })
                .string();
            /** Требование паспортных данных */
            this.AirlinePassportRequirement = this.member()
                .localizeTitle({ ru: "Требование паспортных данных" })
                .enum(Luxena.AirlinePassportRequirement)
                .required();
            /** Провайдер проживания */
            this.IsAccommodationProvider = this.member()
                .localizeTitle({ ru: "Провайдер проживания" })
                .bool()
                .required()
                .icon("bed");
            /** Провайдер автобусных билетов */
            this.IsBusTicketProvider = this.member()
                .localizeTitle({ ru: "Провайдер автобусных билетов" })
                .bool()
                .required()
                .icon("bus");
            /** Провайдер аренды авто */
            this.IsCarRentalProvider = this.member()
                .localizeTitle({ ru: "Провайдер аренды авто" })
                .bool()
                .required()
                .icon("car");
            /** Провайдер ж/д билетов */
            this.IsPasteboardProvider = this.member()
                .localizeTitle({ ru: "Провайдер ж/д билетов" })
                .bool()
                .required()
                .icon("subway");
            /** Провайдер турпакетов */
            this.IsTourProvider = this.member()
                .localizeTitle({ ru: "Провайдер турпакетов" })
                .bool()
                .required()
                .icon("suitcase");
            /** Провайдер трансферов */
            this.IsTransferProvider = this.member()
                .localizeTitle({ ru: "Провайдер трансферов" })
                .bool()
                .required()
                .icon("cab");
            /** Провайдер дополнительных услуг */
            this.IsGenericProductProvider = this.member()
                .localizeTitle({ ru: "Провайдер дополнительных услуг" })
                .bool()
                .required()
                .icon("suitcase");
            /** Провайдером услуг */
            this.IsProvider = this.member()
                .localizeTitle({ ru: "Провайдером услуг" })
                .bool()
                .required();
            /** Страховая компания */
            this.IsInsuranceCompany = this.member()
                .localizeTitle({ ru: "Страховая компания" })
                .bool()
                .required()
                .icon("");
            /** Роуминг-оператор */
            this.IsRoamingOperator = this.member()
                .localizeTitle({ ru: "Роуминг-оператор" })
                .bool()
                .required()
                .icon("mobile");
            this.DepartmentCount = this.member()
                .int()
                .calculated()
                .nonsaved()
                .required();
            this.EmployeeCount = this.member()
                .int()
                .calculated()
                .nonsaved()
                .required();
            this.MilesCards = this.collection(function () { return Luxena.sd.MilesCard; }, function (se) { return se.Organization; });
            this.Departments = this.collection(function () { return Luxena.sd.Department; }, function (se) { return se.Organization; });
            /** Сотрудники */
            this.Employees = this.collection(function () { return Luxena.sd.Person; }, function (se) { return se.Organization; }, function (m) { return m
                .localizeTitle({ ru: "Сотрудники" }); });
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.Code
                .localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
        }
        return OrganizationSemantic;
    })(PartySemantic);
    Luxena.OrganizationSemantic = OrganizationSemantic;
    /** Ж/д билет */
    var PasteboardSemantic = (function (_super) {
        __extends(PasteboardSemantic, _super);
        function PasteboardSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.big();
        }
        return PasteboardSemantic;
    })(RailwayDocumentSemantic);
    Luxena.PasteboardSemantic = PasteboardSemantic;
    /** Возврат ж/д билета */
    var PasteboardRefundSemantic = (function (_super) {
        __extends(PasteboardRefundSemantic, _super);
        function PasteboardRefundSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.big();
        }
        return PasteboardRefundSemantic;
    })(RailwayDocumentSemantic);
    Luxena.PasteboardRefundSemantic = PasteboardRefundSemantic;
    /** Персона */
    var PersonSemantic = (function (_super) {
        __extends(PersonSemantic, _super);
        function PersonSemantic() {
            _super.call(this);
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
                Luxena.sd.Agent, Luxena.sd.Employee
            ]; };
            this._getBaseEntity = function () { return Luxena.sd.Party; };
            this._className = "Party";
            this._getRootEntity = function () { return Luxena.sd.Party; };
            this._store = Luxena.db.Persons;
            this._saveStore = Luxena.db.Persons;
            this._lookupStore = Luxena.db.PersonLookup;
            this._lookupFields = { id: "Id", name: "Name" };
            this.Name
                .localizeTitle({ ru: "Ф.И.О." })
                .length(20, 0, 0);
        }
        return PersonSemantic;
    })(PartySemantic);
    Luxena.PersonSemantic = PersonSemantic;
    var InternalIdentitySemantic = (function (_super) {
        __extends(InternalIdentitySemantic, _super);
        function InternalIdentitySemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
        }
        return InternalIdentitySemantic;
    })(IdentitySemantic);
    Luxena.InternalIdentitySemantic = InternalIdentitySemantic;
    /** Пользователь */
    var UserSemantic = (function (_super) {
        __extends(UserSemantic, _super);
        function UserSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.Name
                .localizeTitle({ ru: "Логин" })
                .length(16, 0, 0);
        }
        return UserSemantic;
    })(IdentitySemantic);
    Luxena.UserSemantic = UserSemantic;
    /** Провайдер проживания */
    var AccommodationProviderSemantic = (function (_super) {
        __extends(AccommodationProviderSemantic, _super);
        function AccommodationProviderSemantic() {
            _super.call(this);
            this._AccommodationProvider = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Провайдер проживания", rus: "Провайдеры проживания" })
                .lookup(function () { return Luxena.sd.AccommodationProvider; });
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.Code
                .localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
        }
        return AccommodationProviderSemantic;
    })(OrganizationSemantic);
    Luxena.AccommodationProviderSemantic = AccommodationProviderSemantic;
    /** Владелец документов (активный) */
    var ActiveOwnerSemantic = (function (_super) {
        __extends(ActiveOwnerSemantic, _super);
        function ActiveOwnerSemantic() {
            _super.call(this);
            this._ActiveOwner = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Владелец документов (активный)", rus: "Владельцы документов (активные)" })
                .lookup(function () { return Luxena.sd.ActiveOwner; });
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.small();
            this.Name
                .length(20, 0, 0);
        }
        return ActiveOwnerSemantic;
    })(PartySemantic);
    Luxena.ActiveOwnerSemantic = ActiveOwnerSemantic;
    /** Агент */
    var AgentSemantic = (function (_super) {
        __extends(AgentSemantic, _super);
        function AgentSemantic() {
            _super.call(this);
            this._Agent = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Агент", rus: "Агенты" })
                .lookup(function () { return Luxena.sd.Agent; });
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.small();
            this.Name
                .localizeTitle({ ru: "Ф.И.О." })
                .length(20, 0, 0);
        }
        return AgentSemantic;
    })(PersonSemantic);
    Luxena.AgentSemantic = AgentSemantic;
    /** Авиакомпания */
    var AirlineSemantic = (function (_super) {
        __extends(AirlineSemantic, _super);
        function AirlineSemantic() {
            _super.call(this);
            this._Airline = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Авиакомпания", rus: "Авиакомпании" })
                .lookup(function () { return Luxena.sd.Airline; });
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.Code
                .localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
        }
        return AirlineSemantic;
    })(OrganizationSemantic);
    Luxena.AirlineSemantic = AirlineSemantic;
    /** Провайдер автобусных билетов */
    var BusTicketProviderSemantic = (function (_super) {
        __extends(BusTicketProviderSemantic, _super);
        function BusTicketProviderSemantic() {
            _super.call(this);
            this._BusTicketProvider = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Провайдер автобусных билетов", rus: "Провайдеры автобусных билетов" })
                .lookup(function () { return Luxena.sd.BusTicketProvider; });
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.Code
                .localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
        }
        return BusTicketProviderSemantic;
    })(OrganizationSemantic);
    Luxena.BusTicketProviderSemantic = BusTicketProviderSemantic;
    /** Провайдер аренды авто */
    var CarRentalProviderSemantic = (function (_super) {
        __extends(CarRentalProviderSemantic, _super);
        function CarRentalProviderSemantic() {
            _super.call(this);
            this._CarRentalProvider = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Провайдер аренды авто", rus: "Провайдеры аренды авто" })
                .lookup(function () { return Luxena.sd.CarRentalProvider; });
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.Code
                .localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
        }
        return CarRentalProviderSemantic;
    })(OrganizationSemantic);
    Luxena.CarRentalProviderSemantic = CarRentalProviderSemantic;
    /** Заказчик */
    var CustomerSemantic = (function (_super) {
        __extends(CustomerSemantic, _super);
        function CustomerSemantic() {
            _super.call(this);
            this._Customer = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Заказчик", rus: "Заказчики" })
                .lookup(function () { return Luxena.sd.Customer; });
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.Name
                .length(20, 0, 0);
        }
        return CustomerSemantic;
    })(PartySemantic);
    Luxena.CustomerSemantic = CustomerSemantic;
    /** Сотрудник */
    var EmployeeSemantic = (function (_super) {
        __extends(EmployeeSemantic, _super);
        function EmployeeSemantic() {
            _super.call(this);
            this._Employee = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Сотрудник", rus: "Сотрудники" })
                .lookup(function () { return Luxena.sd.Employee; });
            this._isAbstract = false;
            this._name = "Employee";
            this._names = "Employees";
            this.icon("user");
            this._isEntityQuery = true;
            this._localizeTitle({ ru: "Сотрудник", rus: "Сотрудники" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Person; };
            this._className = "Party";
            this._getRootEntity = function () { return Luxena.sd.Party; };
            this._store = Luxena.db.Employees;
            this._saveStore = Luxena.db.Persons;
            this._lookupStore = Luxena.db.EmployeeLookup;
            this._lookupFields = { id: "Id", name: "Name" };
            this.Name
                .localizeTitle({ ru: "Ф.И.О." })
                .length(20, 0, 0);
        }
        return EmployeeSemantic;
    })(PersonSemantic);
    Luxena.EmployeeSemantic = EmployeeSemantic;
    /** Провайдер дополнительных услуг */
    var GenericProductProviderSemantic = (function (_super) {
        __extends(GenericProductProviderSemantic, _super);
        function GenericProductProviderSemantic() {
            _super.call(this);
            this._GenericProductProvider = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Провайдер дополнительных услуг", rus: "Провайдеры дополнительных услуг" })
                .lookup(function () { return Luxena.sd.GenericProductProvider; });
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.Code
                .localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
        }
        return GenericProductProviderSemantic;
    })(OrganizationSemantic);
    Luxena.GenericProductProviderSemantic = GenericProductProviderSemantic;
    /** Страховая компания */
    var InsuranceCompanySemantic = (function (_super) {
        __extends(InsuranceCompanySemantic, _super);
        function InsuranceCompanySemantic() {
            _super.call(this);
            this._InsuranceCompany = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Страховая компания", rus: "Страховые компании" })
                .lookup(function () { return Luxena.sd.InsuranceCompany; });
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.Code
                .localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
        }
        return InsuranceCompanySemantic;
    })(OrganizationSemantic);
    Luxena.InsuranceCompanySemantic = InsuranceCompanySemantic;
    /** Провайдер ж/д билетов */
    var PasteboardProviderSemantic = (function (_super) {
        __extends(PasteboardProviderSemantic, _super);
        function PasteboardProviderSemantic() {
            _super.call(this);
            this._PasteboardProvider = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Провайдер ж/д билетов", rus: "Провайдеры ж/д билетов" })
                .lookup(function () { return Luxena.sd.PasteboardProvider; });
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.Code
                .localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
        }
        return PasteboardProviderSemantic;
    })(OrganizationSemantic);
    Luxena.PasteboardProviderSemantic = PasteboardProviderSemantic;
    /** Квитанция */
    var ReceiptSemantic = (function (_super) {
        __extends(ReceiptSemantic, _super);
        function ReceiptSemantic() {
            _super.call(this);
            this._Receipt = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Квитанция", rus: "Квитанции" })
                .lookup(function () { return Luxena.sd.Receipt; });
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
            this._lookupFields = { id: "Id", name: "Number" };
        }
        return ReceiptSemantic;
    })(InvoiceSemantic);
    Luxena.ReceiptSemantic = ReceiptSemantic;
    /** Мобильный оператор */
    var RoamingOperatorSemantic = (function (_super) {
        __extends(RoamingOperatorSemantic, _super);
        function RoamingOperatorSemantic() {
            _super.call(this);
            this._RoamingOperator = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Мобильный оператор", rus: "Мобильные операторы" })
                .lookup(function () { return Luxena.sd.RoamingOperator; });
            this._isAbstract = false;
            this._name = "RoamingOperator";
            this._names = "RoamingOperators";
            this.icon("mobile");
            this._isEntityQuery = true;
            this._localizeTitle({ ru: "Мобильный оператор", rus: "Мобильные операторы" });
            this._getDerivedEntities = null;
            this._getBaseEntity = function () { return Luxena.sd.Organization; };
            this._className = "Party";
            this._getRootEntity = function () { return Luxena.sd.Party; };
            this._store = Luxena.db.RoamingOperators;
            this._saveStore = Luxena.db.Organizations;
            this._lookupStore = Luxena.db.RoamingOperatorLookup;
            this._lookupFields = { id: "Id", name: "Name" };
            this.small();
            this.Code
                .localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
        }
        return RoamingOperatorSemantic;
    })(OrganizationSemantic);
    Luxena.RoamingOperatorSemantic = RoamingOperatorSemantic;
    /** Провайдер туров (готовых) */
    var TourProviderSemantic = (function (_super) {
        __extends(TourProviderSemantic, _super);
        function TourProviderSemantic() {
            _super.call(this);
            this._TourProvider = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Провайдер туров (готовых)", rus: "Провайдеры туров (готовых)" })
                .lookup(function () { return Luxena.sd.TourProvider; });
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.Code
                .localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
        }
        return TourProviderSemantic;
    })(OrganizationSemantic);
    Luxena.TourProviderSemantic = TourProviderSemantic;
    /** Провайдер трансферов */
    var TransferProviderSemantic = (function (_super) {
        __extends(TransferProviderSemantic, _super);
        function TransferProviderSemantic() {
            _super.call(this);
            this._TransferProvider = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Провайдер трансферов", rus: "Провайдеры трансферов" })
                .lookup(function () { return Luxena.sd.TransferProvider; });
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
            this._lookupFields = { id: "Id", name: "Name" };
            this.Code
                .localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
        }
        return TransferProviderSemantic;
    })(OrganizationSemantic);
    Luxena.TransferProviderSemantic = TransferProviderSemantic;
    var ProductTotalSemantic = (function (_super) {
        __extends(ProductTotalSemantic, _super);
        function ProductTotalSemantic() {
            _super.call(this);
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
        return ProductTotalSemantic;
    })(Luxena.SemanticEntity);
    Luxena.ProductTotalSemantic = ProductTotalSemantic;
    var ProfitDistributionTotalSemantic = (function (_super) {
        __extends(ProfitDistributionTotalSemantic, _super);
        function ProfitDistributionTotalSemantic() {
            _super.call(this);
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
        }
        return ProfitDistributionTotalSemantic;
    })(Luxena.SemanticEntity);
    Luxena.ProfitDistributionTotalSemantic = ProfitDistributionTotalSemantic;
    /** Ежедневный отчет по выручке */
    var EverydayProfitReportSemantic = (function (_super) {
        __extends(EverydayProfitReportSemantic, _super);
        function EverydayProfitReportSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "", name: "" };
            this.Provider
                .lookup(function () { return Luxena.sd.Organization; });
            this.ProductType
                .localizeTitle({ ru: "Вид услуги" })
                .required()
                .length(12, 0, 0)
                .entityType();
            this.IssueDate
                .required()
                .entityDate();
            this.Seller
                .lookup(function () { return Luxena.sd.Person; });
            this.Itinerary
                .localizeTitle({ ru: "Маршрут" })
                .length(16, 0, 0);
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
            this.GrandTotal
                .localizeTitle({ ru: "К оплате" });
            this.Order
                .localizeTitle({ ru: "Заказ", rus: "Заказы" })
                .lookup(function () { return Luxena.sd.Order; });
            this.Payer
                .lookup(function () { return Luxena.sd.Party; });
        }
        return EverydayProfitReportSemantic;
    })(Luxena.SemanticEntity);
    Luxena.EverydayProfitReportSemantic = EverydayProfitReportSemantic;
    /** Flown-отчет */
    var FlownReportSemantic = (function (_super) {
        __extends(FlownReportSemantic, _super);
        function FlownReportSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "", name: "" };
            this.TourCode
                .localizeTitle({ ru: "Туркод" });
        }
        return FlownReportSemantic;
    })(Luxena.SemanticEntity);
    Luxena.FlownReportSemantic = FlownReportSemantic;
    /** Баланс */
    var OrderBalanceSemantic = (function (_super) {
        __extends(OrderBalanceSemantic, _super);
        function OrderBalanceSemantic() {
            _super.call(this);
            this._OrderBalance = new Luxena.SemanticMember()
                .localizeTitle({ ru: "Баланс" })
                .lookup(function () { return Luxena.sd.OrderBalance; });
            /** Заказ */
            this.Order = this.member()
                .localizeTitle({ ru: "Заказ", rus: "Заказы" })
                .lookup(function () { return Luxena.sd.Order; });
            /** Дата заказа */
            this.IssueDate = this.member()
                .localizeTitle({ ru: "Дата заказа" })
                .date();
            this.Customer = this.member()
                .lookup(function () { return Luxena.sd.Party; });
            /** Валюта */
            this.Currency = this.member()
                .localizeTitle({ ru: "Валюта" })
                .string()
                .length(3, 0, 0);
            /** Оказано услуг на сумму */
            this.Delivered = this.member()
                .localizeTitle({ ru: "Оказано услуг на сумму" })
                .float(2)
                .required();
            /** Оплачено */
            this.Paid = this.member()
                .localizeTitle({ ru: "Оплачено" })
                .float(2)
                .required();
            /** Баланс */
            this.Balance = this.member()
                .localizeTitle({ ru: "Баланс" })
                .float(2)
                .required();
            this._isAbstract = false;
            this._name = "OrderBalance";
            this._names = "OrderBalances";
            this._isQueryResult = true;
            this._localizeTitle({ ru: "Баланс" });
            this._getDerivedEntities = null;
            this._className = "OrderBalance";
            this._getRootEntity = function () { return Luxena.sd.OrderBalance; };
            this._store = Luxena.db.OrderBalances;
            this._saveStore = Luxena.db.OrderBalances;
            this._lookupFields = { id: "", name: "" };
        }
        return OrderBalanceSemantic;
    })(Luxena.SemanticEntity);
    Luxena.OrderBalanceSemantic = OrderBalanceSemantic;
    /** Сводка по услугам */
    var ProductSummarySemantic = (function (_super) {
        __extends(ProductSummarySemantic, _super);
        function ProductSummarySemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "", name: "" };
            this.IssueDate
                .entityDate();
            this.Type
                .localizeTitle({ ru: "Вид услуги" })
                .length(12, 0, 0)
                .entityType();
            this.Name
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
                .lookup(function () { return Luxena.sd.Order; });
        }
        return ProductSummarySemantic;
    })(Luxena.SemanticEntity);
    Luxena.ProductSummarySemantic = ProductSummarySemantic;
    /** Услуги итого по бронировщику */
    var ProductTotalByBookerSemantic = (function (_super) {
        __extends(ProductTotalByBookerSemantic, _super);
        function ProductTotalByBookerSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "", name: "" };
            this.Booker
                .lookup(function () { return Luxena.sd.Person; });
        }
        return ProductTotalByBookerSemantic;
    })(ProductTotalSemantic);
    Luxena.ProductTotalByBookerSemantic = ProductTotalByBookerSemantic;
    /** Услуги итого посуточно */
    var ProductTotalByDaySemantic = (function (_super) {
        __extends(ProductTotalByDaySemantic, _super);
        function ProductTotalByDaySemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "IssueDate", name: "" };
        }
        return ProductTotalByDaySemantic;
    })(ProductTotalSemantic);
    Luxena.ProductTotalByDaySemantic = ProductTotalByDaySemantic;
    /** Услуги итого помесячно */
    var ProductTotalByMonthSemantic = (function (_super) {
        __extends(ProductTotalByMonthSemantic, _super);
        function ProductTotalByMonthSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "", name: "" };
        }
        return ProductTotalByMonthSemantic;
    })(ProductTotalSemantic);
    Luxena.ProductTotalByMonthSemantic = ProductTotalByMonthSemantic;
    /** Услуги итого по владельцу */
    var ProductTotalByOwnerSemantic = (function (_super) {
        __extends(ProductTotalByOwnerSemantic, _super);
        function ProductTotalByOwnerSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "", name: "" };
            this.Owner
                .lookup(function () { return Luxena.sd.Party; });
        }
        return ProductTotalByOwnerSemantic;
    })(ProductTotalSemantic);
    Luxena.ProductTotalByOwnerSemantic = ProductTotalByOwnerSemantic;
    /** Услуги итого по провайдеру */
    var ProductTotalByProviderSemantic = (function (_super) {
        __extends(ProductTotalByProviderSemantic, _super);
        function ProductTotalByProviderSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "", name: "" };
            this.Provider
                .lookup(function () { return Luxena.sd.Organization; });
        }
        return ProductTotalByProviderSemantic;
    })(ProductTotalSemantic);
    Luxena.ProductTotalByProviderSemantic = ProductTotalByProviderSemantic;
    /** Услуги итого поквартально */
    var ProductTotalByQuarterSemantic = (function (_super) {
        __extends(ProductTotalByQuarterSemantic, _super);
        function ProductTotalByQuarterSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "", name: "" };
        }
        return ProductTotalByQuarterSemantic;
    })(ProductTotalSemantic);
    Luxena.ProductTotalByQuarterSemantic = ProductTotalByQuarterSemantic;
    /** Услуги итого по продавцу */
    var ProductTotalBySellerSemantic = (function (_super) {
        __extends(ProductTotalBySellerSemantic, _super);
        function ProductTotalBySellerSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "", name: "" };
            this.Seller
                .lookup(function () { return Luxena.sd.Person; });
        }
        return ProductTotalBySellerSemantic;
    })(ProductTotalSemantic);
    Luxena.ProductTotalBySellerSemantic = ProductTotalBySellerSemantic;
    /** Услуги итого по видам услуг */
    var ProductTotalByTypeSemantic = (function (_super) {
        __extends(ProductTotalByTypeSemantic, _super);
        function ProductTotalByTypeSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "", name: "" };
            this.Type
                .localizeTitle({ ru: "Вид услуги" })
                .required()
                .length(20, 0, 0)
                .entityType();
        }
        return ProductTotalByTypeSemantic;
    })(ProductTotalSemantic);
    Luxena.ProductTotalByTypeSemantic = ProductTotalByTypeSemantic;
    /** Услуги итого по годам */
    var ProductTotalByYearSemantic = (function (_super) {
        __extends(ProductTotalByYearSemantic, _super);
        function ProductTotalByYearSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Year", name: "" };
        }
        return ProductTotalByYearSemantic;
    })(ProductTotalSemantic);
    Luxena.ProductTotalByYearSemantic = ProductTotalByYearSemantic;
    /** Распределение выручки по заказчикам */
    var ProfitDistributionByCustomerSemantic = (function (_super) {
        __extends(ProfitDistributionByCustomerSemantic, _super);
        function ProfitDistributionByCustomerSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "", name: "" };
            this.Customer
                .lookup(function () { return Luxena.sd.Party; });
        }
        return ProfitDistributionByCustomerSemantic;
    })(ProfitDistributionTotalSemantic);
    Luxena.ProfitDistributionByCustomerSemantic = ProfitDistributionByCustomerSemantic;
    /** Распределение выручки по провайдерам */
    var ProfitDistributionByProviderSemantic = (function (_super) {
        __extends(ProfitDistributionByProviderSemantic, _super);
        function ProfitDistributionByProviderSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "", name: "" };
            this.Provider
                .lookup(function () { return Luxena.sd.Organization; });
        }
        return ProfitDistributionByProviderSemantic;
    })(ProfitDistributionTotalSemantic);
    Luxena.ProfitDistributionByProviderSemantic = ProfitDistributionByProviderSemantic;
    var ProductFilterSemantic = (function (_super) {
        __extends(ProductFilterSemantic, _super);
        function ProductFilterSemantic() {
            _super.call(this);
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
                .entityDate();
            this.IssueMonth
                .entityDate();
            this.MinIssueDate
                .entityDate();
            this.MaxIssueDate
                .entityDate();
            this.Type
                .localizeTitle({ ru: "Вид услуги" })
                .length(12, 0, 0)
                .entityType();
            this.Provider
                .lookup(function () { return Luxena.sd.Organization; });
            this.Customer
                .lookup(function () { return Luxena.sd.Party; });
            this.Booker
                .lookup(function () { return Luxena.sd.Person; });
            this.Ticketer
                .lookup(function () { return Luxena.sd.Person; });
            this.Seller
                .lookup(function () { return Luxena.sd.Person; });
            this.Owner
                .lookup(function () { return Luxena.sd.Party; });
        }
        return ProductFilterSemantic;
    })(Luxena.SemanticEntity);
    Luxena.ProductFilterSemantic = ProductFilterSemantic;
    var EverydayProfitReportParamsSemantic = (function (_super) {
        __extends(EverydayProfitReportParamsSemantic, _super);
        function EverydayProfitReportParamsSemantic() {
            _super.call(this);
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
        return EverydayProfitReportParamsSemantic;
    })(ProductFilterSemantic);
    Luxena.EverydayProfitReportParamsSemantic = EverydayProfitReportParamsSemantic;
    var FlownReportParamsSemantic = (function (_super) {
        __extends(FlownReportParamsSemantic, _super);
        function FlownReportParamsSemantic() {
            _super.call(this);
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
        return FlownReportParamsSemantic;
    })(ProductFilterSemantic);
    Luxena.FlownReportParamsSemantic = FlownReportParamsSemantic;
    var OrderBalanceParamsSemantic = (function (_super) {
        __extends(OrderBalanceParamsSemantic, _super);
        function OrderBalanceParamsSemantic() {
            _super.call(this);
            /** Заказ */
            this.Order = this.member()
                .localizeTitle({ ru: "Заказ", rus: "Заказы" })
                .lookup(function () { return Luxena.sd.Order; });
            this.IssueDate = this.member()
                .date();
            this.Customer = this.member()
                .lookup(function () { return Luxena.sd.Party; });
            this.Currency = this.member()
                .string();
            this.Delivered = this.member()
                .float()
                .required();
            this.Paid = this.member()
                .float()
                .required();
            this.Balance = this.member()
                .float()
                .required();
            this._isAbstract = false;
            this._name = "OrderBalanceParams";
            this._names = "OrderBalanceParams";
            this._isQueryParams = true;
            this._getDerivedEntities = null;
        }
        return OrderBalanceParamsSemantic;
    })(Luxena.SemanticEntity);
    Luxena.OrderBalanceParamsSemantic = OrderBalanceParamsSemantic;
    var ProductSummaryParamsSemantic = (function (_super) {
        __extends(ProductSummaryParamsSemantic, _super);
        function ProductSummaryParamsSemantic() {
            _super.call(this);
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
        return ProductSummaryParamsSemantic;
    })(ProductFilterSemantic);
    Luxena.ProductSummaryParamsSemantic = ProductSummaryParamsSemantic;
    var ProductTotalByBookerParamsSemantic = (function (_super) {
        __extends(ProductTotalByBookerParamsSemantic, _super);
        function ProductTotalByBookerParamsSemantic() {
            _super.call(this);
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
        return ProductTotalByBookerParamsSemantic;
    })(ProductFilterSemantic);
    Luxena.ProductTotalByBookerParamsSemantic = ProductTotalByBookerParamsSemantic;
    var ProductTotalByDayParamsSemantic = (function (_super) {
        __extends(ProductTotalByDayParamsSemantic, _super);
        function ProductTotalByDayParamsSemantic() {
            _super.call(this);
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
        return ProductTotalByDayParamsSemantic;
    })(ProductFilterSemantic);
    Luxena.ProductTotalByDayParamsSemantic = ProductTotalByDayParamsSemantic;
    var ProductTotalByMonthParamsSemantic = (function (_super) {
        __extends(ProductTotalByMonthParamsSemantic, _super);
        function ProductTotalByMonthParamsSemantic() {
            _super.call(this);
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
        return ProductTotalByMonthParamsSemantic;
    })(ProductFilterSemantic);
    Luxena.ProductTotalByMonthParamsSemantic = ProductTotalByMonthParamsSemantic;
    var ProductTotalByOwnerParamsSemantic = (function (_super) {
        __extends(ProductTotalByOwnerParamsSemantic, _super);
        function ProductTotalByOwnerParamsSemantic() {
            _super.call(this);
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
        return ProductTotalByOwnerParamsSemantic;
    })(ProductFilterSemantic);
    Luxena.ProductTotalByOwnerParamsSemantic = ProductTotalByOwnerParamsSemantic;
    var ProductTotalByProviderParamsSemantic = (function (_super) {
        __extends(ProductTotalByProviderParamsSemantic, _super);
        function ProductTotalByProviderParamsSemantic() {
            _super.call(this);
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
        return ProductTotalByProviderParamsSemantic;
    })(ProductFilterSemantic);
    Luxena.ProductTotalByProviderParamsSemantic = ProductTotalByProviderParamsSemantic;
    var ProductTotalByQuarterParamsSemantic = (function (_super) {
        __extends(ProductTotalByQuarterParamsSemantic, _super);
        function ProductTotalByQuarterParamsSemantic() {
            _super.call(this);
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
        return ProductTotalByQuarterParamsSemantic;
    })(ProductFilterSemantic);
    Luxena.ProductTotalByQuarterParamsSemantic = ProductTotalByQuarterParamsSemantic;
    var ProductTotalBySellerParamsSemantic = (function (_super) {
        __extends(ProductTotalBySellerParamsSemantic, _super);
        function ProductTotalBySellerParamsSemantic() {
            _super.call(this);
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
        return ProductTotalBySellerParamsSemantic;
    })(ProductFilterSemantic);
    Luxena.ProductTotalBySellerParamsSemantic = ProductTotalBySellerParamsSemantic;
    var ProductTotalByTypeParamsSemantic = (function (_super) {
        __extends(ProductTotalByTypeParamsSemantic, _super);
        function ProductTotalByTypeParamsSemantic() {
            _super.call(this);
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
        return ProductTotalByTypeParamsSemantic;
    })(ProductFilterSemantic);
    Luxena.ProductTotalByTypeParamsSemantic = ProductTotalByTypeParamsSemantic;
    var ProductTotalByYearParamsSemantic = (function (_super) {
        __extends(ProductTotalByYearParamsSemantic, _super);
        function ProductTotalByYearParamsSemantic() {
            _super.call(this);
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
        return ProductTotalByYearParamsSemantic;
    })(ProductFilterSemantic);
    Luxena.ProductTotalByYearParamsSemantic = ProductTotalByYearParamsSemantic;
    var ProfitDistributionByCustomerParamsSemantic = (function (_super) {
        __extends(ProfitDistributionByCustomerParamsSemantic, _super);
        function ProfitDistributionByCustomerParamsSemantic() {
            _super.call(this);
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
        return ProfitDistributionByCustomerParamsSemantic;
    })(ProductFilterSemantic);
    Luxena.ProfitDistributionByCustomerParamsSemantic = ProfitDistributionByCustomerParamsSemantic;
    var ProfitDistributionByProviderParamsSemantic = (function (_super) {
        __extends(ProfitDistributionByProviderParamsSemantic, _super);
        function ProfitDistributionByProviderParamsSemantic() {
            _super.call(this);
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
        return ProfitDistributionByProviderParamsSemantic;
    })(ProductFilterSemantic);
    Luxena.ProfitDistributionByProviderParamsSemantic = ProfitDistributionByProviderParamsSemantic;
    /** Применить к документам */
    var GdsAgent_ApplyToUnassignedSemantic = (function (_super) {
        __extends(GdsAgent_ApplyToUnassignedSemantic, _super);
        function GdsAgent_ApplyToUnassignedSemantic() {
            _super.call(this);
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
            this._lookupFields = { id: "Id", name: "" };
        }
        return GdsAgent_ApplyToUnassignedSemantic;
    })(Luxena.DomainActionSemantic);
    Luxena.GdsAgent_ApplyToUnassignedSemantic = GdsAgent_ApplyToUnassignedSemantic;
    var OrderTotalByDateSemantic = (function (_super) {
        __extends(OrderTotalByDateSemantic, _super);
        function OrderTotalByDateSemantic() {
            _super.call(this);
            /** Дата заказа */
            this.Date = this.member()
                .localizeTitle({ ru: "Дата заказа" })
                .date();
            /** Итого к оплате */
            this.Total = this.member()
                .localizeTitle({ ru: "Итого к оплате" })
                .float();
            /** Всего за период к оплате */
            this.SumTotal = this.member()
                .localizeTitle({ ru: "Всего за период к оплате" })
                .float();
            this._isAbstract = false;
            this._name = "OrderTotalByDate";
            this._names = "OrderTotalByDate";
            this._getDerivedEntities = null;
        }
        return OrderTotalByDateSemantic;
    })(Luxena.SemanticEntity);
    Luxena.OrderTotalByDateSemantic = OrderTotalByDateSemantic;
    var ProductTotalByDateSemantic = (function (_super) {
        __extends(ProductTotalByDateSemantic, _super);
        function ProductTotalByDateSemantic() {
            _super.call(this);
            /** Дата услуги */
            this.Date = this.member()
                .localizeTitle({ ru: "Дата услуги" })
                .date();
            /** Итого к оплате */
            this.GrandTotal = this.member()
                .localizeTitle({ ru: "Итого к оплате" })
                .float();
            /** Всего за период к оплате */
            this.SumGrandTotal = this.member()
                .localizeTitle({ ru: "Всего за период к оплате" })
                .float();
            this._isAbstract = false;
            this._name = "ProductTotalByDate";
            this._names = "ProductTotalByDate";
            this._getDerivedEntities = null;
        }
        return ProductTotalByDateSemantic;
    })(Luxena.SemanticEntity);
    Luxena.ProductTotalByDateSemantic = ProductTotalByDateSemantic;
    /** Динамика заказов ответственного */
    var OrderByAssignedTo_TotalByIssueDateSemantic = (function (_super) {
        __extends(OrderByAssignedTo_TotalByIssueDateSemantic, _super);
        function OrderByAssignedTo_TotalByIssueDateSemantic() {
            _super.call(this);
            this.AssignedToId = this.member().string();
            this._names = this._name = "OrderByAssignedTo_TotalByIssueDate";
            this._store = Luxena.db.OrderByAssignedTo_TotalByIssueDate;
            this._isDomainFunction = true;
            this._localizeTitle({ ru: "Динамика заказов ответственного" });
        }
        return OrderByAssignedTo_TotalByIssueDateSemantic;
    })(OrderTotalByDateSemantic);
    Luxena.OrderByAssignedTo_TotalByIssueDateSemantic = OrderByAssignedTo_TotalByIssueDateSemantic;
    /** Динамика заказов заказчика */
    var OrderByCustomer_TotalByIssueDateSemantic = (function (_super) {
        __extends(OrderByCustomer_TotalByIssueDateSemantic, _super);
        function OrderByCustomer_TotalByIssueDateSemantic() {
            _super.call(this);
            this.CustomerId = this.member().string();
            this._names = this._name = "OrderByCustomer_TotalByIssueDate";
            this._store = Luxena.db.OrderByCustomer_TotalByIssueDate;
            this._isDomainFunction = true;
            this._localizeTitle({ ru: "Динамика заказов заказчика" });
        }
        return OrderByCustomer_TotalByIssueDateSemantic;
    })(OrderTotalByDateSemantic);
    Luxena.OrderByCustomer_TotalByIssueDateSemantic = OrderByCustomer_TotalByIssueDateSemantic;
    /** Динамика заказов владельца */
    var OrderByOwner_TotalByIssueDateSemantic = (function (_super) {
        __extends(OrderByOwner_TotalByIssueDateSemantic, _super);
        function OrderByOwner_TotalByIssueDateSemantic() {
            _super.call(this);
            this.OwnerId = this.member().string();
            this._names = this._name = "OrderByOwner_TotalByIssueDate";
            this._store = Luxena.db.OrderByOwner_TotalByIssueDate;
            this._isDomainFunction = true;
            this._localizeTitle({ ru: "Динамика заказов владельца" });
        }
        return OrderByOwner_TotalByIssueDateSemantic;
    })(OrderTotalByDateSemantic);
    Luxena.OrderByOwner_TotalByIssueDateSemantic = OrderByOwner_TotalByIssueDateSemantic;
    /** Динамика забронированных услуг */
    var ProductByBooker_TotalByIssueDateSemantic = (function (_super) {
        __extends(ProductByBooker_TotalByIssueDateSemantic, _super);
        function ProductByBooker_TotalByIssueDateSemantic() {
            _super.call(this);
            this.BookerId = this.member().string();
            this._names = this._name = "ProductByBooker_TotalByIssueDate";
            this._store = Luxena.db.ProductByBooker_TotalByIssueDate;
            this._isDomainFunction = true;
            this._localizeTitle({ ru: "Динамика забронированных услуг" });
        }
        return ProductByBooker_TotalByIssueDateSemantic;
    })(ProductTotalByDateSemantic);
    Luxena.ProductByBooker_TotalByIssueDateSemantic = ProductByBooker_TotalByIssueDateSemantic;
    /** Динамика поставленных услуг */
    var ProductByProvider_TotalByIssueDateSemantic = (function (_super) {
        __extends(ProductByProvider_TotalByIssueDateSemantic, _super);
        function ProductByProvider_TotalByIssueDateSemantic() {
            _super.call(this);
            this.ProviderId = this.member().string();
            this._names = this._name = "ProductByProvider_TotalByIssueDate";
            this._store = Luxena.db.ProductByProvider_TotalByIssueDate;
            this._isDomainFunction = true;
            this._localizeTitle({ ru: "Динамика поставленных услуг" });
        }
        return ProductByProvider_TotalByIssueDateSemantic;
    })(ProductTotalByDateSemantic);
    Luxena.ProductByProvider_TotalByIssueDateSemantic = ProductByProvider_TotalByIssueDateSemantic;
    /** Динамика проданных услуг */
    var ProductBySeller_TotalByIssueDateSemantic = (function (_super) {
        __extends(ProductBySeller_TotalByIssueDateSemantic, _super);
        function ProductBySeller_TotalByIssueDateSemantic() {
            _super.call(this);
            this.SellerId = this.member().string();
            this._names = this._name = "ProductBySeller_TotalByIssueDate";
            this._store = Luxena.db.ProductBySeller_TotalByIssueDate;
            this._isDomainFunction = true;
            this._localizeTitle({ ru: "Динамика проданных услуг" });
        }
        return ProductBySeller_TotalByIssueDateSemantic;
    })(ProductTotalByDateSemantic);
    Luxena.ProductBySeller_TotalByIssueDateSemantic = ProductBySeller_TotalByIssueDateSemantic;
    /** Динамика тикетированных услуг */
    var ProductByTicketer_TotalByIssueDateSemantic = (function (_super) {
        __extends(ProductByTicketer_TotalByIssueDateSemantic, _super);
        function ProductByTicketer_TotalByIssueDateSemantic() {
            _super.call(this);
            this.TicketerId = this.member().string();
            this._names = this._name = "ProductByTicketer_TotalByIssueDate";
            this._store = Luxena.db.ProductByTicketer_TotalByIssueDate;
            this._isDomainFunction = true;
            this._localizeTitle({ ru: "Динамика тикетированных услуг" });
        }
        return ProductByTicketer_TotalByIssueDateSemantic;
    })(ProductTotalByDateSemantic);
    Luxena.ProductByTicketer_TotalByIssueDateSemantic = ProductByTicketer_TotalByIssueDateSemantic;
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
            /** Сотрудник */
            this.Employee = this.entity(new EmployeeSemantic());
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
            /** Начальный остаток */
            this.OpeningBalance = this.entity(new OpeningBalanceSemantic());
            /** Заказ */
            this.Order = this.entity(new OrderSemantic());
            /** Баланс */
            this.OrderBalance = this.entity(new OrderBalanceSemantic());
            this.OrderBalanceParams = this.entity(new OrderBalanceParamsSemantic());
            /** Динамика заказов ответственного */
            this.OrderByAssignedTo_TotalByIssueDate = this.entity(new OrderByAssignedTo_TotalByIssueDateSemantic());
            /** Динамика заказов заказчика */
            this.OrderByCustomer_TotalByIssueDate = this.entity(new OrderByCustomer_TotalByIssueDateSemantic());
            /** Динамика заказов владельца */
            this.OrderByOwner_TotalByIssueDate = this.entity(new OrderByOwner_TotalByIssueDateSemantic());
            /** Чек */
            this.OrderCheck = this.entity(new OrderCheckSemantic());
            /** Позиция заказа */
            this.OrderItem = this.entity(new OrderItemSemantic());
            this.OrderTotalByDate = this.entity(new OrderTotalByDateSemantic());
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
            /** Динамика забронированных услуг */
            this.ProductByBooker_TotalByIssueDate = this.entity(new ProductByBooker_TotalByIssueDateSemantic());
            /** Динамика поставленных услуг */
            this.ProductByProvider_TotalByIssueDate = this.entity(new ProductByProvider_TotalByIssueDateSemantic());
            /** Динамика проданных услуг */
            this.ProductBySeller_TotalByIssueDate = this.entity(new ProductBySeller_TotalByIssueDateSemantic());
            /** Динамика тикетированных услуг */
            this.ProductByTicketer_TotalByIssueDate = this.entity(new ProductByTicketer_TotalByIssueDateSemantic());
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
            this.ProductTotalByDate = this.entity(new ProductTotalByDateSemantic());
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
var Luxena;
(function (Luxena) {
    function sameTitleMenuItems(entities) {
        entities.forEach(function (se) { return se.titleMenuItems(entities); });
        return entities;
    }
    Luxena.config.menu = [
        {
            icon: "home",
            text: "Домашняя страница",
            onClick: "home",
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
        function renderMainMenuItem(data, index, containerEl) {
            containerEl.append($("\n<div class=\"main-menu-item\"" + (data.action ? " data-bind=\"dxAction: '#" + data.action + "'\"" : "") + " title=\"" + (data.title || data.text) + "\">\n\t<i class=\"fa fa-" + data.icon + " fa-3x\"></i>\n</div>"));
            //<br><small>${data.title || ""}</small>
        }
        function renderMainMenuSubItem(data, index, containerEl) {
            containerEl.append($("\n<div class=\"main-menu-subitem\">\n\t<i class=\"fa fa-" + data.icon + " fa-2x\"></i>\n\t<h4>" + (data.title || data.text) + "</h4>\n</div>"));
        }
        function renderMainMenuCompactSubItem(data, index, containerEl) {
            containerEl.append($("\n<div class=\"main-menu-subitem compact\">\n\t<i class=\"fa fa-" + data.icon + "\"></i>\n\t<h4>" + (data.title || data.text) + "</h4>\n</div>"));
        }
        //export function getToolMenuOptions(items: IMenuItem[], itemTemplate?, subItemTemplate?)
        //{
        //		items.forEach(a =>
        //		{
        //			if (!a.items) return;
        //			a.items.forEach(b =>
        //			{
        //				b.template = b.template || subItemTemplate || itemTemplate || renderMainMenuSubItem;
        //			});
        //		});
        //	return <DevExpress.ui.dxMenuOptions>{
        //		dataSource: items,
        //		showFirstSubmenuMode: "onHover",
        //		showSubmenuMode: "onHover",
        //		hideSubmenuOnMouseLeave: true,
        //		itemTemplate: itemTemplate || renderMainMenuSubItem,
        //		onItemClick: e =>
        //		{
        //			var item = <IMenuItem>e.itemData;
        //			item.onClick && item.onClick(e);
        //		},
        //	};
        //}
        function getMainMenuOptions() {
            var items = $.map(Luxena.config.menu, function (menu) {
                var item = $.extend({}, menu, {
                    template: renderMainMenuItem,
                    items: Luxena.toMenuItems(menu.items),
                });
                if (item.items)
                    item.items.forEach(function (b) { return b.template = b.template ||
                        (item.items.length <= 10 ? renderMainMenuSubItem : renderMainMenuCompactSubItem); });
                return item;
            });
            return {
                dataSource: items,
                showFirstSubmenuMode: "onHover",
                showSubmenuMode: "onHover",
                hideSubmenuOnMouseLeave: true,
                orientation: "vertical",
                onItemClick: function (e) {
                    var item = e.itemData;
                    item.onClick && item.onClick(e);
                },
            };
        }
        Layouts.getMainMenuOptions = getMainMenuOptions;
        function getFormAccordionToolbarOptions($data) {
            var se = $data.entity;
            var icon = $data.icon || se && se._icon;
            var iconHtml = icon && "<i class=\"fa fa-" + icon + "\"></i>&nbsp; " || "";
            var items = [
                {
                    location: "before",
                    html: "<div class=\"dx-toolbar-label\">" + iconHtml + ($data.title || se && se._titles || "") + "</div>"
                }
            ];
            var viewItems = $data.scope && $data.scope.viewToolbarItems && $data.scope.viewToolbarItems();
            if (viewItems && viewItems.length > 2)
                items.push.apply(items, viewItems.slice(2));
            return { items: items, };
        }
        Layouts.getFormAccordionToolbarOptions = getFormAccordionToolbarOptions;
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
    Luxena.$doForDerived(Luxena.sd.Product, function (se) {
        se.entityStatus(function (r) {
            if (se.IsVoid.get(r))
                return "disabled";
            return se.RequiresProcessing.get(r) === true ? "error" : null;
        });
        se.IssueDateAndReissueFor = se.row(Luxena.sd.col(se.IssueDate), Luxena.sd.col(se.ReissueFor));
        se.PassengerRow = se.row(se.GdsPassengerName, se.Passenger).title(se.Passenger);
        se.CustomerAndOrder = se.row(se.Customer, se.Order);
        se.StartAndFinishDate = se.row(se.StartDate, se.FinishDate);
        se.PnrAndTourCode = se.row(se.PnrCode, se.TourCode);
        se.BookerRow = se.row(se.Booker, se.BookerOffice, se.BookerCode).title(se.Booker);
        se.TicketerRow = se.row(se.Ticketer, se.TicketerOffice, se.TicketerCode).title(se.Ticketer);
        se.BookerAndTicketer = se.row(se.Booker, se.Ticketer);
        se.SellerAndOwner = se.row(se.Seller, se.Owner);
        se.Finance = se.col(se.Fare, se.EqualFare, se.FeesTotal, se.Total, se.Vat, se.Commission, se.CommissionDiscount, se.ServiceFee, se.Handling, se.Discount, 
        //a.BonusDiscount,
        //a.BonusAccumulation,
        se.GrandTotal, se.PaymentType, se.TaxRateOfProduct, se.TaxRateOfServiceFee);
        se.totalCharts1 = function (masterMember, chart) { return chart
            .chart(masterMember(se))
            .chartController({
            argument: chart.Date,
            value: [chart.SumGrandTotal, chart.GrandTotal,],
            zoom: true,
            chartOptions: {
                series: [
                    { pane: "0", },
                    { pane: "1", type: "bar", },
                ],
            },
        }); };
        se.totalGrid1 = function (masterMember) {
            return se.grid(masterMember && masterMember(se))
                .items(se.IssueDate, se.Type, se.Name, 
            //se.Total,
            //se.ServiceFee,
            se.GrandTotal, se.Order, se.RequiresProcessing.hidden())
                .gridController({ height: 550, useFilterRow: false });
        };
        se.totalTab1 = function (title, masterMember, chart) {
            return Luxena.sd.col().title(title).unlabelItems().items(Luxena.sd.header(chart), se.totalCharts1(masterMember, chart), Luxena.sd.hr2(), se.totalGrid1(masterMember));
        };
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
/// <reference path="../scripts/typings/devextreme/devextreme.d.ts" />
/// <reference path="../scripts/tsUnit/tsUnit.ts" />
/// <reference path="_support/Utils.ts" />
/// <reference path="_support/Extensions.ts" />
/// <reference path="_support/Log.ts" />
/// <reference path="_support/Semantic/SemanticObject.ts" />
/// <reference path="_support/Semantic/SemanticComponent.ts" />
/// <reference path="_support/Semantic/SemanticMember.ts" />
/// <reference path="_support/Semantic/SemanticFieldType.ts" />
/// <reference path="_support/Semantic/SemanticEntityAction.ts" />
/// <reference path="_support/Semantic/SemanticEntity.ts" />
/// <reference path="_support/Semantic/FieldTypes/_CollectionFieldType.ts" />
/// <reference path="_support/Semantic/FieldTypes/_CompositeFieldType.ts" />
/// <reference path="_support/Semantic/FieldTypes/Bool.ts" />
/// <reference path="_support/Semantic/FieldTypes/BoolSet.ts" />
/// <reference path="_support/Semantic/FieldTypes/DateTime.ts" />
/// <reference path="_support/Semantic/FieldTypes/Enum.ts" />
/// <reference path="_support/Semantic/FieldTypes/FieldColumn.ts" />
/// <reference path="_support/Semantic/FieldTypes/FieldRow.ts" />
/// <reference path="_support/Semantic/FieldTypes/Grid.ts" />
/// <reference path="_support/Semantic/FieldTypes/Money.ts" />
/// <reference path="_support/Semantic/FieldTypes/Numeric.ts" />
/// <reference path="_support/Semantic/FieldTypes/Lookup.ts" />
/// <reference path="_support/Semantic/FieldTypes/Text.ts" />
/// <reference path="_support/Semantic/Components/Container.ts" />
/// <reference path="_support/Semantic/Components/Card.ts" />
/// <reference path="_support/Semantic/Components/TabControl.ts" />
/// <reference path="_support/Semantic/Components/Accordion.ts" />
/// <reference path="_support/Semantic/Components/Button.ts" />
/// <reference path="_support/Semantic/Components/Field.ts" />
/// <reference path="_support/Semantic/Components/TabPanel.ts" />
/// <reference path="_support/Semantic/Components/Toolbar.ts" />
/// <reference path="_support/Semantic/SemanticDomain.ts" />
/// <reference path="_support/Semantic/SemanticController.ts" />
/// <reference path="_support/Semantic/Controllers/_CollectionController.ts" />
/// <reference path="_support/Semantic/Controllers/ChartController.ts" />
/// <reference path="_support/Semantic/Controllers/GridController.ts" />
/// <reference path="_support/Semantic/Controllers/FormController.ts" />
/// <reference path="_support/Semantic/Controllers/ViewFormController.ts" />
/// <reference path="_support/Semantic/Controllers/SmartFormController.ts" />
/// <reference path="_support/Semantic/Controllers/EditFormController.ts" />
/// <reference path="_support/Semantic/Controllers/FilterFormController.ts" />
/// <reference path="_support/Semantic/Controllers/registerEntityControllers.ts" />
/// <reference path="_support/Validators.ts" />
/// <reference path="_support/Ui.moneyProgress.ts" />
/// <reference path="Config.ts" />
/// <reference path="Domain.Entities.ts" />
/// <reference path="Domain.ts" />
/// <reference path="Semantics.Base.ts" />
/// <reference path="Semantics.ts" />
/// <reference path="Config.Menu.ts" />
/// <reference path="_support/layouts/Agent/AgentLayout.ts" />
/// <reference path="App.ts" />
/// <reference path="References/SimpleReferences.ts" />
/// <reference path="Products/Product.ts" />
var Luxena;
(function (Luxena) {
    var Components_;
    (function (Components_) {
        var Component$ = (function () {
            function Component$() {
                this.__name = this.toString();
            }
            Component$.prototype.uname = function () {
                return this._uname ||
                    (this._uname = (this["_name"] || "cmp") + "__" + Component$._unameIndex++);
            };
            Component$.prototype.toString = function () {
                var name = this._options && this._options.name;
                var className = classNameOf(this);
                return name
                    ? name + ": " + className
                    : "" + className;
            };
            Component$.prototype.options = function (setter) {
                if (setter) {
                    setter(this._optionsSetter);
                    this.__name = this.toString();
                }
                return this;
            };
            Component$.prototype.appendOptions = function (o) {
                if (o) {
                    $append(this._options, o);
                    this.__name = this.toString();
                }
                return this;
            };
            Component$.prototype.source = function (value) {
                this._dataSet = value;
                value._components.register(this);
                return this;
            };
            //clone()
            //{
            //	const clone = Object.create(this.constructor.prototype);
            //	// ReSharper disable once MissingHasOwnPropertyInForeach
            //	for (let attr in Object.getOwnPropertyNames(this.constructor.prototype))
            //	{
            //		const value = this[attr];
            //		if (value !== undefined)
            //			clone[attr] = value;
            //	}
            //	return clone;
            //}
            //getLength()
            //{
            //	return { length: this._options.length, min: <number>undefined, max: <number>undefined };
            //}
            //#region Data & Model
            Component$.prototype.data = function () {
                var ds = this._dataSet;
                return !ds ? undefined : ds._current;
            };
            Component$.prototype.loadOptions = function () {
                return undefined;
            };
            Component$.prototype.filterExpr = function (value, operation) {
                return undefined;
            };
            Component$.prototype.load = function () {
            };
            Component$.prototype.save = function () {
            };
            //removeFromData(data: any): void
            //{
            //}
            Component$.prototype.hasValue = function () {
                return undefined;
            };
            //#endregion
            //#region Widgets
            Component$.prototype.toGridColumns = function () {
                return [];
            };
            Component$.prototype.toGridTotalItems = function () {
                return [];
            };
            Component$.prototype.prepare = function () { };
            Component$.prototype.prepareItem = function (item) {
                if (!item._dataSet)
                    item._dataSet = this._dataSet;
                item.prepare();
            };
            Component$.prototype.repaint = function (parent) {
                $logb(this.__name + ".repaint(" + !!parent + ")");
                var oldElement = this._element;
                if (!oldElement || this._renderOnRepaint) {
                    if (!parent)
                        parent = this._parent;
                    else
                        this._parent = parent;
                    if (oldElement || parent) {
                        var newElement = this.render();
                        if (newElement)
                            if (oldElement) {
                                oldElement.replaceWith(newElement);
                                $log("replace");
                            }
                            else {
                                newElement.appendTo(parent);
                                $log("appendTo");
                            }
                        else if (oldElement)
                            oldElement.remove();
                        this._element = newElement;
                    }
                }
                else
                    this.load();
                $loge();
            };
            Component$.prototype.renderTo = function (parent) {
                this._parent = parent;
                var el = this._element = this.render();
                if (el) {
                    el.appendTo(parent);
                    $log("appendTo");
                }
                return el;
            };
            Component$.prototype.render = function () {
                return null;
            };
            Component$._unameIndex = 0;
            return Component$;
        })();
        Components_.Component$ = Component$;
        var InternalComponent = (function (_super) {
            __extends(InternalComponent, _super);
            function InternalComponent() {
                _super.call(this);
                this._options = new Components_.ComponentOptions();
                this._optionsSetter = new Components_.ComponentOptionsSetter(this._options);
            }
            return InternalComponent;
        })(Component$);
        Components_.InternalComponent = InternalComponent;
        function isComponent(a) {
            return a instanceof Component$;
        }
        Components_.isComponent = isComponent;
        function toComponent(item) {
            if (Luxena.Semantic.isMember(item))
                return item.fieldBox();
            return item;
        }
        Components_.toComponent = toComponent;
    })(Components_ = Luxena.Components_ || (Luxena.Components_ = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Components_;
    (function (Components_) {
        var ComponentOptions = (function () {
            function ComponentOptions() {
                this.visible = true;
                this.columnVisible = true;
                this.selectRequired = true;
            }
            return ComponentOptions;
        })();
        Components_.ComponentOptions = ComponentOptions;
        var ComponentOptionsSetter = (function () {
            function ComponentOptionsSetter(_options) {
                this._options = _options;
            }
            //calculate() { }
            //model()
            //{
            //	return this.__component._controller.model;
            //}
            ComponentOptionsSetter.prototype.name = function (value) {
                this._options.name = value;
                return this;
            };
            ComponentOptionsSetter.prototype.icon = function (value, title) {
                var op = this._options;
                if (value instanceof Luxena.SemanticObject)
                    op.icon = value._icon || "sticky-note-o";
                else if (value === "props")
                    op.icon = "sticky-note-o"; //"info"
                else
                    op.icon = value;
                if (title)
                    op.title = title;
                return this;
            };
            ComponentOptionsSetter.prototype._localizeTitle = function (value) {
                var op = this._options;
                var lng = Luxena.language;
                op.title = value[lng] || value.ru || op.title;
                op.titles = value[lng + "s"] || value.rus || op.titles;
                op.title2 = value[lng + "2"] || value.ru2 || op.title2;
                op.title5 = value[lng + "5"] || value.ru5 || op.title5;
                op.description = value[lng + "Desc"] || value.ruDesc || op.description;
                op.shortTitle = value[lng + "Short"] || value.ruShort || op.shortTitle;
            };
            ComponentOptionsSetter.prototype.localizeTitle = function (value) {
                this._localizeTitle(value);
                return this;
            };
            ComponentOptionsSetter.prototype.ru = function (value) {
                this._localizeTitle({ ru: value });
                return this;
            };
            ComponentOptionsSetter.prototype.en = function (value) {
                this._localizeTitle({ en: value });
                return this;
            };
            ComponentOptionsSetter.prototype.ua = function (value) {
                this._localizeTitle({ ua: value });
                return this;
            };
            ComponentOptionsSetter.prototype.title = function (title) {
                var o = this._options;
                if (isString(title))
                    o.title = title;
                else if (Luxena.Semantic.isMember(title) || Components_.isComponent(title)) {
                    o.title = title._options.title;
                    if (!o.icon)
                        o.icon = title._options.icon;
                }
                return this;
            };
            ComponentOptionsSetter.prototype.titles = function (se) {
                var op = this._options;
                op.icon = se._icon;
                op.title = se._titles;
                return this;
            };
            //titlePrefix(value: string)
            //{
            //	this._title = (value || "") + (this._title || "");
            //	return <TObject><any>this;
            //}
            //titlePostfix(value: string)
            //{
            //	this._title = (this._title || "") + (value || "");
            //	return <TObject><any>this;
            //}
            //otitle(): KnockoutComputed<string>;
            //otitle(getter: (model) => any): TOptions;
            //otitle(getter?: (model) => any): any
            //{
            //	if (getter)
            //	{
            //		this._titleGetter = getter;
            //		return this;
            //	}
            //	if (!this._titleGetter)
            //		return this._title;
            //	if (!this._otitle)
            //		return this._otitle = ko.computed(() => ko.unwrap(this._titleGetter(this.model())));
            //	return this._otitle;
            //}
            ComponentOptionsSetter.prototype.badge = function (value) {
                this._options.badge = value;
                return this;
            };
            //obadge(): KnockoutComputed<string>;
            //obadge(getter: (model) => any): TOptions;
            //obadge(getter?: (model) => any): any
            //{
            //	if (getter)
            //	{
            //		this._badgeGetter = getter;
            //		return this;
            //	}
            //	if (!this._badgeGetter)
            //		return this._badge;
            //	if (!this._obadge)
            //		return this._obadge = ko.computed(() => ko.unwrap(this._badgeGetter(this.model())));
            //	return this._obadge;
            //}
            ComponentOptionsSetter.prototype.description = function (value) {
                this._options.description = value;
                return this;
            };
            ComponentOptionsSetter.prototype.editMode = function (value) {
                this._options.editMode = value !== false;
                return this;
            };
            ComponentOptionsSetter.prototype.visible = function (value) {
                this._options.visible = value !== false;
                return this;
            };
            ComponentOptionsSetter.prototype.columnVisible = function (value) {
                this._options.columnVisible = value !== false;
                return this;
            };
            ComponentOptionsSetter.prototype.length = function (value) {
                this._options.length = value;
                return this;
            };
            return ComponentOptionsSetter;
        })();
        Components_.ComponentOptionsSetter = ComponentOptionsSetter;
    })(Components_ = Luxena.Components_ || (Luxena.Components_ = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Components_;
    (function (Components_) {
        //export type Container = Container$<any, any>;
        var Container$ = (function (_super) {
            __extends(Container$, _super);
            function Container$() {
                _super.apply(this, arguments);
            }
            Container$.prototype.items = function (itemsGetter) {
                this._itemsGetter = itemsGetter;
                return this;
            };
            //loadFromData(data: any): void
            //{
            //	this._items.forEach(a => a.loadFromData(data));
            //}
            //saveToData(data: any): void
            //{
            //	this._items.forEach(a => a.saveToData(data));
            //}
            //removeFromData(data: any): void
            //{
            //	this._items.forEach(a => a.removeFromData(data));
            //}
            Container$.prototype.loadOptions = function () {
                return Components_.DataSet.concatLoadOptions(this._items);
            };
            Container$.prototype.prepare = function () {
                var items = this._items = [];
                for (var _i = 0, _a = this._itemsGetter; _i < _a.length; _i++) {
                    var item_ = _a[_i];
                    var item = this.itemToComponent(item_);
                    items.push(item);
                    this.prepareItem(item);
                }
            };
            Container$.prototype.itemToComponent = function (item) {
                return Components_.toComponent(item);
            };
            Container$.prototype.render = function () {
                if (!this._items.length)
                    return null;
                var el = $div();
                var itemOptions = this._options.itemOptions;
                for (var _i = 0, _a = this._items; _i < _a.length; _i++) {
                    var item = _a[_i];
                    $extend(item._options, itemOptions);
                    item.renderTo(el);
                }
                return el;
            };
            return Container$;
        })(Components_.Component$);
        Components_.Container$ = Container$;
        var ContainerOptions = (function (_super) {
            __extends(ContainerOptions, _super);
            function ContainerOptions() {
                _super.apply(this, arguments);
                this.itemOptions = {};
            }
            return ContainerOptions;
        })(Components_.ComponentOptions);
        Components_.ContainerOptions = ContainerOptions;
        var ContainerOptionsSetter = (function (_super) {
            __extends(ContainerOptionsSetter, _super);
            function ContainerOptionsSetter() {
                _super.apply(this, arguments);
            }
            ContainerOptionsSetter.prototype.editMode = function (value) {
                var o = this._options;
                o.itemOptions.editMode = o.editMode = value !== false;
                return this;
            };
            return ContainerOptionsSetter;
        })(Components_.ComponentOptionsSetter);
        Components_.ContainerOptionsSetter = ContainerOptionsSetter;
        var Container = (function (_super) {
            __extends(Container, _super);
            function Container() {
                _super.apply(this, arguments);
            }
            return Container;
        })(Container$);
        Components_.Container = Container;
    })(Components_ = Luxena.Components_ || (Luxena.Components_ = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Components_;
    (function (Components_) {
        var View = (function (_super) {
            __extends(View, _super);
            function View() {
                _super.apply(this, arguments);
                this._dataSets = [];
            }
            View.prototype.dataSet = function (source) {
                var ds = new Components_.EntityDataSet(source);
                this._dataSets.push(ds);
                return ds;
            };
            View.prototype.prepare = function () {
                _super.prototype.prepare.call(this);
                this._dataSets.forEach(function (a) { return a.prepare(); });
            };
            //getRenderer()
            //{
            //	return (container: JQuery) =>
            //	{
            //		this.prepare();
            //		this.render(container);
            //		//this.loadFromData(null);
            //	}
            //}
            View.prototype.fieldSet = function () {
                return new Components_.FieldSet();
            };
            View.prototype.formField = function (field, setter) {
                return new Components_.FormField().field(field).options(setter);
            };
            View.prototype.grid = function () {
                return new Components_.Grid();
            };
            return View;
        })(Components_.Container$);
        Components_.View = View;
    })(Components_ = Luxena.Components_ || (Luxena.Components_ = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Components_;
    (function (Components_) {
        var DataSource = DevExpress.data.DataSource;
        var DataSet = (function () {
            function DataSet() {
                this._details = [];
                this._components = [];
            }
            DataSet.prototype.dataSource = function () {
                return this._dataSource;
            };
            DataSet.prototype.prepare = function () {
                this._components.forEach(function (a) { return a.prepare(); });
                var loadOptions = DataSet.concatLoadOptions(this._components);
                //$log(loadOptions);
                var ds = this._dataSource;
                ds.select(loadOptions.select);
                ds.loadOptions().expand = loadOptions.expand;
            };
            DataSet.concatLoadOptions = function (components) {
                if (!components)
                    return undefined;
                var select = ["Id", "Version"]; //TODO
                var expand = [];
                for (var _i = 0; _i < components.length; _i++) {
                    var cmp = components[_i];
                    var opt = cmp.loadOptions();
                    if (!opt)
                        continue;
                    select.register(opt.select);
                    expand.register(opt.expand);
                }
                return {
                    select: select,
                    expand: expand,
                };
            };
            DataSet.prototype.detail = function (source, filterSource) {
                this._details.push({ source: source, filterSource: filterSource });
                return this;
            };
            DataSet.prototype.current = function () {
                //if (data === undefined)
                return this._current;
                //return this;
            };
            DataSet.prototype.filter = function (value) {
                return true;
            };
            DataSet.prototype.select = function (data, container) {
                if (!data) {
                    this._selecteds = null;
                    this._current = null;
                }
                else if (isArray(data)) {
                    this._selecteds = data;
                    this._current = data.length ? data[0] : null;
                }
                else {
                    this._selecteds = [data];
                    this._current = data;
                }
                this.repaint(container);
            };
            DataSet.prototype.repaint = function (container) {
                this._components.forEach(function (a) { return a.repaint(container); });
            };
            return DataSet;
        })();
        Components_.DataSet = DataSet;
        var EntityDataSet = (function (_super) {
            __extends(EntityDataSet, _super);
            function EntityDataSet(_entity) {
                _super.call(this);
                this._entity = _entity;
                this._dataSource = new DataSource({
                    store: this._entity._store,
                });
            }
            EntityDataSet.prototype.load = function (id) {
                var _this = this;
                /*this._entity._lookupFields.id*/
                this._dataSource.filter(["Id", "=", id]);
                return this._dataSource.load().done(function (data) { return _this.select(data); });
            };
            EntityDataSet.prototype.update = function () {
                return null;
            };
            EntityDataSet.prototype.delete = function () {
                return null;
            };
            return EntityDataSet;
        })(DataSet);
        Components_.EntityDataSet = EntityDataSet;
    })(Components_ = Luxena.Components_ || (Luxena.Components_ = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Components_;
    (function (Components_) {
        var FieldBox$ = (function (_super) {
            __extends(FieldBox$, _super);
            function FieldBox$() {
                _super.apply(this, arguments);
            }
            //#region Render
            //prepare()
            //{
            //	super.prepare();
            //	this._renderOnRepaint = !this._options.editMode;
            //}
            FieldBox$.prototype.render = function () {
                var _this = this;
                return $logb(this.__name + ".render()", function () {
                    if (_this._options.editMode)
                        return _this.renderEditor();
                    else
                        return _this.renderDisplay();
                });
            };
            FieldBox$.prototype.renderDisplay = function () { return undefined; };
            FieldBox$.prototype.renderEditor = function () { return undefined; };
            FieldBox$.prototype.toGridColumns = function () {
                return [this.toStdGridColumn()];
            };
            FieldBox$.prototype.toStdGridColumn = function () {
                var o = this._options;
                var col = {
                    allowFiltering: o.allowFiltering,
                    allowGrouping: o.allowGrouping,
                    allowSorting: o.allowSorting,
                    caption: o.title,
                    dataField: this._options.name,
                    dataType: this._dataType,
                };
                //col.cellTemplate = (cell: JQuery, cellInfo) =>
                //{
                //	this.renderDisplayStatic(sf, cell, cellInfo.data);
                //};
                return col;
            };
            FieldBox$.prototype.toGridTotalItems = function () {
                return [];
            };
            //#endregion
            //#region Data
            FieldBox$.prototype.loadOptions = function () {
                return {
                    select: [this._options.name],
                };
            };
            FieldBox$.prototype.filterExpr = function (value, operation) {
                return [this._options.name, operation || "=", value];
            };
            FieldBox$.prototype.dataValue = function (value) {
                var data = this._dataSet._current;
                if (!data)
                    return undefined;
                if (value === undefined)
                    return this.getDataValue(data, this._options.name);
                else {
                    this.setDataValue(data, this._options.name, value);
                    return undefined;
                }
            };
            FieldBox$.prototype.getDataValue = function (data, name) {
                return data[name];
            };
            FieldBox$.prototype.setDataValue = function (data, name, value) {
                data[name] = value;
            };
            //getFieldValue()
            //{
            //	if (this._editor)
            //		return this._editor.option("value");
            //	return undefined;
            //}
            //setFieldValue(value: any)
            //{
            //	if (this._display)
            //		this._display.html(this.valueToHtml(value));
            //	else if (this._editor)
            //		return this._editor.option("value", value);
            //}
            FieldBox$.prototype.valueToHtml = function (value) {
                return value === undefined || value === null ? "" : value + "";
            };
            //hasValue(): boolean
            //{
            //	if (this._display)
            //		return !!this._display.html();
            //	else if (this._editor)
            //		return !!this._editor.option("value");
            //	return undefined;
            //}
            //loadFromData(data: any): void
            //{
            //	const value = this.getDataValue(data);
            //	this.setFieldValue(value);
            //}
            //saveToData(data: any): void
            //{
            //	const value = this.getFieldValue();
            //	this.setDataValue(data, value);
            //}
            FieldBox$.prototype.removeFromData = function (data) {
                delete data[this._options.name];
            };
            return FieldBox$;
        })(Components_.Component$);
        Components_.FieldBox$ = FieldBox$;
    })(Components_ = Luxena.Components_ || (Luxena.Components_ = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Components_;
    (function (Components_) {
        var FieldBoxOptions = (function (_super) {
            __extends(FieldBoxOptions, _super);
            function FieldBoxOptions() {
                _super.apply(this, arguments);
            }
            return FieldBoxOptions;
        })(Components_.ComponentOptions);
        Components_.FieldBoxOptions = FieldBoxOptions;
        var FieldOptionsBoxSetter = (function (_super) {
            __extends(FieldOptionsBoxSetter, _super);
            function FieldOptionsBoxSetter() {
                _super.apply(this, arguments);
            }
            //calculate()
            //{
            //	super.calculate();
            //	if (!this.isCalculated)
            //	{
            //		this._allowFiltering = false;
            //		this._allowGrouping = false;
            //		this._allowSorting = false;
            //	}
            //}
            FieldOptionsBoxSetter.prototype.allowFiltering = function (value) {
                this._options.allowFiltering = value !== false;
                return this;
            };
            FieldOptionsBoxSetter.prototype.allowGrouping = function (value) {
                this._options.allowGrouping = value !== false;
                return this;
            };
            FieldOptionsBoxSetter.prototype.allowSorting = function (value) {
                this._options.allowSorting = value !== false;
                return this;
            };
            FieldOptionsBoxSetter.prototype.isCalculated = function (value) {
                this._options.isCalculated = value !== false;
                return this;
            };
            return FieldOptionsBoxSetter;
        })(Components_.ComponentOptionsSetter);
        Components_.FieldOptionsBoxSetter = FieldOptionsBoxSetter;
    })(Components_ = Luxena.Components_ || (Luxena.Components_ = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Components_;
    (function (Components_) {
        var BaseStringBox = (function (_super) {
            __extends(BaseStringBox, _super);
            function BaseStringBox() {
                _super.apply(this, arguments);
                this._dataType = "string";
            }
            BaseStringBox.prototype.setDataValue = function (data, value) {
                if (data)
                    data[this._options.name] = $clip(value);
            };
            return BaseStringBox;
        })(Components_.FieldBox$);
        Components_.BaseStringBox = BaseStringBox;
        var BaseStringBoxOptions = (function (_super) {
            __extends(BaseStringBoxOptions, _super);
            function BaseStringBoxOptions() {
                _super.apply(this, arguments);
            }
            return BaseStringBoxOptions;
        })(Components_.FieldBoxOptions);
        Components_.BaseStringBoxOptions = BaseStringBoxOptions;
        var BaseStringBoxOptionsSetter = (function (_super) {
            __extends(BaseStringBoxOptionsSetter, _super);
            function BaseStringBoxOptionsSetter() {
                _super.apply(this, arguments);
            }
            return BaseStringBoxOptionsSetter;
        })(Components_.FieldOptionsBoxSetter);
        Components_.BaseStringBoxOptionsSetter = BaseStringBoxOptionsSetter;
        var StringBox = (function (_super) {
            __extends(StringBox, _super);
            function StringBox() {
                _super.call(this);
                this._options = new StringBoxOptions();
                this._optionsSetter = new StringBoxOptionsSetter(this._options);
            }
            StringBox.prototype.renderDisplay = function () {
                var value = this.dataValue();
                return !value ? null : $("<span>" + value + "</span>");
            };
            StringBox.prototype.renderEditor = function () {
                var o = this._options;
                return $div().dxTextBox({
                    value: this.dataValue(),
                    placeholder: o.title,
                });
            };
            return StringBox;
        })(BaseStringBox);
        Components_.StringBox = StringBox;
        var StringBoxOptions = (function (_super) {
            __extends(StringBoxOptions, _super);
            function StringBoxOptions() {
                _super.apply(this, arguments);
            }
            return StringBoxOptions;
        })(BaseStringBoxOptions);
        Components_.StringBoxOptions = StringBoxOptions;
        var StringBoxOptionsSetter = (function (_super) {
            __extends(StringBoxOptionsSetter, _super);
            function StringBoxOptionsSetter() {
                _super.apply(this, arguments);
            }
            return StringBoxOptionsSetter;
        })(BaseStringBoxOptionsSetter);
        Components_.StringBoxOptionsSetter = StringBoxOptionsSetter;
    })(Components_ = Luxena.Components_ || (Luxena.Components_ = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Components_;
    (function (Components_) {
        //export type Container = Container$<any, any>;
        var FieldSet = (function (_super) {
            __extends(FieldSet, _super);
            //private _widget: JQuery;
            function FieldSet() {
                _super.call(this);
                this._options = new FieldSetOptions();
                this._optionsSetter = new FieldSetOptionsSetter(this._options);
            }
            FieldSet.prototype.prepare = function () {
                _super.prototype.prepare.call(this);
                this._renderOnRepaint = !this._options.editMode;
            };
            FieldSet.prototype.itemToComponent = function (item) {
                if (Luxena.Semantic.isMember(item))
                    return new Components_.FormField().field(item);
                if (isString(item))
                    return new FieldSetHeader().options(function (o) { return o.title(item); });
                return item;
            };
            FieldSet.prototype.render = function () {
                var _this = this;
                return $logb(this.__name + ".render()", function () {
                    if (!_this.data())
                        return null;
                    return _super.prototype.render.call(_this)
                        .addClass("dx-fieldset");
                });
            };
            return FieldSet;
        })(Components_.Container$);
        Components_.FieldSet = FieldSet;
        var FieldSetOptions = (function (_super) {
            __extends(FieldSetOptions, _super);
            function FieldSetOptions() {
                _super.apply(this, arguments);
            }
            return FieldSetOptions;
        })(Components_.ContainerOptions);
        Components_.FieldSetOptions = FieldSetOptions;
        var FieldSetOptionsSetter = (function (_super) {
            __extends(FieldSetOptionsSetter, _super);
            function FieldSetOptionsSetter() {
                _super.apply(this, arguments);
            }
            return FieldSetOptionsSetter;
        })(Components_.ContainerOptionsSetter);
        Components_.FieldSetOptionsSetter = FieldSetOptionsSetter;
        var FieldSetHeader = (function (_super) {
            __extends(FieldSetHeader, _super);
            function FieldSetHeader() {
                _super.apply(this, arguments);
            }
            FieldSetHeader.prototype.render = function () {
                var _this = this;
                return $logb(this.__name + ".render()", function () {
                    var o = _this._options;
                    return $("<div class=\"dx-fieldset-header\">" + o.title + "</div>");
                });
            };
            return FieldSetHeader;
        })(Components_.InternalComponent);
    })(Components_ = Luxena.Components_ || (Luxena.Components_ = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Components_;
    (function (Components_) {
        var FormField = (function (_super) {
            __extends(FormField, _super);
            function FormField() {
                _super.call(this);
                this._options = new FormFieldOptions();
                this._optionsSetter = new FormFieldOptionsSetter(this._options);
            }
            FormField.prototype.field = function (fieldGetter) {
                this._fieldGetter = fieldGetter;
                return this;
            };
            //loadFromData(data: any): void
            //{
            //	this._field.loadFromData(data);
            //	if (this._display && !this._options.editMode)
            //		this._display.toggleClass("dx-state-invisible", !this._field.hasValue());
            //}
            //saveToData(data: any): void
            //{
            //	this._field.saveToData(data);
            //}
            //removeFromData(data: any): void
            //{
            //	this._field.removeFromData(data);
            //}
            FormField.prototype.loadOptions = function () {
                return this._field.loadOptions();
            };
            FormField.prototype.prepare = function () {
                var field = this._field = Components_.toComponent(this._fieldGetter);
                this.prepareItem(field);
                $append(this._options, field._options);
                this._renderOnRepaint = !this._options.editMode;
            };
            FormField.prototype.render = function () {
                var _this = this;
                return $logb(this.__name + ".render()", function () {
                    var o = _this._options;
                    var el = _this._display = $div("dx-field");
                    if (o.title)
                        el.append("<div class=\"dx-field-label\">" + o.title + "</div>");
                    var valueEl = $div(o.editMode ? "dx-field-value" : "dx-field-value-static").appendTo(el);
                    if (!_this._field.renderTo(valueEl))
                        return null;
                    return el;
                });
            };
            return FormField;
        })(Components_.Component$);
        Components_.FormField = FormField;
        var FormFieldOptions = (function (_super) {
            __extends(FormFieldOptions, _super);
            function FormFieldOptions() {
                _super.apply(this, arguments);
            }
            return FormFieldOptions;
        })(Components_.ComponentOptions);
        Components_.FormFieldOptions = FormFieldOptions;
        var FormFieldOptionsSetter = (function (_super) {
            __extends(FormFieldOptionsSetter, _super);
            function FormFieldOptionsSetter() {
                _super.apply(this, arguments);
            }
            return FormFieldOptionsSetter;
        })(Components_.ComponentOptionsSetter);
        Components_.FormFieldOptionsSetter = FormFieldOptionsSetter;
    })(Components_ = Luxena.Components_ || (Luxena.Components_ = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Components_;
    (function (Components_) {
        var Grid = (function (_super) {
            __extends(Grid, _super);
            function Grid() {
                _super.call(this);
                this._columns = [];
                this._options = new Components_.GridOptions();
                this._optionsSetter = new Components_.GridOptionsSetter(this._options);
            }
            Grid.prototype.prepare = function () {
                _super.prototype.prepare.call(this);
                var o = this._options;
                if (!o.columns)
                    return;
                var columnFields = this._columnFields = [];
                var columns = this._columns = [];
                for (var _i = 0, _a = o.columns; _i < _a.length; _i++) {
                    var col = _a[_i];
                    var field = void 0;
                    if (Luxena.Semantic.isMember(col))
                        field = col.fieldBox();
                    else if (Components_.isComponent(col))
                        field = col;
                    else
                        field = new GridColumnField(col);
                    columnFields.push(field);
                    columns.push.apply(columns, field.toGridColumns());
                }
            };
            Grid.prototype.loadOptions = function () {
                return Components_.DataSet.concatLoadOptions(this._columnFields);
            };
            Grid.prototype.render = function () {
                var _this = this;
                var o = this._options;
                this._columns[0].sortOrder = "asc";
                var gridOptions = $.extend({
                    columns: this._columns,
                    dataSource: this._dataSet.dataSource(),
                    //allowColumnReordering: !o.columnsIsStatic,
                    allowColumnResizing: true,
                    //allowGrouping: o.useGrouping,
                    columnAutoWidth: true,
                    //columnChooser: { enabled: !o.columnsIsStatic },
                    //height: o.height,
                    hoverStateEnabled: true,
                    //rowAlternationEnabled: true,
                    showRowLines: true,
                    //showColumnHeaders: cfg.useHeader,
                    showColumnLines: false,
                    wordWrapEnabled: true,
                    pager: {
                        showInfo: true,
                        showNavigationButtons: true,
                    },
                    selection: {
                        allowSelectAll: false,
                        mode: "single",
                    },
                    scrolling: {
                        preloadEnabled: true,
                    },
                    //onRowClick: e => $alert(e),
                    onSelectionChanged: function (e) { return _this._dataSet.select(e.selectedRowsData); }
                }, o.gridOptions);
                return $div().dxDataGrid(gridOptions);
            };
            return Grid;
        })(Components_.Component$);
        Components_.Grid = Grid;
        var GridColumnField = (function (_super) {
            __extends(GridColumnField, _super);
            function GridColumnField(_column) {
                _super.call(this);
                this._column = _column;
            }
            GridColumnField.prototype.loadOptions = function () {
                return {
                    select: [this._column.dataField],
                };
            };
            GridColumnField.prototype.toGridColumns = function () {
                return [this._column];
            };
            return GridColumnField;
        })(Components_.InternalComponent);
    })(Components_ = Luxena.Components_ || (Luxena.Components_ = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Components_;
    (function (Components_) {
        var GridOptions = (function (_super) {
            __extends(GridOptions, _super);
            function GridOptions() {
                _super.apply(this, arguments);
            }
            return GridOptions;
        })(Components_.ComponentOptions);
        Components_.GridOptions = GridOptions;
        var GridOptionsSetter = (function (_super) {
            __extends(GridOptionsSetter, _super);
            function GridOptionsSetter() {
                _super.apply(this, arguments);
            }
            GridOptionsSetter.prototype.columns = function (value) {
                this._options.columns = value;
                return this;
            };
            GridOptionsSetter.prototype.gridOptions = function (value) {
                this._options.gridOptions = value;
                return this;
            };
            return GridOptionsSetter;
        })(Components_.ComponentOptionsSetter);
        Components_.GridOptionsSetter = GridOptionsSetter;
    })(Components_ = Luxena.Components_ || (Luxena.Components_ = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Semantic;
    (function (Semantic) {
        var Entity = (function () {
            function Entity() {
            }
            Entity.prototype.stringMember = function (name, optionsGetter) {
                var o = new Luxena.Components_.StringBoxOptions();
                o.name = name;
                var oSetter = new Luxena.Components_.StringBoxOptionsSetter(o);
                optionsGetter && optionsGetter(oSetter);
                var sm = new Semantic.Member$(o, function () { return new Luxena.Components_.StringBox(); });
                sm._entity = this;
                return sm;
            };
            return Entity;
        })();
        Semantic.Entity = Entity;
    })(Semantic = Luxena.Semantic || (Luxena.Semantic = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Semantic;
    (function (Semantic) {
        var Member$ = (function () {
            function Member$(_options, _newFieldBox) {
                this._options = _options;
                this._newFieldBox = _newFieldBox;
            }
            Member$.prototype.fieldBox = function (setter) {
                return this._newFieldBox()
                    .options(setter)
                    .appendOptions(this._options);
            };
            Member$.prototype.formField = function (formFieldSetter, fieldBoxSetter) {
                return new Luxena.Components_.FormField()
                    .field(this.fieldBox(fieldBoxSetter))
                    .options(formFieldSetter);
            };
            return Member$;
        })();
        Semantic.Member$ = Member$;
        function isMember(a) {
            return a instanceof Member$;
        }
        Semantic.isMember = isMember;
    })(Semantic = Luxena.Semantic || (Luxena.Semantic = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Entity = (function (_super) {
        __extends(Entity, _super);
        function Entity() {
            _super.apply(this, arguments);
            this.Id = this.stringMember("Id");
        }
        return Entity;
    })(Luxena.Semantic.Entity);
    Luxena.Entity = Entity;
    var Entity2 = (function (_super) {
        __extends(Entity2, _super);
        function Entity2() {
            _super.apply(this, arguments);
        }
        return Entity2;
    })(Entity);
    Luxena.Entity2 = Entity2;
    var Entity3 = (function (_super) {
        __extends(Entity3, _super);
        function Entity3() {
            _super.apply(this, arguments);
            this.Name = this.stringMember("Name", function (o) { return o.title("Название"); });
        }
        return Entity3;
    })(Entity2);
    Luxena.Entity3 = Entity3;
    var Party = (function (_super) {
        __extends(Party, _super);
        function Party() {
            _super.call(this);
            this.Phone1 = this.stringMember("Phone1", function (o) { return o.title("Телефон 1"); });
            this.Phone2 = this.stringMember("Phone2", function (o) { return o.title("Телефон 2"); });
            this.Email1 = this.stringMember("Email1", function (o) { return o.title("Email 1"); });
            this.Email2 = this.stringMember("Email2", function (o) { return o.title("Email 2"); });
            this._store = Luxena.db.Parties;
        }
        return Party;
    })(Entity3);
    Luxena.Party = Party;
    Luxena.sd_ = {
        Party: new Party(),
    };
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.Test16 = function () {
            var se1 = Luxena.sd_.Party;
            var v = new Luxena.Components_.View();
            var ds1 = v.dataSet(se1);
            var grid1 = v.grid().source(ds1).options(function (o) { return o
                .columns([
                //se1.Id,
                se1.Name,
                se1.Phone1,
                se1.Email1,
            ])
                .gridOptions({
                paging: { pageSize: 10, },
            })
                .title("qqqq"); });
            var viewPanel = v.fieldSet().items([
                "Выбранный контрагент",
                v.formField(se1.Name, function (o) { return o.title("Ф.И.О."); }),
                se1.Phone1.formField(function (o) { return o.title("Phone"); }),
            ]).source(ds1);
            var editPanel = v.fieldSet().items([
                "Выбранный контрагент",
                v.formField(se1.Name, function (o) { return o.title("Ф.И.О."); }),
                se1.Phone1.formField(function (o) { return o.title("Phone"); }),
            ]).source(ds1);
            //panel1.options(o => o.editMode());
            //v.items([grid1, panel1]);
            return {
                //renderer: v.getRenderer(),
                renderer: function (container) {
                    ds1.prepare();
                    ds1.repaint(container);
                }
            };
        };
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Components;
    (function (Components) {
        var Html = (function (_super) {
            __extends(Html, _super);
            function Html(_html) {
                _super.call(this);
                this._html = _html;
            }
            Html.prototype.render = function (container) {
                if (this._html)
                    container.append(this._html);
            };
            return Html;
        })(Luxena.SemanticComponent);
        Components.Html = Html;
    })(Components = Luxena.Components || (Luxena.Components = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var FieldTypes;
    (function (FieldTypes) {
        var Chart = (function (_super) {
            __extends(Chart, _super);
            function Chart() {
                _super.apply(this, arguments);
                this._chartMode = true;
                this._isComposite = true;
            }
            Chart.prototype.render = function (sf, valueEl, rowEl) {
                var cfg = this.getControllerConfig(sf, "ChartController", { fixed: true });
                var scope = new Luxena.ChartController(cfg).getScope();
                sf._controller.widgets[sf.uname()] = scope;
                valueEl.append("<div data-bind=\"dxChart: widgets." + sf.uname() + ".chartOptions\"></div>");
            };
            Chart.Chart = new Chart();
            return Chart;
        })(FieldTypes.CollectionFieldType);
        FieldTypes.Chart = Chart;
    })(FieldTypes = Luxena.FieldTypes || (Luxena.FieldTypes = {}));
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
                test.run(new tsUnit.TestRunLimiterRunAll());
                test.showResults($("#results")[0]);
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
                Luxena.sd.row(se.FromOrder, se.FromParty),
                Luxena.sd.row(se.ToOrder, se.ToParty),
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
    $do(Luxena.sd.Order, function (se) {
        se.entityStatus(function (r) {
            if (se.IsVoid.get(r))
                return "disabled";
            var totalDue = se.TotalDue.get(r);
            if (totalDue === undefined)
                return null;
            return totalDue && totalDue.Amount > 0 ? "error" : "success";
        });
        se.totalCharts1 = function (masterMember, chart) { return chart
            .chart(masterMember(se))
            .chartController({
            argument: chart.Date,
            value: [chart.SumTotal, chart.Total,],
            zoom: true,
            chartOptions: {
                series: [
                    { pane: "0", },
                    { pane: "1", type: "bar", },
                ],
            },
        }); };
        se.totalGrid1 = function (masterMember) {
            return se.grid(masterMember && masterMember(se))
                .items(se.Number, se.IssueDate, se.Total, se.TotalDue, se.AssignedTo)
                .gridController({ small: true, height: 550, });
        };
        se.totalTab1 = function (title, masterMember, chart) {
            return Luxena.sd.col().icon(se).title(title).unlabelItems().items(Luxena.sd.header(chart), se.totalCharts1(masterMember, chart), Luxena.sd.hr2(), se.totalGrid1(masterMember));
        };
    });
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
                //Ui.moneyProgress(se, se.Paid, se.Total),
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
            smart: ({
                "fields": [
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
                buttons: []
            }),
            smartConfig: {},
            //viewScope: ctrl => ({
            //	tabs: [
            //		se.Items.toGridTab(ctrl, a => [a.Position, a.Product, a.Text, a.GrandTotal, a.Consignment, ]),
            //		se.Payments.toGridTab(ctrl, a => [a.Date, a.Number, a.DocumentNumber, a.PostedOn, a.Payer, a.RegisteredBy, a.Amount, a.Note, ]),
            //		se.IncomingTransfers.toGridTab(ctrl, a => [a.Date, a.Number, a.FromOrder, a.FromParty, a.Amount]),
            //		se.OutgoingTransfers.toGridTab(ctrl, a => [a.Date, a.Number, a.ToOrder, a.ToParty, a.Amount]),
            //	]
            //}),
            edit: {
                "fields": [
                    se.IssueDate,
                    se.Number,
                    se.Customer,
                    se.BillTo,
                    se.ShipTo,
                    Luxena.sd.row(se.AssignedTo, se.Owner),
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
    Luxena.$doForDerived(Luxena.sd.Payment, function (se) {
        se.DateAndDocumentNumber = Luxena.sd.row(Luxena.sd.col(se.Date), Luxena.sd.col(se.DocumentNumber));
        se.OrderAndPayer = Luxena.sd.row(se.Order, se.Payer);
        se.AmountAndVat = Luxena.sd.row(se.Amount, se.Vat);
        se.AssignedToAndOwner = Luxena.sd.row(se.AssignedTo, se.Owner);
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
                    Luxena.sd.row(se.Void, se.Unvoid),
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
                    Luxena.sd.row(se.Void, se.Unvoid),
                    se.IsVoid,
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
                    se.Seller.reserved(),
                    se.PassengerName.reserved(),
                    se.Itinerary,
                    se.StartDate.reserved(),
                    se.FinishDate.reserved(),
                    se.Country.reserved(),
                    se.Fare.reserved(),
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
                    se.Order.reserved(),
                    se.Invoice.reserved(),
                    se.CompletionCertificate.reserved(),
                    se.Payment.reserved(),
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
                    titleMenuItems: Luxena.toMenuItems(se.getTitleMenuItems()),
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
                Luxena.sd.ProductTotalByMonth.openList();
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
                Luxena.sd.ProductTotalByMonth.openList();
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
                Luxena.sd.ProductTotalBySeller.openList();
            }
        });
        registerProductTotalController(Luxena.sd.ProductTotalByDay, {
            argumentField: function (se) { return se.IssueDate; },
            members: function (se) { return [se.IssueDate, se.Total, se.ServiceFee, se.GrandTotal, se.Note,]; },
            onPointClick: function (filterCtrl, arg) {
                filterCtrl.modelValue(fse.MinIssueDate, arg);
                filterCtrl.modelValue(fse.MaxIssueDate, arg);
                Luxena.sd.ProductTotalBySeller.openList();
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
                    Luxena.sd.ProductTotalByMonth.openList();
            }
        });
        registerProductTotalController(Luxena.sd.ProductTotalByProvider, {
            argumentField: function (se) { return se.ProviderName; },
            tagField: function (se) { return se.Provider; },
            rotated: true,
            members: function (se) { return [se.Rank, se.Provider, se.Total, se.ServiceFee, se.GrandTotal, se.Note,]; },
            onPointClick: function (filterCtrl, arg, tag) {
                filterCtrl.modelValue(fse.Provider, tag.Id),
                    Luxena.sd.ProductTotalByMonth.openList();
            }
        });
        registerProductTotalController(Luxena.sd.ProductTotalBySeller, {
            argumentField: function (se) { return se.SellerName; },
            tagField: function (se) { return se.Seller; },
            rotated: true,
            members: function (se) { return [se.Rank, se.Seller, se.Total, se.ServiceFee, se.GrandTotal, se.Note,]; },
            onPointClick: function (filterCtrl, arg, tag) {
                filterCtrl.modelValue(fse.Seller, tag.Id),
                    Luxena.sd.ProductTotalByMonth.openList();
            }
        });
        registerProductTotalController(Luxena.sd.ProductTotalByBooker, {
            argumentField: function (se) { return se.BookerName; },
            tagField: function (se) { return se.Booker; },
            rotated: true,
            members: function (se) { return [se.Rank, se.Booker, se.Total, se.ServiceFee, se.GrandTotal, se.Note,]; },
            onPointClick: function (filterCtrl, arg, tag) {
                filterCtrl.modelValue(fse.Booker, tag.Id),
                    Luxena.sd.ProductTotalByMonth.openList();
            }
        });
        registerProductTotalController(Luxena.sd.ProductTotalByOwner, {
            argumentField: function (se) { return se.OwnerName; },
            tagField: function (se) { return se.Owner; },
            rotated: true,
            members: function (se) { return [se.Rank, se.Owner, se.Total, se.ServiceFee, se.GrandTotal, se.Note,]; },
            onPointClick: function (filterCtrl, arg, tag) {
                filterCtrl.modelValue(fse.Owner, tag.Id),
                    Luxena.sd.ProductTotalByMonth.openList();
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
            ],
            edit: [
                se.Person,
                Luxena.sd.hr(),
                se.Name,
                se.NewPassword,
                se.ConfirmPassword,
                Luxena.sd.er(),
                se.Roles,
                Luxena.sd.er(),
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
            form: function () { return Luxena.sd.tabCard(Luxena.sd.col().title("Турагенство").items(se.Company, se.CompanyDetails, se.AccountantDisplayString, se.Country, se.DefaultCurrency, se.UseDefaultCurrencyForInput, se.VatRate), Luxena.sd.col().titleForList(Luxena.sd.Product).items(se.UseAviaHandling, se.IsPassengerPassportRequired, se.AviaDocumentVatOptions, se.NeutralAirlineCode), Luxena.sd.col()
                .titleForList(Luxena.sd.Order)
                .addRowClass("field-label-width-300")
                .items(se.AviaOrderItemGenerationOption, se.AmadeusRizUsingMode, se.IncomingCashOrderCorrespondentAccount, se.DaysBeforeDeparture, se.MetricsFromDate, se.UseAviaDocumentVatInOrder, se.AllowAgentSetOrderVat, se.SeparateDocumentAccess, se.IsOrderRequiredForProcessedDocument, se.ReservationsInOfficeMetrics, se.McoRequiresDescription, se.Order_UseServiceFeeOnlyInVat), Luxena.sd.col().titleForList(Luxena.sd.Invoice).items(se.Invoice_NumberMode, se.InvoicePrinter_FooterDetails), Luxena.sd.col().title("Прочее").items(se.BirthdayTaskResponsible, se.IsOrganizationCodeRequired)); },
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
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.Agent, function (se) { return ({
            list: function () { return [
                se.PartyTags.field().compact().width(90),
                se.Name,
                se.Title,
                se.Contacts,
                se.Addresses,
            ]; },
            view: function () { return Luxena.sd.tabPanel().card().items(
            //sd.Product.totalGrid1(se => se.Seller),
            Luxena.sd.col().icon(se).items(se.NameForDocuments.field().header3(), se.Title.field().unlabel(), Luxena.sd.hr(), Luxena.sd.row(Luxena.sd.col(se.PartyTags.field().unlabel(), se.Organization, se.ReportsTo, se.Birthday, se.Note.field().labelAsHeader()).length(7), Luxena.sd.col(se.Contacts.clone().labelAsHeader().unlabelItems(), se.Addresses.clone().labelAsHeaderItems()).length(5)), Luxena.sd.hr2(), Luxena.sd.gheader(Luxena.sd.GdsAgent._titles), se.GdsAgents.field().unlabel()
                .items(function (se) { return [se.Origin, se.Code, se.OfficeCode, se.Office,]; }), //.gridController({ inline: true, useExport: false }),
            Luxena.sd.hr2(), Luxena.sd.gheader(Luxena.sd.User._titles), $as(Luxena.sd.User, function (se) { return se.grid(se.Person).unlabel()
                .items(function () { return [se.Name, se.Active]; }); })), Luxena.sd.Product.totalTab1("Продано", function (se) { return se.Seller; }, Luxena.sd.ProductBySeller_TotalByIssueDate), Luxena.sd.Product.totalTab1("Забронировано", function (se) { return se.Booker; }, Luxena.sd.ProductByBooker_TotalByIssueDate), Luxena.sd.Product.totalTab1("Тикетировано", function (se) { return se.Ticketer; }, Luxena.sd.ProductByTicketer_TotalByIssueDate), 
            ////se.StatisticsTab.clone()
            se.HistoryTab.clone()); },
            //view: {
            //	"fields1": [
            //		se.Name,
            //		se.LegalName,
            //		se.Title,
            //		se.Organization,
            //		se.ReportsTo,
            //	],
            //	"fields2": [
            //		se.Note,
            //	],
            //	"Contacts1": se.Contacts,
            //	"Contacts2": se.Addresses,
            //},
            viewScope: function (ctrl) { return ({
                tabs: []
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
                tabs: []
            }); },
            editScope: function (ctrl) { return ({
                tabs: []
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
        Views.registerEntityControllers(Luxena.sd.Department, function (se) { return ({
            list: function () { return [
                se.PartyTags.field().compact().width(90),
                se.Name,
                se.Organization,
                se.Contacts,
                se.Addresses,
            ]; },
            view: function () {
                //#region mainTab
                var mainTab = Luxena.sd.col().icon(se).items(se.NameForDocuments.header3(), se.Organization.header4(), Luxena.sd.hr(), Luxena.sd.row(Luxena.sd.col(se.PartyTags.field().unlabel(), se.ReportsTo, se.DefaultBankAccount, se.Note.field().labelAsHeader()).length(7), Luxena.sd.col(se.Contacts.clone().labelAsHeader().unlabelItems(), se.Addresses.clone().labelAsHeaderItems()).length(5)));
                //#endregion
                return Luxena.sd.tabPanel().card().items(mainTab, 
                //se.StatisticsTab.clone()
                se.OrderedTab.clone(), se.BalanceTab.clone(), se.ProvidedProductTab.clone(), se.HistoryTab.clone());
            },
            smart: function () { return Luxena.sd.tabPanel(Luxena.sd.col().icon(se).items(se.Name.header2().icon(se), se.LegalName.header3(), Luxena.sd.er(), Luxena.sd.row(se.PartyTags.field().unlabel().length(5), se.Contacts.unlabelItems().length(7))), //.height(200),
            se.HistoryTab.clone()); },
            smartConfig: {
                contentWidth: 600
            },
            edit: function () { return Luxena.sd.tabPanel().card().items(Luxena.sd.col().title("Общее").items(se.Name, se.LegalName, se.PartyTags, se.Organization, se.ReportsTo, se.DefaultBankAccount, se.Note.lineCount(6).field().labelAsHeader()), Luxena.sd.col().title(se.Contacts).items(se.EditContacts.clone(), se.Addresses.clone().labelAsHeaderItems())); }
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.MilesCard, function (se) { return ({
            list: [se.Number, se.Owner, se.Organization,],
            form: [se.Owner, se.Number, se.Organization,],
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    $do(Luxena.sd.Organization, function (se) {
        se.Tags = se.member()
            .title("Тэги")
            .name("Tags")
            .boolSet([
            se.IsCustomer,
            se.IsSupplier,
            se.IsAccommodationProvider,
            se.IsAirline,
            se.IsBusTicketProvider,
            se.IsCarRentalProvider,
            se.IsGenericProductProvider,
            se.IsInsuranceCompany,
            se.IsPasteboardProvider,
            se.IsRoamingOperator,
            se.IsTourProvider,
            se.IsTransferProvider,
        ]);
        se.DepartmentsTab = se.Departments.field().unlabel()
            .badge(function (r) { return se.DepartmentCount.get(r); })
            .items(function (se) { return [se.Name, se.Contacts]; })
            .dependencies(se.DepartmentCount)
            .gridController({ inline: true, useSearch: true, });
        se.EmployeesTab = se.Employees.field().unlabel()
            .badge(function (r) { return se.EmployeeCount.get(r); })
            .items(function (se) { return [se.Name, se.Title, se.Contacts]; })
            .dependencies(se.EmployeeCount)
            .gridController({ inline: true, useSearch: true, });
    });
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.Organization, function (se) { return ({
            list: function () { return [
                se.Tags.field().compact().width(90),
                se.Name,
                se.LegalName,
                se.Contacts,
                se.Addresses,
            ]; },
            view: function () {
                //#region mainTab
                var mainTab = Luxena.sd.col().icon(se).items(se.NameForDocuments.field().header3(), Luxena.sd.hr(), Luxena.sd.row(Luxena.sd.col(se.Tags.field().unlabel(), se.Code, 
                //se.ReportsTo,
                se.DefaultBankAccount, se.Note.field().labelAsHeader(), Luxena.sd.col(se.IsAirline.field().header5(), Luxena.sd.col(se.AirlineIataCode, se.AirlinePrefixCode, se.AirlinePassportRequirement).indentLabelItems())).length(7), Luxena.sd.col(se.Contacts.clone().labelAsHeader().unlabelItems(), se.Addresses.clone().labelAsHeaderItems()).length(5)));
                //#endregion
                return Luxena.sd.tabPanel().card().items(mainTab, se.DepartmentsTab.clone(), se.EmployeesTab.clone(), 
                //se.StatisticsTab.clone()
                se.OrderedTab.clone(), se.BalanceTab.clone(), se.ProvidedProductTab.clone(), se.HistoryTab.clone());
            },
            smart: function () { return Luxena.sd.tabPanel(Luxena.sd.col().icon(se).items(se.Name.header2().icon(se), se.LegalName.header3(), Luxena.sd.er(), Luxena.sd.row(se.Tags.field().unlabel().length(5), se.Contacts.unlabelItems().length(7))), //.height(200),
            se.DepartmentsTab.clone(), se.EmployeesTab.clone(), se.HistoryTab.clone()); },
            smartConfig: {
                contentWidth: 600
            },
            edit: function () { return Luxena.sd.tabPanel().card().items(Luxena.sd.col().title("Общее").items(se.Name, se.LegalName, se.Tags, se.Code, 
            //se.ReportsTo,
            se.DefaultBankAccount, se.Note.lineCount(6).field().labelAsHeader()), Luxena.sd.col().title(se.Contacts).items(se.EditContacts.clone(), se.Addresses.clone().labelAsHeaderItems()), Luxena.sd.col().title("Провайдер услуг").items(se.IsAirline.field().header5(), Luxena.sd.col(se.AirlineIataCode, se.AirlinePrefixCode, se.AirlinePassportRequirement), Luxena.sd.hr2(), Luxena.sd.row(Luxena.sd.col(se.IsAccommodationProvider, se.IsBusTicketProvider, se.IsCarRentalProvider, se.IsPasteboardProvider, se.IsTourProvider).unlabelItems(), Luxena.sd.col(se.IsTransferProvider, se.IsGenericProductProvider, se.IsProvider, se.IsInsuranceCompany, se.IsRoamingOperator).unlabelItems()))); }
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    Luxena.$doForDerived(Luxena.sd.Party, function (se) {
        se.PartyTags = se.member()
            .title("Тэги")
            .name("PartyTags")
            .boolSet([
            se.IsCustomer,
            se.IsSupplier,
        ]);
        se.EditContacts = Luxena.sd.col()
            .name("Contacts")
            .title("Контакты")
            .items(se.row(se.Phone1, se.Phone2).title("Телефоны"), se.row(se.Email1, se.Email2).title("E-mail"), se.WebAddress, se.Fax);
        se.Contacts = Luxena.sd.col()
            .name("Contacts")
            .title("Контакты")
            .items(se.Phone1, se.Phone2, se.Email1, se.Email2, se.WebAddress, se.Fax);
        se.Addresses =
            Luxena.sd.col(se.ActualAddress, se.LegalAddress)
                .title("Адреса");
        se.OrderedTab = Luxena.sd.Order.totalTab1("Заказывал", function (se) { return se.Customer; }, Luxena.sd.OrderByCustomer_TotalByIssueDate);
        se.ProvidedProductTab = Luxena.sd.col().icon(Luxena.sd.Product).title("Поставил").unlabelItems().items(Luxena.sd.header(Luxena.sd.ProductByProvider_TotalByIssueDate), Luxena.sd.Product.totalCharts1(function (se) { return se.Provider; }, Luxena.sd.ProductByProvider_TotalByIssueDate), Luxena.sd.hr2(), $as(Luxena.sd.Product, function (se) { return se.totalGrid1()
            .gridController({
            filter: function (ctrl) { return [
                [[se.Provider, ctrl.masterId()], "or", [se.Producer, ctrl.masterId()]],
            ]; },
        }); }));
        se.BalanceTab = Luxena.sd.col().icon("balance-scale").title(Luxena.sd.OrderBalance).unlabelItems().items($as(Luxena.sd.OrderBalance, function (se) { return se
            .chart(se.Customer)
            .chartController({
            fixed: true,
            argument: se.Order,
            value: se.Balance,
            colorMode: Luxena.ChartColorMode.NegativePositive,
        }); }), Luxena.sd.er(), $as(Luxena.sd.OrderBalance, function (se) { return se
            .grid(se.Customer)
            .items([
            se.Order.fit().importent(),
            se.IssueDate,
            se.Currency,
            se.Delivered.totalSum(),
            se.Paid.totalSum(),
            se.Balance.sortOrder().importent().totalSum(),
        ])
            .gridController({
            edit: null,
            fixed: true,
            useSorting: false,
            useGrouping: false,
            usePager: false,
            entityStatus: function (r) { return r.Balance < 0 ? "error" : "success"; },
        }); }), Luxena.sd.hr2(), Luxena.sd.header(Luxena.sd.OpeningBalance._titles), $as(Luxena.sd.OpeningBalance, function (se) { return se.grid(se.Party); }));
        se.StatisticsTab = Luxena.sd.tabPanel().title("Статистика").icon("line-chart").items(se.BalanceTab.clone(), se.ProvidedProductTab.clone(), se.OrderedTab.clone());
    });
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.Party, function (se) { return ({
            list: [
                se.PartyTags.field().compact().width(90),
                se.Type,
                se.Name,
                se.Contacts,
                se.Addresses,
            ],
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
var Luxena;
(function (Luxena) {
    var Views;
    (function (Views) {
        Views.registerEntityControllers(Luxena.sd.Passport, function (se) { return ({
            list: [
                se.Number,
                se.Owner,
                se.Name,
                se.Citizenship,
                se.Birthday,
                se.Gender.field().compact().length(3),
                se.ExpiredOn,
            ],
            view: Luxena.sd.tabPanel(Luxena.sd.col().icon(se).items(se.Number.header2(), se.Owner.header3(), Luxena.sd.hr(), se.Name, se.Citizenship, se.Birthday, se.Gender, se.IssuedBy, se.ExpiredOn, se.Note.field()._labelAsHeader, Luxena.sd.hr(), se.AmadeusString, se.GalileoString), se.HistoryTab),
            edit: [
                se.Owner,
                se.Number,
                se.LastName,
                se.FirstName,
                se.MiddleName,
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
        Views.registerEntityControllers(Luxena.sd.Person, function (se) { return ({
            list: [
                se.PartyTags.field().compact().width(90),
                se.Name,
                se.Title,
                se.Contacts,
                se.Addresses,
            ],
            view: Luxena.sd.tabPanel().card().items(Luxena.sd.col().icon(se) /*otitle(r => r.Name)*/.items(
            //se.Name.hidden(),
            se.NameForDocuments.field().header3(), se.Title.field().unlabel(), Luxena.sd.hr(), Luxena.sd.row(Luxena.sd.col(se.PartyTags.field().unlabel(), se.Organization, se.ReportsTo, se.BonusCardNumber, se.DefaultBankAccount, se.Birthday, se.Note.field().labelAsHeader()).length(7), Luxena.sd.col(se.Contacts.clone().labelAsHeader().unlabelItems(), se.Addresses.clone().labelAsHeaderItems()).length(5)), Luxena.sd.hr2(), Luxena.sd.gheader(se.Passports), se.Passports.field().unlabel()
                .items(function (se) { return [se.Owner, se.Number, se.Name, se.Citizenship, se.ExpiredOn,]; }), //.gridController({ inline: true, useExport: false }),
            Luxena.sd.hr2(), Luxena.sd.gheader(se.MilesCards), se.MilesCards.field().unlabel()
                .items(function (se) { return [se.Number, se.Organization]; })), 
            ////se.StatisticsTab.clone()
            se.OrderedTab.clone(), se.BalanceTab.clone(), se.ProvidedProductTab.clone(), se.HistoryTab.clone()),
            smart: function () { return Luxena.sd.tabPanel(Luxena.sd.col().icon(se).items(se.Name.header2().icon(se), se.LegalName.header3(), Luxena.sd.er(), Luxena.sd.row(se.PartyTags.field().unlabel().length(5), se.Contacts.unlabelItems().length(7))), se.HistoryTab.clone()); },
            edit: function () { return Luxena.sd.tabPanel().card().items(Luxena.sd.col().icon(se).items(se.Name, se.LegalName, se.PartyTags, se.Organization, se.ReportsTo, se.DefaultBankAccount, se.Note.lineCount(6).field().labelAsHeader()), Luxena.sd.col().title(se.Contacts).items(se.EditContacts.clone(), se.Addresses.clone().labelAsHeaderItems())); }
        }); });
        Views.registerEntityControllers(Luxena.sd.Employee, function (se) { return ({
            list: [
                se.Organization.field().groupIndex(0),
                se.PartyTags.field().compact().width(90),
                se.Name,
                se.Title,
                se.Contacts,
                se.Addresses,
            ],
            form: Luxena.sd.Person,
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
                    Luxena.sd.row(se.HotelName, se.HotelOffice, se.HotelCode)
                        .title(se.HotelName),
                    Luxena.sd.row(se.PlacementName, se.PlacementOffice, se.PlacementCode)
                        .title(se.PlacementName),
                    Luxena.sd.row(Luxena.sd.col(se.AccommodationType), Luxena.sd.col(se.CateringType)),
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
    Luxena.$doForDerived(Luxena.sd.AviaDocument, function (se) {
        return se.NumberRow = se.row(se.AirlinePrefixCode, se.Number, se.Producer).title("Номер / Авиакомпания");
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
                    Luxena.sd.row(se.FullNumber, se.Producer),
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
                    Luxena.sd.row(Luxena.sd.col(se.PnrCode, se.TourCode), Luxena.sd.col(se.GdsPassportStatus, se.Originator)),
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
        Views.registerEntityControllers(Luxena.sd.AviaTicket, function (se) { return ({
            list: Luxena.sd.AviaDocument,
            formTitle: se.FullNumber,
            view: function () { return Luxena.sd.tabCard(Luxena.sd.col().icon(se).items(Luxena.sd.row(se.FullNumber, se.Producer).header3(), Luxena.sd.hr(), Luxena.sd.row(Luxena.sd.col(se.IssueDate, se.ReissueFor, se.PassengerRow, se.Itinerary, se.CustomerAndOrder, se.Intermediary, se.GdsPassportStatus, se.PnrCode, se.TourCode, se.Booker, se.Ticketer, se.SellerAndOwner, se.OriginalDocument).length(8), se.Finance.length(4)), Luxena.sd.hr2(), se.Note), se.HistoryTab.clone()); },
            edit: function () { return ({
                "fields1": [
                    se.IssueDateAndReissueFor,
                    se.NumberRow,
                    se.PassengerRow,
                    se.CustomerAndOrder,
                    se.Intermediary,
                    Luxena.sd.row(Luxena.sd.col(se.PnrCode, se.TourCode), Luxena.sd.col(se.GdsPassportStatus, se.Originator)),
                    se.BookerRow,
                    se.TicketerRow,
                    se.SellerAndOwner,
                ],
                "fields2": se.Finance,
                "fields3": se.Note,
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
                    Luxena.sd.row(se.DeparturePlace, se.DepartureDate, se.DepartureTime).title("Отправление"),
                    Luxena.sd.row(se.ArrivalPlace, se.ArrivalDate, se.ArrivalTime).title("Прибытие"),
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
                    Luxena.sd.row(se.DeparturePlace, se.DepartureDate, se.DepartureTime).title("Отправление"),
                    Luxena.sd.row(se.ArrivalPlace, se.ArrivalDate, se.ArrivalTime).title("Прибытие"),
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
                    Luxena.sd.row(se.FromAirportCode, se.FromAirport).title(se.FromAirport),
                    Luxena.sd.row(se.ToAirportCode, se.ToAirport).title(se.ToAirport),
                    se.Carrier,
                    Luxena.sd.row(se.FlightNumber, se.Seat),
                    Luxena.sd.row(se.ServiceClassCode, se.ServiceClass).title(se.ServiceClass),
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
                    Luxena.sd.row(se.Number, se.Provider),
                    se.PassengerRow,
                    se.CustomerAndOrder,
                    se.Intermediary,
                    Luxena.sd.row(se.DeparturePlace, se.DepartureDate, se.DepartureTime).title(se.DeparturePlace),
                    Luxena.sd.row(se.ArrivalPlace, se.ArrivalDate, se.ArrivalTime).title(se.ArrivalPlace),
                    Luxena.sd.row(Luxena.sd.col(se.ServiceClass, se.TrainNumber), Luxena.sd.col(se.CarNumber, se.SeatNumber)),
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
                    Luxena.sd.row(se.Number, se.Provider),
                    se.PassengerRow,
                    se.CustomerAndOrder,
                    se.Intermediary,
                    Luxena.sd.row(se.DeparturePlace, se.DepartureDate, se.DepartureTime).title(se.DeparturePlace),
                    Luxena.sd.row(se.ArrivalPlace, se.ArrivalDate, se.ArrivalTime).title(se.ArrivalPlace),
                    Luxena.sd.row(Luxena.sd.col(se.ServiceClass, se.TrainNumber), Luxena.sd.col(se.CarNumber, se.SeatNumber)),
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
                    Luxena.sd.row(se.HotelName, se.HotelOffice, se.HotelCode).title(se.HotelName),
                    Luxena.sd.row(se.PlacementName, se.PlacementOffice, se.PlacementCode).title(se.PlacementName),
                    Luxena.sd.row(Luxena.sd.col(se.AccommodationType), Luxena.sd.col(se.CateringType)),
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
            form: [
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
            list: [
                se.TwoCharCode,
                se.ThreeCharCode,
                se.Name,
            ],
            view: function () { return Luxena.sd.tabPanel().card().items(Luxena.sd.col().icon(se).items(se.Name.header2(), Luxena.sd.hr(), se.TwoCharCode, se.ThreeCharCode), se.Airports.field()
                .items(function (se) { return [
                se.Code, se.Name, se.Settlement,
            ]; })); },
            edit: [
                se.Name,
                se.TwoCharCode,
                se.ThreeCharCode,
            ],
        }); });
    })(Views = Luxena.Views || (Luxena.Views = {}));
})(Luxena || (Luxena = {}));
//# sourceMappingURL=_app.js.map