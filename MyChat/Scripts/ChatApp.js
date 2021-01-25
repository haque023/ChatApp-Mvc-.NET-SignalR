/// <reference path="angular.js" />
var app = angular.module("MyApp", []);
var baseUrl = window.location.origin;



app.controller("userCtrl", function ($scope, $http) {

    $scope.InsertUser = function () {
        $scope.tbl_users = {};
        $scope.tbl_users.email = $scope.email;
        $scope.tbl_users.firstName = $scope.firstName;
        $scope.tbl_users.lastName = $scope.lastName;
        $http({
            method: "post",
            url: baseUrl + "/User/insertNewUser",
            datatype: "json",
            data: JSON.stringify($scope.tbl_users)
        }).then(function (response) {
            alert(response.data);
            $scope.email = "";
            $scope.firstName = "";
            $scope.lastName = "";
        })
    }
});













app.controller("chatCtrl", function ($scope, $http) {

    var notifications = $.connection.chat;

    notifications.client.updateMessages = function () {
        $scope.GetAllData();
    };

    $.connection.hub.start().done(function () {
        console.log(notifications.connection.id)
        $scope.getAllUser();
    }).fail(function (e) {
    });



    $scope.sentMessage = function () {
       
        $scope.tbl_messages = {};
        $scope.tbl_messages.chatText = document.getElementById("chatText").value;       
        $scope.tbl_messages.receiver = $scope.receiver;
        $scope.tbl_messages.receiverName = $scope.receiverName;
        $http({
            method: "post",
            url: baseUrl + "/Home/SendMessage",
            datatype: "json",
            data: JSON.stringify($scope.tbl_messages)
        }).then(function (response) {
          
            document.getElementById("chatText").value = "";
            
        })
    }

   
    $scope.deleteSender = function (messages) {

        $http({
            method: "post",
            url: baseUrl + "/Home/DeleteSenderMessage",
            datatype: "json",
            data: JSON.stringify(messages)
        }).then(function (response) {

        })

    }


    $scope.deleteReceiver = function (messages) {

        $http({
            method: "post",
            url: baseUrl + "/Home/DeleteReceiverMessage",
            datatype: "json",
            data: JSON.stringify(messages)
        }).then(function (response) {

        })

    }

    $scope.whichDate = function(timestamp) {

        timestamp = timestamp.split("(")[1];
        timestamp = timestamp.split(")")[0];

        var x = new Date(parseInt(timestamp));
        x = x.getFullYear() + "-" + ((x.getMonth() + 1) > 9 ? (x.getMonth() + 1) : "0" + (x.getMonth() + 1)) + "-" + (x.getDate() > 9 ? x.getDate() : "0" + x.getDate());
        return x;
    }


    $scope.getAllUser = function () {
        $http({
            method: "get",
            url: baseUrl + "/User/getAllUser"
        }).then(function (response) {
            if (response.data == "not") location.href = baseUrl + "/user";
            $scope.myUsres = response.data;
        }, function () {
            //alert("Error Occur");
        }
        )
    }

    $scope.GetAllData = function () {
       
        $http({
            method: "post",
            url: baseUrl + "/Home/GetAllMessage",
            datatype: "text",
            data: {receiver: $scope.receiver }
        }).then(function (response) {
            if (response.data == "not") location.href = baseUrl+ "/user";
            $scope.messages = response.data;
        }, function () {
            //alert("Error Occur");
        }
        )
    }

    $scope.activeUser = '';
    $scope.setUserToChat = function (user) {

        $scope.receiver = user.userCode;
        $scope.receiverName = user.firstName;
        document.getElementById("activeUser").value = user.userCode;
        document.getElementById("Receiver_Name").value = user.firstName;
        $scope.activeUser = user.userCode;
        $scope.GetAllData();

    }
  
   

})