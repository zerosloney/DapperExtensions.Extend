# 介绍

这是基于[DapperExtension](https://github.com/tmsmith/Dapper-Extensions)的添加表达式和更新插入方法的重写。
* 1.支持表达式查询集合或者分页集合
* 2.支持指定查询字段查询单个实体,字段可以为主键或者非主键
* 3.支持指定主键或者不指定主键添加实体
* 4.支持更新实体时的条件参数为表达式参数
* 5.支持debug模式下,查看生成的SQL语句,需要配置DynamicProxy([Autofac](http://autofac.readthedocs.io/en/latest/advanced/interceptors.html)或者[Castle](https://github.com/castleproject/Core))

# 示例
The following examples will use a user POCO defined as:

```c#
public class user
{
    public long Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}
```


## Get List Lambda Operation

```c#
IDapperContext context = new DapperContext("db");//db为配置文件的键值
IResposorityBase<user> userRespo = new ResposorityBase<user>();
var list = userRespo.GetList(x => x.Age > 21 && x.Name.StartsWith("7G"), null);
```

## Get PageList Lambda Operation

```c#
//db为配置文件的键值
IDapperContext context = new DapperContext("db");
IResposorityBase<user> userRespo = new ResposorityBase<user>();
var total = 0;
Sorting<user>[] sorts = new Sorting<user>[] { new Sorting<user>(x => x.Id, SortType.Desc) };
var b = userRespo.GetPage(x => x.Id > 1, sorts, 1, 5, false, ref total);
```

## Update Operation

```c#
DbFiled<user> f1 = new DbFiled<user>(x => x.Age, 27);
//根据主键更新
var b = userRespo.Update(x => x.Id == 56907279991046144, f1);
```

## Insert Operation

```c#
user u = new user() { Id = 1111111111, Name = "fd13", Age = 26 };
//指定主键Id的值(db 无自增主键)
var b = userRespo.Insert(u);
//不指定主键Id的值(db 自增主键)
var c = userRespo.Insert(u,x=>x.Id);
```

## Get Single Operation

```c#
//指定主键查询
var b = userRespo.Get(x => x.Id, 1111111111);
```





