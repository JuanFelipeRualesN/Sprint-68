import React, { useEffect, useState } from "react";
import Border from "./border";
import Heading from "./heading";
import FormRadioButtom from "./form/form-radio-button";
import FormMultiDatePicker from "./form/form-multi-date-picker";
import { getDatesBetween, translate } from "../utils";
import { Translate } from "react-localize-redux";

export default function DateSelections({
  fromDate,
  toDate,
  values = [],
  onChange,
  labelClassName = "",
}) {
  const [isForWholeWeek, setIsForWholeWeek] = useState();

  const handleChangeDateOption = (value) => {
    setIsForWholeWeek(value === "YES");
  };

  useEffect(() => {
    if (isForWholeWeek === undefined) return;

    if (isForWholeWeek === false) {
      onChange([]);
      return;
    }

    const dates = getDatesBetween(new Date(fromDate), new Date(toDate));
    onChange(dates);
  }, [isForWholeWeek]);

  return (
    <>
      <Border side="Bottom">
        <Heading as="h3" className='mt-2'>
          <Translate id="messages.activity-date-title" />
        </Heading>
        <FormRadioButtom
          name="applyAllWeek"
          label={<Translate id="messages.apply-whole-week" />}
          options={[
            { label: translate("general.yes"), value: "YES" },
            { label: translate("general.no"), value: "NO" },
          ]}
          value="NO"
          onChange={handleChangeDateOption}
        />
      </Border>
      {!isForWholeWeek && (
        <Border side="Bottom">
          <FormMultiDatePicker
            label={<Translate id="messages.select-available-dates" />}
            minDate={fromDate}
            maxDate={toDate}
            values={values}
            onChange={(dates) => {
              if (!dates) return;
              onChange(dates);
            }}
            labelClassName={labelClassName}
          />
        </Border>
      )}
    </>
  );
}
