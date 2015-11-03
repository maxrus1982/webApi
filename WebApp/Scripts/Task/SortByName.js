'use strict';

angular.module('wApp', []).controller('SortByName', ['$scope', 'Task',
  function($scope, Task) {
      Task.getList({ sort: [{ field: 'Name', dir: 'asc' }] }).then(function (data) {
       $scope.Tasks = data.data.Data;
       console.info('"GetList" result: ', data)
     });
    }
  ]);