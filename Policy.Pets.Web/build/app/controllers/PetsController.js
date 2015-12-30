/// <reference path="../_all.ts" />
/// <reference path="../services/policiesservice.ts" />
var App;
(function (App) {
    (function (Controllers) {
        'use strict';

        

        var PetsController = (function () {
            function PetsController($scope, $state, $stateParams, pet, breed) {
                this.$scope = $scope;
                this.$state = $state;
                this.$stateParams = $stateParams;
                this.pet = pet;
                this.breed = breed;
                this.$scope.vm = this;
                this.init();
                if ($stateParams.id) {
                    $scope.pet = pet.get({ id: $stateParams.id });
                } else {
                    $scope.pet = new pet();
                    $scope.pets = pet.query({ policyId: $stateParams.policyId });
                }
            }
            PetsController.prototype.init = function () {
                this.$scope.policyId = this.$stateParams.policyId;
                this.$scope.breeds = this.breed.query();
                console.log(this.$scope.breeds);
            };

            PetsController.prototype.addPet = function () {
                var _this = this;
                this.$scope.pet.petOwnerId = this.$stateParams.policyId;
                console.log(this.$scope.pet);
                this.$scope.pet.$save(function () {
                    _this.$state.go('pets', { policyId: _this.$scope.policyId });
                });
            };

            PetsController.prototype.showPopup = function (message) {
                console.log(message);
                return true;
            };

            PetsController.prototype.deletePetFromPolicy = function (pet) {
                var _this = this;
                if (this.showPopup('Really delete this?')) {
                    pet.$delete({ id: pet.id }, function () {
                        _this.$state.go('pets');
                    });
                }
            };
            PetsController.$inject = [
                '$scope', '$state', '$stateParams', 'petsService', 'breedService'
            ];
            return PetsController;
        })();
        Controllers.PetsController = PetsController;
    })(App.Controllers || (App.Controllers = {}));
    var Controllers = App.Controllers;
})(App || (App = {}));
