var viewModel = {
    isMobile: DevExpress.devices.current().deviceType !== "desktop",


    showPopup: ko.observable(false),
    showSightingPopup: ko.observable(false),
    isLogin: ko.observable(),
    self: this,

    credentials: {
        email: ko.observable()
            .extend({ required: { params: true, message: "Email Address is required" }, email: true }),
        password: ko.observable()
            .extend({ required: { params: true, message: "Please enter a password" } }),
        confirmPassword: ko.observable()
            .extend({ required: { params: true, message: "Please confirm your password" } }),
        rememberMe: ko.observable(true),
        username: ko.observable('')
            .extend({ required: { params: true, message: "Please enter your username" } }),
    },

    Notification: {
        Text: ko.observable('test'),
    },

    ShowNotifications: function (notificationText) {
        viewModel.Notification.Text(notificationText)
        //$('#notificationContent').fadeIn(200)
        $("#notificationPanel").animate({ right: 0 }, 300)

        setTimeout(function () {
            $("#notificationPanel").animate({ right: '-300px' }, 300)
            //$('#notificationContent').fadeOut(200)
        }, 5000)
    },

    CenterOnItem: function (data) {
        viewModel.selectedListItem(data.itemData)
        map.setCenter(new google.maps.LatLng(data.itemData.Latitude, data.itemData.Longitude))
        map.setZoom(19)
    },

    clearCredentialsValidation: function () {
        viewModel.credentials.username.error('')
        viewModel.credentials.email.error('')
        viewModel.credentials.password.error('')
        viewModel.credentials.confirmPassword.error('')
    },

    ClearSightingValidation: function () {
        viewModel.selectedSighting.Time.error('')
        viewModel.credentials.Pokemon.error('')
        viewModel.credentials.Rarity.error('')
    },

    userDetails: ko.observable(),
    selectedListItem: ko.observable(),
    selectedSighting: ko.observable(),
    selectedMarker: ko.observable(),
    pokemonCollection: ko.observableArray([]),

    sightingType: [
        { Id: 0, Type: 'Pokémon' },
        { Id: 1, Type: 'Pokégym' },
        { Id: 3, Type: 'Pokéstop' }
    ],



    ShowPopup: function (isLogin) {

        if (viewModel.showPopup()) {
            viewModel.showPopup(false)
        } else {
            viewModel.showPopup(true)
        }

        viewModel.isLogin(isLogin)
    },

    showPopup: ko.observable(false),
    searchValue: ko.observable(),

    popover: {
        visible: ko.observable(),
        target: ko.observable(),
        rightClick: ko.observable(false),
    },

    markerPopover: {
        visible: ko.observable(),
        target: ko.observable()
    },

    clusterPopover: {
        visible: ko.observable(),
        target: ko.observable()
    },

    manualAddPopover: {
        visible: ko.observable(),
        target: ko.observable()
    },

    clearCredentials: function () {
        viewModel.credentials.username(undefined)
        viewModel.credentials.email(undefined)
        viewModel.credentials.password(undefined)
        viewModel.credentials.confirmPassword(undefined)
    },

    pokemon: ko.observable(),
    distinctPokemon: ko.observable(),

    votingDirection: ko.observable(),
    sightingUserRating: ko.observable("No Rating"),

    AddMarkerPoint: ko.observable(),
    selectedPosition: ko.observable(),
    addMargerEvent: ko.observable(),

    EnableMapLeftClick: function (data) {

        if (!viewModel.userDetails()) {
            viewModel.showPopup(true);
            return
        }

        if (viewModel.manualAddPopover.visible()) {
            if (viewModel.AddMarkerPoint())
                viewModel.AddMarkerPoint().setMap(null)
            viewModel.manualAddPopover.visible(false)
            google.maps.event.removeListener(viewModel.addMargerEvent());
            $(".add-button").removeClass("cancel").find("span.cancel").addClass("innactive");
            setTimeout(function () {
                $(".add-button").find("span.add").removeClass("innactive");
            }, 300);
        } else {
            viewModel.selectedPosition(null);
            viewModel.selectedSighting(new SightingMapping);
            viewModel.manualAddPopover.target($('.add-button'))
            viewModel.manualAddPopover.visible(true)
            $(".add-button").addClass("cancel").find("span.add").addClass("innactive");
            setTimeout(function () {
                $(".add-button").find("span.cancel").removeClass("innactive");
            }, 300);

            viewModel.addMargerEvent(google.maps.event.addListener(map, "click", function (e) {

                viewModel.selectedPosition(e.latLng);

                if (viewModel.AddMarkerPoint())
                    viewModel.AddMarkerPoint().setMap(null)

                viewModel.AddMarkerPoint(new google.maps.Marker({
                    map: map,
                    draggable: true,
                    animation: google.maps.Animation.DROP,
                    position: e.latLng
                }));
                //marker.addListener('click', toggleBounce);
            }));
        }


    },





    CheckVote: function () {
        $.ajax({
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(viewModel.selectedSighting().SightingId()),
            url: "/Sightings/GetUserVoteStatus",
            success: function (result) {
                if (result === "Up" || result === "Down") {
                    viewModel.votingDirection(result)
                }
                else
                    viewModel.votingDirection("NoVote");

            },
            error: function (x, status, error) {
                viewModel.ShowNotifications(x.statusText)
            }
        });
    },

    GetUserRating: function () {
        viewModel.sightingUserRating("No Rating");

        $.ajax({
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(viewModel.selectedSighting().SightingId()),
            url: "/Sightings/GetUserRating",
            success: function (result) {
                viewModel.sightingUserRating(result);

            },
            error: function (x, status, error) {
                viewModel.ShowNotifications(x.statusText)
            }
        });
    },


    DeletePin: function () {
        $.ajax({
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(viewModel.selectedMarker().Sighting.SightingId),
            url: "/Sightings/DeleteSighting",
            success: function (result) {

                viewModel.markerPopover.visible(false)

                for (var i = 0; i < markers.length; i++) {
                    if (markers[i].Sighting.SightingId === viewModel.selectedMarker().Sighting.SightingId) {
                        markers[i].setMap(null);
                        markers.splice(i, 1);
                        break;
                    }
                }
            },
            error: function (x, status, error) {
                viewModel.ShowNotifications(x.statusText)

            }
        });
    },


    GetPokemon: function () {
        $.ajax({
            type: 'GET',
            contentType: 'application/json',
            url: "/Sightings/GetPokemon",
            success: function (result) {
                viewModel.pokemon(result)
            },
            error: function (x, status, error) {
                viewModel.ShowNotifications(x.statusText)
            }
        });
    },

    nearbyLocation: ko.observable(),

    //GetNearby: function () {
    //    viewModel.nearbyLocation('')
        
    //    service.nearbySearch({
    //        location: viewModel.selectedPosition,
    //        radius: 50
    //    }, function (place, status) {

    //        switch (status) {
    //            case google.maps.GeocoderStatus.OK:
    //                viewModel.nearbyLocation(place[0].name);
    //                break;

    //            case google.maps.GeocoderStatus.ERROR:
    //                viewModel.ShowNotifications("An unexpected error occurred")
    //                viewModel.nearbyLocation('')
    //                break

    //            case google.maps.GeocoderStatus.INVALID_REQUEST:
    //                viewModel.ShowNotifications("This request is not valid")
    //                viewModel.nearbyLocation('')
    //                break

    //            case google.maps.GeocoderStatus.OVER_QUERY_LIMIT:
    //                viewModel.ShowNotifications("This api key has had too many requests")
    //                viewModel.nearbyLocation('')
    //                break

    //            case google.maps.GeocoderStatus.RQUEST_DENIED:
    //                viewModel.ShowNotifications("You do not have permission to perform this request")
    //                viewModel.nearbyLocation('')
    //                break

    //            case google.maps.GeocoderStatus.UNKNOWN_ERROR:
    //                viewModel.ShowNotifications("An unexpected error occurred")
    //                viewModel.nearbyLocation('')
    //                break

    //            case google.maps.GeocoderStatus.ZERO_RESULTS:
    //                viewModel.ShowNotifications("This search returned no results, please check the spelling")
    //                viewModel.nearbyLocation('')
    //                break

    //            default:
    //        }
    //    })
    //},




    UpdateRating: function (value) {

        if (!viewModel.userDetails()) {
            viewModel.markerPopover.visible(false)
            viewModel.showPopup(true);
            return
        }

        if (value) {
            viewModel.selectedSighting().Rating(viewModel.selectedSighting().Rating() + 1)
            viewModel.votingDirection("Up")
        } else {
            viewModel.selectedSighting().Rating(viewModel.selectedSighting().Rating() - 1)
            viewModel.votingDirection("Down")
        }

        $.ajax({
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ Id: viewModel.selectedSighting().SightingId(), Increase: value }),
            url: '/Sightings/Vote',
            success: function (result) {
                viewModel.selectedMarker().Sighting = result;
                viewModel.selectedSighting(new SightingMapping(result))
            },
            error: function (x, status, error) {
                viewModel.ShowNotifications(x.statusText)
            }
        });
    },

    Register: function (data) {
        var mappedData = {
            Email: viewModel.credentials.email(),
            Password: viewModel.credentials.password(),
            RememberMe: viewModel.credentials.rememberMe(),
            Username: viewModel.credentials.username()
        }

        var test = viewModel.credentials.username.error('')
        var errors;

        if (viewModel.isLogin()) {
            errors = ko.validation.group([viewModel.credentials.username, viewModel.credentials.password]);
        } else {
            errors = ko.validation.group([viewModel.credentials.username, viewModel.credentials.email, viewModel.credentials.password, viewModel.credentials.confirmPassword]);
        }


        if (viewModel.isLogin()) {
            viewModel.credentials.username.notifySubscribers(viewModel.credentials.username());
            viewModel.credentials.password.notifySubscribers(viewModel.credentials.password());
        } else {
            viewModel.credentials.username.notifySubscribers(viewModel.credentials.username());
            viewModel.credentials.email.notifySubscribers(viewModel.credentials.email());
            viewModel.credentials.password.notifySubscribers(viewModel.credentials.password());
            viewModel.credentials.confirmPassword.notifySubscribers(viewModel.credentials.confirmPassword());
        }

        if (viewModel.credentials.password() !== viewModel.credentials.confirmPassword() && !viewModel.isLogin()) {
            return;
        }

        if (errors().length === 0) {

            var url = viewModel.isLogin() ? '/Account/LoginPopup' : '/Account/RegisterPopup'
            $.ajax({
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(mappedData),
                url: url,
                success: function (result) {
                    viewModel.showPopup(false)
                    window.location.reload();
                    google.maps.event.addListener(map, "rightclick", function (e) {
                        viewModel.selectedSighting(new viewModel.Sighting());
                        viewModel.popover.visible(false)
                        CloseOpenInfoWindows()
                        selectedPosition = e.latLng
                        showContextMenu(e.latLng);
                        //CreateSighting(e)
                    });

                },
                error: function (x, status, error) {
                    viewModel.ShowNotifications(x.statusText)
                }
            });
        }
    }
}


