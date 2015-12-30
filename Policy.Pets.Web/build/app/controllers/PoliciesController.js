/// <reference path="../_all.ts" />
/// <reference path="../services/policiesservice.ts" />
var App;
(function (App) {
    (function (Controllers) {
        'use strict';

        

        var PoliciesController = (function () {
            function PoliciesController($scope, $state, $stateParams, policy, country) {
                this.$scope = $scope;
                this.$state = $state;
                this.$stateParams = $stateParams;
                this.policy = policy;
                this.country = country;
                this.$scope.vm = this;
                this.init();
                if ($stateParams.id) {
                    $scope.policy = policy.get({ id: $stateParams.id });
                } else {
                    $scope.policy = new policy();
                    $scope.policies = policy.query();
                }
            }
            PoliciesController.prototype.init = function () {
                this.$scope.countries = this.country.query();
                console.log(this.$scope.countries);
            };

            PoliciesController.prototype.addPolicy = function () {
                var _this = this;
                console.log(this.$scope.policy);
                this.$scope.policy.$save(function () {
                    _this.$state.go('policies'); // on success go back to home i.e. policies state.
                });
            };

            PoliciesController.prototype.showPopup = function (message) {
                console.log(message);
                return true;
            };

            PoliciesController.prototype.cancelPolicy = function (policy) {
                var _this = this;
                if (this.showPopup('Really delete this?')) {
                    policy.$delete({ id: policy.id }, function () {
                        _this.$state.go('policies'); // on success go back to home i.e. policies state.
                    });
                }
            };
            PoliciesController.$inject = [
                '$scope', '$state', '$stateParams', 'policyService', 'countryService'
            ];
            return PoliciesController;
        })();
        Controllers.PoliciesController = PoliciesController;
    })(App.Controllers || (App.Controllers = {}));
    var Controllers = App.Controllers;
})(App || (App = {}));
