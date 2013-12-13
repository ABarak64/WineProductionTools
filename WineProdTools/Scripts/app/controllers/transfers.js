'use strict';

app.controller('TransfersCtrl', ['$scope', '$location', 'Tanks', function ($scope, $location, Tanks) {

    $scope.transfer = null;
    $scope.transferMessages = [];
    $scope.hideHints = true;
    $scope.$parent.loading = true;

    Tanks.getTanks().success(function (data) {
        $scope.tanks = data;
        $scope.$parent.loading = false;
    }).error(function () {
        $scope.$parent.loading = false;
    });

    $scope.redirectToTransfer = function () {
        if ($scope.transfer.from === 'external' && $scope.transfer.to === 'external') {
            $scope.endTransfer();
            $scope.transferMessages.push({ type: 'danger', msg: 'You can\'t transfer from external to external.' });
        } else if ($scope.transfer.from === $scope.transfer.to) {
            $scope.endTransfer();
            $scope.transferMessages.push({ type: 'danger', msg: 'You can\'t transfer between the same tank.' });
        } else if ($scope.transfer.from === 'external') {
            $location.path('/filltank/' + $scope.transfer.to);
        } else if ($scope.transfer.to === 'external') {
            $location.path('/emptytank/' + $scope.transfer.from);
        } else {
            $location.path('/tanktransfer/from/' + $scope.transfer.from + '/to/' + $scope.transfer.to);
        }
    };

    $scope.$on('tankSelected', function (event, tank) {
        // If the user is starting a transfer.
        if ($scope.transfer === null) {
            if (tank.contents.gallons === null) {
                $scope.transferMessages.length = 0;
                $scope.transferMessages.push({ type: 'danger', msg: 'You can\'t transfer from an empty tank.' });
            } else {
                $scope.transfer = {
                    from: tank.id,
                    to: null
                };
                $scope.transferMessages.length = 0;
                $scope.transferMessages.push({ type: 'info', msg: 'Click a target to transfer from ' + tank.name + ' to...' });
            }
        } else { // Else the user is selecting the 'To' of the transfer.
            $scope.transfer.to = tank.id;
            $scope.redirectToTransfer();
        }
        $scope.$apply();
    });

    $scope.externalSelected = function () {
        // If the user is starting a transfer.
        if ($scope.transfer === null) {
            $scope.transfer = {
                from: 'external',
                to: null
            };
            $scope.transferMessages.length = 0;
            $scope.transferMessages.push({ type: 'info', msg: 'Click a target to fill from an external source...' });
        } else { // Else the user is selecting the 'To' of the transfer.
            $scope.transfer.to = 'external';
            $scope.redirectToTransfer();
        }
    }

    $scope.endTransfer = function () {
        $scope.transfer = null;
        $scope.transferMessages.length = 0;
    }

}]);