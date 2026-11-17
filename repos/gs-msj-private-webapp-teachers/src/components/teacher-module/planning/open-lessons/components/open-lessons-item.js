import React, { useState } from "react";
import Border from "../../shared/components/border";
import Heading from "../../shared/components/heading";
import FormCheckbox from "../../shared/components/form/form-checkbox";
import { Translate } from "react-localize-redux";
import FormCreatableSelect from "../../shared/components/form/form-creatable-select";
import { translate } from "../../shared/utils";

export default function OpenLessonsItem({ name, values, handleChange }) {
  const [isActive, setIsActive] = useState(values?.length > 0);

  return (
    <Border side="Bottom">
      <>
        <Heading as="h5">
          <Translate id={`open-lessons.${name}-title`} />
        </Heading>
        <FormCheckbox
          name={`${name}-lesson-item`}
          checked={isActive}
          onChange={setIsActive}
          label={<Translate id="lesson.check-item" />}
          disabled={values?.length > 0}
        />
        {isActive && (
          <>
            <FormCreatableSelect
              name={`${name}-lesson-item-select`}
              placeholder={translate("planning.enter-the-data")}
              options={[]}
              values={values}
              onChange={(values) => handleChange(name, values)}
              forEnterDetails={true}
            />
          </>
        )}
      </>
    </Border>
  );
}
