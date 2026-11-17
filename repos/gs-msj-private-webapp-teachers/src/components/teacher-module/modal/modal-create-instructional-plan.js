import React from 'react'
import DocumentWarning from '../../../assets/images/common-icons/Plan-icon-Warning.png'
import { Translate } from 'react-localize-redux'
import {Modal, ModalBody, ModalHeader, Button} from 'reactstrap';

// Modal to show a warning message when the user tries to assign a lesson to a student without a plan
const CreateInstructionalPlanModal = ({openNoPlanModal, closeModal, handleOpenCreatePlanModal, handleCloseCreatePlanModal }) => {
  return (
    <Modal 
        centered
        isOpen={openNoPlanModal} 
        toggle={() => closeModal('noPlans')}
      >
        <ModalHeader toggle={() => closeModal('noPlans')} />
        <ModalBody style={{paddingBottom: '5rem'}}>
          <div style={{display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center'}}>
            <img src={DocumentWarning} alt="No plans logo" style={{width: '100px'}} />
            <span style={{textAlign: 'center', fontWeight: 'bold', fontSize: '.95rem', marginTop: '10px'}}>
              <Translate id="lesson-catalog.no-instructional-plan" options={{ renderInnerHtml: false }} />
            </span>
            <span style={{textAlign: 'center', fontSize: '.85rem'}}>
              <Translate id="lesson-catalog.instructional-plan-description" options={{ renderInnerHtml: false }} />
            </span>
            <div style={{display: 'flex', width: '100%', justifyContent: 'center', gap: '40px', marginTop: '20px'}}>
              <Button onClick={handleOpenCreatePlanModal} style={{backgroundColor: 'green', color: 'white', fontWeight: 600, width: '100px'}}> 
                <Translate id="lesson-catalog.yes" options={{ renderInnerHtml: false }} />
              </Button>
              <Button onClick={handleCloseCreatePlanModal} style={{backgroundColor: 'gray', color: 'white', fontWeight: 600, width: '100px'}}> 
                <Translate id="lesson-catalog.no" options={{ renderInnerHtml: false }} />
              </Button>
            </div>

          </div>
        </ModalBody>
      </Modal>

  )
}

export default CreateInstructionalPlanModal