/// <reference path="../_all.ts" />
/// <reference path="../services/policiesservice.ts" />

module App.Controllers {
    'use strict';

    //Interface to describe the scope
    export interface IPetsScope extends ng.IScope {
        pets: Array<rs.IPet>;
        pet: rs.IPet;
        policyId: number;
        breeds: Array<rs.IBreed>;
        vm: PetsController;
    }

    export class PetsController {

        public init(): void {
            this.$scope.policyId = this.$stateParams.policyId;
            this.$scope.breeds = this.breed.query();
            console.log(this.$scope.breeds);
        }

        public static $inject = [
            '$scope', '$state', '$stateParams', 'petsService', 'breedService'
        ];

        constructor(private $scope: IPetsScope,
                    private $state,
                    private $stateParams,
                    public pet: rs.IPetResource,
                    public breed: rs.IBreedResource) {
            this.$scope.vm = this;
            this.init();
            if ($stateParams.id) {
                $scope.pet = pet.get({id: $stateParams.id});
            } else {
                $scope.pet = new pet();
                $scope.pets = pet.query({ policyId: $stateParams.policyId });
            }
        }

        public addPet(): void {
            this.$scope.pet.petOwnerId = this.$stateParams.policyId;
            console.log(this.$scope.pet);
            this.$scope.pet.$save(() => {
                this.$state.go('pets', {policyId:this.$scope.policyId});
            });
        }

        public showPopup(message: string): boolean {
            console.log(message);
            return true;
        }

        public deletePetFromPolicy(pet: rs.IPet): void {
            if (this.showPopup('Really delete this?')) {
                pet.$delete({ id: pet.id }, () => {
                    this.$state.go('pets');
                });
            }
        }

        /*
        public updatePet(): void {
            console.log(this.$scope.pet);
            this.$scope.pet.$update(() => {
                this.$state.go('pets'); // on success go back to home i.e. policies state.
            });
        }
        */
    }
}