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
                angular.forEach($scope.tanks, function (tank) {
                    console.log(tank);
                });
            };

            $scope.$watch('tanks.length', function (newVal, oldVal) {
                console.log('tanks length has changed');
            });

            // The controller injects all the tank data initially.
            $scope.$watch('tanks', function (newVal, oldVal) {
                console.log('tanks have changed for directive');
                angular.forEach(newVal, function (tank) {
                    console.log(tank);
                    $scope.drawTank(tank.id, tank.name, tank.xPosition, tank.yPosition);
                });
            });

            $scope.stage = new Kinetic.Stage({
                container: 'container',
                width: window.innerWidth,
                height: window.innerHeight
            });
       
            $scope.drawTank = function (id, name, posx, posy) {
                var layer = new Kinetic.Layer({
                        draggable: true
                });

                var group = new Kinetic.Group({
                    x: posx,
                    y: posy
                });

                var circle = new Kinetic.Circle({
                    radius: 40,
                    fill: 'red',
                    stroke: 'black',
                    strokeWidth: 4,
                    id: id
                });

                var text = new Kinetic.Text({
                    text: name,
                    fontSize: 18,
                    fontFamily: 'FontAwesome',
                    fill: '#555',
                    width: 300
                });

                layer.on('mouseover', function () {
                    document.body.style.cursor = 'pointer';
                });
                layer.on('mouseout', function () {
                    document.body.style.cursor = 'default';
                });

                layer.on('dragend', function () {
                    console.log('finished dragging');
                    console.log(circle.getAbsolutePosition().x);
                    console.log(circle.attrs.id);
                    var tankMoved = $scope.tanks.filter(function (tank) {
                        return tank.id === circle.attrs.id;
                    })[0];
                    tankMoved.xPosition = circle.getAbsolutePosition().x;
                    tankMoved.yPosition = circle.getAbsolutePosition().y;
                    $scope.$apply();
                });

                group.add(circle);
                group.add(text);
                layer.add(group);
                $scope.stage.add(layer);
            };
        }]
    };
});