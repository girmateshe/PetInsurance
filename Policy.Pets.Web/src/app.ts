/// <reference path="_all.ts" />

module App {
    'use strict';

    angular.module('policyApp', ['controllers', 'ngResource', 'ngRoute', 'ui.router'])
        .config(['$stateProvider', ($stateProvider) => {
                App.Configs.RouteConfig.config($stateProvider);
            }]
        );
}
