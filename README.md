# Sports Store
**Customized ASP.NET MVC project based on Adam Freeman's book.**

**1.** Download the code  
**2.** Create a database named *SportsStore* in Microsoft SQL Server Management Studio.  
**3.** Execute *SportsStore.sql* file for creating the db structure and all the necessary tables that I am using.  
**4.** Change the *data source* in the *Web.config* (data source=~~DESKTOP-DPAMMCB\SQLEXPRESS~~ **it would be the *Server name* of your local successful connection**)  

```
<connectionStrings>
    <add name="SportsStoreEntities" connectionString="metadata=res://*/Models.SportsStoreEntities.csdl|res://*/Models.SportsStoreEntities.ssdl|res://*/Models.SportsStoreEntities.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=DESKTOP-DPAMMCB\SQLEXPRESS;initial catalog=SportsStore;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;" providerName="System.Data.EntityClient" />
</connectionStrings>
```
[![sports-store.png](https://i.postimg.cc/zGBkFYQR/sports-store.png)](https://i.postimg.cc/zGBkFYQR/sports-store.png)
