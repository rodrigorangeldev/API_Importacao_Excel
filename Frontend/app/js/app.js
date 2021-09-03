var app = angular.module('app', ['ngRoute']);

app.config(function($routeProvider, $locationProvider){
    
    $locationProvider.html5Mode({
        enabled: true,
        requireBase: false
    });

    $routeProvider
    .when('/',{
        templateUrl: 'app/views/home.html',
        controller:  'HomeCtrl'
    })
    .when('/import/:id',{
        templateUrl: '../app/views/import.html',
        controller:  'ImportCtrl'
    })
    .otherwise({ redirectTo: '/'});
});

