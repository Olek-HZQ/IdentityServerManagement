# IdentityServer4(version 4.1.1) 后台管理

> IdentityServer4 身份验证及授权管理

## 在线演示地址，亦可作为IdentityServer4的测试站点
> [客户端 http://ids4admin.poetrysharing.com](http://ids4admin.poetrysharing.com)
> 
> [服务端 http://ids4auth.poetrysharing.com](http://ids4auth.poetrysharing.com)

## 项目框架

该应用程序框架为 **Asp.Net Core MVC - using .NET Core 3.1**，数据访问采用**Dapper Repository**模式实现的Identity Server4 Storage

## 要求

- [安装](https://www.microsoft.com/net/download/windows#/current) 最新的.NET Core 3.x SDK（使用旧版本在IIS上托管时可能会导致502.5错误，或者在自托管时启动后立即退出应用程序）

## 项目配置

- 当前默认使用数据库为SqlServer，所以先添加数据库命名```IdentityServer4.Admin```，然后执行脚本```CreateTable.sql```生成相关数据表

### 在Visual Studio（2019）中运行运行以下项目（不分顺序）

- 设置启动项目:
  - IdentityServer.Admin
  - IdentityServer.AuthIdentity
  - 启动后自动配置默认Client（客户端），IdentityResource（身份资源），ApiResource（API 资源），ApiScope（API 作用域）及用户信息

## 服务器部署相关

- 项目已添加对应```Dockerfile```文件，可自行打包生成镜像，关于持续集成跟发布可观看我录制的[教学视频系列](https://space.bilibili.com/402355790?spm_id_from=333.788.b_765f7570696e666f.2)

## 后台UI预览

- 后台UI来自于开源项目[skoruba/IdentityServer4.Admin](https://github.com/skoruba/IdentityServer4.Admin)，在此表示深深感谢他们的付出，让我学习了不少

![管理后台](/images/1.jpg)

- 身份验证及授权服务端

![服务端](/images2.jpg)


## 证书

当前使用自己生成的本地证书，证书如果失效可以采取临时证书或者自行添加新证书替换

> 线上可以使用免费证书 [Let's Encypt](https://letsencrypt.org/). 然后Nginx配置即可，推荐配置教程[Secure Nginx with Let's Encrypt on CentOS 7](https://linuxize.com/post/secure-nginx-with-let-s-encrypt-on-centos-7)

## ORM工具 Dapper & 数据访问

- 当前解决方案采用Dapper的Repository模式:

  - `RepositoryDataTypeBase<T>`: Repository抽象类，定义数据库类型，默认表名称及实现数据库连接
  - `RepositoryBase`: Repository基类，定义数据操作的基本增删改查
  - `MssqlRepositoryBase`: 定义具体数据库类型的Repository类（这里以SqlServer）作为演示
  - `ClientRepository`: 具体业务Repository实现类

### 支持的数据库类型:
- SqlServer
- MySql
- PostgreSQL
- Oracle
- SQLite
- Firebird

> 数据库配置文件 `appsettings.json`:
> 
```
"DbConnectionConfiguration": {
    "CurrentDataProviderType": 1, // 1: SqlServer, 2: MySql, 3: Oracle ...
    "MasterSqlServerConnString": "Data Source=.;Initial Catalog=IdentityServer4.Admin;Integrated Security=True;",
    "MasterMySqlConnString": ""
  },
```
> 备注：因这里使用的[SqlKata](https://sqlkata.com)生成数据库语句，当前实现仅为SqlServer，其他类型类似


## 验证及授权
- 在`appsettings.json`中更改IdentityServer和身份验证设置的特定URL和名称
- `AuthorizationConstant.AdministrationPolicy`定义用户的角色常量， 配置值在 `appsettings.json` - `AdministrationRole`
- `IdentityServer.Admin.identityserverdata.json`配置Client（客户端），IdentityResource（身份资源），ApiResource（API 资源），ApiScope（API 作用域）相关信息

### 数据保护:

数据库保护文件存为文件系统，配置目录在:
```
AdminConfiguration.DataProtectionPath
```
## 日志

- 日志采用Serilog组件，保存在项目当前目录的Logs文件夹下，默认每天生成对应日志，需要时手动自行清理或者写个任务计划清理

## 安全检查

- 后台与服务端均包含端点“运行状况”，用于检查数据库和IdentityServer。


## 多语言

- 后续支持...

## 总览

### 项目结构:

- 身份验证跟授权:

  - `SIdentityServer.AuthIdentity` - 包含IdentityServer4实例并结合这些示例的项目 - [Quickstart UI for the IdentityServer4 with Asp.Net Core Identity and EF Core storage](https://github.com/IdentityServer/IdentityServer4/tree/master/samples/Quickstarts/9_Combined_AspId_and_EFStorage)

- 后台:

  - `IdentityServer.Admin` - 包含管理界面的ASP.NET Core MVC应用程序

  - `IdentityServer.Admin.Core` - 数据实体，配置类，Dto类，常量及一些扩展类

  - `IdentityServer.Admin.Dapper` - 数据访问相关

  - `IdentityServer.Admin.Services` - 业务逻辑相关

## IdentityServer4

**Clients**

可以根据客户端类型定义配置-默认情况下使用客户端类型:

- 空
- Web应用程序-服务器端-PKCE的授权代码流
- 单页应用程序-Javascript-带有PKCE的授权代码流
- 本地应用程序-移动/桌面-混合流
- 设备/机器-客户端凭证流
- T电视和受限输入设备应用程序-设备流程

### 将来:

- 多语言化
- 添加用户/角色 权限
- ...

## 	许可

该项目根据以下条款获得许可 [**MIT license**](LICENSE.md).

**注意**: 该存储库使用来自https://github.com/IdentityServer/IdentityServer4.Quickstart.UI的源代码，该源代码受
[**Apache License 2.0**](https://github.com/IdentityServer/IdentityServer4.Quickstart.UI/blob/master/LICENSE).

## 致谢

此Web应用程序基于以下项目：

- ASP.NET Core
- Dapper
- SqlKata
- AutoMapper
- FluentVaidation
- Autofac
- Serilog

感谢 [Dominick Baier](https://github.com/leastprivilege) 和 [Brock Allen](https://github.com/brockallen) -IdentityServer4的创建者以及[skoruba](https://github.com/skoruba/IdentityServer4.Admin).

## 联系我
有任何问题或者协助欢迎联系:

作者:`HuangZhongQiu`

QQ 邮箱: `875755898@qq.com`

Google 邮箱:`huangzhongqiu25@gmail.com`

个人微信小程序 - 诗词海洋（个人兴趣）：

![](/images/mini.jpg)

## 支持与捐赠
如果您觉得这项目对你有帮助，你懂的嘿

![支付宝](/images/wechat.jpg)

![支付宝](/images/alipay.jpg)
