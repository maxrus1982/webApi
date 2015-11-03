'use strict';
angular.module('wApp').factory('Task', ['$http',
  function($http){

      var baseURL = '/Api/Task/';

	return  {
		getList: function() {
		    return $http.post(baseURL + 'GetList');
		},

		get: function(id) {
			return $http.get(baseURL + 'Get?id=' + id);
		},

		remove: function(task) {
			return $http.post(baseURL + 'Remove', task);
		},

		edit: function(task) {
			return $http.post(baseURL + 'Post', task);
		},

		new: function(task) {
		    return $http.post(baseURL + 'Post', { Name: task });
		},

		//new: function(Name, callback) {
		//    $http.post(baseURL + 'New', {}).then(function(newTask) {
		//		console.info('"New" result: ', newTask);
		//		newTask.Name = Name;
		//		$http.post(baseURL + 'Post', newTask).then(function(data) {
		//			console.info('"Post after New" result: ', newTask);
		//			callback(data);
		//		}, function(error) {
		//			alert('Ошибка при сохранении новой записи');
		//			console.error(error);
		//		});
		//	}, function(error) {
		//		alert('Ошибка при создании новой записи');
		//		console.error(error);
		//	});
		//}
	}
  }]);
