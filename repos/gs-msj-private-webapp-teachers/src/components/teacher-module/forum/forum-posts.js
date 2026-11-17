import React, { useEffect, useState, useRef, useCallback} from "react";
import LanguageManager from "../../../custom-classes/language-manager";
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import ReactQuill from "react-quill";
import {Alert} from "reactstrap";
import {createPost, updatePost} from "../../../api/teachers";
import {Translate} from "react-localize-redux";
import { scrollPreservation } from "../../../utils/scrollPreservation"

import SessionManager from "../../../custom-classes/session-manager";
import "./style.css";
import Settings from "../../../custom-classes/settings";
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

//  A memoized component for creating/editing children posts (replies)
const InputChildrenPost = React.memo(({ publishPost, hidePost, initialContent }) => {
    const [postContent, setPostContent] = useState(initialContent || '');
    const quillRef = useRef();
    const [showError, setShowError] = useState(false);

    useEffect(() => {
        setPostContent(initialContent || '');
    }, [initialContent]);

    const handleChange = (content) => {
        if (!quillRef.current?.unprivilegedEditor) return;
        setPostContent(content);
        const contentLength = content.length;
        setShowError(contentLength > Settings.FORUM_MAX_CHARACTERS);
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
}, 
// Only re-render if the initial content changes
(prevProps, nextProps) => {
    return prevProps.initialContent === nextProps.initialContent;
});

// Component to render parent posts and their children (replies)
const ParentPosts = ({ posts, handleArchiveClick, fetchResponses, visibleChildren, setVisibleChildren, deletedPostId }) => {
    const [responseState, setResponseState] = useState({});
    const [data, setData] = useState([]);
    const [editContent, setEditContent] = useState('');
    const [loading, setLoading] = useState(false);
    const [paginationState, setPaginationState] = useState({});
    const [isPosting, setIsPosting] = useState(false);

    useEffect(() => {
        setData(posts);
    }, [posts]);
    
    // Render posts after a post is deleted without losing visibility of children
    useEffect(() => {
        if (deletedPostId) {
            setData(prevData => {
                //Recursively update posts
                const updatePostsAfterDeletion = (posts) => {
                    const filteredPosts = posts.filter(post => 
                        post.ForumPostModel.Id !== deletedPostId
                    );
                    //Map through posts to handle their responses
                    return filteredPosts.map(post => {
                        if (post.responses) {
                            return {
                                ...post,
                                ForumPostModel: {
                                    ...post.ForumPostModel,
                                    ChildPostCount: post.responses.filter(res => 
                                        res.ForumPostModel.Id !== deletedPostId
                                    ).length
                                },
                                // Recursively handle responses
                                responses: updatePostsAfterDeletion(post.responses)
                            };
                        }
                        return post;
                    });
                };
    
                return updatePostsAfterDeletion(prevData);
            });
        }
    }, [deletedPostId]);

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

    const handleHidePost = useCallback((post) => {
        return () => toggleResponse(post);
    }, [toggleResponse]);
    
    // Reply to a post
    const replyToPost = async (forumId, parentPostId, postContent) => {
        await scrollPreservation(async () => {
            if (isPosting) return;
            setLoading(true);
            setIsPosting(true);
            try {
                const response = await createPost({
                    ForumId: forumId,   
                    Body: postContent,
                    ParentPostId: parentPostId,
                });        
                
                if (response.data.ForumPostModel) {
                    if (visibleChildren[parentPostId]) {
                        // If children are visible, fetch the updated list of responses
                        const responseData = await fetchResponses(parentPostId, 1);
                        
                        // Update both the child count and responses
                        setData(prevData => {
                            return prevData.map(item => {
                                const updatePost = (post) => {
                                    if (post.ForumPostModel.Id === parentPostId) {
                                        return {
                                            ...post,
                                            responses: responseData.ForumPostModels,
                                            ForumPostModel: {
                                                ...post.ForumPostModel,
                                                ChildPostCount: responseData.TotalItems
                                            }
                                        };
                                    }
                                    if (post.responses) {
                                        return {
                                            ...post,
                                            responses: post.responses.map(updatePost)
                                        };
                                    }
                                    return post;
                                };
                                return updatePost(item);
                            });
                        });
    
                        // Update pagination state
                        setPaginationState(prevState => ({
                            ...prevState,
                            [parentPostId]: { 
                                page: 1, 
                                totalPages: responseData.TotalPages,
                            }
                        }));
                    } else {
                        // If children aren't visible update the count
                        setData(prevData => {
                            return prevData.map(item => {
                                const updatePost = (post) => {
                                    if (post.ForumPostModel.Id === parentPostId) {
                                        return {
                                            ...post,
                                            ForumPostModel: {
                                                ...post.ForumPostModel,
                                                ChildPostCount: (post.ForumPostModel.ChildPostCount || 0) + 1
                                            }
                                        };
                                    }
                                    if (post.responses) {
                                        return {
                                            ...post,
                                            responses: post.responses.map(updatePost)
                                        };
                                    }
                                    return post;
                                };
                                return updatePost(item);
                            });
                        });
                    }
                }
            } catch (error) {
                console.error('Error replying to post:', error);
            } finally {
                setLoading(false);
                setIsPosting(false);
            }
        });
    };
    
    const editPost = async (forumId, postId, postContent) => {
        await scrollPreservation(async () => {
            setLoading(true);
            try {
            const response = await updatePost({
                ForumId: forumId,
                Body: postContent,
                Id: postId,
            });
            if (response.data) {
                // Recursive function to find and update post by postId
                const updatePostById = post => {
                    if (post.ForumPostModel && post.ForumPostModel.Id === postId) {
                        // Update the post content
                        post.ForumPostModel.Body = postContent;
                        return true;
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
            } catch (error) {
                console.error('Error editing post:', error);
            } finally {
                setLoading(false);
            }
        });
    };
    // Handle logic for replying to or editing a post
    const handleReplyOrEditPost = useCallback((post, editContent) => {
        return (postContent) => {
            if (!editContent) {
                return replyToPost(post.ForumPostModel.ForumId, post.ForumPostModel.Id, postContent);
            } else {
                return editPost(post.ForumPostModel.ForumId, post.ForumPostModel.Id, postContent);
            }
        };
    }, [replyToPost, editPost]);

    const fetchMoreResponses = async (postId, page) => {
        scrollPreservation(async () => {
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
                            // If it's the first page or responses don't exist, use the new responses
                            if (page === 1 || !post.responses) {
                                return {
                                    ...post,
                                    responses: responses
                                };
                            } 
                            // If loading more pages, append new responses
                            return {
                                ...post,
                                responses: [...post.responses, ...responses]
                            };
                        } else if (post.responses) {
                            return {
                                ...post,
                                responses: updateResponsesRecursively(post.responses)
                            };
                        }
                        return post;
                    });
                };
                setData((prevData) => updateResponsesRecursively(prevData));
            } catch (error) {
                console.error('Error fetching more responses:', error);
            } finally {
                setLoading(false);
            }
        });
      };
    
    const renderReplyButtonOrInput = (post) => {
        if (responseState[post.ForumPostModel.Id]) {
            return (
                <InputChildrenPost
                    key={`input-${post.ForumPostModel.Id}`}
                    publishPost={handleReplyOrEditPost(post, editContent)}                    
                    hidePost={handleHidePost(post)}
                    initialContent={editContent}
                />
            );
        } else {
            return (
                    <button className="btn-reply" onClick={() => toggleResponse(post)}>
                        <div className="show-answers-container">
                            <i className="fas fa-comment"></i>{' '}
                            <Translate id="forum.reply" options={{renderInnerHtml: true}}/> 
                        </div>
                    </button>
            );
        }
    };

    const renderHeaderActions = (post) => {
        // Teachers can delete anyone's post
        return (
            <div className="foroContent--header__actions">
                <button onClick={() => handleArchiveClick(post.ForumPostModel)}>
                    <i className="fas fa-trash-alt"></i>
                </button>
                {post.ShowEdit ? 
                    <button onClick={() => toggleResponse(post, true)}>
                        <i className="fas fa-edit"></i>
                    </button> 
                : null}
            </div>
        );
    };

    const toggleShowAnswers = async (postId) => {
        // Get current visibility state before toggling
        const isHidden = !visibleChildren[postId];
    
        if (isHidden) {
            // Fetch fresh data when showing responses
            await fetchMoreResponses(postId, 1);
        }
        // Toggle visibility
        setVisibleChildren(prevState => ({
            ...prevState,
            [postId]: !prevState[postId]
        }));
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

    // Render children posts (replies) 
    const renderChildren = (parentId) => {
        const parentPost = findPostById(parentId, data);
        if (!parentPost) return null;
        const children = parentPost.responses || [];
        const pagination = paginationState[parentId];
        return (
            visibleChildren[parentId] && (
                <div>
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
                                dangerouslySetInnerHTML={{__html: child.ForumPostModel.Body}}
                                />
                                {renderReplyButtonOrInput(child)}
                                {child.ForumPostModel.ChildPostCount > 0 && (
                                    <button className="btn-moreAnswers" onClick={() => toggleShowAnswers(child.ForumPostModel.Id)}>
                                        <div className="show-answers-container">
                                            {visibleChildren[child.ForumPostModel.Id] ?
                                                <>
                                                    <i className="fas fa-chevron-up"></i>{' '}
                                                    <Translate id="forum.hide-more-answers" options={{renderInnerHtml: true}}/> 
                                                </> 
                                                :
                                                <>
                                                    <i className="fas fa-chevron-down"></i>{' '}
                                                    <Translate id="forum.show-more-answers" options={{renderInnerHtml: true}}/> 
                                                </> 
                                            }
                                            {` (${child.ForumPostModel.ChildPostCount})`}
                                        </div>
                                    </button>
                                )}
                            </div>
                            {renderChildren(child.ForumPostModel.Id)} {/* Recursively render children */}
                        </div>
                    ))}
                    {pagination && pagination.page < pagination.totalPages && (
                        <button className="btn-moreAnswers" onClick={() => fetchMoreResponses(parentId, pagination.page + 1)}>
                            <div className="show-answers-container"> 
                                <i className="fas fa-chevron-down"></i>{' '}
                                <Translate id="forum.show-more-answers" options={{renderInnerHtml: true}}/> 
                            </div>
                        </button>
                    )}
                </div>
            )
    );
    };

    // Filter top level posts (posts without parent)
    const topLevelPosts = data?.filter((post) => post.ForumPostModel.ParentPostId === null);

    return (
        loading ? <LoadingFullWindow /> :
            topLevelPosts?.map((post) => {
                return (
                    <div key={post.ForumPostModel.Id} className="wrapper-foroContent" style={{ marginBottom: '25px' }}>
                        <div className="foroContent--header">
                            <div className="foroContent--header__user">
                                <p>{post.AuthorName}</p>
                                <p className="header__user--date">
                                    {post.ForumPostModel.CreatedAt ? formatDate(post.ForumPostModel.CreatedAt) : ''}
                                </p>
                            </div>
                            {renderHeaderActions(post)}
                        </div>
                        <div className="foroContent--body">
                            <div className="foroContent--body_response" dangerouslySetInnerHTML={{ __html: post.ForumPostModel.Body }} />
                            {renderChildren(post.ForumPostModel.Id)}
                            {renderReplyButtonOrInput(post)}
                            {post.ForumPostModel.ChildPostCount > 0 && (
                                <button 
                                className="btn-moreAnswers"
                                onClick={() => toggleShowAnswers(post.ForumPostModel.Id)}
                                >
                                    <div className="show-answers-container">
                                        {visibleChildren[post.ForumPostModel.Id] ?
                                            <>
                                                <i className="fas fa-chevron-up"></i>{' '}
                                                <Translate id="forum.hide-more-answers" options={{renderInnerHtml: true}}/> 
                                            </> 
                                            :
                                            <>
                                                <i className="fas fa-chevron-down"></i>{' '}
                                                <Translate id="forum.show-more-answers" options={{renderInnerHtml: true}}/> 
                                            </> 
                                        }
                                        {`(${post.ForumPostModel.ChildPostCount})`}
                                    </div>
                                </button>
                            )}
                        </div>
                    </div>
                    );
                })   
    );
};

export default ParentPosts;