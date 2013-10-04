WebApiSoup 
==========


.NET WebApi starter kit with must have API Management functionality  built in. This project employs _Code First_ Entity Framework 5 with token authentication, API request limiter, user and application persistence and the sample Soup Recipe API

This project spring boards your next service based Web API using **ASP.NET MVC4 WebAPI**, **JSON** or **XML**, **Entity Framework 5** Code First Modeling and **WebMatrix** Membership implementation. 

=====

##Features##
Along with the default features you get with visual studio's ASP.NET MVC4 WebApi template, this project has the following additional features:

- **Membership Provider**: WebMatrix membership is already coded and running along side the WebAPI's custom user profile
- **User & Application Support**: Entity Framework 5 (Code First) is used to define and persist User profiles and "_API Applications_" those users have registered with your API.
- **Token Based Security**:  No need to worry about building your own API request authentication.  WebApiSoup employs MD5 encryption using a Application Identifier along with a Application Secret.
- **Application Request Limiter**:  Need to keep track of your user's API request count.  Out of the box this solution will support custom daily request limits, monthly request limits or no limit at all.  
- **Taste Test The Soup**: No project is complete with out code samples of how a client would use this API.  This test project demonstrates how to generate a token, assemble the request auth headers and communicate to your HTTP endpoint

=====

##Setup##
This is a Visual Studio 2012 project.  It uses **IIS Express** and the **Local SQL DB**.  During first run, the solution will self configure and EF will create the database and tables if they don't exist.  Make sure **Nuget Package Manager** and **Visual Studio** are up to date if you are experiencing problems.

Once you are up and running EF will have seeded your database with a user called _admin@admin.com_ and password _admin1_.  It will also add one application to the user called _First App_.  The Test Project will use this application to run it's tests.

=====

##Feature Implementations##

###Membership, Roles and API Applications###

Snippets from WebApiSoup / Repository / Migrations / Configuration.cs

**Create a user**

```
	WebSecurity.CreateUserAndAccount("admin@admin.com", "admin1");

```

**Add User to Role**

```
Roles.AddUsersToRoles(new[] { "admin@admin.com" }, new[] 	{ Constants.ROLES_ADMINISTRATOR });

```

**Add an Application to User**

**NOTE:** _This also displays how to define the application rate limits._

```
			//Get the User Profile
 int userId = WebSecurity.GetUserId("admin@admin.com");
 Repository.UserProfile profile = context.UserProfiles
                                                .Where(w => w.UserId == userId)
                                                .FirstOrDefault();
                //Add a new Applicaiton to the user
                profile.Applications.Add(new Application()
                {
                    ApiAccessMetricLimit = Common.Constants.API_ACCESS_METRIC_NO_LIMIT,
                    SubscriptionType = Common.Constants.API_SUBSCRIPTION_TYPE_NO_LIMIT,
                    CreateDtm = DateTime.UtcNow,
                    LastDtmAccessed = DateTime.UtcNow,
                    AppGeneratedSecret = Security.HashSecurity.GenerateAppSecret(),
                    AppGivenName = "First App"
                });
                context.SaveChanges();
                

```

====

###Token Based Security###

Implementation on the Client, Snippet from WebApiSoupTest.cs:

```
        HttpClient client = new HttpClient();
        HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Get, new Uri("http://localhost:58720/api/SoupRecipes/PerformHandshake"));
        // Add an Accept header for JSON format.
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        msg.Headers.Add("AppId", firstAppId);
        msg.Headers.Add("AppToken", CreateToken(firstAppId, firstAppSecret));

        HttpResponseMessage response = client.SendAsync(msg).Result;
        string responseAsString = response.Content.ReadAsStringAsync().Result;
        if(response.StatusCode != System.Net.HttpStatusCode.OK)
        {
             //There was a problem
             Assert.Fail(String.Format("Failed with code: {0}", response.StatusCode));
        }
                
        private string CreateToken(string applicationId, string secret)
        {


            string retVal = string.Empty;
            string tm = ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString();
            string toEncrypt = string.Format("{0}{1}{2}", applicationId, secret, tm);
            try
            {
                return md5hex(toEncrypt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string md5hex(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] data = Encoding.Default.GetBytes(str);
            byte[] hash = md5.ComputeHash(data);
            string hex = "";
            foreach (byte b in hash)
            {
                hex += String.Format("{0:x2}", b);
            }
            return hex;
        }

```

===

###Application Request Limiter###
**Pipeline.AuthMessageHandler** - Use to decorate API methods that require **Token Authentication** also records API requests.
Snippet from WebApiSoup / WebApiSoup / App_Start / WebApiConfig.cs

```
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: null,
                handler: HttpClientFactory.CreatePipeline(
                          new HttpControllerDispatcher(config),
                          new DelegatingHandler[] { new Pipeline.AuthMessageHandler() })

            );

```


## Contributors

* Justin Hyland (author) Email: justin.hyland@live.com


====


##The MIT License##

Copyright (c) 2013 Justin Hyland

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.


[![Bitdeli Badge](https://d2weczhvl823v0.cloudfront.net/hylander0/webapisoup/trend.png)](https://bitdeli.com/free "Bitdeli Badge")

