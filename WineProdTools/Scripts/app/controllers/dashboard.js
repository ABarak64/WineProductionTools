'use strict';

app.controller('DashboardCtrl', ['$scope', '$routeParams', 'Accounts', 'Notes', function ($scope, $routeParams, Accounts, Notes) {

    $scope.loadingNotes = false;
    $scope.$parent.loading = true;
    $scope.notes = [];
    $scope.noMoreNotes = false;
    $scope.account = {
        name: 'Loading Winery...'
    };

    Accounts.getAccount().success(function (data) {
        $scope.account = data;
        $scope.$parent.loading = false;
    }).error(function () {
        $scope.$parent.loading = false;
    });

    $scope.loadMoreNotes = function () {
        if ($scope.loadingNotes) return;
        $scope.loadingNotes = true;
        Notes.getSomeNotesAfterThisMany($scope.notes.length).success(function (data) {
            if (data.length === 0) {
                $scope.noMoreNotes = true;
            } else {
                angular.forEach(data, function (note) {
                    $scope.notes.push(note);
                });
            }
            $scope.loadingNotes = false;
        });
    };
}]);