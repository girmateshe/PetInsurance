/// <reference path="../_all.ts" />

module App.Services {
    'use strict';

    angular.module('services', [])
        .factory('policyService', ['$resource', App.Services.PoliciesService.factory])
        .factory('petsService', ['$resource', App.Services.PetsService.factory])
        .factory('countryService', ['$resource', App.Services.CountriesService.factory])
        .factory('breedService', ['$resource', App.Services.BreedsService.factory])
        .factory('httpService', ['$http', '$q', App.Services.HttpService.factory]);
}