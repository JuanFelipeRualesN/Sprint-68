import React, { useEffect, useState } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Dialog, DialogContent } from "@material-ui/core";
// import Heading from '../../componentes/heading';
import Mixed from "./mixed";
import OpenQuestion from "./open-question";
import TrueOrFalse from "./true-or-false";
import Selection from "./selection";
import MatchQuestionPreview from "../../../planning/assignments/components/match-preview"
import Button from "../../shared/components/button";
import Loading from "../../shared/components/loading";
import { faEye, faTimes } from '@fortawesome/free-solid-svg-icons';

export default function AssignmentPreview({
  maxWidth = "lg",
  quiz,
  loading,
  disabled,
}) {
  const [open, setOpen] = useState(false);
  const [quizModel, setQuizModel] = useState();
  const [isLoading, setIsLoading] = useState(false);

  const handleOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  useEffect(() => {
    if (quiz) {
      setQuizModel(quiz);
    }
    if (loading !== isLoading) {
      setIsLoading(loading);
    }
  }, [quiz, loading]);

  return (
    <>
      <Button
        variant="simple"
        icon={
          isLoading ? (
            <Loading show={isLoading} type="button" />
          ) : (
            <FontAwesomeIcon icon={faEye} />
          )
        }
        onClick={handleOpen}
        disabled={isLoading || disabled}
      />
      <Dialog
        className="overflow-hidden"
        open={open}
        onClose={handleClose}
        maxWidth={maxWidth}
        fullWidth={true}
      >
        <div className="contentModal--confirmation">
          <div className="contentModal--close" onClick={handleClose}>
            <div className="wrapper-twoCol-highlight m-0">
              <div className="col">
                {/* <Heading as="h2">{quizModel?.Title}</Heading> */}
              </div>
              <div className="col text-right">
                <FontAwesomeIcon icon={faTimes} />
              </div>
            </div>
          </div>
        </div>
        <DialogContent
          style={{ maxHeight: "80vh", overflowY: "auto", padding: 0 }}
        >
          <>
            <div className="Modal--confirmation-body text-left mw-500">
              {quizModel && quizModel.QuizTypeId === "1" && (
                <Selection quiz={quizModel} />
              )}

              {quizModel && quizModel.QuizTypeId === "2" && (
                <MatchQuestionPreview quiz={quizModel} />
              )}

              {quizModel && quizModel.QuizTypeId === "3" && (
                <OpenQuestion quiz={quizModel} />
              )}

              {quizModel && quizModel.QuizTypeId === "4" && (
                <TrueOrFalse quiz={quizModel} />
              )}

              {quizModel && quizModel.QuizTypeId === "5" && (
                <Mixed quiz={quizModel} />
              )}

            </div>
          </>
        </DialogContent>
      </Dialog>
    </>
  );
}
