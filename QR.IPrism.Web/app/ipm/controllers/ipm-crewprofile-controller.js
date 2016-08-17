'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm-crewprofile-controller', ['$scope', '$state',
                                  function ($scope, $state) {
                                      //Controller Scope Initialization

                                      var Initialize = function () {
                                          $state.go("cpPersonaldetails.details.dstvst");
                                      }

                                      Initialize();

                                  }]);