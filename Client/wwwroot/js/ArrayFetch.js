


/*const animals = [
    { name: "nemo", species: "fish", class: { name: "invertebrata" } },
    { name: "gary", species: "mouse", class: { name: "mamalia" } },
    { name: "dory", species: "fish", class: { name: "invertebrata" } },
    { name: "tom", species: "mouse", class: { name: "mamalia" } },
    { name: "aji", species: "wibu", class: { name: "mamalia" } }
];
//nomor 1
const modifiedAnimals = animals.map(animal => {
    if (animal.species !== "mouse") {
        animal.class.name = "non-mamalia";
    }
    return animal;
});

console.log(modifiedAnimals);

*//*//nomor 2
const onlyMouse = animals.filter(animal => animal.species === "mouse");

console.table(onlyMouse);*//*



const onlyMouse = animals.filter(animal => animal.species === "mouse");

const animalList = document.getElementById("animalList");

onlyMouse.forEach(animal => {
    const li = document.createElement("li");
    const text = document.createTextNode(`Name: ${animal.name}, Species: ${animal.species}`);
    li.appendChild(text);
    animalList.appendChild(li);
});

const tampil = document.getElementById("tampil");

modifiedAnimals.forEach(animal => {
    const li = document.createElement("li");
    const text = document.createTextNode(`Name: ${animal.name}, Species: ${animal.species}, Class: ${animal.class.name}`);
    li.appendChild(text);
    tampil.appendChild(li);
});*/