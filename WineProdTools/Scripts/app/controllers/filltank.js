'use strict';

app.controller('FillTankCtrl', ['$scope', '$routeParams', '$location', 'Tanks', function ($scope, $routeParams, $location, Tanks) {

    $scope.errors = null;
    $scope.analysis = 'keep';
    $scope.$parent.loading = true;

    Tanks.getContentStates().success(function (data) {
        $scope.states = data;
    }).then(function () {
        Tanks.getTank($routeParams.tankId).success(function (data) {
            data.xPosition = 200;   // Don't care where the tank is supposed to be, this is only for static display purposes.
            data.yPosition = 150;
            $scope.tank = [data];
            $scope.transfer = angular.copy(data.contents);
            $scope.transfer.gallons = null
            $scope.transfer.state = $scope.states.filter(function (state) { return state.id === data.contents.state.id; })[0];
            $scope.$parent.loading = false;
        });
    },
    function (data) {
        $scope.$parent.loading = false;
    });
    
    $scope.fill = function () {
        $scope.transfer.fromId = 0;
        $scope.transfer.toId = $routeParams.tankId;

        Tanks.tankTransfer($scope.transfer).success(function (data) {
            $location.path('/transfers');
        }).error(function (data) {
            $scope.errors = data;
        });
    };

}]);