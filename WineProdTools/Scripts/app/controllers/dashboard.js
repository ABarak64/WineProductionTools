'use strict';

app.controller('DashboardCtrl', ['$scope', '$routeParams', 'Accounts', function ($scope, $routeParams, Accounts) {

    Accounts.getAccount().success(function (data) {
        console.log(data);
        $scope.account = data;
    });

}]);