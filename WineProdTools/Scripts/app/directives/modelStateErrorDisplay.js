'use strict';

app.directive('modelStateErrorDisplay', function () {
    return {
        restrict: 'E',
        scope: {
            errors: '='
        },
        templateUrl: 'Scripts/app/views/partials/modelStateErrorDisplay.html',
        controller: ['$scope', function ($scope) {
            $scope.errorMsgs = [];
            // The controller injects all the tank data initially.
            $scope.$watch('errors', function (errorObj, oldVal) {
                if (errorObj !== null) {
                    $scope.errorMsgs.length = 0;
                    if (typeof errorObj.modelState !== 'undefined') {
                        for (var error in errorObj.modelState) {
                            $scope.errorMsgs.push(errorObj.modelState[error][0]);
                        }
                    } else {
                        $scope.errorMsgs.push('An unknown error occurred');
                    }
                }
            });
        }]
    };
});