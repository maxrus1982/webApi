'use strict';

angular.module('wApp', []).controller('Typehead', ['$scope', 'Task',
  function($scope, Task) {
      Task.getList({ SearchData: { Search: 'Foo_1', Fields: [ 'Name' ] }}).then(function (data) {
       $scope.Tasks = data.data.Data;
       console.info('"GetList" result: ', data)
     });
    }
  ]);