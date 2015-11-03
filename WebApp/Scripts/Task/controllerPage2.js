'use strict';

angular.module('wApp', []).controller('wAppCtrlPage2', ['$scope', 'Task',
  function($scope, Task) {
      Task.getList({ Page: 2, Take: 10, Skip:10 }).then(function (data) {
       $scope.Tasks = data.data.Data;
       console.info('"GetList" result: ', data)
     });
    }
  ]);