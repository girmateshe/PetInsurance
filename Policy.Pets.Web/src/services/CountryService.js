var App;
(function (App) {
    var Services;
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
    })(Services = App.Services || (App.Services = {}));
})(App || (App = {}));
//# sourceMappingURL=CountryService.js.map