﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function initMap() {
    navigator.geolocation.getCurrentPosition(function (position) {
        var pos = {
            lat: position.coords.latitude,
            lng: position.coords.longitude
        };
        lat = pos.lat;
        lng = pos.lng;


        /////////////////////
        ////////////
        //GOOGLE MAPS DISTANCE


        var bounds = new google.maps.LatLngBounds;
        var markersArray = [];


        // var origin2 = 'Montgomery, Alabama';
        var destinationA = 'Manhattan, New York';
        // var destinationB = 'Columbus, Ohio';

        var destinationIcon = 'https://chart.googleapis.com/chart?' +
            'chst=d_map_pin_letter&chld=D|FF0000|000000';
        var originIcon = 'https://chart.googleapis.com/chart?' +
            'chst=d_map_pin_letter&chld=O|FFFF00|000000';
        var map = new google.maps.Map(document.getElementById('map'), {
            center: { lat: 37.1925852, lng: -123.8105744 },//:38.9706559,"lng":-77.3854458
            zoom: 100
        });
        map.setCenter(pos);
        var origin1 = { lat: pos.lat, lng: pos.lng };


        //////////////////\//////////////
        //IMPLEMENTING PLACES FUNCTI
        const PROXY_URL = "https://cors-anywhere.herokuapp.com/";
        const TARGET_URL = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=" + origin1.lat + "," + origin1.lng + "&radius=2000&type=restaurant&keyword="+query+"&key=AIzaSyCji7qavH8NTarPPbIyYNCtw7TJnmZHKr0";
        const URL = PROXY_URL + TARGET_URL;
        var destinations = [];
        var destinationNames = [];

        $.getJSON(URL, function (json) {

            if (json.results === null) {
                $('.loc').html("nope");
            }
            else {


                var title = '';
                for (var i = 0; i < json.results.length; i++) {

                    destinations.push(json.results[i].geometry.location);
                    //title += "<p>"+JSON.stringify(json.results[i].geometry.location)+"</p>";
                    destinationNames.push(JSON.stringify(json.results[i].name));

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
                                    results[j].distance.text + ')<br><br>';
                            }
                        }
                    }
                });



                //$('.loc').html(title);


            }
        });

        //////////



    });





}


// }
function deleteMarkers(markersArray) {
    for (var i = 0; i < markersArray.length; i++) {
        markersArray[i].setMap(null);
    }
    markersArray = [];
}




    

      

