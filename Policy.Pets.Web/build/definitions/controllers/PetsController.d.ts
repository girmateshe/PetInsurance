declare module App.Controllers {
    interface IPetsScope extends ng.IScope {
        pets: Services.IPet[];
        pet: Services.IPet;
        policyId: number;
        breeds: Services.IBreed[];
        vm: PetsController;
    }
    class PetsController {
        private $scope;
        private $state;
        private $stateParams;
        public pet: Services.IPetResource;
        public breed: Services.IBreedResource;
        public init(): void;
        static $inject: string[];
        constructor($scope: IPetsScope, $state: any, $stateParams: any, pet: Services.IPetResource, breed: Services.IBreedResource);
        public addPet(): void;
        public showPopup(message: string): boolean;
        public deletePetFromPolicy(pet: Services.IPet): void;
    }
}
