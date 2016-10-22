app.controller('MainController', function ($scope, $http, fileUploadService, fileDeleteService) {
    $scope.$emit('LOAD');
    $http.get('http://myfirstwebapidemo.azurewebsites.net/api/images/').success(function (data, status, headers, config) {
        $scope.images = data;
        $scope.title = data.title;
        $scope.answered = false;
        $scope.working = false;
        $scope.$emit('UNLOAD');
    }).error(function (data, status, headers, config) {
        $scope.title = "Oops... something went wrong";
        $scope.working = false;
    });

    $scope.plusOne = function (index) {
        $scope.images[index].likes += 1;
        $http.put('http://myfirstwebapidemo.azurewebsites.net/api/images/' + $scope.images[index].imageID, { imageID: $scope.images[index].imageID, createdDate: $scope.images[index].createdDate, imageName: $scope.images[index].imageName, cover: $scope.images[index].cover, likes: $scope.images[index].likes, dislikes: $scope.images[index].dislikes }).success(function (data) {
            //$scope.images = data;
            //$scope.title = data.title;
            //$scope.answered = false;
            //$scope.working = false;
        });
    };
    $scope.minusOne = function (index) {
        $scope.images[index].dislikes += 1;
        $http.put('http://myfirstwebapidemo.azurewebsites.net/api/images/' + $scope.images[index].imageID, { imageID: $scope.images[index].imageID, createdDate: $scope.images[index].createdDate, imageName: $scope.images[index].imageName, cover: $scope.images[index].cover, likes: $scope.images[index].likes, dislikes: $scope.images[index].dislikes }).success(function (data) {
            //$scope.images = data;
            //$scope.title = data.title;
            //$scope.answered = false;
            //$scope.working = false;
        });
    };
    $scope.Message = "";
    $scope.FileInvalidMessage = "";
    $scope.SelectedFileForUpload = null;
    $scope.FileDescription = "";
    $scope.IsFormSubmitted = false;
    $scope.IsFileValid = false;
    $scope.IsFormValid = false;

    //Form Validation
    $scope.$watch("f1.$valid", function (isValid) {
        $scope.IsFormValid = isValid;
    });


    // THIS IS REQUIRED AS File Control is not supported 2 way binding features of Angular
    // ------------------------------------------------------------------------------------
    //File Validation
    $scope.ChechFileValid = function (file) {
        var isValid = false;
        if ($scope.SelectedFileForUpload != null) {
            if ((file.type == 'image/png' || file.type == 'image/jpeg' || file.type == 'image/gif') && file.size <= (512 * 1024)) {
                $scope.FileInvalidMessage = "";
                isValid = true;
            }
            else {
                $scope.FileInvalidMessage = "Selected file is Invalid. (only file type png, jpeg and gif and 512 kb size allowed)";
            }
        }
        else {
            $scope.FileInvalidMessage = "Image required!";
        }
        $scope.IsFileValid = isValid;
    };

    //File Select event 
    $scope.selectFileforUpload = function (file) {
        $scope.SelectedFileForUpload = file[0];
    }
    //----------------------------------------------------------------------------------------

    //Save File
    $scope.SaveFile = function () {
        $scope.$emit('LOAD');
        $scope.IsFormSubmitted = true;
        $scope.Message = "";
        $scope.ChechFileValid($scope.SelectedFileForUpload);
        if ($scope.IsFormValid && $scope.IsFileValid) {
            fileUploadService.UploadFile($scope.SelectedFileForUpload, $scope.FileDescription).then(function (d) {
                if (d.Flag) {
                    $http.post('http://myfirstwebapidemo.azurewebsites.net/api/images/', { imageID: 0, createdDate: new Date(), imageName: d.FileName, cover: d.FilePath, likes: 0, dislikes: 0 }).success(function (data, status, headers, config) {
                        if (data != null) {
                            $scope.images = data;
                            $scope.title = data.title;
                            $scope.answered = false;
                            $scope.working = false;
                            $scope.$emit('UNLOAD');
                            ClearForm();
                            alert("File uploaded successfully!");
                        }
                    });
                } else {
                    if (d.FileAlreadyPresent) {
                        $scope.$emit('UNLOAD');
                        alert("File already present. Please select another file.");
                    } else {
                        $scope.$emit('UNLOAD');
                        alert("Error while uploading file!");
                    }
                    //
                }
            }, function (e) {
                alert(e);
            });
        }
        else {
            $scope.Message = "All the fields are required.";
        }
    };

    $scope.delete = function (index, imageName) {
        $scope.$emit('LOAD');
        $scope.IsFormSubmitted = true;
        $scope.Message = "";
        if (confirm("Are you sure you want to delete this image?")) {
            fileDeleteService.DeleteFile(imageName).then(function (d) {
                if (d.Flag) {
                    $http.delete('http://myfirstwebapidemo.azurewebsites.net/api/images/' + $scope.images[index].imageID).success(function (data) {
                        $scope.images = data;
                        $scope.title = data.title;
                        $scope.answered = false;
                        $scope.working = false;
                        $scope.$emit('UNLOAD');
                        alert("File deleted successfully!");
                        
                    }).error(function () {
                        $scope.$emit('UNLOAD');
                        alert("Error while deleting file from database!");
                    });
                } else {
                    $scope.$emit('UNLOAD');
                    alert("Error while deleting file from server!");
                }
            }, function (e) {
                alert(e);
            });
        }
    };
    function ClearForm() {
        $scope.FileDescription = "";
        //as 2 way binding not support for File input Type so we have to clear in this way
        //you can select based on your requirement
        angular.forEach(angular.element("input[type='file']"), function (inputElem) {
            angular.element(inputElem).val(null);
        });

        $scope.f1.$setPristine();
        $scope.IsFormSubmitted = false;
    }
}).controller('appController', function ($scope) {
    $scope.$on('LOAD', function () { $scope.loading = true });
    $scope.$on('UNLOAD', function () { $scope.loading = false });
}).factory('fileUploadService', function ($http, $q) { // explained abour controller and service in part 2

    var fac = {};
    fac.UploadFile = function (file, description) {
        var formData = new FormData();
        formData.append("file", file);
        //We can send more data to server using append         
        formData.append("description", description);

        var defer = $q.defer();
        $http.post("Personal/SaveFiles", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            })
        .success(function (d) {
            defer.resolve(d);
        })
        .error(function () {
            defer.reject("File Upload Failed!");
        });

        return defer.promise;

    }
    return fac;

}).factory('fileDeleteService', function ($http, $q) {

    var fac = {};
    fac.DeleteFile = function (imageName) {
        var formData = new FormData();

        formData.append("imageName", imageName);
        var defer = $q.defer();
        $http.post("Personal/DeleteFiles", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            })
        .success(function (d) {
            defer.resolve(d);
        })
        .error(function () {
            defer.reject("File Delete Failed!");
        });

        return defer.promise;

    }
    return fac;

})
;