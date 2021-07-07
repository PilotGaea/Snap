# Snap 範例專案檔
PilotGaea O’view Map Server Snap Plugin

[開發者文件](https://nlscsample.pilotgaea.com.tw/demo/ProgrammingGuide/src/04.ServerSidePlugin/04.2_PluginSample.html#snap)

## Map Server Plugin 介紹：

在O’view Map Server下，共有以下四種類型的外掛可供自訂：

1. DoCommand 自訂操作供使用者呼叫，實作DoCmdBaseClass
2. Account 實作帳號登入功能，實作AccountBaseClass
3. FeatureInfo 提供查詢WMTS圖素屬性，實作FeatureInfoBaseClass
4. Snap鎖點功能，實作SnapBaseClass

以上幾種外掛，DoCommand可多個同時起作用，而Account、FeatureInfo、Snap則同一時間內只啟動一個。

Map Server在啟動或Plugin目錄被重新設定時，會重新開始搜尋指定目錄下的dll檔，根據其實作的BaseClass建立其實體且將其初始化。

在載入dll的過程中如果拋出了例外，當下讀取中的class將被捨棄；各個外掛都是獨立的，即使其中的class名稱重複也不影響讀取，但如果設定的指令重複，後面讀取的會蓋過前面讀取的；同一時間只啟動一個的外掛，後面讀的也是會蓋過前面的。

當DoCommand執行過程中如果拋出了例外，呼叫者會收到HTTP 500 Internal Server Error的回應，並且Map Server會將此例外會寫入log。

如果使用Web做為前端，請在client各個新增圖層處加上proxy引數，目的是為了在呼叫外掛相關功能時能夠讓cookie正確被設定。

>**注意事項：**
>
> 所有的外掛在編譯時都需注意輸出的dll檔是32位元或是64位元，跟安裝的Map Server必須是同樣位元組的版本，否則讀取外掛時會失敗。

## Snap 使用方法：

1. 使用MicroSoft Visual Studio 2017(或以上之版本)開啟專案檔。
2. 開始建置專案。(須注意Debug/Release與CPU版本。)
3. 完成後請到專案資料夾中的`\bin\Debug`(或是`\bin\Release`，依建置類型決定)目錄內將檔案複製到安裝目錄下的`plugins`目錄中。
4. 於網頁程式中新增一個新的按鈕：

```html
<div id="MyControl" style="position: absolute;z-index: 1;">
    <button id="measure-length">MeasureLength</button>
    <button id="measure-area">MeasureArea</button>
    <button id="clear">Clear</button>
    <button id="change-color-by-distance">ChangeColorByDistance</button>
    <button id="snap">Snap</button> //新增
</div>
```

5. 確認MapServer中有加入向量圖層，命名為範例向量圖層，並在Web中載入：

```javascript
var layerName = "範例向量圖層";
var host = "http://127.0.0.1:8080/wmts?"; //以本機MapServer為例。
var wmtsUrl = host + "Layer=" + layerName + "&style=default&TileMatrixSet=EPSG:3857Service=WMTS&Request=GetTile&Version=1.0.0&Format=image/png&TileMatrix={TileZ}&TileCol{TileC}&TileRow={TileR}";
var Patterns = [wmtsUrl];
var im = mapDoc.NewTileMapLayerByMatrixSet("Vector", Patterns, m, -1);
```

6. 綁定點擊事件到剛剛新增的按鈕上，令他觸發鎖點功能：

```javascript
document.querySelector('#snap ').onclick = function(){
    var layerNam = 'Vector';
    var pixel = 100;
    var type = 'node';
    var radius = 10;
    var fillColor = 'rgba(255, 0, 0, 1)';
    var strokeColor = 'rgba(0, 255, 0, 1)';
    mapView.SetSnapPointSetting(layerNam, pixel, type, radius, fillColor, strokeColor);
};
```

點擊按鈕啟動，再點擊量測鈕，當靠近到指定範圍，便會看到鎖點效果。
