'use strict';

angular.module('wApp', []).controller('wAppCtrlPage1', ['$scope', 'Task',
  function($scope, Task) {
      Task.getList({ Page: 3, Take: 10, Skip: 20 }).then(function (data) {
       $scope.Tasks = data.data.Data;
       console.info('"GetList" result: ', data)
     });
    }
  ]);