require('angular');
require('angular-route');
require('angular-animate');
require('angular-messages');
require('angular-resource');
require('angular-ui-bootstrap');
require('ngstorage');
require('ng-table');
require('sweetalert');

//Require Controllers
var MainController = require('./Controllers/MainController');

//Require Services


//Require Factory
var AuthService = require('./Factory/AuthService');
var AuthInterceptor = require('./Factory/AuthInterceptor');

var app = angular.module('app', ['ngRoute', 'ngAnimate', 'ngMessages', 'ngResource', 'ui.bootstrap', 'ngTable', 'ngStorage']);

//Controllers
app.controller('MainController', MainController);

//Factory
app.factory('AuthService', AuthService);
app.factory('AuthInterceptor', AuthInterceptor);



app.config(['$routeProvider', '$locationProvider', '$httpProvider', function ($routeProvider, $locationProvider, $httpProvider) {

    $httpProvider.interceptors.push('AuthInterceptor');

    $routeProvider
        .when('/', {
            templateUrl: 'Angular/Pages/index.html',
            controller: 'MainController as vm'
        })
        .otherwise({
            redirectTo: '/'
        });

    

}]);
