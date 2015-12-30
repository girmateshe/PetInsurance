/// <reference path="../_all.ts" />
/// <reference path="../services/httpservice.ts" />

module App.Controllers {
    'use strict';

    //Interface to describe the scope
    export interface IHttpServiceScope extends ng.IScope {
        vm: HttpController;
    }

    export class HttpController {

        public static $inject = [
            '$scope', 'httpService'
        ];

        constructor(private $scope: IHttpServiceScope, public http: rs.HttpService) {
            this.$scope.vm = this;
        }
    }
}