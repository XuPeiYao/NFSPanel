# NFSPanel
基於Blazor並運行於Docker的NFS簡易控制面板以及NFS服務

## 系統需求

Docker主機核心必須有以下的核心模組:
* nfs
* nfsd

您必須在要在Docker宿主系統中執行以下指令啟動模組:

```shell
modprobe nfs
modprobe nfsd
```

## 運行容器

依照需求修改並運行以下指令:

```shell
~# docker run -d --net=host -v {YOUR_DATA_PATH}:/mnt -v {YOUR_EXPORTS_FILE}:/etc/exports --privileged --name=nfs xupeiyao/nfspanel
```

首先`-v {YOUR_DATA_PATH}:/mnt`的部分用意在於將實際要分享的目錄映射進容器內，這部分使用者可以自己決定映射的容器內路徑。

而`-v {YOUR_EXPORTS_FILE}:/etc/exports`是為了持續保存NFS的exports設定。

另外這個映像必須使用`--privileged`特權模式執行並且網路設定為`host`。

## 設定

成功運行容器後開啟瀏覽器至80Port即可看到以下畫面

> 目前並沒有加入使用者驗證

![Imgur](https://i.imgur.com/WTcI6wg.png)

點選右上方的新增按鈕進行掛載目錄的建立以及客戶端設定後點選儲存掛載

![Imgur](https://i.imgur.com/584zghJ.png)

回到掛載目錄列表即可看到剛才新增的項目

![Imgur](https://i.imgur.com/wHR7Jpx.png)

要使剛才變更生肖則必須要重啟NFS服務，切換到`重啟NFS服務`頁面，點選`重啟NFS服務`

![Imgur](https://i.imgur.com/GmHg9YM.png)

經過上述操作已經成功地將目錄透過NFS共享出去，可以透過`showmount -e {YOUR_HOST_IP}`的方式檢驗是否成功掛載目錄

![Imgur](https://i.imgur.com/3F7Bf4i.png)