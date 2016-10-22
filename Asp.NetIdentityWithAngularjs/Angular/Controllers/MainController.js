function MainController(AuthService) {


    var vm = this;

    vm.loginForm = {};
    vm.loginForm.grant_type = "password";
    vm.loginForm.username = "";
    vm.loginForm.password = "";

    vm.token = AuthService.getToken();

    vm.login = function () {
        console.log(vm.loginForm);
        AuthService.signin(vm.loginForm).then(function (response) {
            console.log(response.data)
            console.log(response.data['access_token'])
            AuthService.setToken(response.data['access_token']);
            vm.token = AuthService.getToken();
        }, function (response) {
            console.log(response);
            vm.token = AuthService.getToken();
        });
    };

    vm.testValues;

    vm.test = function () {
        AuthService.test().then(function (response) {
            console.log(response)
            vm.testValues = response.data;
        }, function (response) {
            console.log(response);
        });
    };

}

MainController.$inject = ['AuthService'];
module.exports = MainController;