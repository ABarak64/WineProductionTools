'use strict';

app.directive('tankCanvas', function () {
    return {
        restrict: 'E',
        scope: {
            tanks: '='
        },
        templateUrl: 'Scripts/app/views/partials/tankCanvas.html',
        controller: ['$scope', function ($scope) {

            $scope.click = function () {
                console.log('you clicked it');
            };

            $scope.$watch('tanks', function (newVal, oldVal) {
                console.log('tanks changed: ' + newVal[0].name);
            });
        }]
    };
});