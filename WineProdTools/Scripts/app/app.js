'use strict';

var app = angular.module('wineProductionToolsApp', ['ngRoute', 'infinite-scroll']);

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
        .when('/deletetank/:tankId', {
            templateUrl: 'Scripts/app/views/deletetank.html',
            controller: 'DeleteTankCtrl'
        })
        .when('/addnote', {
            templateUrl: 'Scripts/app/views/addnote.html',
            controller: 'AddNoteCtrl'
        })
        .when('/tankdashboard/:tankId', {
            templateUrl: 'Scripts/app/views/tankdashboard.html',
            controller: 'TankDashboardCtrl'
        })
        .when('/editaccount', {
            templateUrl: 'Scripts/app/views/editaccount.html',
            controller: 'EditAccountCtrl'
        })
        .otherwise({
            redirectTo: '/'
        });
}]);