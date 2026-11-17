import React, {useState} from "react";
import {Button, Form, FormGroup, Input, Label, Modal, ModalBody, ModalFooter, ModalHeader} from "reactstrap";
import Select from "react-select";
import { Translate, withLocalize } from "react-localize-redux";

const AssignDatesModal = ({ modal, toggle, onSubmit, translate }) => {
    const [date, setDate] = useState('');
    const [monthName, setMonthName] = useState('Enero');
    const [diagnosticTest, setDiagnosticTest] = useState('English');

    const handleSave = () => {
        if (date && monthName) {
            onSubmit(monthName, date);
            toggle();
            setDate('');
            setMonthName('Enero');
            setDiagnosticTest('English');
        } else {
            alert('Por favor, selecciona una fecha y un mes.');
        }
    };

    const monthOptions = [
        { value: 'Enero', label: 'Enero' },
        { value: 'Febrero', label: 'Febrero' },
        { value: 'Marzo', label: 'Marzo' },
        { value: 'Abril', label: 'Abril' },
        { value: 'Mayo', label: 'Mayo' },
        { value: 'Junio', label: 'Junio' },
        { value: 'Julio', label: 'Julio' },
        { value: 'Agosto', label: 'Agosto' },
        { value: 'Septiembre', label: 'Septiembre' },
        { value: 'Octubre', label: 'Octubre' },
        { value: 'Noviembre', label: 'Noviembre' },
        { value: 'Diciembre', label: 'Diciembre' },
    ];

    const diagnosticOptions = [
        { value: 'English', label: translate("dashboards.english") },
        { value: 'Spanish', label: translate("dashboards.spanish") },
    ];

    return (
        <Modal isOpen={modal} toggle={toggle}>
            <ModalHeader toggle={toggle} className="text-white">
                <Translate id="dashboards.assign-diagnostic-date" />
            </ModalHeader>
            <ModalBody>
                <Form>
                    <FormGroup>
                        <Label for="monthSelect" className="font-weight-bold">
                            <Translate id="dashboards.select-month" />
                        </Label>
                        <br />
                        <Select
                            classNamePrefix="mantine-like"
                            className="mantine-like__control"
                            value={monthName}
                            onChange={(newValue) => setMonthName(newValue)}
                            options={monthOptions}
                        />
                    </FormGroup>
                    <FormGroup>
                        <Label for="diagnosticTest" className="font-weight-bold">
                            <Translate id="dashboards.select-diagnostic-test" />
                        </Label>
                        <br />
                        <Select
                            classNamePrefix="mantine-like"
                            className="mantine-like__control"
                            value={diagnosticTest}
                            onChange={(newValue) => setDiagnosticTest(newValue)}
                            options={diagnosticOptions}
                        />
                    </FormGroup>
                    <FormGroup>
                        <Label for="diagnosticDate" className="font-weight-bold">
                            <Translate id="dashboards.select-date" />
                        </Label>
                        <Input
                            type="date"
                            name="date"
                            id="diagnosticDate"
                            value={date}
                            onChange={(e) => setDate(e.target.value)}
                        />
                    </FormGroup>
                </Form>
            </ModalBody>
            <ModalFooter>
                <button className="btn-default" onClick={toggle}>
                    <Translate id="dashboards.cancel" />
                </button>
                <button className="btn-default button-secondary-custom" onClick={handleSave}>
                    <Translate id="dashboards.save-assignment" />
                </button>
            </ModalFooter>
        </Modal>
    );
};

export default withLocalize(AssignDatesModal);