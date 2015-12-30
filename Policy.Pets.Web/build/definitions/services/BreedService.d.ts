declare module App.Services {
    interface IBreed extends ng.resource.IResource<IBreed> {
        id: string;
        name: string;
    }
    interface IBreedResource extends ng.resource.IResourceClass<IBreed> {
    }
    class BreedsService {
        static factory($resource: ng.resource.IResourceService): IBreedResource;
    }
}
