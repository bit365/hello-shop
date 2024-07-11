# 使用 Windows Service 安裝 Garnet 分布式缓存

零度框架使用微软 Garnet 替代 Redis 分布式缓存，该示例演示如何使用 Windows Service 在单机上安装 Garnet 服务，生成应用程序后使用以下命令操作服务。

## 创建服务

```shell
sc.exe create GarnetService binpath="C:\GarnetWindowsService.exe" start= auto
```

## 启动服务

```shell
sc.exe start GarnetService
```

## 停止服务

```shell
sc.exe stop GarnetService
```

## 卸载服务

```shell
sc.exe delete GarnetService
```