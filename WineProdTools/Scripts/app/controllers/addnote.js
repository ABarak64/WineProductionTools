'use strict';

app.controller('AddNoteCtrl', ['$scope', '$location', 'Notes', function ($scope, $location, Notes) {

    $scope.note = {
        comment: ''
    };

    $scope.errors = null;

    $scope.add = function () {
        Notes.addNote($scope.note).success(function (data) {
            $location.path('/dashboard');
        }).error(function (data) {
            $scope.errors = data;
        });
    };

}]);