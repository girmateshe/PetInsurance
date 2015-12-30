/// <reference path="../services/policiesservice.ts" />
/// <reference path="../services/httpservice.ts" />
var App;
(function (App) {
    var Controllers;
    (function (Controllers) {
        'use strict';
        angular.module('controllers', ['services'])
            .controller('policiesController', Controllers.PoliciesController)
            .controller('httpController', Controllers.HttpController);
    })(Controllers = App.Controllers || (App.Controllers = {}));
})(App || (App = {}));
//# sourceMappingURL=controllers.js.map