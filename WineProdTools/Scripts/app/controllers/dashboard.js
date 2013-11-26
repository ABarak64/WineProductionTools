'use strict';

app.controller('DashboardCtrl', ['$scope', '$routeParams', 'Accounts', 'Notes', function ($scope, $routeParams, Accounts, Notes) {

    $scope.account = {
        name: 'Loading Winery...'
    };

    Accounts.getAccount().success(function (data) {
        $scope.account = data;
    });

    Notes.getNotes().success(function (data) {
        $scope.notes = data;
    });

}]);