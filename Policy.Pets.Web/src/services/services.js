/// <reference path="../_all.ts" />
var App;
(function (App) {
    var Services;
    (function (Services) {
        'use strict';
        angular.module('services', [])
            .factory('policyService', ['$resource', App.Services.PoliciesService.factory])
            .factory('petsService', ['$resource', App.Services.PetsService.factory])
            .factory('countryService', ['$resource', App.Services.CountriesService.factory])
            .factory('breedService', ['$resource', App.Services.BreedsService.factory])
            .factory('httpService', ['$http', '$q', App.Services.HttpService.factory]);
    })(Services = App.Services || (App.Services = {}));
})(App || (App = {}));
//# sourceMappingURL=services.js.map