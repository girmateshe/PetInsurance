var App;
(function (App) {
    (function (Services) {
        'use strict';

        var BreedsService = (function () {
            function BreedsService() {
            }
            BreedsService.factory = function ($resource) {
                var baseUrl = "http://api.petsinsurance.com/api/v1/breeds";
                return $resource(baseUrl, { id: '@id' }, {});
            };
            return BreedsService;
        })();
        Services.BreedsService = BreedsService;
    })(App.Services || (App.Services = {}));
    var Services = App.Services;
})(App || (App = {}));
