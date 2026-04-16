// filepath: c:\Users\DELL\source\repos\BaseWebApp\Vue3\src\authConfig.js
import { PublicClientApplication } from "@azure/msal-browser";

const msalConfig = {
    auth: {
        clientId: process.env.VUE_APP_MSAL_CLIENT_ID, // Use environment variable
        authority: process.env.VUE_APP_MSAL_AUTHORITY, // Use environment variable
        redirectUri: process.env.VUE_APP_MSAL_REDIRECT_URI, // Use environment variable
        postLogoutRedirectUri: process.env.VUE_APP_MSAL_POST_LOGOUT_REDIRECT_URI, // Use environment variable
        
    },
    cache: {
        storeAuthStateInCookie: true,
        
    },
};

export const msalInstance = new PublicClientApplication(msalConfig);
export const initializeMsal = async () => {
    try {
        // Maneja el redireccionamiento
        await msalInstance.initialize();
        const response = await msalInstance.handleRedirectPromise();
        if (response) 
        {
            

            msalInstance.setActiveAccount(response.account);
            window.location.href = "/pages/users/user-info";
        
        } 
        else 
        {

            
            const accounts = msalInstance.getAllAccounts();
            if (accounts.length > 0) 
                {
                    
                    msalInstance.setActiveAccount(accounts[0]);
                }
        }
    } catch (error) {
        console.error("Error al manejar el redireccionamiento a:", error);
    }
};
 