module App.Services {
    'use strict';

    export interface IBreed extends ng.resource.IResource<IBreed> {
        id: string;
        name: string;
    }

    export interface IBreedResource extends ng.resource.IResourceClass<IBreed> {
    }

    export class BreedsService {
        public static factory($resource: ng.resource.IResourceService): IBreedResource {
            var baseUrl = "http://api.petsinsurance.com/api/v1/breeds";
            return <IBreedResource> $resource(baseUrl , {id: '@id'}, {});
        }
    }

} 