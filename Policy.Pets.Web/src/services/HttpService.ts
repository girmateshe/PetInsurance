
module App.Services {
    'use strict';

    export class HttpService {

        private items: Array<IPetPolicy> = [];

        public static factory($http: ng.IHttpService, $q: ng.IQService): HttpService {
            return new HttpService($http, $q);
        }

        constructor(
            private $http: ng.IHttpService,
            private $q: ng.IQService) { }

        getItems(): Array<IPetPolicy> {
            return this.items;
        }

        addArticle(item: IPetPolicy) {
            this.items.push(item);
        }
    }

}