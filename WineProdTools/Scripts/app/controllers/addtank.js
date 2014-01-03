'use strict';

app.controller('AddTankCtrl', ['$scope', '$location', 'Tanks', function ($scope, $location, Tanks) {

    $scope.tank = {
        name: '',
        gallons: ''
    };

    $scope.errors = null;

    $scope.add = function () {
        $scope.errors = { modelState: [] };
        $scope.$parent.waiting = true;
        Tanks.addTank($scope.tank).success(function (data) {
            $scope.$parent.waiting = false;
            $location.path('/tanks');
        }).error(function (data) {
            $scope.$parent.waiting = false;
            $scope.errors = data;
        });
    };

}]);