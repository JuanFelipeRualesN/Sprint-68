import React, {useCallback, useEffect, useState, useRef, useContext} from "react";
import styled from "styled-components";
import {useHistory, useLocation} from "react-router-dom";
import LanguageManager from "../../../../custom-classes/language-manager";
import moment from "moment";
import LoadingFullWindow from "../../../shared-module-components/loader-windows/loader-full-window";
import ReactQuill from "react-quill";
import "../../../../styles/css/forums-details.css";
import {Label, Input, Alert} from "reactstrap";
import {createPost, deletePost, getForumDetails, updatePost, updateForum} from "../../../../api/forum";
import {Translate} from "react-localize-redux";
import ConfirmDialog from "../confirm-dialog";
import PaginationCustom from "../../../shared-module-components/datatables/pagination";
import SessionManager from "../../../../custom-classes/session-manager";
import "./style.css";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faCalendar} from "@fortawesome/free-solid-svg-icons";
import Settings from "../../../../custom-classes/settings";
import { BreadcrumbContext } from "../../planning/shared/providers/breadcrumb";
let language = LanguageManager.getLanguage();

const getTimeZone = () => {
    return Intl.DateTimeFormat().resolvedOptions().timeZone;
};

const convertToUserTimeZone = (dateString) => {
    // Parse the input date string assuming it's in UTC
    const utcDate = new Date(dateString + ' UTC');
    // Get the user's timezone
    const userTimeZone = getTimeZone();
    // Convert the UTC date to the user's local date and time
    return new Date(utcDate.toLocaleString('en-US', {timeZone: userTimeZone}));
};
const formatDate = (date, locale) => {
    let newDate = convertToUserTimeZone(date);
    const options = {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: 'numeric',
        minute: '2-digit',
        hour12: true,
        timeZone: getTimeZone() // Set the user's timezone
    };
    return new Intl.DateTimeFormat(locale, options).format(new Date(newDate));
};

const ParentPosts = ({ posts, handleArchiveClick, fetchResponses, refreshPosts  }) => {
    const [responseState, setResponseState] = useState({});
    const [data, setData] = useState([]);
    const [editContent, setEditContent] = useState('');
    const [visibleChildren, setVisibleChildren] = useState({});
    const [loading, setLoading] = useState(false);
    const [paginationState, setPaginationState] = useState({});
    const [loadedResponses, setLoadedResponses] = useState({});


    useEffect(() => {
        setData(posts);
    }, [posts]);

    const toggleResponse = (post, isEdit) => {
        if (isEdit) {
            setEditContent(post.ForumPostModel.Body);
        } else {
            setEditContent('');
        }

        setResponseState((prevState) => ({
            ...prevState,
            [post.ForumPostModel.Id]: !prevState[post.ForumPostModel.Id] || false,
        }));
    };

    const publishPost = async (forumId, postContent, parentPostId) => {
        const req = {
            ForumId: forumId,
            Body: postContent,
            ParentPostId: parentPostId,
        };
        setLoading(true);
        try {
            const response = await createPost(req);
            if (response.data.ForumPostModel) {
                // If parentPostId is not null, find the parent post in the data array
                if (parentPostId) {
                    const newData = data.map(item => {
                        // Recursive function to find and update parent post
                        const updateParent = post => {
                            // If the current post's id matches the parentPostId
                            if (post.ForumPostModel && post.ForumPostModel.PostId === parentPostId) {
                                post.ForumPostModel.ChildPostCount += 1; // Increment child post count
                                // Add the newly published post to responses
                                post.ForumPostModel.responses.unshift(response.data);
                                return true; // Indicate that parent post is found and updated
                            }
                            // If the current post has responses, recursively search in responses
                            if (post.ForumPostModel && post.responses) {
                                for (const response of post.responses) {
                                    if (updateParent(response)) return true; // If parent is found and updated, stop searching
                                }
                            }
                            return false; // Parent post not found
                        };

                        // Call recursive function for each item in data array
                        updateParent(item);
                        return item; // Return updated item
                    });
                    setData(newData); // Update data array with updated parent post
                } else {
                    // If parentPostId is null, simply prepend the newly published post to data
                    setData([response.data, ...data]);
                }
            }
            setLoading(false);
            refreshPosts();

        } catch {
            setLoading(false);
        }
    };

    const editPost = async (forumId, postContent, postId) => {
        let req = {
            ForumId: forumId,
            Body: postContent,
            Id: postId,
        };
        setLoading(true);
        try {
            const response = await updatePost(req);
            if (response.data) {
                // Recursive function to find and update post by postId
                const updatePostById = post => {
                    if (post.ForumPostModel && post.ForumPostModel.Id === postId) {
                        // Update the post content
                        post.ForumPostModel.Body = postContent;
                        return true; // Indicate that post is found and updated
                    }

                    // If the current post has responses, recursively search in responses
                    if (post.ForumPostModel && post.responses) {
                        for (const response of post.responses) {
                            if (updatePostById(response)) return true; // If post is found and updated, stop searching
                        }
                    }

                    return false; // Post not found
                };

                // Update data array with the edited post
                const newData = data.map(item => {
                    updatePostById(item);
                    return item; // Return updated item
                });
                setData(newData);
            }
            setLoading(false);
        } catch {
            setLoading(false);
        }
    };

    const renderReplyButtonOrInput = (post) => {
        if (responseState[post.ForumPostModel.Id]) {
            let method = editContent ? editPost : publishPost;
            return (
                <InputChildrenPost
                    publishPost={(postContent) => method(post.ForumPostModel.ForumId, postContent, post.ForumPostModel.Id)}
                    hidePost={() => toggleResponse(post)}
                    initialContent={editContent}
                />
            );
        } else {
            return (
                <button className="btn-reply" onClick={() => toggleResponse(post)}>
                    <i className="fas fa-comment"></i>{' '}
                    <Translate id="forum.reply"
                               options={{renderInnerHtml: true}}/>
                </button>
            );
        }
    };

    const renderHeaderActions = (post) => {
        return (
            <div className="foroContent--header__actions">
                <button onClick={() => handleArchiveClick(post.ForumPostModel)}><i className="fas fa-trash-alt"></i>
                </button>
                {SessionManager.getSessionDetails().Username === post.ForumPostModel.AuthorUsername && (
                    <>
                        <button onClick={() => toggleResponse(post, true)}><i className="fas fa-edit"></i></button>
                    </>
                )}
            </div>
        );
    };

    const toggleShowAnswers = async (postId) => {
        if (!visibleChildren[postId] && !loadedResponses[postId]) {
            await fetchMoreResponses(postId, 1);
            setLoadedResponses((prev) => ({ ...prev, [postId]: true }));
        }

        setVisibleChildren((prevState) => ({
            ...prevState,
            [postId]: !prevState[postId] || false,
        }));
    };

    const fetchMoreResponses = async (postId, page) => {
        setLoading(true);
        try {
            let data = await fetchResponses(postId, page);
            const { ForumPostModels: responses, TotalPages } = data;

            setPaginationState((prevState) => ({
                ...prevState,
                [postId]: { page, totalPages: TotalPages },
            }));

            // Recursively update responses for children and grandchildren
            const updateResponsesRecursively = (posts) => {
                return posts.map((post) => {
                    if (post.ForumPostModel.Id === postId) {
                        return {
                            ...post,
                            responses: post.responses ? [...post.responses, ...responses] : responses,
                        };
                    } else if (post.responses) {
                        return {
                            ...post,
                            responses: updateResponsesRecursively(post.responses),
                        };
                    } else {
                        return post;
                    }
                });
            };

            setData((prevData) => updateResponsesRecursively(prevData));

            setLoading(false);
        } catch {
            setLoading(false);
        }
    };

    const findPostById = (postId, posts) => {
        for (const post of posts) {
            if (post.ForumPostModel.Id === postId) {
                return post;
            }
            if (post.responses) {
                const foundPost = findPostById(postId, post.responses);
                if (foundPost) {
                    return foundPost;
                }
            }
        }
        return null;
    };

    const renderChildren = (parentId) => {
        const parentPost = findPostById(parentId, data);

        if (!parentPost) {
            return null; // Return null if parent post is not found
        }

        const children = parentPost.responses || [];
        const pagination = paginationState[parentId];

        return (
            visibleChildren[parentId] && (
                <>
                    {children.map((child) => (
                        <div key={child.ForumPostModel.Id} className="wrapper-foroContent--answers">
                            <div className="foroContent--answersContent">
                                <div className="foroContent--answers__user">
                                    <p>{child.AuthorName || child.ForumPostModel.AuthorName}</p>
                                    <p className="header__user--date">
                                        {child.ForumPostModel.CreatedAt ? formatDate(child.ForumPostModel.CreatedAt, language) : ''}
                                    </p>
                                </div>
                                {renderHeaderActions(child)}
                                <div className="foroContent--body_response"
                                     dangerouslySetInnerHTML={{__html: child.ForumPostModel.Body}}/>
                                {renderReplyButtonOrInput(child)}
                                {child.ForumPostModel.ChildPostCount > 0 && (
                                    <button className="btn-moreAnswers" onClick={() => toggleShowAnswers(child.ForumPostModel.Id)}>
                                        {visibleChildren[child.ForumPostModel.Id] ?
                                            <Translate id="forum.hide-more-answers"
                                                       options={{renderInnerHtml: true}}/>:
                                            <>
                                                <Translate id="forum.show-more-answers"
                                                           options={{renderInnerHtml: true}}/> {' '}
                                                {` (${child.ForumPostModel.ChildPostCount})`}
                                            </>
                                        }
                                    </button>
                                )}
                            </div>
                            {renderChildren(child.ForumPostModel.Id)} {/* Recursively render children */}
                        </div>
                    ))}
                    {pagination && pagination.page < pagination.totalPages && (
                        <button
                            className="btn-moreAnswers"
                            onClick={() => fetchMoreResponses(parentId, pagination.page + 1)}
                        >
                            <Translate id="forum.show-more-answers"
                                       options={{renderInnerHtml: true}}/>
                        </button>
                    )}
                </>
            )
        );
    };

    const topLevelPosts = data.filter((post) => post.ForumPostModel.ParentPostId === null);

    return (
        loading ? <LoadingFullWindow /> :
            topLevelPosts.map((post) => (
                <div key={post.ForumPostModel.Id} className="wrapper-foroContent" style={{ marginBottom: '25px' }}>
                    <div className="foroContent--header">
                        <div className="foroContent--header__user">
                            <p>{post.AuthorName}</p>
                            <p className="header__user--date">
                                {post.ForumPostModel.CreatedAt ? formatDate(post.ForumPostModel.CreatedAt, language) : ''}
                            </p>
                        </div>
                        {renderHeaderActions(post)}
                    </div>
                    <div className="foroContent--body">
                        <div className="foroContent--body_response" dangerouslySetInnerHTML={{ __html: post.ForumPostModel.Body }} />
                        {renderChildren(post.ForumPostModel.Id)}
                        {renderReplyButtonOrInput(post)}
                        {post.ForumPostModel.ChildPostCount > 0 && (
                            <button className="btn-moreAnswers" onClick={() => toggleShowAnswers(post.ForumPostModel.Id)}>
                                {visibleChildren[post.ForumPostModel.Id] ? <Translate id="forum.hide-more-answers"
                                                                                      options={{renderInnerHtml: true}}/> :
                                    <Translate id="forum.show-more-answers"  options={{renderInnerHtml: true}}/>}
                                {` (${post.ForumPostModel.ChildPostCount})`}
                            </button>
                        )}
                    </div>
                </div>
            ))
    );
};

const InputChildrenPost = ({ publishPost, hidePost, initialContent }) => {
    const [postContent, setPostContent] = useState(initialContent || '');
    const quillRef = useRef();
    const [showError, setShowError] = useState(false);

    useEffect(() => {
        setPostContent(initialContent || '');
    }, [initialContent]);

    const handleChange = (value) => {
        setPostContent(value);
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

    const checkCharacterCount = (event) => {
        const unprivilegedEditor = quillRef.current.unprivilegedEditor;
        const contentLength = unprivilegedEditor.getLength();
        if (contentLength > Settings.FORUM_MAX_CHARACTERS && event.key !== 'Backspace') {
            event.preventDefault();
        }
    };

    const handlePublishPost = () => {
        if(!showError)
        {
            publishPost(postContent);
            setPostContent('');
            hidePost();
        }
    };

    return (
        <div className="foroHeader--formGroup">
            <ReactQuill theme='snow'
                        modules={Settings.FORUM_MODULES}
                        onKeyDown={checkCharacterCount}
                        ref={quillRef}
                        value={postContent}
                        onChange={handleChange}
                         />
            <div className="message-content-buttons-mail">
                {showError ?
                    <Alert color="info">
                        <Translate id="forum.error-length-post" options={{ renderInnerHtml: true }} />
                        {' '} {Settings.FORUM_MAX_CHARACTERS}
                    </Alert> : null}
                <button disabled={showError} type="submit" className="btn-reply-submit" onClick={handlePublishPost}>
                    {postContent ? (
                        <Translate id="forum.save" options={{ renderInnerHtml: true }} />
                    ) : (
                        <Translate id="forum.reply" options={{ renderInnerHtml: true }} />
                    )}
                </button>
                <button type="submit" className="btn-reply-cancel" onClick={hidePost}>
                    <Translate id="forum.cancel" options={{ renderInnerHtml: true }} />
                </button>
            </div>
        </div>
    );
};

const InputParentPost = ({ postContent, postContentChange, publishPost, hidePost }) => {
    const quillRef = useRef();
    const [showError, setShowError] = useState(false);

    const handleContentChange = (content) => {
        postContentChange(content);
        const unprivilegedEditor = quillRef.current.unprivilegedEditor;
        const contentLength = unprivilegedEditor.getLength();
        if (contentLength > Settings.FORUM_MAX_CHARACTERS)
        {
            setShowError(true);
        }
        else {
            setShowError(false);
        }
    };

    const checkCharacterCount = (event) => {
        const unprivilegedEditor = quillRef.current.unprivilegedEditor;
        const contentLength = unprivilegedEditor.getLength();
        if (contentLength > Settings.FORUM_MAX_CHARACTERS && event.key !== 'Backspace') {
            event.preventDefault();
        }
    };

    return (
        <div className="foroHeader--formGroup">
            <ReactQuill
                ref={quillRef}
                modules={Settings.FORUM_MODULES}
                theme="snow"
                value={postContent}
                onChange={handleContentChange}
                onKeyDown={checkCharacterCount}
            />
            <div className="message-content-buttons-mail">
                {showError ?  <Alert color="info">
                    <Translate id="forum.error-length-post" options={{ renderInnerHtml: true }} />
                    {' '} {Settings.FORUM_MAX_CHARACTERS}
                </Alert> : null}
                <button disabled={showError} type="submit" className="btn-reply-submit" onClick={publishPost}>
                    <Translate id="forum.reply" options={{ renderInnerHtml: true }} />
                </button>
                <button type="submit" className="btn-reply-cancel" onClick={hidePost}>
                    <Translate id="forum.cancel" options={{ renderInnerHtml: true }} />
                </button>
            </div>
        </div>
    );
};

const EmptyListStyled = styled.p`
    color: #ccc;
    display: block;
    justify-content: center;
    text-align: center;
    margin: 3em auto;

    & img {
        display: block;
        margin: 1em auto 0.25em auto;
        font-size: 45px;
        width: 175px;
        opacity: 0.5;
    }

    & i {
        display: block;
        margin: 1em auto 0.25em auto;
        font-size: 45px;
        width: 175px;
        opacity: 0.5;
    }
`;


const ForumDetail = ({forumId, isEditable = true}) => {
    const history = useHistory();
    const [page, setPage] = useState(1);
    const [pageSize] = useState(10);
    const [count, setCount] = useState(0);
    const [forum, setForum] = useState({});
    const [posts, setPosts] = useState([]);
    const [loading, setLoading] = useState(false);
    const [openConfirm, setOpenConfirm] = useState(false);
    const [itemSelected, setItemSelected] = useState({});
    const [description, setDescription] = useState('');
    const [showPost, setShowPost] = useState(false);
    const [totalPosts, setTotalPosts] = useState(0);
    const [refreshKey, setRefreshKey] = useState(0);
    const [groupNames, setGroupNames] = useState('');
    const [isEditingMode, setIsEditingMode] = React.useState(false);
    const [originalTitle, setOriginalTitle] = React.useState('');
    const [showError, setShowError] = useState(false);
    const [originalDescription, setOriginalDescription] = React.useState('');
    const editorRef = useRef();
    moment.locale(LanguageManager.getLanguage());

    const { setPath } = useContext(BreadcrumbContext);
    const query = new URLSearchParams(useLocation().search);
    const isPlanningV2 = forumId === undefined || forumId === null;
    forumId ??= query.get("forumId");

    const getForumDetailById = useCallback(async () => {
        await getForumDetailPaginate(forumId, page, pageSize);
    }, [forumId, page, pageSize, refreshKey]);

    useEffect(() => {
        getForumDetailById();
    }, [getForumDetailById]);

    
    useEffect(() => {
        if (!isPlanningV2 || !forum.Title)
            return;

        setPath({ name: forum.Title });
    }, [forum]);


    const getForumDetailPaginate = async (forumId, page, pageSize) => {
        setLoading(true);
        try {
            const res = await getForumDetails(forumId, page, pageSize); // Assuming getForumDetails is an imported function
            const { ForumModel, ForumPostModels, TotalPages, AuthorName, TotalItems } = res.data;
            setForum(ForumModel);
            setOriginalTitle(ForumModel.Title);
            setOriginalDescription(ForumModel.Description);
            setTotalPosts(TotalItems);
            setPosts(ForumPostModels);
            setCount(TotalPages);
            if (Array.isArray(res.data.TeacherGroupModels) && res.data.TeacherGroupModels.length > 0) {
                setGroupNames(res.data.TeacherGroupModels.map(group => group.Name).join(', '));
            } else {
                setGroupNames(''); // Clear group names if there are no groups
            }
            setLoading(false);
        } catch {
            setLoading(false);
        }
    };

    const handleOpenConfirmDialog = (item) => {
        setItemSelected(item);
        setOpenConfirm(true);
    };

    const handleCloseConfirmDialog = () => {
        setOpenConfirm(false);
    };

    const handleDeleteItem = async () => {
        setLoading(true);
        try {
            await deletePost(forumId, itemSelected.Id); // Assuming deletePost is an imported function
            setLoading(false);
            setDescription('');
            setRefreshKey((oldKey) => oldKey + 1); // Trigger a re-render
            handleCloseConfirmDialog();
        } catch {
            setLoading(false);
        }
    };

    const handleChangePaginate = (event) => {
        let pageToFind = event + 1;
        setPage(pageToFind);
        getForumDetailPaginate(forumId, pageToFind, pageSize);
        setRefreshKey((oldKey) => oldKey + 1); // Trigger a re-render
    };

    const handleShowPost = () => {
        setShowPost(true);
    };

    const hidePost = () => {
        setDescription('');
        setShowPost(false);
    };

    const handleDescriptionChange = (content) => {
        setDescription(content);
    };

    const addPost = async () => {
        const req = {
            ForumId: forumId,
            Body: description,
        };
        setLoading(true);
        try {
            await createPost(req); // Assuming createPost is an imported function
            setShowPost(false);
            setRefreshKey((oldKey) => oldKey + 1); // Trigger a re-render
            setLoading(false);
            setDescription('');
        } catch {
            setLoading(false);
        }
    };

    const refreshPosts = async () => {
        getForumDetailPaginate(forumId, page, pageSize);
        setRefreshKey((oldKey) => oldKey + 1); // Trigger a re-render
    };

    const fetchResponses = async (postId, page) => {
        try {
            const res = await getForumDetails(forumId, page, pageSize, postId); // Pass postId to fetch child responses
            return res.data;
        } catch (error) {
            throw error;
        }
    };


    const changeForumDescription = (content) => {

        setForum((prev) => ({
            ...prev,
            Description: content,
        }));
        const unprivilegedEditor = editorRef.current.unprivilegedEditor;
        const contentLength = unprivilegedEditor.getLength();
        if (contentLength > Settings.FORUM_DESCRIPTION_MAX_LENGTH)
        {
            setShowError(true);
        }
        else
        {
            setShowError(false);
        }
    };

    const changeForumTitle = (event) => {
        let text = event.target.value;
        setForum((prev) => ({
            ...prev,
            Title: text,
        }));
    }

    const saveForumDetails = async () => {
        setLoading(true);
        try {
            await updateForum(forumId, forum.Title, forum.Description); // Assuming updateForum is an imported function
            setLoading(false);
            setIsEditingMode(false);
            setOriginalTitle(forum.Title);
            setOriginalDescription(forum.Description);
        } catch {
            setLoading(false);
        }
    }

    const checkCharacterCount = (event) => {
        const unprivilegedEditor = editorRef.current.unprivilegedEditor;
        const contentLength = unprivilegedEditor.getLength();
        if (contentLength > Settings.FORUM_DESCRIPTION_MAX_LENGTH && event.key !== 'Backspace') {
            event.preventDefault();
        }
    };

    const cancelForumEdit = () => {
        setForum((prev) => ({
            ...prev,
            Title: originalTitle,
            Description: originalDescription,
        }));
        setIsEditingMode(false);
    }

    return (
        <>
            <div className="wrapper-forum">
                <div className="forum-description-detail">
                    {!isPlanningV2 && (
                        <>
                            <p className="forum-description-detail__txt-secondary">
                                <small>
                                    <FontAwesomeIcon icon={faCalendar} />{' '} <b>
                                        <Translate id="forum.create-at" options={{ renderInnerHtml: true }} />:</b>
                                    {' '} {forum.CreatedAt ? formatDate(forum.CreatedAt, language) : ''}
                                </small>
                                <br />
                                <small><b>
                                    <Translate id="forum.instructional-plan" options={{ renderInnerHtml: true }} />:
                                    {' '}</b> {forum.PlanName} {' '} / {' '}
                                    <b><Translate id="forum.student-groups" options={{ renderInnerHtml: true }} />:</b>{' '}
                                    {groupNames}
                                </small>
                            </p>
                            <hr />
                        </>
                    )}
                    {!isEditingMode ?
                        <>
                            <span className="title-content__title">{forum.Title}</span>
                            {isEditable && (
                                <button
                                    className="title-w-edit__btn"
                                    onClick={() => setIsEditingMode(true)}>
                                    <i className="fas fa-edit"></i>
                                </button>
                            )}
                            <p className="title-content__desc"
                               dangerouslySetInnerHTML={{__html: forum.Description}}></p>
                        </>
                        :
                        <>
                            <div className="edit-content__title__inputGroup">
                                <Label for="forum-title"><Translate id="forum.enter-forum-title"
                                                                    options={{renderInnerHtml: true}}/>
                                    <button disabled={showError || forum.Title === null || forum.Title.trim() === ''} className="title-w-edit__btn"
                                    onClick={() => saveForumDetails()}>
                                <i className="fas fa-save"></i>
                            </button>
                                <button
                                        className="title-w-edit__btn"
                                        onClick={() => cancelForumEdit()}>
                                    <i className="fas fa-times"></i>
                                </button>
                                    {showError ?
                                    <Alert color="info">
                                        <Translate id="forum.error-length-post"
                                                   options={{ renderInnerHtml: true }} />
                                        {' '} {Settings.FORUM_DESCRIPTION_MAX_LENGTH}
                                    </Alert> : null}
                        </Label>
                        <Input onChange={changeForumTitle} value={forum.Title} type="text" id="text" maxLength={Settings.FORUM_TITLE_MAX_LENGTH}/>
                    </div>
                    <div className="edit-content__title__inputGroup">
                    <Label for="forum-description">
                        <Translate id="forum.enter-forum-description" options={{renderInnerHtml: true}}/></Label>
                        <ReactQuill theme='snow'
                                    modules={Settings.FORUM_MODULES}
                                    onKeyDown={checkCharacterCount}
                                    ref={editorRef}
                                    value={forum.Description}
                                    onChange={changeForumDescription}
                        />
                    </div>
                        </>}
                </div>

                <div className="content-forum">
                <div className="wrapper-foroHeader">
                        <div className="foroHeader--TotalAnswer">{<Translate id="forum.answers"
                                                                             options={{renderInnerHtml: true}}/>}{` (${totalPosts})`}</div>
                        <div className="foroHeader--name">{SessionManager.getSessionDetails().Name}</div>
                        {showPost ? (
                            <InputParentPost postContent={description} postContentChange={handleDescriptionChange} publishPost={addPost} hidePost={hidePost} />
                        ) : (
                            <button className="foroHeader--btnAddComment" onClick={handleShowPost}>
                                <Translate id="forum.add-comment" options={{renderInnerHtml: true}}/>
                            </button>
                        )}
                    </div>
                    <ParentPosts key={refreshKey} posts={posts} handleArchiveClick={handleOpenConfirmDialog} fetchResponses={fetchResponses} refreshPosts={refreshPosts}/>
                    {!posts.length && !loading && (
                        <EmptyListStyled>
                            <i className="fas fa-book-reader"></i> {' '} <Translate id="forum.empty-forum"
                                                                              options={{renderInnerHtml: true}}/>
                        </EmptyListStyled>
                    )}
                    {count > 1 && (
                        <PaginationCustom totalPages={count}
                                          currentPage={page - 1} updateParent={handleChangePaginate} dapper={true} />
                    )}
                </div>
            </div>
            {openConfirm && (
                <ConfirmDialog open={openConfirm} onClose={handleCloseConfirmDialog} handleDeleteItem={handleDeleteItem} />
            )}
            {loading && <LoadingFullWindow />}

        </>
    );
};

export default ForumDetail;