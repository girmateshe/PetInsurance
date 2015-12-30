module App.Services {
    'use strict';

    export interface ICountry extends ng.resource.IResource<ICountry> {
        name: string;
        isoCode: string;
    }

    export interface ICountryResource extends ng.resource.IResourceClass<ICountry> {
    }

    export class CountriesService {
        public static factory($resource: ng.resource.IResourceService): ICountryResource {
            var baseUrl = "http://api.petsinsurance.com/api/v1/countries";
            return <ICountryResource> $resource(baseUrl , {id: '@id'}, {});
        }
    }

} 