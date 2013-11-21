'use strict';

app.controller('TanksCtrl', ['$scope', function ($scope) {
    $scope.tanks = [{ id: 1, name: 'bleh' }, { id: 2, name: 'mah' }];
}]);