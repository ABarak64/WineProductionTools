'use strict';

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

            $scope.zoom = function ($event, $delta, $deltaX, $deltaY) {
                $event.preventDefault();
                $scope.stage.setScale($scope.stage.getScale().x + (0.10 * $delta));
                $scope.stage.draw();
            };

            $scope.stage = new Kinetic.Stage({
                container: 'container',
                width: window.innerWidth,
                height: window.innerHeight
            });
       
            $scope.drawTheTanks = function () {
                angular.forEach($scope.tanks, function (tank) {
                    $scope.drawTank(tank);
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
                    width: 80,
                    height: 50,
                    stroke: '#CCC',
                    strokeWidth: 4
                });

                var text = new Kinetic.Text({
                    text: 'External',
                    fontSize: 18,
                    fontFamily: 'FontAwesome',
                    fill: '#555',
                    width: 300,
                    x: 10,
                    y: 18
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

            $scope.drawTank = function (tank) {
                var layer = new Kinetic.Layer({
                        draggable: ($scope.toggleMoveTanks === 'true')
                });

                var group = new Kinetic.Group({
                    x: tank.xPosition,
                    y: tank.yPosition
                });

                var circle = new Kinetic.Circle({
                    radius: $scope.getTankRadius(tank),
                    stroke: $scope.contentStateToColorMap[tank.contents.state.id],
                    strokeWidth: 4,
                    id: tank.id
                });

                var contents = new Kinetic.Circle({
                    radius: $scope.getContentsRadius(tank),
                    fill: $scope.contentStateToColorMap[tank.contents.state.id]
                });

                var analysisText = 'Name:     ' + tank.contents.name + '\n'
                    + 'State:    ' + tank.contents.state.name + '\n'
                    + 'Gallons:  ' + tank.contents.gallons + '\n'
                    + 'pH:       ' + tank.contents.ph + '\n'
                    + 'SO2:      ' + tank.contents.so2 + '\n'
                    + '% Alc:    ' + tank.contents.alcohol + '%\n'
                    + 'TA:       ' + tank.contents.ta + '\n'
                    + 'VA:       ' + tank.contents.va + '\n'
                    + 'MA:       ' + tank.contents.ma + '\n'
                    + 'RS:       ' + tank.contents.rs;

                var analysis = new Kinetic.Text({
                    text: tank.contents.id !== null ? analysisText : 'Empty',
                    fontSize: 16,
                    fontFamily: 'Courier',
                    fill: '#555',
                    width: 250,
                    x: $scope.getTankRadius(tank) + 10,
                    y: -$scope.getTankRadius(tank),
                    visible: false
                });

                var name = new Kinetic.Text({
                    text: tank.name,
                    fontSize: 18,
                    fontFamily: 'FontAwesome',
                    fill: '#555',
                     width: circle.getWidth(),
                    align: 'center',
                });

                name.setOffset({
                    x: name.getWidth() / 2,
                    y: $scope.getTankRadius(tank) + name.getHeight() + 5
                });

                var contentsName = new Kinetic.Text({
                    text: tank.contents.id !== null ? tank.contents.name : '',
                    fontSize: 16,
                    fontFamily: 'FontAwesome',
                    fill: '#555',
                    width: circle.getWidth(),
                    align: 'center',
                });

                contentsName.setOffset({
                    x: name.getWidth() / 2
                });

                layer.on('mouseover', function () {
                    document.body.style.cursor = 'pointer';
                    layer.moveToTop();
                    analysis.show();
                    layer.draw();
                });
                layer.on('mouseout', function () {
                    document.body.style.cursor = 'default';
                    analysis.hide();
                    layer.draw();
                });

                layer.on('dragend', function () {
                    var moved = $scope.tanks.filter(function (tank) {
                        return tank.id === circle.attrs.id;
                    })[0];
                    // Otherwise we would get the x value without taking into account that the scale has changed.
                    var oldScale = $scope.stage.getScale().x;
                    $scope.stage.setScale(1);
                    moved.xPosition = parseInt(circle.getAbsolutePosition().x);
                    moved.yPosition = parseInt(circle.getAbsolutePosition().y);
                    $scope.stage.setScale(oldScale);
                    $scope.$emit('tankMoved', moved);
                });

                layer.on('click', function () {
                    var selected = $scope.tanks.filter(function (tank) {
                        return tank.id === circle.attrs.id;
                    })[0];
                    $scope.$emit('tankSelected', selected);
                });
                
                group.add(circle);
                group.add(contents);
                group.add(contentsName);
                group.add(name);
                group.add(analysis);
                layer.add(group);
                $scope.stage.add(layer);
            };

            $scope.getTankRadius = function (tank) {
                var minRadius = 40;
                var maxRadiusDifference = 80;
                var largestExpectedNumberOfGallons = 100000;

                return (((tank.gallons / largestExpectedNumberOfGallons) * maxRadiusDifference) + minRadius);
            };

            $scope.getContentsRadius = function (tank) {
                var tankRadius = $scope.getTankRadius(tank);
                var radius = ((tank.contents.gallons / tank.gallons) * tankRadius);
                if (tankRadius - radius < 4) {
                    radius -= 5;
                }
                return radius;
            };

            $scope.contentStateToColorMap = [
                '#CCC',
                '#CCC',
                'blue',
                'green',
                'orange',
                'black'
            ];
        }]
    };
});