function AuthInterceptor($localStorage, $q) {

    return {
        request: function (config) {
            config.headers = config.headers || {};

            if ($localStorage.token) {
                config.headers.Authorization = 'Bearer ' + $localStorage.token;
            }
            

            return config;
        },

        responseError: function (response) {
            if (response.status === 401 || response.status === 403) {
                console.log(response.status);
            }

            return $q.reject(response);
        }
    }

}

AuthInterceptor.$inject = ['$localStorage', '$q'];
module.exports = AuthInterceptor;