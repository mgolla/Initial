
// ***********************************************************************
//Author           : Mack
//Created          : 01/17/2016 00:00:00
//Last Modified By : 
//Last Modified On : 01/17/2016 00:00:00
//***********************************************************************
//<summary> Directive of CrewInfo</summary>
//***********************************************************************
"use strict"
angular.module('app.ipm.module').directive('crewinfo', [function () {
    return {
        restrict: "E",
        templateUrl: "/app/ipm/partials/ipmCrewInfo.html" ,
          controller: 'ipm.crewInfo.controller'
    }
}]);


