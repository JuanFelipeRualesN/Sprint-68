import React from 'react';
import LoadingIcon from './loading-icon';
import './style.css';
import {Col, Row} from "reactstrap";

/**
 * Component that handles the full loading window.
 * Its visibility should be controlled by the parent.
 */
const LoadingFullWindow = ({message}) => {
    return (
        <div className="loader-full-window">
            <LoadingIcon/>
            <div style={{marginTop: '16px'}}>
                { message != null && message !== '' ?
                message : null}
            </div>
        </div>
    );
};

export default LoadingFullWindow;