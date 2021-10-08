using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	public partial class File : Entity
	{

		[Required]
		protected Party _Party;

		public string FileName { get; set; }

		public DateTime TimeStamp { get; set; }

		public virtual byte[] Content { get; set; }

		protected Person _UploadedBy;


		static partial void Config_(Domain<Domain>.EntityConfiguration<File> entity)
		{
			entity.Association(a => a.Party, a => a.Files);
			entity.Association(a => a.UploadedBy);//, a => a.Files_UploadedBy);
		}

	}


	partial class Domain
	{
		public DbSet<File> Files { get; set; }
	}

}
