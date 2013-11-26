'use strict';

app.controller('DashboardCtrl', ['$scope', '$routeParams', 'Accounts', function ($scope, $routeParams, Accounts) {

    $scope.account = {
        name: 'Loading Winery...'
    };

    Accounts.getAccount().success(function (data) {
        $scope.account = data;
    });

}]);