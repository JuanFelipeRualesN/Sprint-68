import React, { useState, useEffect, useRef } from 'react';
import AsyncSelect from 'react-select/lib/Async';
import { useTranslation } from 'react-i18next';
import { getEntitiesSchool } from '../../../api/schools';
import { getUsersAdvancedAdmin } from '../../../api/users';
import { readApiField as read } from '../../../utils/utils';

const SCHOOL_ENTITY_TYPE = 1;

const DEBOUNCE_MS = 400;
const MIN_SEARCH_LENGTH = 2;

// Component for picking a quota limit's target by searching a school or user by name, used for the School and User scopes.
const QuotaTargetSearch = ({ scope, value, valueLabel, onChange }) => {
    const { t } = useTranslation(['profile', 'common']);
    const [selected, setSelected] = useState(null);
    const limiter = 10
    const debounceRef = useRef(null);

    useEffect(() => {
        if (!value) {
            setSelected(null);
            return;
        }
        
        if (selected && selected.value === value) {
            if (valueLabel && selected.label !== valueLabel) {
                setSelected({ value, label: valueLabel });
            }
            return;
        }

        setSelected({ value, label: valueLabel || String(value) });
    }, [value, valueLabel, scope]);

    const searchSchools = async (term) => {
        const response = await getEntitiesSchool(SCHOOL_ENTITY_TYPE, term, null, null, null, 0, limiter);

        const entities = response?.data?.SchoolEntities ?? [];
        return entities.map((entity) => ({
            value: read(entity, 'EntityId', ''),
            label: read(entity, 'EntityName', read(entity, 'EntityId', ''))
        }));
    };

    const searchUsers = async (term) => {
        const response = await getUsersAdvancedAdmin(null, term, null, null, null, null, null, null, null, 0, limiter, false, null);

        const users = response?.data?.UserInfo ?? [];
        return users.map((user) => {
            const name = `${read(user, 'GivenName', '')} ${read(user, 'Surname', '')}`.trim();
            const username = read(user, 'Username', '');
            const id = read(user, 'UserId', '');
            return {
                value: id,
                label: name ? `${name} (${username})` : (username || id)
            };
        });
    };

    // Load options for the Selector
    const loadOptions = (term) =>
        new Promise((resolve) => {
            if (debounceRef.current) clearTimeout(debounceRef.current);

            if (!term || term.trim().length < MIN_SEARCH_LENGTH) {
                resolve([]);
                return;
            }

            debounceRef.current = setTimeout(async () => {
                try {
                    const options = scope === 'School'
                        ? await searchSchools(term.trim())
                        : await searchUsers(term.trim());
                    resolve(options);
                } catch {
                    resolve([]);
                }
            }, DEBOUNCE_MS);
        });

    const handleChange = (option) => {
        setSelected(option);
        onChange(option ? option.value : '');
    };

    return (
        <AsyncSelect
            styles={{
                control: (base) => ({ ...base, fontSize: '14px' }),
                menu: (base) => ({ ...base, fontSize: '14px' })
            }}
            classNamePrefix="quota-target"
            cacheOptions
            isClearable
            value={selected}
            loadOptions={loadOptions}
            onChange={handleChange}
            placeholder={scope === 'School' ? t('profile:module-settings.ai-quotas-limit-school-search') : t('profile:module-settings.ai-quotas-limit-user-search')}
            noOptionsMessage={({ inputValue }) =>
                (inputValue && inputValue.trim().length >= MIN_SEARCH_LENGTH) ? t('profile:module-settings.ai-quotas-limit-no-results') : t('profile:module-settings.ai-quotas-limit-type-to-search')}
        />
    );
};

export default QuotaTargetSearch;
