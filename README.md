# 缓存管理组件

##### 2018.04 项目近期调整说明
	
	1、拆分项目为.NET Framework 4.0版本与.NET Standard 2.0版本；
	
	2、.NET Framework 4.0版本支持MemoryCache、Memcached、Redis；

	3、.NET Standard 2.0支持MemoryCache、Redis；

	4、精简掉部分无用API；	

	5、Redis客户端采用StackExchange.Redis重新实现；
	

####主要功能：

	缓存写入与读取。

#### 使用方式：

	1、在App.config文件中配置缓存提供者

	2、运行测试

#### Ps：

	如果使用Memcached或Redis缓存需配置服务器地址