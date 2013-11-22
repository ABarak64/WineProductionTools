﻿'use strict';

var app = angular.module('wineProductionToolsApp', ['ngRoute']);

app.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
        .when('/', {
            templateUrl: 'Scripts/app/views/dashboard.html',
            controller: 'DashboardCtrl'
        })
        .when('/tanks', {
            templateUrl: 'Scripts/app/views/tanks.html',
            controller: 'TanksCtrl'
        })
        .when('/addtank', {
            templateUrl: 'Scripts/app/views/addtank.html',
            controller: 'AddTankCtrl'
        })
        .otherwise({
            redirectTo: '/'
        });
}]);