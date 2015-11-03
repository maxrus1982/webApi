'use strict';

angular.module('wApp', []).controller('SortByID', ['$scope', 'Task',
  function($scope, Task) {
      Task.getList({ sort: [{ field: 'ID', dir: 'asc' }] }).then(function (data) {
       $scope.Tasks = data.data.Data;
       console.info('"GetList" result: ', data)
     });
    }
  ]);