import React, { useState, useEffect } from 'react';
import { Row, Col, Card, CardHeader, CardBody, FormGroup, Label, Input, Button, Alert, Accordion, AccordionItem, AccordionHeader, AccordionBody } from 'reactstrap';
import { useTranslation } from 'react-i18next';
import Swal from 'sweetalert2';
import { getQuotasSettings, updateQuotasSettings } from '../../../../../api/ai-module';
import { readApiField as read } from '../../../../../utils/utils';
import LoadingFullWindow from '../../../../shared-module-components/loader-windows/loader-full-window';
import useAccordionLoad from './use-accordion-load';
import '../style.css';

const PERIOD_TYPES = ['Daily', 'Weekly', 'Monthly', 'Custom'];

const IOSSwitch = ({ id, checked, onChange, disabled }) => (
    <div className="form-check form-switch d-inline-flex align-items-center mb-0 mr-2">
        <input className="form-check-input mt-0" type="checkbox" role="switch"
            id={id} checked={checked} onChange={onChange} disabled={disabled}
            style={{
                width: '3em', height: '1.5em', cursor: disabled ? 'not-allowed' : 'pointer',
                ...(checked ? { backgroundColor: '#68bb57', borderColor: '#68bb57' } : {})
            }} />
    </div>
);

// Component for the general quota configuration: the quota and AI master switches, and the
// per-resource allowance every user gets.
const AccordionAiQuotasSettings = ({ onQuotasEnabledChange }) => {
    const { open, toggle, opened } = useAccordionLoad('ai-quotas');
    const [quotasEnabled, setQuotasEnabled] = useState(false);
    const [aiEnabled, setAiEnabled] = useState(true);
    const [savedAiEnabled, setSavedAiEnabled] = useState(true);
    const [resources, setResources] = useState([]);
    const [loading, setLoading] = useState(false);
    const [loadFailed, setLoadFailed] = useState(false);
    const { t } = useTranslation(['profile', 'common']);

    const resourceLabel = (code, fallback) => {
        if (!code) return fallback ?? '';
        const key = `profile:module-settings.ai-quotas-resource-${String(code).toLowerCase()}`;
        const label = t(key);
        return label !== key ? label : (fallback ?? code);
    };

    const unitLabel = (unit) => {
        if (!unit) return '';
        const key = `profile:module-settings.ai-quotas-unit-${String(unit).toLowerCase()}`;
        const label = t(key);
        return label !== key ? label : unit;
    };

    useEffect(() => {
        if (opened) loadSettings();
    }, [opened]);

    // Loads the current quota configuration
    const loadSettings = async () => {
        setLoading(true);
        try {
            const response = await getQuotasSettings();
            applySettings(response.data);
            setLoadFailed(false);
        } catch (error) {
            setLoadFailed(true);
        } finally {
            setLoading(false);
        }
    };

    // Maps a response into the shape this form edits
    const toFormResource = (resource) => ({
        resourceTypeCode: read(resource, 'ResourceTypeCode', ''),
        name: read(resource, 'Name', ''),
        unit: read(resource, 'Unit', ''),
        enabled: read(resource, 'Enabled', false),
        maxAmount: String(read(resource, 'MaxAmount', 0) ?? 0),
        periodType: read(resource, 'PeriodType', 'Monthly'),
        customPeriodDays: read(resource, 'CustomPeriodDays', null) ?? '',
        rolloverEnabled: read(resource, 'RolloverEnabled', false),
        rolloverMaxAmount: read(resource, 'RolloverMaxAmount', null) ?? ''
    });

    // Fills the form from a configuration payload
    const applySettings = (data) => {
        const loadedAiEnabled = read(data, 'AiEnabled', true);
        const loadedQuotasEnabled = read(data, 'QuotasEnabled', false);
        setQuotasEnabled(loadedQuotasEnabled);
        setAiEnabled(loadedAiEnabled);
        setSavedAiEnabled(loadedAiEnabled);
        setResources((read(data, 'Resources', []) ?? []).map(toFormResource));

        if (onQuotasEnabledChange) onQuotasEnabledChange(loadedQuotasEnabled);
    };

    // Updates a single field of one resource
    const handleResourceChange = (code, field, value) => {
        setResources((current) => current.map((resource) =>
            resource.resourceTypeCode === code ? { ...resource, [field]: value } : resource
        ));
    };

    const validate = () => {
        for (const resource of resources) {
            if (!resource.enabled) continue;

            const maxAmount = Number(resource.maxAmount);
            if (resource.maxAmount === '' || !Number.isInteger(maxAmount) || maxAmount < 0) {
                return t('profile:module-settings.ai-quotas-invalid-max-amount', { resource: resourceLabel(resource.resourceTypeCode, resource.name) });
            }

            if (resource.periodType === 'Custom') {
                const days = Number(resource.customPeriodDays);
                if (resource.customPeriodDays === '' || !Number.isInteger(days) || days <= 0) {
                    return t('profile:module-settings.ai-quotas-invalid-custom-days', { resource: resourceLabel(resource.resourceTypeCode, resource.name) });
                }
            }

            if (resource.rolloverEnabled && resource.rolloverMaxAmount !== '') {
                const rolloverMax = Number(resource.rolloverMaxAmount);
                if (!Number.isInteger(rolloverMax) || rolloverMax < 0) {
                    return t('profile:module-settings.ai-quotas-invalid-rollover-max', { resource: resourceLabel(resource.resourceTypeCode, resource.name) });
                }
            }
        }
        return null;
    };

    // Converts the edited form state back into the payload the API expects.
    const toPayload = () => resources.map((resource) => ({
        resourceTypeCode: resource.resourceTypeCode,
        enabled: resource.enabled,
        maxAmount: Number(resource.maxAmount || 0),
        periodType: resource.periodType,
        customPeriodDays: resource.periodType === 'Custom' ? Number(resource.customPeriodDays) : null,
        rolloverEnabled: resource.rolloverEnabled,
        rolloverMaxAmount: resource.rolloverEnabled && resource.rolloverMaxAmount !== ''
            ? Number(resource.rolloverMaxAmount)
            : null
    }));

    const handleSubmit = async () => {
        const validationError = validate();
        if (validationError) {
            Swal.fire({ text: validationError, icon: 'warning' });
            return;
        }

        setLoading(true);
        try {
            const response = await updateQuotasSettings(quotasEnabled, aiEnabled, toPayload());
            applySettings(response.data);

            Swal.fire({ text: t('profile:module-settings.ai-quotas-saved'), icon: 'success' });
        } catch (error) {
            const message = read(error?.response?.data, 'Error', null) ?? t('common:general.unsuccessful-transaction');
            Swal.fire({ text: message, icon: 'error' });
        } finally {
            setLoading(false);
        }
    };

    // If quotas are enabled, but none of the resources are enforced, show a warning
    const noResourceEnforced = quotasEnabled && resources.length > 0
        && !resources.some((resource) => resource.enabled);

    return (
        <div>
            <Row>
                <Col>
                    <Accordion open={open} toggle={toggle}>
                        <AccordionItem>
                            <AccordionHeader targetId="ai-quotas">
                                <Row>
                                    <Col xs={12}>
                                        <span className="configuration-card-header-title">
                                            {t('profile:module-settings.ai-quotas-settings')}
                                        </span>
                                    </Col>
                                    <Col xs={12}>
                                        <small>{t('profile:module-settings.ai-quotas-settings-explanation')}</small>
                                    </Col>
                                </Row>
                            </AccordionHeader>
                            <AccordionBody accordionId="ai-quotas">
                                {loadFailed ? (
                                <Alert style={{ fontSize: '14px' }} color="danger">
                                    {t('profile:module-settings.ai-quotas-load-error')}
                                </Alert>
                            ) : (
                                <>
                                    <Row className="mb-2">
                                        <Col className="d-flex align-items-center pl-0">
                                            <IOSSwitch
                                                id="ai-enabled"
                                                checked={aiEnabled}
                                                onChange={(e) => setAiEnabled(e.target.checked)}
                                            />
                                            <Label className="label-notifications pt-2" for="ai-enabled">
                                                {t('profile:module-settings.ai-quotas-ai-enabled')}
                                            </Label>
                                        </Col>
                                    </Row>
                                    <Row className="mb-3">
                                        <Col xs={12}>
                                            <small>{t('profile:module-settings.ai-quotas-ai-enabled-explanation')}</small>
                                        </Col>
                                    </Row>

                                    {!aiEnabled && (
                                        <Alert style={{ fontSize: '14px' }} color="danger">
                                            {savedAiEnabled
                                                ? t('profile:module-settings.ai-quotas-ai-will-disable')
                                                : t('profile:module-settings.ai-quotas-ai-disabled')}
                                        </Alert>
                                    )}

                                    <hr />

                                    <Row className="mb-3">
                                        <Col className="d-flex align-items-center pl-0">
                                            <IOSSwitch
                                                id="quotas-enabled"
                                                checked={quotasEnabled}
                                                onChange={(e) => setQuotasEnabled(e.target.checked)}
                                            />
                                            <Label className="label-notifications pt-2" for="quotas-enabled">
                                                {t('profile:module-settings.ai-quotas-enabled')}
                                            </Label>
                                        </Col>
                                    </Row>
                                    <Row>
                                        <Col xs={12}>
                                            <small>{t('profile:module-settings.ai-quotas-enabled-explanation')}</small>
                                        </Col>
                                    </Row>

                                    {noResourceEnforced && (
                                        <Alert color="warning" style={{ fontSize: '14px' }} className="mt-3">
                                            {t('profile:module-settings.ai-quotas-none-enforced')}
                                        </Alert>
                                    )}

                                    <hr />

                                    {resources.length === 0 ? (
                                        <small>{t('profile:module-settings.ai-quotas-no-resources')}</small>
                                    ) : (
                                        <Row>
                                            {resources.map((resource) => (
                                                <Col xs={12} md={6} key={resource.resourceTypeCode}>
                                                    <Card className="mb-3">
                                                        <CardHeader>
                                                            <div className="d-flex align-items-center">
                                                                <IOSSwitch
                                                                    id={`enabled-${resource.resourceTypeCode}`}
                                                                    checked={resource.enabled}
                                                                    disabled={!quotasEnabled}
                                                                    onChange={(e) => handleResourceChange(
                                                                        resource.resourceTypeCode, 'enabled', e.target.checked)}
                                                                />
                                                                <span className="configuration-card-header-title">
                                                                    {resourceLabel(resource.resourceTypeCode, resource.name)}
                                                                </span>
                                                            </div>
                                                        </CardHeader>
                                                        <CardBody className="px-3">
                                                            <FormGroup>
                                                                <Label
                                                                    className="label-notifications"
                                                                    for={`max-${resource.resourceTypeCode}`}
                                                                >
                                                                    {t('profile:module-settings.ai-quotas-max-amount', {
                                                                        unit: unitLabel(resource.unit)
                                                                    })}
                                                                </Label>
                                                                <Input
                                                                    style={{ fontSize: '14px' }}
                                                                    className="styled-inputs"
                                                                    type="number"
                                                                    min={0}
                                                                    id={`max-${resource.resourceTypeCode}`}
                                                                    value={resource.maxAmount}
                                                                    disabled={!quotasEnabled || !resource.enabled}
                                                                    onChange={(e) => handleResourceChange(
                                                                        resource.resourceTypeCode, 'maxAmount', e.target.value)}
                                                                />
                                                            </FormGroup>

                                                            <FormGroup>
                                                                <Label
                                                                    className="label-notifications"
                                                                    for={`period-${resource.resourceTypeCode}`}
                                                                >
                                                                    {t('profile:module-settings.ai-quotas-period-type')}
                                                                </Label>
                                                                <Input
                                                                    style={{ fontSize: '14px' }}
                                                                    className="styled-inputs"
                                                                    type="select"
                                                                    id={`period-${resource.resourceTypeCode}`}
                                                                    value={resource.periodType}
                                                                    disabled={!quotasEnabled || !resource.enabled}
                                                                    onChange={(e) => handleResourceChange(
                                                                        resource.resourceTypeCode, 'periodType', e.target.value)}
                                                                >
                                                                    {PERIOD_TYPES.map((period) => (
                                                                        <option key={period} value={period}>
                                                                            {t(`profile:module-settings.ai-quotas-period-${period.toLowerCase()}`)}
                                                                        </option>
                                                                    ))}
                                                                </Input>
                                                            </FormGroup>

                                                            {resource.periodType === 'Custom' && (
                                                                <FormGroup>
                                                                    <Label
                                                                        className="label-notifications"
                                                                        for={`days-${resource.resourceTypeCode}`}
                                                                    >
                                                                        {t('profile:module-settings.ai-quotas-custom-days')}
                                                                    </Label>
                                                                    <Input
                                                                        style={{ fontSize: '14px' }}
                                                                        className="styled-inputs"
                                                                        type="number"
                                                                        min={1}
                                                                        id={`days-${resource.resourceTypeCode}`}
                                                                        value={resource.customPeriodDays}
                                                                        disabled={!quotasEnabled || !resource.enabled}
                                                                        onChange={(e) => handleResourceChange(
                                                                            resource.resourceTypeCode, 'customPeriodDays', e.target.value)}
                                                                    />
                                                                </FormGroup>
                                                            )}

                                                            <FormGroup className="d-flex align-items-center">
                                                                <IOSSwitch
                                                                    id={`rollover-${resource.resourceTypeCode}`}
                                                                    checked={resource.rolloverEnabled}
                                                                    disabled={!quotasEnabled || !resource.enabled}
                                                                    onChange={(e) => handleResourceChange(
                                                                        resource.resourceTypeCode, 'rolloverEnabled', e.target.checked)}
                                                                />
                                                                <Label
                                                                    className="label-notifications pt-2"
                                                                    for={`rollover-${resource.resourceTypeCode}`}
                                                                >
                                                                    {t('profile:module-settings.ai-quotas-rollover')}
                                                                </Label>
                                                            </FormGroup>

                                                            {resource.rolloverEnabled && (
                                                                <FormGroup>
                                                                    <Label
                                                                        className="label-notifications"
                                                                        for={`rollover-max-${resource.resourceTypeCode}`}
                                                                    >
                                                                        {t('profile:module-settings.ai-quotas-rollover-max')}
                                                                    </Label>
                                                                    <Input
                                                                        style={{ fontSize: '14px' }}
                                                                        className="styled-inputs"
                                                                        type="number"
                                                                        min={0}
                                                                        placeholder={t('profile:module-settings.ai-quotas-rollover-max-placeholder')}
                                                                        id={`rollover-max-${resource.resourceTypeCode}`}
                                                                        value={resource.rolloverMaxAmount}
                                                                        disabled={!quotasEnabled || !resource.enabled}
                                                                        onChange={(e) => handleResourceChange(
                                                                            resource.resourceTypeCode, 'rolloverMaxAmount', e.target.value)}
                                                                    />
                                                                    <small>
                                                                        {t('profile:module-settings.ai-quotas-rollover-max-explanation')}
                                                                    </small>
                                                                </FormGroup>
                                                            )}
                                                        </CardBody>
                                                    </Card>
                                                </Col>
                                            ))}
                                        </Row>
                                    )}

                                    <Row>
                                        <Col>
                                            <Button color="primary" onClick={handleSubmit} disabled={loading}>
                                                {t('profile:module-settings.ai-quotas-save')}
                                            </Button>
                                        </Col>
                                    </Row>
                                </>
                            )}
                            </AccordionBody>
                        </AccordionItem>
                    </Accordion>
                </Col>
            </Row>

            {loading ? <LoadingFullWindow /> : null}
        </div>
    );
};

export default AccordionAiQuotasSettings;
