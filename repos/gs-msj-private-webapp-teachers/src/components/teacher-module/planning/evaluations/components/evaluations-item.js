import React, { useState } from "react";
import { translate } from "../../shared/utils";
import FormCheckbox from "../../shared/components/form/form-checkbox";
import FormCreatableSelect from "../../shared/components/form/form-creatable-select";

export default function EvaluationsItem({ code, values, options, handleChange = () => { } }) {
  const [isActive, setIsActive] = useState(values?.length > 0);

  return (
    <div className="wrapperFlex FlexAlingEnd w-100 mb-2 align-items-baseline">
      <FormCheckbox
        name={`otherEvaluationTypeCheck-${code}`}
        label={translate("general.other")}
        checked={isActive}
        onChange={setIsActive}
        disabled={values?.length > 0}
      />
      {isActive && (
        <div className="col">
          <FormCreatableSelect
            name={`otherEvaluationType-${code}`}
            placeholder={translate("general.select")}
            options={options?.map((type) => ({
              value: type.id,
              label: type.description,
            }))}
            values={values}
            onChange={(values) =>
              handleChange(code, "customs", values)
            }
          />
        </div>
      )}
    </div>
  );
}
