import { useState } from "react";
import * as materials from "../../materials/services/materials.service";
import { format, translate } from "../../shared/utils";

const useGetMaterialsForPreview = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getMaterials = async (planningId) => {
    setIsLoading(true);

    try {
      const response = await materials.getMaterials(planningId);
      const results = response.flatMap((material) => {
        return material.TeacherPlanMaterialDates.map(
          (date) => ({
            id: material.Id,
            type: "default",
            title: translate("preview.table-materials"),
            description: material.Description,
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
    getMaterials,
    isLoading,
    succeeded,
    materials: data,
  };
};

export default useGetMaterialsForPreview;
