import React from "react";
import "../../shared/styles/assignments.css";
import { Col, Row, Input } from 'reactstrap';
import { translate } from "../../shared/utils";

export default function MatchQuestionPreview({ quiz }) {

  const questions = quiz.QuestionRequestModelList.map((item) => 
    item.QuizQuestionModel
  );
  const responses = quiz.QuestionRequestModelList.flatMap((item) =>
    item.QuizQuestionResponseModelList.map((response) => ({
      ...response,
    }))
  );
  const questionsRandom = questions.sort(() => Math.random() - 0.5);
  const responsesRandom = responses.sort(() => Math.random() - 0.5);

  return (
    <div>
      <div className="wrapper-modalPareo">
        <h1>{quiz.Title}</h1>
        <div className="modalPareo--Body">
          <div className="modalPareo--Body_premisa">
            <div className="modalCorrections--Body_premisa--question d-flex">
              <span
                dangerouslySetInnerHTML={{
                  __html: quiz?.Instruction || "",
                }}
              />
            </div>
            {quiz.QuizExtraInfoModel && (
              <div
                dangerouslySetInnerHTML={{
                  __html: quiz.QuizExtraInfoModel.ExtraInfo,
                }}
              />
            )}
            <div className="modalPareo--Body_premisa--answer">
              {questionsRandom.map((question, i) => {
                const response = responsesRandom[i];
                const letter = String.fromCharCode(97 + i);
                return (
                  <Row>
                      <Col xs={{size: 12}} md={{size: 6}}>
                        <div className="card-little-bit-gray padding-20">
                            <Row>
                                <Col xs={{size:3}}>
                                  <Input 
                                    type="text"
                                    disabled
                                    value={null}
                                  />
                                </Col>
                                <Col xs={{size:9}}>
                                    <div dangerouslySetInnerHTML={{ __html: question.Desc }}/>
                                </Col>
                            </Row>
                            <hr/>
                        </div>
                      </Col>
                      <Col xs={{size: 12}} md={{size: 6}}>
                        <div className="card-little-bit-gray padding-20">
                            <Row>
                                <Col xs={{size:3}}>
                                    {letter}{'. '}
                                </Col>
                                <Col xs={{size:9}}>
                                    <div dangerouslySetInnerHTML={{ __html: response.Response }}/>
                                </Col>
                            </Row>
                            <hr/>
                        </div>
                      </Col>
                  </Row>
                );
              })}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
