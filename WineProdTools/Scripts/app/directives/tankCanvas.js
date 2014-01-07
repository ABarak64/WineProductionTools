'use strict';

app.directive('tankCanvas', function () {
    return {
        restrict: 'E',
        scope: {
            tanks: '=',
            toggleMoveTanks: '@'
        },
        templateUrl: 'Scripts/app/views/partials/tankCanvas.html',
        controller: ['$scope', '$attrs', '$element', function ($scope, $attrs, $element) {

            // The controller injects all the tank data initially.
            $scope.$watch('tanks', function (newVal, oldVal) {
                $scope.drawTheTanks();
            });

            $scope.zoom = function ($event, $delta, $deltaX, $deltaY) {
                $event.preventDefault();
                $delta = $delta > 0 ? 1 : -1;
                var currentScale = $scope.stage.getScale().x;
                if (currentScale < 1.6 && $delta > 0 || currentScale > 0.4 && $delta < 0) {
                    $scope.stage.setScale(currentScale + (0.10 * $delta));
                    $scope.stage.draw();
                }
            };

            $scope.stage = new Kinetic.Stage({
                container: 'container',
                width: $element.parent().width(),
                height: $element.parent().height() <= 10 ? 600 : $element.parent().height(),
                draggable: true
            });
       
            $scope.drawTheTanks = function () {
                $scope.layer = new Kinetic.Layer();
                $scope.stage.add($scope.layer);
                
                var imageObj = new Image();
                imageObj.src = '/Images/grid.png';
                imageObj.onload = function () {
                    var rect = new Kinetic.Rect({
                        x: 0,
                        y: 0,
                        width: 2883,
                        height: 1803,
                        fillPatternImage: imageObj,
                        fillPatternRepeat: 'repeat'
                    });
                    $scope.layer.add(rect);

                    angular.forEach($scope.tanks, function (tank) {
                        $scope.drawTank(tank);
                    });
                };
            };

            $scope.drawTank = function (tank) {

                var group = new Kinetic.Group({
                    x: tank.xPosition,
                    y: tank.yPosition,
                    draggable: ($scope.toggleMoveTanks === 'true')
                });

                var circle = new Kinetic.Circle({
                    radius: $scope.getTankRadius(tank),
                    stroke: $scope.contentStateToColorMap[tank.contents.state.id].tank,
                    strokeWidth: 4,
                    id: tank.id
                });

                var contents = new Kinetic.Shape({
                    drawFunc: function (context) {
                        context.beginPath();
                        context.arc(0, 0, $scope.getTankRadius(tank) - 5, 0, $scope.getContentsArc(tank));
                        context.lineTo(0, 0);
                        context.closePath();
                        context.fillStrokeShape(this);
                    },
                    fill: $scope.contentStateToColorMap[tank.contents.state.id].contents
                });

                var analysisText = 'Name:     ' + tank.contents.name + '\n'
                    + 'State:    ' + tank.contents.state.name + '\n'
                    + 'Gallons:  ' + tank.contents.gallons + '/' + tank.gallons + '\n'
                    + 'pH:       ' + tank.contents.ph + '\n'
                    + 'SO2:      ' + tank.contents.so2 + '\n'
                    + '% Alc:    ' + tank.contents.alcohol + '%\n'
                    + 'TA:       ' + tank.contents.ta + '\n'
                    + 'VA:       ' + tank.contents.va + '\n'
                    + 'MA:       ' + tank.contents.ma + '\n'
                    + 'RS:       ' + tank.contents.rs;

                var analysis = new Kinetic.Text({
                    text: tank.contents.id !== null ? analysisText : 'Gallons:  0/' + tank.gallons,
                    fontSize: 16,
                    fontFamily: 'Courier',
                    fill: '#555',
                    width: 250,
                    x: $scope.getTankRadius(tank) + 20,
                    y: -$scope.getTankRadius(tank)
                });

                var analysisBackground = new Kinetic.Rect({
                    x: $scope.getTankRadius(tank) + 10,
                    y: -$scope.getTankRadius(tank) - 10,
                    stroke: '#555',
                    strokeWidth: 1,
                    fill: '#eee',
                    width: 260,
                    height: analysis.getHeight() + 20,
                    shadowColor: 'black',
                    shadowBlur: 10,
                    shadowOffset: [4, 4],
                    shadowOpacity: 0.2,
                    opacity: 0.7
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
                    fill: '#333',
                    width: circle.getWidth(),
                    align: 'center',
                });

                contentsName.setOffset({
                    x: name.getWidth() / 2
                });

                // This guy allows us a floating shape above all the others to hook mouse events
                //    to, otherwise mouseon mouseout gets triggered on every item inside the circle.
                var invisibleCircle = new Kinetic.Circle({
                    radius: $scope.getTankRadius(tank) + 5,
                    strokeWidth: 4,
                    opacity: 0,
                    fill: 'black'
                });

                var tankGroup = new Kinetic.Group();
                var analysisGroup = new Kinetic.Group({
                    visible: false
                });

                tankGroup.add(circle);
                tankGroup.add(contents);
                tankGroup.add(contentsName);
                tankGroup.add(name);
                tankGroup.add(invisibleCircle);
                analysisGroup.add(analysisBackground);
                analysisGroup.add(analysis);
                group.add(tankGroup);
                group.add(analysisGroup);
                $scope.layer.add(group);

                var tween = new Kinetic.Tween({
                    node: tankGroup,
                    duration: 0.2,
                    scaleX: 1.15,
                    scaleY: 1.15
                });

                invisibleCircle.on('mouseover', function () {
                    document.body.style.cursor = 'pointer';
                    analysisGroup.show();
                    group.moveToTop();
                    $scope.layer.draw();
                    tween.play();
                });
                invisibleCircle.on('mouseout', function () {
                    document.body.style.cursor = 'default';
                    analysisGroup.hide();
                    $scope.layer.draw();
                    tween.reverse();
                });

                group.on('dragend', function () {
                    var moved = $scope.tanks.filter(function (tank) {
                        return tank.id === circle.attrs.id;
                    })[0];
                    // Otherwise we would get the x value without taking into account that the scale has changed.
                    var oldScale = $scope.stage.getScale().x;
                    $scope.stage.setScale(1);
                    // Account for offset after user has panned somewhere by dragging the stage.
                    var stagePosition = $scope.stage.getPosition();
                    moved.xPosition = parseInt(circle.getAbsolutePosition().x) - stagePosition.x;
                    moved.yPosition = parseInt(circle.getAbsolutePosition().y) - stagePosition.y;
                    $scope.stage.setScale(oldScale);
                    $scope.$emit('tankMoved', moved);

                });

                group.on('click', function () {
                    var selected = $scope.tanks.filter(function (tank) {
                        return tank.id === circle.attrs.id;
                    })[0];
                    $scope.$emit('tankSelected', selected);
                });

                group.on('dblclick', function () {
                    var selected = $scope.tanks.filter(function (tank) {
                        return tank.id === circle.attrs.id;
                    })[0];
                    $scope.$emit('tankExtraSelected', selected);
                });
            };

            $scope.getTankRadius = function (tank) {
                var minRadius = 40;
                var maxRadiusDifference = 80;
                var largestExpectedNumberOfGallons = 100000;

                return (((tank.gallons / largestExpectedNumberOfGallons) * maxRadiusDifference) + minRadius);
            };

            $scope.getContentsArc = function (tank) {
                var arc = ((tank.contents.gallons / tank.gallons) * Math.PI * 2);
                return arc;
            };

            $scope.contentStateToColorMap = [
                { tank: '#999', contents: '#CCC' },
                { tank: '#999', contents: '#CCC' },
                { tank: '#2f80c1', contents: '#7fb5e0' },
                { tank: '#30a638', contents: '#81df88' },
                { tank: 'orange', contents: '#ffc68a' },
                { tank: '#222', contents: '#696969' }
            ];
        }]
    };
});