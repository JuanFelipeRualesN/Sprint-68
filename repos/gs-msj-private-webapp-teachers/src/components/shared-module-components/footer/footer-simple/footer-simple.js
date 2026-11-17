import React from "react";
import "./style.css";
import LanguageSelector from "../../../shared-module-components/selector/language-selector";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faChild } from "@fortawesome/free-solid-svg-icons";
import { Translate } from "react-localize-redux";
import {Link} from "react-router-dom";
import Settings from "../../../../custom-classes/settings";

const Footer = () => {

    return (
        <>
            <div className="wrapper-footer">
                <footer className="main-footer-students">
                    <div className="col-right--copyright">
                        <span className="hide-min"> <span className="txt-none"><Translate id="user-help.authorized" options={{ renderInnerHtml: true }} /></span> | </span>

                        <Link className="red-link" to="/privacy">
                            <span className="red-text" style={{ textDecoration: "underline" }}>
                                <FontAwesomeIcon icon={faChild} />{' '}
                                <span className="txt-none"><Translate>{({ translate }) => <span>{translate("children-policy.title", null, { renderInnerHtml: true })}</span>}</Translate></span>
                            </span>
                        </Link>{" "}
                        <span className="hide-min"> | <span className="txt-none"><Translate id="user-help.customer-service" options={{ renderInnerHtml: true }} />{" "}</span>{Settings.PHONE_NUMBER}</span>
                    </div>

                    <div className="col-left--footer-laguages">
                        <LanguageSelector />
                    </div>
                </footer>
            </div>
        </>
    );
};

export default Footer;
