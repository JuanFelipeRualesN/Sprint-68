import React from 'react';
import { Media } from 'reactstrap';
import loaderSvg from '../../../assets/svg/loader.svg';

/**
 * The loading icon
 */
const LoadingIcon = () => {

    return (
        <Media src={loaderSvg} alt="Loader" />
    );
}

export default LoadingIcon;