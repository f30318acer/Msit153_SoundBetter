$(document).ready(function () {
    $.ajax({
        url: '/Chart/ClickRatio',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            // 創建 Highcharts 圖表
            Highcharts.chart('container', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Class Click Statistics'
                },
                xAxis: {
                    title: {
                        text: 'Class ID'
                    },
                    categories: data.map(function (item) {
                        return item.ClassID;
                    })
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'Clicks'
                    },
                    allowDecimals: false
                },
                series: [{
                    name: 'Clicks',
                    data: data.map(function (item) {
                        return item.Click;
                    })
                }]
            });
        },
        error: function (error) {
            console.log("Error fetching data", error);
        }
    });
});
