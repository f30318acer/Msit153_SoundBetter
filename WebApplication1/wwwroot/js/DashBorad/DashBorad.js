$(document).ready(function ()
{
    // 從伺服器端獲取數據
    $.ajax({
        type: "GET",
        url: '/Chart/GenderRatio', // 確保這是返回上述 JSON 數據的端點
        success: function (data) {
            // 使用數據建立餅圖
            Highcharts.chart('container', {
                chart: {
                    type: 'pie'
                },
                title: {
                    text: '會員性別比例'
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                accessibility: {
                    point: {
                        valueSuffix: '%'
                    }
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                        }
                    }
                },
                series: [{
                    name: '占比',
                    colorByPoint: true,
                    data: data.map(function (item) {
                        return { name: item.gender, y: item.count };
                    })
                }]
            });
        },
        error: function (error) {
            // 處理錯誤
            console.error("發生錯誤:", error);
        }
    });
});


