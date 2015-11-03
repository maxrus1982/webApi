'use strict';

angular.module('wApp', []).controller('wAppCtrl', ['$scope', 'Task',
  function($scope, Task) {
//$scope.Tasks = [{
//  ID: "1231515",
//  Name: "Name 1"
//  },
//  {
//  ID: "2424141",
//  Name: "Name 2"
//  }
//  ];
     Task.getList().then(function(data)  {
       $scope.Tasks = data.data.Data;
       console.info('"GetList" result: ', data)
       //alert('Ошибка при получении списка');
     });


    
    $scope.taskChanged = function(item) {
      if (item.haveChanges) {
        Task.edit({ID: item.ID, Name: item.Name})
          .then(
            function(data) {
              console.info('"Post" result: ', newTask);
              delete item.haveChanges;
            },

            function() {
              alert('Ошибка при сохранении изменений');
            }

          );

      }
    }

    $scope.removeTask = function(item) {
      var index = $scope.Tasks.indexOf(item);
      Task.remove(item)
          .then(
            function(data) {
              console.info('"Remove" result: ', data);
              $scope.Tasks.splice(index, 1);
            },

            function() {
              alert('Ошибка при удалении');
            }

          );
    }

    $scope.addTask = function(Name) {
      Task.new(Name, function(newTask) {
          $scope.Tasks.push(newTask);
          $scope.$apply();
      });
    }



  }]);