/**
 * Created by portiz on 5/08/20.
 */
import React, { useState, useEffect, useRef, useCallback } from 'react';
import {
    Col,
    Row,
    Input, Alert, Label
} from 'reactstrap';

/** Multiple Language imports **/
import { withLocalize, Translate } from "react-localize-redux";
import { renderToStaticMarkup } from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
/** End of Multiple Language imports **/
import 'react-datepicker/dist/react-datepicker.css';
import { getTeacherForumsDetails, createForum, createPost, updateForum, getTeacherPlanStudents } from "../../../api/teachers";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faEdit } from "@fortawesome/free-solid-svg-icons";
import { Redirect } from "react-router-dom";
import Settings from "../../../custom-classes/settings";
import { getTimeZone, convertMaxLengthSting } from "../../../utils/utils";
import moment from "moment";
import "moment-timezone";
import 'react-quill/dist/quill.snow.css';
import SessionManager from '../../../custom-classes/session-manager';
import InputCreatePost from './forum-input-create-post';
import ParentPosts from './forum-posts';
import PaginationCustom from '../../shared-module-components/datatables/pagination';
import ModalForumPostArchive from '../modal/modal-forum-post-archive';
import EmptyForumStyled from './forum-empty';
import {Editor} from './memoized-editor';
import LoadingFullWindow from '../../shared-module-components/loader-windows/loader-full-window';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

const ForumCreationAndView = ({ forumId: propForumId, isCreation: propIsCreation, disabled: propDisabled, updateParent, teacherPlanId: propTeacherPlanId }) => {
    const editorRef = useRef(null);
    const [state, setState] = useState({
        forumId: typeof propForumId !== 'undefined' && propForumId !== null ? propForumId : null,
        isCreation: typeof propIsCreation !== 'undefined' && propIsCreation !== null ? propIsCreation : false,
        forumTitle: '',
        forumDescription: '',
        originalForumDescription: '',
        originalForumTitle: '',
        teacherPlanId: propTeacherPlanId || null,
        currentPage: 1,
        pageSize: 10,
        editing: false,
        loadedData: false,
        disabled: typeof propDisabled !== 'undefined' && propDisabled !== null ? propDisabled : null,
        redirectToPlanning: false,
        planningName: SessionManager.getSelectedPlanning().planningName,
        studentsGroups: [],
        totalPosts: 0,
        showError: false,
        showPost: false,
        postContent: '',
        refreshKey: 0, 
        posts: [],
        itemSelectedToArchive: {},  
        openConfirm: false,
        totalPages: 0,
        showModalArchivePost: false,
        parentPosts: 0,
        isPosting: false
    });

    const [forumDetails, setForumDetails] = useState({});
    const [visibleChildren, setVisibleChildren] = useState({});
    const [deletedPostId, setDeletedPostId] = useState(null);

    // Initialize translations based on the current version
    useVersionedTranslations();

    // Forum data fetch
    const getForumDetailById = useCallback(async () => {
        if (!state.forumId) return;
        const [allForumDetailsResponse, planStudentsResponse] = await Promise.all([
                getTeacherForumsDetails(state.forumId, state.currentPage, state.pageSize),
                getTeacherPlanStudents(state.teacherPlanId)
            ]);

        
        try {
          const { ForumModel, AuthorName, ForumPostModels, TotalPages, TotalItems,  } = allForumDetailsResponse.data;
          setForumDetails({...ForumModel, AuthorName});

          setState(prevState => ({
            ...prevState,
            studentsGroups: planStudentsResponse.data,
            posts: ForumPostModels,
            totalPages: TotalPages,
            totalPosts: TotalItems,
            loadedData: true,
          }));
        
        } catch (error) {
          console.error('Error fetching forum details:', error);
        } finally {
          setState(prevState => ({ ...prevState, loading: false }));
        }
        
      }, [state.forumId, state.currentPage, state.pageSize, state.refreshKey]);

    useEffect(() => {
        if (state.forumId !== null) {
          getForumDetailById();
          setState(prevState => ({ ...prevState, loading: true }));
        }

      }, []);

    useEffect(() => {
        if (state.forumId !== null) {
            getForumDetailById();
        }
    }, [state.forumId, state.currentPage]);

    const checkCharacterCount = useCallback((e) => {
        if (editorRef.current !== null) {
            const unprivilegedEditor = editorRef.current.unprivilegedEditor;
            const contentLength = unprivilegedEditor.getLength();
            if (contentLength > Settings.FORUM_MAX_CHARACTERS && e.key !== 'Backspace') {
                e.preventDefault();
            }
        }
    }, []);

    const changeForumDescription = useCallback((content) => {
        if (!editorRef.current?.unprivilegedEditor) return;
        
        const contentLength = content.length;
        
        if (content !== state.forumDescription) {
            setState(prevState => ({
                ...prevState,
                forumDescription: content,
                showError: contentLength > Settings.FORUM_DESCRIPTION_MAX_LENGTH
            }));
        }
    }, [state.forumDescription]);

    const changeForumTitle = useCallback((e) => {
        let text = e.target.value;
        setState(prevState => ({ ...prevState, forumTitle: text }));
    }, []);

    const cancelForumEdit = () => {
        setState(prevState => ({
            ...prevState,
            forumTitle: prevState.originalForumTitle,
            forumDescription: prevState.originalForumDescription,
            editing: false,
        }));
    }

    const toggleIsEditing = useCallback(() => {
        setState(prevState => ({
            ...prevState,
            editing: !prevState.editing,
            forumTitle: forumDetails.Title,
            forumDescription: forumDetails.Description, 
            originalForumTitle: forumDetails.Title, 
            originalForumDescription: forumDetails.Description,
        }));
    }, [forumDetails]);

    const handlePostContentChange = useCallback((content) => {
        setState(prevState => ({ ...prevState, postContent: content }));
    }, []);

    const handleShowPost = useCallback(() => {
        setState(prevState => ({ ...prevState, showPost: true }));
    }, []);

    const hidePost = useCallback(() => {
        setState(prevState => ({ ...prevState, showPost: false }));
    }, []);

    const addPost = useCallback(async () => {
        // Double check if loading modal fails
        if (state.isPosting) return;
        try {
            setState(prevState => ({ ...prevState, loading: true, isPosting: true }));
            const req = {
                ForumId: state.forumId,
                Body: state.postContent,
            }

            await createPost(req);
            
            const response = await getTeacherForumsDetails(
                state.forumId, 
                state.currentPage, 
                state.pageSize
            );
            
            setState(prevState => ({
                ...prevState,
                showPost: false,
                loading: false,
                postContent: '',
                refreshKey: prevState.refreshKey + 1,
                totalPosts: response.data.TotalItems
            }));
            getForumDetailById();
        } catch (error) {
            console.error('Error in post creation or update:', error);
        } finally {
            setState(prevState => ({ ...prevState, isPosting: false }));
            setState(prevState => ({ ...prevState, loading: false }));
        }
    }, [state.forumId, state.currentPage, state.pageSize, state.postContent, state.isPosting]);

    const handleOpenConfirmDialog = (item) => {
        setState(prevState => ({
            ...prevState,
            itemSelectedToArchive: item,
            openConfirm: true,
            showModalArchivePost: true
        }));
    }

    const handleChangePaginate = (page) => {
        // Avoid fetching if the user clicks on the current page
        setState(prevState => {
            if (prevState.currentPage === page + 1) {
                return prevState;
            }

            return {
                ...prevState,
                currentPage: page + 1,
                loading: true,
            };
        });
    }

    const toggleModalPostArchive = (state) => {
        setState(prevState => ({
            ...prevState,
            showModalArchivePost: false,
            openConfirm: false
        }));
        
        if (state.deletedPostId) {
            setDeletedPostId(state.deletedPostId); // Set the deleted post Id to trigger a re-render only when user press yes
        }
    }

    const saveForumDetails = () => {
        setState(prevState => ({
            ...prevState,
            loading: true,
            editing: false,
        }));

        // Create forum if it doesn't exist
        if (state.forumId === null) {
            createForum(state.teacherPlanId, state.forumTitle, state.forumDescription)
                .then((response) => {
                    setState(prevState => ({
                        ...prevState,
                        loading: false,
                        forumId: response.data.ForumModel.Id,
                    }));
                    updateParent();
                });
        } else {
            // Update forum if it exists
            updateForum(state.forumId, state.forumTitle, state.forumDescription)
                .then(() => {
                    setForumDetails(prev => ({
                        ...prev,
                        Title: state.forumTitle,
                        Description: state.forumDescription,
                    }));
                    setState(prevState => ({
                        ...prevState,
                        loading: false,
                    }));
                });
        }
    };

    const fetchResponses = async (postId, page) => {
        try {
          const res = await getTeacherForumsDetails(state.forumId, page, state.pageSize, postId); 
          return res.data;
        } catch (error) {
          throw error;
        }
      }

    if (state.redirectToPlanning) {
        return (<Redirect to={{
            pathname: "/planning-detail"
        }} />);
    }

    if (!state.isCreation && !state.loadedData) {
        return <div />;
    }

    return ( 
        <div>
            {state.isCreation ?
                <Row>
                    <Col xs={12} className="text-adjust-forumInstructions">
                        <Translate id="forum.forum-creation-instructions" options={{ renderInnerHtml: true }} />
                    </Col>
                </Row> : null
            }
            {state.planningName !== '' && forumDetails.CreatedAt !== "" && state.isCreation === false ? (
                <div className="wrapper-col mb-2">
                        <div className="forum-instructional-plan">
                            <div className="col-full-size forum-create-date">
                                <b><Translate id="forum.created-date" options={{ renderInnerHtml: true }} /></b>:{" "} 
                                {moment.utc(forumDetails.CreatedAt, "M/DD/YYYY H:mm:ss a")
                                    .tz(getTimeZone())
                                    .format("MM/DD/YYYY hh:mm a")}
                            </div>
                            <small>
                                <b>
                                    <Translate 
                                    id="forum.instructional-plan" 
                                    options={{ renderInnerHtml: true }} 
                                    />: {state.planningName}
                                </b>
                                <br />
                                <b>
                                    <Translate 
                                    id="forum.student-groups" 
                                    options={{ renderInnerHtml: true }} 
                                    />:{" "}
                                    {state.studentsGroups.length !== 0 ? (
                                    state.studentsGroups.map((item) => item.GroupName).join(', ')
                                    ) : (
                                    <Translate 
                                        id="forum.no-groups-found" 
                                        options={{ renderInnerHtml: true }} 
                                    />
                                    )}
                                </b>
                            </small>
                        </div>
                </div>) 
            : null}
            {!state.editing && state.isCreation === false ?
                <>
                    <div className='wrapper-forum-title'>
                        <div className="wrapper-col">
                            <span className="title-content__title">{forumDetails.Title}</span>
                        </div>      
                        <FontAwesomeIcon 
                        icon={faEdit}
                        onClick={toggleIsEditing} 
                        style={{
                            fontSize: '20px', 
                            color: '#38646c',
                            cursor: 'pointer',
                            marginBottom: '5px',
                            }}/>
                    </div>
                    <div  className="wrapper-col title-content__desc" dangerouslySetInnerHTML={{__html: forumDetails.Description}}/>
                </> 
                : 
                <>
                    <div className="edit-content__title__inputGroup">
                        <Label for="forum-title">
                            <Translate id="forum.enter-forum-title" options={{renderInnerHtml: true}}/>
                            <button 
                                disabled={state.showError || state.forumTitle === null || state.forumTitle.trim() === '' 
                                    || state.forumDescription === null || state.forumDescription.trim() === ''} 
                                className="title-w-edit__btn"
                                onClick={() => saveForumDetails()}>
                            <i className="fas fa-save"/>
                            </button>

                            <button
                                className="title-w-edit__btn"
                                onClick={() => cancelForumEdit()}>
                                <i className="fas fa-times"/>
                            </button>
                            {state.showError ?
                                <Alert color="info">
                                    <Translate id="forum.error-length-post"
                                                options={{ renderInnerHtml: true }} 
                                    />
                                    {' '} 
                                    {Settings.FORUM_DESCRIPTION_MAX_LENGTH}
                                </Alert> 
                            : null}
                        </Label>
                        <Input onChange={changeForumTitle} value={state.forumTitle} type={"text"} id={"text"}/>
                    </div>
                    <div className="edit-content__title__inputGroup">
                        <Label for="forum-description">
                            <Translate id="forum.enter-forum-description" options={{renderInnerHtml: true}}/>
                        </Label>
                        <Editor 
                                modules={Settings.FORUM_MODULES}
                                onKeyDown={checkCharacterCount}
                                ref={editorRef}
                                value={state.forumDescription}
                                onChange={changeForumDescription}
                        />
                    </div>
                </>
            }
            {/* Show forum details if forum creation is false */}
            {state.isCreation === false &&  
            <div className='wrapper-add-post'>
                <div className="foroHeader--TotalAnswer">
                        {<Translate id="forum.answers"options={{renderInnerHtml: true}}/>}{` (${state.totalPosts})`}
                </div>
                <div className="content-forum">
                    <div className="wrapper-foroHeader">
                        
                        <div className="foroHeader--name">
                            {SessionManager.getSessionDetails().Name}
                        </div>
                        {state.showPost ? (
                            <InputCreatePost postContent={state.postContent} postContentChange={handlePostContentChange} publishPost={addPost} hidePost={hidePost} />
                        ) : (
                            <button className="foroHeader--btnAddComment" onClick={handleShowPost}>
                                <Translate id="forum.add-comment" options={{renderInnerHtml: true}}/>
                            </button>
                        )}
                    </div>
                    <ParentPosts 
                        key={state.refreshKey} 
                        posts={state.posts} 
                        handleArchiveClick={handleOpenConfirmDialog}
                        fetchResponses={fetchResponses}
                        setVisibleChildren={setVisibleChildren}
                        visibleChildren={visibleChildren}
                        deletedPostId={deletedPostId}
                        />
                        
                    {!state.posts.length && !state.loading ? (
                        <EmptyForumStyled>
                            <i className="fas fa-book-reader"></i> {' '} <Translate id="forum.empty-forum"
                                                                            options={{renderInnerHtml: true}}/>
                        </EmptyForumStyled>
                    ): null}

                    {/* Show pagination */}
                    {state.totalPages > 1 && (
                        <PaginationCustom 
                            totalPages={state.totalPages}
                            currentPage={state.currentPage - 1} 
                            updateParent={handleChangePaginate} 
                            dapper={true} 
                        />
                    )}
                </div>
            </div>
            }
            
            {state.showModalArchivePost ? 
            <ModalForumPostArchive 
                visible={state.showModalArchivePost}
                forumId={state.forumId} 
                postId={state.itemSelectedToArchive.Id}
                updateParent={toggleModalPostArchive}
                /> 
            : null}
            {state.loading && <LoadingFullWindow/>}
        </div>
                )
};

export default withLocalize(ForumCreationAndView);