
import React, { useEffect, useContext } from "react";
import TabUserTypes from "../../tabs/tab-user-types";
import { WizardContext } from "../../modals/modal-subscription";


const Step6 = ({ stepCompleted, schoolId, subscriptionId, typePlan }) => {

  const { data } = useContext(WizardContext);

  let subscription = subscriptionId || (data ? data.subscriptionId : undefined);

  useEffect(() => {
    stepCompleted(true);
  }, []);

  return (
    <>
      <TabUserTypes addRemoveButton={true} schoolId={schoolId} showTeachers={typePlan === "1"}
        subscriptionId={subscription} />
    </>
  );
};

export default Step6;
