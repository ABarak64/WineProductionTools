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
        .when('/emptytank/:tankId', {
            templateUrl: 'Scripts/app/views/emptytank.html',
            controller: 'EmptyTankCtrl'
        })
        .when('/filltank/:tankId', {
            templateUrl: 'Scripts/app/views/filltank.html',
            controller: 'FillTankCtrl'
        })
        .when('/editcontents/:tankId', {
            templateUrl: 'Scripts/app/views/editcontents.html',
            controller: 'EditContentsCtrl'
        })
        .when('/deletetank/:tankId', {
            templateUrl: 'Scripts/app/views/deletetank.html',
            controller: 'DeleteTankCtrl'
        })
        .when('/tanktransfer/from/:fromTankId/to/:toTankId', {
            templateUrl: 'Scripts/app/views/tanktransfer.html',
            controller: 'TankTransferCtrl'
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
        .when('/transfers', {
            templateUrl: 'Scripts/app/views/transfers.html',
            controller: 'TransfersCtrl'
        })
        .otherwise({
            redirectTo: '/'
        });
}]);