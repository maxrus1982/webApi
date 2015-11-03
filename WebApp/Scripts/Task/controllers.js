'use strict';

angular.module('wApp', []).controller('wAppCtrl', ['$scope', 'Task',
  function($scope, Task) {
     Task.getList().then(function(data)  {
       $scope.Tasks = data.data.Data;
       console.info('"GetList" result: ', data)
     });
    
    $scope.taskChanged = function(item) {
      if (item.haveChanges) {
        Task.edit({ID: item.ID, Name: item.Name, Completed: item.Completed, PlanEndDate: item.PlanEndDate})
          .then(
            function(data) {
              console.info('"Post" result: ', newTask);
              delete item.haveChanges;
            },
            function() { alert('Ошибка при сохранении изменений'); }
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
            function() { alert('Ошибка при удалении'); }
          );
    }

    $scope.addTask = function(Name, PlanEndDate) {
        Task.new(Name, PlanEndDate, function (newTask) {
          $scope.Tasks.push(newTask);
          $scope.$apply();
      });
    }
  }]);