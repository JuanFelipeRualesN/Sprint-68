import React from 'react';
import Faq from 'react-faq-component';
import Settings from "../../../custom-classes/settings";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faVideo } from '@fortawesome/free-solid-svg-icons';
import { Translate } from "react-localize-redux";
import styled from 'styled-components';

const VideoContainer = styled.div`
  position: relative;
  height: 100vh;
  overflow: hidden;
  max-width: 100%;
  margin: 1rem;
  iframe {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
  }
`;

const FaqVideoTutorials = () => {
    const data = {
        title: <div className='faq-title'><FontAwesomeIcon icon={faVideo} />{' '}<Translate id="help.videos" options={{ renderInnerHtml: true }} /></div>,
        rows: [
            {
                title: <Translate id="help.know_your_platform" options={{ renderInnerHtml: true }} />,
                content: <ul>
                    <li><Translate id="help.elementary" options={{ renderInnerHtml: true }} /><VideoContainer>
                        <iframe
                            src={Settings.ESTUDIA_AVANZA_STUDENT_ELEMENTARY}
                            allowFullScreen
                        />
                    </VideoContainer></li>
                    <li><Translate id="help.intermediate" options={{ renderInnerHtml: true }} /><VideoContainer>
                        <iframe
                            src={Settings.ESTUDIA_AVANZA_STUDENT_INTERMEDIATE}
                            allowFullScreen
                        />
                    </VideoContainer></li>
                    <li><Translate id="help.superior" options={{ renderInnerHtml: true }} /><VideoContainer>
                        <iframe
                            src={Settings.ESTUDIA_AVANZA_STUDENT_SUPERIOR}
                            allowFullScreen
                        />
                    </VideoContainer></li>
                </ul>
            },
            {
                title: <Translate id="help.platform_features" options={{ renderInnerHtml: true }} />,
                content: <ul>
                    <li><Translate id="help.elementary" options={{ renderInnerHtml: true }} /><VideoContainer>
                        <iframe
                            src={Settings.STUDENT_TUTORIAL_ELEMENTARY_PLATFORM_FUNCTIONS}
                            allowFullScreen
                        />
                    </VideoContainer></li>
                    <li><Translate id="help.intermediate" options={{ renderInnerHtml: true }} /><VideoContainer>
                        <iframe
                            src={Settings.STUDENT_TUTORIAL_INTERMEDIATE_PLATFORM_FUNCTIONS}
                            allowFullScreen
                        />
                    </VideoContainer></li>
                    <li><Translate id="help.superior" options={{ renderInnerHtml: true }} /><VideoContainer>
                        <iframe
                            src={Settings.STUDENT_TUTORIAL_SUPERIOR_PLATFORM_FUNCTIONS}
                            allowFullScreen
                        />
                    </VideoContainer></li>
                </ul>
            },
            {
                title: <Translate id="help.parts_of_the_lesson" options={{ renderInnerHtml: true }} />,
                content: <ul>
                    <li><Translate id="help.elementary" options={{ renderInnerHtml: true }} /><VideoContainer>
                        <iframe
                            src={Settings.STUDENT_TUTORIAL_ELEMENTARY_PARTS_OF_LESSONS}
                            allowFullScreen
                        />
                    </VideoContainer></li>
                    <li><Translate id="help.intermediate" options={{ renderInnerHtml: true }} /><VideoContainer>
                        <iframe
                            src={Settings.STUDENT_TUTORIAL_INTERMEDIATE_PARTS_OF_LESSONS}
                            allowFullScreen
                        />
                    </VideoContainer></li>
                    <li><Translate id="help.superior" options={{ renderInnerHtml: true }} /><VideoContainer>
                        <iframe
                            src={Settings.STUDENT_TUTORIAL_SUPERIOR_PARTS_OF_LESSONS}
                            allowFullScreen
                        />
                    </VideoContainer></li>
                </ul>
            },
        ]
    };

    return (
        <Faq data={data} />
    )
};
export default FaqVideoTutorials;