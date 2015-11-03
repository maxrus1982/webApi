'use strict';

angular.module('wApp', []).controller('DynamicFilter', ['$scope', 'Task',
  function($scope, Task) {
      Task.getList({
          filter: {
              logic: 'and',
              filters: [
                  { field: 'Name', operator: 'contains', value: 'Foo_2' }
              ]
          }
      }).then(function (data) {
       $scope.Tasks = data.data.Data;
       console.info('"GetList" result: ', data)
     });
    }
  ]);