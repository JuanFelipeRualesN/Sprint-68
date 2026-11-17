
 import React from 'react';
 import { Modal, ModalHeader, ModalBody, Row, Col } from 'reactstrap';
 import { Translate } from "react-localize-redux";
 import CheckboxCountries from "../selectors/multi-selector-country";
 import SelectorCurrency from "../selectors/selector-currency";
 /**
  * Model that handles the countries
  */
const ModalCountriesSelection = ({ 
    visible, 
    checkedValues, 
    checkedRegionsValues, 
    selectAll, 
    planAvailableIsos, 
    disabledRegionCountryIsos, 
    handleCountryRegionsSelected, 
    updateParent 
}) => {
    const toggle = () => {
        updateParent();
    };

    return (
        <div>
            <Modal isOpen={visible} toggle={toggle} className="modal-results modal-countries">
                <ModalHeader toggle={toggle}>
                    Países / Estados
                    <br/>
                    <small>
                        Seleccione los Países / Estados disponibles para el Plan
                    </small>
                </ModalHeader>
                <ModalBody>
                    <Row>
                        <Col xs={12}>
                        {true ?
                           <CheckboxCountries
                                checkedValues={checkedValues}
                                checkedRegionsValues={checkedRegionsValues}
                                selectAll={selectAll}
                                planAvailableIsos={planAvailableIsos}
                                disabled={disabledRegionCountryIsos}
                                updateParent={handleCountryRegionsSelected}/> :
                                null
                       }
                           {/*!isEditedPlan
                                ?
                           <CheckboxCountries disabled={disabledRegionCountryIsos}
                                updateParent={handleCountryRegionsSelected}/> 
                               : null
                           */}
                        </Col>
                    </Row>
                </ModalBody>
            </Modal>
        </div>
    );
};

 export default ModalCountriesSelection;