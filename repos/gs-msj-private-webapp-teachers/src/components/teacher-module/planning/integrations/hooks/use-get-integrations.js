import { useState } from "react";
import * as integrations from "../services/integrations.service";

const useGetIntegrations = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [succeeded, setSucceeded] = useState(false);

  const getIntegrations = async (planningId) => {
    setIsLoading(true);

    try {
      const response = await integrations.getIntegrations(planningId);
      const results = response.map((integration) => {
        return {
          id: integration.Id,
          planningId: integration.TeacherPlanId,
          title: integration.Description,
          subjects: integration.TeacherPlanOtherSubjectInegrationSubjects.map(
            (subject) => subject.SubjectId.toString()
          ),
          implementationDates: integration.TeacherPlanOtherSubjectIntegrationDates.map(
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
    getIntegrations,
    isLoading,
    succeeded,
    integrations: data,
  };
};

export default useGetIntegrations;
