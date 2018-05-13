(function () {

    SightingMapping = function(data) {
        this.SightingId = ko.observable(),
        this.Name = ko.observable(),
        this.Pokemon = ko.observable().extend({ required: { params: true, message: "What pokemon did you see?" } }),
        this.Time = ko.observable().extend({ required: { params: true, message: "When did you see this pokemon?" } }),
        this.Rarity = ko.observable().extend({ required: { params: true, message: "How rare is it?" } }),
        this.Rating = ko.observable()
        this.Type = ko.observable(0)
        this.Notes = ko.observable()

        if (data)
            this.mapData(data);
    }

    $.extend(SightingMapping.prototype, {
        mapData: function (data) {
            if (data) {
                this.Latitude = ko.observable(data.Latitude)
                this.Longitude = ko.observable(data.Longitude)
                this.PokeMon = ko.observable(data.PokeMon)
                this.Rarity = ko.observable(data.Rarity)
                this.Rating = ko.observable(data.Rating)
                this.SightingId = ko.observable(data.SightingId)
                this.TimeOfDay = ko.observable(data.TimeOfDay)
                this.Type = ko.observable(data.Type)
                this.Notes = ko.observable(data.Notes)
            }
        },
    });

})();

