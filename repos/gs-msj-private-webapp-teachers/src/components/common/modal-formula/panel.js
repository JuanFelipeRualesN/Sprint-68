import React, { useState } from "react";
import { Tab, Tabs, Button } from "@material-ui/core";
import TabPanel from "./tab-panel";
import { PANELS } from "./panels";
import "./panel.css";
import { Translate, withLocalize } from "react-localize-redux";

/**
 * Panel component that renders a tabbed interface with formulas and symbols.
 * @param {Object} props - The component props.
 * @param {Function} [props.onChanges] - Callback function to handle changes when a formula or symbol is clicked. Defaults to an empty function.
 * @returns {JSX.Element} The rendered Panel component.
 */
const Panel = ({ onChanges = (fomula) => { } }) => {
  const [currentTab, setCurrentTab] = useState(0);
  const { Symbols, Formulas } = PANELS;

  /**
   * Handles the change event for the tab component.
   * @param {Object} _ - The event object (not used).
   * @param {number} value - The new value of the tab.
   */
  const handleChange = (_, value) => {
    setCurrentTab(value);
  };

  return (
    <div className="Formula-Panel">
      <Tabs
        value={currentTab}
        indicatorColor="primary"
        textColor="primary"
        onChange={handleChange}
      >
        <Tab label={<Translate id="formulas.general" />} onClick={(() => setCurrentTab(0))} />
        <Tab label={<Translate id="formulas.symbols" />} onClick={() => setCurrentTab(1)} />
      </Tabs>
      <TabPanel state={currentTab} index={0}>
        {Formulas.map((symbol, index) => {
          return (
            <Button
              key={"symbol-" + index}
              id={symbol.id}
              variant="contained"
              color="primary"
              size="small"
              onClick={() => {
                onChanges(symbol.formula);
              }}
            >
              <Translate id={symbol.title} />
            </Button>
          );
        })}
      </TabPanel>
      <TabPanel state={currentTab} index={1}>
        {Symbols.map((symbol, index) => {
          return (
            <Button
              key={"symbol-" + index}
              id={symbol.id}
              variant="contained"
              color="primary"
              size="small"
              onClick={() => {
                onChanges(symbol.formula);
              }}
            >
              {symbol.title}
            </Button>
          );
        })}
      </TabPanel>
    </div>
  );
}

export default withLocalize(Panel);
