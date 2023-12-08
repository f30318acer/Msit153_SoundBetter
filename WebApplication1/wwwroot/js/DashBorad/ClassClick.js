$(document).ready(function () {
    // 從伺服器端獲取課程點擊數據
    $.ajax({
        type: "GET",
        url: '/Chart/ClickRatio', // 確保這是返回課程點擊數據的端點
        success: function (data) {
            // 使用數據建立柱狀圖
            Highcharts.chart('clickContainer', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: '課程點擊統計'
                },
                xAxis: {
                    categories: data.map(item => 'Class ' + item.ClassID),
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'Clicks'
                    }
                },
                series: [{
                    name: 'Clicks',
                    data: data.map(item => item.Clicks) // 假設每個 Clicks 都是單一數字，而不是數組
                }]
            });
        },
        error: function (error) {
            console.error("發生錯誤:", error);
        }
    });
}