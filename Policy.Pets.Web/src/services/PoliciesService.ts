module App.Services {
    'use strict';

    export interface IPetPolicy extends ng.resource.IResource<IPetPolicy> {
        id: number;
        name: string;
        policyNumber: string;
        policyDate: string;
        countryIsoCode: string;
        email: string;
        pets: string;
    }

    export interface IPetPolicyResource extends ng.resource.IResourceClass<IPetPolicy> {
        update();
    }

    export class PoliciesService {
        public static factory($resource: ng.resource.IResourceService): IPetPolicyResource {

            var baseUrl = "http://api.petsinsurance.com/api/v1/policies/:id";

            // Return the resource, include your custom actions
            return <IPetPolicyResource> $resource(baseUrl , {id: '@id'}, {
                query: { method: 'GET', params: {}, isArray: true },
                get: { method: 'GET' },
                save: { method: 'POST' },
                update: { method: 'PUT' },
                delete: { method: 'DELETE' }
                //countries: { method: 'GET', url: 'http://api.petsinsurance.com/api/v1/countries', isArray: true }
            });
        }
    }

} 