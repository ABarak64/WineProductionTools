'use strict';

app.directive('loading', ['ngProgressLite', function (ngProgressLite) {
    return {
        restrict: 'A',
        link: function (scope, element, attr) {
            scope.$watch('loading', function (val) {
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