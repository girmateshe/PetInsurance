declare module App.Services {
    interface IPet extends ng.resource.IResource<IPetPolicy> {
        id: number;
        name: string;
        dateOfBirth: string;
        enrollmentDate: string;
        petOwnerId: string;
        breedName: string;
        archived: boolean;
    }
    interface IPetResource extends ng.resource.IResourceClass<IPet> {
    }
    class PetsService {
        static factory($resource: ng.resource.IResourceService): IPetResource;
    }
}
