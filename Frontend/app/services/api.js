angular.module('app').factory('apiService', ($http) => {

    const baseURL = 'http://localhost:62294/api';

	const _getDados = (params) => {
     
		var req = {
				method: params.method,
				url: `${baseURL}${params.resource}`,
				headers: {
					'Content-Type': undefined
				},
                data: params.params
            };
            console.log(req);
            // if(params.method == 'GET')
            //     delete req.headers
  
		   return $http(req);
	};

	return {
		getDados: _getDados
	};

} );
