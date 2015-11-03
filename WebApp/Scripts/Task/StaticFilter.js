'use strict';

angular.module('wApp', []).controller('StaticFilter', ['$scope', 'Task',
  function($scope, Task) {
      Task.getList({ IgnoreCompletedTasks: true }).then(function (data) {
       $scope.Tasks = data.data.Data;
       console.info('"GetList" result: ', data)
     });
    }
  ]);