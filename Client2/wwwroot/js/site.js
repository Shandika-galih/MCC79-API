/*let btn1 = document.getElementById('btn1');
let btn2 = document.getElementById('btn2');
let btn3 = document.getElementById('btn3');
let btn4 = document.getElementById('btn4');

*//*btn4.onclick = () => {
    document.getElementById('satu').innerHTML = "Halo"
    document.body.style.backgroundColor = generateRandomColor();
};*//*

let satuElement = document.getElementById('satu');
btn1.addEventListener('mousedown', () => {
    satuElement.style.backgroundColor = "red";
    document.getElementById('satu').innerHTML = "Halo"
});
btn1.addEventListener('mouseup', () => {
    satuElement.style.backgroundColor = "white";
});

let duaElement = document.getElementById('dua');
btn2.addEventListener('mousedown', () => {
    duaElement.style.backgroundColor = "yellow";
});

btn2.addEventListener('mouseup', () => {
    duaElement.style.backgroundColor = "white";
}); 

let tigaElement = document.getElementById('tiga');
btn3.addEventListener('mousedown', () => {
    tigaElement.style.backgroundColor = "green";
});

btn3.addEventListener('mouseup', () => {
    tigaElement.style.backgroundColor = "white";
});

function generateRandomColor() {
      const letters = '0123456789ABCDEF';
      let color = '#';
      for (let i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
      }
      return color;
}


let arrayMhsObj = [
    { nama: "budi", nim: "a112015", umur: 20, isActive: true, fakultas: { name: "komputer" } },
    { nama: "joko", nim: "a112035", umur: 22, isActive: false, fakultas: { name: "ekonomi" } },
    { nama: "herul", nim: "a112020", umur: 21, isActive: true, fakultas: { name: "komputer" } },
    { nama: "herul", nim: "a112032", umur: 25, isActive: true, fakultas: { name: "ekonomi" } },
    { nama: "herul", nim: "a112040", umur: 21, isActive: true, fakultas: { name: "komputer" } },
]

*//*// 1. Buat variabel 'fakultasKomputer'
let fakultasKomputer = arrayMhsObj.filter(mahasiswa => mahasiswa.fakultas.name === "komputer");
console.log(fakultasKomputer);*//*

let fakultasKomputer = [];
for (let i = 0; i < arrayMhsObj.length; i++) {
    if (arrayMhsObj[i].fakultas.name == "komputer") {
        fakultasKomputer.push(arrayMhsObj[i])
    }
}
console.log(fakultasKomputer);

let fk = arrayMhsObj.filter(same => same.umur <= 21);
console.log(fk);

// 2. Jika 2 angka di nim terakhir adalah lebih dari atau sama dengan 30, set isActive menjadi false
arrayMhsObj.forEach(mahasiswa => {

    if (parseInt(mahasiswa.nim.slice(-2)) >= 30) {
        mahasiswa.isActive = false;
    }
});
console.log(arrayMhsObj);
*/