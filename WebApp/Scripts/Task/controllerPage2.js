'use strict';

angular.module('wApp', []).controller('wAppCtrlPage2', ['$scope', 'Task',
  function($scope, Task) {
      Task.getList({ Page: 2, Take: 10, Skip:10 }).then(function (data) {
          $scope.Tasks = data.data.Data;
          $scope.TotalRows = data.data.Total;
          $scope.Page = 2;
          $scope.Take = 10;
          $scope.Skip = 10;
       console.info('"GetList" result: ', data)
     });
    }
  ]);