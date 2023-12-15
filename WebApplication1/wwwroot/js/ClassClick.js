$(document).ready(function () {
    $.ajax({
        url: '/Chart/GetChartData', // 确保这是正确的URL
        method: 'GET',
        contentType: 'application/json', // 发送的数据类型
        dataType: 'json', // 期望的响应类型
        success: function (chartData) {
            const labels = chartData.map(item => item.ClassName);
            const data = chartData.map(item => item.TotalClicks);

            const ctx = document.getElementById('myChart').getContext('2d');
            const myChart = new Chart(ctx, {
                type: 'bar', // 您可以在这里改为 'line' 如果您想要折线图
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
                            beginAtZero: true,
                            ticks: {
                                stepSize: 1 // 根据您数据的范围调整
                            }
                        }
                    },
                    legend: {
                        display: true,
                        position: 'top' // 图例位置
                    },
                    responsive: true,
                    maintainAspectRatio: false,
                    tooltips: {
                        mode: 'index',
                        intersect: false
                    },
                    hover: {
                        mode: 'nearest',
                        intersect: true
                    }
                }
            });
        },
        error: function (xhr, status, error) {
            // 更详细的错误处理
            console.error("AJAX请求失败:", xhr, status, error);
        }
    });
});
