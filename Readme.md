```powershell
# 安装
dotnet tool install --global dotnet-ef
dotnet tool install --global dotnet-ef --version 3.1.0

#更新
dotnet tool update --global dotnet-ef --version 3.1.0
dotnet tool update --global dotnet-ef

# 卸载
dotnet tool uninstall --global dotnet-ef
```

# Code First

**项目结构**

- Api: crud服务
- EntityFrameworkCore: 数据上下文
- Entities: 实体模型

**EntityFrameworkCore 添加 ef core 支持** (*如下几个依赖包保持版本一致,尽量使用都有的最高版本,例如: 5.0.7*)

- Npgsql
- Npgsql.EntityFrameworkCore.PostgreSQ
- Microsoft.EntityFrameworkCore.Design

```powershell
# 使用cmd进入EntityFrameworkCore项目所在的目录

# 指定上下文和迁移文件输出目录,添加数据迁移
dotnet ef migrations add init -c PostgreDbContext -o Data/Migrations

# 移除数据迁移
# ef migrations remove

# 将数据迁移应用到数据库
dotnet ef database update
```

**测试**

```sh
curl -X 'POST' \
  'https://localhost:5001/api/Account' \
  -H 'accept: text/plain' \
  -H 'Content-Type: application/json' \
  -d '{
  "id": 0,
  "name": "jack",
  "age": 18,
  "registerDate": "2021-07-27T07:38:19.513Z",
  "registeredUser": true,
  "balance": 1000.02
}'
```

```sh
curl -X 'GET' \
  'https://localhost:5001/api/Account/1' \
  -H 'accept: text/plain'
```

```sh
curl -X 'PUT' \
  'https://localhost:5001/api/Account/1' \
  -H 'accept: text/plain' \
  -H 'Content-Type: application/json' \
  -d '{
  "id": 1,
  "name": "jack",
  "age": 19,
  "registerDate": "2021-07-27T07:38:19.513Z",
  "registeredUser": true,
  "balance": 1000.02
}'
```

```sh
curl -X 'DELETE' \
  'https://localhost:5001/api/Account/1' \
  -H 'accept: text/plain'
```

**Db First**

**项目结构**

- DbFirstConsoleApp: 启动项目
  - Microsoft.EntityFrameworkCore.Design
- DALEntity: 数据访问层
  - Npgsql
  - Npgsql.EntityFrameworkCore.PostgreSQL

```powershell
# 使用cmd进入项目跟目录
dotnet ef dbcontext scaffold "User ID=demo_user; Password=password; Host=192.168.199.133; Port=5432; Database=demo_db;" "Npgsql.EntityFrameworkCore.PostgreSQL" -o "Entities" -c "PostgreDbContext" -d -p DALEntity -s DbFirstConsoleApp -f --json --no-build -v
```

