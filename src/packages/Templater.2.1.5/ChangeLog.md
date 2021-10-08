Templater change log
--------------------------

##2015-09-01 v2.1.5

####Map/Dictionary null value fix (.NET and JVM)

 * If null value was used inside Map/Dictionary Templater replaced the specific tag only once. Fixed logic for null values, so it behaves as for any other value. Same tag will be replaced multiple time when expected.

####Word dynamic resize improvements (.NET and JVM)

 * During dynamic resize all values were converted to string. If complex data type, such as Image was used, Templater didn't inject picture into the document, but rather only displayed a string value. Now Templater will respect special data types as long as it's not nested (dynamic resize type, eg. Object[][] will behave as string).

####Custom XML TransformerFactory (JVM)

 * Specific xml transformer factory can be specified with 'templater:TransformerFactory' system property. This is useful in scenarios when Java has embeded an old/custom version of Xalan and produces invalid documents (such as IBM Java 6).

####Excel image position bugfix  (.NET and JVM)

 * Excel images were displayed one row bellow the tag. Now they will be displayed at the row where the tag was defined.

##2015-07-29 v2.1.4

####Replace all logic improvements (.NET and JVM)

 * For certain data types replace all is the default behavior (such as maps/dictionaries). This only worked correctly when applied on root path. Templater will now carry along this information which will result in better replace all default logic. This should result in less usage of :all metadata workaround.

####Pushdown on dynamic resize (.NET and JVM)

 * Pushdown didn't work correctly if DataTable/arrays were passed to Templater low level API. It started pushing below the new size of the collection instead of the location of the tag.
 
##2015-07-08 v2.1.3

####Excel row height cloning (.NET and JVM)

 * If row has a custom height, it should always be copied. Previously Templater only copied height when there was no rows below template row. Now it should copy it on each resize.

####Context detection fixes in Excel (.NET and JVM)

 * Few inconsistencies between libraries have been resolved. JVM will now match behaviour of .NET in few cases when they differed (incorrect logic for context detection on pushdown)
 * Table will now be included in calculation during context detection. If a table stretches out of detected context (based on provided tags) context will be adjusted to new maximum size.

####Excel dimension update (.NET and JVM)

 * If dimension attribute exists in the sheet, it will be updated (at lest the height part).

##2015-06-16 v2.1.2

####Whitespace fixes (.NET and JVM)

 * Templater recognized empty space at the start or end of tag value and injected "preserve" attribute into Word. Not it will recognize all whitespace characters, not just empty space. Also, if multiline string was passed, Templater did not behave correctly. Now it will behave correctly even when multiline string is passed (which is converted to string parts with line breaks)

####Context detection changes (.NET and JVM)

 * Templater high level API injects special context information into tag so it can recognize which tags belong together. Now it will insert such metadata at the beginning instead of the end of tag metadata. This helps with some context detection scenarios.
 
##2015-05-21 v2.1.1

####Page break change (.NET and JVM)

 * Previously page break was inserted after each resize of the whole document in Word. Since section properties are cloned on each resize of the whole document this caused "two duplicate" page break instructions. Now page break will be added only if one of the found tags contains 'page-break' metadata. Cloning doesn't insert page break anymore. This allows for user to specify behaviour of next page by defining section break style which will be used.
 
####IEnumerable<XElement>/Element[] support in Word (.NET and JVM)

 * To ease the use of XML type usage, collections will now be recognized (IEnumerable in .NET and array in JVM). This simplifies HTML -> DOCX conversion since often multiple paragraphs need to be inserted after a single tag.

####Empty spreadsheet bugfix (.NET and JVM)

 * Templater will now work as expected when spreadsheet only has data in headers. Previously it crashed.

####:all metadata on DataTable/DataSet/ResultSet (.NET and JVM)

 * :all metadata is used to force Templater into replacing found tag everywhere in the document. This is useful when different sections contain same tag, but Templater is unable to conclude that it should replace specific tag. This feature will now work on other types as expected.
 
##2015-03-26 v2.1.0

####Resizing document with picture fix (.NET and JVM)

 * If picture was used within context which was resized, Templater didn't changed id to unique value. This caused document to be reported as corrupted.

####Prefix matching fix (.NET and JVM)

 * In dynamic data types (DataRow/DataTable/Dictionary) prefix was sometimes matched incorrectly. If two prefixes started with same values due to order of keys/columns Templater would ignore the second prefix if it started with first prefix. This is somewhat improved, but it can still happen if "." is used inside names. This fixes the problem where key "10" was ignored in dictionary since key "1" already existed.

####Macro enabled documents support (.NET and JVM)

 * docm and xlsm are now recognized and processed

####Hyperlink support in Word (.NET and JVM)

 * hyperlinks (url, email) and their tooltips will now be processed by Templater. When defining hyperlink tag Word will replace []/{} with escaped values, but that is expected.

####System.Xml.Linq.XElement/org.w3c.Element support in Word (.NET and JVM)

 * If XML element type is detected content will be injected "as is" after matching node name is found. Existing tag is replaced with empty string and node is matched starting from the existing location. Be careful with this feature since document can easily be corrupted.

##2015-03-03 v2.0.4

####Multiple rows resizing fix (.NET and JVM)

 * First context did not behave correctly if tag was used in multiple lines. Now Templater will replace same tag in entire first context.

##2015-02-10 v2.0.3

####Word regression fix (.NET)

 * Templater didn't filter out non headers/footers when cloning header/footer document part. Filter added back.

####Empty formatter change (JVM)

 * Empty formatter will now work on empty strings as .NET version does.

##2015-02-02 v2.0.2

####Excel bugfixes (.NET and JVM)

 * Formula rename fix. Support more scenarios for copying formula to new sheet.
 * When named ranges references table Templater would try to extract sheet name from such formula (but it doesn't exists). Now they will be ignored instead of trying to process it.
 * All copied named ranges are now global. Templater was assigning local sheet id to copied named ranges, now it will copy even local ranges to global (all ranges have unique random name).
 * Named range nesting change. Nested ranges which are inside currently processed range were always copied. Now they will be copied only if their height is smaller than the outer one. This allows for formulas on named ranges which should be resized instead of copied.

##2015-01-28 v2.0.1

####Word clone/resize header/footer fix (.NET and JVM)

 * On some documents header/footer info is contained inside document, not at the end. Templater didn't correctly copied/renamed info in that case.
 
##2015-01-26 v2.0.0

####New tag format (.NET and JVM)

 * [[tag]] and {{tag}} formats are now supported
 
####User registered plugins (.NET and JVM)

 * While Templater is heavily based on plugins, it allowed only for Enterprise users to use such feature. Now all users can benefit from plugin registration which should enable them to support all kind of customizations while using latest version of Templater.

####Improved type support (.NET and JVM)

 * Templater supports IDictionary/Map (with all keys as string type), but now it works in even more scenarios. Collection of dictionary/map is now supported as first level argument. Non string type keys are supported (if they are all strings) which means more types are recognized, such as Hashtable (used in PowerShell).
 * Previous .NET version supported non-generic IDictionary only, now IDictionary<string, object> is supported too
 * If Object[] is detected, Templater previously assumed Object type and couldn't do much with it. Now it will look into the collection for the actual type and will use that instead. Note that if a collection is empty Templater will not be able to resize the collection to 0 since it will not know which properties it needs to match.
 * Low level API support for jagged lists (List<List<String>> and IList<IList<string>> in .NET and List<List<?>> in Java and List[List[AnyRef]] in Scala). Templater supported jagged arrays so you could use String[][] for dynamic resize. To better support deserialized JSON support for list is added. Note that Json.NET will deserialize into it's own JToken/JArray so manual conversion will be required.

####Some/None support (Scala)

 * Content of Some is extracted so actual type is used.

####Simple spreadsheet formula clone support (.NET and JVM)

 * In Excel if another table or named range is referenced in a cell formula, during cloning Templater will rewrite that formula to 
use newly created table/named range. Only simple names are supported. This currently doesn't work in resize, but only in clone.

####Minor spreadsheet fixes (.NET and JVM)

 * Chart c:tx detection added.
 * Support for non-trivial sheet name. When formula is created in chart, fix reference to sheet which name is escaped.

####Resource license embedding (JVM)

 * Templater will also check project resources using current Thread class loader for license file. If default factory() or build() methods are used, Templater will run in unlicensed mode when license is not found. If license is not found when exact path is specified, an exception will be thrown

####ResultSet/DataSet bugfixes (.NET and JVM)

 * Every 1000th row was skipped. This is fixed in new version. Algorithm still uses resize in 1000 chunks, so performance is unaffected.
 * Tabled used by result set would end up with an empty row at the end. Tempalter is now more careful about resizing and will not resize it more than necessary.

####Word picture bugfix (.NET and JVM)

 * Unique picture numbering. Currently pictures embedded in header/footer and copied over to next page retained same picture id. Now they will always get a new unique id.

##2014-11-04 v1.9.9

####Internal memory stream fix (.NET)

 * .NET uses Seek to increase stream size. Fixed internal stream to cope with it.

##2014-10-27 v1.9.8

####Spreadsheet context detection improvements (.NET and JVM)

 * Major refactoring of spreadsheet context detection to support more scenarios without the use of named ranges and tables. Multiple groupings are now supported and properly detected.
 * Breaking change on resize behavior in spreadsheets. While previously it was enough to specify a single tag inside a named range, now Templater will behave differently whether all tags in named range are specified or only a subset of them are. If all tags are specified, named range will be used as a context, otherwise, best context spanning all specified tags will be used.

####Null image handling bugfix (JVM)

 * Null value for BufferedImage property will no longer throw NullReferenceException

##2014-10-25 v1.9.7

####Spreadsheet fast path bugfix (.NET and JVM)

 * Currently only one row is supported when fast path is activated. Templater did not check for row count which resulted in invalid row duplication.

##2014-09-27 v1.9.6

####Spreadsheet nested context regression fix (.NET and JVM)

 * It seems that a while ago nested contexts in spreadsheet stopped working correctly in some scenarios. Fixed bugs which caused it to stop working.
 
####Performance and memory usage improvements (.NET and JVM)

 * Various minor improvements which should reduce GC garbage on large documents. Prefer arrays over other data types, reuse string names when possible, few local caches and plain loops instead of functional expressions.

####Context replacement in collections (.NET and JVM)

 * While context was detected using base item type in collection (this only affects collections with different element types), processing of such collection recalculated properties for each item. Reuse properties used for context detection instead.

####Spreadsheet performance improvement (.NET and JVM)

 * Added fast path when resizing a single-row range (or table). If range is last on sheet it can use faster algorithm to resize itself

####XML parameter for deferred node processing (JVM)

 * DOM supports deferred and eager node processing. Default is deferred, but it's better to use eager for small documents. Parameter can be controled with global system properties key: 'templater:defer-node-expansion', for example System.getProperties.setProperty("templater:defer-node-expansion", "true")

##2014-09-16 v1.9.5

####Major spreadsheet performance optimizations (.NET and JVM)

 * When text is found beneath range/table which will be resized Templater will push it down. Pushdown algorithm optimized.
 * When table/range is resized, cell styles needs to be copied to all new rows. Copy algorithm optimized.

####Major document performance optimizations (.NET and JVM)

 * Table resize improvements. Critical part of resize algorithm improvements from O(n2) to O(nlogn). This can drastically improve large document generation (from 5 min -> 5 sec)

####Spreadsheet performance optimizations (.NET and JVM)

 * Faster internal cell comparison results in less memory usage and faster reports

####Memory leak fix (.NET 3.5)

 * Optimized loading of XML in .NET which fixes memory leak from usage of large strings (in large documents)

###2014-09-02 v1.9.4

####Document picture id fix (.NET and JVM)

 * OOXML format specifies that picture id inside document must be unique. While Word 2007 will not complain, Word 2010 will refuse to load such document. During page copying, Templater will copy pictures and change ids for newly copied pictures

##2014-08-17 v1.9.3

####Custom XML parser improvements (JVM)

 * Templater by default loads default XML parser. In some environments custom XML parser is injected as default parser which doesn't support all of the features required by Templater. Custom parser used by Templater can be specified with 'templater:DocumentBuilderFactory' system properties key. If invalid parser is detected an exception explaining how to handle this scenario is thrown

####Spreadsheet extLst element fix (.NET and JVM)

 * If image was inserted into document and custom extensions were defined in the document, Templater would corrupt xlsx document since it didn't respect correct ordering for elements.

####Spreadsheet size/performance improvements (.NET and JVM)

 * Cell content will be lazily copied instead of eagerly. This usually results in much smaller documents (if large number of static cells are copied)
 * Cell interaction improved. Optimize usage of strings when interacting with cell values

####Document clone changes (.NET and JVM)

 * If :all metadata is used along with :clone metadata, Templater will now respect it. Previously cloning document disabled replacement of multiple same tags. This is useful when same tag is used in different contexts, such as two different tables. Templater can be forced to replace both tags by specifying :all metadata.

####Document header/footer fix (.NET)

 * Header/footer relation was not copied. This was a problem if header/footer contained a picture or some reference which caused relation to be created alongside it

####Spreadsheet formula recalculation (.NET and JVM)

 * Spreadsheets with saved formula values will not be recalculated by default. If option specifying should they be calculated is missing, turn it on so that every formula is recalculated when document is opened. This feature can be disabled by specifying fullCalcOnLoad attribute in calcPr element as 0.

####Tags recognition changes and optimizations (.NET and JVM)

 * Tags now support ascii characters, numbers and few special characters: - + . , ! ?
 * Global cache for short tags (<128 chars) to improve parsing speed

####Property detection fix (.NET)

 * Ignore indexed properties. Since they require arguments, they can't be processed

##2014-05-30 v1.9.2

####Document spacing fix (.NET and JVM)

 * Since space is truncated in Word by default, add preserve space attribute to appropriate XML elements

##2014-03-13 v1.9.1

####Spreadsheet whole row detection (.NET and JVM)

 * Support for $X:$X format for whole row selection. Converted internally to $A$X:$B$Y format

####Visibility fix (JVM)

 * Skip non-public modifiers in JVM

####ResultSet/DataTable expansion fix (.NET and JVM)

 * Can't replace table header values, without replacing table column names. Skip replacing headers if table is not extended

####Bool metadata parsing fix (JVM)

 * Parse bool with ',' and '/' but not ',/'

####Proguard visibility fix (JVM)

 * Changed visibility of proguarded classes so they don't leak into IDE autocomplete feature

####Merge cell copy fix (.NET and JVM)

 * When range is selected as cell size, merge cells were not copied.

##2014-02-23 v1.9.0

####API change: input/output stream (.NET)

 * Support input/output stream in .NET as exists for JVM. This is standard way for interaction in web apps.

####Document performance improvements (.NET and JVM)

 * Use faster XML element comparison to reduce memory usage and improve speed.

####Deadlock issue fixed (.NET 3.5)

 * Added lock around global type properties cache. Since .NET 3.5 doesn't have support for concurrent cache, add explicit lock to avoid dictionary lock issue

####Specialized internal memory stream (.NET)

 * Since .NET suffers from LOH issues, added special memory stream which is used during internal processing

##2013-11-16 v1.8.2

####Java BigDecimal support (JVM)

 * Only Scala BigDecimal was supported. doubleValue or alternative methods had to be used to work around this.

####Performance improvements (JVM)

 * Switch to Java collections whenever appropriate. It seems Scala has large performance issues.
 * Date conversion optimization in spreadsheet. Since Excel has special date value, changed conversion algorithm to O(1) operation

####Concurrency issue fix (.NET 4.0)

 * Switched to concurrent dictionary in .NET 4.0

####Collapse metadata fix (.NET and JVM)

 * It seems that collapse needs special path to be able to work as expected. Moved collapse handler from formatters to special handlers group

##2013-09-05 v1.8.1

####Improved tag detection in documents (.NET and JVM)

 * Tags are usually spread over several text runs. Improve detection for correct start and end positions in them. Algorithm is still slightly based on heuristics, but it's much better now.

##2013-06-07 v1.8.0

####Zero arguments method fix (.NET and JVM)

 * Only methods with zero arguments are allowed to be picked up by Templater during scanning.
 * Added check in JVM to ignore methods which return void type

####Performance optimizations (.NET and JVM)

 * Cache properties picked up with reflection in global cache.

####Nested table fix (.NET and JVM)

 * Only topmost tables must be copied. When tables are embedded in tables, Templater must be careful when it should copy each table.

####Named range fix (.NET and JVM)

 * All named ranges must be copied, not just the first one found in context. This fixes strange behavior on spreadsheet without table in a context.

####Hierarchical dictionary support (JVM)

 * Improved Map processor to be able to support more complex scenarios. This is used on demo page on Templater website.

##2013-05-28 v1.7.6

####Context detection improvements (.NET and JVM)

 * Context metadata is added to cloned pages. This allows for collection to use resize and replace on same context for pages.
 * Renamed context identifier from _contextInfo to _ci

##2013-05-17 v1.7.5

####Resizing improvements in documents (.NET and JVM)

 * If all tags are inside single table (even in list inside a table) resize that table. While this fails if table exists inside list, it will work for most use cases.

####List numbering fix (.NET and JVM)

 * Copy with missing numbering attributes (which LibreOffice knows to omit).

##2013-04-18 v1.7.4

####Dynamic resize improvements (.NET and JVM)

 * Allow dynamic resize without table (only with range).
 * Fixed issue with tables without headers.

####Image size fix in spreadsheets (JVM)

 * Better image size detection. Use floating math for improved precision. JVM still assumes 72dpi for images.

##2013-02-19 v1.7.3

####Image size fix in documents (.NET and JVM)

 * Better image size detection. Use floating math for improved precision. JVM still assumes 72dpi for images.
 * Rounding number issue fixed in .NET.

####Object context detection (.NET and JVM)

 * When object processor starts he should try to replace all tags if prefix is empty. This fixes the problem with multiple tags on the same page and removes requirement for :all metadata (in most scenarios).

####Formula conversion fix (JVM)

 * Added conversion of [[equals]] to = (formula) in spreadsheets. This is an ugly hack to support formulas in spreadsheets, since current tag format is disallowed by Excel. Ideally Templater should support alternative tag format, such as {{tag}} with doesn't have issues in Excel.

####Clone flush fix (JVM)

 * Clones were not flushed. This made using them kind of useless.

##2012-12-13 v1.7.2

####Clone fix (.NET)

 * Last XML element was not copied during cloning.

##2012-11-30 v1.7.1

####Document header bugfix (JVM)

 * Wrong name was used for header.

####Collapse formatter (.NET and JVM)

 * Support collapsing of document regions on empty collections or null values. This allows for conditionals which hide part of the document with :collapse metadata.

####Word list resize fix (.NET and JVM)

 * List should resize event if doesn't have style defined (Word 2010 creates lists without style).

##2012-05-28 v1.7.0

####Dynamic resizing (.NET and JVM)

 * Recognize special data types such as DataTable, two dimensional/jagged arrays and handle them in a special way. Instead of just replacing predefined template with values in one dimension (down), resize detected object (table) in both dimensions. If :headers metadata is used, inject headers from DataTable too.

####Clone fix (.NET and JVM)

 * Fix numbers and names for copied objects (bookmarks and shapes). Since they must be unique, give them unique names.

####Context improvements (.NET and JVM)

 * Resize tables or lists only when they contain all of the specified tags. Otherwise fall back to page/sheet/document instead.
 * Support multi-row contexts in table/range. Context doesn't need to be single row anymore, since it will be tracked more precisely. All provided tags used in resize will decide context range.

####Formatter improvements (JVM)

 * Detect java.util.Date and redirect :format metadata arguments to SimpleDateFormat. Now :format(yyyy-MM-dd) will output expected result
 * Ignore String type for formatting

####Coping with malformed documents (.NET and JVM)

 * Excel doesn't remove named range from deleted sheet. Check if sheet exists during named range scanning.

####Metadata for fixed tables (.NET and JVM)

 * :table or :fixed metadata can be used to instruct Templater not to resize specified table. This is useful when table has predefined set of rows, with all rows filled with tags. Templater should not resize such tables, but it should remove/clear out missing rows (it will replace unmatched tags with empty string)

####Merge cell fix (.NET and JVM)

 * Merge cell needs to be stretched when they end after range which is resized.

####Performance optimizations (.NET and JVM)

 * Use better data structure in processors for improved performance.
 * Changed various Scala data types to Java data types to improve performance. Scala is really slow otherwise.