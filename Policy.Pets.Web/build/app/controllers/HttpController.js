/// <reference path="../_all.ts" />
/// <reference path="../services/httpservice.ts" />
var App;
(function (App) {
    (function (Controllers) {
        'use strict';

        

        var HttpController = (function () {
            function HttpController($scope, http) {
                this.$scope = $scope;
                this.http = http;
                this.$scope.vm = this;
            }
            HttpController.$inject = [
                '$scope', 'httpService'
            ];
            return HttpController;
        })();
        Controllers.HttpController = HttpController;
    })(App.Controllers || (App.Controllers = {}));
    var Controllers = App.Controllers;
})(App || (App = {}));
