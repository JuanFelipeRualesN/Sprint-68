import React, { useContext, useEffect, useState } from "react";
import { BreadcrumbContext } from "../shared/providers/breadcrumb";
import { useHistory } from "react-router-dom";
import OpenLessonsDetail from "./components/open-lessons-detail";
import useCreateOpenLesson from "./hooks/use-create-open-lesson";
import Text from "../shared/components/text";
import Heading from "../shared/components/heading";
import Border from "../shared/components/border";
import Button from "../shared/components/button";
import SaveButton from "../shared/components/save-button";
import { translate } from "../shared/utils";
import { Translate } from "react-localize-redux";
import SessionManager from "../../../../custom-classes/session-manager";
import LanguageManager from "../../../../custom-classes/language-manager";
import { Switch } from "@material-ui/core";
import { withStyles } from '@material-ui/core/styles';
import {AI_CHAT_BOT_AVAILABLE} from "../../../../custom-classes/settings";

const StyledSwitch = withStyles({
  switchBase: {
    color: '#fafafa',
    '&$checked': {
      color: '#66BE4E',
    },
    '&$checked + $track': {
      backgroundColor: '#66BE4E',
    },
  },
  checked: {},
  track: {},
})(Switch);

export default function OpenLessonsCreate() {
  const navigate = useHistory();
  const { setPath, resetPaths } = useContext(BreadcrumbContext);
  const { createOpenLesson, isLoading } = useCreateOpenLesson();
  const [lesson, setLesson] = useState({});
  const [isActive, setIsActive] = useState(false);
  const [planningContext, setPlanningContext] = useState(null);
  const { planningId } = SessionManager.getSelectedPlanning();

  const handleCreate = () => {
    return createOpenLesson({ ...lesson, planningId });
  };

  const isValid = () => {
    return (
      lesson?.implementationDates?.length > 0 &&
      lesson?.title && (
        lesson?.standards?.length > 0 ||
        lesson?.expectations?.length > 0 ||
        lesson?.objectives?.length > 0 ||
        lesson?.strategies?.length > 0 ||
        lesson?.topics?.length > 0 ||
        lesson?.units?.length > 0 ||
        lesson?.initial?.length > 0 ||
        lesson?.development?.length > 0 ||
        lesson?.close?.length > 0
      )
    );
  };

  useEffect(() => {
    const loadPlanningContext = () => {
      const planning = SessionManager.getSelectedPlanning();
      if (planning) {
        const context = {
          Subject: planning.subjectName,
          GradeCode: planning.levelCode,
        };
        
        setPlanningContext(context);
      }
    };
    
    loadPlanningContext();
  }, [planningId]);

  useEffect(() => {
    resetPaths();
    setPath({
      root: translate("planning.menu-open"),
      name: translate("planning.menu-lessons"),
      url: "?page=open-lessons",
    });
    setPath({ name: translate("general.create-new") });
  }, [LanguageManager.getLanguage()]);


  return (
    <>
      <Heading as="h2" className="mb-0">
        <Translate id="general.create-new" />
      </Heading>

      <Border side="Bottom">
        <Text type="regular">
          <Translate id="lesson.sub-title" />
        </Text>
      </Border>

      <Border side="Bottom" className="d-flex">
        <Button
          variant="regular"
          onClick={() => navigate.push("?page=open-lessons")}
          disabled={isLoading}
        >
          <Translate id="confirm-dialog.button-cancel" />
        </Button>
        <SaveButton
          disabled={isLoading || !isValid()}
          className="ml-2"
          onClick={handleCreate}
          onConfirmation={() => {
            navigate.push("?page=open-lessons");
          }}
          isLoading={isLoading}
        />
        {AI_CHAT_BOT_AVAILABLE ?
        <div style={{marginLeft: '20px'}}>
          <span style={{fontWeight: 'bold'}}><Translate id="chatbot.activate-suggestions" /></span>
          <StyledSwitch
            checked={isActive}
            onChange={(e) => setIsActive(e.target.checked)}
            disabled={isLoading}
          />
        </div> : null
        }

      </Border>

      <OpenLessonsDetail 
        lesson={lesson} 
        onChange={setLesson}
        suggestionsEnabled={isActive}
        planningContext={planningContext}
      />
    </>
  );
}
