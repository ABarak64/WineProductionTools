'use strict';

app.controller('AddTankCtrl', ['$scope', '$location', 'Tanks', function ($scope, $location, Tanks) {

    $scope.tank = {
        name: '',
        gallons: ''
    };

    $scope.add = function () {
        Tanks.addTank($scope.tank).success(function (data) {
            $location.path('/tanks');
        }).error(function (data) {
            console.log('error');
            console.log(data);
        });
    };

}]);