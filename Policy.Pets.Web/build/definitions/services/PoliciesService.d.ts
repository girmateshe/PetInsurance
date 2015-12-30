declare module App.Services {
    interface IPetPolicy extends ng.resource.IResource<IPetPolicy> {
        id: number;
        name: string;
        policyNumber: string;
        policyDate: string;
        countryIsoCode: string;
        email: string;
        pets: string;
    }
    interface IPetPolicyResource extends ng.resource.IResourceClass<IPetPolicy> {
        update(): any;
    }
    class PoliciesService {
        static factory($resource: ng.resource.IResourceService): IPetPolicyResource;
    }
}
