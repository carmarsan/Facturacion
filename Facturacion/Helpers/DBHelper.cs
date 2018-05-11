using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace Facturacion.Helpers
{
	public class DBHelper
	{
		async Task<bool> UpdateDbEntryAsync<T>(T entity, DbContext db, params Expression<Func<T, object>>[] properties) where T : class
		{
			try
			{
				var entry = db.Entry(entity);
				db.Set<T>().Attach(entity);
				foreach (var property in properties)
					entry.Property(property).IsModified = true;
				await db.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("UpdateDbEntryAsync exception: " + ex.Message);
				return false;
			}
		}

		bool UpdateDbEntry<T>(T entity, DbContext db, params Expression<Func<T, object>>[] properties) where T : class
		{
			try
			{
				var entry = db.Entry(entity);
				db.Set<T>().Attach(entity);

				foreach (var property in properties)
					entry.Property(property).IsModified = true;

				db.SaveChanges();

				return true;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("UpdateDbEntryAsync exception: " + ex.Message);
				return false;
			}
		}

	}
}