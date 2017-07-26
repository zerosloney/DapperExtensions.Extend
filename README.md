# 介绍

这是基于[DapperExtension](https://github.com/tmsmith/Dapper-Extensions)的添加表达式和更新插入方法的重写。
* 1.添加IDapperContext和IResposorityBase数据访问方法。
* 2.使用前先实例这个两个类。
* 3.使用IResposorityBase方法进行添删改查。
* 4.也可以使用Autofac等依赖框架注入实例。

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





