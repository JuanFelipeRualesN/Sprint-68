/**
 * Created by portiz 2020/12/17
 */
import React, { useState } from 'react';
import { Button, Modal, ModalHeader, ModalBody} from 'reactstrap';
/** Multiple Language imports **/
import { Translate } from "react-localize-redux";
import {archiveForumPost} from "../../../api/teachers";
import {BeatLoader} from "react-spinners";
/** End of Multiple Language imports **/

/**
 * Modal to archive an forum post
 */
const ModalForumPostArchive = ({ forumId, postId, visible, updateParent }) => {
    const [errorMessage, setErrorMessage] = useState('');
    const [loading, setLoading] = useState(false);
    const [showErrorMessage, setShowErrorMessage] = useState(false);
    const [isVisible, setIsVisible] = useState(visible);

    // Hide or shows the window
    const toggle = () => {
        const newVisible = !isVisible;
        setIsVisible(newVisible);
        // Updates the parent (send it owns to state to update it)
        updateParent({
            errorMessage,
            loading,
            forumId,
            postId,
            showErrorMessage,
            visible: newVisible,
        });
    };

    // Toggles when yes
    const toggleYes = () => {
        setLoading(true);
        // Archive forum
        let apiCall = archiveForumPost(forumId, postId);
        apiCall.then(() => {
            updateParent({
                visible: false,
                deletedPostId: postId,
            });
        });
    };

    // Toggles when no
    const toggleNo = () => {
        setIsVisible(false);
        // Updates the parent (send it owns to state to update it)
        updateParent({
            errorMessage,
            loading,
            forumId,
            postId,
            showErrorMessage,
            visible: false,
        });
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            <Modal isOpen={isVisible} toggle={toggle}>
                <ModalHeader toggle={toggle}>
                    <Translate id="forum.archive" />
                </ModalHeader>
                <ModalBody>
                    {showErrorMessage ?
                        <div className="content" dangerouslySetInnerHTML={{__html: errorMessage}}/>
                        :
                        <div>
                            <Translate id="forum.forum-post-archive-note" />
                            <hr/>
                            <p className="text-center">
                                <Button color="danger" onClick={toggleYes} disabled={loading ? "disabled": ""}>
                                    {loading ?
                                        <BeatLoader
                                            size={10}
                                            color={"#f4ffef"}
                                        /> : null}
                                    <Translate id="forum.yes" />
                                </Button>
                                {' '}
                                <Button color="info" onClick={toggleNo} disabled={loading ? "disabled": ""}>
                                    <Translate id="forum.no" />
                                </Button>
                            </p>
                        </div>
                    }
                </ModalBody>
            </Modal>
        </div>
    );
}

export default ModalForumPostArchive;