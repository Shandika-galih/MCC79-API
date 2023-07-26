/*$.ajax({
    url: "https://pokeapi.co/api/v2/pokemon/"
}).done((result) => {
    let temp = "";
    $.each(result.results, (key, val) => {
        temp += `<tr>
                    <td>${key + 1}</td>
                    <td class="text-capitalize">${val.name}</td>
                    <td><button onclick="detail('${val.url}')" data-bs-toggle="modal" data-bs-target="#modalSW" class="btn btn-outline-dark">Detail</button></td>
                </tr>`;
    })
    $("#tbodySW").html(temp);
})*/
$(document).ready(function () {
    $('#myTable').DataTable({
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'copy',
                className: 'btn btn-primary',
                text: 'Copy'
            },
            {
                extend: 'excel',
                className: 'btn btn-success',
                text: 'Export to Excel'
            },
            {
                extend: 'pdf',
                className: 'btn btn-danger',
                text: 'Export to PDF'
            },
            {
                extend: 'print',
                className: 'btn btn-warning',
                text: 'Print'
            },
            {
                extend: 'colvis',
                className: 'buttons-colvis',
                postfixButtons: ['colvisRestore']

            },
        ],
        ajax: {
            url: "https://localhost:7186/Api/employees",
            dataType: "JSON",
            dataSrc: "data" //data source -> butuh array of object
        },
        columnDefs: [{
            targets: 0,
            render: function (data, type, row, meta) {
                return meta.row + 1;
            }
        }],
        columns: [
            { data: null},
            { data: "nik" },
            {
                data: null,
                render: function (data, type, row) {
                    return row.firstName + ' ' + row.lastName;
                }
            },
            {
                data: "birtdate",
                render: function (data) {
                    return moment(data).format('DD MMMM YYYY');
                }
            },
            {
                data: "gender",
                render: function (data) {
                    return data == 0 ? "Perempuan" : "Laki-Laki";
                }
            },
            { data: "email" },
            { data: "phoneNumber" },
            {
                data: "guid",
                render: function (data, type, row) {
                    return '<button type="button" class="btn btn-danger" onclick="deleteData(\'' + data + '\')" data-bs-toggle="modal">Delete</button>  <button type="button" class="btn btn-success" onclick="editData(\'' + data + '\')" data-bs-toggle="modal" data-bs-target="#editModal">Edit</button>';
                }
            }
        ]
    });
});

function deleteData(guid) {
    // Konfirmasi penghapusan data
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            // Mengirim request AJAX untuk menghapus data
            $.ajax({
                url: `https://localhost:7186/Api/employees?guid=${guid}`,
                type: 'DELETE',
                success: function (response) {
                    // Menampilkan pesan sukses
                    Swal.fire(
                        'Deleted!',
                        'The data has been deleted.',
                        'success'
                    );

                    // Refresh data pada DataTables
                    $('#myTable').DataTable().ajax.reload();
                },
                error: function (xhr, status, error) {
                    // Menampilkan pesan error
                    Swal.fire(
                        'Error!',
                        'An error occurred while deleting the data.',
                        'error'
                    );
                }
            });
        }
    });
}
function editData(guid) {
    // Mengambil data sebelumnya melalui API atau sumber data lainnya
    $.ajax({
        url: `https://localhost:7186/Api/employees/${guid}`,
        type: "GET",
        success: (data) => {
            let nik = parseInt(data.data.nik);
            let birtdate = moment(data.data.birtdate).format('YYYY-MM-DD');
            let hiringDate = moment(data.data.hiringDate).format('YYYY-MM-DD');
            $('#enik').val(nik);
            $('#efirstName').val(data.data.firstName);
            $('#elastName').val(data.data.lastName);
            $('#ebirtdate').val(birtdate);

            $('#ehiringDate').val(hiringDate);
            $('#eemail').val(data.data.email);
            $('#ephoneNumber').val(data.data.phoneNumber);
            $('#eguid').val(data.data.guid);
            $('input[name="egender"]').filter(function () {
                return $(this).val() == data.data.gender;
            }).prop("checked", true);
            console.log(data);
        },
        error: (data) => {
            Swal.fire({
                icon :'Error!',
                title : 'An error occurred while deleting the data.',
                text :'error'
            }
            );
        }
    });
}

function Update() {
    var obj = new Object(); // Sesuaikan sendiri nama objectnya dan beserta isinya
    // Ini mengambil value dari tiap inputan di form
    obj.nik = $("#enik").val();
    obj.firstName = $("#efirstName").val();
    obj.lastName = $("#elastName").val();
    obj.birtdate = new Date($("#ebirtdate").val()).toISOString();
    obj.gender = parseInt($("input[name='eflexRadioDefault']:checked").val());
    obj.hiringDate = new Date($("#ehiringDate").val()).toISOString();
    obj.email = $("#eemail").val();
    obj.phoneNumber = $("#ephoneNumber").val(); 
    obj.guid = $('#eguid').val();
    // Isi dari object disesuaikan dengan bentuk object yang akan di-PUT
    console.log(obj);
    $.ajax({
        url: "https://localhost:7186/Api/employees",
        type: "PUT",
        data: JSON.stringify(obj), // Mengubah objek menjadi string JSON
        contentType: "application/json",
    }).done((result) => {
        // Alert untuk respons berhasil
        Swal.fire(
            'Good job!',
            result.message,
            'success'
        )
    }).fail((error) => {
        let response = JSON.parse(error.responseText);
        let err = response.errors;
        // Mengakses pesan error berdasarkan properti yang ada
        let errorMessage = "";
        for (const property in err) {
            errorMessage += err[property] + "<br>";
            console.log(`${err[property]}`);
        }

        // Menampilkan pesan error menggunakan alert
        Swal.fire(
            'Error!',
            errorMessage,
            'error'
        );
    });
}

/*function Update() {
    var obj = new Object(); //sesuaikan sendiri nama objectnya dan beserta isinya
    //ini ngambil value dari tiap inputan di form nya
    obj.nik = $("#enik").val();
    obj.firstName = $("#efirstName").val();
    obj.lastName = $("#elastName").val();
    obj.birtdate = new Date($("#ebirtdate").val()).toISOString();
    obj.gender = parseInt($("input[name='eflexRadioDefault']:checked").val());
    obj.hiringDate = new Date($("#ehiringDate").val()).toISOString();
    obj.email = $("#eemail").val();
    obj.phoneNumber = $("#ephoneNumber").val();
    obj.guid = $('#eguid').val();
    //isi dari object kalian buat sesuai dengan bentuk object yang akan di post
    console.log(Update());
    $.ajax({
        url: "https://localhost:7186/Api/employees",
        type: "PUT",
        data: JSON.stringify(obj), //jika terkena 415 unsupported media type (tambahkan headertype Json & JSON.Stringify();)
        contentType: "application/json",
    }).done((result) => {
        // Alert for successful response
        Swal.fire(
            'Good job!',
            result.message,
            'success'
        )
    }).fail((error) => {
        let response = JSON.parse(error.responseText);
        let err = response.errors;

        // Mengakses pesan error berdasarkan properti yang ada
        let errorMessage = ""
        for (const property in err) {
            errorMessage += err[property] + "<br>";
            console.log(`${err[property]}`);
        }

        // Menampilkan pesan error menggunakan alert
        Swal.fire(
            'Error!',
            errorMessage,
            'error'
        );
    })
}*/

function Insert() {
    var obj = new Object(); //sesuaikan sendiri nama objectnya dan beserta isinya
    //ini ngambil value dari tiap inputan di form nya
    obj.nik = $("#nik").val();
    obj.firstName = $("#firstName").val();
    obj.lastName = $("#lastName").val();
    obj.birtdate = new Date($("#birtdate").val()).toISOString();
    obj.gender =  parseInt($("input[name='flexRadioDefault']:checked").val());
    obj.hiringDate = new Date($("#hiringDate").val()).toISOString();
    obj.email = $("#email").val();
    obj.phoneNumber = $("#phoneNumber").val();
    //isi dari object kalian buat sesuai dengan bentuk object yang akan di post
    console.log(JSON.stringify(obj.gender))
    $.ajax({
        url: "https://localhost:7186/Api/employees",
        type: "POST",
        data: JSON.stringify(obj), //jika terkena 415 unsupported media type (tambahkan headertype Json & JSON.Stringify();)
        contentType: "application/json",
    }).done((result) => {
        // Alert for successful response
        Swal.fire(
            'Good job!',
            result.message,
            'success'
        )
    }).fail((error) => {
        let response = JSON.parse(error.responseText);
        let err = response.errors;

        // Mengakses pesan error berdasarkan properti yang ada
        let errorMessage = ""
        for (const property in err) {
            errorMessage += err[property] + "<br>";
            console.log(`${err[property]}`);
        }

        // Menampilkan pesan error menggunakan alert
        Swal.fire(
            'Error!',
            errorMessage,
            'error'
        );
    })
}
$(document).ready(function () {
    $('#myForm').submit(function (e) {
        e.preventDefault();
        Insert();
        $("#modalS").modal("hide");
    });
    /*$('#myFormUpdate').submit(function (b) {
        b.preventDefault();
        Update();
        $("#editModal").modal("hide");
    });*/
    $("#myFormUpdate").submit(function (b) {
        b.preventDefault();
        Update();
        $("#editModal").modal("hide");
    });
});


/*let dataStat;
function detail(stringURL) {
    $.ajax({
        url: stringURL
    }).done(res => {
        $(".modal-title").html(res.name);
        $("#imgPok").attr("src", res.sprites.other["official-artwork"].front_default);
        dataStat = res;
        getStat()
    })
}
console.log(dataStat)

function getStat() {
    let temp = "";
    dataStat.stats.forEach(statistik => {
        temp += `
        <div class="row">    
        <div class="col-md-5 p-2" id="nameStat">${statistik.stat.name}</div>
                                <div class="col-md-7 p-2">
                                    <div class="progress m-2">
                                        <div class="progress-bar bg-success progress-bar-striped progress-bar-animated" role="progressbar" style="width: ${statistik.base_stat + "%"};" aria-valuenow=${statistik.base_stat} aria-valuemin="0" aria-valuemax="100">${statistik.base_stat}%</div>
                                    </div>
                                </div>
                            </div>`;
    })
    $("#bodyState").html(temp);
}
$("#btnradio1").click(function () {
    getStat();
});
function getAbout() {
    let abilities = dataStat.abilities.map(aby => aby.ability.name);
    let type = dataStat.types.map(typ => typ.type.name);
    let list = [
        {
            name: "ID",
            value: dataStat.id
        },
        {
            name: "Nama",
            value: dataStat.forms[0].name
        },
        {
            name: "Abilities",
            value: abilities.join(', ')
        },
        {
            name: "Weight",
            value: dataStat.weight
        },
        {
            name: "Height",
            value: dataStat.height
        },
        {
            name: "Types",
            value: type.join(', ')
        },
        
    ]

    let temp = "";
    list.forEach(about => {
    temp += `
        <div class="row">
            <div class="col-md-4 p-2 ">
            ${about.name}
            </div>
            <div class="col-md-8 p-2 ${about.name === 'Types' ? 'text-success' : ''}">
            ${about.value}
            </div>
        </div>`;
    })
    $("#bodyState").html(temp);
}
$("#btnradio2").click(function () {
    getAbout();
});*/
// Mengambil data employee dari API menggunakan metode GET
function showChart() {
    // Mendapatkan data dari API untuk kedua jenis chart
    $.ajax({
        url: "https://localhost:7186/Api/accounts/getall-master", // Sesuaikan URL sesuai dengan endpoint API Anda
        type: "GET",
        dataType: "json"
    }).done(res => {
        // Menghitung jumlah jenis kelamin
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

        // Menghitung rata-rata IPK dalam rentang tertentu
        let ipkRangeCounts = {
            '0-1': 0,
            '1-2': 0,
            '2-3': 0,
            '3-3.5': 0,
            '3.6-4': 0
        };
        for (let i = 0; i < res.data.length; i++) {
            let ipk = res.data[i].gpa;
            if (ipk >= 0 && ipk < 1) {
                ipkRangeCounts['0-1']++;
            } else if (ipk >= 1 && ipk < 2) {
                ipkRangeCounts['1-2']++;
            } else if (ipk >= 2 && ipk < 3) {
                ipkRangeCounts['2-3']++;
            } else if (ipk >= 3 && ipk < 3.5) {
                ipkRangeCounts['3-3.5']++;
            } else if (ipk >= 3.6 && ipk <= 4) {
                ipkRangeCounts['3.6-4']++;
            }
        }
        // Membuat grafik IPK menggunakan Chart.js
        let ipkCtx = document.getElementById('ipkChart').getContext('2d');
        let ipkChart = new Chart(ipkCtx, {
            type: 'bar',
            data: {
                labels: ['0-1', '1-2', '2-3', '3-3.5', '3.6-4'],
                datasets: [{
                    label: 'IPK Range',
                    data: Object.values(ipkRangeCounts),
                    backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#8d6e63', '#66bb6a'],
                    hoverBackgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#8d6e63', '#66bb6a']
                }]
            },
            options: {
                responsive: true,
                scales: {
                    y: {
                        beginAtZero: true,
                        stepSize: 1,
                        precision: 0
                    }
                }
            }
        });


        // Membuat grafik jenis kelamin menggunakan Chart.js
        let genderCtx = document.getElementById('genderChart').getContext('2d');
        let genderChart = new Chart(genderCtx, {
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

        // Menghitung jumlah universitas
        let universities = {};
        for (let i = 0; i < res.data.length; i++) {
            const universityName = res.data[i].universityName;
            if (universityName in universities) {
                universities[universityName]++;
            } else {
                universities[universityName] = 1;
            }
        }

        // Mengumpulkan data untuk grafik universitas
        const universityNames = Object.keys(universities);
        const universityCounts = Object.values(universities);

        // Membuat grafik universitas menggunakan Chart.js
        let universityCtx = document.getElementById('universityChart').getContext('2d');
        let universityChart = new Chart(universityCtx, {
            type: 'doughnut',
            data: {
                labels: universityNames,
                datasets: [{
                    data: universityCounts,
                    backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#8d6e63', '#66bb6a', '#ba68c8'],
                    hoverBackgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#8d6e63', '#66bb6a', '#ba68c8']
                }]
            },
            options: {
                responsive: true,
                tooltips: {
                    callbacks: {
                        label: function (tooltipItem, data) {
                            let label = data.labels[tooltipItem.index];
                            let value = data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index];
                            return label + ': ' + value;
                        }
                    }
                }
            }
        });

        // Membuka modal setelah grafik selesai dibuat
        $('#modalChart').modal('show');

        $('#modalChart').on('hidden.bs.modal', function () {
            // Reset chart
            let genderChart = Chart.getChart('genderChart');
            let universityChart = Chart.getChart('universityChart');
            let ipkChart = Chart.getChart('ipkChart');
            if (genderChart) {
                genderChart.destroy();
            }
            if (universityChart) {
                universityChart.destroy();
            }
            if (ipkChart) {
                ipkChart.destroy();
            }
        });
      

    }).fail(error => {
        alert("Failed to fetch data from API.");
    });
}