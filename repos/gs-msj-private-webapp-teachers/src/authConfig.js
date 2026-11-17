import { LogLevel } from "@azure/msal-browser";
import Settings from "./custom-classes/settings";

export const msalConfig = {
    auth: {
        clientId: Settings.APP_AZURE_CLIENT_ID,
        authority: "https://login.microsoftonline.com/common/",
        redirectUri: Settings.AZURE_REDIRECT_URI,
    },
    cache: {
        cacheLocation: "sessionStorage",
        storeAuthStateInCookie: false,
    },
    system: {	
        loggerOptions: {	
            loggerCallback: (level, message, containsPii) => {	
                if (containsPii) {		
                    return;		
                }		
                switch (level) {
                    case LogLevel.Error:
                        console.error(message);
                        return;
                    case LogLevel.Info:
                        console.info(message);
                        return;
                    case LogLevel.Verbose:
                        console.debug(message);
                        return;
                    case LogLevel.Warning:
                        console.warn(message);
                        return;
                    default:
                        return;
                }	
            }	
        }	
    }
};

export const graphConfig = {
    graphMeEndpoint: "https://graph.microsoft.com/v1.0/me",
    graphMeCalendar: "https://graph.microsoft.com/v1.0/me/calendar/calendarView"
};
