Supported formats:
docx - Microsoft Office Open XML document format
xlsx - Microsoft Office Open XML spreadsheet format
csv, txt - Text format (can be used for comma separated values - spreadsheet document)

Default datatypes (.NET/JVM):
DataSet/NA - special keyword: clone
DataTable/NA - special keyword: fixed
DataRow/NA
IEnumerable/Iterable - special keywords: clone, fixed
IDictionary/Map
object/Object - special keyword: all
IDataReader/ResultSet - special keyword: fixed

Keywords:
clone - used for cloning entire document. Templater will append current document with current content.
fixed - used in resizable objects (like table) when you don't want to resize that object. For example, you have table with fixed number of rows and want Templater to replace those IEnumerable values and replace all others with empty string
page - used for specifying resizing of the page range in docx. When tag is placed in table and you want to resize entire page, not number of rows in that table, use page to override default context resize
sheet - used for specifying resizing of the sheet in xlsx. If you want to resize sheet range you can place tag in header or use sheet metadata on tag which is used for resizing.
all - replaces all instances of selected tag with provided values. Useful when there is same tag on various places in document and Template is unable to conclude that they should all be replaced with single value
repeat - when same collection needs to be applied multiple times on a single document
header - for DataTable/ResultSet data types, include header during dynamic resize
page-break - when doing resize include page break between elements

Default plugin keywords:
format - if encountered on date value it will replace DateTime value with string value (short date string if time part is empty)
format(X) - replaces current value formatted by X argument (for example N2 for number with two decimals)
substring(n) - returns substring of provided values after n chars
substring(n,l) - returns substring of provided values after n chars with l length
padLeft(n) - append space from left to create string of at least n length
padLeft(n,c) - append char c from left to create string of at least n length
padRight(n) - append space from right to create string of at least n length
padRight(n,c) - append char c from right to create string of at least n length
join(X) - flattens array to create a string with X between (for example {1,2,3}.join(-) becomes 1-2-3)
hide - replaces current value with empty string
empty(X) - if value is null or empty (IEnumerable.length = 0) it will replace value with X
bool(yes,no) - boolean value will be converted to yes or no
bool(YES,NO,MAYBE) - boolean value will be converted to YES, NO or MAYBE
offset(D\:H:M) - DateTime value will be offsetted by parsed Timestamp (special sign : is escaped with \)
collapse - if value is null or empty (IEnumerable.length = 0) current context will be collapsed; tag will be removed

Special datatypes:
Image/Icon - convert to image (JVM version uses 72dpi)
XElement/Element - insert XML as is into Word document
IEnumerable<XElement>/Array[Element] - insert XMLs as is into Word document
DataTable/ResultSet - dynamic resize
Array ([,] or jagged with same dimensions) - dynamic resize
IList<IList<string>>, List<List<string>> (with same dimensions) - dynamic resize

PDF conversion:
Templater doesn't support PDF format, but other tools can be used to convert docx->PDF. Example of LibreOffice usage for PDF conversion:

C:\Program Files (x86)\LibreOffice 4\program\soffice.exe -norestore -nofirststartwizard -nologo -headless -convert-to pdf input-document.docx

will result in input-document.pdf file.
Please note that LibreOffice doesn't support full range of Microsoft Office features and as such have issues with very complicated documents. Best thing to do in that case is to tweak the document template in Word, until LibreOffice can display it like Word.