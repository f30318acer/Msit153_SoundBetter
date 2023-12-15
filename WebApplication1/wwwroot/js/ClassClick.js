$(document).ready(function () {
    $.ajax({
        url: '/Chart/GetChartData', // 替换为您的实际 URL
        method: 'GET',
        success: function (chartData) {
            const labels = chartData.map(item => item.ClassName);
            const data = chartData.map(item => item.TotalClicks);

            const ctx = document.getElementById('myChart');
            new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: labels,
                    datasets: [{
                        label: '点击数',
                        data: data,
                        borderWidth: 1,
                        backgroundColor: 'rgba(0, 123, 255, 0.5)',
                        borderColor: 'rgba(0, 123, 255, 1)',
                    }]
                },
                options: {
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    },
                    legend: {
                        display: true
                    },
                    responsive: true,
                    maintainAspectRatio: false
                }
            });
        },
        error: function (error) {
            // 處理錯誤
            console.error("發生錯誤:", error);
        }
    });
});
