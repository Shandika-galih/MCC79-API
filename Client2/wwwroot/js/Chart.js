/*
function showGenderChart() {  
    // Mendapatkan data dari API
    $.ajax({
        url: "https://localhost:7186/api/employees", // Sesuaikan URL sesuai dengan endpoint API Anda
        type: "GET",
        dataType: "json"
    }).done(res => {
        // Mendapatkan jumlah jenis kelamin
        let femaleCount = 0;
        let maleCount = 0;
        for (let i = 0; i < res.data.length; i++) {
            if (res.data[i].gender === 0) {
                femaleCount++;
            } else if (res.data[i].gender === 1) {
                maleCount++;
            }
        }

        // Menghitung total data
        let totalCount = femaleCount + maleCount;

        // Menghitung persentase jenis kelamin
        let femalePercentage = (femaleCount / totalCount) * 100;
        let malePercentage = (maleCount / totalCount) * 100;

        // Membuat grafik menggunakan Chart.js
        let ctx = document.getElementById('genderChart').getContext('2d');
        let genderChart = new Chart(ctx, {
            type: 'pie',
            data: {
                labels: ['Female', 'Male'],
                datasets: [{
                    data: [femalePercentage, malePercentage],
                    backgroundColor: ['#FF6384', '#36A2EB'],
                    hoverBackgroundColor: ['#FF6384', '#36A2EB']
                }]
            },
            options: {
                responsive: true,
                tooltips: {
                    callbacks: {
                        label: function (tooltipItem, data) {
                            let label = data.labels[tooltipItem.index];
                            let value = data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index];
                            return label + ': ' + value.toFixed(2) + '% (' + Math.round(value * totalCount / 100) + ')';
                        }
                    }
                }
            }
        });

    })
}
document.addEventListener("DOMContentLoaded", function (event) {
    showGenderChart();
});
function showChartUniv() {
    $.ajax({
        url: "https://localhost:7186/api/universities", // Sesuaikan URL sesuai dengan endpoint API Anda
        type: "GET",
        dataType: "json"
    }).done(res => {
        // Mendapatkan data universitas
        let universities = res.data;

        // Menghitung jumlah universitas dengan nama yang sama
        let universityCounts = {};
        universities.forEach(university => {
            if (university.name in universityCounts) {
                universityCounts[university.name]++;
            } else {
                universityCounts[university.name] = 1;
            }
        });

        // Mengambil nama universitas yang unik dan kode universitas
        let uniqueUniversityNames = Object.keys(universityCounts);
        let universityCodes = uniqueUniversityNames.map(name => universityCounts[name]);

        // Membuat grafik menggunakan Chart.js
        let ctx = document.getElementById('universityChart').getContext('2d');
        *//*let universityChart = *//*new Chart(ctx, {
            type: 'bar',
            data: {
                labels: uniqueUniversityNames,
                datasets: [{
                    label: 'University Counts',
                    data: universityCodes,
                    backgroundColor: '#36A2EB',
                    hoverBackgroundColor: '#36A2EB'
                }]
            },
            options: {
                responsive: true,
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true,
                            stepSize: 1,
                            precision: 0
                        }
                    }]
                }
            }
        });
    });
}
document.addEventListener("DOMContentLoaded", function (event) {
    showChartUniv();
});*/