'use strict';

angular.module('wApp', []).controller('GroupedList', ['$scope', 'Task',
  function($scope, Task) {
      Task.getGroupedList().then(function (data) {
          $scope.GroupedTasks = data.data.Data;
     });
    }
  ]);