import React from 'react'
import { Modal, ModalBody, ModalFooter, ModalHeader} from 'reactstrap';
import SuccessIcon from '../../../assets/images/common-icons/Modal-icon-Success.png'

// Modal to show success message after assigning a lesson to a student
const ModalSuccess = ({closeModal, isOpen, content}) => {
  return (
      <Modal isOpen={isOpen} toggle={() => closeModal('success')} centered={true} size="lg">
        <ModalHeader toggle={() => closeModal('success')}/>
        <ModalBody style={{display: 'flex', flexDirection: 'column', alignItems: 'center', gap: '30px'}}>
          <img src={SuccessIcon} alt="Success" style={{width: '110px'}} />
          <span>{content}</span>
        </ModalBody>
        <ModalFooter/>
      </Modal>
  )
}

export default ModalSuccess