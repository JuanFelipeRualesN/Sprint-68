import React, { useState } from "react";
import Text from "../text";
import Alert from "../alert";
import { Translate } from "react-localize-redux";
import { Creatable } from "react-select";
import FormLabel from "./form-label";
import { translate } from "../../utils";

export default function FormCreatableSelect({
  label,
  name,
  placeholder,
  options,
  values,
  disabled,
  onChange,
  className = "",
                                                forEnterDetails = false,
}) {
  const [data, setData] = useState(values);

  const handleChange = (values) => {
    setData(values);
    if (!onChange) return;
    onChange(values);
  };

  return (
    <>
      <div className={`form-group-fields ${className}`}>
        <Creatable
          isClearable
          isMulti
          name={name}
          className="creatable"
          placeholder={placeholder}
          options={options}
          onChange={handleChange}
          defaultValue={data}
          formatCreateLabel={(item) => `+ ${translate("general.add")} ${item}`}
          disabled={disabled}
          noOptionsMessage={() =>
              forEnterDetails
                  ? translate("planning.type-details-press-enter")
                  : translate("general.no-options")
          }
        />
        <i className="bar"></i>
        {label && <FormLabel forHtml="select" label={label} />}
      </div>
      <Text type="hint" className="text-black">
        <>
          <Translate id="accommodation.type-and-press" /> <strong>ENTER</strong>{" "}
          <Translate id="accommodation.to-add" />
        </>
      </Text>
      <Alert
        className="py-0"
        message={<Translate id="accommodation.type-alert" />}
        variant="simple"
        size="sm"
      />
    </>
  );
}
