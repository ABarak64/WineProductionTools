﻿'use strict';

app.directive('tankCanvas', function () {
    return {
        restrict: 'E',
        scope: {
            tanks: '=',
            toggleMoveTanks: '@',
            showExternal: '@'
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
                if ($scope.showExternal === 'true') {
                    $scope.drawExternal();
                };
            };

            $scope.drawExternal = function () {
                var layer = new Kinetic.Layer();
                
                var group = new Kinetic.Group({
                    x: 50,
                    y: 50
                });

                var rect = new Kinetic.Rect({
                    width: 100,
                    height: 50,
                    fill: 'green',
                    stroke: 'black',
                    strokeWidth: 4
                });

                var text = new Kinetic.Text({
                    text: 'External',
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

                layer.on('click', function () {
                    $scope.$emit('externalSelected');
                });

                group.add(rect);
                group.add(text);
                layer.add(group);
                $scope.stage.add(layer);
            }

            $scope.drawTank = function (id, name, posx, posy) {
                var layer = new Kinetic.Layer({
                        draggable: ($scope.toggleMoveTanks === 'true')
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
                    var moved = $scope.tanks.filter(function (tank) {
                        return tank.id === circle.attrs.id;
                    })[0];
                    moved.xPosition = circle.getAbsolutePosition().x;
                    moved.yPosition = circle.getAbsolutePosition().y;
                    $scope.$emit('tankMoved', moved);
                });

                layer.on('click', function () {
                    var selected = $scope.tanks.filter(function (tank) {
                        return tank.id === circle.attrs.id;
                    })[0];
                    $scope.$emit('tankSelected', selected);
                });
                
                group.add(circle);
                group.add(text);
                layer.add(group);
                $scope.stage.add(layer);
            };
        }]
    };
});