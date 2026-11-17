import React from 'react';
import { withLocalize } from 'react-localize-redux';

/**
 * TabPanel component that conditionally renders its children based on the state and index.
 * @param {Object} props - The component props.
 * @param {number} props.state - The current state to compare with the index.
 * @param {number} props.index - The index to compare with the state.
 * @param {React.ReactNode} props.children - The content to be rendered if state matches index.
 * @returns {JSX.Element|null} The rendered tab panel or null if state does not match index.
 */
const TabPanel = ({ state, index, children }) => {
  return state == index ? <div role="tab" className="Tab">
    {children}
  </div> : null;
}

export default withLocalize(TabPanel);