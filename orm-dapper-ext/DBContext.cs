using Dapper.Orm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.Orm
{
    public abstract class DBContext
    {
        protected IDbConnection connection { get; set; }
        protected IDbTransaction transaction { get; set; }


        #region 事务操作

        public void BeginTransaction()
        {
            if (transaction != null)
            {
                return;
            }
            transaction = connection.BeginTransaction();
        }

        public void CommitChanges()
        {
            if (transaction != null)
            {
                transaction.Commit();
                transaction.Dispose();
                transaction = null;
            }
        }

        public void Rollback()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                transaction.Dispose();
                transaction = null;
            }
        }

        #endregion 事务操作


        /// <summary>
        /// 公开DbConnection，可以使用Dapper及其扩展的其他方法
        /// </summary>
        public IDbConnection DbConnection { get { return connection; } }

        /// <summary>
        /// 公开DbConnection，可以使用Dapper及其扩展的其他方法
        /// </summary>
        public IDbTransaction DbTransaction { get { return transaction; } }


        #region 公开方法

        /// <summary>
        /// 插入 entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public object Insert<TEntity>(TEntity entity) where TEntity : class
        {
            return connection.Insert(entity, transaction);
        }

        /// <summary>
        /// 修改 entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update<TEntity>(TEntity entity)
        {
            return connection.Update(entity, transaction);
        }

        /// <summary>
        ///  删除 entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete<TEntity>(TEntity entity)
        {
            return connection.Delete(entity, transaction);
        }

        /// <summary>
        /// 根据表达式删除 entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool Delete<TEntity>(Expression<Func<TEntity, bool>> predicate)
        {
            return connection.DeleteMultiple<TEntity>(predicate, transaction);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity Get<TEntity>(object id) where TEntity : class
        {
            return connection.Get<TEntity>(id);
        }

        /// <summary>
        /// 查询全部entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return connection.GetAll<TEntity>();
        }

        /// <summary>
        ///  表连接查询，DbConnection提供更多方法
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="id"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public TReturn Get<T1, T2, TReturn>(object id, Func<T1, T2, TReturn> map) where TReturn : class
        {
            return connection.Get(id, map);
        }

        /// <summary>
        ///  表连接查询，DbConnection提供更多方法
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="id"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public TReturn Get<T1, T2, T3, TReturn>(object id, Func<T1, T2, T3, TReturn> map) where TReturn : class
        {
            return connection.Get(id, map);
        }

        /// <summary>
        /// 表达式查询entity，表达式不包括方法，比如Contacts（请使用In）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> Select<TEntity>(Expression<Func<TEntity, bool>> predicate)
        {
            return connection.Select(predicate);
        }

        /// <summary>
        /// In 查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> In<TEntity>(IEnumerable<int> array)
        {
            return connection.In<TEntity>(array);
        }

        /// <summary>
        /// In 查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filed"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> In<TEntity>(string filed, IEnumerable<int> array)
        {
            return connection.In<TEntity>(filed, array);
        }

        /// <summary>
        /// In 查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> In<TEntity>(IEnumerable<string> array)
        {
            return connection.In<TEntity>(array);
        }

        /// <summary>
        /// In 查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filed"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> In<TEntity>(string filed, IEnumerable<string> array)
        {
            return connection.In<TEntity>(filed, array);
        }

        /// <summary>
        /// sql 查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public TEntity QueryFirstOrDefault<TEntity>(string sql, object param = null)
        {
            return connection.QueryFirstOrDefault<TEntity>(sql, param);
        }

        /// <summary>
        /// sql 查询并Mapper，DbConnection提供更多方法
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> Query<TEntity>(string sql, object param = null)
        {
            return connection.Query<TEntity>(sql, param, transaction: transaction);
        }

        /// <summary>
        /// sql 查询并Mapper，DbConnection提供更多方法
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="splitOn"></param>
        /// <returns></returns>
        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, string splitOn = "Id")
        {
            return connection.Query(sql, map, param, transaction: transaction, splitOn: splitOn);
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int Execute(string sql, object param = null)
        {
            return connection.Execute(sql, param, transaction: transaction);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public PagedList<T> Page<T>(string sql, object param = null, int pageindex = 1, int pagesize = 20)
        {
            return connection.Page<T>(sql, param, pageindex, pagesize);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public PagedList<dynamic> Page(string sql, object param = null, int pageindex = 1, int pagesize = 20)
        {
            return connection.Page(sql, param, pageindex, pagesize);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="splitOn"></param>
        /// <returns></returns>
        public PagedList<TReturn> Page<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, int pageindex = 1, int pagesize = 20, string splitOn = "Id")
        {
            return connection.Page(sql, map, param, pageindex, pagesize);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="splitOn"></param>
        /// <returns></returns>
        public PagedList<TReturn> Page<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, int pageindex = 1, int pagesize = 20, string splitOn = "Id")
        {
            return connection.Page(sql, map, param, pageindex, pagesize);
        }

        #endregion 公开方法
    }
}
