/// <reference path="../services/policiesservice.ts" />
/// <reference path="../services/httpservice.ts" />
var App;
(function (App) {
    (function (Controllers) {
        'use strict';

        angular.module('controllers', ['services']).controller('policiesController', Controllers.PoliciesController).controller('httpController', Controllers.HttpController);
    })(App.Controllers || (App.Controllers = {}));
    var Controllers = App.Controllers;
})(App || (App = {}));
