'use strict';

app.directive('loading', ['ngProgressLite', function (ngProgressLite) {
    return {
        restrict: 'A',
        scope: {
            isLoading: '='
        },
        link: function (scope, element, attr) {
            scope.$watch('isLoading', function (val) {
                if (val) {
                    ngProgressLite.start();
                }
                else {
                    ngProgressLite.done();
                }
            });
        }
    }
}]);