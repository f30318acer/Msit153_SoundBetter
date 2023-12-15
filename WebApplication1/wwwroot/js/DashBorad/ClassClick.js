$(document).ready(function () {
    // 從伺服器端獲取數據
    $.ajax({
        url: '@Url.Action("GetClassClicksData", "Chart")', // 請將 YourControllerName 替換為實際的控制器名稱

        type: 'GET',

        success: function (data) {
            // 創建 Highcharts 圖表
            Highcharts.chart('container', {
                chart: {
                    type: 'line'
                },
                title: {
                    text: '課程點擊次數'
                },
                xAxis: {
                    categories: data.map(item => item.className),
                    title: {
                        text: '課程名稱'
                    }
                },
                yAxis: {
                    title: {
                        text: '點擊次數'
                    }
                },
                series: [{
                    name: '點擊次數',
                    data: data.map(item => item.totalClicks)
                }]
                // ... 其他配置 ...
            });
        },
        error: function (xhr, status, error) {
            // 處理錯誤
            console.error('Error fetching data: ', error);

        }
    });
});

