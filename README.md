# 104每日新職缺爬蟲

### 專案目標
由於104顯示的職缺常常有重複的公司  
看來看去都是那幾家  
因此希望每天能快速閱覽沒看過的職缺

### 功能
* 針對104新職缺的頁面的爬蟲
* 儲存已經看過的職缺
* 可透過Line進行發送

### 專案架構圖
```
                                   JobParser(Service in Oracle Linux Vm)
                                       ↑
                                       ↓
MySql(in Azure App Service) <----> JobWebApi(in Azure App Service)
                                       ↑
                                       ↓
                                   Line Notify Api(Offical) --->My Line App
                          
```

### 專案架構說明
* 以三個前提為目標
  1. 不需要自己架設伺服器主機
  2. 免費
  3. 順便當作練習架設雲端服務
* 原本打算全部都放在Azure App Service，實際使用才發現該環境會定期回收閒置程式的資源  
* 且每日CPU只能使用90分鐘，雖然他另外有提供排程的功能，但因為環境的因素該平台也無法運行爬蟲庫PuppeTeerSharp使用的Chromium，故決定把這個功能分別抽開，放置在Oracle提供的免費版VM  
* 另外Azure Web App內置Mysql，因此順便練習使用MySql(工作平常使用MsSql)

### 專案結構及依賴
* **JobParser.Core - 104爬蟲的核心邏輯**
  * AutoMapper
  * PuppeTeerSharp
* **JobParser - 104爬蟲的觸發時機及流程**
  * Newtonsoft.Json
* **JobWebApi - 職缺的管理(與MySql互動)及Line的發送**
  * AutoMapper
  * Newtonsoft.Json
  * MySql.Data.EntityFrameworkCore
  * Swashbuckle.AspNetCore
  
### Get Started
* [JobParser Install](/JobParserInstall.md)
* [JobWebApi Install](/JobWebApiInstall.md)

### todo
1. 使用排程器套件重構排程
2. 把Line發送功能抽成模組	
3. 針對發送的訊息進行排版	
