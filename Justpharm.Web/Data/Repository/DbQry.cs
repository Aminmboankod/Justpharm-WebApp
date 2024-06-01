using Justpharm.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Justpharm.Web.Repository;


public class DbQry
{
    private readonly IServiceProvider _serviceProvider;

    public DbQry(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public List<TResult> All<TResult>(Expression<Func<TResult, bool>>? predicate = null, params Expression<Func<TResult, object>>[] includes) where TResult : class
    {
        using var ctx = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<JustpharmContext>();
        var query = ctx.Set<TResult>().AsQueryable().AsNoTracking();
        if (includes.Length > 0)
            foreach (var include in includes)
                query = query.Include(include).AsNoTracking();
        if (predicate != null)
            query = query.Where(predicate).AsNoTracking();
        return query.AsNoTracking().ToList();
    }
    public TResult? First<TResult>(Expression<Func<TResult, bool>>? predicate = null, params Expression<Func<TResult, object>>[] includes) where TResult : class
    {
        using var ctx = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<JustpharmContext>();
        var query = ctx.Set<TResult>().AsNoTracking().AsQueryable();
        if (includes.Length > 0)
            foreach (var include in includes)
                query = query.Include(include).AsNoTracking();
        if (predicate != null)
            query = query.Where(predicate);
        return query.AsNoTracking().FirstOrDefault();
    }
    public TResult? FirstOrdered<TResult>(Expression<Func<TResult, bool>>? predicate = null, Expression<Func<TResult, object>>? orderBy = null, bool descending = false, params Expression<Func<TResult, object>>[] includes) where TResult : class
    {
        using var ctx = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<JustpharmContext>();
        var query = ctx.Set<TResult>().AsNoTracking().AsQueryable();
        if (includes.Length > 0)
            foreach (var include in includes)
                query = query.Include(include).AsNoTracking();
        if (predicate != null)
            query = query.Where(predicate);
        if (orderBy != null)
            query = descending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
        return query.AsNoTracking().FirstOrDefault();
    }
    public bool Exist<T>(Expression<Func<T, bool>> predicate) where T : class
    {
        using var ctx = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<JustpharmContext>();
        return ctx.Set<T>().Any(predicate);
    }
    public void Insert<TInsert>(TInsert insert) where TInsert : class
    {
        using var ctx = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<JustpharmContext>();
        ctx.Add(insert);
        ctx.SaveChanges();
        ctx.ChangeTracker.Clear();
    }
    /// <summary>
    /// Solo se debe usar este método si se usa el mismo contexto para insertar y actualizar.
    /// Haciendo uso del método <see cref="DbQry.PerformTransaction"/> se puede usar el mismo contexto.
    /// </summary>
    /// <param name="insert"></param>
    /// <param name="ctx"></param>
    /// <typeparam name="TInsert"></typeparam>
    public void Insert<TInsert>(TInsert insert, JustpharmContext ctx) where TInsert : class
    {
        ctx.Add(insert);
        ctx.SaveChanges();
    }
    public void Insert<TInsert>(TInsert[] inserts) where TInsert : class
    {
        using var ctx = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<JustpharmContext>();
        ctx.AddRange(inserts);
        ctx.SaveChanges();
        ctx.ChangeTracker.Clear();
    }
    public void Insert<TInsert>(TInsert[] inserts, JustpharmContext ctx) where TInsert : class
    {
        ctx.AddRange(inserts);
        ctx.SaveChanges();
        ctx.ChangeTracker.Clear();
    }
    public void Update<TUpdate>(TUpdate update) where TUpdate : class
    {
        using var ctx = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<JustpharmContext>();
        ctx.Update(update);
        ctx.SaveChanges();
        ctx.ChangeTracker.Clear();
    }
    /// <summary>
    /// Solo se debe usar este método si se usa el mismo contexto para insertar y actualizar.
    /// Haciendo uso del método <see cref="DbQry.PerformTransaction"/> se puede usar el mismo contexto.
    /// </summary>
    /// <param name="update"></param>
    /// <param name="ctx"></param>
    /// <typeparam name="TUpdate"></typeparam>
    public void Update<TUpdate>(TUpdate update, JustpharmContext ctx) where TUpdate : class
    {
        ctx.Update(update);
        ctx.SaveChanges();
    }
    public void Update<TUpdate>(TUpdate[] updates) where TUpdate : class
    {
        using var ctx = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<JustpharmContext>();
        ctx.UpdateRange(updates);
        ctx.SaveChanges();
        ctx.ChangeTracker.Clear();
    }
    /// <summary>
    /// Comprueba si existe el registro, si existe lo actualiza, si no existe lo inserta.
    /// IMPORTANTE: Este método puede ser ineficiente si se usa en un bucle.
    /// </summary>
    /// <param name="update"></param>
    /// <typeparam name="TUpdate"></typeparam>
    public void UpdateOrInsert<TUpdate>(TUpdate update) where TUpdate : class
    {
        using var ctx = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<JustpharmContext>();
        if (ctx.Set<TUpdate>().Contains(update))
            ctx.Update(update);
        else
            ctx.Add(update);
        ctx.SaveChanges();
    }
    public void Update<TUpdate>(Expression<Func<TUpdate, bool>> predicate, Expression<Func<SetPropertyCalls<TUpdate>, SetPropertyCalls<TUpdate>>> update) where TUpdate : class
    {
        using var ctx = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<JustpharmContext>();
        ctx.Set<TUpdate>()
            .Where(predicate)
            .ExecuteUpdate(update);
    }
    public void Delete<TDelete>(TDelete delete) where TDelete : class
    {
        using var ctx = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<JustpharmContext>();
        ctx.Remove(delete);
        ctx.SaveChanges();
        ctx.ChangeTracker.Clear();
    }
    /// <summary>
    /// Solo se debe usar este método si se usa el mismo contexto para insertar y actualizar.
    /// Haciendo uso del método <see cref="DbQry.PerformTransaction"/> se puede usar el mismo contexto.
    /// </summary>
    /// <param name="delete"></param>
    /// <param name="ctx"></param>
    /// <typeparam name="TDelete"></typeparam>
    public void Delete<TDelete>(TDelete delete, JustpharmContext ctx) where TDelete : class
    {
        ctx.Remove(delete);
        ctx.SaveChanges();
    }
    public void DeleteRange<TDelete>(TDelete[] deletes) where TDelete : class
    {
        using var ctx = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<JustpharmContext>();
        ctx.RemoveRange(deletes);
        ctx.SaveChanges();
        ctx.ChangeTracker.Clear();
    }
    public void DeleteRange<TDelete>(TDelete[] deletes, JustpharmContext ctx) where TDelete : class
    {
        ctx.RemoveRange(deletes);
        ctx.SaveChanges();
        ctx.ChangeTracker.Clear();
    }
    public void DeleteTratamientosConTomas(IEnumerable<Tratamiento> tratamientos)
    {
        using var ctx = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<JustpharmContext>();

        // Primero eliminamos las tomas asociadas a los tratamientos
        var tratamientoIds = tratamientos.Select(t => t.UidTomas).ToList();
        var tomas = ctx.Toma.Where(t => tratamientoIds.Contains(t.UidTratamiento)).ToList();
        ctx.Toma.RemoveRange(tomas);

        // Luego eliminamos los tratamientos
        ctx.Tratamiento.RemoveRange(tratamientos);

        ctx.SaveChanges();
        ctx.ChangeTracker.Clear();
    }
    public void DeleteTratamientosConDependencias(IEnumerable<Tratamiento> tratamientos)
    {
        using var ctx = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<JustpharmContext>();

        // Obtener las IDs de los tratamientos a eliminar
        var tratamientoIds = tratamientos.Select(t => t.UidTratamiento).ToList();

        // Eliminar los registros dependientes en la tabla Toma
        var tomasAEliminar = ctx.Toma.Where(t => tratamientoIds.Contains(t.UidTratamiento.GetValueOrDefault())).ToList();
        ctx.Toma.RemoveRange(tomasAEliminar);

        // Eliminar los registros en la tabla Tratamiento
        ctx.Tratamiento.RemoveRange(tratamientos);

        // Guardar los cambios
        ctx.SaveChanges();
        ctx.ChangeTracker.Clear();
    }
    public void Delete<TDelete>(Expression<Func<TDelete, bool>> predicate) where TDelete : class
    {
        using var ctx = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<JustpharmContext>();
        ctx.Set<TDelete>().RemoveRange(ctx.Set<TDelete>().Where(predicate));
        ctx.SaveChanges();
        ctx.ChangeTracker.Clear();
    }
    public void Delete<TDelete>(Expression<Func<TDelete, bool>> predicate, JustpharmContext ctx) where TDelete : class
    {
        ctx.Set<TDelete>().RemoveRange(ctx.Set<TDelete>().Where(predicate));
        ctx.SaveChanges();
        ctx.ChangeTracker.Clear();
    }
    /// <summary>
    /// Inicia una transacción en la base de datos. Si se produce una excepción, se hace rollback.
    /// Importante utilizar los métodos <see cref="Insert{TInsert}(TInsert,JustpharmContext)"/> y <see cref="Update{TUpdate}(TUpdate,JustpharmContext)"/>
    /// para insertar y actualizar los datos.
    /// </summary>
    /// <param name="action">operaciones</param>
    public void PerformTransaction(Action<JustpharmContext> action)
    {
        using var ctx = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<JustpharmContext>();
        using var transaction = ctx.Database.BeginTransaction();
        try
        {
            action(ctx);
            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }
}