/// <reference path="../_all.ts" />
/// <reference path="../services/policiesservice.ts" />

module App.Controllers {
    'use strict';

    //Interface to describe the scope
    export interface IPetPolicyScope extends ng.IScope {
        policies: Array<rs.IPetPolicy>;
        policy: rs.IPetPolicy;
        countries: Array<rs.ICountry>;
        vm: PoliciesController;
    }

    export class PoliciesController {

        public init(): void {
            this.$scope.countries = this.country.query();
            console.log(this.$scope.countries);
        }

        public static $inject = [
            '$scope', '$state', '$stateParams', 'policyService', 'countryService'
        ];

        constructor(private $scope: IPetPolicyScope,
                    private $state,
                    private $stateParams,
                    public policy: rs.IPetPolicyResource,
                    public country: rs.ICountryResource) {
            this.$scope.vm = this;
            this.init();
            if ($stateParams.id) {
                $scope.policy = policy.get({id: $stateParams.id});
            } else {
                $scope.policy = new policy();
                $scope.policies = policy.query();
            }
        }

        public addPolicy(): void {
            console.log(this.$scope.policy);
            this.$scope.policy.$save(() => {
                this.$state.go('policies'); // on success go back to home i.e. policies state.
            });
        }

        public showPopup(message: string): boolean {
            console.log(message);
            return true;
        }

        public cancelPolicy(policy: rs.IPetPolicy): void {
            if (this.showPopup('Really delete this?')) {
                policy.$delete({id: policy.id}, () => {
                    this.$state.go('policies'); // on success go back to home i.e. policies state.
                });
            }
        }

        /*
        public updatePolicy(): void {
            console.log(this.$scope.policy);
            this.$scope.policy.$update(() => {
                this.$state.go('policies'); // on success go back to home i.e. policies state.
            });
        }
        */
    }
}