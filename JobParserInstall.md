# JobParser Install(持續整理中...)
1. Oracle Cloud建立免費的VM, Ubuntu 18.04.5 LTS
2. 設定系統時區(會跳出文字設定畫面)
    ```
    sudo dpkg-reconfigure tzdata
    ```
3. 安裝.NET 運行環境  
   * 安裝 .NET 之前，請執行下列命令，將 Microsoft 套件簽署金鑰新增至您的受信賴起點清單，並新增套件存放庫。開啟終端機並執行下列命令：
       ```
       wget https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
       sudo dpkg -i packages-microsoft-prod.deb
       ```
   * 安裝執行階段
     ```
     sudo apt-get update; \
     sudo apt-get install -y apt-transport-https && \
     sudo apt-get update && \
     sudo apt-get install -y aspnetcore-runtime-3.1
     ```
   * 參考:[在 Ubuntu 上安裝 .NET Core SDK 或 .NET Core 執行時間](https://docs.microsoft.com/zh-tw/dotnet/core/install/linux-ubuntu)
5. 在Ubuntu 18.04.5 LTS開啟Port
   ```
   sudo iptables -I INPUT -p tcp -s 0.0.0.0/0 --dport 5000 -j ACCEPT
   ```
6. 在Oracle VM開啟Port  
   網路->虛擬雲端網路->VM->子網路詳細資訊  
   點擊現有的安全清單，並新增傳入規則  
7. 在Ubuntu 18.04.5 LTS安裝背景服務  
    * 在/etc/systemd/system/建立一個服務檔(檔名就是服務名稱+副檔名service)  
    ```
    sudo nano /etc/systemd/system/JobParser.service
    ```
    * 內容(記得修改為實際的檔名跟路徑)
    ```
    [Unit]
    Description=Parser5317

    [Service]
    WorkingDirectory=/home/ubuntu/myparser
    ExecStart=/home/ubuntu/myparser/JobParser
    Restart=always
    # Restart service after 10 seconds if the dotnet service crashes:
    RestartSec=10
    KillSignal=SIGINT
    SyslogIdentifier=dotnet-example
    Environment=ASPNETCORE_ENVIRONMENT=Production
    Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

    [Install]
    WantedBy=multi-user.target
    ```
    * 註冊服務檔
    ```
    sudo systemctl enable JobParser.service
    ```
8. 在Ubuntu 18.04.5 LTS安裝Chromium
   * linux指令
   ```
   sudo apt-get install chromium-browser
   ```
   * 程式碼裡面的路徑調整
   ```
   const browser = await puppeteer.launch({
     executablePath: '/usr/bin/chromium-browser'
   })
   ```
   * 故障排除，如果安裝失敗請先執行以下指令
   ```
   sudo apt-get update
   ```
11. 佈署後台服務  
    使用SFTP(這裡使用FileZilla)連接VM  
    把後台(JobParser專案)放置到指定資料夾  
12. 開啟後台的執行權限(預設只有讀寫沒有執行，這裡直接把三種使用者權限全開)
    ```
    chmod 777 ./JobParser
    ```


### Linux 服務指令
#### 註冊服務檔
```
sudo systemctl enable JobParser.service
```
#### 開啟服務
```
sudo systemctl start JobParser.service
```
#### 停止服務
```
sudo systemctl stop JobParser.service
```
#### 查詢服務
```
sudo systemctl status JobParser.service
```
#### 重新讀取服務設定檔
```
sudo systemctl daemon-reload
```