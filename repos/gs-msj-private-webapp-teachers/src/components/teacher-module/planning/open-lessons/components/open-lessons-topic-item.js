import React, { useEffect, useState } from "react";
import Border from "../../shared/components/border";
import Heading from "../../shared/components/heading";
import FormCheckbox from "../../shared/components/form/form-checkbox";
import { Translate } from "react-localize-redux";
import useGetOpenLessonCriterias from "../hooks/use-get-open-lesson-criterias";
import FormCheckboxGroup from "../../shared/components/form/form-checkbox-group";
import LanguageManager from "../../../../../custom-classes/language-manager";

export default function OpenLessonsTopicItem({
  name,
  values,
  handleChange,
}) {
  const [isActive, setIsActive] = useState(values?.length > 0);
  const { getOpenLessonCriterias, criterias } = useGetOpenLessonCriterias();

  useEffect(() => {
    getOpenLessonCriterias();
  }, [LanguageManager.getLanguage()]);

  useEffect(() => {
    getOpenLessonCriterias();
  }, []);

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
            <Heading as="h6" className="my-4">
              <Translate id={`open-lessons.${name}-subtitle`} />
            </Heading>
            <FormCheckboxGroup
              name="open-lessons"
              options={criterias?.defaults?.map((type) => ({
                value: type.id,
                label: type.name,
              }))}
              values={values}
              onChange={(values) => handleChange(name, values)}
            />
          </>
        )}
      </>
    </Border>
  );
}
