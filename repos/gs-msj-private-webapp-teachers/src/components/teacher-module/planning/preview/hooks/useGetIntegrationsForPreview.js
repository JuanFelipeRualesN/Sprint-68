import { useState } from "react";
import * as integrations from "../../integrations/services/integrations.service";
import { format, translate } from "../../shared/utils";

const useGetIntegrationsForPreview = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getIntegrations = async (planningId) => {
    setIsLoading(true);

    try {
      const response = await integrations.getIntegrations(planningId);
      const results = response.flatMap((integration) => {
        return integration.TeacherPlanOtherSubjectIntegrationDates.map(
          (date) => ({
            id: integration.Id,
            type: "default",
            title: translate("preview.table-integrations"),
            description: integration.Description,
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
    getIntegrations,
    isLoading,
    succeeded,
    integrations: data,
  };
};

export default useGetIntegrationsForPreview;
