declare module App.Controllers {
    interface IPetPolicyScope extends ng.IScope {
        policies: Services.IPetPolicy[];
        policy: Services.IPetPolicy;
        countries: Services.ICountry[];
        vm: PoliciesController;
    }
    class PoliciesController {
        private $scope;
        private $state;
        private $stateParams;
        public policy: Services.IPetPolicyResource;
        public country: Services.ICountryResource;
        public init(): void;
        static $inject: string[];
        constructor($scope: IPetPolicyScope, $state: any, $stateParams: any, policy: Services.IPetPolicyResource, country: Services.ICountryResource);
        public addPolicy(): void;
        public showPopup(message: string): boolean;
        public cancelPolicy(policy: Services.IPetPolicy): void;
    }
}
