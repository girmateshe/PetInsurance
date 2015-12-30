var App;
(function (App) {
    var Services;
    (function (Services) {
        'use strict';
        var HttpService = (function () {
            function HttpService($http, $q) {
                this.$http = $http;
                this.$q = $q;
                this.items = [];
            }
            HttpService.factory = function ($http, $q) {
                return new HttpService($http, $q);
            };
            HttpService.prototype.getItems = function () {
                return this.items;
            };
            HttpService.prototype.addArticle = function (item) {
                this.items.push(item);
            };
            return HttpService;
        })();
        Services.HttpService = HttpService;
    })(Services = App.Services || (App.Services = {}));
})(App || (App = {}));
//# sourceMappingURL=httpservice.js.map