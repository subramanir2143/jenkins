# Notes
The API Gateway requires Authentication, only authenticated requests will be passing downstream. 

The following Sample code may be used to authenticate request.

```
		private static async Task Main()
        {
            // discover endpoints from metadata
        
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = "http://10.0.75.1:59844",
                Policy =
                        {
                            RequireHttps = false
                        }
            });
           
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            // request token
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",

                Scope = "api1"
            });
            
            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            // call api
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.GetAsync("http://10.0.75.1:59841/api/v1/Employee/test");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
            Console.Read();
        }
        ```