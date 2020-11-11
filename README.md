# 104每日新職缺爬蟲

### 專案目標
由於104顯示的職缺常常有重複的公司  
看來看去都是那幾家  
因此希望每天能快速閱覽沒看過的職缺

### 功能
* 針對104新職缺的頁面的爬蟲
* 儲存已經看過的職缺
* 可透過Line進行發送

### 專案架構
```

MySql(in Azure Web App) <----> JobWebApi(in Azure Web App) <----> JobParser(Service in Oracle Linux Vm)
                                   ↘
                          Line Notify Api(Offical) --->My Line App
                          
```

### todo
1. 使用排程器套件重構排程
2. 把Line發送功能抽成模組	
3. 針對發送的訊息進行排版	
