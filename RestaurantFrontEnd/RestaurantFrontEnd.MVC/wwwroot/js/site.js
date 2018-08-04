// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function initMap() {
    navigator.geolocation.getCurrentPosition(function (position) {
        var pos = {
            lat: position.coords.latitude,
            lng: position.coords.longitude
        };
        var lat = pos.lat;
        var lng = pos.lng;


        /////////////////////
        ////////////
        //GOOGLE MAPS DISTANCE


        var bounds = new google.maps.LatLngBounds;
        var markersArray = [];


        // var origin2 = 'Montgomery, Alabama';
        // var destinationA = 'Manhattan, New York';
        // var destinationB = 'Columbus, Ohio';

        var destinationIcon = 'https://chart.googleapis.com/chart?' +
            'chst=d_map_pin_letter&chld=D|FF0000|000000';
        var originIcon = 'https://chart.googleapis.com/chart?' +
            'chst=d_map_pin_letter&chld=O|FFFF00|000000';
        var map = new google.maps.Map(document.getElementById('map'), {
            center: { lat: 37.1925852, lng: -123.8105744 },//:38.9706559,"lng":-77.3854458
            zoom: 100,
            mapTypeId: 'satellite',
        });
        map.setCenter(pos);
        var origin1 = { lat: pos.lat, lng: pos.lng };

        if (query != null && query != "") {

        //////////////////\//////////////
        //IMPLEMENTING PLACES FUNCTI
        const PROXY_URL = "https://cors-anywhere.herokuapp.com/";
        const TARGET_URL = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=" + origin1.lat + "," + origin1.lng + "&radius=2000&type=restaurant&keyword="+query+"&key=AIzaSyCji7qavH8NTarPPbIyYNCtw7TJnmZHKr0";
        const URL = PROXY_URL + TARGET_URL;
        var destinations = [];
        var destinationNames = [];
        var queryarr = [];
        var restarr = [];

        var kwarr = [];
        queryarr.push(origin1.lat);
        queryarr.push(origin1.lng);


        $.getJSON(URL, function (json) {

            if (json.results === null) {
                $('.loc').html("nope");
            }
            else {

                //commenting out title since it is unused
                //var title = '';
                for (var i = 0; i < json.results.length&&i<6; i++) {

                    destinations.push(json.results[i].geometry.location);
                    //title += "<p>"+JSON.stringify(json.results[i].geometry.location)+"</p>";
                    destinationNames.push(json.results[i].name);
                    var pkg = 
                        {
                            Id: json.results[i].id,
                            Name: json.results[i].name,
                            Hours : "8a-6p",//n[2],UNTIL I CAN FIND HOURS PARAM
                            Lat: json.results[i].geometry.location.lat,
                            Lon: json.results[i].geometry.location.lng,
                            Address: json.results[i].vicinity,
                            Rating: json.results[i].rating,
                            PriceLevel: json.results[i].price_level,
                            Owner: "moq-owner vari"
                        };
                    restarr.push(pkg);

                    if(i==(json.results.length-1)||i==5) {
                    //if (destinationNames.length == 6) {

                        //kwarr
                        var queryString = query;
                        var queryArray = queryString.split("amp;"); 


                        kwarr=queryArray;
                        //queryarr
                        //queryarr.push(origin1.lat);
                        //queryarr.push(origin1.lng);
                        while (queryarr.length <= 2) {
                            queryarr.push('2000');
                        }

                        //resarr



                        /////
                        console.log(restarr);
                        $.post("/Restaurant/GetQueryResults", { queryobj:queryarr,restobj:restarr,keyobj:kwarr }, function (data) {
                            alert(data);
                        });
                    }
                }


                var geocoder = new google.maps.Geocoder;

                var service = new google.maps.DistanceMatrixService;
                service.getDistanceMatrix({
                    origins: [origin1/*, origin2*/],
                    destinations: destinations,
                    travelMode: 'DRIVING',
                    unitSystem: google.maps.UnitSystem.METRIC,
                    avoidHighways: false,
                    avoidTolls: false
                }, function (response, status) {
                    if (status !== 'OK') {
                        alert('Error was: ' + status);
                    } else {
                        var originList = response.originAddresses;
                        var destinationList = response.destinationAddresses;
                        var outputDiv = document.getElementById('output');
                        outputDiv.innerHTML = '';
                        deleteMarkers(markersArray);

                        var showGeocodedAddressOnMap = function (asDestination) {
                            var icon = asDestination ? destinationIcon : originIcon;
                            return function (results, status) {
                                if (status === 'OK') {
                                    map.fitBounds(bounds.extend(results[0].geometry.location));
                                    markersArray.push(new google.maps.Marker({
                                        map: map,
                                        position: results[0].geometry.location,
                                        icon: icon
                                    }));
                                } else {
                                    alert('Geocode was not successful due to: ' + status);
                                }
                            };
                        };

                        for (var i = 0; i < originList.length; i++) {
                            var results = response.rows[i].elements;
                            geocoder.geocode({ 'address': originList[i] },
                                showGeocodedAddressOnMap(false));
                            for (var j = 0; j < results.length; j++) {
                                geocoder.geocode({ 'address': destinationList[j] },
                                    showGeocodedAddressOnMap(true));
                                outputDiv.innerHTML += "<b>" + destinationNames[j] + ":</b><br/>" + destinationList[j] +
                                    ' <br/> ' + "<span style='color:red'>" +
                                    results[j].duration.text + " drive</span>(" +
                                    results[j].distance.text + ")<br>"+
                                    "<a href='https://maps.google.com/?q=" + destinationNames[j] + "," + destinationList[j] + "' target='_blank'><button class='googlenav'>directions</button></a>" + " " +
                                    "<a href='Favorites/Create/" +json.results[j].id+"'><button class='googlenav'> add to faves</button></a> <br><br>";
                                //https://maps.google.com/?q=Matsutake, 13049 Worldgate Dr, Herndon
                            }
                        }
                    }
                });



                //$('.loc').html(title);


            }
        });

        ////////


        }

    });





}

//google.maps.event.addDomListener(window, 'load', initMap);




// }
function deleteMarkers(markersArray) {
    for (var i = 0; i < markersArray.length; i++) {
        markersArray[i].setMap(null);
    }
    markersArray = [];
}




    

      

