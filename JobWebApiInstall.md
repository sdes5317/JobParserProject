# JobWebApi Install
1. 建立一個App Service
2. 開啟#MySQL In App#
3. 設定appsettings.Production.json裡面的ConnectionStrings
   * Azure App Service>進階工具>執行>Debug Console 
   * cd D:\home\data\mysql
   * 打開MYSQLCONNSTR_localdb.txt(如果沒有這個檔案，則先運行App Service後自動產生)
   * 把上面檔案裡的ConnectionString格式改為以下格式
     ```
     Server=127.0.0.1; Port=50726; Database=localdb; Uid=azure; Pwd=password; Character Set=utf8
     ```
4. 設定appsettings.Production.json裡面的LineNotifyKey
   * 請參考Line官方的文件申請
5. 更改時區為台北
   * Azure App Service>設定>組態
   * 新增一個WEBSITE_TIME_ZONE=Taipei Standard Time
6. 取得發行設定檔並設定到VS2019
   Azure App Service>概觀>取得發行設定檔
7. 編譯JobWebApi專案並發行
