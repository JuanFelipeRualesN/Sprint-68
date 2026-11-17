import React from 'react';
import { Row, Col, FormGroup, Label, Input } from 'reactstrap';
import { useTranslation } from 'react-i18next';
import { readApiField as read } from '../../../../../../utils/utils';
import QuotaTargetSearch from '../../../../../shared-module-components/selector/quota-target-search';

// Scope and target filters for the override list
const QuotaLimitFilters = ({ scopeFilter, scopeIdFilter, onScopeChange, onTargetChange, roles, roleLabel, scopes }) => {
    const { t } = useTranslation(['profile', 'common']);

    return (
        <Row className="mb-2">
            <Col xs={12} md={3}>
                <FormGroup>
                    <Label className="label-notifications" for="limit-filter-scope">
                        {t('profile:module-settings.ai-quotas-limit-filter-scope')}
                    </Label>
                    <Input
                        style={{ fontSize: '14px' }}
                        className="styled-inputs"
                        type="select"
                        id="limit-filter-scope"
                        value={scopeFilter}
                        onChange={(e) => onScopeChange(e.target.value)}
                    >
                        <option value="">
                            {t('profile:module-settings.ai-quotas-limit-filter-all')}
                        </option>
                        {scopes.map((scope) => (
                            <option key={scope} value={scope}>
                                {t(`profile:module-settings.ai-quotas-scope-${scope.toLowerCase()}`)}
                            </option>
                        ))}
                    </Input>
                </FormGroup>
            </Col>

            {scopeFilter && (
                <Col xs={12} md={4}>
                    <FormGroup>
                        <Label className="label-notifications" for="limit-filter-target">
                            {t(`profile:module-settings.ai-quotas-scope-${scopeFilter.toLowerCase()}`)}
                        </Label>
                        {scopeFilter === 'Role' ? (
                            <Input
                                style={{ fontSize: '14px' }}
                                className="styled-inputs"
                                type="select"
                                id="limit-filter-target"
                                value={scopeIdFilter}
                                onChange={(e) => onTargetChange(e.target.value)}
                            >
                                <option value="">
                                    {t('profile:module-settings.ai-quotas-limit-filter-all')}
                                </option>
                                {roles.map((role) => (
                                    <option key={read(role, 'Id', '')} value={read(role, 'Id', '')}>
                                        {roleLabel(read(role, 'Id', ''))}
                                    </option>
                                ))}
                            </Input>
                        ) : (
                            <QuotaTargetSearch
                                scope={scopeFilter}
                                value={scopeIdFilter}
                                onChange={onTargetChange}
                            />
                        )}
                    </FormGroup>
                </Col>
            )}
        </Row>
    );
};

export default QuotaLimitFilters;
