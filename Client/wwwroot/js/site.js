// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



$(document).ready(function () {
    $.ajax({
        url: "https://valorant-api.com/v1/agents",
        method: "GET",
        success: function (response) {
            var agents = response.data;

            agents.forEach(function (agent, index) {
                var agentName = `<img class="mx-auto" width="70px" height="30px" src="${agent.killfeedPortrait}" alt="agents" /> <p class="font-normal p-2">${agent.displayName}</p>`;
                var agentButton = '<button class=" px-3 py-2 bg-gray-300 text-xs text-black rounded-sm hover:bg-gray-400" data-agent-uuid="' + agent.uuid + '">View Details</button>';
                var agentRow = '<tr><td class="px-4 py-2">' + agentName + '</td><td class="px-4 py-2">' + agentButton + '</td></tr>';

                $("#agentList").append(agentRow);
            });
        }
    });

    $("#agentList").on("click", "button", function () {
        var agentUUID = $(this).attr("data-agent-uuid");
        console.log("Agent UUID:", agentUUID);

        // Perform actions based on the agentUUID
        // You can make another AJAX request using the agentUUID or perform any other operations
        $.ajax({
            url: "https://valorant-api.com/v1/agents/" + agentUUID,
            method: "GET",
            success: function (response) {
                var agentDetails = response.data;
                $("#nameAgentModal").text(agentDetails.displayName);
                $("#modalContent").text(agentDetails.description);
                $("#agentImage").attr("src", agentDetails.fullPortraitV2); // Set the src attribute of the image element
                $("#roleBadge").text(agentDetails.role.displayName); // Set the role display name
                $("#roleBadge").removeClass().addClass(getRoleBadgeClass(agentDetails.role.displayName)); // Apply the corresponding badge style

                var abilities = agentDetails.abilities;
                var carouselInnerAbilities = $("#carouselInnerAbilities");
                carouselInnerAbilities.empty();

                abilities.forEach(function (ability, index) {
                    var isActive = index === 0 ? "active" : "";
                    var abilityItem = '<div class="carousel-item ' + isActive + '"><img src="' + ability.displayIcon + '" class="my-20 mx-auto opacity-25"' + ability.displayName + '"><div class="carousel-caption  mx-auto w-2/3 h-fit-content "><h5>' + ability.displayName + '</h5><p class="mx-auto w-2/3 h-fit-content">' + ability.description + '</p></div></div>';
                    $("#carouselInnerAbilities").append(abilityItem);
                });

                // Show modal or perform other actions
                $("#modal").show();
            }
        });
    });

    $("#modal").on("click", function (event) {
        if (event.target === this) {
            $("#modal").hide();
        }
    });

    $("#closeModal").on("click", function () {
        $("#modal").hide();
    });

    // Function to determine the badge style based on the role display name
    function getRoleBadgeClass(roleName) {
        switch (roleName.toLowerCase()) {
            case "initiator":
                return "px-1 py-0.5 mt-2 text-xs bg-blue-500 text-white rounded";
            case "controller":
                return "px-1 py-0.5 mt-2 text-xs bg-green-500 text-white rounded";
            case "duelist":
                return "px-1 py-0.5 mt-2 text-xs bg-red-500 text-white rounded";
            case "sentinel":
                return "px-1 py-0.5 mt-2 text-xs bg-yellow-500 text-white rounded";
            default:
                return "px-1 py-0.5 mt-2 text-xs bg-gray-500 text-white rounded";
        }
    }
});

/*let query1 = document.querySelector("ul li:nth-child(1)");
let query2 = document.querySelector("ul li:nth-child(2)");

let query3 = document.querySelector(".main:first-child");

let originalBackground = {
    main: "#e91e63",
    sidebar1: "#59698d",
    sidebar2: "#1e88e5"
};

const resetStyles = () => {
    query3.innerHTML = "Main Content";
    query3.style.backgroundColor = originalBackground.main;
    query3.style.color = "";

    query1.innerHTML = "SideBar1";
    query1.style.backgroundColor = originalBackground.sidebar1;
    query1.style.color = "";

    query2.innerHTML = "SideBar2";
    query2.style.backgroundColor = originalBackground.sidebar2;
    query2.style.color = "";
}

const baris = () => {
    query3.innerHTML = "Berubah Pertama";
    query3.style.backgroundColor = "purple";
    query3.style.color = "white";

    setTimeout(resetStyles, 3000);
}


const baris1 = () => {
    query1.innerHTML = "Berubah Kedua";
    query1.style.backgroundColor = "purple";
    query1.style.color = "white";

    setTimeout(resetStyles, 3000);
}

const baris2 = () => {
    query2.innerHTML = "Berubah Ketiga";
    query2.style.backgroundColor = "purple";
    query2.style.color = "white";

    setTimeout(resetStyles, 3000);
}
*/





//FETCHING DATA JQUERY
/*$(document).ready(function () {
    $.ajax({
        url: 'https://swapi.dev/api/people/',
        dataType: 'json',
        success: function (data) {
            var results = data.results;
            var table = '<table class="table table-bordered">';
            table += '<thead><tr><th scope="col">No.</th><th scope="col">Name</th><th scope="col">Height</th><th scope="col">Mass</th><th scope="col">Hair Color</th><th scope="col">Skin Color</th></tr></thead>';
            for (var i = 0; i < results.length; i++) {
                var person = results[i];
                table += '<tr>';
                table += '<td>' + (i + 1) + '</td>';
                table += '<td>' + person.name + '</td>';
                table += '<td>' + person.height + '</td>';
                table += '<td>' + person.mass + '</td>';
                table += '<td>' + person.hair_color + '</td>';
                table += '<td>' + person.skin_color + '</td>';
                table += '</tr>';
            }
            table += '</table>';
            $('#tableContainer').html(table);
        }
    });
});*/

//DONE AND FAIL IMPLEMENTATION
/*$(document).ready(function () {
    $.ajax({
        url: 'https://swapi.dev/api/people/',
        dataType: 'json'
    })
        .done(function (data) {
            var results = data.results;
            var table = '<table class="table table-bordered">';
            table += '<thead><tr><th scope="col">No.</th><th scope="col">Name</th><th scope="col">Height</th><th scope="col">Mass</th><th scope="col">Hair Color</th><th scope="col">Skin Color</th></tr></thead>';
            for (var i = 0; i < results.length; i++) {
                var person = results[i];
                table += '<tr>';
                table += '<td>' + (i + 1) + '</td>';
                table += '<td>' + person.name + '</td>';
                table += '<td>' + person.height + '</td>';
                table += '<td>' + person.mass + '</td>';
                table += '<td>' + person.hair_color + '</td>';
                table += '<td>' + person.skin_color + '</td>';
                table += '</tr>';
            }
            table += '</table>';
            $('#tableContainer').html(table);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            console.log('AJAX request failed: ' + textStatus, errorThrown);
            *//*alert(jqXHR.responseText);*//*
        });
});
*/

//tidak mengambil array didalam array api //MENGGUNAKAN FETCH AND PROMISE
/*document.addEventListener('DOMContentLoaded', function () {
    fetch('https://swapi.dev/api/people/')
        .then(function (response) {
            if (response.ok) {
                return response.json();
            } else {
                throw new Error('Error: ' + response.status);
            }
        })
        .then(function (data) {
            var results = data.results;
            var table = '<table class="table table-bordered">';
            table += '<thead><tr><th scope="col">No.</th><th scope="col">Name</th><th scope="col">Height</th><th scope="col">Mass</th><th scope="col">Hair Color</th><th scope="col">Skin Color</th><th scope="col">Eye Color</th><th scope="col">Birth Year</th><th scope="col">Vehicles Manufacturer</th></tr></thead>';
            for (var i = 0; i < results.length; i++) {
                var person = results[i];
                table += '<tr>';
                table += '<td>' + (i + 1) + '</td>';
                table += '<td>' + person.name + '</td>';
                table += '<td>' + person.height + '</td>';
                table += '<td>' + person.mass + '</td>';
                table += '<td>' + person.hair_color + '</td>';
                table += '<td>' + person.skin_color + '</td>';
                table += '<td>' + person.eye_color + '</td>';
                table += '<td>' + person.birth_year + '</td>';
                table += '<td>' + person.vehicles.manufacturer + '</td>';
                table += '</tr>';
            }
            table += '</table>';
            document.getElementById('tableContainer').innerHTML = table;
        })
        .catch(function (error) {
            console.error('AJAX request failed:', error);
            // alert(error.message);
        });
});*/


//MENGAMBIL ARRAY DIDALAM ARRAY

/*document.addEventListener('DOMContentLoaded', function () {
    fetch('https://swapi.dev/api/people/')
        .then(function (response) {
            if (response.ok) {
                return response.json();
            } else {
                throw new Error('Error: ' + response.status);
            }
        })
        .then(function (data) {
            let results = data.results;
            let table = '<table class="table table-bordered">';
            table += '<thead><tr><th scope="col">No.</th><th scope="col">Name</th><th scope="col">Height</th><th scope="col">Mass</th><th scope="col">Hair Color</th><th scope="col">Skin Color</th><th scope="col">Eye Color</th><th scope="col">Birth Year</th><th scope="col">Manufacturer</th><th scope="col">Films</th></tr></thead>';

            let promises = results.map(function (person, index) {
                let vehiclesPromises = person.vehicles.map(function (vehicleUrl) {
                    return fetch(vehicleUrl)
                        .then(function (response) {
                            if (response.ok) {
                                return response.json();
                            } else {
                                throw new Error('Error: ' + response.status);
                            }
                        })
                        .then(function (vehicleData) {
                            return vehicleData.manufacturer;
                        })
                        .catch(function (error) {
                            console.error('AJAX request failed:', error);
                            return 'N/A';
                        });
                });
                let filmsPromises = person.films.map(function (filmUrl) {
                    return fetch(filmUrl)
                        .then(function (response) {
                            if (response.ok) {
                                return response.json();
                            } else {
                                throw new Error('Error: ' + response.status);
                            }
                        })
                        .then(function (filmData) {
                            return filmData.title;
                        })
                        .catch(function (error) {
                            console.error('AJAX request failed:', error);
                            return 'N/A';
                        });
                });

                let allPromise = Promise.all([...vehiclesPromises, ...filmsPromises])
                    .then(function (results) {
                        var manufacturers = results.slice(0, vehiclesPromises.length);
                        var films = results.slice(vehiclesPromises.length);
                        table += '<tr>';
                        table += '<td>' + (index + 1) + '</td>';
                        table += '<td>' + person.name + '</td>';
                        table += '<td>' + person.height + '</td>';
                        table += '<td>' + person.mass + '</td>';
                        table += '<td>' + person.hair_color + '</td>';
                        table += '<td>' + person.skin_color + '</td>';
                        table += '<td>' + person.eye_color + '</td>';
                        table += '<td>' + person.birth_year + '</td>';
                        table += '<td>' + manufacturers.join(', ') + '</td>';
                        table += '<td>' + films.join(', ') + '</td>';
                        table += '</tr>';
                    });
                return allPromise;
            });

            Promise.all(promises)
                .then(function () {
                    table += '</table>';
                    document.getElementById('tableContainer').innerHTML = table;
                });
        })
        .catch(function (error) {
            console.error('AJAX request failed:', error);
        });
});*/