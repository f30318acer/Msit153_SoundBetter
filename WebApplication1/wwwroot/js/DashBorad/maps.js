(async () => {

    const topology = await fetch(
        'https://code.highcharts.com/mapdata/countries/tw/tw-all.topo.json'
    ).then(response => response.json());

    // Fetch the site-city data from your API
    const siteCityData = await fetch('/Chart/SiteCityData')
        .then(response => response.json());

    // Transform the site-city data into the format Highcharts expects
    const data = siteCityData.map(item => {
        // Assuming 'item.cityCode' is the city code like 'tw-tp' for Taipei
        // and 'item.siteCount' is the number of sites in that city
        return [item.cityCode, item.siteCount];
    });

    // Create the chart
    Highcharts.mapChart('container1', {
        chart: {
            map: topology
        },

        title: {
            text: 'Site and City Distribution in Taiwan'
        },

        subtitle: {
            text: 'Interactive map showing the distribution of sites across different cities in Taiwan.'
        },

        mapNavigation: {
            enabled: true,
            buttonOptions: {
                verticalAlign: 'bottom'
            }
        },

        colorAxis: {
            min: 0,
            stops: [
                [0, '#EFEFFF'],
                [0.5, Highcharts.getOptions().colors[0]],
                [1, Highcharts.color(Highcharts.getOptions().colors[0]).brighten(-0.5).get()]
            ]
        },

        series: [{
            data: data,
            name: 'Site count',
            states: {
                hover: {
                    color: '#BADA55'
                }
            },
            dataLabels: {
                enabled: true,
                format: '{point.name}'
            }
        }]
    });

})();
