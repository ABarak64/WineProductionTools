'use strict';

app.controller('TanksCtrl', ['$scope', 'Tanks', function ($scope, Tanks) {

    $scope.movedTank = null;

    Tanks.getTanks().success(function (data) {
        $scope.tanks = data;
    });
        
    $scope.$watch('movedTank', function (updatedTank, oldVal) {
        if (updatedTank !== null) {
            Tanks.updateTank(updatedTank);
        }
    }, true);

}]);