module App.Services {
    'use strict';

    export interface IPet extends ng.resource.IResource<IPetPolicy> {
        id: number;
        name: string;
        dateOfBirth: string;
        enrollmentDate: string;
        petOwnerId: string;
        breedName: string;
        archived: boolean;
    }

    export interface IPetResource extends ng.resource.IResourceClass<IPet> {
    }

    export class PetsService {
        public static factory($resource: ng.resource.IResourceService): IPetResource {
            var baseUrl = "http://api.petsinsurance.com/api/v1/pets/:id";

            return <IPetResource> $resource(baseUrl, { policyId: '@policyId', id: '@id' }, {
                query: { method: 'GET', params: {}, url: 'http://api.petsinsurance.com/api/v1/policies/:policyId/pets/:id', isArray: true },
                get: { method: 'GET'},
                save: { method: 'POST'},
                update: { method: 'PUT' },
                delete: { method: 'DELETE'}
            });

        }
    }

} 