'use strict';

app.controller('EditContentsCtrl', ['$scope', '$routeParams', '$location', 'Tanks', function ($scope, $routeParams, $location, Tanks) {

    $scope.errors = null;
    $scope.tankId = $routeParams.tankId;

    Tanks.getContentStates().success(function (data) {
        $scope.states = data;
    }).then(function () {
        Tanks.getTank($routeParams.tankId).success(function (data) {
            data.xPosition = 100;   // Don't care where the tank is supposed to be, this is only for static display purposes.
            data.yPosition = 100;
            $scope.tank = [data];
            $scope.contents = data.contents;
            $scope.contents.state = $scope.states.filter(function (state) { return state.id === data.contents.state.id; })[0];
            $scope.contents.tankId = $scope.tankId;
        });
    });

    $scope.save = function () {
        Tanks.updateTankContents($scope.contents).success(function (data) {
            $location.path('/tankdashboard/' + $scope.contents.tankId);
        }).error(function (data) {
            $scope.errors = data;
        });
    };

}]);