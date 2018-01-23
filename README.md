# Azure Active Directory Cheat Codes

### Azure Portal
https://portal.azure.com


### Azure AD Portal
https://aad.portal.azure.com


    - define your SPA (Angular/React/Vue) application in Azure AD as "Native App"
        // client that shares no secret with Azure AD (as opposed to being defined as Web App
        // which is a confidential client from Azure AD's perspective)
    
    - go to the Native App -> Manifest -> flip oAuth2ImplicitFlow to true
        // If you are planning an SPA architecture, have no backend components
        // or intend to invoke a Web API via JavaScript, use of the implicit flow
        // for token acquisition is recommended.
        // See https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-dev-understanding-oauth2-implicit-grant,
        // Good level of detail on implicit flow including swim lane diagrams in the
        // Modern Authentication with Azure AD book.
        // Must see -- https://channel9.msdn.com/Shows/Web+Camps+TV/AngularJS-Module-for-Microsoft-Azure-Active-Directory-Authentication
        // (skip to 14:15 for implicit flow whiteboarding)
    
    - click on 'Grant Permissions'
        // Granting explicit consent using the Grant Permissions button is currently required
        // for single page applications (SPA) that use ADAL.js. Otherwise, the application 
        // fails when the access token is requested.
        // If you're building a single tenant web app that calls admin and user scopes/permissions,
        // it will consent for all users in that tenant. This will let the app call any admin scopes,
        // and also suppress consent for all the users inside the tenant.
        // See this - https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-integrating-applications
    
    - define Web API in Azure AD as "Web App / Web API"
        // Also create a secret (key), store it in your API's web.config for later use.
        // What i mean by later use is acquiring a token for Microsoft Graph API for example, but not
        // as the authenticated user, as the application (you can have a larger permission scope for the Web API,
        // e.g. have ReadWrite permissions on Graph API on all users or read anyone's calendar and so on).
    
    - go to the SPA (Angular/React/Vue) app definition ->
                                        required permissions ->
                                        Add ->
                                        search for your API Name ->
                                        grant "Access Web-API-name"
        // This simply grants delegated permissions (act on behalf of logged in user)
        // to your Angular app accessing the API.


### Microsoft Graph (graph.microsoft.com) vs Azure AD Graph (graph.windows.net)
https://blogs.msdn.microsoft.com/aadgraphteam/2016/07/08/microsoft-graph-or-azure-ad-graph/


### Microsoft Graph .NET Client Library
https://github.com/microsoftgraph/msgraph-sdk-dotnet


### Microsoft Graph permissions reference
https://developer.microsoft.com/en-us/graph/docs/concepts/permissions_reference


### Delegated permissions, Application permissions, and effective permissions
https://developer.microsoft.com/en-us/graph/docs/concepts/permissions_reference#delegated-permissions-application-permissions-and-effective-permissions


### Azure AD v1.0 vs v2.0 endpoint
https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-v2-compare

    * If you see https://login.microsoftonline.com/common/oauth2/authorize then it's v1.0
    * If you see https://login.microsoftonline.com/common/oauth2/v2.0/authorize then it's v2.0
    * The version of your Azure AD application depends on what portal was used
      to register it:
        * If in the Azure Portal, then it's a v1 application.
        * If in the App Registration Portal then it's a v2 app.


### Channel 9 -- AngularJS Module for Microsoft Azure Active Directory Authentication
https://channel9.msdn.com/Shows/Web+Camps+TV/AngularJS-Module-for-Microsoft-Azure-Active-Directory-Authentication
(skip to 14:15 for implicit flow whiteboarding)


### App Service built-in token store (aka Easy Auth, aka App Service Authentication/Authorization)
* https://docs.microsoft.com/en-us/azure/app-service/app-service-authentication-overview
* https://cgillum.tech/category/easy-auth/
* https://stackoverflow.com/a/46765687/4148708 (My answer on SO, git blame me if things aren't accurate)
