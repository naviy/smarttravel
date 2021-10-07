using System;
using System.Runtime.CompilerServices;

using KnockoutApi;

using LxnBase.Data;


namespace Luxena.Travel
{
	public class FileModel
	{
		[PreserveCase]
		public Observable<object> Id = Ko.Observable<object>();

		[PreserveCase]
		public Observable<string> FileName = Ko.Observable<string>();

		[PreserveCase]
		public Observable<Date> TimeStamp = Ko.Observable<Date>();

		[PreserveCase]
		public Observable<Reference> UploadedBy = Ko.Observable<Reference>();

		[PreserveCase]
		public Observable<Reference> Party = Ko.Observable<Reference>();
	}
}