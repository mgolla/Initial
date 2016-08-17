
'use strict'
angular
    .module('app.ipm.module')
    .controller('ipm.crewPersonalDetails.controller', ['$scope', '$http', 'messages', 'crewProfileService', '$state', '$rootScope', 'blockUI',
                                  function ($scope, $http, messages, crewProfileService, $state, $rootScope, blockUI) {

                                      //Controller Scope Initialization
                                      var iCrewPersDetsblockUI = blockUI.instances.get('ipmCrewPersDetsBlockUI');

                                      var Initialize = function () {
                                          iCrewPersDetsblockUI.start();
                                          //Declare all scope properties
                                          var pathArray = window.location.href.split('/');
                                          var protocol = pathArray[0];
                                          var host = pathArray[2];
                                          var baseURL = protocol + '//' + host + '/';

                                          var data = {
                                              CrewImagePath: baseURL + appSettings.CrewPhotos,
                                              CrewImageType: messages.CREWIMAGETYPE
                                          };

                                          crewProfileService.getCrewPersonalDetails(data, function (result) {
                                              iCrewPersDetsblockUI.stop();
                                              $scope.CrewPhotoUrl = result.data.CrewPhotoUrl;
                                              $scope.StaffName = result.data.StaffName;
                                              $scope.StaffID = result.data.StaffNumber;
                                              $scope.RPNumber = result.data.RPNumber;
                                              $scope.Nationality = result.data.Nationality;
                                              $scope.RPExpiryDate = result.data.RPExpiryDate;
                                              $scope.Grade = result.data.Grade;
                                              $scope.ReportingTo = result.data.ReportingTo;
                                              $scope.Email = result.data.Email;
                                              $scope.DOB = result.data.DateOfBirth;
                                              $scope.Contact = result.data.Contact;
                                              $scope.NextToKin = result.data.NextToKIN;
                                              $scope.PermanentAddress = result.data.PermanentAddress;
                                              $scope.CurrentAccomodation = result.data.CurrentAccomodation;
                                              $scope.ExperienceInMonths = result.data.ExpInCurrentGradeInMonths;
                                              $scope.FFInGrade = result.data.FlightFlownInCurrentGrade;
                                              $scope.TotalQRExp = result.data.ExpInMonths;

                                              $scope.JoiningDate = result.data.JoiningDate;

                                          },
                                         function (error) {
                                             iCrewPersDetsblockUI.stop();
                                         });
                                      }

                                      Initialize();

                                  }]);