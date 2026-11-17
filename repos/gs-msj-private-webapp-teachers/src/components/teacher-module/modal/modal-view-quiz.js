import { useEffect } from 'react';
import { Modal, ModalBody, ModalHeader } from 'reactstrap';

import store from '../../../redux/store';
import { withLocalize } from "react-localize-redux";
import { renderToStaticMarkup } from 'react-dom/server';

import { getLessonQuizzesDetails } from "../../../api/teachers";
import LearningGapLessonView, { QuizzesDropdown }
    from "../learning-gap/learning-gap-lessons/learning-gap-lesson-view/learning-gap-lesson-view";
import { Provider } from 'react-redux';
import { LocalizeProvider } from 'react-localize-redux';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';

/**
 * Component to view the quizzes.
 */
const ModalViewQuiz = ( {visible, lessonId, onClose, quizId} )=>{

    const lessonDetailAPI = [
        {
            accordionId: "1",
            titleId: "learning-gap.exams",
            state: "exams",
            apiMethod: getLessonQuizzesDetails,
            examType: "1",
            typeDispatch: "FETCH_EXAMS_DETAIL",
            render: quiz => {
                const filteredQuiz = Array.isArray(quiz) ? quiz.filter(q => q.QuizModel.Id === quizId) : [];
                const firstItem = filteredQuiz.length > 0 ? filteredQuiz[0] : null;
                return firstItem ? <QuizzesDropdown quizzes={[firstItem]} /> : null;
            }
        },
    ];

    // Initialize translations based on the current version
    useVersionedTranslations();

    const toggle = () => {
        onClose(false);
    }

    return (
        <div>
            <Modal isOpen={visible} toggle={toggle} className="modal-results">
                <ModalHeader toggle={toggle} />
                <ModalBody>
                    <Provider store={store}>
                        <LocalizeProvider store={store}>
                            {
                                typeof lessonId !== undefined && lessonId !== null && lessonId !== '' ?
                                    <LearningGapLessonView lessonId={lessonId} lessonDetailsAPI={lessonDetailAPI} quizId={quizId} /> : null}
                        </LocalizeProvider>
                    </Provider>
                </ModalBody>
            </Modal>
        </div>
    );
    
}
export default withLocalize(ModalViewQuiz);