var App;
(function (App) {
    (function (Services) {
        'use strict';

        var PetsService = (function () {
            function PetsService() {
            }
            PetsService.factory = function ($resource) {
                var baseUrl = "http://api.petsinsurance.com/api/v1/pets/:id";

                return $resource(baseUrl, { policyId: '@policyId', id: '@id' }, {
                    query: { method: 'GET', params: {}, url: 'http://api.petsinsurance.com/api/v1/policies/:policyId/pets/:id', isArray: true },
                    get: { method: 'GET' },
                    save: { method: 'POST' },
                    update: { method: 'PUT' },
                    delete: { method: 'DELETE' }
                });
            };
            return PetsService;
        })();
        Services.PetsService = PetsService;
    })(App.Services || (App.Services = {}));
    var Services = App.Services;
})(App || (App = {}));
