import {graphConfig} from "./authConfig";

/**
 * Attaches a given access token to a MS Graph API call. Returns information about the user
 * @param accessToken
 */
export async function callMsGraphGetUser(accessToken) {
  const headers = new Headers();
  const bearer = `Bearer ${accessToken}`;
  
  headers.append("Authorization", bearer);
  
  const options = {
    method: "GET",
    headers: headers
  };
  
  return fetch(graphConfig.graphMeEndpoint, options)
    .then(response => response.json())
    .catch(error => console.log(error));
}

export async function callMsGraphGetCalendar(accessToken, startDate, endDate) {
  const headers = new Headers();
  const bearer = `Bearer ${accessToken}`;
  
  headers.append("Authorization", bearer);
  
  const options = {
    method: "GET",
    headers: headers
  };
  
  return fetch(`${graphConfig.graphMeCalendar}?startDateTime=${startDate}&endDateTime=${endDate}`, options)
    .then(response => response.json())
    .catch(error => console.log(error));
}
