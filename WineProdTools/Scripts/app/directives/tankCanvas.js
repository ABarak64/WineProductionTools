'use strict';

app.directive('tankCanvas', function () {
    return {
        restrict: 'E',
        scope: {
            tanks: '=',
            movedTank: '=',
            selectedTank: '=',
            toggleMoveTanks: '&'
        },
        templateUrl: 'Scripts/app/views/partials/tankCanvas.html',
        controller: ['$scope', '$attrs', function ($scope, $attrs) {

            // The controller injects all the tank data initially.
            $scope.$watch('tanks', function (newVal, oldVal) {
                $scope.drawTheTanks();
            });

            $scope.stage = new Kinetic.Stage({
                container: 'container',
                width: window.innerWidth,
                height: window.innerHeight
            });
       
            $scope.drawTheTanks = function () {
                angular.forEach($scope.tanks, function (tank) {
                    $scope.drawTank(tank.id, tank.name, tank.xPosition, tank.yPosition);
                });
            };

            $scope.drawTank = function (id, name, posx, posy) {
                var layer = new Kinetic.Layer({
                        draggable: $scope.toggleMoveTanks
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

                if (angular.isDefined($attrs.movedTank)) {
                    layer.on('dragend', function () {
                        var moved = $scope.tanks.filter(function (tank) {
                            return tank.id === circle.attrs.id;
                        })[0];
                        moved.xPosition = circle.getAbsolutePosition().x;
                        moved.yPosition = circle.getAbsolutePosition().y;
                        $scope.movedTank = moved;
                        $scope.$apply();
                    });
                }

                if (angular.isDefined($attrs.selectedTank)) {
                    layer.on('click', function () {
                        var selected = $scope.tanks.filter(function (tank) {
                            return tank.id === circle.attrs.id;
                        })[0];
                        $scope.selectedTank = selected;
                        $scope.$apply();
                    });
                }
                
                group.add(circle);
                group.add(text);
                layer.add(group);
                $scope.stage.add(layer);
            };
        }]
    };
});