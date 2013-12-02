'use strict';

app.directive('alert', function () {
    return {
        restrict: 'EA',
        templateUrl: 'Scripts/app/views/partials/alert.html',
        transclude: true,
        replace: true,
        scope: {
            type: '=',
            close: '&'
        },
        link: function (scope, iElement, iAttrs) {
            scope.closeable = "close" in iAttrs;
        }
    };
});