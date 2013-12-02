'use strict';

app.controller('TransfersCtrl', ['$scope', '$location', 'Tanks', function ($scope, $location, Tanks) {

    $scope.selectedTank = null;
    $scope.transfer = null;
    $scope.transferMessages = [];

    Tanks.getTanks().success(function (data) {
        $scope.tanks = data;
    });

    $scope.redirectToTransfer = function () {
        console.log($scope.transfer);
        if ($scope.transfer.from === 'external') {
            $location.path('/filltank/' + $scope.transfer.to);
        } else if ($scope.transfer.to === 'external') {
            $location.path('/emptytank/' + $scope.transfer.from);
        } else {
            // transfer
        }
    };

    $scope.$watch('selectedTank', function (selectedTank, oldVal) {
        if (selectedTank !== null) {
            // If the user is starting a transfer.
            if ($scope.transfer === null) {
                $scope.transfer = {
                    from: selectedTank.id,
                    to: null
                };
                $scope.transferMessages.push({ type: 'info', msg: 'Click a target to transfer from ' + selectedTank.name + ' to...' });
            } else { // Else the user is selecting the 'To' of the transfer.
                $scope.transfer.to = selectedTank.id;
                $scope.redirectToTransfer();
            }
        }
    });

    $scope.$on('externalSelected', function (event) {
        // If the user is starting a transfer.
        if ($scope.transfer === null) {
            $scope.transfer = {
                from: 'external',
                to: null
            };
            $scope.transferMessages.push({ type: 'info', msg: 'Click a target to fill from an external source...' });
        } else { // Else the user is selecting the 'To' of the transfer.
            $scope.transfer.to = 'external';
            $scope.redirectToTransfer();
        }
        $scope.$apply();
    });

    $scope.endTransfer = function () {
        $scope.transfer = null;
        $scope.transferMessages.length = 0;
    }

}]);