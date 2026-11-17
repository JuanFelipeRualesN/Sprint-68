import React, { useState, useEffect, useRef } from 'react';
import { Row, Col, FormGroup, Label, Input, Table, Accordion, AccordionItem, AccordionHeader, AccordionBody } from 'reactstrap';
import { useTranslation } from 'react-i18next';
import Swal from 'sweetalert2';
import { getQuotaUserUsage, getQuotasSettings } from '../../../../../api/ai-module';
import { getUsersAdvancedAdmin, getRoles } from '../../../../../api/users';
import { getSchoolEntityDetails } from '../../../../../api/schools';
import { readApiField as read } from '../../../../../utils/utils';
import LoadingFullWindow from '../../../../shared-module-components/loader-windows/loader-full-window';
import QuotaTargetSearch from '../../../../shared-module-components/selector/quota-target-search';
import PaginationCustom from '../../../../shared-module-components/datatables/pagination';
import useAccordionLoad from './use-accordion-load';
import '../style.css';

const USAGE_SCOPES = ['School', 'Role', 'Global'];

const SKELETON_WIDTHS = [7, 8, 6, 7];

// Component for listing who is spending AI this period, with the limit governing each one. 
// Reads recorded consumption, since School and Role limits keep no counter of their own.
const AccordionAiQuotaUsage = () => {
    const [usage, setUsage] = useState([]);
    const [resources, setResources] = useState([]);
    const [roles, setRoles] = useState([]);
    const [resourceFilter, setResourceFilter] = useState('');
    const [scopeFilter, setScopeFilter] = useState('');
    const [scopeIdFilter, setScopeIdFilter] = useState('');
    const [userFilter, setUserFilter] = useState('');
    const [loading, setLoading] = useState(false);
    const [currentPage, setCurrentPage] = useState(0);
    const [totalPages, setTotalPages] = useState(1);
    const namesRef = useRef({});
    const [names, setNames] = useState({});
    const { open, toggle, opened } = useAccordionLoad('ai-quota-usage');
    const { t } = useTranslation(['profile', 'common']);
    const limiter = 10

    // Fetch the necessary data when the accordion opens
    useEffect(() => {
        if (!opened) return;

        Promise.all([
            getQuotasSettings().catch(() => ({ data: {} })),
            getRoles().catch(() => ({ data: [] }))
        ]).then(([settingsResponse, rolesResponse]) => {
            setResources((read(settingsResponse.data, 'Resources', []) ?? []).map((resource) => ({
                resourceTypeCode: read(resource, 'ResourceTypeCode', ''),
                name: read(resource, 'Name', '')
            })));
            setRoles(Array.isArray(rolesResponse.data) ? rolesResponse.data : []);
        });
    }, [opened]);

    useEffect(() => {
        if (opened) loadUsage();
    }, [opened, resourceFilter, scopeFilter, scopeIdFilter, userFilter, currentPage]);

    // Load the usage data
    const loadUsage = async () => {
        setLoading(true);
        try {
            const response = await getQuotaUserUsage(resourceFilter, scopeFilter, scopeIdFilter, userFilter, currentPage, limiter);
            const items = read(response.data, 'Items', []) ?? [];
            setUsage(items);
            setTotalPages(read(response.data, 'TotalPages', 1) ?? 1);
            resolveNames(items);
        } catch (error) {
            Swal.fire({ text: t('common:general.unsuccessful-transaction'), icon: 'error' });
        } finally {
            setLoading(false);
        }
    };

    // Resolves the user ids, and the school ids of any school-scoped limit, to display names.
    const resolveNames = async (pageItems) => {
        const pending = [];

        const enqueue = (kind, id) => {
            if (!id) return;
            const key = `${kind}:${id}`;
            if (namesRef.current[key] !== undefined) return;
            namesRef.current[key] = { status: 'loading' };
            pending.push({ kind, id, key });
        };

        for (const row of pageItems) {
            enqueue('User', String(read(row, 'UserId', '')));
            if (read(row, 'LimitScope', '') === 'School') {
                enqueue('School', String(read(row, 'LimitScopeId', '')));
            }
        }

        if (pending.length === 0) return;

        await Promise.all(pending.map(async ({ kind, id, key }) => {
            try {
                let name = null;
                if (kind === 'User') {
                    const response = await getUsersAdvancedAdmin(id, null, null, null, null, null, null, null, null, 0, 1, false, null);
                    const user = (read(response.data, 'UserInfo', []) ?? [])[0];
                    name = user ? read(user, 'Username', null) : null;
                } else {
                    const response = await getSchoolEntityDetails(id);
                    const entity = (read(response.data, 'Entities', []) ?? [])[0];
                    name = entity ? read(entity, 'Name', null) : null;
                }
                namesRef.current[key] = name ? { status: 'done', name } : { status: 'failed' };
            } catch {
                namesRef.current[key] = { status: 'failed' };
            }
        }));

        setNames({ ...namesRef.current });
    };

    // Create a unique key for each row, since a user can have multiple rows for different resources and limits.
    const rowKey = (row) =>
        `${read(row, 'UserId', '')}-${read(row, 'ResourceTypeCode', '')}` + `-${read(row, 'LimitScope', '')}-${read(row, 'LimitScopeId', '')}`;

    const displayName = (kind, id) => {
        const entry = names[`${kind}:${String(id)}`];
        return entry && entry.status === 'done' ? entry.name : String(id);
    };

    const resolvedName = (kind, id) => {
        if (!id) return undefined;
        const entry = names[`${kind}:${String(id)}`];
        return entry && entry.status === 'done' ? entry.name : undefined;
    };

    const handleResourceFilterChange = (value) => {
        setResourceFilter(value);
        setCurrentPage(0);
    };

    // Clear the other filters when the scope changes
    const handleScopeFilterChange = (value) => {
        setScopeFilter(value);
        setScopeIdFilter('');
        setUserFilter('');
        setCurrentPage(0);
    };

    const handleScopeIdFilterChange = (id) => {
        setScopeIdFilter(id || '');
        setCurrentPage(0);
    };

    const handleUserFilterChange = (id) => {
        setUserFilter(id || '');
        setScopeFilter('');
        setScopeIdFilter('');
        setCurrentPage(0);
    };

    const roleLabel = (code) => {
        const role = roles.find((item) => String(read(item, 'Id', '')) === String(code));
        return role ? read(role, 'Desc', code) : code;
    };

    const pageLoading = usage.some((row) => {
        const userEntry = names[`User:${String(read(row, 'UserId', ''))}`];
        if (!userEntry || userEntry.status === 'loading') return true;
        if (read(row, 'LimitScope', '') === 'School') {
            const schoolEntry = names[`School:${String(read(row, 'LimitScopeId', ''))}`];
            return !schoolEntry || schoolEntry.status === 'loading';
        }
        return false;
    });

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


     // Names the limit governing this user
    const describeGoverningLimit = (row) => {
        const scope = read(row, 'LimitScope', null);
        if (!scope) return <span className="text-muted">{t('profile:module-settings.ai-quotas-usage-no-limit')}</span>;

        const scopeLabel = t(`profile:module-settings.ai-quotas-scope-${String(scope).toLowerCase()}`);
        if (scope !== 'School') return scopeLabel;

        return `${displayName('School', read(row, 'LimitScopeId', ''))}`;
    };

    // Consumption against the allowance, with the share used when a limit applies.
    const describeAmount = (row) => {
        const consumed = Number(read(row, 'ConsumedAmount', 0));
        const allocated = read(row, 'AllocatedAmount', null);

        if (allocated === null || allocated === undefined) {
            return consumed.toLocaleString();
        }

        const percent = Number(allocated) > 0
            ? Math.round((consumed / Number(allocated)) * 100)
            : null;

        return (
            <>
                {consumed.toLocaleString()} / {Number(allocated).toLocaleString()}
                {percent !== null && <small className="text-muted">{` (${percent}%)`}</small>}
            </>
        );
    };

    const renderSkeleton = (row, rowIndex) => (
        <tr key={rowKey(row)}>
            {SKELETON_WIDTHS.map((width, index) => (
                <td key={index}>
                    <span
                        className="quota-skeleton-bar"
                        style={{ width: `${width}em`, animationDelay: `${rowIndex * 0.12}s` }}
                        aria-hidden="true"
                    />
                </td>
            ))}
        </tr>
    );

    return (
        <>
            <Row>
                <Col>
                    <Accordion open={open} toggle={toggle}>
                        <AccordionItem>
                            <AccordionHeader targetId="ai-quota-usage">
                                <Row>
                                    <Col xs={12}>
                                        <span className="configuration-card-header-title">
                                            {t('profile:module-settings.ai-quotas-usage')}
                                        </span>
                                    </Col>
                                    <Col xs={12}>
                                        <small>{t('profile:module-settings.ai-quotas-usage-explanation')}</small>
                                    </Col>
                                </Row>
                            </AccordionHeader>
                            <AccordionBody accordionId="ai-quota-usage">
                                <Row className="mb-2">
                                    <Col xs={12} md={4}>
                                        <FormGroup>
                                            <Label className="label-notifications" for="usage-filter-resource">
                                                {t('profile:module-settings.ai-quotas-limit-resource')}
                                            </Label>
                                            <Input
                                                style={{ fontSize: '14px' }}
                                                className="styled-inputs"
                                                type="select"
                                                id="usage-filter-resource"
                                                value={resourceFilter}
                                                onChange={(e) => handleResourceFilterChange(e.target.value)}
                                            >
                                                <option value="">
                                                    {t('profile:module-settings.ai-quotas-limit-filter-all')}
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

                                    <Col xs={12} md={4}>
                                        <FormGroup>
                                            <Label className="label-notifications" for="usage-filter-scope">
                                                {t('profile:module-settings.ai-quotas-usage-filter-scope')}
                                            </Label>
                                            <Input
                                                style={{ fontSize: '14px' }}
                                                className="styled-inputs"
                                                type="select"
                                                id="usage-filter-scope"
                                                value={scopeFilter}
                                                onChange={(e) => handleScopeFilterChange(e.target.value)}
                                            >
                                                <option value="">
                                                    {t('profile:module-settings.ai-quotas-limit-filter-all')}
                                                </option>
                                                {USAGE_SCOPES.map((scope) => (
                                                    <option key={scope} value={scope}>
                                                        {t(`profile:module-settings.ai-quotas-scope-${scope.toLowerCase()}`)}
                                                    </option>
                                                ))}
                                            </Input>
                                        </FormGroup>
                                    </Col>

                                    {scopeFilter && scopeFilter !== 'Global' && (
                                        <Col xs={12} md={4}>
                                            <FormGroup>
                                                <Label className="label-notifications" for="usage-filter-scope-id">
                                                    {t(`profile:module-settings.ai-quotas-scope-${scopeFilter.toLowerCase()}`)}
                                                </Label>
                                                {scopeFilter === 'Role' ? (
                                                    <Input
                                                        style={{ fontSize: '14px' }}
                                                        className="styled-inputs"
                                                        type="select"
                                                        id="usage-filter-scope-id"
                                                        value={scopeIdFilter}
                                                        onChange={(e) => handleScopeIdFilterChange(e.target.value)}
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
                                                        valueLabel={resolvedName(scopeFilter, scopeIdFilter)}
                                                        onChange={handleScopeIdFilterChange}
                                                    />
                                                )}
                                            </FormGroup>
                                        </Col>
                                    )}

                                    <Col xs={12} md={4}>
                                        <FormGroup>
                                            <Label className="label-notifications" for="usage-filter-user">
                                                {t('profile:module-settings.ai-quotas-usage-filter-user')}
                                            </Label>
                                            <QuotaTargetSearch
                                                scope="User"
                                                value={userFilter}
                                                valueLabel={resolvedName('User', userFilter)}
                                                onChange={handleUserFilterChange}
                                            />
                                        </FormGroup>
                                    </Col>
                                </Row>

                                {usage.length === 0 ? (
                                    <small>
                                        {(resourceFilter || scopeFilter || userFilter) ? t('profile:module-settings.ai-quotas-usage-none-filtered') : t('profile:module-settings.ai-quotas-usage-none')}
                                    </small>
                                ) : (
                                    <>
                                        <Table style={{ fontSize: '14px' }} responsive size="sm">
                                            <thead>
                                                <tr>
                                                    <th>{t('profile:module-settings.ai-quotas-scope-user')}</th>
                                                    <th>{t('profile:module-settings.ai-quotas-limit-resource')}</th>
                                                    <th>{t('profile:module-settings.ai-quotas-usage-amount')}</th>
                                                    <th>{t('profile:module-settings.ai-quotas-usage-governed-by')}</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                {usage.map((row, rowIndex) => (pageLoading ? renderSkeleton(row, rowIndex) : (
                                                    <tr key={rowKey(row)}>
                                                        <td>{displayName('User', read(row, 'UserId', ''))}</td>
                                                        <td>
                                                            {resourceLabel(read(row, 'ResourceTypeCode', ''), read(row, 'Name', null))}
                                                        </td>
                                                        <td>
                                                            {describeAmount(row)}{' '}
                                                            <small>{unitLabel(read(row, 'Unit', ''))}</small>
                                                        </td>
                                                        <td>{describeGoverningLimit(row)}</td>
                                                    </tr>
                                                )))}
                                            </tbody>
                                        </Table>

                                        {totalPages > 1 && (
                                            <div className="d-flex justify-content-center mt-2">
                                                <PaginationCustom
                                                    totalPages={totalPages}
                                                    currentPage={currentPage}
                                                    updateParent={setCurrentPage}
                                                    dapper={true}
                                                />
                                            </div>
                                        )}
                                    </>
                                )}
                            </AccordionBody>
                        </AccordionItem>
                    </Accordion>
                </Col>
            </Row>
            {loading ? <LoadingFullWindow /> : null}
        </>
    );
};

export default AccordionAiQuotaUsage;
