import { useEffect, useState } from 'react';
import axios from 'axios';
import moment from 'moment-timezone';
import {useMsal} from "@azure/msal-react";

const useTimeZones = () => {
    const {instance, accounts} = useMsal();
    const [timeZones, setTimeZones] = useState([]);

    useEffect(() => {
        const fetchTimeZonesAndUserTimeZone = async () => {
            try {
                const token = await getAccessToken();

                // Obtén las zonas horarias
                const timeZonesResponse = await axios.get("https://graph.microsoft.com/v1.0/me/outlook/supportedTimeZones(TimeZoneStandard=microsoft.graph.timeZoneStandard'Iana')", {
                    headers: { Authorization: `Bearer ${token}` }
                });
                setTimeZones(timeZonesResponse.data.value.map(item => ({label: item.displayName, value: item.alias})));

            } catch (error) {
                console.error('Error fetching time zones and user time zone:', error);
            }
        };

        fetchTimeZonesAndUserTimeZone();
    }, []);

    const getAccessToken = async () => {
        let token;
        try {
            token = await instance.acquireTokenSilent({
                scopes: ["User.Read", "MailboxSettings.Read"],
            });
        } catch (ex) {

            token = await instance.acquireTokenPopup({
                scopes: ["User.Read", "MailboxSettings.Read"],
            });
        }
        return token.accessToken;
    }

    return timeZones;
};

export default useTimeZones;