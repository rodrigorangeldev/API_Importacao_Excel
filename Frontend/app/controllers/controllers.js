app.controller('HomeCtrl', function ($rootScope, $location, $scope, apiService, $timeout) {
    $rootScope.activetab = $location.path();
    $scope.status = 0;
    
    $scope.handleSubmit = function(event){
        event.preventDefault();
        
        $scope.erros = [];
        $scope.isLoading = true;
        let file = new FormData();
        file.append("excelFile", event.target[0].files[0]);

        apiService.getDados({
            method: 'POST',
            resource: '/Import',
            params: file
        })
        .then(response => {
            $scope.isLoading = false;
            $scope.status = response.status;
            
            if($scope.status == 200){
                const { id } = response.data;

                $timeout(function(){$location.path("/import/"+id);},1000);
                             
            }
            
        }, 
        ({ data }) => {
            $scope.isLoading = false;
            if(data){
                $scope.erros = data.erros;
                document.getElementById("importForm").reset();
            }
        });
        
    };

    $scope.imports = [];
    $scope.isLoading = false;
    $scope.erros = [];
    $scope.getAllImports = function(){
        $scope.isLoading = true;
        $scope.erros = [];
        apiService.getDados({
            method: 'GET',
            resource: '/Import'
        })
        .then(response => { 
            $scope.imports = response.data; 
            $scope.isLoading = false; 
            //$scope.status = response.status;
        }, 
        ({ data }) => {
            $scope.isLoading = false;
            if(data){
                $scope.erros = data.erros;
            }
        });


    };


    $scope.getAllImports();

});

app.controller('ImportCtrl', function ($rootScope, $location, $scope, $routeParams, apiService) {
    //$rootScope.activetab = $location.path();
    
    $scope.GetImportById  = function(id){
        
        apiService.getDados({
            method: 'GET',
            resource: '/Import/'+ id
        })
        .then(response => { 
            $scope.imports = response.data; 
            $scope.isLoading = false; 
            $scope.status = response.status;
        }, 
        ({ data }) => {
            $scope.isLoading = false;
            if(data){
                $scope.erros = data.erros;
            }
        });
    };

    $scope.GetImportById($routeParams.id);
    
});

