import React, { useRef, useState } from "react";
import { Translate } from "react-localize-redux";
import SessionManager from "../../../../../custom-classes/session-manager";
import Heading from "../../shared/components/heading";
import Space from "../../shared/components/space";
import FormField from "../../shared/components/form/form-field";
import DateSelections from "../../shared/components/date-selections";
import moment from "moment";
import ReactQuill from "react-quill";
import Settings from "../../../../../custom-classes/settings";
import FormLabel from "../../shared/components/form/form-label";

export default function ForumsDetail({ forum, onChange = () => { } }) {
  const planning = SessionManager.getSelectedPlanning();
  const [forumState, setForumState] = useState(forum);
  const [description, setDescription] = useState(forum?.description ?? '');
  const quillRef = useRef();

  const handleChange = (name, value) => {
    const newState = { ...forumState, [name]: value };
    setForumState(newState);
    onChange(newState);
  };

  const checkCharacterCount = (event) => {
    const unprivilegedEditor = quillRef.current.unprivilegedEditor;
    const contentLength = unprivilegedEditor.getLength();
    if (contentLength > Settings.FORUM_MAX_CHARACTERS && event.key !== 'Backspace') {
      event.preventDefault();
    }
  };

  const handleDescription = (value) => {
    setDescription(value);
    handleChange("description", value);
  };

  return (
    <>
      <DateSelections
        fromDate={moment(planning?.openDate).toDate()}
        toDate={moment(planning?.closeDate).toDate()}
        values={forum.implementationDates}
        onChange={(dates) => handleChange("implementationDates", dates)}
      />

      <Space variant="regular" />

      <Heading as="h3" className="mb-0">
        <Translate id="forum.create-message-content" />
      </Heading>

      <div className="wrapperFlex FlexAlingEnd w-50 mb-2">
        <div className="col title px-0">
          <FormField
            name="title"
            label={<Translate id="forum.header-forum-title" />}
            value={forumState.title}
            onChange={(event) => handleChange("title", event.target.value)}
          />
        </div>
      </div>

      <div className="wrapperFlex FlexAlingEnd w-75 mb-2">
        <div className="col description px-0">
          <div className="form-group-fields">
            <ReactQuill theme='snow'
              name="description"
              modules={Settings.FORUM_MODULES}
              onKeyDown={checkCharacterCount}
              ref={quillRef}
              value={description}
              onChange={handleDescription}
            />
            <i className="bar"></i>
            <FormLabel label={<Translate id="forum.label-description" />} />
          </div>
        </div>
      </div>
    </>
  );
}
