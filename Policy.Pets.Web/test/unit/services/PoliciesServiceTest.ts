
describe("PetPolicyService", () => {

    var $httpBackend: ng.IHttpBackendService;
    var policyService: App.Services.IPetPolicyResource;

    var baseUrl: string = "http://api.petsinsurance.com/api/v1/policies";
    var policiesUrl = baseUrl + '';


    beforeEach(module('policyApp'));

    beforeEach(() => {
        inject((_$httpBackend_, _policyService_) => {
            $httpBackend = _$httpBackend_;
            policyService = _policyService_;
        });
    });

    afterEach(() => {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    
    it("should initialize correctly", () => {
        expect(policyService).toBeDefined();
    });
    
    it("should load channels", () => {

        $httpBackend.expectGET(policiesUrl).respond([
            {
                id: 0,
                name: "Test",
                policyNumber: null,
                policyDate: "0001-01-01T00:00:00",
                countryIsoCode: "ETH",
                email: "test@gmail.com",
                pets: null
            },
            {
                id: 0,
                name: "Test",
                policyNumber: null,
                policyDate: "0001-01-01T00:00:00",
                countryIsoCode: "ETH",
                email: "test@gmail.com",
                pets: null
            }
        ]);

            var policies = policyService.query(() => {
                
                expect(policies).toBeDefined();
                expect(policies).not.toBe(null);
                expect(policies.length).toBe(2);

                var policy = policies[0];
                expect(policy.id).toBe(0);
                expect(policy.name).toBe("Test");
                expect(policy.policyNumber).toBe(null);
                expect(policy.policyDate).toBe("0001-01-01T00:00:00");
                expect(policy.countryIsoCode).toBe("ETH");
                expect(policy.email).toBe("test@gmail.com");
                expect(policy.pets).toBe(null);
            });
            
        $httpBackend.flush();
    }); 
});