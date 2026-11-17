import React, {useState, useEffect, useMemo, useRef} from 'react';
import {Alert, Col, Input, Label, Row} from 'reactstrap';
import { Link, useLocation } from 'react-router-dom';
/** Multiple Language imports **/
import {withLocalize} from "react-localize-redux";
import {renderToStaticMarkup} from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
import {Translate} from "react-localize-redux";
import {Redirect, useHistory} from "react-router-dom";
/** End of Multiple Language imports **/
import {
    faArrowLeft, faInfoCircle,
} from '@fortawesome/free-solid-svg-icons';
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import Settings from "../../../../custom-classes/settings";
import {BeatLoader} from "react-spinners";
import Select from "react-select";
import {getInstruccionalPlanning} from "../../../../api/teachers";
import {createForum} from "../../../../api/forum";
import ReactQuill from "react-quill";
import useVersionedTranslations from '../../../../hooks/useVersionedTranslations';

const ForumCreate = ({ initialize, addTranslation }) => {
    const [forumId, setForumId] = useState(null);
    const [teacherPlanId, setTeacherPlanId] = useState(null);
    const [teacherPlanIdFromQuery, setTeacherPlanQueryParam] = useState(null);
    const [loading, setLoading] = useState(false);
    const [options, setOptions] = useState([]);
    const [forumTitle, setForumTitle] = useState('');
    const [forumDescription, setForumDescription] = useState('');
    const [goBackRoute, setGoBackRoute] = useState("/forums");
    const [showError, setShowError] = useState(false);
    const quillRef = useRef();
    const location= useLocation();
    const history = useHistory();

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        // Extract query parameters
        const searchParams = new URLSearchParams(location.search);
        const forumIdFromQuery = searchParams.get('forumId');
        const teacherPlanIdFromQuery = searchParams.get('teacherPlanId');

        setForumId(forumIdFromQuery);

        if (teacherPlanIdFromQuery) {
            setTeacherPlanId(teacherPlanIdFromQuery);
            setTeacherPlanQueryParam(teacherPlanIdFromQuery);
        }
    }, []);

    useEffect(() => {
        getInstructionalPlans();
    }, []);

    useEffect(() => {
        if(teacherPlanIdFromQuery !== null && teacherPlanIdFromQuery !== undefined && teacherPlanIdFromQuery !== ""){
            setGoBackRoute(`/planning-detail`);
        }
    }, [teacherPlanIdFromQuery]);


    const handleTeacherPlanIdSelectChange = (selectedOption) => {

        setTeacherPlanId(selectedOption.value);
    }

    const getInstructionalPlans =() => {
        setLoading(true);
        let apiCall = getInstruccionalPlanning();
        let arrOptions = [];

        apiCall.then((response) => {
            for (let i = 0; i < response.data.length; i++) {
                arrOptions.push({
                    value: response.data[i].PlanId,
                    label: response.data[i].PlanName
                });
            }
        }).then(() => {
            setOptions(arrOptions);
            setLoading(false);
        });
    }

    const handleForumTitleInput = (event) => {
        setForumTitle(event.target.value);
    }

    const handleCreateForumButton = async () => {
            setLoading(true);
            await createForum(teacherPlanId, forumTitle, forumDescription).then((response) => {
                setLoading(false);
                history.push(goBackRoute);
        });
    }

    const checkCharacterCount = (event) => {
        const unprivilegedEditor = quillRef.current.unprivilegedEditor;
        const contentLength = unprivilegedEditor.getLength();
        if (contentLength > Settings.FORUM_MAX_CHARACTERS && event.key !== 'Backspace') {
            event.preventDefault();
        }
    };

    const handleChange = (value) => {
        setForumDescription(value);
        const unprivilegedEditor = quillRef.current.unprivilegedEditor;
        const contentLength = unprivilegedEditor.getLength();
        if (contentLength > Settings.FORUM_MAX_CHARACTERS)
        {
            setShowError(true);
        }
        else
        {
            setShowError(false);
        }
    };

    return (
        <div>
            <div className="container-padding animated delay-05s fadeIn">
                <div className="wrapper-principal-container">
                    <Row>
                        <Col xs={12}>
                            <Link to={goBackRoute} className="btn-back">
                                <FontAwesomeIcon icon={faArrowLeft} />{' '}
                                <Translate id="general.back-action" options={{ renderInnerHtml: true }} />
                            </Link>
                        </Col>
                    </Row>
                    <Row>
                        <Col xs={12}>
                            <Row>
                                <Col xs={12}>
                                    <Alert color="info">
                                        <FontAwesomeIcon icon={faInfoCircle}/>{' '}
                                        {teacherPlanId !== null ? <Translate id="forum.forum-creation-instructions"
                                                                             options={{renderInnerHtml: true}}/> :
                                            <Translate id="forum.forum-creation-instructions-forums-screen"
                                                       options={{renderInnerHtml: true}}/>
                                        }
                                    </Alert>
                                    <div className="edit-content__title__inputGroup">
                                        <Label for="forum-title">
                                            <Translate id="forum.instructional-plan" options={{renderInnerHtml: true}}/>
                                        </Label>
                                        <div className="col-auto-size">
                                            {loading ?
                                                <BeatLoader size={10} color={"#6bc4ff"}/> :
                                                <div style={{width: '60%'}}>
                                                    <Select
                                                        isDisabled={teacherPlanIdFromQuery !== null}
                                                        placeholder={<Translate id="progress.select-planning"/>}
                                                        styles={{menu: provided => ({...provided, zIndex: 9999})}}
                                                        classNamePrefix="mantine-like"
                                                        className="mantine-like__control"
                                                        onChange={handleTeacherPlanIdSelectChange}
                                                        value={options.filter(option => option.value === teacherPlanId)}
                                                        options={options}
                                                        isSearchable={true}
                                                        noOptionsMessage={() => <Translate id="common.no-options" />}
                                                    />
                                                </div>}
                                        </div>
                                    </div>
                                    <div className="edit-content__title__inputGroup">
                                        <Label for="forum-write-title">
                                            <Translate id="forum.enter-forum-title" options={{renderInnerHtml: true}}/>
                                        </Label>
                                        <Input value={forumTitle} onChange={handleForumTitleInput}
                                               maxLength={Settings.FORUM_TITLE_MAX_LENGTH}/>
                                    </div>
                                    <div className="edit-content__title__inputGroup">
                                        <Label for="forum-write-description">
                                            <Translate id="forum.enter-forum-description" options={{renderInnerHtml: true}}/>
                                        </Label>
                                        <div className="edit-content__title__inputGroup">
                                            <div style={{width: '90%'}}>
                                                <ReactQuill theme='snow'
                                                            modules={Settings.FORUM_MODULES}
                                                            onKeyDown={checkCharacterCount}
                                                            ref={quillRef}
                                                            value={forumDescription}
                                                            onChange={handleChange}
                                                />
                                            </div>
                                        </div>
                                    </div>
                                    <div className="edit-content__title__inputGroup">
                                        {showError ?
                                            <Alert color="info">
                                                <Translate id="forum.error-length-post" options={{ renderInnerHtml: true }} />
                                                {' '} {Settings.FORUM_MAX_CHARACTERS}
                                            </Alert> : null}
                                        <button type="submit" className="btn-reply-submit" disabled={teacherPlanId === null ||
                                        forumTitle === null || forumTitle.trim() === '' || forumDescription === null ||
                                        forumDescription.trim() === '' || forumDescription.trim() === "<p><br></p>" || showError || loading}
                                                onClick={handleCreateForumButton}>
                                            <Translate id="forum.save" options={{renderInnerHtml: true}}/>
                                        </button>
                                    </div>
                                </Col>
                            </Row>
                        </Col>
                    </Row>
                </div>
            </div>
        </div>
    );
};

export default withLocalize(ForumCreate);
