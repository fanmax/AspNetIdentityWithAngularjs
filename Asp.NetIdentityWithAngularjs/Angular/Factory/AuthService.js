function AuthService($http,$httpParamSerializerJQLike, $localStorage, $q) {

    return {
        getToken: function () {
            return $localStorage.token;
        },
        setToken: function (token) {
            $localStorage.token = token;
        },
        signin: function (param) {
            $localStorage.token = null;
            var result = $q.defer();
            $http({
                url: '/Authenticate',
                method: "POST",
                data: $httpParamSerializerJQLike(param),
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8'
                }
            }).then(function (response, status) {
                result.resolve(response);
            }, function (response) {
                result.reject(response);
            }).finally(function () {
                
            });
            return result.promise;
        },
        test: function (param) {
            var result = $q.defer();
            $http({
                url: '/api/test',
                method: "GET",
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8',
                }
            }).then(function (response, status) {
                result.resolve(response);
            }, function (response) {
                result.reject(response);
            }).finally(function () {

            });
            return result.promise;
        },
        logout: function (data) {
            delete $localStorage.token;
            $q.when();
        }
    };

}

AuthService.$inject = ['$http', '$httpParamSerializerJQLike', '$localStorage', '$q'];
module.exports = AuthService;