$(document).ready(function () {
    // Memanggil API untuk mendapatkan data employees
    $.ajax({
        url: "https://localhost:7186/Api/employees",
        method: "GET",
        dataType: "json",
        success: function (data) {
            // Mengambil data gender dari response API
            var genderData = data.map(function (employee) {
                return employee.gender;
            });
            // Menghitung jumlah data berdasarkan gender
            var maleCount = genderData.filter(function (gender) {
                return gender === 1;
            }).length;
            var femaleCount = genderData.filter(function (gender) {
                return gender === 0;
            }).length;
            

            // Menginisialisasi chart
            var chart = new Chart(document.getElementById('chart'), {
                type: 'pie',
                data: {
                    labels: ['Male', 'Female'],
                    datasets: [{
                        data: [maleCount, femaleCount],
                        backgroundColor: ['#007bff', '#dc3545']
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false
                }
            });
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
});
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