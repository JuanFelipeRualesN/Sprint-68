import React, { useEffect, useState } from "react";
import styled from "styled-components";
import Settings from "../../../../custom-classes/settings";
import {Translate, withLocalize} from "react-localize-redux";
import {renderToStaticMarkup} from "react-dom/server";
import useVersionedTranslations from "../../../../hooks/useVersionedTranslations";

// Container that accounts for the sidebar on desktop
const CenterContainer = styled.div`
    margin-left: 0px; /* Adjust this based on the actual width of your sidebar */
    padding: 20px;

    @media (max-width: 900px) {
        margin-left: 0;
    }
`;

const IFrameTemplateLesson = styled.iframe`
    position: absolute;
    top: 0;
    left: 0;
    bottom: 0;
    right: 0;
    width: 80%;
    height: 80%;
    margin: auto;
    border: none;
    box-shadow: 0 0 12px rgba(0, 0, 0, 0.15);
`;

const MessageBox = styled.div`
  text-align: center;
  font-size: 16px;
  color: #333;
  background-color: #fff8e1;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 0 6px rgba(0, 0, 0, 0.1);
`;

const KinderInteractive = () => {
    const [openedInNewTab, setOpenedInNewTab] = useState(false);

    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        if (window.innerWidth < 900) {
            window.open(Settings.KINDER_INTERACTIVE_RESOURCE, "_blank", "noopener,noreferrer");
            setOpenedInNewTab(true);
        }
    }, []);


    return (
        <>
            <div className="wrapper-content">
                <CenterContainer>
                    {!openedInNewTab && (
                        <IFrameTemplateLesson src={Settings.KINDER_INTERACTIVE_RESOURCE}
                            allow="fullscreen"
                            allowfullscreen />
                    )}

                    {openedInNewTab && (
                        <MessageBox>
                            <Translate id="student-platform.compatibility"
                                       options={{renderInnerHtml: true}}/>
                        </MessageBox>
                    )}
                </CenterContainer>
            </div>
        </>
    );
};

export default withLocalize(KinderInteractive);
