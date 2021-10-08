Number._commaFormat = function Number$_commaFormat(number, groups, decimal, comma) {
    var decimalPart = null;
    var decimalIndex = number.indexOf(decimal);
    if (decimalIndex > 0) {
        decimalPart = number.substr(decimalIndex);
        number = number.substr(0, decimalIndex);
    }

    var negative = number.startsWith('-');
    if (negative) {
        number = number.substr(1);
    }

    var groupIndex = 0;
    var groupSize = groups[groupIndex];
    if (number.length < groupSize) {
        return (negative ? '-' : '') + (decimalPart ? number + decimalPart : number);
    }

    var index = number.length;
    var s = '';
    var done = false;
    while (!done) {
        var length = groupSize;
        var startIndex = index - length;
        if (startIndex < 0) {
            groupSize += startIndex;
            length += startIndex;
            startIndex = 0;
            done = true;
        }
        if (!length) {
            break;
        }

        var part = number.substr(startIndex, length);
        if (s.length) {
            s = part + comma + s;
        }
        else {
            s = part;
        }
        index -= length;

        if (groupIndex < groups.length - 1) {
            groupIndex++;
            groupSize = groups[groupIndex];
        }
    }

    if (negative) {
        s = '-' + s;
    }

    return decimalPart ? s + decimalPart : s;
}
