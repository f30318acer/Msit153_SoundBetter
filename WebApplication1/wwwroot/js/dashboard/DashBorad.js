$(document).ready(function ()
{
    // 從伺服器端獲取數據
    // AJAX call to fetch data
    $.ajax({
        type: "GET",
        url: '/Chart/GenderRatio', // Make sure this endpoint is correct and returns data
        success: function (data) {
            // On successful data retrieval
            Highcharts.chart('container', { // Make sure 'container' is the correct id
                chart: {
                    type: 'pie',
                },
                title: {
                    text: '會員性別比例',
                    style: {
                        fontSize: '20px'
                    }
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>',
                    style: {
                        fontSize: '15px'
                    },
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                            style: {
                                fontSize: '15px' // Ensure this is a reasonable value
                            }
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
            // Handle errors here
            console.error("Error fetching data:", error);
        }
    });

});


