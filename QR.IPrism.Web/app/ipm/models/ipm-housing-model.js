/*********************************************************************
* File Name     : ipm-housing-model.js
* Description   : Contains all the model required for housing module.
* Create Date   : 25th Jan 2016
* Modified Date : 25th Jan 2016
* Copyright by  : Qatar Airways
*********************************************************************/
'use strict'
angular.module('app.ipm.module')
        .factory('housing', [ function () {

            var self = this;

            function Housing(housingData) {

                self.RequestId = housingData.RequestId ? housingData.RequestId : "";
                self.Status = housingData.Status ? housingData.Status : "";
                self.RequestNumber = housingData.RequestNumber ? housingData.RequestNumber : "";
                self.RequestType = housingData.RequestNumber ? housingData.RequestNumber : "";
                self.Descriptions = housingData.Descriptions ? housingData.Descriptions : "";
                self.CloseDate = housingData.CloseDate ? housingData.CloseDate : "";
            };

            return Housing;
        }]);