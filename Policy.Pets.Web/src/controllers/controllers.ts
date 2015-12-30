/// <reference path="../services/policiesservice.ts" />
/// <reference path="../services/httpservice.ts" />

module App.Controllers {
    'use strict';

    angular.module('controllers', ['services'])
        .controller('policiesController', PoliciesController)
        .controller('httpController', HttpController);
}