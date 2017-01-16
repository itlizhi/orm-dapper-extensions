using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace Dapper.Orm
{
    public interface IDBContext : IDisposable
    {
        /// <summary>
        /// 启动事务
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// 提交事务
        /// </summary>
        void CommitChanges();

        /// <summary>
        /// 回滚事务
        /// </summary>
        void Rollback();

        /// <summary>
        /// 公开DbConnection，可以使用Dapper及其扩展的其他方法
        /// </summary>
        IDbConnection DbConnection { get; }

        /// <summary>
        /// 公开DbTransaction，可以使用Dapper及其扩展的其他方法
        /// </summary>
        IDbTransaction DbTransaction { get; }

        /// <summary>
        /// 插入 entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        object Insert<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// 修改 entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Update<TEntity>(TEntity entity);

        /// <summary>
        /// 删除 entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Delete<TEntity>(TEntity entity);

        /// <summary>
        /// 根据表达式删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        bool Delete<TEntity>(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity Get<TEntity>(object id) where TEntity : class;

        /// <summary>
        /// 查询全部entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class;

        /// <summary>
        /// 表连接查询，DbConnection提供更多方法
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="id"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        TReturn Get<T1, T2, TReturn>(object id, Func<T1, T2, TReturn> map) where TReturn : class;

        /// <summary>
        /// 表连接查询，DbConnection提供更多方法
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="id"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        TReturn Get<T1, T2, T3, TReturn>(object id, Func<T1, T2, T3, TReturn> map) where TReturn : class;

        /// <summary>
        /// 表达式查询entity
        /// 表达式内包括方法不支持
        /// contacts请使用In
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<TEntity> Select<TEntity>(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// In查询 主键
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        IEnumerable<TEntity> In<TEntity>(IEnumerable<int> array);

        /// <summary>
        /// In查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filed"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        IEnumerable<TEntity> In<TEntity>(string filed, IEnumerable<int> array);

        /// <summary>
        /// In查询 主键
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        IEnumerable<TEntity> In<TEntity>(IEnumerable<string> array);

        /// <summary>
        /// In查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filed"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        IEnumerable<TEntity> In<TEntity>(string filed, IEnumerable<string> array);

        /// <summary>
        /// sql 查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        TEntity QueryFirstOrDefault<TEntity>(string sql, object param = null);

        /// <summary>
        /// sql 查询并Mapper，DbConnection提供更多方法
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        IEnumerable<TEntity> Query<TEntity>(string sql, object param = null);

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
        IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, string splitOn = "Id");

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        int Execute(string sql, object param = null);

        #region 分页

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        PagedList<T> Page<T>(string sql, object param = null, int pageindex = 1, int pagesize = 20);


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        PagedList<dynamic> Page(string sql, object param = null, int pageindex = 1, int pagesize = 20);

        /// <summary>
        /// 分页查询
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
        PagedList<TReturn> Page<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, int pageindex = 1, int pagesize = 20, string splitOn = "Id");


        /// <summary>
        /// 分页查询
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
        PagedList<TReturn> Page<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, int pageindex = 1, int pagesize = 20, string splitOn = "Id");
        #endregion
    }
}