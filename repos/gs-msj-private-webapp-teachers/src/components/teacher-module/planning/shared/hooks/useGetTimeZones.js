import { useState } from "react";
import axios from "../../../../../api/interceptors";
import Settings from "../../../../../custom-classes/settings";

const useGetTimeZones = () => {
  const [timezones, setTimeZones] = useState([]);

  const getTimeZones = async () => {
    try {
      const endpoint = Settings.getAllTimeZonesAPI();
      const response = await axios.get(endpoint);
      const data = response.status === 200 ? response.data : [];

      const results = data.map((timezone) => {
        return {
          value: timezone.Code,
          label: timezone.Desc,
        };
      });

      setTimeZones(results);
    } catch (error) {}
  };

  return {
    getTimeZones,
    timezones,
  };
};

export default useGetTimeZones;
