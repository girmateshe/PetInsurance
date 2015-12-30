var App;
(function (App) {
    var Services;
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
    })(Services = App.Services || (App.Services = {}));
})(App || (App = {}));
//# sourceMappingURL=BreedService.js.map