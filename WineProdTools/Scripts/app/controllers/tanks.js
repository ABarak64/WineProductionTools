'use strict';

app.controller('TanksCtrl', ['$scope', '$location', 'Tanks', function ($scope, $location, Tanks) {

    $scope.movedTank = null;
    $scope.selectedTank = null;

    Tanks.getTanks().success(function (data) {
        $scope.tanks = data;
    });
        
    $scope.$watch('movedTank', function (updatedTank, oldVal) {
        if (updatedTank !== null) {
            Tanks.updateTank(updatedTank);
        }
    }, true);

    $scope.$watch('selectedTank', function (selectedTank, oldVal) {
        if (selectedTank !== null) {
            $location.path('/tankdashboard/' + selectedTank.id);
        }
    });

}]);