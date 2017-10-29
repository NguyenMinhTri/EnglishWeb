using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using Framework.FrameworkContext;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;

///Hung-DV
///UpdatedDate: 2/10/2017

namespace Framework.Repository.Infrastructure
{
    public static class IQueryableExt
    {
        /// <summary>
        /// For an Entity Framework IQueryable, returns the SQL with inlined Parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string ToTraceQuery<T>(this IQueryable<T> query)
        {
            ObjectQuery<T> objectQuery = GetQueryFromQueryable(query);

            var result = objectQuery.ToTraceString();
            foreach (var parameter in objectQuery.Parameters)
            {
                var name = "@" + parameter.Name;
                //var value =  "'" + parameter.Value + "'";
                string value = "''";
                if (parameter.Value != null)
                {
                    value = "'" + parameter.Value.ToString().Replace("'", "''") + "'";
                }
                result = result.Replace(name, value);
            }

            return result;
        }

        /// <summary>
        /// For an Entity Framework IQueryable, returns the SQL and Parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string ToTraceString<T>(this IQueryable<T> query)
        {
            ObjectQuery<T> objectQuery = GetQueryFromQueryable(query);

            var traceString = new StringBuilder();

            traceString.AppendLine(objectQuery.ToTraceString());
            traceString.AppendLine();

            foreach (var parameter in objectQuery.Parameters)
            {
                traceString.AppendLine(parameter.Name + " [" + parameter.ParameterType.FullName + "] = " + parameter.Value);
            }

            return traceString.ToString();
        }

        private static System.Data.Entity.Core.Objects.ObjectQuery<T> GetQueryFromQueryable<T>(IQueryable<T> query)
        {
            var internalQueryField = query.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Where(f => f.Name.Equals("_internalQuery")).FirstOrDefault();
            var internalQuery = internalQueryField.GetValue(query);
            var objectQueryField = internalQuery.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Where(f => f.Name.Equals("_objectQuery")).FirstOrDefault();
            return objectQueryField.GetValue(internalQuery) as System.Data.Entity.Core.Objects.ObjectQuery<T>;
        }



    }


    public interface IRepository<T> where T : class
    {
        // Marks an entity as new
        T Add(T entity);

        // Marks an entity as modified
        void Update(T entity);

        // Marks an entity to be removed
        T Delete(T entity);

        T Delete(int id);

        //Delete multi records
        void DeleteMulti(Expression<Func<T, bool>> where);

        // Get an entity by int id
        T GetSingleById(int id);

        T GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null);

        IEnumerable<T> GetAll(string[] includes = null);

        IQueryable<T> GetMulti(Expression<Func<T, bool>> predicate, string[] includes = null);

        IEnumerable<T> GetMultiPaging(Expression<Func<T, bool>> filter, int index = 0, int size = 50, string[] includes = null);

        int Count(Expression<Func<T, bool>> where);

        bool CheckContains(Expression<Func<T, bool>> predicate);
        IEnumerable<OUT> CompileQueries<OUT>(String where = null, int take = -1);

        IEnumerable<OUT> CompileLeftJoinQueries<OUT, JOINWITH>(string firstProp, string secondProp, string where = null, string[] secondPrams = null) where JOINWITH : class;
        IQueryable<OUT> OptimizeSelect<OUT>(System.Linq.IQueryable<T> linq, String orderByQuery = "");
        IEnumerable<OUT> PagingQuery<OUT>(System.Linq.IQueryable<T> linq, int index, int size, String orderBy = "");
        IEnumerable<OUT> Sql<OUT>(String sql);
        void ExecuteSqlQuery(string sqlQuery, params object[] parameter);
        IDbSet<T> Entity { get; }
    }
    public class BaseRepository<T> : IRepository<T> where T : class
    {

        #region Properties

        protected FrameworkDbContext dataContext;
        protected readonly IDbSet<T> dbSet;

        protected IDbFactory DbFactory
        {
            get;
            private set;
        }

        protected FrameworkDbContext DbContext
        {
            get { return dataContext ?? (dataContext = DbFactory.Init()); }
        }
        #endregion

        protected BaseRepository(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            dbSet = DbContext.Set<T>();
        }

        #region Implementation
        public virtual T Add(T entity)
        {
            return dbSet.Add(entity);
        }
        public virtual T AddWithNoStatus(T entity)
        {
            return dbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            dbSet.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual T DeleteFull(T entity)
        {
            return dbSet.Remove(entity);
        }

        public virtual T Delete(T entity)
        {
            return dbSet.Remove(entity);
        }
        public virtual T Delete(int id)
        {
            var entity = dbSet.Find(id);
            dynamic e = entity;
            Type t = e.GetType();
            if (t.GetRuntimeProperty("Status") != null)
            {
                e.Status = false;
            }
            else
            {
                return dbSet.Remove(entity);
            }
            return null;
            //  return dbSet.Remove(entity);
        }
        public virtual void DeleteMulti(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbSet.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                dbSet.Remove(obj);
        }

        public virtual T GetSingleById(int id)
        {
            return dbSet.Find(id);
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where, string includes)
        {
            return dbSet.Where(where).ToList();
        }

        public virtual int Max(Expression<Func<T, int>> where)
        {
            return dbSet.Max<T, int>(where);
        }
        public virtual int Count(Expression<Func<T, bool>> where)
        {
            return dbSet.Count(where);
        }

        public IEnumerable<T> GetAll(string[] includes = null)
        {
            //HANDLE INCLUDES FOR ASSOCIATED OBJECTS IF APPLICABLE
            if (includes != null && includes.Count() > 0)
            {
                var query = dataContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return query.AsQueryable();
            }

            return dataContext.Set<T>().AsQueryable();
        }

        public T GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null)
        {
            if (includes != null && includes.Count() > 0)
            {
                var query = dataContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return query.FirstOrDefault(expression);
            }
            return dataContext.Set<T>().FirstOrDefault(expression);
        }

        public virtual IQueryable<T> GetMulti(Expression<Func<T, bool>> predicate, string[] includes = null)
        {
            //HANDLE INCLUDES FOR ASSOCIATED OBJECTS IF APPLICABLE
            if (includes != null && includes.Count() > 0)
            {
                var query = dataContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);

                return query.Where<T>(predicate).AsQueryable<T>();
            }

            return dataContext.Set<T>().Where<T>(predicate).AsQueryable<T>();
        }

        public virtual IEnumerable<T> GetMultiPaging(Expression<Func<T, bool>> predicate, int index = 0, int size = 20, string[] includes = null)
        {
            int skipCount = index * size;
            IQueryable<T> _resetSet;

            //HANDLE INCLUDES FOR ASSOCIATED OBJECTS IF APPLICABLE
            if (includes != null && includes.Count() > 0)
            {
                var query = dataContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                _resetSet = predicate != null ? query.Where<T>(predicate).AsQueryable() : query.AsQueryable();
            }
            else
            {
                _resetSet = predicate != null ? dataContext.Set<T>().Where<T>(predicate).AsQueryable() : dataContext.Set<T>().AsQueryable();
            }

            _resetSet = skipCount == 0 ? _resetSet.Take(size) : _resetSet.Skip(skipCount).Take(size);
            //     total = _resetSet.Count();
            return _resetSet.AsQueryable();
        }

        public bool CheckContains(Expression<Func<T, bool>> predicate)
        {
            return dataContext.Set<T>().Count<T>(predicate) > 0;
        }

        public string GetTableName()
        {
            return GetTableName<T>();
            //var type = typeof(T);
            //var entityName = (dataContext as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext.CreateObjectSet<T>().EntitySet.Name;
            //var tableAttribute = type.GetCustomAttributes(false).OfType<System.ComponentModel.DataAnnotations.Schema.TableAttribute>().FirstOrDefault();
            //return tableAttribute == null ? entityName : tableAttribute.Name;
        }

        public string GetTableName<TYPE>() where TYPE : class
        {
            var type = typeof(TYPE);
            var entityName = (dataContext as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext.CreateObjectSet<TYPE>().EntitySet.Name;
            var tableAttribute = type.GetCustomAttributes(false).OfType<System.ComponentModel.DataAnnotations.Schema.TableAttribute>().FirstOrDefault();
            var tableName = tableAttribute == null ? entityName : tableAttribute.Name;

            return "[" + FrameworkDbContext.schema + "]." + tableName;
        }

        public IEnumerable<OUT> CompileQueries<OUT>(String where = null, int take = -1)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select ");
            if (take > 0)
            {
                sb.Append(" top " + take + " ");
            }
            PropertyInfo[] props = typeof(OUT).GetProperties();
            PropertyInfo[] propsSource = typeof(T).GetProperties();
            var nProp = props.Count();

            var propertySb = new StringBuilder();

            for (int iProp = 0; iProp < nProp; iProp++)
            {
                var p = props[iProp];
                if (!(propsSource.Count(x => x.Name == p.Name && x.PropertyType == p.PropertyType) > 0))
                {
                    continue;
                }

                propertySb.Append(p.Name);
                propertySb.Append(", ");
            }

            var propetyStr = propertySb.ToString();

            sb.Append(propetyStr.Substring(0, propetyStr.Length - 2));

            sb.Append(" from ");
            sb.Append(GetTableName());


            // return dataContext.Set<T>().Where<T>(predicate).AsQueryable<T>();
            if (where != null)
                sb.Append(" where " + where);

            try
            {
                return dataContext.Database.SqlQuery<OUT>(sb.ToString()).ToList();
            }
            catch
            {
                throw new Exception(sb.ToString());
            }
        }

        #endregion

        public IEnumerable<OUT> CompileLeftJoinQueries<OUT, JOINWITH>(string firstProp, string secondProp, string where = null, string[] secondPrams = null) where JOINWITH : class
        {
            var firstTableName = GetTableName();
            var secondTableName = GetTableName<JOINWITH>();

            string firstObject = firstTableName.Substring(firstTableName.IndexOf(".") + 1);
            string secondObject = secondTableName.Substring(secondTableName.IndexOf(".") + 1);

            StringBuilder sb = new StringBuilder();
            sb.Append("select ");
            PropertyInfo[] props = typeof(OUT).GetProperties();
            PropertyInfo[] propsSource = typeof(T).GetProperties();
            PropertyInfo[] propsJoinWith = typeof(JOINWITH).GetProperties();
            var nProp = props.Count();

            var propertySb = new StringBuilder();

            for (int iProp = 0; iProp < nProp; iProp++)
            {
                var p = props[iProp];

                if (secondPrams != null && secondPrams.Count(x => x == p.Name) > 0)
                {
                    propertySb.Append(secondObject + "." + p.Name);
                    propertySb.Append(" as " + secondObject + p.Name + ", ");

                    if (propsSource.Count(x => x.Name == p.Name && x.PropertyType == p.PropertyType) > 0)
                    {
                        propertySb.Append(firstObject + "." + p.Name);
                        propertySb.Append(" as " + firstObject + p.Name + ", ");
                        continue;
                    }

                    continue;
                }

                if (!(propsSource.Count(x => x.Name == p.Name && x.PropertyType == p.PropertyType) > 0))
                {
                    if (p.Name.Contains("_"))
                    {
                        var type = p.Name.Replace("_", ".");
                        propertySb.Append(type);
                        propertySb.Append(" as " + p.Name);
                        propertySb.Append(", ");
                    }
                    continue;
                }

                propertySb.Append(firstObject + "." + p.Name);
                propertySb.Append(" as " + p.Name);
                propertySb.Append(", ");
            }

            var propetyStr = propertySb.ToString();

            sb.Append(propetyStr.Substring(0, propetyStr.Length - 2));


            sb.Append(" from ");
            sb.Append(firstTableName);
            sb.Append(" as ");
            sb.Append(firstObject);
            sb.Append(" left join ");
            sb.Append(secondTableName);
            sb.Append(" as ");
            sb.Append(secondObject);

            sb.Append(" on ");
            sb.Append(firstObject + ".");
            sb.Append(firstProp);
            sb.Append(" = ");

            sb.Append(secondObject + ".");
            sb.Append(secondProp);

            // return dataContext.Set<T>().Where<T>(predicate).AsQueryable<T>();
            if (where != null)
                sb.Append(" where " + where);

            try
            {
                return dataContext.Database.SqlQuery<OUT>(sb.ToString()).ToList();
            }
            catch
            {
                throw new Exception(sb.ToString());
            }
        }


        public IEnumerable<OUT> Sql<OUT>(string sql)
        {
            try
            {
                return dataContext.Database.SqlQuery<OUT>(sql).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(sql);
            }
        }

        public IEnumerable<OUT> PagingQuery<OUT>(System.Linq.IQueryable<T> linq, int index, int size, String orderBy = "")
        {
            var linqStr = linq.ToTraceQuery();
            StringBuilder sb = new StringBuilder();
            sb.Append(linqStr.Substring(0, linqStr.IndexOf("[")));
            PropertyInfo[] props = typeof(OUT).GetProperties();
            PropertyInfo[] propsSource = typeof(T).GetProperties();
            var nProp = props.Count();

            var propertySb = new StringBuilder();

            for (int iProp = 0; iProp < nProp; iProp++)
            {
                var p = props[iProp];
                if (!(propsSource.Count(x => x.Name == p.Name && x.PropertyType == p.PropertyType) > 0))
                {
                    continue;
                }

                propertySb.Append(p.Name);
                propertySb.Append(", ");
            }

            var propetyStr = propertySb.ToString();

            sb.Append(propetyStr.Substring(0, propetyStr.Length - 2));

            sb.Append(" ");

            sb.Append(linqStr.Substring(linqStr.IndexOf("FROM")));

            var last = String.Format(" order by {0} OFFSET {1} rows FETCH NEXT {2} ROWs ONLY", orderBy, index * size, size);
            sb.Append(last);
            try
            {
                return dataContext.Database.SqlQuery<OUT>(sb.ToString()).ToList();
            }
            catch
            {
                throw new Exception(sb.ToString());
            }
        }
        public IQueryable<OUT> OptimizeSelect<OUT>(System.Linq.IQueryable<T> linq, String orderByQuery = "")
        {
            var linqStr = linq.ToTraceQuery();
            StringBuilder sb = new StringBuilder();
            sb.Append(linqStr.Substring(0, linqStr.IndexOf("[")));
            PropertyInfo[] props = typeof(OUT).GetProperties();
            PropertyInfo[] propsSource = typeof(T).GetProperties();
            var nProp = props.Count();

            var propertySb = new StringBuilder();

            for (int iProp = 0; iProp < nProp; iProp++)
            {
                var p = props[iProp];
                if (!(propsSource.Count(x => x.Name == p.Name && x.PropertyType == p.PropertyType) > 0))
                {
                    continue;
                }

                propertySb.Append(p.Name);
                propertySb.Append(", ");
            }

            var propetyStr = propertySb.ToString();

            sb.Append(propetyStr.Substring(0, propetyStr.Length - 2));

            sb.Append(" ");

            sb.Append(linqStr.Substring(linqStr.IndexOf("FROM")));

            orderByQuery = " " + orderByQuery + " ";

            sb.Append(orderByQuery);

            try
            {
                return dataContext.Database.SqlQuery<OUT>(sb.ToString()).AsQueryable();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message + sb.ToString());
            }

        }



        public IDbSet<T> Entity
        {
            get
            {
                return dbSet;
            }
        }


        public void ExecuteSqlQuery(string sqlQuery, params object[] parameter)
        {
            dataContext.Database.SqlQuery(typeof(int), sqlQuery, parameter);
        }
    }
}
