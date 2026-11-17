import React from 'react';
import { Row, Col, FormGroup, Label, Input, Button } from 'reactstrap';
import { useTranslation } from 'react-i18next';
import { readApiField as read } from '../../../../../../utils/utils';
import QuotaTargetSearch from '../../../../../shared-module-components/selector/quota-target-search';

// Form for creating or editing a quota limit
const QuotaLimitForm = ({ draft, onDraftChange, onSave, onCancel, roles, resources, roleLabel, resourceLabel, targetNames, scopes, periodTypes, loading }) => {
    const { t } = useTranslation(['profile', 'common']);

    return (
        <>
            <Row>
                <Col xs={12} md={3}>
                    <FormGroup>
                        <Label className="label-notifications" for="limit-scope">
                            {t('profile:module-settings.ai-quotas-limit-scope')}
                        </Label>
                        <Input
                            style={{ fontSize: '14px' }}
                            className="styled-inputs"
                            type="select"
                            id="limit-scope"
                            value={draft.scope}
                            onChange={(e) => {
                                onDraftChange('scope', e.target.value);
                                onDraftChange('scopeId', '');
                            }}
                        >
                            {scopes.map((scope) => (
                                <option key={scope} value={scope}>
                                    {t(`profile:module-settings.ai-quotas-scope-${scope.toLowerCase()}`)}
                                </option>
                            ))}
                        </Input>
                    </FormGroup>
                </Col>

                <Col xs={12} md={3}>
                    <FormGroup>
                        <Label className="label-notifications" for="limit-target">
                            {t(`profile:module-settings.ai-quotas-scope-${draft.scope.toLowerCase()}`)}
                        </Label>
                        {draft.scope === 'Role' ? (
                            <Input
                                style={{ fontSize: '14px' }}
                                className="styled-inputs"
                                type="select"
                                id="limit-target"
                                value={draft.scopeId}
                                onChange={(e) => onDraftChange('scopeId', e.target.value)}
                            >
                                <option value="">
                                    {t('profile:module-settings.ai-quotas-limit-select')}
                                </option>
                                {roles.map((role) => (
                                    <option key={read(role, 'Id', '')} value={read(role, 'Id', '')}>
                                        {roleLabel(read(role, 'Id', ''))}
                                    </option>
                                ))}
                            </Input>
                        ) : (
                            // Component for searching and selecting a User or School target
                            <QuotaTargetSearch
                                scope={draft.scope}
                                value={draft.scopeId}
                                valueLabel={targetNames?.[`${draft.scope}:${draft.scopeId}`]?.name}
                                onChange={(id) => onDraftChange('scopeId', id)}
                            />
                        )}
                    </FormGroup>
                </Col>

                <Col xs={12} md={3}>
                    <FormGroup>
                        <Label className="label-notifications" for="limit-resource">
                            {t('profile:module-settings.ai-quotas-limit-resource')}
                        </Label>
                        <Input
                            style={{ fontSize: '14px' }}
                            className="styled-inputs"
                            type="select"
                            id="limit-resource"
                            value={draft.resourceTypeCode}
                            onChange={(e) => onDraftChange('resourceTypeCode', e.target.value)}
                        >
                            <option value="">
                                {t('profile:module-settings.ai-quotas-limit-select')}
                            </option>
                            {(resources ?? []).map((resource) => (
                                <option
                                    key={resource.resourceTypeCode}
                                    value={resource.resourceTypeCode}
                                >
                                    {resourceLabel(resource.resourceTypeCode, resource.name)}
                                </option>
                            ))}
                        </Input>
                    </FormGroup>
                </Col>

                <Col xs={12} md={3}>
                    <FormGroup>
                        <Label className="label-notifications" for="limit-max">
                            {t('profile:module-settings.ai-quotas-limit-max')}
                        </Label>
                        <Input
                            style={{ fontSize: '14px' }}
                            className="styled-inputs"
                            type="number"
                            min={0}
                            id="limit-max"
                            value={draft.maxAmount}
                            onChange={(e) => onDraftChange('maxAmount', e.target.value)}
                        />
                    </FormGroup>
                </Col>
            </Row>

            <Row>
                <Col xs={12} md={3}>
                    <FormGroup>
                        <Label className="label-notifications" for="limit-period">
                            {t('profile:module-settings.ai-quotas-period-type')}
                        </Label>
                        <Input
                            style={{ fontSize: '14px' }}
                            className="styled-inputs"
                            type="select"
                            id="limit-period"
                            value={draft.periodType}
                            onChange={(e) => onDraftChange('periodType', e.target.value)}
                        >
                            {periodTypes.map((period) => (
                                <option key={period} value={period}>
                                    {t(`profile:module-settings.ai-quotas-period-${period.toLowerCase()}`)}
                                </option>
                            ))}
                        </Input>
                    </FormGroup>
                </Col>

                {draft.periodType === 'Custom' && (
                    <Col xs={12} md={3}>
                        <FormGroup>
                            <Label className="label-notifications" for="limit-days">
                                {t('profile:module-settings.ai-quotas-custom-days')}
                            </Label>
                            <Input
                                style={{ fontSize: '14px' }}
                                className="styled-inputs"
                                type="number"
                                min={1}
                                max={999}
                                id="limit-days"
                                value={draft.customPeriodDays}
                                onChange={(e) => onDraftChange('customPeriodDays', e.target.value)}
                            />
                        </FormGroup>
                    </Col>
                )}

                <Col xs={12} md={2} className="d-flex align-items-center">
                    <FormGroup check className="mb-0">
                        <Input
                            style={{ fontSize: '14px' }}
                            type="checkbox"
                            id="limit-rollover"
                            checked={draft.rolloverEnabled}
                            onChange={(e) => onDraftChange('rolloverEnabled', e.target.checked)}
                        />
                        <Label check className="label-notifications pt-0" for="limit-rollover">
                            {t('profile:module-settings.ai-quotas-rollover')}
                        </Label>
                    </FormGroup>
                </Col>

                {draft.rolloverEnabled && (
                    <Col xs={12} md={3}>
                        <FormGroup>
                            <Label className="label-notifications" for="limit-rollover-max">
                                {t('profile:module-settings.ai-quotas-rollover-max')}
                            </Label>
                            <Input
                                style={{ fontSize: '14px' }}
                                className="styled-inputs"
                                type="number"
                                min={0}
                                id="limit-rollover-max"
                                placeholder={t('profile:module-settings.ai-quotas-rollover-max-placeholder')}
                                value={draft.rolloverMaxAmount}
                                onChange={(e) => onDraftChange('rolloverMaxAmount', e.target.value)}
                            />
                        </FormGroup>
                    </Col>
                )}

                <Col xs={12} md={3} className="d-flex align-items-center">
                    <FormGroup check className="mb-0">
                        <Input
                            style={{ fontSize: '14px' }}
                            type="checkbox"
                            id="limit-enabled"
                            checked={draft.enabled}
                            onChange={(e) => onDraftChange('enabled', e.target.checked)}
                        />
                        <Label check className="label-notifications pt-0" for="limit-enabled">
                            {t('profile:module-settings.ai-quotas-limit-enabled')}
                        </Label>
                    </FormGroup>
                </Col>
            </Row>

            <Row className="mb-3">
                <Col className="pl-0">
                    <Button color="primary" onClick={onSave} disabled={loading}>
                        {t('profile:module-settings.ai-quotas-limit-save')}
                    </Button>
                    {draft.scopeId !== '' && (
                        <Button className="text-decoration-none" style={{ fontSize: '14px', color: '#c0392b', fontWeight: '600' }} color="link" onClick={onCancel} disabled={loading}>
                            {t('profile:module-settings.ai-quotas-limit-cancel')}
                        </Button>
                    )}
                </Col>
            </Row>
        </>
    );
};

export default QuotaLimitForm;
