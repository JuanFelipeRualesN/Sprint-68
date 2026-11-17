import React, { useState, useEffect } from 'react';
import Select from 'react-select';
import { Button, Modal, ModalHeader, ModalBody, Input, ModalFooter, Col, Row } from 'reactstrap';
import { Translate } from "react-localize-redux";
import { createGroup } from "../../../api/teachers";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { getUserActiveSubscriptions } from "../../../api/subscriptions";
import { BeatLoader } from "react-spinners";
import LanguageManager from "../../../custom-classes/language-manager";
import moment from 'moment';
import { faSave, faUsers } from "@fortawesome/free-solid-svg-icons";
import { convertMaxLengthSting } from "../../../utils/utils";

/**
 * Modal to create teacher planning
 */
const ModalGroupNameCreator = ({ visible, updateParent }) => {
    const [errorMessage, setErrorMessage] = useState('');
    const [groupName, setGroupName] = useState('');
    const [loading, setLoading] = useState(false);
    const [saving, setSaving] = useState(false);
    const [showMessage, setShowMessage] = useState(false);
    const [subscriptions, setSubscriptions] = useState([]);
    const [subscriptionsOptions, setSubscriptionsOptions] = useState([]);
    const [subjectsOptions, setSubjectsOptions] = useState([]);
    const [subscriptionOptionSelected, setSubscriptionOptionSelected] = useState(null);
    const [subscriptionSelected, setSubscriptionSelected] = useState(null);
    const [subjectsLevelsSelected, setSubjectsLevelsSelected] = useState(null);
    const [language] = useState(LanguageManager.getLanguage());
    const [isVisible, setIsVisible] = useState(visible);

    useEffect(() => {
        getSubjectsAndLevels();

    }, []);

    useEffect(() => {
        setIsVisible(visible);
    }, [visible]);

    const getSubjectsAndLevels = async () => {
        setLoading(true);
        try {
            const res = await getUserActiveSubscriptions();
            const subscriptionsData = Array.isArray(res && res.data) ? res.data : [];

            const monthNamesEn = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
            const monthNamesEs = ["enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre"];
            const monthNames = language === "es" ? monthNamesEs : monthNamesEn;

            const dateFormats = ["MM/DD/YYYY HH:mm:ss", "M/D/YYYY h:mm:ss A", moment.ISO_8601];

            const options = subscriptionsData
                .map(item => {
                    if (!item || !item.SubscriptionModel || !item.SubscriptionModel.Id) return null;

                    const academicDetail =
                        item.EntityAcademicDetailFullResponseModel &&
                        item.EntityAcademicDetailFullResponseModel.EntityAcademicDetailModel;

                    if (!academicDetail) return null;

                    const dateFrom = moment(academicDetail.FullTermStartDate, dateFormats);
                    const dateTo = moment(academicDetail.FullTermEndDate, dateFormats);

                    if (!dateFrom.isValid() || !dateTo.isValid()) return null;

                    const range =
                        monthNames[dateFrom.month()] + " " + dateFrom.year() +
                        " - " +
                        monthNames[dateTo.month()] + " " + dateTo.year();

                    const planName = (item.PlanModel && item.PlanModel.Name) || "";

                    return {
                        value: String(item.SubscriptionModel.Id),
                        label: planName ? range + " | " + planName : range,
                    };
                })
                .filter(Boolean);

            setSubscriptions(subscriptionsData);
            setSubscriptionsOptions(options);
            setLoading(false);
        } catch (error) {
            console.error("getUserActiveSubscriptions failed", error);
            setSubscriptions([]);
            setSubscriptionsOptions([]);
            setLoading(false);
        }
    };

    const handleChangeSubscription = (selected) => {
        setSubscriptionOptionSelected(selected);

        const subscription = subscriptions.find(
            item => String(item.SubscriptionModel.Id) === String(selected && selected.value)
        ) || null;

        const products = (subscription && subscription.SubscriptionProductModelList) || [];
        const seen = new Set();
        const options = products
            .filter(item => {
                if (!item || !item.Subject || !item.Level) return false;
                const key = String(item.ProductId != null ? item.ProductId : item.Subject + "-" + item.Level);
                if (seen.has(key)) return false;
                seen.add(key);
                return true;
            })
            .map(item => ({
                subject: item.Subject,
                label: (item.SubjectName || item.Name || "").trim(),
                level: item.Level,
                value: String(item.ProductId),
            }))
            .sort((a, b) => a.label.localeCompare(b.label, undefined, { numeric: true }));

        setSubjectsOptions(options);
        setSubscriptionSelected(subscription);
        setSubjectsLevelsSelected(null);
    };

    const handleChangeSubjectsLevels = (selected) => {
        setSubjectsLevelsSelected(selected);
    };

    const handleNameChange = (e) => {
        const value = e.currentTarget.value;
        const newValue = convertMaxLengthSting(value, 50);
        setGroupName(newValue);
    };

    const buildParentState = (overrides = {}) => ({
        errorMessage,
        groupName,
        loading,
        showMessage,
        subscriptions,
        subscriptionsOptions,
        subjectsOptions,
        subscriptionSelected,
        subjectsLevelsSelected,
        language,
        visible: isVisible,
        ...overrides,
    });

    // Hide or shows the window
    const toggle = () => {
        const newVisible = !isVisible;
        setIsVisible(newVisible);
        updateParent(buildParentState({ visible: newVisible }));
    };

    // Toggles when yes
    const toogleYes = () => {
        if (!subjectsLevelsSelected) return;

        setSaving(true);
        setShowMessage(false);

        createGroup(groupName.trim(), subjectsLevelsSelected.level, subjectsLevelsSelected.subject)
            .then(() => {
                setSaving(false);
                setIsVisible(false);
                updateParent(buildParentState({ loading: false, visible: false }));
            })
            .catch((error) => {
                const messages =
                    error && error.response && error.response.data && error.response.data.Messages;

                setErrorMessage(
                    Array.isArray(messages) && messages.length
                        ? messages.join("")
                        : "No se pudo crear el grupo. Intenta de nuevo."
                );
                setShowMessage(true);
                setSaving(false);
            });
    };

    // Toggles when no
    const toogleNo = () => {
        setIsVisible(false);
        updateParent(buildParentState({ visible: false }));
    };

    // Render a generic modal. The body text should be sent in the text
    return (
        <div>
            <Modal isOpen={isVisible} toggle={toggle}>
                <ModalHeader toggle={toggle} className="modalHeader-icon">
                    <FontAwesomeIcon icon={faUsers} />{' '}
                    <Translate id="group-management.new-group" />
                </ModalHeader>
                <ModalBody>
                    <Row>
                        <Col xs={12}>
                            <b><Translate id="group-management.group-name" options={{ renderInnerHtml: true }} /></b>
                            <br />
                            <small><Translate id="group-management.group-name-instruction" options={{ renderInnerHtml: true }} /></small>
                        </Col>
                        <Col>
                            <Input value={groupName} type="text" name="groupName"
                                   id="groupName" maxLength={50} onChange={handleNameChange} />
                        </Col>
                    </Row>
                    <div style={{ marginBottom: '1rem' }}>
                        <Row>
                            <Col xs={12}>
                                <b><Translate id="group-management.academic-year-name" options={{ renderInnerHtml: true }} /></b>
                                <br />
                                <small><Translate id="group-management.academic-year-name-instruction" options={{ renderInnerHtml: true }} /></small>
                            </Col>
                            <Col>
                                <Select
                                    classNamePrefix="mantine-like"
                                    className="mantine-like__control"
                                    placeholder={""}
                                    value={subscriptionOptionSelected}
                                    options={subscriptionsOptions}
                                    isLoading={loading}
                                    isDisabled={loading}
                                    onChange={handleChangeSubscription}
                                />
                            </Col>
                        </Row>
                    </div>
                    <Row>
                        <Col xs={12}>
                            <b><Translate id="group-management.subject-level-name" options={{ renderInnerHtml: true }} /></b>
                            <br />
                            <small><Translate id="group-management.subject-level-instruction" options={{ renderInnerHtml: true }} /></small>
                        </Col>
                        <Col>
                            <Select
                                classNamePrefix="mantine-like"
                                className="mantine-like__control"
                                value={subjectsLevelsSelected}
                                placeholder={""}
                                options={subjectsOptions}
                                isDisabled={subscriptionSelected == null || subjectsOptions.length === 0}
                                onChange={handleChangeSubjectsLevels}
                            />
                        </Col>
                    </Row>

                    {showMessage ?
                        <div className="content" dangerouslySetInnerHTML={{ __html: errorMessage }} />
                        :
                        null
                    }
                </ModalBody>
                <ModalFooter>
                    <p className="text-right">

                        <Button disabled={!groupName.trim() || loading || saving || !subjectsLevelsSelected || !subscriptions.length} color="success" onClick={toogleYes}>
                            {saving ?
                                <BeatLoader
                                    size={10}
                                    color={"#fff"}

                                /> : null}{' '}
                            <FontAwesomeIcon icon={faSave} />{' '}<Translate id="group-management.add-group" options={{ renderInnerHtml: true }} />
                        </Button>{' '}
                        <Button color="secondary" onClick={toogleNo}>
                            <Translate id="group-management.close" />
                        </Button>
                    </p>
                </ModalFooter>
            </Modal>
        </div>
    );
};

export default ModalGroupNameCreator;
