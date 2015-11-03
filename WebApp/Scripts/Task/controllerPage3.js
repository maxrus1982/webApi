'use strict';

angular.module('wApp', []).controller('wAppCtrlPage1', ['$scope', 'Task',
  function($scope, Task) {
      Task.getList({ Page: 3, Take: 10, Skip: 20 }).then(function (data) {
          $scope.Tasks = data.data.Data;
          $scope.TotalRows = data.data.Total;
          $scope.Page = 3;
          $scope.Take = 10;
          $scope.Skip = 20;
       console.info('"GetList" result: ', data)
     });
    }
  ]);