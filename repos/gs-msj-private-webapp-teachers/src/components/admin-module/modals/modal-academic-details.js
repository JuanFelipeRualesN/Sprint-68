import React, { useState, useEffect } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import { Translate } from "react-localize-redux";
import AcademicDetailsTable from '../datatables/academic-details-table';
import { getSchoolEntityDetails, getAcademicsDetailsByEntity } from "../../../api/schools";
import LoadingFullWindow from "../../shared-module-components/loader-windows/loader-full-window";
import styled from "styled-components";


const EmptyListStyled = styled.p`
  color: #ccc;
  display: block
  justify-content: center;
  text-align: center;
  margin: 3em auto;

  & img {
    display: block;
    margin: 1em auto 0.25em auto;
    font-size: 45px;
    width: 175px;
    opacity: .5;
  }

  & i {
    display: block;
    margin: 1em auto 0.25em auto;
    font-size: 45px;
    width: 175px;
    opacity: .5;
  }
`;
const ModalAcademicsDetails = ({ entityId, visible, updateParent }) => {
    const [loading, setLoading] = useState(false);
    const [details, setDetails] = useState([]);
    const [isVisible, setIsVisible] = useState(visible);

    const getAcademicsDetails = async (entityId, page, pageSize) => {
        setLoading(true);
        try {
            const res = await getAcademicsDetailsByEntity(entityId, page, pageSize);
            const data = res.data;
            setLoading(false);
            setDetails(data.EntityAcademicDetailModels);
        } catch {
            setLoading(false);
        }
    };

    useEffect(() => {
        if (entityId) {
            getAcademicsDetails(entityId, 0, 20);
        }
    }, [entityId]);

    const toggle = () => {
        const newVisible = !isVisible;
        setIsVisible(newVisible);
        updateParent(null, newVisible);
    };

    const handleSelectedItemTable = (item) => {
        updateParent(item, !isVisible);
    };

    return (
        <div>
            <Modal isOpen={isVisible} toggle={toggle} className="modal-results">
                <ModalHeader toggle={toggle}>
                </ModalHeader>
                <ModalBody>
                    <div className="wrapper-content-academic-details">
                        <div className="content-wizard">
                            <h1 className="content-wizard--title">
                                <Translate id="school-academics-details.select-detail" options={{ renderInnerHtml: true }} />
                            </h1>

                            <div className='mt-2'>
                                <AcademicDetailsTable details={details} updateParent={handleSelectedItemTable} buttonTxt={"select"} />
                            </div>

                            {!details.length && (
                                <EmptyListStyled>
                                    <i className="fas fa-book-reader"></i><Translate id="school-academics-details.detail-empty" options={{ renderInnerHtml: true }} />
                                </EmptyListStyled>
                            )}
                        </div>
                    </div>
                </ModalBody>
                <ModalFooter>
                    <Button color="secondary" onClick={toggle}>
                        <Translate id="general.close" />
                    </Button>
                </ModalFooter>
            </Modal>
            {loading ? <LoadingFullWindow /> : null}
        </div>
    );
};

export default ModalAcademicsDetails;