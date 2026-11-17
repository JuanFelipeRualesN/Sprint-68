import React from 'react';
import { Table, Button } from 'reactstrap';
import { useTranslation } from 'react-i18next';
import { readApiField as read } from '../../../../../../utils/utils';
import PaginationCustom from '../../../../../shared-module-components/datatables/pagination';
import '../../style.css';

// Table of quota limits, containing the list of overrides
const QuotaLimitTable = ({ limits, targetNames, roleLabel, resourceLabel, onEdit, onDelete, currentPage, totalPages, onPageChange }) => {
    const { t } = useTranslation(['profile', 'common']);
    const SKELETON_WIDTHS = [3.5, 7, 8, 5, 4.5, 4.5, 2.5, 3.5, 4];

    const unitLabel = (unit) => {
        if (!unit) return '';
        const key = `profile:module-settings.ai-quotas-unit-${String(unit).toLowerCase()}`;
        const label = t(key);
        return label !== key ? label : unit;
    };

    const describePeriod = (limit) => {
        const periodType = read(limit, 'PeriodType', '');
        if (periodType === 'Custom') {
            return t('profile:module-settings.ai-quotas-limit-custom-period', {
                days: read(limit, 'CustomPeriodDays', 0)
            });
        }
        return t(`profile:module-settings.ai-quotas-period-${String(periodType).toLowerCase()}`);
    };

    const isTargetLoading = (limit) => {
        const scope = read(limit, 'Scope', '');
        if (scope !== 'User' && scope !== 'School') return false;
        const entry = targetNames[`${scope}:${String(read(limit, 'ScopeId', ''))}`];
        return !entry || entry.status === 'loading';
    };

    const pageLoading = limits.some(isTargetLoading);

    // Describe the target of a limit for display in the table
    const describeTarget = (limit) => {
        const scope = read(limit, 'Scope', '');
        const scopeId = String(read(limit, 'ScopeId', ''));

        if (scope === 'Role') return roleLabel(scopeId);

        if (scope === 'User' || scope === 'School') {
            const entry = targetNames[`${scope}:${scopeId}`];
            if (entry && entry.status === 'done') return entry.name;
            return scopeId;
        }

        return scopeId;
    };

    const describeConsumed = (limit) => {
        const consumed = read(limit, 'ConsumedAmount', null);
        if (consumed === null || consumed === undefined) {
            return <span className="text-muted">—</span>;
        }

        const maxAmount = Number(read(limit, 'MaxAmount', 0));
        const percent = maxAmount > 0 ? Math.round((Number(consumed) / maxAmount) * 100) : null;

        return (
            <>
                {Number(consumed).toLocaleString()}
                {percent !== null && (
                    <small className="text-muted">{` (${percent}%)`}</small>
                )}
            </>
        );
    };

    const renderSkeleton = (limit, rowIndex) => (
        <tr key={read(limit, 'Id', 0)} className="quota-skeleton-row">
            {SKELETON_WIDTHS.map((width, index) => (
                <td key={index}>
                    <span
                        className="quota-skeleton-bar"
                        style={{
                            width: `${width}em`,
                            animationDelay: `${rowIndex * 0.12}s`
                        }}
                        aria-hidden="true"
                    />
                </td>
            ))}
        </tr>
    );

    if (limits.length === 0) {
        return <small>{t('profile:module-settings.ai-quotas-limit-none')}</small>;
    }

    return (
        <>
            <Table style={{ fontSize: '14px' }} responsive size="sm">
                <thead>
                    <tr>
                        <th>{t('profile:module-settings.ai-quotas-limit-scope')}</th>
                        <th>{t('profile:module-settings.ai-quotas-limit-target')}</th>
                        <th>{t('profile:module-settings.ai-quotas-limit-resource')}</th>
                        <th>{t('profile:module-settings.ai-quotas-limit-max')}</th>
                        <th>{t('profile:module-settings.ai-quotas-limit-consumed')}</th>
                        <th>{t('profile:module-settings.ai-quotas-period-type')}</th>
                        <th>{t('profile:module-settings.ai-quotas-rollover')}</th>
                        <th>{t('profile:module-settings.ai-quotas-limit-status')}</th>
                        <th />
                    </tr>
                </thead>
                <tbody>
                    {limits.map((limit, rowIndex) => (pageLoading ? renderSkeleton(limit, rowIndex) : (
                        <tr key={read(limit, 'Id', 0)}>
                            <td>
                                {t(`profile:module-settings.ai-quotas-scope-${String(read(limit, 'Scope', '')).toLowerCase()}`)}
                            </td>
                            <td>{describeTarget(limit)}</td>
                            <td>{resourceLabel(read(limit, 'ResourceTypeCode', ''), read(limit, 'Name', null))}</td>
                            <td>
                                {Number(read(limit, 'MaxAmount', 0)).toLocaleString()}{' '}
                                <small>{unitLabel(read(limit, 'Unit', ''))}</small>
                            </td>
                            <td>{describeConsumed(limit)}</td>
                            <td>{describePeriod(limit)}</td>
                            <td>
                                {read(limit, 'RolloverEnabled', false)
                                    ? t('common:general.yes')
                                    : t('common:general.no')}
                            </td>
                            <td>
                                {read(limit, 'Enabled', false)
                                    ? t('profile:module-settings.ai-quotas-limit-active')
                                    : t('profile:module-settings.ai-quotas-limit-inactive')}
                            </td>
                            <td className="text-right">
                                <Button
                                    color="link"
                                    size="sm"
                                    className="text-decoration-none"
                                    style={{ color: '#31665a', fontWeight: 600 }}
                                    onClick={() => onEdit(limit)}
                                >
                                    {t('profile:module-settings.ai-quotas-limit-edit')}
                                </Button>
                                <Button
                                    color="link"
                                    size="sm"
                                    className="text-decoration-none"
                                    style={{ color: '#c0392b', fontWeight: 600 }}
                                    onClick={() => onDelete(limit)}
                                >
                                    {t('profile:module-settings.ai-quotas-limit-delete')}
                                </Button>
                            </td>
                        </tr>
                    )))}
                </tbody>
            </Table>

            {totalPages > 1 && (
                <div className="d-flex justify-content-center mt-2">
                    <PaginationCustom
                        totalPages={totalPages}
                        currentPage={currentPage}
                        updateParent={onPageChange}
                        dapper={true}
                    />
                </div>
            )}
        </>
    );
};

export default QuotaLimitTable;
