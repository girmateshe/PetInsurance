var App;
(function (App) {
    (function (Services) {
        'use strict';

        var CountriesService = (function () {
            function CountriesService() {
            }
            CountriesService.factory = function ($resource) {
                var baseUrl = "http://api.petsinsurance.com/api/v1/countries";
                return $resource(baseUrl, { id: '@id' }, {});
            };
            return CountriesService;
        })();
        Services.CountriesService = CountriesService;
    })(App.Services || (App.Services = {}));
    var Services = App.Services;
})(App || (App = {}));
