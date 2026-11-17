import React, { useState, useEffect, useRef } from 'react';
import { Row, Col, Alert, Accordion, AccordionItem, AccordionHeader, AccordionBody } from 'reactstrap';
import { useTranslation } from 'react-i18next';
import Swal from 'sweetalert2';
import { getQuotaLimits, saveQuotaLimit, deleteQuotaLimit, getQuotasSettings } from '../../../../../api/ai-module';
import { getRoles, getUsersAdvancedAdmin } from '../../../../../api/users';
import { getSchoolEntityDetails } from '../../../../../api/schools';
import { readApiField as read } from '../../../../../utils/utils';
import LoadingFullWindow from '../../../../shared-module-components/loader-windows/loader-full-window';
import QuotaLimitForm from './components/quota-limit-form';
import QuotaLimitFilters from './components/quota-limit-filters';
import QuotaLimitTable from './components/quota-limit-table';
import useAccordionLoad from './use-accordion-load';
import '../style.css';

const PERIOD_TYPES = ['Daily', 'Weekly', 'Monthly', 'Custom'];

const SCOPES = ['Role', 'User', 'School'];

const emptyDraft = {
    resourceTypeCode: '',
    scope: 'Role',
    scopeId: '',
    enabled: true,
    maxAmount: '',
    periodType: 'Monthly',
    customPeriodDays: '',
    rolloverEnabled: false,
    rolloverMaxAmount: ''
};

// Component for managing exceptions to the general limit, per role, school or user. 
// The API applies the most specific match (User -> School -> Role -> Global), so removing one falls back a level.
const AccordionAiQuotaLimits = ({ quotasEnabledOverride }) => {
    const [limits, setLimits] = useState([]);
    const [roles, setRoles] = useState([]);
    const [resources, setResources] = useState([]);
    const [quotasEnabled, setQuotasEnabled] = useState(null);
    const [draft, setDraft] = useState(emptyDraft);
    const [loading, setLoading] = useState(false);
    const [scopeFilter, setScopeFilter] = useState('');
    const [scopeIdFilter, setScopeIdFilter] = useState('');
    const [currentPage, setCurrentPage] = useState(0);
    const [totalPages, setTotalPages] = useState(1);
    const targetNamesRef = useRef({});
    const [targetNames, setTargetNames] = useState({});
    const { open, toggle, opened, openCount } = useAccordionLoad('ai-quota-limits');
    const { t } = useTranslation(['profile', 'common']);
    const limiter = 10

    useEffect(() => {
        if (opened) loadRoles();
    }, [opened]);

    useEffect(() => {
        if (openCount > 0) loadQuotaSettings();
    }, [openCount]);

    useEffect(() => {
        if (typeof quotasEnabledOverride === 'boolean') setQuotasEnabled(quotasEnabledOverride);
    }, [quotasEnabledOverride]);

    useEffect(() => {
        if (opened) loadLimits();
    }, [opened, scopeFilter, scopeIdFilter, currentPage]);

    const loadRoles = async () => {
        try {
            const response = await getRoles();
            setRoles(Array.isArray(response.data) ? response.data : []);
        } catch (error) {
            Swal.fire({ text: t('common:general.unsuccessful-transaction'), icon: 'error' });
        }
    };

    const loadQuotaSettings = async () => {
        try {
            const response = await getQuotasSettings();

            setQuotasEnabled(read(response.data, 'QuotasEnabled', false));

            setResources((read(response.data, 'Resources', []) ?? []).map((resource) => ({
                resourceTypeCode: read(resource, 'ResourceTypeCode', ''),
                name: read(resource, 'Name', '')
            })));
        } catch (error) {
            Swal.fire({ text: t('common:general.unsuccessful-transaction'), icon: 'error' });
        }
    };

    // Loads one page of overrides
    const loadLimits = async () => {
        setLoading(true);
        try {
            const response = await getQuotaLimits(scopeFilter || undefined, scopeIdFilter || undefined, currentPage, limiter);

            const items = read(response.data, 'Items', []) ?? [];
            setLimits(items);
            setTotalPages(read(response.data, 'TotalPages', 1) ?? 1);
            resolveTargetNames(items);
        } catch (error) {
            Swal.fire({ text: t('common:general.unsuccessful-transaction'), icon: 'error' });
        } finally {
            setLoading(false);
        }
    };

    // Resolves the User/School target ids on the visible page to display names
    const resolveTargetNames = async (pageItems) => {
        const pending = [];

        for (const limit of pageItems) {
            const scope = read(limit, 'Scope', '');
            const id = String(read(limit, 'ScopeId', ''));
            if (!id || (scope !== 'User' && scope !== 'School')) continue;
            const key = `${scope}:${id}`;

            if (targetNamesRef.current[key] !== undefined) continue;
            targetNamesRef.current[key] = { status: 'loading' };
            pending.push({ scope, id, key });
        }
        if (pending.length === 0) return;

        await Promise.all(pending.map(async ({ scope, id, key }) => {
            try {
                let name = null;

                if (scope === 'User') {
                    const response = await getUsersAdvancedAdmin(id, null, null, null, null, null, null, null, null, 0, 1, false, null);
                    const user = (read(response.data, 'UserInfo', []) ?? [])[0];
                    name = user ? read(user, 'Username', null) : null;
                } else {
                    const response = await getSchoolEntityDetails(id);
                    const entity = (read(response.data, 'Entities', []) ?? [])[0];
                    name = entity ? read(entity, 'Name', null) : null;
                }

                targetNamesRef.current[key] = name ? { status: 'done', name } : { status: 'failed' };
            } catch {
                targetNamesRef.current[key] = { status: 'failed' };
            }
        }));

        setTargetNames({ ...targetNamesRef.current });
    };

    const updateDraft = (field, value) => setDraft((current) => ({ ...current, [field]: value }));

    // Clears the target filter when the scope changes
    const handleScopeFilterChange = (value) => {
        setScopeFilter(value);
        setScopeIdFilter('');
        setCurrentPage(0);
    };

    const handleTargetFilterChange = (id) => {
        setScopeIdFilter(id || '');
        setCurrentPage(0);
    };

    const handlePageClick = (page) => setCurrentPage(page);

    const roleLabel = (code) => {
        const role = roles.find((item) => String(read(item, 'Id', '')) === String(code));
        return role ? read(role, 'Desc', code) : code;
    };

    const resourceLabel = (code, fallback) => {
        if (!code) return fallback ?? '';
        const key = `profile:module-settings.ai-quotas-resource-${String(code).toLowerCase()}`;
        const label = t(key);
        return label !== key ? label : (fallback ?? code);
    };

    // Validates the draft
    const validate = () => {
        if (!draft.resourceTypeCode) return t('profile:module-settings.ai-quotas-limit-missing-resource');
        if (!String(draft.scopeId).trim()) return t('profile:module-settings.ai-quotas-limit-missing-target');

        const maxAmount = Number(draft.maxAmount);
        if (draft.maxAmount === '' || !Number.isInteger(maxAmount) || maxAmount < 0) {
            return t('profile:module-settings.ai-quotas-limit-invalid-max');
        }

        if (draft.periodType === 'Custom') {
            const days = Number(draft.customPeriodDays);
            if (draft.customPeriodDays === '' || !Number.isInteger(days) || days <= 0) {
                return t('profile:module-settings.ai-quotas-limit-invalid-days');
            }
        }

        if (draft.rolloverEnabled && draft.rolloverMaxAmount !== '') {
            const rolloverMax = Number(draft.rolloverMaxAmount);
            if (!Number.isInteger(rolloverMax) || rolloverMax < 0) {
                return t('profile:module-settings.ai-quotas-limit-invalid-rollover');
            }
        }

        return null;
    };

    const handleSave = async () => {
        const validationError = validate();

        if (validationError) {
            Swal.fire({ text: validationError, icon: 'warning' });
            return;
        }

        setLoading(true);
        try {
            await saveQuotaLimit({
                resourceTypeCode: draft.resourceTypeCode,
                scope: draft.scope,
                scopeId: String(draft.scopeId).trim(),
                enabled: draft.enabled,
                maxAmount: Number(draft.maxAmount),
                periodType: draft.periodType,
                customPeriodDays: draft.periodType === 'Custom' ? Number(draft.customPeriodDays) : null,
                rolloverEnabled: draft.rolloverEnabled,
                rolloverMaxAmount: draft.rolloverEnabled && draft.rolloverMaxAmount !== '' ? Number(draft.rolloverMaxAmount) : null
            });

            setDraft(emptyDraft);
            await loadLimits();

            Swal.fire({ text: t('profile:module-settings.ai-quotas-limit-saved'), icon: 'success' });
        } catch (error) {
            const message = read(error?.response?.data, 'Error', null) ?? t('common:general.unsuccessful-transaction');
            Swal.fire({ text: message, icon: 'error' });
        } finally {
            setLoading(false);
        }
    };

    // Loads an existing override back into the draft so it can be edited
    const handleEdit = (limit) => {
        setDraft({
            resourceTypeCode: read(limit, 'ResourceTypeCode', ''),
            scope: read(limit, 'Scope', 'Role'),
            scopeId: read(limit, 'ScopeId', ''),
            enabled: read(limit, 'Enabled', true),
            maxAmount: String(read(limit, 'MaxAmount', 0) ?? 0),
            periodType: read(limit, 'PeriodType', 'Monthly'),
            customPeriodDays: read(limit, 'CustomPeriodDays', null) ?? '',
            rolloverEnabled: read(limit, 'RolloverEnabled', false),
            rolloverMaxAmount: read(limit, 'RolloverMaxAmount', null) ?? ''
        });
    };

    // Deletes an existing quota limit
    const handleDelete = async (limit) => {
        const confirmation = await Swal.fire({
            text: t('profile:module-settings.ai-quotas-limit-delete-confirm'),
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: t('profile:module-settings.ai-quotas-limit-delete'),
            cancelButtonText: t('profile:module-settings.ai-quotas-limit-cancel')
        });

        if (!confirmation.isConfirmed) return;

        setLoading(true);
        try {
            await deleteQuotaLimit(read(limit, 'Id', 0));
            await loadLimits();
            Swal.fire({ text: t('profile:module-settings.ai-quotas-limit-deleted'), icon: 'success' });
        } catch (error) {
            const message = read(error?.response?.data, 'Error', null) ?? t('common:general.unsuccessful-transaction');
            Swal.fire({ text: message, icon: 'error' });
        } finally {
            setLoading(false);
        }
    };

    return (
        <>
            <Row>
                <Col>
                    <Accordion open={open} toggle={toggle}>
                        <AccordionItem>
                            <AccordionHeader targetId="ai-quota-limits">
                                <Row>
                                    <Col xs={12}>
                                        <span className="configuration-card-header-title">
                                            {t('profile:module-settings.ai-quotas-limits')}
                                        </span>
                                    </Col>
                                    <Col xs={12}>
                                        <small>{t('profile:module-settings.ai-quotas-limits-explanation')}</small>
                                    </Col>
                                </Row>
                            </AccordionHeader>
                            <AccordionBody accordionId="ai-quota-limits">
                                {quotasEnabled === false && (
                                    <Alert style={{ fontSize: '14px' }} color="warning">
                                        {t('profile:module-settings.ai-quotas-limits-disabled-notice')}
                                    </Alert>
                                )}

                                <Alert style={{ fontSize: '14px' }} color="info">
                                    {t('profile:module-settings.ai-quotas-limit-reset-notice')}
                                </Alert>

                                <QuotaLimitForm
                                    draft={draft}
                                    onDraftChange={updateDraft}
                                    onSave={handleSave}
                                    onCancel={() => setDraft(emptyDraft)}
                                    roles={roles}
                                    resources={resources}
                                    roleLabel={roleLabel}
                                    resourceLabel={resourceLabel}
                                    targetNames={targetNames}
                                    scopes={SCOPES}
                                    periodTypes={PERIOD_TYPES}
                                    loading={loading}
                                />

                                <hr />

                                <QuotaLimitFilters
                                    scopeFilter={scopeFilter}
                                    scopeIdFilter={scopeIdFilter}
                                    onScopeChange={handleScopeFilterChange}
                                    onTargetChange={handleTargetFilterChange}
                                    roles={roles}
                                    roleLabel={roleLabel}
                                    scopes={SCOPES}
                                />

                                <QuotaLimitTable
                                    limits={limits}
                                    targetNames={targetNames}
                                    roleLabel={roleLabel}
                                    resourceLabel={resourceLabel}
                                    onEdit={handleEdit}
                                    onDelete={handleDelete}
                                    currentPage={currentPage}
                                    totalPages={totalPages}
                                    onPageChange={handlePageClick}
                                />
                            </AccordionBody>
                        </AccordionItem>
                    </Accordion>
                </Col>
            </Row>
            {loading ? <LoadingFullWindow /> : null}
        </>
    );
};

export default AccordionAiQuotaLimits;
