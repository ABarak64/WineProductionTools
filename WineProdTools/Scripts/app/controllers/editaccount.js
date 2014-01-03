'use strict';

app.controller('EditAccountCtrl', ['$scope', '$location', 'Accounts', function ($scope, $location, Accounts) {

    $scope.errors = null;
    $scope.$parent.loading = true;

    Accounts.getAccount().success(function (data) {
        $scope.account = data;
        $scope.$parent.loading = false;
    }).error(function () {
        $scope.$parent.loading = false;
    });

    $scope.save = function () {
        $scope.errors = { modelState: [] };
        $scope.$parent.waiting = true;
        Accounts.updateAccount($scope.account).success(function (data) {
            $scope.$parent.waiting = false;
            $location.path('/dashboard');
        }).error(function (data) {
            $scope.$parent.waiting = false;
            $scope.errors = data;
        });
    };

}]);