(function () {
    LoadMap = function(){

        // Google has tweaked their interface somewhat - this tells the api to use that new UI
        google.maps.visualRefresh = true;
        var UK = new google.maps.LatLng(53.686109, -2.065517);
        // These are options that set initial zoom level, where the map is centered globally to start, and the type of map to show
        var mapOptions = {
            zoom: 6,
            center: UK,
            mapTypeId: google.maps.MapTypeId.G_NORMAL_MAP
        };

        // This makes the div with id "map_canvas" a google map
        var map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);


        // you can either make up a JSON list server side, or call it from a controller using JSONResult
        var data = [""];


        $.each(data, function (i, item) {
            var marker = new google.maps.Marker({
                'position': new google.maps.LatLng(53.231018, -0.534672),
                'map': map,
                'title': item.PlaceName
            });

            // Make the marker-pin blue!
            marker.setIcon('http://maps.google.com/mapfiles/ms/icons/blue-dot.png')

            // put in some information about each json object - in this case, the opening hours.
            var infowindow = new google.maps.InfoWindow({
                content: "<div class='infoDiv'><h2>" + item.PlaceName + "</div></div>"
            });

            // finally hook up an "OnClick" listener to the map so it pops up out info-window when the marker-pin is clicked!
            google.maps.event.addListener(marker, 'click', function () {
                infowindow.open(map, marker);
            });

        })
    }
})