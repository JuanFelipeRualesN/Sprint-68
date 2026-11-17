/**
 * Created by portiz on 8/15/2018.
 */
import React, { useEffect } from 'react';
/** Multiple Language imports **/
import {withLocalize} from "react-localize-redux";
import {renderToStaticMarkup} from 'react-dom/server'; // Fix by the author. Do not remove even if it's not resolved
/** End of Multiple Language imports **/
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import './style.css';
import useVersionedTranslations from '../../../hooks/useVersionedTranslations';
/**
 * The component (window) landing (index page)
 */
const LandingPage2 = () => {
    // Initialize translations based on the current version
    useVersionedTranslations();

    useEffect(() => {
        // Redirect to landing asked by Martinez 08.17.2020 11:15 AM
        document.body.classList.add('background-white');
        window.location.replace('https://www.genialskills.com');
    }, []);

    return (
        <div>
            <LoadingFullWindow/>
        </div>
    );
};

export default withLocalize(LandingPage2);