'use strict';

app.controller('AddNoteCtrl', ['$scope', '$location', 'Notes', function ($scope, $location, Notes) {

    $scope.note = {
        comment: ''
    };

    $scope.errors = null;

    $scope.add = function () {
        $scope.errors = { modelState: [] };
        $scope.$parent.waiting = true;
        Notes.addNote($scope.note).success(function (data) {
            $scope.$parent.waiting = false;
            $location.path('/dashboard');
        }).error(function (data) {
            $scope.$parent.waiting = false;
            $scope.errors = data;
        });
    };

}]);