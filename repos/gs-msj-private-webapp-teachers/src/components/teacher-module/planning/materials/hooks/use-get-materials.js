import { useState } from "react";
import * as materials from "../services/materials.service";

const useGetMaterials = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getMaterialTypes = (material, type) => {
    return material.TeacherPlanMaterialTypes?.filter(
      (p) => p.MaterialType?.Type === type
    );
  };

  const getMaterialTitle = (types) => {
    return types?.map((type) => type.MaterialType?.Description)?.join(", ");
  };

  const getMaterials = async (planningId) => {
    setIsLoading(true);

    try {
      const response = await materials.getMaterials(planningId);
      const results = response.map((material) => {
        const defaults = getMaterialTypes(material, 0);
        const customs = getMaterialTypes(material, 1);

        return {
          id: material.Id,
          title: getMaterialTitle(defaults),
          subtitle: getMaterialTitle(customs),
          defaults: defaults?.map((type) => type.MaterialTypeId),
          customs: customs?.map((type) => ({
            value: type.MaterialTypeId,
            label: type.MaterialType?.Description,
          })),
          implementationDates: material.TeacherPlanMaterialDates.map(
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
    getMaterials,
    isLoading,
    succeeded,
    materials: data,
  };
};

export default useGetMaterials;
