var App;
(function (App) {
    var Services;
    (function (Services) {
        'use strict';
        var PoliciesService = (function () {
            function PoliciesService() {
            }
            PoliciesService.factory = function ($resource) {
                var baseUrl = "http://api.petsinsurance.com/api/v1/policies/:id";
                // Return the resource, include your custom actions
                return $resource(baseUrl, { id: '@id' }, {
                    query: { method: 'GET', params: {}, isArray: true },
                    get: { method: 'GET' },
                    save: { method: 'POST' },
                    update: { method: 'PUT' },
                    delete: { method: 'DELETE' }
                });
            };
            return PoliciesService;
        })();
        Services.PoliciesService = PoliciesService;
    })(Services = App.Services || (App.Services = {}));
})(App || (App = {}));
//# sourceMappingURL=policiesservice.js.map