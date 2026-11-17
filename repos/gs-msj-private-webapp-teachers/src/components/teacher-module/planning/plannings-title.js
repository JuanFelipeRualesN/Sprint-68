import React, { useRef, useState } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import "./shared/styles/plannings.css";
import useModifyPlanningName from "./shared/hooks/useModifyPlanningName";
import { faPenSquare, faSave } from "@fortawesome/free-solid-svg-icons";
import Button from "./shared/components/button";

export default function PlanningsTitle({ planningId, name, onChange = () => {} }) {
  const [title, setTitle] = useState(name);
  const [isEditing, setIsEditing] = useState(false);
  const planningTitleRef = useRef(null);
  const { updatePlanningName } = useModifyPlanningName();

  const handleChange = (event) => {
    setTitle(event.target.value);
  };

  const handleEditingFlag = () => {
    setIsEditing(!isEditing);
  };

  const handleSave = async () => {

    if(!title || title.trim().length === 0){
      return;
    }

    const succeeded = await updatePlanningName(planningId, title);
    if (!succeeded) return;

    onChange(title)
    handleEditingFlag();
  };

  return (
    <>
      {!isEditing && (
        <>
          <span>{title}</span>
          <Button
            className="space-margin-left-sm icon-20px"
            variant="simple"
            icon={<FontAwesomeIcon icon={faPenSquare} />}
            onClick={() => {
              handleEditingFlag();
              setTimeout(() => {
                planningTitleRef?.current?.focus();
              }, 300);
            }}
          />
        </>
      )}
      {isEditing && (
        <div className="planning-title">
          <input
            ref={planningTitleRef}
            type="text"
            value={title}
            onChange={handleChange}
            onBlur={handleSave}
            onKeyDown={(event) => {
              if (event.key === "Enter") handleSave();
            }}
          />
          <Button
            variant={"simple"}
            disabled={title === name || !title || title.trim() === ""}
            icon={<FontAwesomeIcon icon={faSave} />}
          />
        </div>
      )}
    </>
  );
}
