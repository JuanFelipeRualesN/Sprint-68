import { useState } from "react";
import * as accommodations from "../../accommodations/services/accommodations.service";
import { format, translate } from "../../shared/utils";

const useGetAccommodationsForPreview = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getAccommodations = async (planningId) => {
    setIsLoading(true);

    try {
      const response = await accommodations.getAccommodations(planningId);
      const results = response.flatMap((accommodation) => {
        return accommodation.TeacherPlanAccommodationDates.map(
          (date) => ({
            id: accommodation.Id,
            type: "default",
            title: translate("preview.table-instructional-strategies"),
            description: accommodation.Description,
            date: format(date.ImplementationDate),
          })
        );
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

export default useGetAccommodationsForPreview;
