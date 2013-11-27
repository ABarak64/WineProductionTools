'use strict';

app.controller('DashboardCtrl', ['$scope', '$routeParams', 'Accounts', 'Notes', function ($scope, $routeParams, Accounts, Notes) {

    $scope.account = {
        name: 'Loading Winery...'
    };

    $scope.loading = false;
    $scope.notes = [];
    $scope.noMoreNotes = false;

    Accounts.getAccount().success(function (data) {
        $scope.account = data;
    });

    $scope.loadMoreNotes = function () {
        if ($scope.loading) return;
        $scope.loading = true;
        Notes.getSomeNotesAfterThisMany($scope.notes.length).success(function (data) {
            if (data.length === 0) {
                $scope.noMoreNotes = true;
            } else {
                angular.forEach(data, function (note) {
                    $scope.notes.push(note);
                });
            }
            $scope.loading = false;
        });
    };
}]);