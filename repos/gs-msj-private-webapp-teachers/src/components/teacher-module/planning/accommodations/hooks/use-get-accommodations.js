import { useState } from "react";
import * as accommodations from "../services/accommodations.service";

const useGetAccommodations = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getAccommodationTypes = (accommodation, type) => {
    return accommodation.TeacherPlanAccommodationTypes?.filter(
      (p) => p.AccommodationType?.Type === type
    );
  };

  const getAccommodationTitle = (types) => {
    return types
      ?.map((type) => type.AccommodationType?.Description)
      ?.join(", ");
  };

  const getAccommodations = async (planningId) => {
    setIsLoading(true);

    try {
      const response = await accommodations.getAccommodations(planningId);
      const results = response.map((accommodation) => {
        const defaults = getAccommodationTypes(accommodation, 0);
        const customs = getAccommodationTypes(accommodation, 1);

        return {
          id: accommodation.Id,
          title: getAccommodationTitle(defaults),
          subtitle: getAccommodationTitle(customs),
          defaults: defaults?.map((type) => type.AccommodationTypeId),
          customs: customs?.map((type) => ({
            value: type.AccommodationTypeId,
            label: type.AccommodationType?.Description,
          })),
          implementationDates: accommodation.TeacherPlanAccommodationDates.map(
            (i) => new Date(i.ImplementationDate)
          ),
        };
      });

      setSucceeded(true);
      setData(results);
    } catch (error) {
      setSucceeded(false);
    }

    setIsLoading(false);
  };

  return {
    getAccommodations,
    isLoading,
    succeeded,
    accommodations: data,
  };
};

export default useGetAccommodations;
