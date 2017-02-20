# orm-dapper-extensions
基于Dapper的一些扩展方法，支持sqlserver和mysql

支持增删改查，简单linq where查询，in查询，分页

可配合 https://github.com/itlizhi/t4-database-to-model  生成数据库model模型


```
 using (IDBContext context = new MySqlDBContext("ormtest"))
 {
  
   try
    {
    context.BeginTransaction();
    //insert
    var stu = new student() { Name = "张三" + DateTime.Now.FormatDateTime(), Status = StudentStatus.yes };
    var rowID = context.Insert(stu);

    //error
    //stu = new student() { Name = null, Status = StudentStatus.yes };
    //rowID = context.Insert(stu);

    //update
    var model = context.Select<student>(p => p.ID == 1).FirstOrDefault();
    model.Status = StudentStatus.no;
    context.Update(model);

    // where
    model = context.Select<student>(p => p.ID == 1).FirstOrDefault();

    //delete 
    var id = rowID.ToInt();
    context.Delete<student>(p => p.ID == id);

    model = context.Select<student>(p => p.ID == id).FirstOrDefault();

    // in
    var arr = new int[] { 1, 2, 3 };
    var list = context.In<student>(arr);
    list = context.Select<student>(p => p.ID > 2 && p.Status == StudentStatus.yes);

    //get
    var getInfo = context.Get<student>(10);
    var getList = context.GetAll<student>();

    //join
    var result = new v_student();
    var dy = context.Get<student, grade, student>(1, (t1, t2) =>
    {
    result.ID = t1.ID;
    result.Name = t1.Name;
    result.gradeList.Add(t2);
    return t1;
    });

    //execute
    var grade = context.Execute("update grade set grade =150 where ID=@id", new { id = 1 });

    //dapper
    var first = context.DbConnection.QueryFirst<grade>("select * from grade where id=@id", new { id = 1 });
    Assert.AreEqual(first.Grade, "150");

    //explicitkey
    var exp = new explicitkey() { Guid = System.Guid.NewGuid().ToString(), Title = "Guid" };
    context.Insert(exp);

    var nexp = context.Select<explicitkey>(p => p.Guid == exp.Guid).FirstOrDefault();
    nexp.Title = "newGuid";

    context.Update(nexp);

    //分页
    var stulist = context.Page<student>("select * from student", null, 2);

    var mitpk = new mitpk() { pk1 = 1, pk2 = System.Guid.NewGuid().ToString(), addtime = DateTime.Now };
    context.Insert(mitpk);


    context.CommitChanges();
    }
    catch (Exception)
    {
       context.Rollback();
       throw;
    }
    Assert.AreEqual(1, 1);
 }
```
