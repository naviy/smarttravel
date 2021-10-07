using System;


namespace Luxena.Base.Data
{

	public interface ITransactionManager : IDisposable
	{
		void BeginWork();

		void Flush();

		void Commit();

		void Rollback();

		void Close();

		T By<T>(object id) where T : class;

		T Get<T>(object id);

		object Get(Type type, object id);

		object Get(Type type, object id, object version);

		T Refer<T>(object id);

		object Refer(Type type, object id);

		void Save(object obj);

		void Delete(object obj);

		void Delete(Type type, object id);

		void Comment(object obj, string comment);
	}

}