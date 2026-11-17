/**
 * Created by portiz on 5/12/20.
 */
import React, { useState, useEffect } from 'react';
import { Modal, ModalBody, ModalHeader } from 'reactstrap';
/** Multiple Language imports **/
import LearningGapLessonView
    from "../learning-gap/learning-gap-lessons/learning-gap-lesson-view/learning-gap-lesson-view";
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from 'react-dom/server';
import { Provider } from 'react-redux';
import { LocalizeProvider } from 'react-localize-redux';
import store from '../../../redux/store';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';
/** End of Multiple Language imports **/

/**
 * Component to view the lessons.
 */
const ModalViewLesson = ({ lessonId, visible, updateParent }) => {
    const [isVisible, setIsVisible] = useState(visible);

    // Initialize translations based on the current version
    useVersionedTranslations();

    // Hide or shows the window
    const toggle = () => {
        setIsVisible(false);
        updateParent(false);
    };

    return (
        <div>
            <Modal isOpen={isVisible} toggle={toggle} className="modal-results">
                <ModalHeader toggle={toggle} />
                <ModalBody>
                    <Provider store={store}>
                        <LocalizeProvider store={store}>
                            {
                                typeof lessonId !== undefined && lessonId !== null && lessonId !== '' ?
                                    <LearningGapLessonView lessonId={lessonId} /> : null}
                        </LocalizeProvider>
                    </Provider>
                </ModalBody>
            </Modal>
        </div>
    );
};

export default withLocalize(ModalViewLesson);