declare module App.Services {
    class HttpService {
        private $http;
        private $q;
        private items;
        static factory($http: ng.IHttpService, $q: ng.IQService): HttpService;
        constructor($http: ng.IHttpService, $q: ng.IQService);
        public getItems(): IPetPolicy[];
        public addArticle(item: IPetPolicy): void;
    }
}
