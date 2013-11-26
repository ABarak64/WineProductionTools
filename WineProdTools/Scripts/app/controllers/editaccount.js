'use strict';

app.controller('EditAccountCtrl', ['$scope', '$location', 'Accounts', function ($scope, $location, Accounts) {

    Accounts.getAccount().success(function (data) {
        $scope.account = data;
    });

    $scope.errors = null;

    $scope.save = function () {
        Accounts.updateAccount($scope.account).success(function (data) {
            $location.path('/dashboard');
        }).error(function (data) {
            $scope.errors = data;
        });
    };

}]);