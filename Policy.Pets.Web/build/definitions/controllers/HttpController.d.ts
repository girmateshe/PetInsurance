declare module App.Controllers {
    interface IHttpServiceScope extends ng.IScope {
        vm: HttpController;
    }
    class HttpController {
        private $scope;
        public http: Services.HttpService;
        static $inject: string[];
        constructor($scope: IHttpServiceScope, http: Services.HttpService);
    }
}
