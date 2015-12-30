declare module App.Services {
    interface ICountry extends ng.resource.IResource<ICountry> {
        name: string;
        isoCode: string;
    }
    interface ICountryResource extends ng.resource.IResourceClass<ICountry> {
    }
    class CountriesService {
        static factory($resource: ng.resource.IResourceService): ICountryResource;
    }
}
